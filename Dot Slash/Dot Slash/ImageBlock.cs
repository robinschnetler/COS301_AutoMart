using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dot_Slash
{
    class ImageBlock
    {
        private int x_coord;
        public int X_coord
        {
            get
            {
                return x_coord;
            }
        }
        private int y_coord;
        public int Y_coord
        {
            get
            {
                return y_coord;
            }
        }
        private int width;
        public int Width
        {
            get
            {
                return width;
            }
        }
        private int height;
        public int Height
        {
            get
            {
                return height;
            }
        }
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

        public ImageBlock(int _xCoord, int _yCoord, int _height, int _width)
        {
            x_coord = _xCoord;
            y_coord = _yCoord;
            height = _height;
            width = _width;
        }
    }
}
