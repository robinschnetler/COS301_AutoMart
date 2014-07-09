using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Dot_Slash
{
	//main entry point into the program
	class Program
	{
		static void Main(string[] args)
		{
			ImageProcessor profiler = new ImageProcessor("images/");
			//Console.WriteLine("Applying Gauusian Filter");
			//profiler.applyGaussian();
			//Console.WriteLine("Done Gaussian Filter");
			Console.WriteLine("Applying Edge Detector");
			profiler.detectEdges();
			Console.WriteLine("Done Edge Detection");
		}
	}

	class Tools
	{
		public int[,] generateIntegralImage()
		{

		}
	}

	class ImageProcessor
	{
		String imagePath; 
		Strategy strategy;

		public ImageProcessor(String imgPath)
		{
			imagePath = imgPath;
		}

		public void makeGreyscale()
		{
			strategy = new Greyscaler(imagePath);
			strategy.execute();
			strategy = null;
		}

		public void detectEdges()
		{
			strategy = new EdgeDetector(imagePath);
			strategy.execute();
			strategy = null;
		}

		public void applyGaussian()
		{
			strategy = new GaussianFilter(imagePath);
			strategy.execute();
			strategy = null;
		}
	}

	//interface for multiple image proccessing tools: each sub-program will implement its own overriden execute() function
	interface Strategy
	{
		void execute();
	}

	class EdgeDetector : Strategy
	{
		private int threshold = 35;
		private Boolean diagonal = true;
		private string imagePath;
		public EdgeDetector(String _imagePath)
		{
			imagePath = _imagePath;
		}

		public virtual void execute()
		{
			String[] pictures = Directory.GetFiles(imagePath, "*.jpg", SearchOption.TopDirectoryOnly);
			bool isExists = System.IO.Directory.Exists("edged/");
			if (!isExists)
				System.IO.Directory.CreateDirectory("edged/");
			for (int i = 0; i < pictures.Length; i++)
			{
				Bitmap img = new Bitmap(pictures[i]);
				Bitmap edgedImg = new Bitmap(img.Width, img.Height);
				makeEdge(img, edgedImg);
				edgedImg.Save("edged/Edged_" + new FileInfo(pictures[i]).Name);	
			}
		}

		public void makeEdge(Bitmap img, Bitmap edge)
		{
			/**
			 * white colour for edge and black for any other pixel
			 * */
			Color black = Color.FromArgb(255, 0, 0, 0);
			Color white = Color.FromArgb(255, 255, 255, 255);
			for (int i = 1; i < img.Width - 1; i++)
			{
				for (int j = 1; j < img.Height - 1; j++)
				{
					//get RGB values for current pixel
					Color color = img.GetPixel(i, j);
					byte r, g, b;
					r = color.R;
					g = color.G;
					b = color.B;
					//get RGB Values for pixel directly right of current pixel
					Color colorRight = img.GetPixel(i, j + 1);
					byte rR, gR, bR;
					rR = colorRight.R;
					gR = colorRight.G;
					bR = colorRight.B;
					//get RGB values for pixel directly under current pixel
					Color colorBottom = img.GetPixel(i + 1, j);
					byte rB, gB, bB;
					rB = colorBottom.R;
					gB = colorBottom.G;
					bB = colorBottom.B;
					//get RGB values for pixel diagonally to the bottom right of the current pixel
					Color colorBottomRight = img.GetPixel(i + 1, j + 1);
					byte rBR, gBR, bBR;
					rBR = colorBottomRight.R;
					gBR = colorBottomRight.G;
					bBR = colorBottomRight.B;

					/**the difference between the RGB values of each pixel are calculated and raised to the power of 2 respectively to get the
					 * absolute difference between values. 
					 **/
					double differenceRight = (Math.Pow((r - rR), 2) + Math.Pow((g - gR), 2) + Math.Pow((b - bR), 2));
					double differenceBottom = (Math.Pow((r - rB), 2) + Math.Pow((g - gB), 2) + Math.Pow((b - bB), 2));
					double differenceBottomRight = 0.0;
					if (diagonal)
					{
						differenceBottomRight = (Math.Pow((r - rBR), 2) + Math.Pow((g - gBR), 2) + Math.Pow((b - bBR), 2));
					}

					//instead of rooting the distance between the colour values, we rather raise the threshold to the power of 2 as well
					Double thresholdSquared = Math.Pow(threshold, 2);

					//if bottom right pixel was used to determine whether or not the pixel is an edge
					if (diagonal)
					{
						/**
						 * If either the right, bottom, or bottom right differs from the current pixel by some threshold, then the pixel is on an edge
						 */
						if (differenceBottom > thresholdSquared || differenceRight > thresholdSquared || differenceBottomRight > thresholdSquared)
							edge.SetPixel(i, j, white);
						else
							edge.SetPixel(i, j, black);
					}
					else
					{
						if (differenceBottom > thresholdSquared || differenceRight > thresholdSquared)
							edge.SetPixel(i, j, white);
						else
							edge.SetPixel(i, j, black);
					}
				}
			}
		}

	}

	class GaussianFilter : Strategy
	{
		String imagePath;

		public GaussianFilter(String _imagePath)
		{
			imagePath = _imagePath;
		}

		public virtual void execute()
		{
			bool isExists = System.IO.Directory.Exists("gaussian/");
			if (!isExists)
				System.IO.Directory.CreateDirectory("gaussian/");
			String[] pictures = Directory.GetFiles(imagePath, "*.jpg", SearchOption.TopDirectoryOnly);
			
			for (int i = 0; i < pictures.Length; i++)
			{
				Console.WriteLine(pictures[i]);
				Bitmap img = new Bitmap(pictures[i]);
				Bitmap gaus = new Bitmap(img.Width, img.Height);
				smooth(img, gaus);
				gaus.Save("gaussian/Filtered_" + new FileInfo(pictures[i]).Name);
			}
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
			//int[,] filter = new int[5, 5] { { 2, 4, 5, 4, 2 }, { 4, 9, 12, 9, 4 }, { 5, 12, 15, 12, 5 }, { 4, 9, 12, 9, 4 }, { 2, 4, 5, 4, 2 } };
				
			int[,] filter = new int[3,3] {{1,2,1}, {2,4,2}, {1,2,1}};
			//Loop through every pixel
			for (int i = 2; i < img.Width - 2; i++)
			{
				for (int j = 2; j < img.Height - 2; j++)
				{
					int smoothValueR = 0;
					int smoothValueG = 0;
					int smoothValueB = 0;
					int divisor = 0;

					//i = i coordinate of the pixel
					//j = j coordinate of the pixel

					//Loop around the current pixel
					for (int x = -1; x <= 1; x++)
					{
						for (int y = -1; y <= 1; y++)
						{
							try
							{
								//newI & newJ = coordinate of the pixel around the current pixel we are smoothing
								int newI = i + x;
								int newJ = j + y;

								//int r = (img.GetPixel(newI, newJ).R) * filter[x + 2, y + 2];	//to accomodate for x & y starting at -2 and filter indices only starting at [0,0]
								//int g = (img.GetPixel(newI, newJ).G) * filter[x + 2, y + 2];
								//int b = (img.GetPixel(newI, newJ).B) * filter[x + 2, y + 2];

								int r = (img.GetPixel(newI, newJ).R) * filter[x + 1, y + 1];	//to accomodate for x & y starting at -2 and filter indices only starting at [0,0]
								int g = (img.GetPixel(newI, newJ).G) * filter[x + 1, y + 1];
								int b = (img.GetPixel(newI, newJ).B) * filter[x + 1, y + 1];

								smoothValueR += r;
								smoothValueG += g;
								smoothValueB += b;
								divisor += filter[x + 1, y + 1];
							}
							catch
							{
								Console.WriteLine("Exception Caught");
								//Pixel out of bounds - do nothing with this pixel
							}
						}
					}

					int smoothedColorR = smoothValueR / divisor;
					int smoothedColorG = smoothValueG / divisor;
					int smoothedColorB = smoothValueB / divisor;

					Color newPixel = Color.FromArgb(smoothedColorR, smoothedColorG, smoothedColorB);
					gaus.SetPixel(i, j, newPixel);
				}
			}
		}
	}

	class Greyscaler : Strategy
	{
		public Greyscaler(String _imagePath)
		{
		
		}

		public virtual void execute()
		{
			
		}
	} 
}
