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
using System.Runtime.Serialization.Json;

namespace Dot_Slash
{
	public class AdvertDetails
	{
		//public List<String> ExceptionList = new List<string>();
		public string exception;
		public bool blurry;
		public bool Blurry
		{
			get
			{
				return blurry;
			}
			set
			{
				blurry = value;
			}
		}

		public bool error;
		public bool Error
		{
			get
			{
				return error;
			}
			set
			{
				error = value;
			}
		}

		private bool carFound;
		public bool CarFound
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

		public int image_ID;
		public int Image_ID
		{
			get
			{
				return image_ID;
			}
			set
			{
				image_ID = value;
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
		string colour1;
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

		string colour2;
		public string Colour2
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
		string colour3;
		public string Colour3
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

		string hex1;
		public string Hex1
		{
			get
			{
				return hex1;
			}
			set
			{
				hex1 = value;
			}
		}

		string hex2;
		public string Hex2
		{
			get
			{
				return hex2;
			}
			set
			{
				hex2 = value;
			}
		}

		string hex3;
		public string Hex3
		{
			get
			{
				return hex3;
			}
			set
			{
				hex3 = value;
			}
		}

		private string view;
		public string View
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
			blurry = false;
			coverageValue = 0.0f;
			colour1 = null;
			colour2 = null;
			colour3 = null;
			Error = false;
			view = "Uknown";
			rect = new Rectangle();

		}

		public String retrieveDetails()
		{
			String output = "";
			output += "&car_found = " + carFound + "&view=" + view + "&blur_Value=" + blurValue + "&Coverage_value=" + coverageValue + "&colour1=" + colour1 +"&colour2=" + colour2 + "&colour3=" + colour3 + "&error=" +exception;
			return output;
		}

		public string JsonSerializer<T> (T t)
		{
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
			MemoryStream ms = new MemoryStream();
			ser.WriteObject(ms, t);
			string jsonString = Encoding.UTF8.GetString(ms.ToArray());
			ms.Close();
			return jsonString;
		}

		public byte[] getImage()
		{
			return image.ToJpegData();
		}
	}
}
