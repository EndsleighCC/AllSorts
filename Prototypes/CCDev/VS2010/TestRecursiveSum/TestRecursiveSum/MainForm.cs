using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestRecursiveSum
{
    public partial class frmMainForm : Form
    {
        public frmMainForm()
        {
            InitializeComponent();
        }

        int SumOfTermsFor( int n )
        {
            int sumNterms = 0;

            if ( n != 0 )
                sumNterms = n + SumOfTermsFor(n-1);

            return sumNterms;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (txtNumber.Text.Length > 0)
            {
                int highestNumber = System.Convert.ToInt32(txtNumber.Text);

                int sum = 0;

                for (int number = 0; number <= highestNumber; ++number)
                {
                    int sumForThisNumber = SumOfTermsFor(number);
                    sum += sumForThisNumber;
                }

                txtRecursiveSum.Text = System.Convert.ToString(sum);
            }
        }
    }
}
