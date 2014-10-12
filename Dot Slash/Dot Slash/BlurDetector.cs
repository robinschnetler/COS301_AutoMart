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
    public class BlurDetector : Filter
    {
        double blurTreshold;

	public BlurDetector(double t)
	{
		blurTreshold = t;
	}

        public void pump(ref AdvertDetails _advertDetails)
        {
		Image<Gray, Byte> image;
		if(!_advertDetails.CarFound)
			image = _advertDetails.Image.Convert<Gray, byte>();
		else
		{
			image = _advertDetails.Image.GetSubRect(_advertDetails.Rect).Convert<Gray, byte>();
			image.Resize(480, 320, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
		}
		Image<Gray, float> con = image.Laplace(1);
		Bitmap b = image.ToBitmap();
		float sum = 0;
		int rad = 56;
		for (int j = 0; j < con.Width - 1; j++)
		{

		for (int k = 0; k < con.Height - 1; k++)
		{
			if (Math.Abs(b.GetPixel(j, k).R - b.GetPixel(j + 1, k).R) > rad || Math.Abs(b.GetPixel(j, k).R - b.GetPixel(j, k + 1).R) > rad)
			{
				sum++;
			}
		}
		}
		_advertDetails.BlurValue = sum / (b.Width * b.Height) * 100;
		if(_advertDetails.BlurValue < blurTreshold)
		{
			_advertDetails.Blurry = true;
			_advertDetails.Error = true;
			throw new Exception("Image is Blurry");
		}
        }
    }
}
