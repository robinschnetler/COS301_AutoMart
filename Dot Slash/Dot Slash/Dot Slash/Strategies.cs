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

		public void detectBlur(String path, double thresh)
		{
			strategy = new BlurDetector(path, thresh);
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

		public void detectColour(String path, String s)
		{
			strategy = new ColourDetector(path, s);
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

	public class BlurDetector : Strategy
	{
		String imagePath;
		double thresh;
		public BlurDetector(String path, double _thresh)
		{
			imagePath = path;
			thresh = _thresh;
		}
		public virtual void execute()
		{
			String[] pictures = Tools.getImages(imagePath, Globals.extensions);
			for (int i = 0; i < pictures.Length; i++)
			{
				Tools.UpdateProgress(i + 1, pictures.Length, 50, '=');
				Image<Gray, Byte> image = new Image<Gray,byte>(pictures[i]);
				Image<Gray, float> con = image.Laplace(1);
				Bitmap b = image.ToBitmap();
				double sum = 0;
				double avg = 0;
				int rad = 56;
				for (int j = 0; j < con.Width-1; j++)
				{

					for (int k = 0; k < con.Height-1; k++)
					{
						if (Math.Abs(b.GetPixel(j, k).R - b.GetPixel(j + 1, k).R) > rad || Math.Abs(b.GetPixel(j, k).R - b.GetPixel(j, k+1).R) > rad)
						{
							sum++;
						}
					}
				}
				avg =sum / (b.Width * b.Height) * 100;
				if(avg > thresh)
				{
					new Image<Bgr, Int32>(pictures[i]).Save("convoluted/non_blurry" + avg + "_" + new FileInfo(pictures[i]).Name);
					con.Save("convoluted/non_blurry" + avg + "_con_" + new FileInfo(pictures[i]).Name);
				}
				else
				{
					new Image<Bgr, Int32>(pictures[i]).Save("convoluted/blurry" + avg + "_" + new FileInfo(pictures[i]).Name);
					con.Save("convoluted/blurry" + avg + "_con_" + new FileInfo(pictures[i]).Name);
				}
			}
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


	public class ColourDetector : Strategy
	{
		static String imagePath;
		static String saveTo;

		public ColourDetector(String iP, String sT)
		{
			imagePath = iP;
			saveTo = sT;
		}

		public virtual void execute()
		{
			String[] pictures = Tools.getImages(imagePath, Globals.extensions);
			bool existsColourDetected = Directory.Exists(saveTo + "/");
			if(!existsColourDetected)
				Directory.CreateDirectory(saveTo + "/");

			ArrayList colourBuckets = new ArrayList();
			colourBuckets.Add(new ColourBucket("White", 255, 255, 255));
			colourBuckets.Add(new ColourBucket("Silver", 192, 192, 192));
			colourBuckets.Add(new ColourBucket("Grey", 128, 128, 128));
			colourBuckets.Add(new ColourBucket("Black", 0, 0, 0));
			colourBuckets.Add(new ColourBucket("Blue", 0, 0, 255));
			colourBuckets.Add(new ColourBucket("Turquoise", 64, 224, 208));
			colourBuckets.Add(new ColourBucket("Green", 0, 128, 0));
			colourBuckets.Add(new ColourBucket("Yellow", 255, 255, 0));
			colourBuckets.Add(new ColourBucket("Gold", 255, 215, 0));
			colourBuckets.Add(new ColourBucket("Orange", 255, 165, 0));
			colourBuckets.Add(new ColourBucket("Brown", 73, 49, 28));
			colourBuckets.Add(new ColourBucket("Red", 255, 0, 0));
			colourBuckets.Add(new ColourBucket("Maroon", 128, 0, 0));
			colourBuckets.Add(new ColourBucket("Violet", 238, 130, 238));
			colourBuckets.Add(new ColourBucket("Bronze", 205, 127, 50));
			colourBuckets.Add(new ColourBucket("Charcoal", 51, 51, 51));
			//colourBuckets.Add(new ColourBucket("Beige", 245, 245, 220));

			for (int i = 0; i < pictures.Length; i++)
			{
				Tools.UpdateProgress(i + 1, pictures.Length, 50, '=');
				Bitmap img = new Bitmap(pictures[i]);
				colourDetect(img, colourBuckets, i);
			}
		}

		static private String colourDetect(Bitmap img, ArrayList colourBuckets, int i)
		{
			int width = img.Width;
			int height = img.Height;

			//Loop through and find dominate colour
			loopThroughPixels(1, 0, colourBuckets, width, height, img, i);

			return "";
		}

		private static bool isInRange(double pixelValue, double binValue, int range)
		{
			if (pixelValue > (binValue + range) || pixelValue < (binValue - range))
				return false;
			else
				return true;
		}

		private static ArrayList loopThroughPixels(int pixelHop, int numBinsToEliminate, ArrayList colourBuckets, int width, int height, Bitmap img, int num)
		{
			int hueTolerance = 2;		//Should be in the range of 5 - 10
			int saturationTolerance = 80;	//Should be approx 100
			int valueTolerance = 150;	//Should be in the range of 170 - 200

			int counter = 0;
			//Console.WriteLine("NUM COLOUR BUCKETS: " + colourBuckets.Count);
			int numColourBuckets = colourBuckets.Count;

			int[] colourCounter = new int[numColourBuckets];

			for (int i = 0; i < numColourBuckets; i++)
			{
				colourCounter[i] = 0;
			}

			foreach (ColourBucket currentBucket in colourBuckets)
			{
				//Console.WriteLine("Testing on colour bucket " + counter);
				int halfx = width /2;
				int halfy = height / 2;
				int thirdx = width / 3;
				int thirdy = height / 3;

				for (int x = width/4; x < width * 3/4; x++)
				{
					for (int y = height/2; y < height * 3/4; y += pixelHop)
					{
						Color clr = img.GetPixel(x, y);

						double p_hue = 0.0, p_saturation = 0.0, p_value = 0.0;
						double bin_hue = currentBucket.h;
						double bin_saturation = currentBucket.s;
						double bin_value = currentBucket.v;
						convertRGBtoHSV(clr, out p_hue, out p_saturation, out p_value);
						//Console.WriteLine("HSV OF PIXEL: H" + p_hue + " S:" + p_saturation + " V: " + p_value);
						if (isInRange(p_hue, bin_hue, hueTolerance))						//Check if the hue is in range
							if (isInRange(p_saturation * 255, bin_saturation * 255, saturationTolerance))	//Check if the saturation is in range
								if (isInRange(p_value * 255, bin_value * 255, valueTolerance))		//Check if the value is in range
									colourCounter[counter] = colourCounter[counter] + 1;
					}
				}
				counter++;
			}

			int max = -99999;
			int index1 = 0;
			for (int i = 0; i < colourCounter.Length; i++)
			{
				//Console.WriteLine("colourCounter[" + i + "] = " + colourCounter[i]);
				if (colourCounter[i] >= max)
				{
					index1 = i;
					max = colourCounter[i];
				}
			}
			//Console.WriteLine();
			ColourBucket dominantColour = (ColourBucket) colourBuckets[index1];
			String firstMostCommon = dominantColour.colourName;
			//Console.WriteLine("Most common colour: " + dominantColour.colourName);

			max = -99999;
			int index2 = 0;
			for (int i = 0; i < colourCounter.Length; i++)
			{
				if (colourCounter[i] >= max && i != index1)
				{
					index2 = i;
					max = colourCounter[i];
				}
			}

			dominantColour = (ColourBucket)colourBuckets[index2];
			String secondMostCommon = dominantColour.colourName;
			//Console.WriteLine("Second most common colour: " + dominantColour.colourName);

			max = -99999;
			int index3 = 0;
			for (int i = 0; i < colourCounter.Length; i++)
			{
				if (colourCounter[i] >= max && i != index1 && i != index2)
				{
					index3 = i;
					max = colourCounter[i];
				}
			}

			dominantColour = (ColourBucket)colourBuckets[index3];
			String thirdMostCommon = dominantColour.colourName;
			img.Save(saveTo + "/" + firstMostCommon + "_" + secondMostCommon + "_" + thirdMostCommon + num + ".jpg");
			//Console.WriteLine("Third most common colour: " + dominantColour.colourName);

			return colourBuckets;
		}

		/// <summary>
		/// A function that will convert a colour from RGB colour space to HSV colour space (as seen on stackoverflow.com/questions/359612/how-to-change-rgb-color-to-hsv)
		/// </summary>
		/// <param name="colour"> The RGB colour to convert </param>
		/// <param name="hue"> An out parameter that returns the Hue of the colour in range of 0 - 360</param>
		/// <param name="saturation"> An out parameter that returns the Saturation of the colour in range of 0 - 1</param>
		/// <param name="value"> An out parameter that returns the Value of the colour in range of 0 - 1</param>
		private static void convertRGBtoHSV(Color colour, out double hue, out double saturation, out double value)
		{
			int max = Math.Max(colour.R, Math.Max(colour.G, colour.B));
			int min = Math.Min(colour.R, Math.Min(colour.G, colour.B));

			hue = colour.GetHue();
			saturation = (max == 0) ? 0 : 1d - (1d * min / max);
			value = max / 255d;
		}

		/// <summary>
		/// A function that converts a colour from HSV colour space to RGB colour space (as seen on stackoverflow.com/questions/359612/how-to-change-rgb-color-to-hsv)
		/// </summary>
		/// <param name="hue"> The hue of the colour </param>
		/// <param name="saturation"> The saturation of the colour </param>
		/// <param name="value"> The value of the colour </param>
		/// <returns> A colour object of the HSV colour in RGB colour space </returns>
		static private Color convertHSVtoRGB(double hue, double saturation, double value)
		{
			int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
			double f = hue / 60 - Math.Floor(hue / 60);

			value = value * 255;
			int v = Convert.ToInt32(value);
			int p = Convert.ToInt32(value * (1 - saturation));
			int q = Convert.ToInt32(value * (1 - f * saturation));
			int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

			if (hi == 0)
				return Color.FromArgb(255, v, t, p);
			else if (hi == 1)
				return Color.FromArgb(255, q, v, p);
			else if (hi == 2)
				return Color.FromArgb(255, p, v, t);
			else if (hi == 3)
				return Color.FromArgb(255, p, q, v);
			else if (hi == 4)
				return Color.FromArgb(255, t, p, v);
			else
				return Color.FromArgb(255, v, p, q);
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
