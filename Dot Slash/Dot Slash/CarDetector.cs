﻿using System;
using System.Linq;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Data.Linq;

namespace Dot_Slash
{
    /// <summary>
    /// 
    /// </summary>
    public class CarDetector : Filter
    {
        private String frontClassifier;
        private String backClassifier;
        private String sideClassifier;
	    private const int numNeighbours = 1;
	    private const double scaleFac = 1.05;
	    private Size side_minSize = new Size(140, 120), fb_minSize = new Size(150, 125);
	    private Size maxSize = new Size(480, 320); //width height


        /// <summary>
        /// 
        /// </summary>
        /// <param name="front"></param>Filename of the front classifier.
        /// <param name="back"></param>Filename of the back classifier.
        /// <param name="side"></param>Filename of the side classifier.
        public CarDetector(String _front, String _back, String _side)
        {
		    frontClassifier = _front;
		    backClassifier = _back;
		    sideClassifier = _side;
	    }

        //idea combine the views to get angled view
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_advertDetails"></param>AdvertDetails object containing the information about the advert image.
        public virtual void pump(ref AdvertDetails _advertDetails) 
        {
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
                _advertDetails.Error = "No car found.";
                throw new Exception("No car found.");
		    }
	    }
	}
}
