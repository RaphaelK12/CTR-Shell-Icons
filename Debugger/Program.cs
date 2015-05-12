using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Debugger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type in the CXI path");
            string file_path = Console.ReadLine();
            using (FileStream stream = new FileStream(file_path, FileMode.Open))
            {
                Bitmap res = Helpers.CreateBitmap(stream);
                res.Save("output.bmp");
            }
        }
    }
}
