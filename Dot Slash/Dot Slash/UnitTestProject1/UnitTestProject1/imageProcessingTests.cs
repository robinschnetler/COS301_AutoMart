using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dot_Slash;
using System.Drawing;
using System.IO;

namespace UnitTestProject1
{
	[TestClass]
	public class imageProcessingTests
	{
		[TestMethod]
		public void testResize()
		{
			String testPath = "testResize/";
			String[] pictures = Directory.GetFiles(testPath);
			int testWidth = 60;
			int testHeight = 30;
			ImageResizer iR = new ImageResizer(testPath, testWidth, testHeight);
			iR.execute();
			String directory = "Resized/";
			String[] resizedPictures = Directory.GetFiles(directory);
			Assert.AreEqual(resizedPictures.Length, pictures.Length, "Some images were not resized");
			Size testSize = new Size(testWidth, testHeight);
			for (int i = 0; i < resizedPictures.Length; i++)
			{
				Size finalImages = new Bitmap(resizedPictures[i]).Size;
				Assert.AreEqual(testSize, finalImages, "The image was resized to the wrong size");
			}
		}

		[TestMethod]
		public void testEdgeDetection()
		{
			String testPath = "testEdge/";
			String[] pictures = Directory.GetFiles(testPath);
			EdgeDetector eD = new EdgeDetector(testPath);
			eD.execute();
			String directory = "Edged/";
			String[] edgedPictures = Directory.GetFiles(directory);
			Assert.AreEqual(edgedPictures.Length, pictures.Length, "Some images were not preccessed in Edge Detection");
			int edgedCount = 0;
			for (int i = 0; i < edgedPictures.Length; i++)
			{
				bool isBlackOrWhite = true;
				Bitmap finalImages = new Bitmap(edgedPictures[i]);
				for (int j = 0; j < finalImages.Width; j++)
				{
					for (int k = 0; k < finalImages.Height; k++)
					{
						if(finalImages.GetPixel(j,k).R > 0 && finalImages.GetPixel(j,k).R < 255)
						{
							isBlackOrWhite = false;
							break;
						}
					}
					if(!isBlackOrWhite)
						break;
				}
				if (isBlackOrWhite)
					edgedCount++;
				Assert.AreEqual(true, isBlackOrWhite, "Image is not an edged image");
			}
			Assert.AreEqual(edgedPictures.Length, edgedCount, "Not all images were Edged");
		}

		[TestMethod]
		public void testGreyscale()
		{
			String testPath = "testGrey/";
			String[] pictures = Directory.GetFiles(testPath);
			Greyscaler gS = new Greyscaler(testPath);
			gS.execute();
			String directory = "Greyscale/";
			String[] greyPictures = Directory.GetFiles(directory);
			Assert.AreEqual(greyPictures.Length, pictures.Length, "Some images were not greyscaled");
			int greycount = 0;
			for (int i = 0; i < greyPictures.Length; i++)
			{
				bool isGrey = true;
				Bitmap finalImages = new Bitmap(greyPictures[i]);
				for (int j = 0; j < finalImages.Width; j++)
				{
					for (int k = 0; k < finalImages.Height; k++)
					{
						Color c = finalImages.GetPixel(j, k);
						if (c.R != c.G && c.R != c.B)
						{
							isGrey = false;
							break;
						}
					}
					if (!isGrey)
						break;
				}
				if (isGrey)
					greycount++;
				Assert.AreEqual(true, isGrey, "Image is not a greyscaled image");
			}
			Assert.AreEqual(greyPictures.Length, greycount, "Not all images were greyscaled");
		}
	}
}
