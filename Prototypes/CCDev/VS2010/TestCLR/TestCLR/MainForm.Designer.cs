namespace TestCLR
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.txtEnter = new System.Windows.Forms.TextBox();
            this.txtExit = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Exit";
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(108, 459);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // txtEnter
            // 
            this.txtEnter.Location = new System.Drawing.Point(98, 35);
            this.txtEnter.Name = "txtEnter";
            this.txtEnter.Size = new System.Drawing.Size(147, 20);
            this.txtEnter.TabIndex = 3;
            // 
            // txtExit
            // 
            this.txtExit.Location = new System.Drawing.Point(98, 82);
            this.txtExit.Name = "txtExit";
            this.txtExit.Size = new System.Drawing.Size(147, 20);
            this.txtExit.TabIndex = 4;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(98, 128);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(147, 20);
            this.txtMessage.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Message";
            // 
            // txtResults
            // 
            this.txtResults.Location = new System.Drawing.Point(25, 180);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResults.Size = new System.Drawing.Size(251, 260);
            this.txtResults.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnExecute;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 507);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.txtExit);
            this.Controls.Add(this.txtEnter);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Test CLR";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TextBox txtEnter;
        private System.Windows.Forms.TextBox txtExit;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtResults;
    }
}

