using System;
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

namespace Dot_Slash
{
	public class Tools
	{
		public void createDat()
		{
			String imgPath = "images/";
			Boolean greyExists = Directory.Exists("Greyscale/");
			if(greyExists)
				imgPath = "Greyscale";
			String[] pictures = Tools.getImages(imgPath, Globals.extensions);
			Bitmap sample = new Bitmap(pictures[0]);
			int width = sample.Width;
			int height = sample.Height;
			StreamWriter writer = new StreamWriter("samples.dat",false);
			ProgressBar progress = new ProgressBar();
			for (int i = 0; i < pictures.Length; i++)
			{
				writer.WriteLine(pictures[i] + " 1 " + "0 0 " + width + " "  + height);
				progress.UpdateProgress(i + 1, pictures.Length, 50, '=');
			}
			writer.Close();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("generated samples.dat file for "+pictures.Length + " images.");
		}

		static public String[] getImages(String imagePath, String[] extension)
		{
			ArrayList images = new ArrayList();
			for (int i = 0; i < extension.Length; i++)
			{
				String[] pictures = Directory.GetFiles(imagePath, "*." + extension[i], SearchOption.TopDirectoryOnly);
				for (int j = 0; j < pictures.Length; j++)
					images.Add(pictures[j]);
			}
			return (string[])images.ToArray(typeof(string));
		}

		/// <summary>
		/// allows for easy cropping of photos. Will be used to crop photos for positive samples to cascading classifier
		/// </summary>
		public void photoCropper()
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			Console.WriteLine("please select folder with all images of car");
			Console.WriteLine("Note: sub directories will not be parsed");
			String path = "";
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				path = dialog.SelectedPath;
			}
			String[] pictures = Tools.getImages(path, Globals.extensions);
			PictureCropper pc = new PictureCropper(pictures, 3, 2);
			pc.Activate();
			pc.ShowDialog();
		}

		/// <summary>
		/// This tool allows for user to filter through images that are valid and invalid from a directory with mixed images
		/// </summary>
		public void photoChooser()
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			Console.WriteLine("please select folder with all images");
			Console.WriteLine("Note: sub directories will not be parsed");
			String path = "";
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				path = dialog.SelectedPath;
			}
			//create "classified" file that stores image paths of images that are already classified. These paths will be used to delete
			//already classified images in the next run
			if (!File.Exists("classified"))
			{
				File.Create("classified");
			}
			else
			{
				cleanOld();
			}
			String[] pictures = Tools.getImages(path, Globals.extensions);
			photoChooser pc = new photoChooser(pictures);
			pc.Activate();
			pc.ShowDialog();
			Console.WriteLine(pc.current + " images classified");
		}

		/// <summary>
		/// Deletes files that were already classified during the image filtering by hand. The file paths to be deleted are listed in the "classified" file in the directory
		/// </summary>
		private void cleanOld()
		{
			StreamReader read = new StreamReader("classified");
			while (!read.EndOfStream)
			{
				File.Delete(read.ReadLine());
			}
			read.Close();
		}

		/// <summary>
		/// We are getting in an image and returning the Summed Area Table (A 2 dimensional matrix of integers)
		/// See http://computersciencesource.wordpress.com/2010/09/03/computer-vision-the-integral-image/
		/// </summary>
		/// <param name="filename">directory to image file that the integral image will be based upon</param>
		/// <returns>Bitmap representation of integral image</returns>
		public Bitmap generateIntegralImage(String filename)
		{
			Image<Gray, Double> integral = new Image<Gray, Double>(filename);
			integral = integral.Integral();
			return integral.ToBitmap();
		}
	}
	/// <summary>
	/// Each image processing task must be  encapsulated into a class that inherits from interface Strategy. Based on the mathod called
	/// in ImageProcessor, the correct strategy object is created and executed.
	/// </summary>
	class ImageProcessor
	{
		String imagePath;
		Strategy strategy;

		public ImageProcessor()
		{
			imagePath = "images/";
		}

		public void makeGreyscale()
		{
			strategy = new Greyscaler(imagePath);
			strategy.execute();
			strategy = null;
		}

		public void detectEdges()
		{
			strategy = new EdgeDetector(imagePath);
			strategy.execute();
			strategy = null;
		}

		public void applyGaussian()
		{
			strategy = new GaussianFilter(imagePath);
			strategy.execute();
			strategy = null;
		}

		public void resize()
		{
			strategy = new ImageResizer(imagePath);
			strategy.execute();
			strategy = null;
		}
	} 
}
