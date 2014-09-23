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
	class AdvertDetails
	{
		Boolean carFound{ get; set; }
		Image<Bgr, Int32> image { get; set; }
		float blurValue { get; set; }
		float coverageValue { get; set; }
		String colour1 { get; set; }
		String colour2 { get; set; }
		String colour3 { get; set; }
		public AdvertDetails(Image<Bgr, Int32> im)
		{
			carFound = false;
			image = im;
			blurValue = 0.0f;
			coverageValue = 0.0f;
			colour1 = null;
			colour2 = null;
			colour3 = null;
		}


	}
}
