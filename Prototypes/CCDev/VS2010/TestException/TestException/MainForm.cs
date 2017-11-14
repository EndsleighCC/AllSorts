using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace TestException
{
    public partial class frmMainForm : Form
    {
        public frmMainForm()
        {
            InitializeComponent();
        }

        public class MyException : Exception
        {
            public MyException(string errorMessage) : base( errorMessage )
            {
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    throw new MyException("Nothing here");
                }
                catch (Exception eek)
                {
                    Debug.WriteLine(String.Format("Inner got an Exception \"{0}\"", eek.ToString()));
                    throw;
                }
            }
            catch (Exception eek)
            {
                Debug.WriteLine(String.Format("Outer got an Exception \"{0}\"",eek.ToString()));
            }
        }
    }
}
