using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.Util;

namespace Dot_Slash
{
    class Training
    {
        private String positivesLocation;
        private String negativesLocation;
        private int width;
        private int height;
        private int numStages;
        private int numSamples;
        private String saveLocation;
        private String positiveDat;
        private String negativeDat;
        private String sampleDat;
        private String samples;
        int background_colour;
        int background_tresh;
        double max_x_angle;
        double max_y_angle;
        double max_z_angle;
        double max_i_dev;

        private String[] IMAGE_EXTENSIONS= { "jpg", "jpeg", "bmp", "png", "pgm" };

        public Training(String _positives, String _negatives, int _w, int _h, int _stages, int _samples, int _bgColor, int _bgThresh, double _maxX, double _maxY, double _maxZ, double _maxI )
        {
            positivesLocation = _positives;
            negativesLocation = _negatives;
            width = _w;
            height = _h;
            numStages = _stages;
            numSamples = _samples;
            background_colour = _bgColor;
            background_tresh = _bgThresh;
            max_x_angle = _maxX;
            max_y_angle = _maxY;
            max_z_angle = _maxZ;
            max_i_dev = _maxI;
        }

        public void prepare()
        {
            changeExtensionToJpg(positivesLocation);
            changeExtensionToJpg(negativesLocation);
            resizeImages(positivesLocation);
            grayscale(positivesLocation);
            grayscale(negativesLocation);
            createDatFile(positivesLocation, "positives", "dat");
            createDatFile(negativesLocation, "negatives", "dat");
            //createSamples();
            //createDatFile("samples", "samples", "vec");
            //mergeVecFiles();
        }

        private void changeExtensionToJpg(String _path)
        {
            String[] all_images = getImages(_path);
            for (int i = 0; i < all_images.Length; i++)
            {
                String img = all_images[i];
                String filename = System.IO.Path.ChangeExtension(_path, ".jpg");
                System.IO.File.Move(_path, filename);
            }
            all_images = null;
        }

        private void resizeImages(String _path)
        {
            String[] pictures = Directory.GetFiles(_path, "*.jpg", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < pictures.Length; i++)
            {
                Bitmap image = new Bitmap(new Bitmap(pictures[i]), new Size(width, height));
                if (System.IO.File.Exists(pictures[i]))
                    System.IO.File.Delete(pictures[i]);
                image.Save(pictures[i]);
                image.Dispose();
            }
            pictures = null;
        }

        private void grayscale(String _path)
        {
            String[] all_images = getImages(_path);
            for (int i = 0; i < all_images.Length; i++)
            {
                Bitmap img = new Bitmap(all_images[i]);
                Image<Gray, Byte> grey;
                grey = convertToGrayscale(img);
                if (System.IO.File.Exists(all_images[i]))
                    System.IO.File.Delete(all_images[i]);
                grey.Save(new FileInfo(all_images[i]).Name);
                img.Dispose();
            }
        }

        private void createDatFile(String _path, String _filename, String _extension)
        {
            String[] files = Directory.GetFiles(_path, "*." + _extension, SearchOption.TopDirectoryOnly);
            Image<Gray, Byte> sample = new Image<Gray, Byte>(files[0]);
            StreamWriter writer = new StreamWriter(_filename, false);
            for (int i = 0; i < files.Length; i++)
            {
                writer.WriteLine(files[i]);
            }
            writer.Close();
        }

        private void createSamples()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"createtrainsamples.pl";
            String arg = String.Format("positives.dat negatives.dat samples -bgColour {0} -bgtresh {1} -maxxangle {2} -maxyangle {3} -maxzangle {4} -maxidev {5} -w {6} -h {7}", background_colour, background_tresh, max_x_angle, max_y_angle, max_y_angle, max_z_angle, max_i_dev, width, height);
            startInfo.Arguments = arg;
            Process.Start(startInfo);
        }

        private void mergeVecFiles()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"mergevec.exe";
            startInfo.Arguments = "samples.dat samples.vec";
            Process.Start(startInfo);
        }

        private Image<Gray, Byte> convertToGrayscale(Bitmap originalImage)
        {
            Image<Bgra, Byte> c = new Image<Bgra, Byte>(originalImage);
            Image<Gray, Byte> img = c.Convert<Gray, Byte>();
            return img;
        }

        private void startTraining() 
        {

        }

        private String[] getImages(String imagePath)
        {
            List<String> images = new List<String>();
            for (int i = 0; i < IMAGE_EXTENSIONS.Length; i++)
            {
                String[] pictures = Directory.GetFiles(imagePath, "*." + IMAGE_EXTENSIONS[i], SearchOption.TopDirectoryOnly);
                for (int j = 0; j < pictures.Length; j++)
                    images.Add(pictures[j]);
            }

            return images.ToArray();           
        }
    }
}
