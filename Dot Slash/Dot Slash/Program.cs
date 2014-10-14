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
	//main entry point into the program
	class Program
	{
		[STAThread] //allows for main to open dialogs(something to do with threads)
		static void Main(string[] args)
		{
			//Byte[] image = File.ReadAllBytes("pipe/image.jpg");
			//AdvertDetails advertDetails = new AdvertDetails(image);
			//Filter[] filters = { new CarDetector("classifiers/frontClassifier.xml", "classifiers/backClassifier.xml", "classifiers/sideClassifier.xml"), new BlurDetector(0.3), new ColourDetector(), new CoverageDetector() };
			//List<Filter> filterList = new List<Filter>(filters);
			//Pipe pipe = new Pipe(filterList, advertDetails);
			//advertDetails = pipe.flow();
			//Console.WriteLine(advertDetails.retrieveDetails());
			//String input = Console.ReadLine();
			BlurDetector bd = new BlurDetector(0.5);
			Image<Gray, Byte> image = new Image<Gray,byte>("EdgeTest/image.jpg");
			ArrayList a = bd.getEdgedPixels(image);
			float f = bd.calculateBlur(image.ToBitmap(), a);
			Console.WriteLine("value: " + f);
			Console.ReadLine(); 
		}
	}
}
