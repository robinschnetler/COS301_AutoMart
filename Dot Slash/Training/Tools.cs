using System;
using System.Collections;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*using System.Windows.Forms.DataVisualization;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.Util;

/*namespace Dot_Slash
{
	public class Tools
	{
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
			String path = "images/";
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
			pc.Dispose();
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


		static public void UpdateProgress(int complete, int maxVal, int barSize, char progressCharacter)
		{
			Console.CursorVisible = false;
			int left = Console.CursorLeft;
			decimal perc = (decimal)complete / (decimal)maxVal;
			int chars = (int)Math.Floor(perc / ((decimal)1 / (decimal)barSize));
			string p1 = String.Empty, p2 = String.Empty;

			for (int i = 0; i < chars; i++) p1 += progressCharacter;
			for (int i = 0; i < barSize - chars; i++) p2 += progressCharacter;

			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(p1);
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.Write(p2);

			Console.ResetColor();
			Console.Write(" {0}%", (perc * 100).ToString("N2"));
			Console.CursorLeft = left;
		}

		public Image<Bgr, Int32> subDivide(Image<Bgr, Int32> image, int divisions)
		{
			Bgr pen = new Bgr(255,255,255);
			LineSegment2D[] lines = new LineSegment2D[(divisions-1) * 2];
			int widSpace = (image.Width % divisions)-1;
			int heiSpace = (image.Height % divisions)-1;
			int count = 0;
			for (int i = 0; i < image.Width; i += widSpace)
			{
				lines[count++] = new LineSegment2D(new Point(i, 0), new Point(i, image.Height-1));
			}
			for (int i = 0; i < image.Height; i += heiSpace)
			{
				lines[count++] = new LineSegment2D(new Point(0, i), new Point(image.Width-1, i));
			}
			for (int i = 0; i < lines.Length; i++)
			{
				image.Draw(lines[i], pen, 1);
			}
			return image;
		}

		public void changeExtension(String dir, String from, String to)
		{
			String[] filePaths = System.IO.Directory.GetFiles(dir + "/", "*." + from);
			Console.WriteLine("Converting files from ." + from + " to ." + to);
			for (int i = 0; i < filePaths.Length; i++)
			{
				String filePath = filePaths[i];
				String filename = System.IO.Path.ChangeExtension(filePath, "." + to);
				System.IO.File.Move(filePath, filename);
				Tools.UpdateProgress(i + 1, filePaths.Length, 50, '=');
			}

			Console.WriteLine();
			Console.WriteLine();
		}
	}
}*/
