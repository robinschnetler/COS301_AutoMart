using System;
using System.Collections.Generic;
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
	/// <summary>
	/// A class that implements the Filter interface 
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
			_advertDetails.Rating = calculateCoverageRating(coverage);
		}

		/// <summary>
		/// A function for calculating how many stars this image gets based on its coverage value (max 2 stars for coverage)
		/// </summary>
		/// <param name="_coverage">The coverage value of the image</param>
		/// <returns>The number of stars (out of 2) based on the coverage value</returns>
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