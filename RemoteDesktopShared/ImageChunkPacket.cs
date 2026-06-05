using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktopShared
{
    [Serializable]
    public class ImageChunkPacket
    {
        public long FrameId { get; set; }      // ID để nhận diện các mảnh của cùng 1 bức ảnh
        public int TotalChunks { get; set; }    // Tổng số mảnh cắt ra
        public int ChunkIndex { get; set; }    // Thứ tự của mảnh hiện tại
        public byte[] Payload { get; set; }    // Dữ liệu hình ảnh thật
    }
}
