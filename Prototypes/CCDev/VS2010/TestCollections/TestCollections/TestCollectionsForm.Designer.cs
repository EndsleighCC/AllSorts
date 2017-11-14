namespace TestCollections
{
    partial class TestCollectionsForm
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
            this.btnLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInputData = new System.Windows.Forms.TextBox();
            this.txtOutputData = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtDeleteKey = new System.Windows.Forms.TextBox();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.btnChangeKey = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(68, 193);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Input Data";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(131, 367);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Output Data";
            // 
            // txtInputData
            // 
            this.txtInputData.Location = new System.Drawing.Point(12, 35);
            this.txtInputData.Multiline = true;
            this.txtInputData.Name = "txtInputData";
            this.txtInputData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInputData.Size = new System.Drawing.Size(330, 140);
            this.txtInputData.TabIndex = 5;
            this.txtInputData.WordWrap = false;
            // 
            // txtOutputData
            // 
            this.txtOutputData.Location = new System.Drawing.Point(12, 396);
            this.txtOutputData.Multiline = true;
            this.txtOutputData.Name = "txtOutputData";
            this.txtOutputData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutputData.Size = new System.Drawing.Size(330, 167);
            this.txtOutputData.TabIndex = 6;
            this.txtOutputData.WordWrap = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(28, 234);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtDeleteKey
            // 
            this.txtDeleteKey.Location = new System.Drawing.Point(134, 236);
            this.txtDeleteKey.Name = "txtDeleteKey";
            this.txtDeleteKey.Size = new System.Drawing.Size(178, 20);
            this.txtDeleteKey.TabIndex = 8;
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(134, 281);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(73, 20);
            this.txtFrom.TabIndex = 9;
            // 
            // btnChangeKey
            // 
            this.btnChangeKey.Location = new System.Drawing.Point(28, 278);
            this.btnChangeKey.Name = "btnChangeKey";
            this.btnChangeKey.Size = new System.Drawing.Size(75, 23);
            this.btnChangeKey.TabIndex = 10;
            this.btnChangeKey.Text = "Change Key";
            this.btnChangeKey.UseVisualStyleBackColor = true;
            this.btnChangeKey.Click += new System.EventHandler(this.btnChangeKey_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(213, 284);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "to";
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(242, 281);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(100, 20);
            this.txtTo.TabIndex = 12;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(206, 193);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // TestCollectionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 575);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtTo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnChangeKey);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.txtDeleteKey);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.txtOutputData);
            this.Controls.Add(this.txtInputData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoad);
            this.Name = "TestCollectionsForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInputData;
        private System.Windows.Forms.TextBox txtOutputData;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtDeleteKey;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Button btnChangeKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.Button btnClear;
    }
}

