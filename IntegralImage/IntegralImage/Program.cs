using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace IntegralImage
{
	class Program
	{
		static void Main(string[] args)
		{
			GreyScalar greyscale = new GreyScalar();
			Console.WriteLine("Press Enter to Start:");
			Console.ReadKey();
			Console.WriteLine("Starting the Program");
			Bitmap greyImg = greyscale.createGreyScaleImage("image.jpg");
			greyImg.Save("newGreyscaleImg.jpg");
			Console.WriteLine("Done");
		}
	}

	class SummedAreaTable
	{
		int[,] createSummedAreaTable(String filename)
		{
			//We are getting in an image and returning the Summed Area Table (A 2x2 matrix of integers)
			//See http://computersciencesource.wordpress.com/2010/09/03/computer-vision-the-integral-image/
			Bitmap img = new Bitmap(filename);
			int width = img.Width;
			int height = img.Height;

			int[,] SummedAreaTable = new int[width, height];
			
			//Initialize first value in the Summed Area Table as the top, right most value of the image

			SummedAreaTable[0, 0] = img.GetPixel(0, 0).R;

			//Loop through every pixel in the image, row by row
			for (int x = 0; x < height; x++)
			{
				for (int y = 0; y < width; y++)
				{
					//s(x,y) = i(x,y) + s(x-1, y) + s(x, y-1) - s(x-1, y-1)
					int value = 0;	//This value is s(x,y), the value in the Summed Area Table at co-ordinates x & y

					value += img.GetPixel(x, y).R;	//Since the image is greyscale, we can add any of the RGB components as they are all equal

					if (x - 1 > 0)	//s(x-1, y) is in range in the Summed Area Table (otherwise the value to add is 0)
					{
						value += SummedAreaTable[x - 1, y];
					}

					if (y - 1 > 0) //Similar to the above step
					{
						value += SummedAreaTable[x, y - 1];
					}

					if (x - 1 > 0 && y - 1 > 0) //Again, just checking ranges
					{
						value -= SummedAreaTable[x - 1, y - 1];
					}
				}				
			}
			
			return SummedAreaTable;
		}
	}

	class GreyScalar
	{
		public Bitmap createGreyScaleImage(string filename)
		{
			Bitmap originalImage = new Bitmap(filename);
			Bitmap img = new Bitmap(originalImage.Width, originalImage.Height);
			//Bitmap img = new Bitmap(originalImage.Width, originalImage.Height, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale);
			//Bitmap img = new Bitmap(originalImage.Width, originalImage.Height, originalImage.PixelFormat);
			//Loop through every pixel in the image, row by row
			for (int y = 0; y < originalImage.Height; y++)
			{
				for (int x = 0; x < originalImage.Width; x++)
				{
					//See http://www.johndcook.com/blog/2009/08/24/algorithms-convert-color-grayscale/
					//Using the Luminosity method, the formula for luminosity is 0.21 R + 0.72 G + 0.07 B

					Color pixel = originalImage.GetPixel(x,y);
					double greyValue = (pixel.R * 0.21) + (pixel.G * 0.72) + (pixel.B * 0.07);
					Color newPixel = Color.FromArgb(255, (Int16) greyValue, (Int16) greyValue, (Int16) greyValue);
					//greyScaleImg.SetPixel(x, y, newPixel);
					img.SetPixel(x, y, newPixel);
				}
			}

			//Bitmap clone = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale);
			//using (Graphics gr = Graphics.FromImage(clone))
			//{
			//	gr.DrawImage(img, new Rectangle(0, 0, clone.Width, clone.Height));
			//}
			return img;
		}
	}
}
