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
    public class CoverageDetector : Filter
    {
        public void pump(ref AdvertDetails _advertDetails)
        {
			if(!_advertDetails.CarFound)
				throw new Exception("Cannot calculate coverage if car not found");
			float totalArea = _advertDetails.Image.Width * _advertDetails.Image.Height;
			float area = _advertDetails.Rect.Width * _advertDetails.Rect.Height;
			float coverage = area/totalArea*100;
			_advertDetails.CoverageValue = coverage;
			_advertDetails.Rating = calculateCoverageRating(coverage);
        }

		private int calculateCoverageRating(float _coverage)
		{
			if (_coverage >= 70.0)
				return 2;
			else if (_coverage >= 34.0)
				return 1;
			else
				return 0;
		}
    }
}
