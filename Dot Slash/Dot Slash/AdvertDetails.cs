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
		public String exception;
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

		
	}
}
