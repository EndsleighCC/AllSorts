namespace TestString
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
            this.txtValue = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMultiline = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(146, 49);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(174, 20);
            this.txtValue.TabIndex = 0;
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(146, 114);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(174, 20);
            this.txtResult.TabIndex = 2;
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(146, 370);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 3;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Text to be filtered";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Result";
            // 
            // txtMultiline
            // 
            this.txtMultiline.Location = new System.Drawing.Point(59, 186);
            this.txtMultiline.Multiline = true;
            this.txtMultiline.Name = "txtMultiline";
            this.txtMultiline.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMultiline.Size = new System.Drawing.Size(261, 161);
            this.txtMultiline.TabIndex = 6;
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnExecute;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 456);
            this.Controls.Add(this.txtMultiline);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtValue);
            this.Name = "frmMain";
            this.Text = "Test String";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMultiline;
    }
}

