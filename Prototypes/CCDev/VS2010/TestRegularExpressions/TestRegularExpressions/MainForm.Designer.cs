namespace TestRegularExpressions
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearchIn = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPattern = new System.Windows.Forms.TextBox();
            this.txtDisplay = new System.Windows.Forms.TextBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text to search in:";
            // 
            // txtSearchIn
            // 
            this.txtSearchIn.Location = new System.Drawing.Point(107, 37);
            this.txtSearchIn.Name = "txtSearchIn";
            this.txtSearchIn.Size = new System.Drawing.Size(283, 20);
            this.txtSearchIn.TabIndex = 1;
            this.txtSearchIn.Text = "ABCDEFTT.001 ABCDEFRR.002";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pattern";
            // 
            // txtPattern
            // 
            this.txtPattern.Location = new System.Drawing.Point(107, 92);
            this.txtPattern.Name = "txtPattern";
            this.txtPattern.Size = new System.Drawing.Size(283, 20);
            this.txtPattern.TabIndex = 3;
            this.txtPattern.Text = "[A-Z]{6}[NRT0][NRT12]\\.[0-9]{3}";
            // 
            // txtDisplay
            // 
            this.txtDisplay.Location = new System.Drawing.Point(15, 142);
            this.txtDisplay.Multiline = true;
            this.txtDisplay.Name = "txtDisplay";
            this.txtDisplay.Size = new System.Drawing.Size(375, 259);
            this.txtDisplay.TabIndex = 4;
            this.txtDisplay.WordWrap = false;
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(166, 419);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 5;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnExecute;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 466);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.txtDisplay);
            this.Controls.Add(this.txtPattern);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSearchIn);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Regular Expression Tester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearchIn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPattern;
        private System.Windows.Forms.TextBox txtDisplay;
        private System.Windows.Forms.Button btnExecute;
    }
}

