using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security.Cryptography;
using RemoteDesktopShared;

namespace RemoteDesktopServer
{
    public class ScreenCaster
    {
        private Socket _udpSocket;
        private IPEndPoint _clientEndPoint;
        private bool _isStreaming;
        private long _currentFrameId = 0;
        private const int MAX_PAYLOAD_SIZE = 50000;
        private string _lastFrameHash = string.Empty;

        // Các biến cấu hình từ Settings
        public long ImageQuality { get; set; } = 50L;
        public float ImageScale { get; set; } = 1.0f; // 1.0 (Low), 0.75 (Medium), 0.5 (High)

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
            using (MD5 md5 = MD5.Create())
            {
                while (_isStreaming)
                {
                    try
                    {
                        Rectangle bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

                        using (Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height))
                        {
                            using (Graphics g = Graphics.FromImage(screenshot))
                            {
                                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                            }

                            byte[] imageBytes;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                                myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, ImageQuality);

                                // Xử lý Compression Level (Scale ảnh)
                                if (ImageScale < 1.0f)
                                {
                                    int newW = (int)(bounds.Width * ImageScale);
                                    int newH = (int)(bounds.Height * ImageScale);
                                    using (Bitmap resizedBmp = new Bitmap(screenshot, new Size(newW, newH)))
                                    {
                                        resizedBmp.Save(ms, jpgEncoder, myEncoderParameters);
                                    }
                                }
                                else
                                {
                                    screenshot.Save(ms, jpgEncoder, myEncoderParameters);
                                }

                                imageBytes = ms.ToArray();
                            }

                            byte[] hashBytes = md5.ComputeHash(imageBytes);
                            string currentHash = BitConverter.ToString(hashBytes);

                            if (currentHash != _lastFrameHash)
                            {
                                SendImageChunks(imageBytes);
                                _lastFrameHash = currentHash;
                            }
                        }
                        Thread.Sleep(30);
                    }
                    catch { /* Bỏ qua lỗi đồ họa */ }
                }
            }
        }

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

                _udpSocket.SendTo(packet.ToBytes(), _clientEndPoint);
            }
        }
    }
}