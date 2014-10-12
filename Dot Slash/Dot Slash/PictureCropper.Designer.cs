namespace Dot_Slash
{
	partial class PictureCropper
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Path = new System.Windows.Forms.Label();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// Path
			// 
			this.Path.AutoSize = true;
			this.Path.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.Path.Location = new System.Drawing.Point(0, 351);
			this.Path.Name = "Path";
			this.Path.Size = new System.Drawing.Size(359, 13);
			this.Path.TabIndex = 1;
			this.Path.Text = "left click to select points to crop, right click to crop and move to next image";
			this.Path.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pictureBox
			// 
			this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox.Location = new System.Drawing.Point(0, 0);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(568, 364);
			this.pictureBox.TabIndex = 2;
			this.pictureBox.TabStop = false;
			this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
			// 
			// PictureCropper
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(568, 364);
			this.Controls.Add(this.Path);
			this.Controls.Add(this.pictureBox);
			this.Name = "PictureCropper";
			this.Text = "PictureCropper";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label Path;
		private System.Windows.Forms.PictureBox pictureBox;
	}
}