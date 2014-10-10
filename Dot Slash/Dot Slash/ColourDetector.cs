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
            List<ImageBlock> imageBlocks = getImageBlocks(_advertDetails);
            ImageBlock dominantBlock = getDominantBlock(_advertDetails, imageBlocks);
            String[] colour = dominantBlock.Colours;
            _advertDetails.Colour1 = colour[0];
            _advertDetails.Colour2 = colour[1];
            _advertDetails.Colour3 = colour[2];
        }

        private List<ImageBlock> getImageBlocks(AdvertDetails _advertDetails)
        {
            int num_rows = 15;
            int num_cols = 15;
            int step_x = (int)(_advertDetails.Rect.Height / num_rows);
            int step_y = (int)(_advertDetails.Rect.Width / num_cols);
            int current_x = 0, current_y = 0;
            List<ImageBlock> imageBlocks = new List<ImageBlock>();

            double dots_treshold = 0.15;
            int max_dots = (int)((_advertDetails.Rect.Height * _advertDetails.Rect.Width) * dots_treshold);

            Bitmap edgedImage = drawEdge(_advertDetails.Image.ToBitmap());
            edgedImage.Save("EdgedImage.jpg");

            while(true)
            {
                int num_dots = 0;
                for (int x = current_x; x < current_x + step_x; x++)
                {
                    for (int y = current_y; y < current_y + step_y; y++)
                    {
                        if (edgedImage.GetPixel(x, y).ToArgb().Equals(Color.White.ToArgb()))
                            num_dots++;

                        if (num_dots > max_dots)
                        {
                            goto Next;
                        }
                    }
                }
                imageBlocks.Add(new ImageBlock(current_x, current_y, step_x, step_y));

            Next:
                {
                    Console.WriteLine("current_x:" + current_x + " current_y:" + step_y);
                    if (current_x + step_x < _advertDetails.Rect.Height)
                    {
                        current_x += step_x;
                        current_y = 0;
                    }
                    else if (current_y + step_y < _advertDetails.Rect.Width)
                    {
                        current_y += step_y;
                    }
                    else
                    {
                        return imageBlocks;
                    }
                }
            }
        }

        private ImageBlock getDominantBlock(AdvertDetails _advertDetails, List<ImageBlock> _imageBlocks)
        {
            List<List<ImageBlock>> groupedBlocks = new List<List<ImageBlock>>();
            bool added = false;
            foreach (ImageBlock block in _imageBlocks)
            {
                block.Colours = getBlockColours(_advertDetails.Image.ToBitmap(), block);
                added = false;

                foreach (List<ImageBlock> list in groupedBlocks)
                {
                    if (containsColour(list, block))
                    {
                        list.Add(block);
                        added = true;
                        break;
                    }
                }
                if (!added)
                    groupedBlocks.Add(new List<ImageBlock>() { block });
            }

            groupedBlocks.Sort(
                delegate(List<ImageBlock> b1, List<ImageBlock> b2)
                {
                    return b1.Count.CompareTo(b2.Count);
                }
            );

            return groupedBlocks.First().First();
        }


        private bool containsColour(List<ImageBlock> _list, ImageBlock _block)
        {
            if (_list.First().Colours[1].Equals(_block.Colours[1]))
                return true;
            else
                return false;
        }

        private Bitmap drawEdge(Bitmap img)
        {
            Image<Gray, Byte> image = new Image<Gray, Byte>(img);
            image = image.Canny(39, 60);
            return image.ToBitmap();
        }

        /// <summary>
        /// The function returns the three main colours of the the specified block.
        /// </summary>
        /// <param name="_image"></param>
        /// <param name="_block"></param>
        /// <returns></returns>
        private String[] getBlockColours(Bitmap _image, ImageBlock _block)
        {
            int pixelHop = 1;
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
                for (int x = _block.X_coord; x < _block.X_coord + _block.Height; x++)
                {
                    for (int y = _block.Y_coord; y < _block.Y_coord + _block.Width; y += pixelHop)
                    {
                        Color clr = _image.GetPixel(x, y);

                        double p_hue = 0.0, p_saturation = 0.0, p_value = 0.0;
                        double bin_hue = currentBucket.h;
                        double bin_saturation = currentBucket.s;
                        double bin_value = currentBucket.v;
                        convertRGBtoHSV(clr, out p_hue, out p_saturation, out p_value);
                        if (inRange(p_hue, bin_hue, hueTolerance))						//Check if the hue is in range
                            if (inRange(p_saturation * 255, bin_saturation * 255, saturationTolerance))	//Check if the saturation is in range
                                if (inRange(p_value * 255, bin_value * 255, valueTolerance))		//Check if the value is in range
                                    colourCounter[counter]++;// = colourCounter[counter] + 1;
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
