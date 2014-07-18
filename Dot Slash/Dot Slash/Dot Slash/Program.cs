﻿using System;
using System.Collections.Generic;
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
	//main entry point into the program
	class Program
	{
		/// <summary>
		/// display function to show program options to user
		/// </summary>
		static void display()
		{
			Console.WriteLine("Image processing options:");
			Console.WriteLine("1) Resizing (Default: to 480 X 240 in 'images/')");
			Console.WriteLine("2) Apply Gaussian Filter");
			Console.WriteLine("3) Greyscaling");
			Console.WriteLine("4) Edge Detection");
			Console.WriteLine("5) Generate Integral Image");
			Console.WriteLine("6) Crop images");
			Console.WriteLine("7) Filter images by hand");
			Console.WriteLine("8) Create positives.dat");
			Console.WriteLine("9) Create negatives.dat");
			Console.WriteLine("10) Change filename extensions");
			Console.WriteLine("11) Exit");
		}

		[STAThread] //allows for main to open dialogs(something to do with threads)
		static void Main(string[] args)
		{
			ImageProcessor imageProcessor = new ImageProcessor();
			Tools tools = new Tools();
			display();
			int chosen = Convert.ToInt32(Console.ReadLine());
			while(chosen != 11)
			{ 
				switch(chosen)
				{ 
					case 1:
						{ 
							String input;
							Console.WriteLine("Resizing Images");
							Console.WriteLine("Width: (default = 480)");
							int w, h;
							input = Console.ReadLine();
							if(input.Length == 0)
							{
								w = 480;
							}
							else
							{
								w = Convert.ToInt32(input);
							}
							Console.WriteLine("Height: default = 240");
							input = Console.ReadLine();
							if(input.Length == 0)
							{
								h = 240;
							}
							else
							{
								h = Convert.ToInt32(input);
							}
							Console.WriteLine("Folder: (Default = 'images/')");
							string folder = Console.ReadLine() + "/";
							if(Directory.Exists(folder))
							{
								imageProcessor.resize(folder, w, h);
							}
							else
							{
								imageProcessor.resize(w, h);
							}
							Console.WriteLine();
							break;
						}
					case 2:
						{
							Console.WriteLine("Applying Gaussian Filter");
							imageProcessor.applyGaussian();
							Console.WriteLine();
							break;
						}
					case 3:
						{
							Console.WriteLine("Changing images to Greyscale");
							imageProcessor.makeGreyscale();
							Console.WriteLine();
							break;
						}
					case 4:
						{
							Console.WriteLine("Applying Edge Detector");
							imageProcessor.detectEdges();
							Console.WriteLine();
							break;
						}
					case 5:
						{
							Console.WriteLine("Creating Integral Image(summed area table) for 'integral/grey.jpg'");
							Tools t = new Tools();
							Bitmap integralImage = t.generateIntegralImage("integral/grey.jpg");
							integralImage.Save("integral/integralimage.jpg");
							Console.WriteLine("Saving integral image for 'integral/grey.jpg' as 'integral/integralimage.jpg'");
							Console.WriteLine();
							break;
						}
					case 6:
						{
							tools.photoCropper();
							break;
						}
					case 7:
						{
							tools.photoChooser();
							break;
						}
					case 8:
						{
							tools.createDat("Greyscale/", "positives.dat", true);
							break;
						}
					case 9:
						{
							tools.createDat("negative/", "negatives.dat", false);
							break;
						}
					case 10:
						{
							tools.changeExtension();
							break;
						}
					default:
						{
							Console.WriteLine("Please choose valid option");
							Console.WriteLine();
							break;
						}
				}
				display();
				chosen = Convert.ToInt32(Console.ReadLine());
				Console.WriteLine();
			}
		}
	}
}
