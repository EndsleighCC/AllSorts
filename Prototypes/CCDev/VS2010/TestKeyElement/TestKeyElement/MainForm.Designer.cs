namespace TestKeyElement
{
    partial class frmMain
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
            this.txtKeyElement0 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtKeyElement1 = new System.Windows.Forms.TextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtKeyElement0
            // 
            this.txtKeyElement0.Location = new System.Drawing.Point(173, 38);
            this.txtKeyElement0.Name = "txtKeyElement0";
            this.txtKeyElement0.Size = new System.Drawing.Size(100, 20);
            this.txtKeyElement0.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Key Element Text 0";
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(145, 201);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Key Element Text 1";
            // 
            // txtKeyElement1
            // 
            this.txtKeyElement1.Location = new System.Drawing.Point(173, 95);
            this.txtKeyElement1.Name = "txtKeyElement1";
            this.txtKeyElement1.Size = new System.Drawing.Size(100, 20);
            this.txtKeyElement1.TabIndex = 1;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(142, 150);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(53, 13);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "Unknown";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Key Element 0 is";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(247, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Key Element 1";
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnExecute;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 265);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.txtKeyElement1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKeyElement0);
            this.Name = "frmMain";
            this.Text = "Test Key Element";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtKeyElement0;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtKeyElement1;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

