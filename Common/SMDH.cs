using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CXIThumbnailsShellExtension
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMDH
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] Magic;
        short Version;
        short Reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x2000)]
        byte[] Titles;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
        byte[] Settings;
        ulong Reserved2;
        public IconGraphics Graphics;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IconGraphics
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x480)]
        public byte[] Small;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x1200)]
        public byte[] Large;
    }
}
