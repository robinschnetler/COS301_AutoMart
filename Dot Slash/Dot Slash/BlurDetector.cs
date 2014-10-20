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
		
		/// <summary>
		/// calculates the blur of the image by means of calculating the difference between the current pixel and
		/// the pixel directly to the right and bottom of the current pixel
		/// </summary>
		/// <param name="image">image to perform blur detection on</param>
		/// <returns>float value representing the average blur on the </returns>
		public float calculateBlur(Bitmap image)
		{
			Bitmap b = image;
			float sum = 0;
			int rad = 56;

			for (int j = 0; j < b.Width -1; j++)
			{

				for (int k = 0; k < b.Height-1; k++)
				{
					int currentPixel = b.GetPixel(j, k).R;
						if (Math.Abs(currentPixel - b.GetPixel(j + 1, k).R) > rad || Math.Abs(currentPixel - b.GetPixel(j, k + 1).R) > rad)
							sum++;
				}
			}
			return sum;
		}

		public void pump(ref AdvertDetails _advertDetails)
		{
			Image<Gray, Byte> image;
			if (!_advertDetails.CarFound)
				image = _advertDetails.Image.Convert<Gray, Byte>();
			else
			{
				image = _advertDetails.Image.GetSubRect(_advertDetails.Rect).Convert<Gray, Byte>();
				image.Resize(480, 320, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
			}
			Image<Gray, float> con = image.Laplace(1);
			float sum = calculateBlur(con.ToBitmap());
			_advertDetails.BlurValue = sum / ((image.Width-1) * (image.Height-1)) * 100;
			if (_advertDetails.BlurValue < blurTreshold)
			{
				_advertDetails.Blurry = true;
				_advertDetails.Error += "; image is blury";
			}
			else
			{
				if (_advertDetails.BlurValue < blurTreshold + (blurTreshold * 0.125))
					_advertDetails.blurRating = 1;
				else
					_advertDetails.blurRating = 2;
			}
		}
	}
}
