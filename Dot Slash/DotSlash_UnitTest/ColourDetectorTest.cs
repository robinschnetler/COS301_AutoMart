using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dot_Slash;
using System.Drawing;

namespace DotSlash_UnitTest
{
	[TestClass]
	public class ColourDetectorTest
	{
		[TestMethod]
		public void getBlockColourIndex_Test()
		{
			int expected = 13;
			int height = 50, width = 50;
			ColourDetector clrDetector = new ColourDetector();
			ImageBlock block = new ImageBlock(0, 0, height, width, true, 0.0);

			Image image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			using (Graphics grp = Graphics.FromImage(image))
			{
				grp.FillRectangle(Brushes.Violet, 0, 0, width, height);
			}
			Bitmap colouredImage = new Bitmap(image);

			int actual = clrDetector.getBlockColourIndex(colouredImage, block);

			Assert.AreEqual(expected, actual, "Wrong colour index returned.");
		}

		[TestMethod]
		public void inRange_Test()
		{
			bool expected = true;
			double pixelValue = 80, binValue = 73;
			int range = 10;
			ColourDetector cd = new ColourDetector();

			bool actual = cd.inRange(pixelValue, binValue, range);

			Assert.AreEqual(expected, actual, "The value should be in range.");
		}

		[TestMethod]
		public void convertRGBtoHSV_Test()
		{
			Color colour = Color.FromArgb(101,34,28);
			double expectedHue = 5, expectedSat = 72.3, expectedVal = 39.6;
			double hue, sat, val;
			ColourDetector cd = new ColourDetector();
			cd.convertRGBtoHSV(colour, out hue, out sat, out val);

			Assert.AreEqual(expectedHue, Math.Round(hue), "Hue is incorrect.");
			Assert.AreEqual(expectedSat, Math.Round(sat,1), "Saturation is incorrect.");
			Assert.AreEqual(expectedVal, Math.Round(val,1), "Value is incorrect.");
		}

		[TestMethod]
		public void convertHSVtoRGB_Test()
		{

		}

		[TestMethod]
		public void getDisance_Test()
		{
			int expected = 87;

			ColourDetector cd = new ColourDetector();
			Color colour = Color.FromArgb(87, 81, 67);
			//int actual = cd.getDistance(colour, new ColourBucket("Turquoise", 64, 224, 208, "40E0D0"));
			int actual = cd.getDistance(colour, new ColourBucket("Silver", 192, 192, 192, "C0C0C0"));

			Assert.AreEqual(expected, actual, "Incorrect distance.");
		}

	}
}