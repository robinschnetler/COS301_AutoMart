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
            colourBuckets.Add(new ColourBucket("White", 255, 255, 255, "FFFFFF"));      //0 white
            colourBuckets.Add(new ColourBucket("Silver", 192, 192, 192, "C0C0C0"));     //1 silver
            colourBuckets.Add(new ColourBucket("Grey", 128, 128, 128, "808080"));       //2 grey
            colourBuckets.Add(new ColourBucket("Black", 0, 0, 0, "000000"));            //3 black
            colourBuckets.Add(new ColourBucket("Blue", 0, 0, 255, "0000FF"));           //4 blue
            colourBuckets.Add(new ColourBucket("Turquoise", 64, 224, 208, "40E0D0"));   //5 turqioise
            colourBuckets.Add(new ColourBucket("Green", 0, 128, 0, "008000"));          //6 green
            colourBuckets.Add(new ColourBucket("Yellow", 255, 255, 0, "FFFF00"));       //7 yellow
            colourBuckets.Add(new ColourBucket("Gold", 255, 215, 0, "FFD700"));         //8 gold
            colourBuckets.Add(new ColourBucket("Orange", 255, 165, 0, "FFA500"));       //9 orange
            colourBuckets.Add(new ColourBucket("Brown", 73, 49, 28, "49311C"));         //10 brown
            colourBuckets.Add(new ColourBucket("Red", 255, 0, 0, "FF0000"));            //11 red
            colourBuckets.Add(new ColourBucket("Maroon", 128, 0, 0, "800000"));         //12 maroon
            colourBuckets.Add(new ColourBucket("Violet", 238, 130, 238, "EE82EE"));     //13 violet
            colourBuckets.Add(new ColourBucket("Bronze", 205, 127, 50, "CD7F32"));      //14 bronze
            colourBuckets.Add(new ColourBucket("Charcoal", 51, 51, 51, "333333"));      //15 charcoal
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
                _advertDetails.Error = "Cannot calculate coverage if car not found.";
                return;
            }
            ImageBlock[][] imageBlocks = getImageBlocks(_advertDetails.Image.ToBitmap(), _advertDetails.Rect);
            int dominantBucketIndex = getCarColourIndex(_advertDetails.Image.ToBitmap(), imageBlocks);        
            _advertDetails.Colour = colourBuckets[dominantBucketIndex].ColourName;
        }

        public ImageBlock[][] getImageBlocks(Bitmap _image, Rectangle _rect)
        {
            //number of cols and rows to divide the image into
            int num_cols = 25;
            int num_rows = 20;

            //column width and height
            double colWidth = (double)_rect.Width / num_cols;
            double rowHeight = (double)_rect.Height / num_rows;

            //minimum number of white pixels allowed int the block
            double pixels_treshold = 0.05;
            int max_white_pixels = (int)((colWidth * rowHeight) * pixels_treshold);

            //variables used to keept track of the current block in the 2D array of ImageBlocks
            int blockXCoord = 0, blockYCoord = 0;
            int nextBlockXCoord = blockXCoord + 1;
            int nextBlockYCoord = blockYCoord + 1;

            //variables used to keep track of the current block position and size in the image
            int current_x = 0, current_y = 0;
            int current_width = (int)Math.Round((nextBlockXCoord) * colWidth, 0, MidpointRounding.AwayFromZero);
            int current_height = (int)Math.Round((nextBlockYCoord) * rowHeight, 0, MidpointRounding.AwayFromZero);

            //image put through edge detection
            Bitmap edgedImage = drawEdge(_image.Clone(_rect, _image.PixelFormat));

            //TODO Remove saving of image when done
            //for visualisation purposes
            edgedImage.Save("edged.jpg");

            //image grided for visualisation purposes
            Bitmap gridedImage = gridImage(new Bitmap(edgedImage), num_cols, num_rows, colWidth, rowHeight);

            //TODO Remove saving of image when done
            //for visualisation purposes
            gridedImage.Save("grided.jpg");
            Bitmap colourImage = gridImage(_image.Clone(_rect, _image.PixelFormat), num_cols, num_rows, colWidth, rowHeight);

            //2D array of ImageBlocks
            ImageBlock[][] blocks = new ImageBlock[num_cols][];
            for (int k = 0; k < num_cols; k++)
                blocks[k] = new ImageBlock[num_rows];

            while (true)
            {
                //reset white pixel counter
                int num_white_pixels = 0;

                for (int y = current_y; y < current_y + current_height; y++)
                {
                    for (int x = current_x; x < current_x + current_width; x++)
                    {
                        //check if the pixel is white
                        if (edgedImage.GetPixel(x, y).ToArgb().Equals(Color.White.ToArgb()))
                            num_white_pixels++;

                        //check if the number of white pixels exceeds the threshold
                        if (num_white_pixels > max_white_pixels)
                        {
                            //for visualisation purposes
                            gridedImage = drawX(gridedImage, current_x, current_y, current_width, current_height);
                            colourImage = drawX(colourImage, current_x, current_y, current_width, current_height);

                            blocks[blockXCoord][blockYCoord] = new ImageBlock(current_x, current_y, current_width, current_height, false);
                            goto Next;
                        }
                    }
                }
                //block with pixel count less than the treshold
                blocks[blockXCoord][blockYCoord] = new ImageBlock(current_x, current_y, current_width, current_height, true);

            Next:
                {
                    //check if you can right
                    if (blockXCoord < num_cols - 1)
                    {
                        blockXCoord++;
                        nextBlockXCoord = blockXCoord + 1;
                        current_x += current_width;
                        current_width = (int)Math.Round(nextBlockXCoord * colWidth, 0, MidpointRounding.AwayFromZero) - current_x;
                    }
                    //check if you can go down
                    else if (blockYCoord < num_rows - 1)
                    {
                        blockYCoord++;
                        nextBlockYCoord = blockYCoord + 1;
                        blockXCoord = 0;
                        nextBlockXCoord = blockXCoord + 1;
                        current_x = 0;
                        current_y += current_height;
                        current_height = (int)Math.Round(nextBlockYCoord * rowHeight, 0, MidpointRounding.AwayFromZero) - current_y;
                    }
                    else
                    {
                        //for visualisation purposes
                        gridedImage.Save("gridedIndicated.jpg");
                        colourImage.Save("shit.jpg");

                        return blocks;
                    }
                }
            }
        }

        public int getCarColourIndex(Bitmap _image, ImageBlock[][] _blocks)
        {
            //get the middle block coordinates
            int midCol = _blocks.Length / 2;
            int midRow = _blocks[0].Length / 2;

            //current block coordinates
            int[] currentPos = new int[] { midCol, midRow };

            //possible moves
            int[][] moves = new int[4][];
            moves[0] = new int[] { -1, 0 }; //left
            moves[1] = new int[] { 0, -1 }; //up
            moves[2] = new int[] { 1, 0 }; //right
            moves[3] = new int[] { 0, 1 }; //down

            //current move
            int m = 0;

            //number of times to repeat the move
            int repeat = 1;

            bool validFound = _blocks[currentPos[0]][currentPos[1]].Valid;
            //while the valid block is not found keep circling
            while (validFound == false)
            {
                for (int i = 0; i < repeat; i++)
                {
                    currentPos[0] += moves[m][0];
                    currentPos[1] += moves[m][1];

                    if (currentPos[0] > 0 && currentPos[0] < _blocks.Length && currentPos[1] > 0 && currentPos[1] < _blocks[0].Length)
                    {
                        if (_blocks[currentPos[0]][currentPos[1]].Valid)
                        {
                            validFound = true;
                            break;
                        }
                    }
                }

                //next move
                m = (m == 3) ? 0 : (m + 1);
                //increase repeat every second move
                repeat = (m % 2 == 0) ? repeat + 1 : repeat;
            }

            int centX = currentPos[0];
            int centY = currentPos[1];

            //array to store colour occurances
            double[] colourCounter = new double[colourBuckets.Count];

            /* Positions representation
             * 1 2 3
             * 8 x 4
             * 7 6 5
             */
            int[][] positions = new int[8][];
            positions[0] = new int[] { -1, -1 }; //1
            positions[1] = new int[] { 0, -1 };  //2
            positions[2] = new int[] { 1, -1 }; //3
            positions[3] = new int[] { 1, 0 };  //4
            positions[4] = new int[] { 1, 1 };  //5
            positions[5] = new int[] { 0, 1 };  //6
            positions[6] = new int[] { -1, 1 }; //7
            positions[7] = new int[] { -1, 0 }; //8

            //used to increase the size of the circle
            int radius = 1;

            int dominantIndex = 0;

            //get the total number of valid blocks
            int validBlocks = numberOfValidBlocks(_blocks);

            int horizontal = (centX > _blocks.Length - centX) ? centX : _blocks.Length - centX;
            int vertical = (centY > _blocks[0].Length - centY) ? centY : _blocks[0].Length - centY;

            //maximum radius size the circle can expand to
            int maxRadius = (horizontal > vertical) ? horizontal : vertical;

            Random randomGenerator = new Random();
            double random;
            double probability = 0.0;

            colourCounter[getBlockColourIndex(_image, _blocks[centX][centY])] += (int)1 * (1 - probability);

            while (colourCounter[dominantIndex] < (validBlocks * 0.5) && radius < maxRadius)
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    int x = centX + (radius * positions[i][0]);
                    int y = centY + positions[i][1];

                    //check if the coordinates are not out of bounds
                    if (x < _blocks.Length && x > 0 && y < _blocks[0].Length && y > 0)
                    {
                        //check if the block is valid
                        if (_blocks[x][y].Valid)
                        {
                            random = randomGenerator.NextDouble();
                            if (random > probability)
                                colourCounter[getBlockColourIndex(_image, _blocks[x][y])] += (int)1 * (1 - probability);
                        }
                    }
                }
                //update the dominant index
                dominantIndex = getDominantIndex(colourCounter);
                //increse the radius
                radius++;
                //update the probability
                probability = (double)radius / maxRadius;
            }
            return dominantIndex;
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

            int hueTolerance = 30;	//Should be in the range of 5 - 10
            int saturationTolerance = 25;	//Should be approx 100
            int valueTolerance = 25;

            //set the tolerance
            /*int hueTolerance = 30;
            int saturationTolerance = 40;
            int valueTolerance = 40;*/

            //counter to determine the most dominant colour
            int[] colourCounter = new int[colourBuckets.Count];

            for (int x = _block.X_coord; x < _block.X_coord + _block.Width; x++)
            {
                for (int y = _block.Y_coord; y < _block.Y_coord + _block.Height; y++)
                {
                    //counter for looping through the colour buckets
                    int counter = 0;

                    Color clr = _image.GetPixel(x, y);
                    double p_hue = 0.0, p_saturation = 0.0, p_value = 0.0;
                    convertRGBtoHSV(clr, out p_hue, out p_saturation, out p_value);

                    foreach (ColourBucket currentBucket in colourBuckets)
                    {
                        double bin_hue = currentBucket.H;
                        double bin_saturation = currentBucket.S;
                        double bin_value = currentBucket.V;

                        if (inRange(p_hue, bin_hue, hueTolerance))  //Check if the hue is in range
                            if (inRange(p_saturation, bin_saturation, saturationTolerance))	//Check if the saturation is in range
                                if (inRange(p_value, bin_value, valueTolerance))	//Check if the value is in range
                                    colourCounter[counter] = colourCounter[counter] + 1;

                        counter++;
                    }
                }
            }

            //find the most occuring colour in the block
            int index = getDominantIndex(colourCounter);
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
            /*int max = Math.Max(colour.R, Math.Max(colour.G, colour.B));
            int min = Math.Min(colour.R, Math.Min(colour.G, colour.B));
            hue = colour.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;*/

            double max = Math.Max(colour.R / 255d, Math.Max(colour.G / 255d, colour.B / 255d));
            double min = Math.Min(colour.R / 255d, Math.Min(colour.G / 255d, colour.B / 255d));
            double difference = max - min;

            hue = colour.GetHue();
            saturation = Math.Round((max == 0) ? 0 : ((difference / max)*100), 2);
            value = Math.Round(max*100, 2);
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

        public Bitmap gridImage(Bitmap _image, int _numCols, int _numRows, double _colWidth, double _rowHeight)
        {

            using (Graphics graphics = Graphics.FromImage(_image))
            {
                Pen blackPen = new Pen(Color.LightBlue, 1);

                int current = (int)_colWidth;

                for (int i = 0, j = 2; i < _numCols - 1; i++, j++)
                {
                    graphics.DrawLine(blackPen, current, 0, current, _image.Height);
                    current = (int)Math.Round(j * _colWidth, 0, MidpointRounding.AwayFromZero);
                }

                current = (int)_rowHeight;
                for (int i = 0, j = 2; i < _numRows; i++, j++)
                {
                    graphics.DrawLine(blackPen, 0, current, _image.Width, current);
                    current = (int)Math.Round(j * _rowHeight, 0, MidpointRounding.AwayFromZero);
                }
                graphics.Save();
                return _image;
            }
        }

        public Bitmap drawX(Bitmap _image, int _x, int _y, int _width, int _height)
        {
            using (Graphics graphics = Graphics.FromImage(_image))
            {
                Pen redPen = new Pen(Color.Red, 1);
                int xReduction = (int)(_width * 0.9);
                int yReduction = (int)(_height * 0.9);

                graphics.DrawLine(redPen, _x + xReduction, _y + yReduction, _x + _width - xReduction, _y + _height - yReduction);
                graphics.DrawLine(redPen, _x + _width - xReduction, _y + yReduction, _x  + xReduction, _y + _height - yReduction);

                graphics.Save();
                return _image;
            }
        }

        public int getDominantIndex(double[] array)
        {
            int index = 0;
            for(int i = 1; i < array.Length; i++)
            {
                if(array[i] > array[index])
                    index = i;
            }
            return index;
        }

        public int getDominantIndex(int[] array)
        {
            int index = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > array[index])
                    index = i;
            }
            return index;
        }

        public int numberOfValidBlocks(ImageBlock[][] _blocks)
        {
            int count = 0;
            for(int j = 0; j < _blocks[0].Length; j++)
            {
                for(int i = 0; i < _blocks.Length; i++)
                {
                    if (_blocks[i][j].Valid)
                        count++;
                }
            }
            return count;
        }
    }
}
