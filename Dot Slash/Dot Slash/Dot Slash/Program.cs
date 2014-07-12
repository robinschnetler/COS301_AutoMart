using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.IO;

namespace Dot_Slash
{

	public static class Global
	{
		public const short FRONT_VIEW = 0;
		public const short BACK_VIEW = 1;
		public const short SIDE_VIEW = 2;
		public const short ANGLED_VIEW = 3;
	}

	//main entry point into the program
	class Program
	{
		static void display()
		{
			Console.WriteLine("Image processing options:");
			Console.WriteLine("1) Resizing to 300 X 200");
			Console.WriteLine("2) apply gaussian filter");
			Console.WriteLine("3) edge Detection");
			Console.WriteLine("4) grey scaling");
			Console.WriteLine("5) generate integral image");
			Console.WriteLine("6) crop images");
			Console.WriteLine("7) Exit");
		}
		[STAThread]
		static void Main(string[] args)
		{
			ImageProcessor imageProcessor = new ImageProcessor();
			Tools tools = new Tools();
			display();
			int chosen = Convert.ToInt32(Console.ReadLine());
			while(chosen != 7)
			{ 
				switch(chosen)
				{ 
					case 1:
						{ 
							Console.WriteLine("resizing");
							imageProcessor.resize();
							Console.WriteLine("Done resizing");
							break;
						}
					case 2:
						{
							Console.WriteLine("Applying Gauusian Filter");
							imageProcessor.applyGaussian();
							Console.WriteLine("Done Gaussian Filter");
							break;
						}
					case 3:
						{
							Console.WriteLine("Applying Edge Detector");
							imageProcessor.detectEdges();
							Console.WriteLine("Done Edge Detection");
							break;
						}
					case 4:
						{
							Console.WriteLine("Changing images to greyscale");
							imageProcessor.makeGreyscale();
							Console.WriteLine("Done greyscaling");
							break;
						}
					case 5:
						{
							Console.WriteLine("creating integral image(summed are table) for 'integral/grey.jpg'");
							Tools t = new Tools();
							int[,] ii = t.generateIntegralImage("integral/grey.jpg");
							Console.WriteLine("displaying integral image for 'integral/grey.jpg'");
							Bitmap b = new Bitmap("integral/grey.jpg");
							for(int i = 0; i<b.Width; i++)
							{
								for (int j = 0; j < b.Height; j++)
								{
									Console.Write(ii[i, j] + ".");
								}
								Console.WriteLine();
							}
							break;
						}
					case 6:
						{
							tools.photoCropper();
							break;
						}
					default:
						{
							Console.WriteLine("please choose valid option");
							break;
						}
				}
				display();
				chosen = Convert.ToInt32(Console.ReadLine());
			}
		}
	}

	//this is where our openCV shit is going to go
	class Classifier
	{
		
	}

	public class Tools
	{
		public void photoCropper()
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			Console.WriteLine("please select folder with all images of car");
			Console.WriteLine("Note: sub directories will not be parsed");
			String path = "";
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				path = dialog.SelectedPath;
			}
			String[] files = Directory.GetFiles(path, "*.jpg", SearchOption.TopDirectoryOnly);
			PictureCropper pc = new PictureCropper(files, 3, 2);
			pc.Activate();
			pc.ShowDialog();
		}

		public int[,] generateIntegralImage(String filename)
		{
			//We are getting in an image and returning the Summed Area Table (A 2x2 matrix of integers)
			//See http://computersciencesource.wordpress.com/2010/09/03/computer-vision-the-integral-image/
			Bitmap img = new Bitmap(filename);
			Console.WriteLine("Pixel value at [0,0] = " + img.GetPixel(0,0).R);
			int width = img.Width;
			int height = img.Height;

			int[,] SummedAreaTable = new int[width, height];

			//Initialize first value in the Summed Area Table as the top, right most value of the image

			//SummedAreaTable[0, 0] = img.GetPixel(0, 0).R;

			//Loop through every pixel in the image, row by row
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					//s(x,y) = i(x,y) + s(x-1, y) + s(x, y-1) - s(x-1, y-1)
					int value = 0;	//This value is s(x,y), the value in the Summed Area Table at co-ordinates x & y

					value += img.GetPixel(x, y).R;	//Since the image is greyscale, we can add any of the RGB components as they are all equal

					if (x - 1 >= 0)	//s(x-1, y) is in range in the Summed Area Table (otherwise the value to add is 0)
					{
						value += SummedAreaTable[x - 1, y];
					}

					if (y - 1 >= 0) //Similar to the above step
					{
						value += SummedAreaTable[x, y - 1];
					}
					
					if (x - 1 >= 0 && y - 1 >= 0) //Again, just checking ranges
					{
						value -= SummedAreaTable[x - 1, y - 1];
					}
					SummedAreaTable[x,y] = value;
				}
			}
			return SummedAreaTable;
		}
	}

	class ImageProcessor
	{
		String imagePath; 
		Strategy strategy;

		public ImageProcessor()
		{
			imagePath = "images/";
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

		public void resize()
		{
			strategy = new ImageResizer(imagePath);
			strategy.execute();
			strategy = null;
		}
	}

	//interface for multiple image proccessing tools: each sub-program will implement its own overriden execute() function
	interface Strategy
	{
		void execute();
	}

	class ImageResizer: Strategy
	{
		private Size size = new Size(45,30);
		private string imagePath;
		public ImageResizer(String _imagePath)
		{
			imagePath = _imagePath;
		}

		public virtual void execute()
		{
			String[] pictures = Directory.GetFiles(imagePath, "*.jpg", SearchOption.TopDirectoryOnly);
			bool isExists = System.IO.Directory.Exists("Resized/");
			if (!isExists)
				System.IO.Directory.CreateDirectory("Resized/");
			for (int i = 0; i < pictures.Length; i++)
			{
				Bitmap img = new Bitmap(pictures[i]);
				Bitmap resized = new Bitmap(img, size);
				resized.Save("Resized/resized_" + new FileInfo(pictures[i]).Name);
				Console.WriteLine(imagePath+"->Resized/resized_" + new FileInfo(pictures[i]).Name);
			}
		}
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
			bool isExists = System.IO.Directory.Exists("Edged/");
			if (!isExists)
				System.IO.Directory.CreateDirectory("Edged/");
			bool gausExists = System.IO.Directory.Exists("Gaussian/");
			if (gausExists)
				imagePath = "Gaussian/";
			for (int i = 0; i < pictures.Length; i++)
			{
				Bitmap img = new Bitmap(pictures[i]);
				Bitmap edgedImg = new Bitmap(img.Width, img.Height);
				makeEdge(img, edgedImg);
				edgedImg.Save("Edged/Edged_" + new FileInfo(pictures[i]).Name);
				Console.WriteLine(imagePath+ "-> Edged/Edged_" + new FileInfo(pictures[i]).Name);
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
			bool isExists = System.IO.Directory.Exists("Gaussian/");
			if (!isExists)
				System.IO.Directory.CreateDirectory("Gaussian/");
			bool resizedExists = System.IO.Directory.Exists("Resized/");
			if (resizedExists)
				imagePath = "Resized/";
			String[] pictures = Directory.GetFiles(imagePath, "*.jpg", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < pictures.Length; i++)
			{
				Bitmap img = new Bitmap(pictures[i]);
				Bitmap gaus = new Bitmap(img.Width, img.Height);
				smooth(img, gaus);
				gaus.Save("Gaussian/Filtered_" + new FileInfo(pictures[i]).Name);
				Console.WriteLine(imagePath + "-> Gaussian/Filtered_" + new FileInfo(pictures[i]).Name);
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
		String imagePath;
		public Greyscaler(String _imagePath)
		{
			imagePath = _imagePath;
		}

		public virtual void execute()
		{
			bool isExists = System.IO.Directory.Exists("Greyscale/");
			if (!isExists)
				System.IO.Directory.CreateDirectory("Greyscale/");
			bool gausExists = System.IO.Directory.Exists("Gaussian/");
			if(gausExists)
				imagePath = "Gaussian/";
			String[] pictures = Directory.GetFiles(imagePath, "*.jpg", SearchOption.TopDirectoryOnly);

			for (int i = 0; i < pictures.Length; i++)
			{
				Console.WriteLine(pictures[i]);
				Bitmap img = new Bitmap(pictures[i]);
				Bitmap grey = new Bitmap(img.Width, img.Height);
				makeGreyScale(img, grey);
				grey.Save("Greyscale/greyscaled_" + new FileInfo(pictures[i]).Name);
				Console.WriteLine(imagePath + "-> Greyscale/greyscaled_" + new FileInfo(pictures[i]).Name);
			}
		}

		private Bitmap makeGreyScale(Bitmap originalImage, Bitmap img)
		{
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
