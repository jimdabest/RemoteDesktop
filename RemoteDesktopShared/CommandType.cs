using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktopShared
{
    public enum CommandType
    {
        MouseMove,
        LeftMouseDown,
        LeftMouseUp,
        RightMouseDown,
        RightMouseUp,
        KeyDown,
        KeyUp,
        RegisterUdpPort,
        ChangeQuality,       
        ChangeCompression,
        MouseWheel,
        Login,
        LoginResult
    }
}