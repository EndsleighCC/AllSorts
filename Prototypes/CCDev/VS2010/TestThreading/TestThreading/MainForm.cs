using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TestThreading
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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
                this.Invoke( updateOutputDelegate , outputText + " from another thread" ) ;
            }
        }

        void AsyncUpdateThread( Object threadData )
        {
            int sleepTimeMs = (int)threadData;
            Thread.Sleep(sleepTimeMs);
            UpdateOutput("This is different stuff");
        }
        
        private void btnExecute_Click(object sender, EventArgs e)
        {
            UpdateOutput("This is stuff");
            Thread updateThread = new Thread(new ParameterizedThreadStart(AsyncUpdateThread));
            updateThread.Start(System.Convert.ToInt32(txtInput.Text));
        }
    }
}
