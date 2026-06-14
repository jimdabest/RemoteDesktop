using System;
using System.IO;

namespace RemoteDesktopShared
{
    public class ImageChunkPacket
    {
        public long FrameId { get; set; }
        public int TotalChunks { get; set; }
        public int ChunkIndex { get; set; }
        public byte[] Payload { get; set; }

        // Hàm TỰ ĐÓNG GÓI thành mảng byte (Dùng cho Server)
        public byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(FrameId);          // 8 bytes
                writer.Write(TotalChunks);      // 4 bytes
                writer.Write(ChunkIndex);       // 4 bytes
                writer.Write(Payload.Length);   // 4 bytes (Độ dài của hình)
                writer.Write(Payload);          // Dữ liệu hình

                return ms.ToArray();
            }
        }

        // Hàm TỰ GIẢI MÃ từ mảng byte (Dùng cho Client)
        public static ImageChunkPacket FromBytes(byte[] data, int length)
        {
            using (MemoryStream ms = new MemoryStream(data, 0, length))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                ImageChunkPacket packet = new ImageChunkPacket();
                packet.FrameId = reader.ReadInt64();
                packet.TotalChunks = reader.ReadInt32();
                packet.ChunkIndex = reader.ReadInt32();

                int payloadLength = reader.ReadInt32();
                packet.Payload = reader.ReadBytes(payloadLength);

                return packet;
            }
        }
    }
}