using System;
using System.Linq;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Data.Linq;

namespace Dot_Slash
{
	class MakeDetector : Filter
	{
		String VWclassifier = "classifers/makes/vw.xml";
		
		public void pump(ref AdvertDetails _advertDetails)
		{
			CascadeClassifier cc = new CascadeClassifier(VWclassifier);
			bool carExitst = _advertDetails.CarFound?true:false;
			Image<Gray, byte> image;
			if(carExitst)
				image = _advertDetails.Image.GetSubRect(_advertDetails.Rect).Convert<Gray, byte>();
			else
				image = _advertDetails.Image.Convert<Gray, byte>();
			Rectangle[] logosFound = cc.DetectMultiScale(image, 1.05, 1, new Size(20,20), new Size(40,40));		
		}
	}
}
