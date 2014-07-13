using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Dot_Slash
{
	public partial class photoChooser : Form
	{
		string[] files;
		int current = 0;
		int iconWidth, iconHeight;
		public photoChooser(String[] _files)
		{
			bool directoryExists = Directory.Exists("SelectedCars/");
			if (!directoryExists)
				Directory.CreateDirectory("SelectedCars/");
			bool otherdirectoryExists = Directory.Exists("otherImages/");
			if (!otherdirectoryExists)
				Directory.CreateDirectory("otherImages/");
			InitializeComponent();
			iconHeight = pbCurrent.Height;
			iconWidth = pbCurrent.Width;
			files = _files;
			pbView.Image = new Bitmap(files[current]);
			pbCurrent.Image = new Bitmap(pbView.Image, iconWidth, iconHeight);
			try
				{pbNext.Image = new Bitmap(new Bitmap(files[current+1]), iconWidth, iconHeight);}
			catch (System.IndexOutOfRangeException){}
			try
				{pb2next.Image = new Bitmap(new Bitmap(files[current+2]), iconWidth, iconHeight);}
			catch (System.IndexOutOfRangeException){}
		}

		private void keypressed(Object o, KeyPressEventArgs e)
		{
			if(e.KeyChar == (char) Keys.Space)
			{
				try
				{ 
					pbView.Image.Save("SelectedCars/" + new FileInfo(files[current]).Name);
				}
				catch(System.NullReferenceException)
				{
					this.Close();
				}
				catch(System.IndexOutOfRangeException)
				{
					this.Close();
				}
			}
			else
			{
				pbView.Image.Save("otherImages/" + new FileInfo(files[current]).Name);
			}
			updateView();
		}

		private void formClose(Object o, FormClosedEventArgs e)
		{
			pb2previous.Image = null;
			pbPrevious.Image = null;
			pbCurrent.Image = null;
			pbView.Image = null;
			pbNext.Image = null;
			pb2next.Image = null;
			for (int i = 0; i < current; i++)
			{
				File.Delete(files[i]);
			}	
		}

		private void updateView()
		{
			if(current < files.Length)
				current++;
			pb2previous.Image = pbPrevious.Image;
			pb2previous.Refresh();
			pbPrevious.Image = pbCurrent.Image;
			pbPrevious.Refresh();
			pbCurrent.Image = pbNext.Image;
			pbCurrent.Refresh();
			pbNext.Image = pb2next.Image;
			pbNext.Refresh();
			try
			{
				pb2next.Image = new Bitmap(new Bitmap(files[current+2]), iconWidth, iconHeight);
				pb2next.Refresh();
			}
			catch(System.IndexOutOfRangeException)
			{
				pb2next.Image = null;
				pb2next.Refresh();
			}
			try
			{
				pbView.Image = new Bitmap(files[current]);
				pbView.Refresh();
			}
			catch(System.IndexOutOfRangeException)
			{
				pbView.Image = null;
				pbView.Refresh();
			}
			this.Refresh();
		}
	}
}
