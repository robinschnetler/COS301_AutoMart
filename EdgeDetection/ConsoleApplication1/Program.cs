using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace EdgeDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
		String[] pictures = Directory.GetFiles("cars/", "*.jpg", SearchOption.TopDirectoryOnly);
                Console.WriteLine("Enter Threshold Value: ");
                String thresh = Console.ReadLine();
                Console.WriteLine("Use Diagonal? (Default = True) ");
                String input = Console.ReadLine();
                Boolean diag = false;
                if (input.ToUpper() != "N")
                    diag = true;
                Grid grid = new Grid(Convert.ToInt32(thresh), diag);
		for (int i = 0; i < pictures.Length; i++)
		{
			grid.run(pictures[i], i);
		}
                
                Console.WriteLine("Quit? (default = No)");
                if (Console.ReadLine().ToUpper() == "Y")
                    break;
                else
                    Console.Clear();
            }
        }
    }

    class Grid
    {
        int threshold;
        Boolean diagonal;
        public Grid(int th, Boolean d)
        {
            diagonal = d;
            threshold = th;
        }

        public void run(string filename, int val)
        {
            Bitmap img = new Bitmap(filename);
            Bitmap edgedImg = new Bitmap(img.Width, img.Height);
            makeEdge(img, edgedImg);
            edgedImg.Save("edged/edged" + threshold + diagonal + val + ".jpg");
        }

        public void makeEdge(Bitmap img, Bitmap edge)
        {
            /**
             * white colour for edge and black for any other pixel
             * */
            Color black = Color.FromArgb(255, 0, 0, 0);
            Color white = Color.FromArgb(255, 255, 255, 255);
            for (int i = 1; i < img.Width-1; i++)
            {
                for (int j = 1; j < img.Height-1; j++)
                {
                    //get RGB values for current pixel
                    Color color = img.GetPixel(i, j);
                    byte r, g, b;
                    r = color.R;
                    g = color.G;
                    b = color.B;
                    //get RGB Values for pixel directly right of current pixel
                    Color colorRight = img.GetPixel(i, j + 1);
                    byte rR, gR, bR;
                    rR = colorRight.R;
                    gR = colorRight.G;
                    bR = colorRight.B;
                    //get RGB values for pixel directly under current pixel
                    Color colorBottom = img.GetPixel(i + 1, j);
                    byte rB, gB, bB;
                    rB = colorBottom.R;
                    gB = colorBottom.G;
                    bB = colorBottom.B;
                    //get RGB values for pixel diagonally to the bottom right of the current pixel
                    Color colorBottomRight = img.GetPixel(i + 1, j+1);
                    byte rBR, gBR, bBR;
                    rBR = colorBottomRight.R;
                    gBR = colorBottomRight.G;
                    bBR = colorBottomRight.B;

                    /**the difference between the RGB values of each pixel are calculated and raised to the power of 2 respectively to get the
                     * absolute difference between values. 
                     **/
                    double differenceRight = (Math.Pow((r - rR), 2) + Math.Pow((g - gR), 2) + Math.Pow((b - bR), 2));
                    double differenceBottom = (Math.Pow((r - rB), 2) + Math.Pow((g - gB), 2) + Math.Pow((b - bB), 2));
                    double differenceBottomRight = 0.0;
                    if (diagonal)
                    {
                        differenceBottomRight = (Math.Pow((r - rBR), 2) + Math.Pow((g - gBR), 2) + Math.Pow((b - bBR), 2));
                    }

                    //instead of rooting the distance between the colour values, we rather raise the threshold to the power of 2 as well
                    Double thresholdSquared = Math.Pow(threshold, 2);

                    //if bottom right pixel was used to determine whether or not the pixel is an edge
                    if (diagonal)
                    {
                        /**
                         * If either the right, bottom, or bottom right differs from the current pixel by some threshold, then the pixel is on an edge
                         */
                        if (differenceBottom > thresholdSquared || differenceRight > thresholdSquared || differenceBottomRight > thresholdSquared)
                            edge.SetPixel(i, j, white);
                        else
                            edge.SetPixel(i, j, black);
                    }
                    else
                    {
                        if (differenceBottom > thresholdSquared || differenceRight > thresholdSquared)
                            edge.SetPixel(i, j, white);
                        else
                            edge.SetPixel(i, j, black);
                    }
                }
            }
        }
    }
}
