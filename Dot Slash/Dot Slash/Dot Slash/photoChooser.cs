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
		public int current = 0;
		int iconWidth, iconHeight;
		StreamWriter file = new StreamWriter("classified", true);
		public photoChooser(String[] _files)
		{
			bool directoryExists = Directory.Exists("SelectedCars/");
			if (!directoryExists)
				Directory.CreateDirectory("SelectedCars/");
			bool otherdirectoryExists = Directory.Exists("otherImages/");
			if (!otherdirectoryExists)
				Directory.CreateDirectory("otherImages/");
			bool frontDirectory = Directory.Exists("frontView/");
			if (!frontDirectory)
				Directory.CreateDirectory("frontView/");
			bool sideDirectory = Directory.Exists("sideView/");
			if (!sideDirectory)
				Directory.CreateDirectory("sideView/");
			bool angleDirectory = Directory.Exists("angledView/");
			if (!angleDirectory)
				Directory.CreateDirectory("angledView/");
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
			else if(e.KeyChar == 'm')
			{
				pbView.Image.Save("otherImages/" + new FileInfo(files[current]).Name);
			}
			else if (e.KeyChar == 's')
			{
				pbView.Image.Save("sideView/" + new FileInfo(files[current]).Name);
			}
			else if (e.KeyChar == 'f')
			{
				pbView.Image.Save("frontView/" + new FileInfo(files[current]).Name);
			}
			else if (e.KeyChar == 'a')
			{
				pbView.Image.Save("angledView/" + new FileInfo(files[current]).Name);
			}
			updateView();
		}

		private void formClose(Object o, FormClosingEventArgs e)
		{
            file.Close();	
		}

		private void updateView()
		{
            if (current < files.Length)
            {
                file.Write(files[current]+"\n");
                current++;
            }
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
