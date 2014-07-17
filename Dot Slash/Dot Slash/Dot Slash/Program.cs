using System;
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
			Console.WriteLine("1) Resizing to 480 X 240");
			Console.WriteLine("2) apply gaussian filter");
			Console.WriteLine("3) grey scaling");
			Console.WriteLine("4) edge Detection");
			Console.WriteLine("5) generate integral image");
			Console.WriteLine("6) crop images");
			Console.WriteLine("7) filter images by hand");
			Console.WriteLine("8) create samples.dat");
			Console.WriteLine("9) Exit");
		}

		[STAThread] //allows for main to open dialogs(something to do with threads)
		static void Main(string[] args)
		{
			ImageProcessor imageProcessor = new ImageProcessor();
			Tools tools = new Tools();
			display();
			int chosen = Convert.ToInt32(Console.ReadLine());
			while(chosen != 9)
			{ 
				switch(chosen)
				{ 
					case 1:
						{ 
							Console.WriteLine("resizing");
							imageProcessor.resize();
							break;
						}
					case 2:
						{
							Console.WriteLine("Applying Gauusian Filter");
							imageProcessor.applyGaussian();
							break;
						}
					case 3:
						{
							Console.WriteLine("Changing images to greyscale");
							imageProcessor.makeGreyscale();
							break;
						}
					case 4:
						{
							Console.WriteLine("Applying Edge Detector");
							imageProcessor.detectEdges();
							break;
						}
					case 5:
						{
							Console.WriteLine("creating integral image(summed are table) for 'integral/grey.jpg'");
							Tools t = new Tools();
							Bitmap integralImage = t.generateIntegralImage("integral/grey.jpg");
							integralImage.Save("integral/integralimage.jpg");
							Console.WriteLine("saving integral image for 'integral/grey.jpg' as 'integral/integralimage.jpg'");
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
							tools.createDat();
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
}
