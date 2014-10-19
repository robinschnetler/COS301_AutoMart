using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dot_Slash
{
	/// <summary>
	/// This class is used for colour detection purposes. It holds co-ordinates of pixels within the image as well as the width and height of the block and colour detection is run on these ImageBlocks.
	/// </summary>
	public class ImageBlock
	{
		/// <summary>
		/// The x co-ordinate of the left upper pixel of the ImageBlock with a get method
		/// </summary>
		private int x_coord;
		public int X_coord
		{
			get
			{
				return x_coord;
			}
		}

		/// <summary>
		/// Returns the upper-left y-coordinate of the image block.
		/// </summary>
		private int y_coord;
		public int Y_coord
		{
			get
			{
				return y_coord;
			}
		}

		/// <summary>
		/// Returns the width of the image block.
		/// </summary>
		private int width;
		public int Width
		{
			get
			{
				return width;
			}
		}

		/// <summary>
		/// Returns the height of the image block.
		/// </summary>
		private int height;
		public int Height
		{
			get
			{
				return height;
			}
		}

		private int colourIndex;
		
		public int ColourIndex
		{
			get
			{
				return colourIndex;
			}
			set
			{
				colourIndex = value;
			}
		}

		/// <summary>
		/// The most dominant colour in this ImageBlock as well as the relevant get and set methods
		/// </summary>
		private String colour;
		public String Colour
		{
			get
			{
				return colour;
			}
			set
			{
				colour = value;
			}
		}

		/// <summary>
		/// Returns the hexadecimal value of the most dominant colour in the image block.
		/// </summary>
		private String hex;
		public String Hex
		{
			get
			{
				return hex;
			}
			set
			{
				hex = value;
			}
		}

		private bool valid;

		public bool Valid
		{
			get
			{
				return valid;
			}
		}

		private double rating;
		
		public double Rating
		{
			get
			{
				return rating;
			}
			set
			{
				rating = value;
			}
		}

		/// <summary>
		/// The constructor that creates the ImageBlock object
		/// </summary>
		/// <param name="_xCoord">The x co-ordinate of the upper right pixel of the ImageBlock</param>
		/// <param name="_yCoord">The y co-ordinate of the upper right pixel of the ImageBlock</param>
		/// <param name="_height">The height of the ImageBlock</param>
		/// <param name="_width">The width of the ImageBlock</param>
		public ImageBlock(int _xCoord, int _yCoord, int _height, int _width, bool _valid, double b)
		{
			x_coord = _xCoord;
			y_coord = _yCoord;
			height = _height;
			width = _width;
			valid = _valid;
			rating = b;
		}
	}
}
