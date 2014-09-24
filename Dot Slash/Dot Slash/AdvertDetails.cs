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
using System.Data.Linq;

namespace Dot_Slash
{
	public class AdvertDetails
	{
        private Boolean carFound;
        public Boolean CarFound
        {
            get 
            {
                return carFound; 
            }
            set 
            {
                carFound = value; 
            }
        }
        private Image<Bgr, Int32> image;
        public Image<Bgr, Int32> Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }
        float blurValue;
        public float BlurValue
        {
            get
            {
                return blurValue;
            }
            set
            {
                blurValue = value;
            }
        }
        float coverageValue;
        public float CoverageValue
        {
            get
            {
                return coverageValue;
            }
            set
            {
                coverageValue = value;
            }
        }
        String colour1;
        public String Colour1
        {
            get
            {
                return colour1;
            }
            set
            {
                colour1 = value;
            }
        }

        String colour2;
        public String Colour2
        {
            get
            {
                return colour2;
            }
            set
            {
                colour2 = value;
            }
        }
		String colour3;
        public String Colour3
        {
            get
            {
                return colour3;
            }
            set
            {
                colour3 = value;
            }
        }
        private String view;
        public String View
        {
            get
            {
                return view;
            }
            set
            {
                view = value;
            }
        }

        private Rectangle rect;
        public Rectangle Rect
        {
            get
            {
                return rect;
            }
            set
            {
                rect = value;
            }
        }

		public AdvertDetails(Binary im)
		{
			carFound = false;
			MemoryStream ms = new MemoryStream(im.ToArray());
			image = new Image<Bgr,int>(new Bitmap(ms));
			blurValue = 0.0f;
			coverageValue = 0.0f;
			colour1 = null;
			colour2 = null;
			colour3 = null;
            view = "Uknown";
            rect = new Rectangle();
		}

		public String retreiveDetails()
		{
			String output = "&";
			output += carFound + "&" + blurValue + "&" + coverageValue + "&" + colour1 +"&" + colour2 + "&" + colour3;
			return output;
		}
	}
}
