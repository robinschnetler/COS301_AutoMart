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
			if(!_advertDetails.CarFound)
				throw new Exception("Cannot calculate coverage if car not found");

			float totalArea = _advertDetails.Image.Width * _advertDetails.Image.Height;
			float area = _advertDetails.Rect.Width * _advertDetails.Rect.Height;
			float coverage = area/totalArea*100;
			_advertDetails.CoverageValue = coverage;
			_advertDetails.Rating = calculateCoverageRating(coverage);
        }

        /// <summary>
        /// Calculates the cars coverage rating.
        /// </summary>
        /// <param name="_coverage"></param>The cars coverage of the image in precentage.
        /// <returns>Returns integer rating of the coverage.</returns>
		private int calculateCoverageRating(float _coverage)
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
