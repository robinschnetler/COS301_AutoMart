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
	class Filter
	{
		Image<Bgr, Int32> input;
		Image<Bgr, Int32> output;

		public Filter(Image<Bgr, Int32> im)
		{
			input = im;
		}
	}
}
