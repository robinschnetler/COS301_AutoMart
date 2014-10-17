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
        /// Calculates the area of the image and the percentage that the car covers.
        /// </summary>
        /// <param name="_advertDetails"></param>AdvertDetails object contaning information about the image.
        public void pump(ref AdvertDetails _advertDetails)
        {
            //check if the car exists
            if (!_advertDetails.CarFound)
            {
                _advertDetails.Error = "Cannot calculate coverage if car not found.";
                return;
            }
			_advertDetails.CoverageValue = calculateCoverageValue(_advertDetails.Image.Width, _advertDetails.Image.Height, _advertDetails.Rect.Height, _advertDetails.Rect.Height);
			_advertDetails.CoverageRating = calculateCoverageRating(_advertDetails.CoverageValue);
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
        /// Calculates the cars coverage rating.
        /// </summary>
        /// <param name="_coverage"></param>The cars coverage of the image in precentage.
        /// <returns>Returns integer rating of the coverage.</returns>
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
