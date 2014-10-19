using System;
using System.Linq;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Data.Linq;

namespace Dot_Slash
{
	/// <summary>
	/// The CarDetector implements the Filter interface and detects whether there is a car or not in the image
	/// </summary>
	public class CarDetector : Filter
	{
		//Parameters for the OpenCV framework
		const int numNeighbours = 1;
		const double scaleFac = 1.05;
		Size side_minSize = new Size(140, 120), fb_minSize = new Size(150, 125);
		Size maxSize = new Size(480, 320); //width height

		//Strings representing the filenames of the .xml files for the respective classifiers
		String frontClassifier;
		String backClassifier;
		String sideClassifier;

		/// <summary>
		/// The constructor which creates a CarDetector object
		/// </summary>
		/// <param name="front">A string representing the file path of the .xml file for the front classifier</param>
		/// <param name="back">A string representing the file path of the .xml file for the back classifier</param>
		/// <param name="side">A string representing the file path of the .xml file for the side classifier</param>
		public CarDetector(String front, String back, String side)
		{
			frontClassifier = front;
			backClassifier = back;
			sideClassifier = side;
		}

		/// <summary>
		/// Implements the car detector filter and adds the details to the advertDetails object
		/// </summary>
		/// <param name="_advertDetails">The advertDetails object where information about the advert is stored</param>
		public virtual void pump(ref AdvertDetails _advertDetails) 
		{
			string track = "";
			int count = 0;
			Rectangle rect = new Rectangle();
			String view = "Unknown";

			Image<Gray, Byte> image = _advertDetails.Image.Convert<Gray, byte>();
            
			CascadeClassifier classifier = new CascadeClassifier(frontClassifier);
			Rectangle[] rectangleList = classifier.DetectMultiScale(image, scaleFac, numNeighbours, fb_minSize, maxSize);
			if(rectangleList.Length > count)
			{
				count = rectangleList.Length;
				view = "Front";
			}

			classifier = new CascadeClassifier(backClassifier);
			rectangleList = classifier.DetectMultiScale(image, scaleFac, numNeighbours, fb_minSize, maxSize);
			if (rectangleList.Length > count)
			{
				count = rectangleList.Length;
				view = "Back";
			}

			classifier = new CascadeClassifier(sideClassifier);
			rectangleList = classifier.DetectMultiScale(image, scaleFac, numNeighbours, side_minSize, maxSize);
			if (rectangleList.Length > count)
			{
				count = rectangleList.Length; 
				view = "Side";
			}

			rect = getLargest(rectangleList);
			if (count > 0)
			{
				_advertDetails.Rect = rect;
				_advertDetails.CarFound = true;
				_advertDetails.View = view;
				_advertDetails.CarRating = 1;
			}
			else
			{
				_advertDetails.CarFound = false;
				_advertDetails.Error = "No car found.";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		private Rectangle getLargest(Rectangle[] list)
		{
			if (list.Length == 0)
				return new Rectangle();
			Rectangle largest = list[0];
			int currentArea = largest.Width * largest.Height;
			for (int i = 0; i < list.Length; i++)
			{
				int area = list[i].Width * list[i].Height;
				if (area > currentArea)
				{
					currentArea = area;
					largest = list[i];
				}
			}
			return largest;
		}
	}
}
