using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dot_Slash
{
    /// <summary>
    /// Stores a set of four integers that represent the location and the size of the image block as well as two strings
    /// that represent the name and the hexadecimal value of the most dominant colour in the block.
    /// </summary>
    class ImageBlock
    {
        /// <summary>
        /// Returns the upper-left x-coordinate of the image block.
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

        /// <summary>
        /// Returns the name of the most dominant colour in the image block.
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

        /// <summary>
        /// Initializes a new instance of the ImageBlock class with specified location and size.
        /// </summary>
        /// <param name="_xCoord"></param>The x-coordinate of the upper-left corner of the block.
        /// <param name="_yCoord"></param>The y-coordinate of the upper-left corner of the block.
        /// <param name="_height"></param>The height of the block.
        /// <param name="_width"></param>The width of the block.
        public ImageBlock(int _xCoord, int _yCoord, int _height, int _width)
        {
            x_coord = _xCoord;
            y_coord = _yCoord;
            height = _height;
            width = _width;
        }
    }
}
