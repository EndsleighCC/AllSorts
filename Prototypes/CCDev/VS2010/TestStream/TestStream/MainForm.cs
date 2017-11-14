using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TestStream
{
    public partial class MainFormControl : Form
    {
        public MainFormControl()
        {
            InitializeComponent();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            using (StreamWriter streamWriter = new StreamWriter("Test.txt"))
            {
                streamWriter.WriteLine();
                streamWriter.WriteLine("{0} : Some text \"{1}\"", DateTime.Now, "An argument");
            }

        }
    }
}
