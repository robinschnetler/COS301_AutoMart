using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dot_Slash
{
	public partial class ImagePreparation : Form
	{
		String[] pictures;
		public ImagePreparation(String[] _pictures)
		{
			pictures = _pictures;
			InitializeComponent();
		}
	}	
}
