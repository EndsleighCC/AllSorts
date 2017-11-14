namespace TestNumeric
{
    partial class frmMainForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.txtLeftOperand = new System.Windows.Forms.TextBox();
            this.txtPowerOf10 = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOperator = new System.Windows.Forms.TextBox();
            this.txtRightOperand = new System.Windows.Forms.TextBox();
            this.lblRightOperand = new System.Windows.Forms.Label();
            this.groupRoundingType = new System.Windows.Forms.GroupBox();
            this.radRoundUp = new System.Windows.Forms.RadioButton();
            this.radRoundNatural = new System.Windows.Forms.RadioButton();
            this.radRoundDown = new System.Windows.Forms.RadioButton();
            this.chkRoundAbsolute = new System.Windows.Forms.CheckBox();
            this.radRoundNone = new System.Windows.Forms.RadioButton();
            this.groupRoundingType.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Left operand";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 252);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Power of 10 rounding";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 304);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Result";
            // 
            // txtLeftOperand
            // 
            this.txtLeftOperand.Location = new System.Drawing.Point(49, 70);
            this.txtLeftOperand.Name = "txtLeftOperand";
            this.txtLeftOperand.Size = new System.Drawing.Size(100, 20);
            this.txtLeftOperand.TabIndex = 1;
            // 
            // txtPowerOf10
            // 
            this.txtPowerOf10.Location = new System.Drawing.Point(205, 249);
            this.txtPowerOf10.Name = "txtPowerOf10";
            this.txtPowerOf10.Size = new System.Drawing.Size(100, 20);
            this.txtPowerOf10.TabIndex = 4;
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(205, 301);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(100, 20);
            this.txtResult.TabIndex = 5;
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(167, 358);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 6;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(185, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Operator";
            // 
            // txtOperator
            // 
            this.txtOperator.Location = new System.Drawing.Point(167, 70);
            this.txtOperator.Name = "txtOperator";
            this.txtOperator.Size = new System.Drawing.Size(84, 20);
            this.txtOperator.TabIndex = 2;
            // 
            // txtRightOperand
            // 
            this.txtRightOperand.Location = new System.Drawing.Point(277, 70);
            this.txtRightOperand.Name = "txtRightOperand";
            this.txtRightOperand.Size = new System.Drawing.Size(100, 20);
            this.txtRightOperand.TabIndex = 3;
            // 
            // lblRightOperand
            // 
            this.lblRightOperand.AutoSize = true;
            this.lblRightOperand.Location = new System.Drawing.Point(292, 44);
            this.lblRightOperand.Name = "lblRightOperand";
            this.lblRightOperand.Size = new System.Drawing.Size(74, 13);
            this.lblRightOperand.TabIndex = 10;
            this.lblRightOperand.Text = "Right operand";
            // 
            // groupRoundingType
            // 
            this.groupRoundingType.Controls.Add(this.radRoundNone);
            this.groupRoundingType.Controls.Add(this.radRoundUp);
            this.groupRoundingType.Controls.Add(this.radRoundNatural);
            this.groupRoundingType.Controls.Add(this.radRoundDown);
            this.groupRoundingType.Location = new System.Drawing.Point(49, 104);
            this.groupRoundingType.Name = "groupRoundingType";
            this.groupRoundingType.Size = new System.Drawing.Size(328, 69);
            this.groupRoundingType.TabIndex = 11;
            this.groupRoundingType.TabStop = false;
            this.groupRoundingType.Text = "Type of rounding";
            // 
            // radRoundUp
            // 
            this.radRoundUp.AutoSize = true;
            this.radRoundUp.Location = new System.Drawing.Point(253, 30);
            this.radRoundUp.Name = "radRoundUp";
            this.radRoundUp.Size = new System.Drawing.Size(39, 17);
            this.radRoundUp.TabIndex = 3;
            this.radRoundUp.TabStop = true;
            this.radRoundUp.Text = "Up";
            this.radRoundUp.UseVisualStyleBackColor = true;
            // 
            // radRoundNatural
            // 
            this.radRoundNatural.AutoSize = true;
            this.radRoundNatural.Location = new System.Drawing.Point(168, 30);
            this.radRoundNatural.Name = "radRoundNatural";
            this.radRoundNatural.Size = new System.Drawing.Size(59, 17);
            this.radRoundNatural.TabIndex = 2;
            this.radRoundNatural.TabStop = true;
            this.radRoundNatural.Text = "Natural";
            this.radRoundNatural.UseVisualStyleBackColor = true;
            // 
            // radRoundDown
            // 
            this.radRoundDown.AutoSize = true;
            this.radRoundDown.Location = new System.Drawing.Point(85, 30);
            this.radRoundDown.Name = "radRoundDown";
            this.radRoundDown.Size = new System.Drawing.Size(53, 17);
            this.radRoundDown.TabIndex = 1;
            this.radRoundDown.TabStop = true;
            this.radRoundDown.Text = "Down";
            this.radRoundDown.UseVisualStyleBackColor = true;
            // 
            // chkRoundAbsolute
            // 
            this.chkRoundAbsolute.AutoSize = true;
            this.chkRoundAbsolute.Location = new System.Drawing.Point(150, 203);
            this.chkRoundAbsolute.Name = "chkRoundAbsolute";
            this.chkRoundAbsolute.Size = new System.Drawing.Size(101, 17);
            this.chkRoundAbsolute.TabIndex = 0;
            this.chkRoundAbsolute.Text = "Round absolute";
            this.chkRoundAbsolute.UseVisualStyleBackColor = true;
            // 
            // radRoundNone
            // 
            this.radRoundNone.AutoSize = true;
            this.radRoundNone.Location = new System.Drawing.Point(6, 30);
            this.radRoundNone.Name = "radRoundNone";
            this.radRoundNone.Size = new System.Drawing.Size(51, 17);
            this.radRoundNone.TabIndex = 0;
            this.radRoundNone.TabStop = true;
            this.radRoundNone.Text = "None";
            this.radRoundNone.UseVisualStyleBackColor = true;
            // 
            // frmMainForm
            // 
            this.AcceptButton = this.btnExecute;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 416);
            this.Controls.Add(this.chkRoundAbsolute);
            this.Controls.Add(this.groupRoundingType);
            this.Controls.Add(this.lblRightOperand);
            this.Controls.Add(this.txtRightOperand);
            this.Controls.Add(this.txtOperator);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtPowerOf10);
            this.Controls.Add(this.txtLeftOperand);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmMainForm";
            this.Text = "Test Numeric";
            this.groupRoundingType.ResumeLayout(false);
            this.groupRoundingType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLeftOperand;
        private System.Windows.Forms.TextBox txtPowerOf10;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOperator;
        private System.Windows.Forms.TextBox txtRightOperand;
        private System.Windows.Forms.Label lblRightOperand;
        private System.Windows.Forms.GroupBox groupRoundingType;
        private System.Windows.Forms.RadioButton radRoundNatural;
        private System.Windows.Forms.RadioButton radRoundDown;
        private System.Windows.Forms.RadioButton radRoundUp;
        private System.Windows.Forms.CheckBox chkRoundAbsolute;
        private System.Windows.Forms.RadioButton radRoundNone;
    }
}

