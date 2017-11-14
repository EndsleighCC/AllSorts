using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TestThreadingPool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        delegate void UpdateOutputThreadDelegate(int waitTimeMs , string outputText);

        void UpdateOutputThread(int waitTimeMs , string outputText)
        {
            Thread.Sleep(waitTimeMs);
            UpdateOutput(outputText);
        }

        delegate void UpdateOutputDelegate(string outputText);

        public void UpdateOutput(string outputText)
        {
            if (!txtOutput.InvokeRequired)
            {
                txtOutput.Text = outputText;
            }
            else
            {
                UpdateOutputDelegate updateOutputDelegate = new UpdateOutputDelegate(UpdateOutput);
                this.Invoke(updateOutputDelegate, outputText + " from another thread");
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            txtOutput.Text = txtInput.Text;
            UpdateOutputThreadDelegate updateOutputThreadDelegate = new UpdateOutputThreadDelegate(UpdateOutputThread);
            updateOutputThreadDelegate.BeginInvoke(System.Convert.ToInt32(txtInput.Text), "This is from the other thread", null, null);
        }
    }
}
