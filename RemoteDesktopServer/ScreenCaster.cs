using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices; // Bắt buộc phải có để dùng DllImport
using System.Threading;
using RemoteDesktopShared;

namespace RemoteDesktopServer
{
    public class ScreenCaster
    {
        private Socket _udpSocket;
        private IPEndPoint _clientEndPoint;
        private bool _isStreaming;
        private long _currentFrameId = 0;
        private const int MAX_PAYLOAD_SIZE = 50000; // Giới hạn 50KB

        // Import API của Windows để lấy kích thước màn hình vật lý thực tế (Bỏ qua DPI ảo)
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        public ScreenCaster(string clientIp, int clientPort)
        {
            _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _clientEndPoint = new IPEndPoint(IPAddress.Parse(clientIp), clientPort);
        }

        public void StartStreaming()
        {
            _isStreaming = true;
            Thread streamThread = new Thread(StreamLoop) { IsBackground = true };
            streamThread.Start();
        }

        public void StopStreaming()
        {
            _isStreaming = false;
            _udpSocket?.Close();
        }

        private void StreamLoop()
        {
            while (_isStreaming)
            {
                try
                {
                    // 1. Lấy kích thước màn hình CHUẨN (Nhờ Program.cs đã bật DPI Aware)
                    Rectangle bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

                    using (Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using (Graphics g = Graphics.FromImage(screenshot))
                        {
                            // Chụp nền màn hình (Giờ sẽ lấy full 100% bao gồm cả Taskbar)
                            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                        }

                        // 2. Ép xung nén ảnh JPEG (Giảm chất lượng xuống 50% để truyền cho mượt)
                        byte[] imageBytes;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 50L);
                            screenshot.Save(ms, jpgEncoder, myEncoderParameters);

                            imageBytes = ms.ToArray();
                        }

                        // 3. Cắt nhỏ ảnh và bắn đi
                        SendImageChunks(imageBytes);
                    }

                    Thread.Sleep(30); // Tạm nghỉ để giữ tốc độ khoảng 30 FPS, tránh lag máy
                }
                catch { /* Bỏ qua lỗi nếu máy tính đang bận xử lý đồ họa khác */ }
            }
        }

        // Hàm hỗ trợ tìm bộ giải mã JPEG của Windows
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid) return codec;
            }
            return null;
        }

        private void SendImageChunks(byte[] imageBytes)
        {
            _currentFrameId++;
            int totalChunks = (int)Math.Ceiling((double)imageBytes.Length / MAX_PAYLOAD_SIZE);

            for (int i = 0; i < totalChunks; i++)
            {
                int offset = i * MAX_PAYLOAD_SIZE;
                int size = Math.Min(MAX_PAYLOAD_SIZE, imageBytes.Length - offset);
                byte[] chunkData = new byte[size];
                Array.Copy(imageBytes, offset, chunkData, 0, size);

                ImageChunkPacket packet = new ImageChunkPacket
                {
                    FrameId = _currentFrameId,
                    TotalChunks = totalChunks,
                    ChunkIndex = i,
                    Payload = chunkData
                };

                byte[] sendBytes = packet.ToBytes();
                _udpSocket.SendTo(sendBytes, _clientEndPoint);
            }
        }
    }
}