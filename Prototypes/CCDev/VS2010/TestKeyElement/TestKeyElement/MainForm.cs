using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestKeyElement
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            int intValue = 0;

            // Debug.WriteLine("Value is {0} but other value is {1}", 1.ToString(), 2.ToString());

            if (Int32.TryParse("17.10", out intValue))
                Console.WriteLine("Success");
            else
                Console.WriteLine("Failure");

            lblResult.Text = "Unknown";

            if (txtKeyElement0.Text == "")
                lblResult.Text = "Key Element 0 is empty";
            else if ( txtKeyElement1.Text == "" )
                lblResult.Text = "Key Element 1 is empty";
            else
            {
                // Neither are empty

                KeyCollection keyCollection0 = new KeyCollection(txtKeyElement0.Text);
                KeyCollection keyCollection1 = new KeyCollection(txtKeyElement1.Text);

                int compare0with1Result = keyCollection0.CompareTo(keyCollection1);
                if (compare0with1Result == 0)
                    lblResult.Text = "Equal to";
                else if (compare0with1Result < 0)
                    lblResult.Text = "Less Than";
                else
                    lblResult.Text = "Greater Than";

            } // Neither are empty

        }
    }
}
