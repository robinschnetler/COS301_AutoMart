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
	static class Globals
	{
		static public String[] extensions = {"jpg", "jpeg", "bmp", "png", "pbm"};
	}
	//interface for multiple image proccessing tools: each sub-program will implement its own overriden execute() function
	interface Strategy
	{
		void execute();
	}


	/// <summary>
	/// Tool used to resize images in a directory to a size specified in the "size" variable
	/// </summary>
	class ImageResizer : Strategy
	{
		//set the desired size to resize to here in the format:(width, height)
		private Size size = new Size(480, 240);
		private string imagePath;
		public ImageResizer(String _imagePath)
		{
			imagePath = _imagePath;
		}

		public virtual void execute()
		{
			String[] pictures = Tools.getImages(imagePath, Globals.extensions);
			bool isExists = System.IO.Directory.Exists("Resized/");
			if (!isExists)
				System.IO.Directory.CreateDirectory("Resized/");
			ProgressBar progress = new ProgressBar();
			for (int i = 0; i < pictures.Length; i++)
			{
				Bitmap img = new Bitmap(pictures[i]);
				Bitmap resized = new Bitmap(img, size);
				resized.Save("Resized/resized_" + new FileInfo(pictures[i]).Name);
				progress.UpdateProgress(i + 1, pictures.Length, 50, '=');
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
			ProgressBar progress = new ProgressBar();
			for (int i = 0; i < pictures.Length; i++)
			{
				Bitmap img = new Bitmap(pictures[i]);
				Image<Gray, Byte> edgedImg;
				edgedImg = makeEdge(img);
				edgedImg.Save("Edged/Edged_" + new FileInfo(pictures[i]).Name);
				progress.UpdateProgress(i + 1, pictures.Length, 50, '=');
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Edged " + pictures.Length + " images from " + imagePath + "/ to " + "Edged/");
		}

		public Image<Gray, Byte> makeEdge(Bitmap img)
		{
			Image<Gray, Byte> image = new Image<Gray, Byte>(img);
			image = image.Canny(39, 60);
			return image;
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
			ProgressBar progress = new ProgressBar();
			for (int i = 0; i < pictures.Length; i++)
			{
				Bitmap img = new Bitmap(pictures[i]);
				Image<Bgra, Byte> gaus;
				gaus = smooth(img);
				gaus.Save("Gaussian/Filtered_" + new FileInfo(pictures[i]).Name);
				progress.UpdateProgress(i + 1, pictures.Length, 50, '=');
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
			ProgressBar progress = new ProgressBar();
			for (int i = 0; i < pictures.Length; i++)
			{
				Bitmap img = new Bitmap(pictures[i]);
				Image<Gray, Byte> grey;
				grey = makeGreyScale(img);
				grey.Save("Greyscale/greyscaled_" + new FileInfo(pictures[i]).Name);
				progress.UpdateProgress(i + 1, pictures.Length, 50, '=');
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
