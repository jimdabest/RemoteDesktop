using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktopShared
{
    [Serializable] // Đánh dấu để có thể nén thành byte truyền qua mạng
    public class CommandPacket
    {
        public CommandType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int KeyCode { get; set; }
    }
}
