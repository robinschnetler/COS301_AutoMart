using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dot_Slash
{
	public partial class PictureCropper : Form
	{
		String[] files;
		int index = 0;
		int x , y;
		int rx = 0, ry = 0;
		int aspectx, aspecty;
		int width, height;
		int clickCount = 0;
		public PictureCropper(String[] _files, int _aspectx, int _aspecty)
		{
			InitializeComponent();
			files = _files;
			aspectx = _aspectx;
			aspecty = _aspecty;
			Bitmap b = new Bitmap(files[0]);
			width = b.Width;
			height = b.Height;
			x = width;
			y = height;
			pictureBox.ImageLocation = files[index];
			pictureBox.Load();
		}

		private void pictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				if(e.X < x)
					x = e.X;
				if (e.Y < y)
					y = e.Y;
				if (e.X > rx)
					rx = e.X;
				if (e.Y > ry)
					ry = e.Y;
				clickCount++;
				Graphics g = Graphics.FromImage(pictureBox.Image);
				Pen pen = new Pen(Color.Red, 2);
				g.DrawEllipse(pen, e.X, e.Y, 2,2);
				if(clickCount < 4)
					g.Save();
				Pen p = new Pen(Color.Yellow, 1);
				if(clickCount > 3)
				{
					g.DrawRectangle(p, x, y, rx - x, ry - y);
					g.Save();
				}
				pictureBox.Refresh();
			}
			else if(e.Button == MouseButtons.Right)
			{
				if(clickCount < 4)
					MessageBox.Show("Please select more regions by left-clicking", "Not enough information", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
				else
				{ 
					Bitmap oldPic = new Bitmap(files[index]);
					Bitmap newImage = oldPic.Clone(new Rectangle(x, y, rx - x, ry-y), oldPic.PixelFormat);
					bool cropExist = Directory.Exists("Cropped/");
					if(!cropExist)
						Directory.CreateDirectory("Cropped/");
					newImage.Save("Cropped/Cropped_"+new FileInfo(files[index++]).Name);
					if(index < files.Length)
					{ 
						pictureBox.ImageLocation = files[index];
						pictureBox.Refresh();
						rx = 0; 
						ry = 0;
						x = width;
						y = height;
						clickCount = 0;
					}
					else
						this.Close();
				}
			}
		}
	}
}
