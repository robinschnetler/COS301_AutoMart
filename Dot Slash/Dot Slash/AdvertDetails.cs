using System;
using System.Drawing;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Data.Linq;
using System.Runtime.Serialization.Json;

namespace Dot_Slash
{
	/// <summary>
	/// Class used to store advert image details.
	/// </summary>
	public class AdvertDetails
	{
		private int image_ID;
		/// <summary>
		/// Returns the advert image ID.
		/// </summary>
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


		private bool carFound;
		/// <summary>
		/// Returns car existance status in the advert image.
		/// </summary>
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

		private Rectangle rect;
		/// <summary>
		/// Returns the rectangle representing the location of the car in the advert image.
		/// </summary>
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

		private double blurValue;
		/// <summary>
		/// Returns the advert image blur value.
		/// </summary>
		public double BlurValue
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

		public Boolean blurry;
		public Boolean Blurry
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

		private double coverageValue;
		/// <summary>
		/// Returns the coverage value that represents the cars coverage in the advert image.
		/// </summary>
		public double CoverageValue
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

		private string colour1;
		/// <summary>
		/// Returns the colour of the car in the advert image.
		/// </summary>
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

		private string colour2;
		/// <summary>
		/// Returns the colour of the car in the advert image.
		/// </summary>
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

		private string colour3;
		/// <summary>
		/// Returns the colour of the car in the advert image.
		/// </summary>
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
		/// <summary>
		/// Returns the view of the car in the image.
		/// </summary>
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

		public int blurRating;
		/// <summary>
		/// Returns the blur rating value.
		/// </summary>
		public int BlurRating
		{
			get
			{
				return blurRating;
			}
			set
			{
				blurRating = value;
			}
		}

		private int coverageRating;
		/// <summary>
		/// Returns the coverage rating value.
		/// </summary>
		public int CoverageRating
		{
			get
			{
				return coverageRating;
			}
			set
			{
				coverageRating = value;
			}
		}

		/// <summary>
		/// Returns the rating of the advert image.
		/// </summary>

		public int carRating;
		public int CarRating
		{
			get
			{
				return carRating;
			}
			set
			{
				carRating = value;
			}
		}
		private String error;
        /// <summary>
        /// Returns any errors encountered.
        /// </summary>
        public String Error
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
        /// Initialises a new instance of the AdevertDetails class with specified image in binary format.
        /// </summary>
        /// <param name="_binary"></param>The binary representation of a image.
		public AdvertDetails(Binary _binary)
		{
			MemoryStream ms = new MemoryStream(_binary.ToArray());
			image = new Image<Bgr, int>(new Bitmap(ms));
			carFound = false;
			image = new Image<Bgr, int>(new Bitmap(ms));
			blurValue = 0.0f;
			blurry = false;
			coverageValue = 0.0f;
			colour1 = null;
			colour2 = null;
			colour3 = null;
			hex1 = null;
			hex2 = null;
			hex3 = null;
			view = "";
			rect = new Rectangle();
			carRating = 0;
			coverageRating = 0;
			blurRating = 0;
		}
		/// <summary>
		/// Returns string containg all AdvertDetail object information.
		/// </summary>
		/// <returns>String contains all the AdvertDetails information separated by '&' symbol.</returns>
		/// <remarks>
		public String retrieveDetails()
		{
			String output = "";
			output += "&car_found = " + carFound + "&blur_value=" + blurValue + "&coverage_value="
			+ coverageValue + "&colour1=" + colour1 + "&colour2=" + colour2 + "&colour3=" + colour3 +
			"&hex3=" + hex1 + "&hex3=" + hex2 + "&hex3=" + hex3 + "&view=" + view + "&error=" + error;
			return output;
		}

		/// <summary>
		/// Returns the json string with all the AdvertDetails information.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <returns>Returns a json string contains all the AdvertDetails information.</returns>
		public string JsonSerializer<T> (T t)
		{
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
			string jsonString;
			using (MemoryStream ms = new MemoryStream())
			{
				ser.WriteObject(ms, t);
				jsonString = Encoding.UTF8.GetString(ms.ToArray());
			}
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
