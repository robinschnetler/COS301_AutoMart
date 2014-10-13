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

        private Image<Bgr, Int32> image;
        /// <summary>
        /// Returns the advert image.
        /// </summary>
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

        private float blurValue;
        /// <summary>
        /// Returns the advert image blur value.
        /// </summary>
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

		private float coverageValue;
        /// <summary>
        /// Returns the coverage value that represents the cars coverage in the advert image.
        /// </summary>
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

		private string colour;
        /// <summary>
        /// Returns the colour of the car in the advert image.
        /// </summary>
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

		private string hex;
        /// <summary>
        /// Returns the hexadecimal value of the cars colour. 
        /// </summary>
        public string Hex
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

		private int rating;
        /// <summary>
        /// Returns the rating of the advert image.
        /// </summary>
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

        private string error;
        /// <summary>
        /// Returns any errors encountered.
        /// </summary>
        public string Error
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
                + coverageValue + "&colour=" + colour + "&hex=" + hex + "&view=" + view + "&rating=" 
                + rating + "&error=" + error;
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
	}
}
