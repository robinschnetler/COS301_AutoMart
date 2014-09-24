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
	class CarDetector : Filter
	{
		const int numNeighbours = 3;
        const double scaleFac = 1.05;
		Size side_minSize = new Size(162, 108), fb_minSize = new Size(150, 125);
        Size maxSize = new Size(480, 320); //width height
    	const String frontClassifier = "classifiers/frontClassifier.xml";
        const String backClassifier = "classifiers/backClassifier.xml";
        const String sideClassifier = "classifiers/sideClassifier.xml";

        //idea comnine the views to get angled view
        public virtual void pump(ref AdvertDetails _advertDetails) 
        {
            int count = 0;
            int numClassified = 0;
            Rectangle rect = new Rectangle();
            String view = "Uknown";

            Image<Gray, Byte> image = _advertDetails.Image.Convert<Gray, byte>();
            
            CascadeClassifier classifier = new CascadeClassifier(frontClassifier);
            Rectangle[] rectangleList = classifier.DetectMultiScale(image, scaleFac, numNeighbours, fb_minSize, maxSize);
            if(rectangleList.Length > count)
            {
                count = rectangleList.Length;
                view = "Front View";
                rect = rectangleList.Last();
                numClassified += 1;
            }

            classifier = new CascadeClassifier(backClassifier);
            rectangleList = classifier.DetectMultiScale(image, scaleFac, numNeighbours, fb_minSize, maxSize);
            if (rectangleList.Length > count)
            {
                if(count < rectangleList.Length)
                {
                    count = rectangleList.Length;
                    view = "Back View";
                    rect = rectangleList.Last();
                    numClassified += 1;
                }
            }

            classifier = new CascadeClassifier(sideClassifier);
            rectangleList = classifier.DetectMultiScale(image, scaleFac, numNeighbours, side_minSize, maxSize);
            if (rectangleList.Length > count)
            {
                if (count < rectangleList.Length)
                {
                    count = rectangleList.Length;
                    view = "Side View";
                    rect = rectangleList.Last();
                    numClassified += 1;
                }
            }

            if (count > 0)
            {
                _advertDetails.Rect = rect;
                _advertDetails.CarFound = true;
                _advertDetails.View = view;
            }
            else
                _advertDetails.CarFound = false;
        }
	}
}
