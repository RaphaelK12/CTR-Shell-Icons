using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Common;
using SharpShell.Attributes;
using SharpShell.SharpIconHandler;

namespace CXIThumbnailsShellExtension
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.FileExtension, ".cxi")]
    [COMServerAssociation(AssociationType.FileExtension, ".cci")]
    class CXIIconHandler : SharpIconHandler
    {
        protected override Icon GetIcon(bool smallIcon, uint iconSize)
        {
            using (FileStream fs = new FileStream(SelectedItemPath, FileMode.Open))
                return Icon.FromHandle(Helpers.CreateBitmap(fs).GetHicon());
        }
    }
}
