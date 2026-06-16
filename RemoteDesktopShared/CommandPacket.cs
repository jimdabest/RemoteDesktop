/*
Doc cho ban Pham Nguyen
Ham dong goi du lieu :
- CommandType la loai lenh (vd: MouseMove di chuyen chuot,KeyDown nhan phim,...)
- X , Y la toa do cua chuot, KeyCode la ma phim (vd: enter la 13, space la 32, a la 65, b la 66,...)
- ClientWidth, ClientHeight la kich thuoc vung anh tren Client de tinh ti le toa do chuan xac.
- using() de sau khi chay xong se giai phong bo nho
- MemoryStream() khoi tao mot vung nho de ghi du lieu
- BinaryWriter de ghi du lieu theo kieu byte vao MemoryStream
Ham giai ma du lieu
- MemoryStream(data) luc nay da co du lieu tu mang byte[] truyen vao 
- ReadInt32() de doc du lieu theo kieu int 4 byte va ep kieu ve CommandType, X, Y, KeyCode, ClientWidth, ClientHeight
 */
using System;
using System.IO; // (MemoryStream, BinaryWriter, BinaryReader)

namespace RemoteDesktopShared
{
    public class CommandPacket
    {
        public CommandType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int KeyCode { get; set; }
        public int ClientWidth { get; set; }
        public int ClientHeight { get; set; }

        // Ham dong goi du lieu thanh mang byte[]
        public byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((int)Type);    // Enum cast ve int 4 bytes
                writer.Write(X);            // 4 bytes
                writer.Write(Y);            // 4 bytes
                writer.Write(KeyCode);      // 4 bytes
                writer.Write(ClientWidth);  // 4 bytes
                writer.Write(ClientHeight); // 4 bytes

                return ms.ToArray();        // Dong goi lai thanh mang byte[] tong la 24 bytes
            }
        }

        // Ham giai ma du lieu tu mang byte[] ve CommandPacket
        public static CommandPacket FromBytes(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                return new CommandPacket
                {
                    Type = (CommandType)reader.ReadInt32(),
                    X = reader.ReadInt32(),
                    Y = reader.ReadInt32(),
                    KeyCode = reader.ReadInt32(),
                    ClientWidth = reader.ReadInt32(),
                    ClientHeight = reader.ReadInt32()
                };
            }
        }
    }
}