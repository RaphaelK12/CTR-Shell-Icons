using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using CXIThumbnailsShellExtension;

namespace Common
{
    class Helpers
    {
        public static uint BlockSize = 0x200;

        private static CXIHeader ReadCXIHeader(Stream stream)
        {
            CXIHeader header;
            int count = Marshal.SizeOf(typeof(CXIHeader));
            byte[] readBuffer = new byte[count];
            BinaryReader reader = new BinaryReader(stream);
            readBuffer = reader.ReadBytes(count);
            GCHandle handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
            header = (CXIHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(CXIHeader));
            handle.Free();
            return header;
        }

        private static ExeFsHeader ReadExeFSHeader(Stream stream, long offset)
        {
            ExeFsHeader header;
            int count = Marshal.SizeOf(typeof(ExeFsHeader));
            byte[] readBuffer = new byte[count];
            BinaryReader reader = new BinaryReader(stream);
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            readBuffer = reader.ReadBytes(count);
            GCHandle handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
            header = (ExeFsHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(ExeFsHeader));
            handle.Free();
            return header;
        }

        private static T ReadExeFSSection<T>(Stream stream, ExeFSSectionHeader section, long exefs_offset)
        {
            T data;
            long offset = exefs_offset + section.Offset + Marshal.SizeOf(typeof(ExeFsHeader));
            byte[] readBuffer = new byte[section.Size];
            BinaryReader reader = new BinaryReader(stream);
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            readBuffer = reader.ReadBytes(section.Size);
            GCHandle handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
            data = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return data;
        }

        public static ExeFSSectionHeader? FindExeFSSection(ExeFsHeader header, string name)
        {
            return header.Sections.FirstOrDefault(p => new string(p.Name).Contains(name));
        }

        static int MortonInterleave(int x, int y)
        {
            int i = (x & 7) | ((y & 7) << 8); // ---- -210
            i = (i ^ (i << 2)) & 0x1313;      // ---2 --10
            i = (i ^ (i << 1)) & 0x1515;      // ---2 -1-0
            i = (i | (i >> 7)) & 0x3F;
            return i;
        }

        public static int CalculateMortonIndex(int x, int y, int width)
        {
            const int block_height = 8;

            int coarse_x = x & ~7;
            int coarse_y = y & ~7;

            int i = MortonInterleave(x, y);

            int offset = coarse_x * block_height;

            return (i + offset) * 2 + coarse_y * width * 2;
        }

        private static int Convert5To8(int value)
        {
            return (value << 3) | (value >> 2);
        }

        private static int Convert6To8(int value)
        {
            return (value << 2) | (value >> 4);
        }

        public static Bitmap DecodeMortonImage(byte[] data, int size)
        {
            Bitmap image = new Bitmap(size, size);
            for (int x = 0; x < size; ++x)
            {
                for (int y = 0; y < size; ++y)
                {
                    int offset = CalculateMortonIndex(x, y, size);

                    short pixel = (short)((data[offset]) | (data[offset + 1] << 8));
                    int b = Convert5To8(pixel & 0x1F);
                    int g = Convert6To8((pixel >> 5) & 0x3F);
                    int r = Convert5To8((pixel >> 11) & 0x1F);
                    image.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return image;
        }

        public static Bitmap CreateBitmap(Stream stream)
        {
            // Read the header
            CXIHeader header = Helpers.ReadCXIHeader(stream);
            header.ExeFSOffset *= BlockSize;
            header.ExeFSLength *= BlockSize;

            // Now read the ExeFS header
            ExeFsHeader exefs_header = ReadExeFSHeader(stream, header.ExeFSOffset);

            ExeFSSectionHeader? icon_section = FindExeFSSection(exefs_header, "icon");

            if (icon_section != null && icon_section.Value.Size != 0)
            {
                SMDH icon = ReadExeFSSection<SMDH>(stream, icon_section.Value, header.ExeFSOffset);
                return DecodeMortonImage(icon.Graphics.Large, 48);
            }

            return new Bitmap(100, 100);
        }
    }
}
