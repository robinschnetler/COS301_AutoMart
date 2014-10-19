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

		public ArrayList getEdgedPixels(Image<Gray, Byte> image)
		{
			image = image.SmoothGaussian(3);
			image = image.Canny(350, 20);
			Bitmap b = image.ToBitmap();
			ArrayList coordinates = new ArrayList();
			for (int i = 0; i < image.Width; i++)
			{
				for (int j = 0; j < image.Height; j++)
				{
					if (b.GetPixel(i, j).R != 0)
						coordinates.Add(new Coordinate(i, j));
				}
			}
			return coordinates;
		}

		private class Coordinate
		{
			int x, y;
			public Coordinate(int X, int Y)
			{
				x = X;
				y = Y;
			}
			public int getX()
			{
				return x;
			}
			public int getY()
			{
				return y;
			}
		}

		public float calculateBlur(Bitmap b, ArrayList coordinates)
		{
			float sum = 0;
			int rad = 56;
			int totalCoordinates = coordinates.Count;
			for (int i = 0; i < totalCoordinates; i++)
			{
				Coordinate c = (Coordinate)coordinates[i];
				int currentPixel = b.GetPixel(c.getX(), c.getY()).R;
				if (c.getX() < b.Width - 1 && c.getY() < b.Height - 1)
				{
					if (Math.Abs(currentPixel - b.GetPixel(c.getX() + 1, c.getY()).R) > rad
					|| Math.Abs(currentPixel - b.GetPixel(c.getX(), c.getY() + 1).R) > rad)
					{
						sum++;
					}
				}
			}
			float output = sum / totalCoordinates * 100;
			return output;
		}

		public void pump(ref AdvertDetails _advertDetails)
		{
			Image<Gray, Byte> image;
			if (!_advertDetails.CarFound)
				image = _advertDetails.Image.Convert<Gray, byte>();
			else
			{
				image = _advertDetails.Image.GetSubRect(_advertDetails.Rect).Convert<Gray, byte>();
				image.Resize(480, 320, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
			}
			ArrayList coordinates = getEdgedPixels(image);
			Image<Gray, float> con = image.Laplace(1);
			Bitmap b = con.ToBitmap();

			float blurValue = calculateBlur(b, coordinates);
			_advertDetails.BlurValue = blurValue;
			if (_advertDetails.BlurValue < blurTreshold)
			{
				_advertDetails.Blurry = true;
				_advertDetails.Error = true;
				throw new Exception("Image is Blurry");
			}
			else
			{
				if (blurValue < blurTreshold + 10)
					_advertDetails.Rating += 1;
				else
					_advertDetails.Rating += 2;
			}
		}
	}
}