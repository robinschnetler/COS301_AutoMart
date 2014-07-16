namespace Dot_Slash
{
	partial class photoChooser
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(formClose);
			this.panel1 = new System.Windows.Forms.Panel();
			this.pbNext = new System.Windows.Forms.PictureBox();
			this.pb2next = new System.Windows.Forms.PictureBox();
			this.pbCurrent = new System.Windows.Forms.PictureBox();
			this.pbPrevious = new System.Windows.Forms.PictureBox();
			this.pb2previous = new System.Windows.Forms.PictureBox();
			this.pbView = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbNext)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb2next)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbCurrent)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbPrevious)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb2previous)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbView)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.pbNext);
			this.panel1.Controls.Add(this.pb2next);
			this.panel1.Controls.Add(this.pbCurrent);
			this.panel1.Controls.Add(this.pbPrevious);
			this.panel1.Controls.Add(this.pb2previous);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 375);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(627, 89);
			this.panel1.TabIndex = 1;
			// 
			// pbNext
			// 
			this.pbNext.Location = new System.Drawing.Point(372, 1);
			this.pbNext.Name = "pbNext";
			this.pbNext.Size = new System.Drawing.Size(124, 86);
			this.pbNext.TabIndex = 4;
			this.pbNext.TabStop = false;
			// 
			// pb2next
			// 
			this.pb2next.Location = new System.Drawing.Point(502, 1);
			this.pb2next.Name = "pb2next";
			this.pb2next.Size = new System.Drawing.Size(122, 86);
			this.pb2next.TabIndex = 3;
			this.pb2next.TabStop = false;
			// 
			// pbCurrent
			// 
			this.pbCurrent.Location = new System.Drawing.Point(242, 1);
			this.pbCurrent.Name = "pbCurrent";
			this.pbCurrent.Size = new System.Drawing.Size(124, 86);
			this.pbCurrent.TabIndex = 2;
			this.pbCurrent.TabStop = false;
			// 
			// pbPrevious
			// 
			this.pbPrevious.Location = new System.Drawing.Point(118, 0);
			this.pbPrevious.Name = "pbPrevious";
			this.pbPrevious.Size = new System.Drawing.Size(118, 86);
			this.pbPrevious.TabIndex = 1;
			this.pbPrevious.TabStop = false;
			// 
			// pb2previous
			// 
			this.pb2previous.Location = new System.Drawing.Point(0, 0);
			this.pb2previous.Name = "pb2previous";
			this.pb2previous.Size = new System.Drawing.Size(112, 86);
			this.pb2previous.TabIndex = 0;
			this.pb2previous.TabStop = false;
			// 
			// pbView
			// 
			this.pbView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbView.Location = new System.Drawing.Point(0, 0);
			this.pbView.Name = "pbView";
			this.pbView.Size = new System.Drawing.Size(627, 375);
			this.pbView.TabIndex = 2;
			this.pbView.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 359);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(412, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "press space to save image as car or any other key to save image to alternate dire" +
    "ctory";
			// 
			// photoChooser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(627, 464);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pbView);
			this.Controls.Add(this.panel1);
			this.Name = "photoChooser";
			this.Text = "photoChooser";
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(keypressed);
			//this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(formClose);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbNext)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb2next)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbCurrent)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbPrevious)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb2previous)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pbNext;
		private System.Windows.Forms.PictureBox pb2next;
		private System.Windows.Forms.PictureBox pbCurrent;
		private System.Windows.Forms.PictureBox pbPrevious;
		private System.Windows.Forms.PictureBox pb2previous;
		private System.Windows.Forms.PictureBox pbView;
		private System.Windows.Forms.Label label1;
	}
}