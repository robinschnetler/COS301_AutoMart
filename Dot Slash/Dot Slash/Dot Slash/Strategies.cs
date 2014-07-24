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
	/// <summary>
	/// all extensions specified in extensions array will be considered when doing image processing
	/// </summary>
	static class Globals
	{
		static public String[] extensions = {"jpg", "jpeg", "bmp", "png", "pgm"};
	}
	//interface for multiple image proccessing tools: each sub-program will implement its own overriden execute() function
	interface Strategy
	{
		void execute();
	}

	/// <summary>
	/// Each image processing task must be  encapsulated into a class that inherits from interface Strategy. Based on the mathod called
	/// in ImageProcessor, the correct strategy object is created and executed.
	/// </summary>
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

		public void detectCars(String path)
		{
			strategy = new CarClassifier(path);
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

		public void resize(int width, int height)
		{
			strategy = new ImageResizer(imagePath, width, height);
			strategy.execute();
			strategy = null;
		}

		public void resize(String folder, int width, int height)
		{
			strategy = new ImageResizer(folder, width, height);
			strategy.execute();
			strategy = null;
		}
	} 

	/// <summary>
	/// Tool used to resize images in a directory to a size specified in the "size" variable
	/// </summary>
	class ImageResizer : Strategy
	{
		//set the desired size to resize to here in the format:(width, height)
		private Size size;
		private string imagePath;
		public ImageResizer(String _imagePath, int _width, int _height)
		{
			size = new Size(_width, _height);
			imagePath = _imagePath;
		}

		public virtual void execute()
		{
			String[] pictures = Tools.getImages(imagePath, Globals.extensions);
			bool isExists = System.IO.Directory.Exists("Resized/");
			if (!isExists)
				System.IO.Directory.CreateDirectory("Resized/");
			for (int i = 0; i < pictures.Length; i++)
			{
				//Check extension, create image based on file extension
				string ext = Path.GetExtension(pictures[i]);
				
				if (ext == ".pgm")	//Image is greyscale
				{
					Image<Gray, Byte> img = new Image<Gray, Byte>(pictures[i]);
					img = img.Resize(size.Width, size.Height, Emgu.CV.CvEnum.INTER.CV_INTER_NN);
					img.Save("Resized/resized_" + new FileInfo(pictures[i]).Name);
					Tools.UpdateProgress(i + 1, pictures.Length, 50, '=');
				}
				else	//Assume image is in Bgra format (eg jpg or png)
				{
					Image<Bgra, Byte> img = new Image<Bgra, Byte>(pictures[i]);
					img = img.Resize(size.Width, size.Height, Emgu.CV.CvEnum.INTER.CV_INTER_NN);
					img.Save("Resized/resized_" + new FileInfo(pictures[i]).Name);
					Tools.UpdateProgress(i + 1, pictures.Length, 50, '=');
				}
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Resized " + pictures.Length + " images from " + imagePath + " to " + "Resized/");
		}
	}

	class EdgeDetector : Strategy
	{
		private string imagePath;

		public EdgeDetector(String _imagePath)
		{
			imagePath = _imagePath;
		}

		public virtual void execute()
		{
			bool isExists = System.IO.Directory.Exists("Edged/");
			if (!isExists)
				System.IO.Directory.CreateDirectory("Edged/");
			bool gausExists = System.IO.Directory.Exists("Gaussian/");
			if (gausExists)
				imagePath = "Gaussian/";
			String[] pictures = Tools.getImages(imagePath, Globals.extensions);
			for (int i = 0; i < pictures.Length; i++)
			{
				Bitmap img = new Bitmap(pictures[i]);
				Image<Gray, Byte> edgedImg;
				edgedImg = makeEdge(img);
				edgedImg.Save("Edged/Edged_" + new FileInfo(pictures[i]).Name);
				Tools.UpdateProgress(i + 1, pictures.Length, 50, '=');
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Edged " + pictures.Length + " images from " + imagePath + " to " + "Edged/");
		}

		public Image<Gray, Byte> makeEdge(Bitmap img)
		{
			Image<Gray, Byte> image = new Image<Gray, Byte>(img);
			image = image.Canny(39, 60);
			return image;
		}

	}

	class CarClassifier : Strategy
	{
		String imagePath;
		public CarClassifier(String _imagePath)
		{
			imagePath = _imagePath;
		}

		public virtual void execute()
		{
			String[] pictures = Tools.getImages(imagePath, Globals.extensions);
			bool existsDetected = Directory.Exists("Detected/");
			if(!existsDetected)
				Directory.CreateDirectory("Detected/");
			CascadeClassifier cc = new CascadeClassifier("classifier/cascade.xml");
			Bgr pen = new Bgr(255, 153, 51);
			for (int i = 0; i < pictures.Length; i++)
			{
				Tools.UpdateProgress(i + 1, pictures.Length, 50, '=');
				Image<Gray, Byte> image = new Image<Gray,Byte>(pictures[i]);
				Image<Bgr, Int32> original = new Image<Bgr, Int32>(pictures[i]);
				Rectangle[] objects = cc.DetectMultiScale(image,1.05, 16, new Size(150, 90), new Size(480, 320));
				//Console.WriteLine("Number of Rectangles: " + objects.Length);
				for (int j = 0; j < objects.Length; j++)
				{
					try
					{ 
						original.Draw(objects[j], pen, 2);
					}
					catch(IndexOutOfRangeException)
					{ }

				}
				if(objects.Length > 0)
					original.Save("Detected/Detected_" + new FileInfo(pictures[i]).Name);
			}

			Console.WriteLine();
			Console.WriteLine();
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
			String[] pictures = Tools.getImages(imagePath, Globals.extensions);
			for (int i = 0; i < pictures.Length; i++)
			{
				Bitmap img = new Bitmap(pictures[i]);
				Image<Bgra, Byte> gaus;
				gaus = smooth(img);
				gaus.Save("Gaussian/Filtered_" + new FileInfo(pictures[i]).Name);
				Tools.UpdateProgress(i + 1, pictures.Length, 50, '=');
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Smoothed " + pictures.Length + " images from " + imagePath + " to " + "Gaussian/");
		}

		public Image<Bgra, Byte> smooth(Bitmap img)
		{
			Image<Bgra, Byte> smoothed = new Image<Bgra, Byte>(img);
			smoothed = smoothed.SmoothGaussian(3);
			return smoothed;
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
			bool gausExists = System.IO.Directory.Exists("Resized/");
			if (gausExists)
				imagePath = "Resized/";
			String[] pictures = Tools.getImages(imagePath, Globals.extensions);
			for (int i = 0; i < pictures.Length; i++)
			{
				Bitmap img = new Bitmap(pictures[i]);
				Image<Gray, Byte> grey;
				grey = makeGreyScale(img);
				grey.Save("Greyscale/greyscaled_" + new FileInfo(pictures[i]).Name);
				Tools.UpdateProgress(i + 1, pictures.Length, 50, '=');
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Greyscaled " + pictures.Length + " images from " + imagePath + " to " + "Greyscaled/");
		}

		private Image<Gray, Byte> makeGreyScale(Bitmap originalImage)
		{
			Image<Bgra, Byte> c = new Image<Bgra, Byte>(originalImage);
			Image<Gray, Byte> img = c.Convert<Gray, Byte>();
			return img;
		}
	}
}
