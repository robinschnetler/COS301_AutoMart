using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dot_Slash;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace DotSlash_UnitTest
{
	[TestClass]
	public class BlurDetectorTest
	{
		[TestMethod]
		public void calculateBlurTest()
		{
			double threshold = 1.00;
			BlurDetector bd = new BlurDetector(threshold);
			Bitmap blurryImage = new Bitmap("blurTest/blurry.jpg");
			Bitmap sharpImage = new Bitmap("blurTest/sharp.jpg");
			Bitmap check = createCheckBoard();
			double blurryTest = bd.calculateBlur(blurryImage) / ((blurryImage.Width-1) * (blurryImage.Height-1)) * 100;
			double sharpTest = bd.calculateBlur(sharpImage) / ((sharpImage.Width-1) * (sharpImage.Height-1)) * 100;
			double checkTest = bd.calculateBlur(check)/((check.Width-1) * (check.Height-1))*100;
			Boolean blurry = (blurryTest < threshold) ? true : false;
			Boolean sharp = (sharpTest >= threshold) ? true : false;
			Assert.AreEqual(true, blurry, "Image incorrectly classified as non-blurry");
			Assert.AreEqual(true, sharp, "Image incorrectly classified as blurry");
			Assert.AreEqual(100.00, checkTest, "should be 100 %");
		}

		public Bitmap createCheckBoard()
		{
			Bitmap b = new Bitmap(8,8);
			Color black = Color.FromArgb(255,0,0,0);
			Color white = Color.FromArgb(255,255,255,255);

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if(i%2 == 0)
					{
						if (j % 2 == 0)
							b.SetPixel(i, j, black);
						else
							b.SetPixel(i, j, white);
					}
					else
					{
						if (j % 2 == 0)
							b.SetPixel(i, j, white);
						else
							b.SetPixel(i, j, black);
					}
				}
			}
			Image<Gray, Byte> im = new Image<Gray, Byte>(b);
			b = im.Laplace(1).ToBitmap();
			b.Save("check.jpg");
			return b;
		}
	}
}
