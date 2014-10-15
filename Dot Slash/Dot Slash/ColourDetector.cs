using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Dot_Slash
{
    /// <summary>
    /// Class used to detemine the colour of the car in the image.
    /// </summary>
    public class ColourDetector : Filter
    {
        List<ColourBucket> colourBuckets;

        /// <summary>
        /// Initilises a new instance of the ColourDetector class.
        /// </summary>
        public ColourDetector()
        {
            createColourBuckets();
        }

        /// <summary>
        /// Method creates ColourBucket objects representing possible colours that represent the car colour in the image.
        /// </summary>
        public void createColourBuckets()
        {
            colourBuckets = new List<ColourBucket>();
            colourBuckets.Add(new ColourBucket("White", 255, 255, 255, "FFFFFF"));
            colourBuckets.Add(new ColourBucket("Silver", 192, 192, 192, "C0C0C0"));
            colourBuckets.Add(new ColourBucket("Grey", 128, 128, 128, "808080"));
            colourBuckets.Add(new ColourBucket("Black", 0, 0, 0, "000000"));
            colourBuckets.Add(new ColourBucket("Blue", 0, 0, 255, "0000FF"));
            colourBuckets.Add(new ColourBucket("Turquoise", 64, 224, 208, "40E0D0"));
            colourBuckets.Add(new ColourBucket("Green", 0, 128, 0, "008000"));
            colourBuckets.Add(new ColourBucket("Yellow", 255, 255, 0, "FFFF00"));
            colourBuckets.Add(new ColourBucket("Gold", 255, 215, 0, "FFD700"));
            colourBuckets.Add(new ColourBucket("Orange", 255, 165, 0, "FFA500"));
            colourBuckets.Add(new ColourBucket("Brown", 73, 49, 28, "49311C"));
            colourBuckets.Add(new ColourBucket("Red", 255, 0, 0, "FF0000"));
            colourBuckets.Add(new ColourBucket("Maroon", 128, 0, 0, "800000"));
            colourBuckets.Add(new ColourBucket("Violet", 238, 130, 238, "EE82EE"));
            colourBuckets.Add(new ColourBucket("Bronze", 205, 127, 50, "CD7F32"));
            colourBuckets.Add(new ColourBucket("Charcoal", 51, 51, 51, "333333"));
        }

        /// <summary>
        /// Metod calculates the colour of the car and stores it in the AdvertDetails object.
        /// </summary>
        /// <param name="_advertDetails"></param>AdvertDetails object containing the information about the advert image.
        public void pump(ref AdvertDetails _advertDetails)
        {
            //check if the car exists
            if (!_advertDetails.CarFound)
            {
                _advertDetails.Error = "Cannot calculate coverage if car not found";
                return;
            }
            List<ImageBlock> imageBlocks = getImageBlocks(_advertDetails.Rect, _advertDetails.Image.ToBitmap());
            int dominantBucketIndex = getCarColourIndex(_advertDetails.Image.ToBitmap(), imageBlocks);        
            _advertDetails.Colour = colourBuckets[dominantBucketIndex].ColourName;
        }

        /// <summary>
        /// The method converts the image to a edged images. The edged image is then divided into block. Each block is 
        /// put through a test to determine if the block is situated on the edge or on the solid surface. As the edges are
        /// represented by white pixels, if the number of white pixels does not exceeds the maximum treshold of allowed 
        /// pixels the block is added to the list.
        /// </summary>
        /// <param name="_rect"></param>The rectangle specifying the location of the car in the image.
        /// <param name="_image"></param>The advert image.
        /// <returns>List containing ImageBlock objects.</returns>
        public List<ImageBlock> getImageBlocks(Rectangle _rect, Bitmap _image)
        {
            int num_rows = 12;
            int num_cols = 12;
            int step_x = (int)(_rect.Width / num_cols);
            int step_y = (int)(_rect.Height / num_rows);
            int current_x = 0, current_y = 0;
			int new_height = step_y * num_rows;
			int new_width = step_x * num_cols;
            List<ImageBlock> imageBlocks = new List<ImageBlock>();

            double dots_treshold = 0.05;
            int max_dots = (int)((_rect.Height * _rect.Width) * dots_treshold);

            Bitmap edgedImage = drawEdge(_image.Clone(_rect, _image.PixelFormat));
            edgedImage.Save("edged.jpg");
            Bitmap gridedImage = gridImage(new Bitmap(edgedImage), _rect.Width, _rect.Height, num_cols, num_rows, step_x, step_y);
            gridedImage.Save("grided.jpg");

            while(true)
            {
                int num_dots = 0;
				for (int y = current_y; y < current_y + step_y; y++)
                {
                    for (int x = current_x; x < current_x + step_x; x++)
                    {
                        if (edgedImage.GetPixel(x, y).ToArgb().Equals(Color.White.ToArgb()))
                            num_dots++;

                        if (num_dots > max_dots)
                        {
                            goto Next;
                        }
                    }
                }
                imageBlocks.Add(new ImageBlock(current_x, current_y, step_y, step_x));

            Next:
                {
                    if (current_x + step_x < new_width)
                    {
                        current_x += step_x;
                    }
                    else if (current_y + step_y < new_height)
                    {
                        current_y += step_y;
						current_x = 0;
                    }
                    else
                    {
                        return imageBlocks;
                    }
                }
            }
        }

        /// <summary>
        /// The method loops through the list of ImageBlocks and calculates the colour of each block. 
        /// The most occuring colour is the colour of the car.
        /// </summary>
        /// <param name="_image"></param>Image containg the car.
        /// <param name="_imageBlocks"></param>List of ImageBlock objects.
        /// <returns>Integer index representing the ColourBucket.</returns>
        public int getCarColourIndex(Bitmap _image, List<ImageBlock> _imageBlocks)
        {
            int[] colourCounter = new int[colourBuckets.Count];

            foreach (ImageBlock block in _imageBlocks)
            {
                colourCounter[getBlockColourIndex(_image, block)]++;
            }

            int max = colourCounter[0];
            int index = 0;
            for (int i = 1; i < colourCounter.Length; i++)
            {
                if (colourCounter[i] >= max)
                {
                    index = i;
                    max = colourCounter[i];
                }
            }
            return index;
        }

        /// <summary>
        /// The method loops through the pixel in the block and gets the pixel colour. The most occuring colour index
        /// is calculated and returned.
        /// </summary>
        /// <param name="_image"></param>Image of the car.
        /// <param name="_block"></param>ImageBlock containing the coordinates and the size of the block.
        /// <returns>Integer index representing the most dominant colour bucket in the block.</returns>
        public int getBlockColourIndex(Bitmap _image, ImageBlock _block)
        {
            int hueTolerance = 5;	//Should be in the range of 5 - 10
            int saturationTolerance = 50;	//Should be approx 100
            int valueTolerance = 50;	//Should be in the range of 170 - 200

            int[] colourCounter = new int[colourBuckets.Count];
            int counter = 0;

            foreach (ColourBucket currentBucket in colourBuckets)
            {
                for (int x = _block.X_coord; x < _block.X_coord + _block.Width; x++)
                {
                    for (int y = _block.Y_coord; y < _block.Y_coord + _block.Height; y++)
                    {
                        Color clr = _image.GetPixel(x, y);

                        double p_hue = 0.0, p_saturation = 0.0, p_value = 0.0;
                        double bin_hue = currentBucket.H;
                        double bin_saturation = currentBucket.S;
                        double bin_value = currentBucket.V;
                        convertRGBtoHSV(clr, out p_hue, out p_saturation, out p_value);
                        if (inRange(p_hue, bin_hue, hueTolerance))	//Check if the hue is in range
                            if (inRange(p_saturation * 255, bin_saturation * 255, saturationTolerance))	//Check if the saturation is in range
                                if (inRange(p_value * 255, bin_value * 255, valueTolerance))	//Check if the value is in range
                                    colourCounter[counter] = colourCounter[counter] + 1;
                    }
                }
                counter++;
            }

            //find the most occuring colour in the block
            int max = colourCounter[0];
            int index = 0;
            for (int i = 1; i < colourCounter.Length; i++)
            {
                if (colourCounter[i] >= max)
                {
                    index = i;
                    max = colourCounter[i];
                }
            }
            return index;
        }

        /// <summary>
        /// Returns the edged image.
        /// </summary>
        /// <param name="_image"></param>Original image to be edged.
        /// <returns>Edged Bitmap image.</returns>
        public Bitmap drawEdge(Bitmap _image)
        {
            Image<Gray, Byte> edgesImage = new Image<Gray, Byte>(_image);
            edgesImage = edgesImage.Canny(39, 60);
            return edgesImage.ToBitmap();
        }

        /// <summary>
        /// Method checks if the specific colour value is in range.
        /// </summary>
        /// <param name="pixelValue"></param>Image pixel colour value.
        /// <param name="binValue"></param>ColourBucket colour value.
        /// <param name="range"></param>Allowed range.
        /// <returns>Boolean value stating if the pixel colour is with in the range of the colourBucket.</returns>
        public bool inRange(double pixelValue, double binValue, int range)
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
        public void convertRGBtoHSV(Color colour, out double hue, out double saturation, out double value)
        {
            /*double max = Math.Max(colour.R / 255d, Math.Max(colour.G / 255d, colour.B / 255d));
            double min = Math.Min(colour.R / 255d, Math.Min(colour.G / 255d, colour.B / 255d));
            double difference = max - min;

            hue = colour.GetHue();
            saturation = (max == 0) ? 0 : ((difference / max)*100);
            value = max*100;*/
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
        public Color convertHSVtoRGB(double hue, double saturation, double value)
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

        public Bitmap gridImage(Bitmap image, int width, int height, int cols, int rows, int col_step, int row_step)
        {
            int current = col_step;
            using (Graphics graphics = Graphics.FromImage(image))
            {
                Pen blackPen = new Pen(Color.LightBlue, 1);
                for (int i = 0; i < cols; i++)
                {
                    graphics.DrawLine(blackPen, current, 0, current, height);
                    current += col_step;
                }

                current = row_step;
                for (int i = 0; i < rows; i++)
                {
                    graphics.DrawLine(blackPen, 0, current, width, current);
                    current += row_step;
                }
                graphics.Save();
                return image;
            }
        }
    }
}
