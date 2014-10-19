using System;
using System.Drawing;

namespace Dot_Slash
{
	/// <summary>
	/// Class used to calculate the car coverage of the image.
	/// </summary>
	public class CoverageDetector : Filter
	{
		/// <summary>
		/// Implements the coverage detector filter and adds the details to the advertDetails object
		/// </summary>
		/// <param name="_advertDetails">The advertDetails object where information about coverage is stored</param>
		public void pump(ref AdvertDetails _advertDetails)
		{
			if(!_advertDetails.CarFound)
				throw new Exception("Cannot calculate coverage if car not found");
			float totalArea = _advertDetails.Image.Width * _advertDetails.Image.Height;
			float area = _advertDetails.Rect.Width * _advertDetails.Rect.Height;
			float coverage = area/totalArea * 100;
			_advertDetails.CoverageValue = coverage;
			_advertDetails.CoverageRating = calculateCoverageRating(coverage);
		}

		/// <summary>
		/// Calculates the percentage that the car covers in the image.
		/// </summary>
		/// <param name="_imageWidth"></param>Integer representing the width of the image.
		/// <param name="_imageHeight"></param>Integer representinh the height of the value.
		/// <param name="_carRect"></param>Rectangle representing the location and the size of the area covered by the car.
		/// <returns>Float value representing the percentage of the area covered by the car.</returns>
		public double calculateCoverageValue(int _imageWidth, int _imageHeight, int _carWidth, int _carHeight)
		{
			return Math.Round((double)(_carWidth * _carHeight) / (_imageWidth * _imageHeight) * 100, 2);
}

		/// <summary>
		/// A function for calculating how many stars this image gets based on its coverage value (max 2 stars for coverage)
		/// </summary>
		/// <param name="_coverage">The coverage value of the image</param>
		/// <returns>The number of stars (out of 2) based on the coverage value</returns>
		public int calculateCoverageRating(double _coverage)
		{
			if (_coverage >= 70.0)
				return 2;
			else if (_coverage >= 34.0)
				return 1;
			else
				return 0;
		}
	}
}