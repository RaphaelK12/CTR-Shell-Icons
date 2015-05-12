using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using SharpShell.Attributes;
using SharpShell.SharpThumbnailHandler;

namespace CXIThumbnailsShellExtension
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.FileExtension, ".cxi")]
    [COMServerAssociation(AssociationType.FileExtension, ".cci")]
    public class CXIThumbnailHandler : SharpThumbnailHandler
    {
        public CXIThumbnailHandler()
        {

        }

        

        protected override Bitmap GetThumbnailImage(uint width)
        {
            try
            {
                return Helpers.CreateBitmap(SelectedItemStream);
            }
            catch (Exception exception)
            {
                //  Log the exception and return null for failure.
                LogError("An exception occurred opening the CXI file.", exception);
                return null;
            }
        }
    }
}
