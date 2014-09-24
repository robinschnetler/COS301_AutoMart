using System;
using System.Collections.Generic;
using System.Collections;
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
	class ColourBucket
	{
		//				   Colour Bins
		// Colour		INT			HEX		R	G	B
		//==================================================================================
		//White			16777215		FFFFFF		255	255	255
		//Silver		12632256		C0C0C0		192	192	192
		//Grey			8421504			808080		128	128	128
		//Black			0			000000		0	0	0
		//Blue			255			0000FF		0	0	255
		//Turquoise		4251856			40E0D0		64	224	208
		//Green			32768			008000		0	128	0
		//Yellow		16776960		FFFF00		255	255	0
		//Gold			16766720		FFD700		255	215	0
		//Orange		16753920		FFA500		255	165	0
		//Brown			4796700			49311C		73	49	28
		//Red			16711680		FF0000		255	0	0
		//Maroon		8388608			800000		128	0	0
		//Violet		15631086		EE82EE		238	130	238
		//Beige			16119260		F5F5DC		245	245	220
		//Bronze		13467442		CD7F32		205	127	50
		//Charcoal		3355443			333333		51	51	51

		public string colourName;
		public int r;
		public int g;
		public int b;

		public double h;
		public double s;
		public double v;

		public ColourBucket(string name, int red, int green, int blue)
		{
			colourName = name;
			r = red;
			g = green;
			b = blue;

			int max = Math.Max(r, Math.Max(g, b));
			int min = Math.Min(r, Math.Min(g, b));

			h = Color.FromArgb(r, g, b).GetHue();
			s = (max == 0) ? 0 : 1d - (1d * min / max);
			v = max / 255d;
		}
	}

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
	public class ImageProcessor
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

		public void detect(String path, String classifier, String to, Bgr b, Size min, Size max, int n, Boolean input)
		{
			strategy = new Classifier(path, classifier, to, b, min, max, n, input);
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
	public class ImageResizer : Strategy
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

	public class EdgeDetector : Strategy
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

	public class Classifier : Strategy
	{
		String imagePath;
		String classifer;
		String saveTo;
		Size min, max;
		Bgr pen;
		Boolean toCrop;
		int numNeighbours;
		public Classifier(String _imagePath, String _classifier, String _saveTo, Bgr _pen, Size _min, Size _max, int _numN, Boolean input)
		{
			imagePath = _imagePath;
			classifer = _classifier;
			saveTo = _saveTo;
			pen = _pen;
			min = _min;
			max = _max;
			numNeighbours = _numN;
			toCrop = input;
		}

		public virtual void execute()
		{
			String[] pictures = Tools.getImages(imagePath, Globals.extensions);
			bool existsDetected = Directory.Exists(saveTo + "/");
			if (!existsDetected)
				Directory.CreateDirectory(saveTo + "/");
			existsDetected = Directory.Exists(saveTo + "/" + "Cropped/");
			if (!existsDetected)
				Directory.CreateDirectory(saveTo + "/" + "Cropped/");
			
			CascadeClassifier cc = new CascadeClassifier(classifer);
			//new CascadeClassifier("classifier/1.xml"), new CascadeClassifier("classifier/2.xml"), 
			//new CascadeClassifier("classifier/3.xml"), new CascadeClassifier("classifier/6.xml"), 
			//new CascadeClassifier("classifier/7.xml"), new CascadeClassifier("classifier/8.xml"), 
			//new CascadeClassifier("classifier/9.xml") 
			

			//bonnet (44, 22)
			for (int i = 0; i < pictures.Length; i++)
			{
				Tools.UpdateProgress(i + 1, pictures.Length, 50, '=');
				Image<Gray, Byte> image = new Image<Gray,Byte>(pictures[i]);
				Image<Bgr, Int32> original = new Image<Bgr, Int32>(pictures[i]);
				Rectangle[] rectangleList = cc.DetectMultiScale(image, 1.05, numNeighbours, min, max);
				if (toCrop)
				{
					if (rectangleList.Length > 0)
						original.GetSubRect(rectangleList.Last()).Save(saveTo + "/Cropped/Detected_" + new FileInfo(pictures[i]).Name);
				}
				for (int j = 0; j < rectangleList.Length; j++)
				{
					original.Draw(rectangleList[j], pen, 2);	
				}
				original.Save(saveTo+"/Detected_" + new FileInfo(pictures[i]).Name);
				
			}
			Console.WriteLine();
			Console.WriteLine();
		}
	}

	public class GaussianFilter : Strategy
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

	public class Greyscaler : Strategy
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
			//bool gausExists = System.IO.Directory.Exists("Resized/");
			//if (gausExists)
			//	imagePath = "Resized/";
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

	public class Coverage : Strategy
	{
		String imagePath;
		Rectangle r;
		public Coverage(String _imagePath, Rectangle _rectangle)
		{
			imagePath = _imagePath;
		}

		public virtual void execute()
		{
			String[] images = Tools.getImages(imagePath, Globals.extensions);

			double area = r.Width * r.Height;
			area /= 1536; // (480*320)*100 = %
		}
	}
}
