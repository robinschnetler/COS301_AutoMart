using System;
using System.Collections.Generic;
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
		private List<ColourBucket> colourBuckets;

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
			colourBuckets.Add(new ColourBucket("Violet", 238, 130, 238, "EE82EE"));     //0 violet
			colourBuckets.Add(new ColourBucket("Blue", 0, 0, 255, "0000FF"));           //1 blue
			colourBuckets.Add(new ColourBucket("Turquoise", 64, 224, 208, "40E0D0"));   //2 turqioise
			colourBuckets.Add(new ColourBucket("Green", 0, 128, 0, "008000"));          //3 green
			colourBuckets.Add(new ColourBucket("Yellow", 255, 255, 0, "FFFF00"));       //4 yellow
			colourBuckets.Add(new ColourBucket("Gold", 255, 215, 0, "FFD700"));         //5 gold
			colourBuckets.Add(new ColourBucket("Orange", 255, 165, 0, "FFA500"));       //6 orange
			colourBuckets.Add(new ColourBucket("Bronze", 205, 127, 50, "CD7F32"));      //7 bronze
			colourBuckets.Add(new ColourBucket("Brown", 73, 49, 28, "49311C"));         //8 brown
			colourBuckets.Add(new ColourBucket("Red", 255, 0, 0, "FF0000"));            //9 red
			colourBuckets.Add(new ColourBucket("Maroon", 128, 0, 0, "800000"));         //10 maroon
			colourBuckets.Add(new ColourBucket("White", 255, 255, 255, "FFFFFF"));      //11 white
			colourBuckets.Add(new ColourBucket("Silver", 192, 192, 192, "C0C0C0"));     //12 silver
			colourBuckets.Add(new ColourBucket("Grey", 128, 128, 128, "808080"));       //13 grey
			colourBuckets.Add(new ColourBucket("Charcoal", 51, 51, 51, "333333"));      //14 charcoal
			colourBuckets.Add(new ColourBucket("Black", 0, 0, 0, "000000"));            //15 black
		}

		/// <summary>
		/// Metod calculates the colour of the car and stores it in the AdvertDetails object.
		/// </summary>
		/// <param name="_advertDetails">AdvertDetails object containing the information about the advert image</param>
		public void pump(ref AdvertDetails _advertDetails)
		{
			//check if the car exists
			if (!_advertDetails.CarFound)
			{
				_advertDetails.Error = "Cannot calculate coverage if car not found.";
				ImageBlock[][] imageBlocks = getImageBlocks(_advertDetails.Image.ToBitmap(), new Rectangle(0, 0, _advertDetails.Image.Width, _advertDetails.Image.Height));
				imageBlocks = calculateImageBlocksColourIndex(_advertDetails.Image.ToBitmap(), imageBlocks);
				int dominantBucketIndex = getCarColourIndex(imageBlocks);
				_advertDetails.Colour1 = colourBuckets[dominantBucketIndex].ColourName;
				return;
			}
			else
			{
				ImageBlock[][] imageBlocks = getImageBlocks(_advertDetails.Image.ToBitmap(), _advertDetails.Rect);
				imageBlocks = calculateImageBlocksColourIndex(_advertDetails.Image.ToBitmap(), imageBlocks);
				int dominantBucketIndex = getCarColourIndex(imageBlocks);
				_advertDetails.Colour1 = colourBuckets[dominantBucketIndex].ColourName;
			}
		}

		public ImageBlock[][] getImageBlocks(Bitmap _image, Rectangle _rect)
		{
			//number of cols and rows to divide the image into
			int num_cols = 15;
			int num_rows = 15;

			//get the middle of cols and rows
			int midCols = num_cols / 2;
			int midRows = num_rows / 2;

			//calculate the sum of x and y, 2 is to avoid division of 0
			int midSum = midCols + midRows + 2;

			//column width and height
			double colWidth = (double)_rect.Width / num_cols;
			double rowHeight = (double)_rect.Height / num_rows;

			//maximum number of white pixels allowed int the block
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
			Bitmap cropped = _image.Clone(_rect, _image.PixelFormat);
			cropped.Save("cropped.jpg");
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

			int value;
			double rating;

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

							value = getSum(midCols, midRows, blockXCoord, blockYCoord);
							rating = value / midSum;
							blocks[blockXCoord][blockYCoord] = new ImageBlock(current_x, current_y, current_width, current_height, false, rating);
							goto Next;
						}
					}
				}
<<<<<<< HEAD
=======

>>>>>>> 702ac1f6ccce09cf1aaa9578baafff9ff5ccb6f2
				value = getSum(midCols, midRows, blockXCoord, blockYCoord);
				rating = value / midSum;
				//block with pixel count less than the treshold
				blocks[blockXCoord][blockYCoord] = new ImageBlock(current_x, current_y, current_width, current_height, true, rating);

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
		public int getSum(int _midX, int _midyY, int _blockX, int _blockY)
		{
			if (_blockX <= _midX && _blockY <= _midyY)
			{
				return _blockX + _blockY;
			}
			else if (_blockX <= _midX)
			{
				return _blockX + (_blockY - (_blockY - _midyY));
			}
			else if (_blockY <= _midyY)
			{
				return _blockY + (_blockX - (_blockX - _midX));
			}
			return 0;
		}

		public ImageBlock[][] calculateImageBlocksColourIndex(Bitmap _image, ImageBlock[][] _blocks)
		{
			int count = 0;
			for (int j = 0; j < _blocks.Length; j++)
			{
				for (int i = 0; i < _blocks[0].Length; i++)
				{
					if (_blocks[i][j].Valid)
					{
						_blocks[i][j].ColourIndex = getBlockColourIndex(_image, _blocks[i][j]);
						Console.WriteLine("X: " + i + " Y: " + j + " " + colourBuckets[_blocks[i][j].ColourIndex].ColourName);
						count++;
					}
				}
			}
			Console.WriteLine("count " + count);
			return _blocks;
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
			//counter to determine the most dominant colour
			int[] colourCounter = new int[colourBuckets.Count];
			for (int x = _block.X_coord; x < _block.X_coord + _block.Width - 1; x++)
			{
				for (int y = _block.Y_coord; y < _block.Y_coord + _block.Height - 1; y++)
				{
					Color clr = _image.GetPixel(x, y);

					//Console.WriteLine("HSV: " + p_hue%360 +","+p_saturation*100 +","+p_value*100);
					double min = double.MaxValue;
					int ind = 0;
					for (int i = 0; i < colourBuckets.Count; i++)
					{
						//double bin_hue = colourBuckets[i].H;
						//double bin_saturation = colourBuckets[i].S;
						//double bin_value = colourBuckets[i].V;
						double distance = getDistance(clr, colourBuckets[i]);
						if (distance < min)
						{
							ind = i;
							min = distance;
						}
					}
					colourCounter[ind] = colourCounter[ind] + 1;
				}
			}

			//find the most occuring colour in the block
			int index = getDominantIndex(colourCounter);
			return index;
		}

		public int getDistance(Color currentPixel, ColourBucket bucket)
		{
			double p_hue = 0.0, p_saturation = 0.0, p_value = 0.0;
			convertRGBtoHSV(currentPixel, out p_hue, out p_saturation, out p_value);
			double hueDifference = Math.Min(Math.Abs((int)bucket.h - (int)p_hue), 360 - Math.Abs(bucket.h - (int)p_hue));
			double satDifference = Math.Abs(((int)bucket.s - (int)p_saturation));
			double valDifference = Math.Abs((int)bucket.v - (int)p_value);
			int distance = (int)Math.Sqrt(Math.Pow(hueDifference, 2.0) + Math.Pow(satDifference, 2.0) + Math.Pow(valDifference, 2.0));
			return distance;
		}

		public int getCarColourIndex(ImageBlock[][] _blocks)
		{
			double[] colourCounter = new double[colourBuckets.Count];

			for (int j = 0; j < _blocks[0].Length; j++)
			{
				for (int i = 0; i < _blocks.Length; i++)
				{
					if (_blocks[i][j].Valid)
						colourCounter[_blocks[i][j].ColourIndex] += 1 * _blocks[i][j].Rating;
				}
			}
			int dominantIndex = getDominantIndex(colourCounter);

			return dominantIndex;
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
			int max = Math.Max(colour.R, Math.Max(colour.G, colour.B));
			int min = Math.Min(colour.R, Math.Min(colour.G, colour.B));
			hue = colour.GetHue();
			saturation = (max == 0) ? 0 : 1d - (1d * min / max);
			saturation *= 100;
			value = (max / 255d) * 100;
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
				graphics.DrawLine(redPen, _x + _width - xReduction, _y + yReduction, _x + xReduction, _y + _height - yReduction);

				graphics.Save();
				return _image;
			}
		}

		public int getDominantIndex(double[] array)
		{
			int index = 0;
			for (int i = 1; i < array.Length; i++)
			{
				if (array[i] > array[index])
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
			for (int j = 0; j < _blocks[0].Length; j++)
			{
				for (int i = 0; i < _blocks.Length; i++)
				{
					if (_blocks[i][j].Valid)
						count++;
				}
			}
			return count;
		}
	}
}
