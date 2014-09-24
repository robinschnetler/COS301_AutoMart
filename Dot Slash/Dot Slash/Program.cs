using System;
using System.Collections.Generic;
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
		/// <summary>
		/// display function to show program options to user
		/// </summary>
		static void display()
		{
			Console.WriteLine("Image processing options:");
			Console.WriteLine("1) Resizing (Default: to 480 X 240 in 'images/')");
			Console.WriteLine("2) Apply Gaussian Filter");
			Console.WriteLine("3) Greyscaling");
			Console.WriteLine("4) Edge Detection");
			Console.WriteLine("5) Generate Integral Image");
			Console.WriteLine("6) Crop images");
			Console.WriteLine("7) Filter images by hand");
			Console.WriteLine("8) Create positives.dat");
			Console.WriteLine("9) Create negatives.dat");
			Console.WriteLine("10) Change filename extensions");
			Console.WriteLine("11) Detect car objects");
			Console.WriteLine("12) Detect blur on images");
			Console.WriteLine("13) Detect Model");
			Console.WriteLine("14) Detect Colour");
			Console.WriteLine("15) Subdivide image");
            Console.WriteLine("16) PipeLine");
			Console.WriteLine("17) Exit");
		}

		[STAThread] //allows for main to open dialogs(something to do with threads)
		static void Main(string[] args)
		{
			ImageProcessor imageProcessor = new ImageProcessor();
			Tools tools = new Tools();
			display();
			int chosen = Convert.ToInt32(Console.ReadLine());
			while(chosen != 17)
			{ 
				switch(chosen)
				{ 
					case 1:
						{ 
							String input;
							Console.WriteLine("Resizing Images");
							Console.WriteLine("Width: (Default = 480)");
							int w, h;
							input = Console.ReadLine();
							
							if(input.Length == 0)
							{
								w = 480;
							}
							else
							{
								w = Convert.ToInt32(input);
							}
							
							Console.WriteLine("Height: (Default = 240)");
							input = Console.ReadLine();
							
							if(input.Length == 0)
							{
								h = 240;
							}
							else
							{
								h = Convert.ToInt32(input);
							}
							
							Console.WriteLine("Folder: (Default = 'images')");
							input = Console.ReadLine();
							String folder;

							if (input.Length == 0)
							{
								folder = "images";
							}
							else
							{
								folder = input;
							}

							folder += "/";						
							if(Directory.Exists(folder))
							{
								imageProcessor.resize(folder, w, h);
							}
							else
							{
								imageProcessor.resize(w, h);
							}
							Console.WriteLine();
							break;
						}
					case 2:
						{
							Console.WriteLine("Applying Gaussian Filter");
							imageProcessor.applyGaussian();
							Console.WriteLine();
							break;
						}
					case 3:
						{
							Console.WriteLine("Changing images to Greyscale");
							imageProcessor.makeGreyscale();
							Console.WriteLine();
							break;
						}
					case 4:
						{
							Console.WriteLine("Applying Edge Detector");
							imageProcessor.detectEdges();
							Console.WriteLine();
							break;
						}
					case 5:
						{
							Console.WriteLine("Creating Integral Image(summed area table) for 'integral/grey.jpg'");
							Tools t = new Tools();
							Bitmap integralImage = t.generateIntegralImage("integral/grey.jpg");
							integralImage.Save("integral/integralimage.jpg");
							Console.WriteLine("Saving integral image for 'integral/grey.jpg' as 'integral/integralimage.jpg'");
							Console.WriteLine();
							break;
						}
					case 6:
						{
							tools.photoCropper();
							break;
						}
					case 7:
						{
							tools.photoChooser();
							break;
						}
					case 8:
						{
							Console.WriteLine("Which folder has positive samples?");
							String path = Console.ReadLine();
							tools.createDat(path + "/", "positives.dat", true);
							break;
						}
					case 9:
						{
							Console.WriteLine("Which folder has negative samples?");
							String path = Console.ReadLine();
							tools.createDat(path + "/", "negatives.dat", false);
							break;
						}
					case 10:
						{
							Console.WriteLine("Renaming filename extensions:");

							Console.WriteLine("Directory containing the files you wish to rename: [eg 'negative' or 'negative/rename']");
							String dir = Console.ReadLine();
							Console.WriteLine();

							Console.WriteLine("Extension from: [eg 'jpg' to change all .jpg files]");
							String from = Console.ReadLine();
							Console.WriteLine();

							Console.WriteLine("Extension to: [eg 'vec' to change to .vec files]");
							String to = Console.ReadLine();
							Console.WriteLine();
							tools.changeExtension(dir, from, to);
							break;
						}
					case 11:
						{
							Console.WriteLine("Which folder of images would you like to run the classifier on?");
							String path = Console.ReadLine();
							Console.WriteLine("Would you like to crop the detected car?");
							Boolean input  = (Console.ReadLine().ToUpper().Equals("Y"))?true:false;
							//162, 10
							Console.WriteLine("Num neighbours?");
							int n = Convert.ToInt32(Console.ReadLine());
							imageProcessor.detect(path + "/", "classifier/classifiers/front/cascade.xml", "DetectedCars", new Bgr(50, 50, 255), new Size(140, 120), new Size(480, 480), n, input);
							break;
						}
					case 12:
						{
							Console.WriteLine("Which folder of images would you like to run the blur detector on?");
							String path = Console.ReadLine();
							//imageProcessor.detectBlur(path + "/", 0.5);
							break;
						}
					case 13:
						{
							Console.WriteLine("Which folder of image would you like to run the Model Detection on?");
							String path = Console.ReadLine();
							Console.WriteLine("Num neighbours?");
							int input = Convert.ToInt32(Console.ReadLine());
							imageProcessor.detect(path + "/", "classifier/classifiers/model/Volkwagen.xml", "DetectedModels", new Bgr(255,50,50), new Size(15,15), new Size(60,60), input, false);
							break;
						}
					case 14:
						{
							Console.WriteLine("Which folder if images would you like to run colour detector on?");
							String path = Console.ReadLine();
							//imageProcessor.detectColour(path, "ColourDetector");
							break;
						}
					case 15:
						{
							Console.WriteLine("Which folder would you like to run subdivision on?");
							String path = Console.ReadLine();
							String[] images = Tools.getImages(path + "/", Globals.extensions);
							Console.WriteLine("How many sub-divisions?");
							int div = Convert.ToInt32(Console.ReadLine());
							Console.WriteLine("Where would you like to save to?");
							String to = Console.ReadLine();
							if(!Directory.Exists(to + "/"))
								Directory.CreateDirectory(to + "/");
							for (int i = 0; i < images.Length; i++)
							{
								tools.subDivide(new Image<Bgr,Int32>(images[i]), div).Save("divided_" + new FileInfo(images[i]).Name);
							}
							break;
						}
                    case 16:
                        {
                            Byte[] image = File.ReadAllBytes("pipe/image.jpeg"); 
                            AdvertDetails advertDetails = new AdvertDetails(image);
                            Filter[] filters = { new CarDetector(), new BlurDetector(), new ColourDetector(), new CoverageDetector() };
                            List<Filter> filterList = new List<Filter>(filters);
                            Pipe pipe = new Pipe(filterList, advertDetails);
                            advertDetails = pipe.flow();
                            Console.WriteLine(advertDetails.retreiveDetails());
                            break;
                        }
					default:
						{
							Console.WriteLine("Please choose valid option");
							Console.WriteLine();
							break;
						}
				}
				display();
				chosen = Convert.ToInt32(Console.ReadLine());
				Console.WriteLine();
			}
		}
	}
}
