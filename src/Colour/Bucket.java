/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Colour;

/**
 *
 * @author Yashu
 */
public class Bucket {
	int key;
	int numPixels;
	
	/**
	 * 
	 * @param k	each bucket will have a specific value associated to it= (100*red_value)+(10*blue_value)+(green_value)
	 *		which will associate the bucket with a certain combination of RGB ranges
	 */
	public Bucket(int k)
	{
		key = k;
		numPixels = 0;
	}
	/**
	 * 
	 * @param _key the range of the pixel that is to be added to the bucket
	 * @return The number of pixels that are in this bucket
	 */
	public int addToBucket(int _key)
	{
		int num = (_key==key)?(++numPixels):-1;
		return num;
	}
	
	/**
	 * 
	 * @return number of pixels in this bucket
	 */
	public int getnumPixels()
	{
		return numPixels;
	}
}
