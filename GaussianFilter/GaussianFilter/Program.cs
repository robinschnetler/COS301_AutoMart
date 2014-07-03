using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace GausianFilter
{
	class Program
	{
		static void Main(string[] args)
		{
			GausianFilter gf = new GausianFilter();
			Console.WriteLine("Press Enter to Start:");
			Console.ReadKey();
			gf.run("image.jpg");
			Console.WriteLine("Done");
		}
	}

	class GausianFilter
	{
		public void run(string filename)
		{
			Bitmap img = new Bitmap(filename);
			Bitmap gausImg = new Bitmap(img.Width, img.Height);
			smooth(img, gausImg);
			gausImg.Save("Gausian" +  ".jpg");
		}

		public void smooth(Bitmap img, Bitmap gaus)
		{
			//The filter matrix we use for smoothing

			/*		i-2	i-1	i	i+1	i+2
			*	j-2	2	4	5	4	2
			*	j-1	4	9	12	9	4
			*	j	5	12	15	12	5
			*	j+1	4	9	12	9	4
			*	j+2	2	4	5	4	2
			*/
			int[,] filter = new int[5, 5] { { 2, 4, 5, 4, 2 }, { 4, 9, 12, 9, 4 }, { 5, 12, 15, 12, 5 }, { 4, 9, 12, 9, 4 }, { 2, 4, 5, 4, 2 } };

			//Loop through every pixel
			for (int i = 2; i < img.Width - 2; i++)
			{
				for (int j = 2; j < img.Height - 2; j++)
				{
					int smoothValue = 0;
					int divisor = 0;

					//i = i coordinate of the pixel
					//j = j coordinate of the pixel

					//Loop around the current pixel
					for (int x = -2; x <= 2; x++)
					{
						for (int y = -2; y <= 2; y++)
						{
							try
							{
								//newI & newJ = coordinate of the pixel around the current pixel we are smoothing
								int newI = i + x;
								int newJ = j + y;

								int c = (img.GetPixel(newI, newJ).ToArgb()) * filter[x + 2, y + 2];	//to accomodate for x & y starting at -2 and filter indices only starting at [0,0]

								smoothValue += c;
								divisor += filter[x + 2, y + 2];
							}
							catch
							{
								Console.WriteLine("Exception Caught");
								//Pixel out of bounds - do nothing with this pixel
							}
						}
					}

					int smoothedColor = smoothValue/divisor;

					Color newPixel = Color.FromArgb(smoothedColor);
					gaus.SetPixel(i, j, newPixel);
				}
			}
		}
	}
}
