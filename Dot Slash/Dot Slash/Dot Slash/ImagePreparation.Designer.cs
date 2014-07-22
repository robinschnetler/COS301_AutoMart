namespace Dot_Slash
{
	partial class ImagePreparation
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.imagePreparattionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FilterDrop = new System.Windows.Forms.ToolStripMenuItem();
			this.sourceFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.destinationDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CropDrop = new System.Windows.Forms.ToolStripMenuItem();
			this.sourceDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.destinationDirectoryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.NaturalDrop = new System.Windows.Forms.ToolStripMenuItem();
			this.sourceDirectoryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.datFileToAppendToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lblInfo = new System.Windows.Forms.Label();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imagePreparattionToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(838, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// imagePreparattionToolStripMenuItem
			// 
			this.imagePreparattionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FilterDrop,
            this.CropDrop,
            this.NaturalDrop});
			this.imagePreparattionToolStripMenuItem.Name = "imagePreparattionToolStripMenuItem";
			this.imagePreparattionToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.imagePreparattionToolStripMenuItem.Text = "Tools";
			// 
			// FilterDrop
			// 
			this.FilterDrop.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceFolderToolStripMenuItem,
            this.destinationDirectoryToolStripMenuItem});
			this.FilterDrop.Name = "FilterDrop";
			this.FilterDrop.Size = new System.Drawing.Size(154, 22);
			this.FilterDrop.Text = "Filter By Hand";
			// 
			// sourceFolderToolStripMenuItem
			// 
			this.sourceFolderToolStripMenuItem.Name = "sourceFolderToolStripMenuItem";
			this.sourceFolderToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.sourceFolderToolStripMenuItem.Text = "Source Directory";
			// 
			// destinationDirectoryToolStripMenuItem
			// 
			this.destinationDirectoryToolStripMenuItem.Name = "destinationDirectoryToolStripMenuItem";
			this.destinationDirectoryToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.destinationDirectoryToolStripMenuItem.Text = "Destination Directory";
			// 
			// CropDrop
			// 
			this.CropDrop.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceDirectoryToolStripMenuItem,
            this.destinationDirectoryToolStripMenuItem1});
			this.CropDrop.Name = "CropDrop";
			this.CropDrop.Size = new System.Drawing.Size(154, 22);
			this.CropDrop.Text = "Crop Image";
			// 
			// sourceDirectoryToolStripMenuItem
			// 
			this.sourceDirectoryToolStripMenuItem.Name = "sourceDirectoryToolStripMenuItem";
			this.sourceDirectoryToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.sourceDirectoryToolStripMenuItem.Text = "Source Directory";
			// 
			// destinationDirectoryToolStripMenuItem1
			// 
			this.destinationDirectoryToolStripMenuItem1.Name = "destinationDirectoryToolStripMenuItem1";
			this.destinationDirectoryToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
			this.destinationDirectoryToolStripMenuItem1.Text = "Destination Directory";
			// 
			// NaturalDrop
			// 
			this.NaturalDrop.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceDirectoryToolStripMenuItem1,
            this.datFileToAppendToToolStripMenuItem});
			this.NaturalDrop.Name = "NaturalDrop";
			this.NaturalDrop.Size = new System.Drawing.Size(154, 22);
			this.NaturalDrop.Text = "Natural Images";
			// 
			// sourceDirectoryToolStripMenuItem1
			// 
			this.sourceDirectoryToolStripMenuItem1.Name = "sourceDirectoryToolStripMenuItem1";
			this.sourceDirectoryToolStripMenuItem1.Size = new System.Drawing.Size(188, 22);
			this.sourceDirectoryToolStripMenuItem1.Text = "Source Directory";
			// 
			// datFileToAppendToToolStripMenuItem
			// 
			this.datFileToAppendToToolStripMenuItem.Name = "datFileToAppendToToolStripMenuItem";
			this.datFileToAppendToToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
			this.datFileToAppendToToolStripMenuItem.Text = ".dat File to Append to";
			// 
			// lblInfo
			// 
			this.lblInfo.AutoSize = true;
			this.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblInfo.Location = new System.Drawing.Point(0, 232);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(180, 13);
			this.lblInfo.TabIndex = 1;
			this.lblInfo.Text = "Please select a tool from Tools menu";
			// 
			// ImagePreparation
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(838, 245);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "ImagePreparation";
			this.Text = "ImagePreparation";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem imagePreparattionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem FilterDrop;
		private System.Windows.Forms.ToolStripMenuItem CropDrop;
		private System.Windows.Forms.ToolStripMenuItem NaturalDrop;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.ToolStripMenuItem sourceFolderToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem destinationDirectoryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sourceDirectoryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem destinationDirectoryToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem sourceDirectoryToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem datFileToAppendToToolStripMenuItem;
	}
}