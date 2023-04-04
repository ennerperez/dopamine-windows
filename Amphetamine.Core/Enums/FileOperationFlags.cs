using System;

namespace Amphetamine.Core.Enums
{
    [Flags]
    internal enum FileOperationFlags : ushort
    {
        FOF_SILENT = 4,
        FOF_NOCONFIRMATION = 16, // 0x0010
        FOF_ALLOWUNDO = 64, // 0x0040
        FOF_SIMPLEPROGRESS = 256, // 0x0100
        FOF_NOERRORUI = 1024, // 0x0400
        FOF_WANTNUKEWARNING = 16384, // 0x4000
    }
}