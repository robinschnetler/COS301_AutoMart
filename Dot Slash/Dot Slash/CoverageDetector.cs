using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.Util;

namespace Dot_Slash
{
    class CoverageDetector : Filter
    {
        public void pump(ref AdvertDetails _advertDetails)
        {
            float area = _advertDetails.Image.Width * _advertDetails.Image.Height;
            area /= 1536; // (480*320)*100 = %
            _advertDetails.CoverageValue = area;
        }
    }
}
