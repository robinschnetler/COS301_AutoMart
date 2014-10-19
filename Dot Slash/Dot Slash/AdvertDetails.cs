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
	/// <summary>
	/// This object stores all details of the advert that are picked up from the various filters/classifiers
	/// </summary>
	public class AdvertDetails
	{
		//public List<String> ExceptionList = new List<string>();
		/// <summary>
		/// A string that stores any errors picked up in the image (such as if the image was too blurry or a car wasn't detected in the image)
		/// </summary>
		public string exception;

		/// <summary>
		/// A boolean that stores whether the image was detected as a blurry image or not with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// A boolean that stores whether there was an error with the image with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// A boolean that stores whether a car was detected in the image or not as well as the relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// The ID of the image that is being processed with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// The image itself (stored as an OpenCV Image object)
		/// </summary>
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
		
		/// <summary>
		/// A float which stores how much blur was detected in the image with relevant get and set methods
		/// </summary>
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
		
		/// <summary>
		/// A float that stores how much of the image contains the detected car with relevant get and set methods
		/// </summary>
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
		
		/// <summary>
		/// A string storing the name of the most dominant colour in the image with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// A string storing the name of the second most dominant colour in the image with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// A string storing the name of the third most dominant colour in the image with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// A string storing the hex value of the most dominant colour in the image with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// A string storing the hex value of the second most dominant colour in the image with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// A string storing the hex value of the third most dominant colour in the image with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// A string that stores which classifier found a car in the image (Front, Side or Back) with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// A rectangle which stores the co-ordinates of the detected car in the image with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// A rating out of five which stores the overall quality/rating of the image based on whether a car was detected, the blur value of the image and the coverage of the car in the image
		/// </summary>
		private int rating;
		public int Rating
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
		/// The constructor which creates an advertDetails object
		/// </summary>
		/// <param name="im">A binary stream of the image uploaded by the user</param>
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
			hex1 = null;
			hex2 = null;
			hex3 = null;
			Error = false;
			view = "";
			rect = new Rectangle();
			rating = 0;
		}

		/// <summary>
		/// A function which returns all the details of the advert as a &-separated string
		/// </summary>
		/// <returns>The string detailing all the image details</returns>
		public String retrieveDetails()
		{
			String output = "";
			output += "&car_found = " + carFound + "&view=" + view + "&blur_Value=" + blurValue + "&coverage_value=" + coverageValue + "&colour1=" + colour1 + "&error=" +exception;
			return output;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <returns></returns>
		public string JsonSerializer<T> (T t)
		{
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
			MemoryStream ms = new MemoryStream();
			ser.WriteObject(ms, t);
			string jsonString = Encoding.UTF8.GetString(ms.ToArray());
			ms.Close();
			return jsonString;
		}

		/// <summary>
		/// A function that converts the image to a Bitmap object and returns it
		/// </summary>
		/// <returns>The image as a Bitmap object</returns>
		public Bitmap getImage()
		{
			return image.ToBitmap();
		}
	}
}
