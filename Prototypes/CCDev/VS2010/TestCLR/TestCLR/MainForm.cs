using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using TestCLRCommon;
using TestCLRCPlusPlus;

namespace TestCLR
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void DisplayResults( TestCLRCommonOutput testCLRCommonOutput)
        {
            txtResults.Text += "Result is " + testCLRCommonOutput.Result.ToString() + Environment.NewLine;
            foreach (string text in testCLRCommonOutput.DescriptionData)
            {
                txtResults.Text += text + Environment.NewLine;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtEnter.Text))
            {
                int enteredValue = System.Convert.ToInt32(txtEnter.Text);

                TestCLRCommonClass testCLRCommonClass = new TestCLRCommonClass(enteredValue);

                TestCLRCPlusPlusClass testCLRCPlusPlusClass = new TestCLRCPlusPlusClass();

                txtExit.Text = testCLRCPlusPlusClass.Multiply(testCLRCommonClass, 5).ToString();

                txtMessage.Text = testCLRCPlusPlusClass.Message();

                TestCLRCommonInput testCLRCommonInput = new TestCLRCommonInput(enteredValue, "crap");

                TestCLRCommonOutput testCLRCommonOutput = testCLRCPlusPlusClass.PerformFunction(testCLRCommonInput);

                txtResults.Clear();

                txtResults.Text += "From Function Return" + Environment.NewLine;
                DisplayResults(testCLRCommonOutput);

                TestCLRCommonOutput testCLRCommonOutputAsOut = null;
                testCLRCPlusPlusClass.PerformFunction(testCLRCommonInput, out testCLRCommonOutputAsOut);

                txtResults.Text += Environment.NewLine;
                txtResults.Text += "From Out Parameter" + Environment.NewLine;
                DisplayResults(testCLRCommonOutputAsOut);
            }
        }
    }
}
