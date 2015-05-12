using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CXIThumbnailsShellExtension
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct String8
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] text;
    }

    public class CXIPlaingRegion
    {
        public string[] PlainRegionStrings;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] NCCHHeaderSignature;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] Magic;
        public uint CXILength;
        public ulong TitleID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public char[] MakerCode;
        public ushort Version;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved0;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] ProgramID;
        public ulong TempFlag;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Unknown0_0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Unknown0_1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] ProductCode;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] ExtendedHeaderHash;
        public uint ExtendedHeaderSize;

        public uint Unknown1;
        public uint Unknown2;
        public uint Flags;
        public uint PlainRegionOffset;
        public uint PlainRegionSize;

        public uint Unknown3;
        public uint Unknown4;

        public uint ExeFSOffset;
        public uint ExeFSLength;
        public uint ExeFSHashRegionSize;

        public uint Unknown5;

        public uint RomFSOffset;
        public uint RomFSLength;
        public uint RomFSHashRegionSize;

        public uint Unknown6;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] ExeFSSuperBlockhash;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] RomFSSuperBlockhash;

        public static CXIPlaingRegion getPlainRegionStringsFrom(byte[] buffer)
        {
            CXIPlaingRegion temp = new CXIPlaingRegion();
            string bigstring = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
            string[] splited = bigstring.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            temp.PlainRegionStrings = splited;
            return temp;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeader
    {
        public CXIExtendedHeaderCodeSetInfo CodeSetInfo;
        public CXIExtendedHeaderDependencyList DependencyList;
        public CXIExtendedHeaderSystemInfo SystemInfo;
        public CXIExtendedHeaderArm11SystemLocalCaps Arm11SystemLocalCaps;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderCodeSetInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] Name;
        public CXIExtendedHeaderSystemInfoFlags Flags;
        public CXIExtendedHeaderCodeSegmentInfo Text;
        public uint StackSize;
        public CXIExtendedHeaderCodeSegmentInfo Ro;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;
        public CXIExtendedHeaderCodeSegmentInfo Data;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] BssSize;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderSystemInfoFlags
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] Reserved;
        public byte Flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] RemasterVersion;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderCodeSegmentInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Address;
        public uint NumMaxPages;
        public uint CodeSize;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderDependencyList
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
        public String8[] ProgramID;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderSystemInfo
    {
        public uint SaveDataSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] JumpID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
        public byte[] Reserved2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderStorageInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] ExtSaveDataID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] SystemSaveDataID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] AccessInfo;
        public byte OtherAttributes;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ResourceLimit
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderArm11SystemLocalCaps
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] ProgramID;
        public ulong Flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public ResourceLimit[] ResourceLimitDescriptor;
        public CXIExtendedHeaderStorageInfo StorageInfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public String8[] ServiceAccessControl;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x1f)]
        public byte[] Reserved;
        public byte ResourceLimitCategory;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ExeFSSectionHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] Name;
        public uint Offset;
        public int Size;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ExeFsHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public ExeFSSectionHeader[] Sections;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
        byte[] Reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8 * 0x20)]
        byte[] Hashes;
    }
}
