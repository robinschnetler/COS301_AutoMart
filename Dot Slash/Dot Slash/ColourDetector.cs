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
    public class ColourDetector : Filter
    {
        ArrayList colourBuckets; 
        public ColourDetector()
        {
            colourBuckets  = new ArrayList();
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
            //colourBuckets.Add(new ColourBucket("Beige", 245, 245, 220
        }

        public void pump(ref AdvertDetails _advertDetails)
        {
		if(!_advertDetails.CarFound)
			_advertDetails.ExceptionList.Add("Car not found");
		Bitmap img = _advertDetails.Image.ToBitmap().Clone(_advertDetails.Rect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
		int width = img.Width;
		int height = img.Height;
		String[] colours = loopThroughPixels(1, 0, colourBuckets, width, height, img);
		_advertDetails.Colour1 = colours[0];
		_advertDetails.Colour2 = colours[1];
		_advertDetails.Colour3 = colours[2];
        }

        private String[] loopThroughPixels(int pixelHop, int numBinsToEliminate, ArrayList colourBuckets, int width, int height, Bitmap img)
        {
            int hueTolerance = 2;		//Should be in the range of 5 - 10
            int saturationTolerance = 80;	//Should be approx 100
            int valueTolerance = 150;	//Should be in the range of 170 - 200

            int counter = 0;
            int numColourBuckets = colourBuckets.Count;

            int[] colourCounter = new int[numColourBuckets];

            for (int i = 0; i < numColourBuckets; i++)
            {
                colourCounter[i] = 0;
            }

            foreach (ColourBucket currentBucket in colourBuckets)
            {
                int halfx = width / 2;
                int halfy = height / 2;
                int thirdx = width / 3;
                int thirdy = height / 3;

                for (int x = width / 4; x < width * 3 / 4; x++)
                {
                    for (int y = height / 2; y < height * 3 / 4; y += pixelHop)
                    {
                        Color clr = img.GetPixel(x, y);

                        double p_hue = 0.0, p_saturation = 0.0, p_value = 0.0;
                        double bin_hue = currentBucket.h;
                        double bin_saturation = currentBucket.s;
                        double bin_value = currentBucket.v;
                        convertRGBtoHSV(clr, out p_hue, out p_saturation, out p_value);
                        if (inRange(p_hue, bin_hue, hueTolerance))						//Check if the hue is in range
                            if (inRange(p_saturation * 255, bin_saturation * 255, saturationTolerance))	//Check if the saturation is in range
                                if (inRange(p_value * 255, bin_value * 255, valueTolerance))		//Check if the value is in range
                                    colourCounter[counter] = colourCounter[counter] + 1;
                    }
                }
                counter++;
            }

            int max = -99999;
            int index1 = 0;
            for (int i = 0; i < colourCounter.Length; i++)
            {
                if (colourCounter[i] >= max)
                {
                    index1 = i;
                    max = colourCounter[i];
                }
            }

            ColourBucket dominantColour = (ColourBucket)colourBuckets[index1];
            String firstMostCommon = dominantColour.colourName;

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
            return (new String[] { firstMostCommon, secondMostCommon, thirdMostCommon });
        }

        private bool inRange(double pixelValue, double binValue, int range)
        {
            if (pixelValue > (binValue + range) || pixelValue < (binValue - range))
                return false;
            else
                return true;
        }
        /// <summary>
        /// A function that will convert a colour from RGB colour space to HSV colour space (as seen on stackoverflow.com/questions/359612/how-to-change-rgb-color-to-hsv)
        /// </summary>
        /// <param name="colour"> The RGB colour to convert </param>
        /// <param name="hue"> An out parameter that returns the Hue of the colour in range of 0 - 360</param>
        /// <param name="saturation"> An out parameter that returns the Saturation of the colour in range of 0 - 1</param>
        /// <param name="value"> An out parameter that returns the Value of the colour in range of 0 - 1</param>
        private void convertRGBtoHSV(Color colour, out double hue, out double saturation, out double value)
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
        private Color convertHSVtoRGB(double hue, double saturation, double value)
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
