/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Colour;

import Persister.Persister;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;

/**
 *
 * @author Yashu
 */
public class Profiler {
	
	Bucket[] buckets;
	int offset;
	double[] inputs;
	int divisions;
	double mean;
	BufferedImage bimg;
	Persister persister;
	
	/**
	 *	divisions	= 3
	 *	remainder	= 256 % divisions				=1
	 *	offset		= (256 - remainder) / divisions			= 85
	 *	range[1]	= 0 - (offset-1)				= 0	- 84
	 *	range[2]	= offset - ((2 * offset) - 1)			= 85	- 169
	 *	range[3]	= (2 * offset) - ((3 * offset) - 1 + remainder)	= 170	- 255
	 * @param divisions specifies the amount of buckets the image RGB values will be indexed with
	 */
	public Profiler(int _divisions)
	{
		mean = 0;
		divisions = _divisions;
		buckets = new Bucket[(int)Math.pow(divisions, 3)];
		int remainder = (256 % divisions);
		offset = (256-remainder)/divisions;
		int index = 0;
		for (int i = 0; i < divisions; i++) 
		{
			for (int j = 0; j < divisions; j++) 
			{
				for (int k = 0; k < divisions; k++) {
					int key = (100*i)+(10*j)+(k);
					buckets[index++] = new Bucket(key);
				}
			}
		}
		//Each bucket will have an input 
		//The additional 2 consist of the bias input and standard deviation of the bucket frequencies
		
		/**TODO change to this line when adding standard deviation to inputs
		inputs = new double[buckets.length + 2];**/
		inputs = new double[buckets.length + 1];
		persister = new Persister("Profiler_Class.txt");
	}
	
	//self explanatory
	public int getNumBuckets()
	{
		return buckets.length+1;
	}
	
	/**
	 * initialize all inputs by looping through buckets, normalizing the frequency of the pixels
	 * in that bucket, and placing them in the inputs[] array.
	 */
	private void initInputs()
	{
		int numInputs = buckets.length;
		for (int i = 0; i < numInputs; i++) 
		{
			inputs[i] = normalize((double)buckets[i].getnumPixels(), (double)Math.sqrt(3)*-1, (double)Math.sqrt(3), 0, bimg.getWidth() * bimg.getHeight());
		}
		inputs[numInputs] = -1;
		/*TODO add this line when neural network is working. also need to figure out how to normalize
		inputs[numInputs+1] = standardDeviation();*/
	}
	
	/**
	 * The standard deviation is the average of the differences between each pixels color and the 
	 * mean color of the image. This value will be one of the inputs into the neural network
	 * @return standard deviation
	 */
	public double standardDeviation()
	{
		int width = bimg.getWidth();
		int height = bimg.getHeight();
		double value = 0;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				value += Math.pow(((double)bimg.getRGB(i, j) - mean), 2);
			}
		}
		value = value * (1/((width*height)-1));
		value = (double) Math.sqrt(value);
		return value;
	}
	
	/**
	 * 
	 * @return mean RGB value of the image 
	 */
	public double getMean()
	{
		mean = 0;
		int width = bimg.getWidth();
		int height = bimg.getHeight();
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				mean+= bimg.getRGB(j,i);
			}
		}
		mean /= width*height;
		return mean;
	}
	
	
	/**
	 * normalize the input parameter to a double value between lower and upper for input 
	 * values into the neural network
	 * @param num integer value that needs to be converted ranging from 0 to total number of pixels (width * height of image)
	 * @param sLower lower scaled boundary to normalize to
	 * @param sUpper upper scaled boundary to normalize to
	 * @param uLower lower un-scaled boundary to normalize to
	 * @param uUpper upper un-scaled boundary to normalize to
	 * @return new double value between -sqrt(3) and sqrt(3)
	 */
	public static double normalize(double num, double sLower, double sUpper, double uLower, double uUpper)
	{
		return ( (num - uLower) / (uUpper-uLower) ) * (sLower-sUpper) + (sUpper);
	}
	
	/**
	 * 
	 * @param file file directory of image to do colour profiling on
	 * @return true if profiling completes without any problem, else false
	 */
	public double[] profile(String file)
	{
		//System.out.println("profile(): filename: " + file);
		try 
		{
			bimg = ImageIO.read(new File(file));
			int width = bimg.getWidth();
			int height = bimg.getHeight();
			getMean();
			for(int i = 0; i<height; i++)
			{
				for (int j = 0; j < width; j++) 
				{
					int rgb = bimg.getRGB(j, i);
					if(!getBin(rgb))
					{
						System.out.println("returning null");
						return null;
					}
				}
			}
			initInputs();
			if(!persister.write(this.toString()))
				System.out.println("failed to write to Profiler_Class.txt");
			persister.flush();
			return inputs;
		} 
		catch (IOException ex) 
		{
			System.out.println("problem loading image");
			return null;
		}
	}
	
	@Override
	public String toString()
	{
		String out = "Image Data:\n";
		out += bimg.toString();
		out += "\nGenerated Data:\n\n";
		out += "Number of Profiling Buckets: ";
		out += buckets.length;
		out += "\n\nMean RGB Value: ";
		out += mean;
		out += "\n\n"+inputs.length+" Input Values to Neural Network:\n";
		for (int i = 0; i < inputs.length; i++) 
		{
			out += (i+1)+":"+inputs[i] + "\n";
		}
		return out;
	}
	
	/**
	 * 
	 * @param colour unsigned int colour value representing RGB value in the form #RRGGBBAA
	 * @return true if able to find bin to allocate pixel to else false.
	 */
	public boolean getBin(int colour)
	{
		//get seperate RGB values
		int r = (colour)&0xFF;
		int g = (colour>>8)&0xFF;
		int b = (colour>>16)&0xFF;
		int lower = 0;
		int rBin, gBin, bBin;
		//initialize these to go in the last bin by default if they are unable to get categorized withing the loops
		rBin = gBin = bBin = divisions -1;
		for (int i = 0; i < divisions; i++) 
		{
			//categorize the division the R, G and B value falls in
			lower = i*offset;
			int upper = lower + offset +1;
			if(lower <= r && r < upper)
				r = i;
			if(lower <= g && g < upper)
				g = i;
			if(lower <= b && b < upper)
				b = i;
		}
		//generate the key specifying that combination of bucket entries
		int key = 100*r + 10*g + b;
		
		for (int i = 0; i < buckets.length; i++) 
		{			
			//finally add to bucket containing the same combination of R, G and B configuration
			if(buckets[i].addToBucket(key) != -1)
				return true;
			/*{
				shows which bucket the pixel is being added to...remember to comment line above if you uncomment this
				System.out.println("adding to bucket: " + i);
				return true;
			}*/
		}
		return false;
	}
}