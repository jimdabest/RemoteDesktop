using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace RemoteDesktop
{
    public class ScreenReceiver
    {
        private Socket _udpSocket;
        private bool _isListening;
        private Dictionary<long, ImageChunkPacket[]> _frameBuffer = new Dictionary<long, ImageChunkPacket[]>();

        // Đây cũng là một Delegate (Action) dùng để báo cáo khi có ảnh mới
        public event Action<Image> OnImageReceived;

        // Giỏ chứa dữ liệu UDP và EndPoint của người gửi
        private byte[] _buffer = new byte[65536];
        private EndPoint _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        public int StartListening(int port)
        {
            _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _udpSocket.ReceiveBufferSize = 1024 * 1024;
            _udpSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            _isListening = true;

            // XÓA BỎ THREAD CŨ Ở ĐÂY!
            // Thay bằng hàm Non-blocking của UDP: BeginReceiveFrom kết hợp Delegate (AsyncCallback)
            _udpSocket.BeginReceiveFrom(_buffer, 0, _buffer.Length, SocketFlags.None, ref _remoteEndPoint, new AsyncCallback(ReceiveCallback), null);

            return ((IPEndPoint)_udpSocket.LocalEndPoint).Port;
        }

        public void StopListening()
        {
            _isListening = false;
            _udpSocket?.Close();
        }

        // Hàm Delegate Callback sẽ được Windows tự động gọi khi có gói tin UDP bay tới
        private void ReceiveCallback(IAsyncResult ar)
        {
            if (!_isListening) return;

            try
            {
                // 1. Chốt lại xem nhận được bao nhiêu bytes
                int receivedBytes = _udpSocket.EndReceiveFrom(ar, ref _remoteEndPoint);

                if (receivedBytes > 0)
                {
                    // 2. Giải mã và ráp ảnh
                    ImageChunkPacket chunk = ImageChunkPacket.FromBytes(_buffer, receivedBytes);
                    ProcessChunk(chunk);
                }
            }
            catch { /* Bỏ qua nếu có gói tin bị rớt do lỗi mạng */ }
            finally
            {
                // 3. Tiếp tục giăng lưới Non-blocking để nghe gói tin tiếp theo
                if (_isListening)
                {
                    _udpSocket.BeginReceiveFrom(_buffer, 0, _buffer.Length, SocketFlags.None, ref _remoteEndPoint, new AsyncCallback(ReceiveCallback), null);
                }
            }
        }

        private void ProcessChunk(ImageChunkPacket chunk)
        {
            if (!_frameBuffer.ContainsKey(chunk.FrameId))
            {
                _frameBuffer[chunk.FrameId] = new ImageChunkPacket[chunk.TotalChunks];
            }

            _frameBuffer[chunk.FrameId][chunk.ChunkIndex] = chunk;

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