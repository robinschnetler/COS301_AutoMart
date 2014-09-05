using System;
using System.Collections;	//For ArrayLists
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HSVColourDetection
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

	class ColourBucket
	{
		public string colourName;
		public int r;
		public int g;
		public int b;

		public ColourBucket(string name, int red, int green, int blue)
		{
			colourName = name;
			r = red;
			g = green;
			b = blue;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Name of the image? [Make sure it's in the same folder]");
			string fileName = Console.ReadLine();
			Bitmap img = new Bitmap(fileName);

			Color dominantColour = colourDetect(img);
		}

		/// <summary>
		/// This colour determines the most dominant colour in the Bitmap image provided as a paramter.
		/// </summary>
		/// <param name="img"> The image that is tested for colour detection </param>
		/// <returns></returns>
		static private Color colourDetect(Bitmap img)
		{
			//A
			int width = img.Width;
			int height = img.Height;

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
			colourBuckets.Add(new ColourBucket("Beige", 245, 245, 220));
			colourBuckets.Add(new ColourBucket("Bronze", 205, 127, 50));
			colourBuckets.Add(new ColourBucket("Charcoal", 51, 51, 51));

			int numColourBuckets = colourBuckets.Count;

			int hueTolerance = 8;		//Should be in the range of 5 - 10
			int saturationTolerance = 100;	//Should be approx 100
			int valueTolerance = 180;	//Should be in the range of 170 - 200

			int[] bucketCounter = new int[numColourBuckets];

			for (int i = 0; i < numColourBuckets; i++ )
			{
				bucketCounter[i] = 0;
			}

			foreach(ColourBucket bucketColour in colourBuckets)
			{
				Color currentBucketColour = Color.FromArgb(bucketColour.r, bucketColour.g, bucketColour.b);

				
				//Loop through every 5th pixel
				for (int i = 0; i < width; i++)
				{
					for (int j = 0; j < height; j += 5)	//only evaluate every 5th pixel [CHANGE AND TEST]
					{
					//Loop through every colour bucket
					
						Color currentColour = img.GetPixel(i, j);
						double p_hue = 0.0, p_saturation = 0.0, p_value = 0.0;
						convertRGBtoHSV(currentColour, p_hue, p_saturation, p_value);

					}
				}
			}
			
			return new Color();
		}

		/// <summary>
		/// A function that will convert a colour from RGB colour space to HSV colour space (as seen on stackoverflow.com/questions/359612/how-to-change-rgb-color-to-hsv)
		/// </summary>
		/// <param name="colour"> The RGB colour to convert </param>
		/// <param name="hue"> An out parameter that returns the Hue of the colour </param>
		/// <param name="saturation"> An out parameter that returns the Saturation of the colour </param>
		/// <param name="value"> An out parameter that returns the Value of the colour </param>
		private static void convertRGBtoHSV(Color colour, double hue, double saturation, double value)
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
		static public Color convertHSVtoRGB(double hue, double saturation, double value)
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
}
