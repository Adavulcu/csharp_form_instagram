namespace _14331017MerveCandir
{
    partial class albumFotoEkleForm
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
            this.tumFotogroupBox1 = new System.Windows.Forms.GroupBox();
            this.secilenFotopictureBox1 = new System.Windows.Forms.PictureBox();
            this.ekleBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.secilenFotopictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tumFotogroupBox1
            // 
            this.tumFotogroupBox1.Location = new System.Drawing.Point(13, 149);
            this.tumFotogroupBox1.Name = "tumFotogroupBox1";
            this.tumFotogroupBox1.Size = new System.Drawing.Size(562, 250);
            this.tumFotogroupBox1.TabIndex = 0;
            this.tumFotogroupBox1.TabStop = false;
            // 
            // secilenFotopictureBox1
            // 
            this.secilenFotopictureBox1.Location = new System.Drawing.Point(228, 13);
            this.secilenFotopictureBox1.Name = "secilenFotopictureBox1";
            this.secilenFotopictureBox1.Size = new System.Drawing.Size(113, 101);
            this.secilenFotopictureBox1.TabIndex = 1;
            this.secilenFotopictureBox1.TabStop = false;
            // 
            // ekleBtn
            // 
            this.ekleBtn.Location = new System.Drawing.Point(247, 120);
            this.ekleBtn.Name = "ekleBtn";
            this.ekleBtn.Size = new System.Drawing.Size(75, 23);
            this.ekleBtn.TabIndex = 2;
            this.ekleBtn.Text = "EKLE";
            this.ekleBtn.UseVisualStyleBackColor = true;
            this.ekleBtn.Click += new System.EventHandler(this.ekleBtn_Click);
            // 
            // albumFotoEkleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 397);
            this.Controls.Add(this.ekleBtn);
            this.Controls.Add(this.secilenFotopictureBox1);
            this.Controls.Add(this.tumFotogroupBox1);
            this.Name = "albumFotoEkleForm";
            this.Text = "FOROĞRAF EKLE";
            this.Load += new System.EventHandler(this.albumFotoEkleForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.secilenFotopictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox tumFotogroupBox1;
        private System.Windows.Forms.PictureBox secilenFotopictureBox1;
        private System.Windows.Forms.Button ekleBtn;
    }
}