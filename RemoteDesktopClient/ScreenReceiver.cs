using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using RemoteDesktopShared;

namespace RemoteDesktopClient
{
    public class ScreenReceiver
    {
        private Socket _udpSocket;
        private bool _isListening;
        private Dictionary<long, ImageChunkPacket[]> _frameBuffer = new Dictionary<long, ImageChunkPacket[]>();

        // Sự kiện báo cho Form biết khi ráp xong một bức ảnh
        public event Action<Image> OnImageReceived;

        public void StartListening(int port)
        {
            // Khởi tạo Socket thuần cho UDP
            _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Tăng bộ đệm nhận của OS lên 1 Megabyte để chống nghẽn mạng cục bộ
            _udpSocket.ReceiveBufferSize = 1024 * 1024;

            _udpSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            _isListening = true;

            Thread listenThread = new Thread(ReceiveLoop) { IsBackground = true };
            listenThread.Start();
        }

        public void StopListening()
        {
            _isListening = false;
            _udpSocket?.Close();
        }

        private void ReceiveLoop()
        {
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = new byte[65536]; // Buffer đủ to để chứa 1 mảnh cắt (chunk)

            while (_isListening)
            {
                try
                {
                    // Lắng nghe gói tin UDP
                    int receivedBytes = _udpSocket.ReceiveFrom(buffer, ref remoteEndPoint);

                    ImageChunkPacket chunk = ImageChunkPacket.FromBytes(buffer, receivedBytes);
                    ProcessChunk(chunk);
                }
                catch { /* Bỏ qua nếu có gói tin bị rớt */ }
            }
        }

        private void ProcessChunk(ImageChunkPacket chunk)
        {
            // Khởi tạo mảng hứng ảnh nếu đây là frame mới
            if (!_frameBuffer.ContainsKey(chunk.FrameId))
            {
                _frameBuffer[chunk.FrameId] = new ImageChunkPacket[chunk.TotalChunks];
            }

            // Gắn mảnh ghép vào đúng vị trí
            _frameBuffer[chunk.FrameId][chunk.ChunkIndex] = chunk;

            // Kiểm tra xem đã nhận đủ tất cả các mảnh của frame này chưa
            bool isComplete = true;
            foreach (var p in _frameBuffer[chunk.FrameId])
            {
                if (p == null)
                {
                    isComplete = false;
                    break;
                }
            }

            if (isComplete) ReassembleAndDisplay(chunk.FrameId);
        }

        private void ReassembleAndDisplay(long frameId)
        {
            var chunks = _frameBuffer[frameId];
            int totalSize = 0;
            foreach (var chunk in chunks) totalSize += chunk.Payload.Length;

            byte[] fullImageBytes = new byte[totalSize];
            int currentOffset = 0;
            foreach (var chunk in chunks)
            {
                Array.Copy(chunk.Payload, 0, fullImageBytes, currentOffset, chunk.Payload.Length);
                currentOffset += chunk.Payload.Length;
            }

            try
            {
                using (MemoryStream ms = new MemoryStream(fullImageBytes))
                {
                    Image frame = Image.FromStream(ms);
                    OnImageReceived?.Invoke((Image)frame.Clone());
                }
            }
            catch { /* File bị lỗi do bit mạng lật, bỏ qua bức ảnh này */ }

            // Dọn dẹp RAM: Xóa ảnh đã ráp xong và các mảnh rác quá cũ
            _frameBuffer.Remove(frameId);
            var oldKeys = new List<long>();
            foreach (var key in _frameBuffer.Keys)
            {
                if (key < frameId - 2) oldKeys.Add(key);
            }
            foreach (var key in oldKeys) _frameBuffer.Remove(key);
        }
    }
}