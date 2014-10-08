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
	public class CarDetector : Filter
	{
		const int numNeighbours = 1;
		const double scaleFac = 1.05;
		Size side_minSize = new Size(140, 120), fb_minSize = new Size(150, 125);
		Size maxSize = new Size(480, 320); //width height
    		String frontClassifier;
		String backClassifier;
		String sideClassifier;

	public CarDetector(String front, String back, String side)
	{
		frontClassifier = front;
		backClassifier = back;
		sideClassifier = side;
	}
        //idea combine the views to get angled view
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
			rect = rectangleList.Last();
		    }

		    classifier = new CascadeClassifier(backClassifier);
		    rectangleList = classifier.DetectMultiScale(image, scaleFac, numNeighbours, fb_minSize, maxSize);
		    if (rectangleList.Length > count)
		    {
			    count = rectangleList.Length;
			    view = "Back";
			    rect = rectangleList.Last();
		    }

		    classifier = new CascadeClassifier(sideClassifier);
		    rectangleList = classifier.DetectMultiScale(image, scaleFac, numNeighbours, side_minSize, maxSize);
		    if (rectangleList.Length > count)
		    {
			    count = rectangleList.Length; 
			    view = "Side";
			    rect = rectangleList.Last();
		    }

		    if (count > 0)
		    {
			_advertDetails.Rect = rect;
			_advertDetails.CarFound = true;
			_advertDetails.View = view;
		    }
		    else
		    { 
			_advertDetails.CarFound = false;
			_advertDetails.Error = true;
			throw new Exception(track);
		    }
		}
	}
}
