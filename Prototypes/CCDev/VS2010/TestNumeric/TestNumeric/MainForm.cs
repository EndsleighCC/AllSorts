using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestNumeric
{
    public partial class frmMainForm : Form
    {
        public frmMainForm()
        {
            InitializeComponent();

            txtOperator.Text = "Add";
            txtPowerOf10.Text = "-2";
            radRoundNatural.Checked = true;
            chkRoundAbsolute.Checked = true;
        }

        private Enumerations.BinaryOperator GetBinaryOperator( string binaryOperator )
        {
            Enumerations.BinaryOperator binaryOp = Enumerations.BinaryOperator.Set;

            if (binaryOperatorMap.Keys.Contains(binaryOperator))
                binaryOp = binaryOperatorMap[binaryOperator];
            else
                binaryOp = Enumerations.BinaryOperator.Set;

            return binaryOp;
        }

        private Enumerations.RoundingStrategy GetRoundingStrategy()
        {
            Enumerations.RoundingStrategy roundingStrategy = Enumerations.RoundingStrategy.Natural;

            if ( radRoundNone.Checked )
                roundingStrategy = Enumerations.RoundingStrategy.None;
            else if (radRoundDown.Checked)
                roundingStrategy = Enumerations.RoundingStrategy.Down;
            else if ( radRoundNatural.Checked )
                roundingStrategy = Enumerations.RoundingStrategy.Natural;
            else
                roundingStrategy = Enumerations.RoundingStrategy.Up;

            return roundingStrategy;
        }

        private bool GetRoundAbsolute()
        {
            bool roundAbsolute = false;
            if (chkRoundAbsolute.Checked)
                roundAbsolute = true;
            return roundAbsolute;
        }

        private void TestDoubleExceptions()
        {
            double x = 1.0;
            double y = 0.0;

            double z = x/y;

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            TestDoubleExceptions();

            if ( String.IsNullOrEmpty( txtLeftOperand.Text ) || String.IsNullOrWhiteSpace( txtLeftOperand.Text ) )
                MessageBox.Show( "Left operand is empty" , _applicationName ) ;
            else if ( String.IsNullOrEmpty( txtOperator.Text ) || String.IsNullOrWhiteSpace( txtOperator.Text ) )
                MessageBox.Show( "Operator is empty" , _applicationName ) ;
            else if ( String.IsNullOrEmpty( txtRightOperand.Text ) || String.IsNullOrWhiteSpace( txtRightOperand.Text ) )
                MessageBox.Show( "Right operand is empty" , _applicationName ) ;
            else if ( String.IsNullOrEmpty( txtPowerOf10.Text ) || String.IsNullOrWhiteSpace( txtLeftOperand.Text ) )
                MessageBox.Show( "Rounding Power is empty" , _applicationName ) ;
            else
            {
                double doubleLeft = System.Convert.ToDouble( txtLeftOperand.Text ) ;
                Enumerations.BinaryOperator binaryOperator = GetBinaryOperator( txtOperator.Text ) ;
                double doubleRight = System.Convert.ToDouble( txtRightOperand.Text ) ;
                int roundAtPowerOf10 = System.Convert.ToInt32( txtPowerOf10.Text ) ;

                double doubleResult = NumericHelper.BinaryOperate(
                                            doubleLeft,
                                            binaryOperator,
                                            doubleRight,
                                            GetRoundingStrategy(),
                                            GetRoundAbsolute(),
                                            roundAtPowerOf10);

                txtResult.Text = System.Convert.ToString(doubleResult);

            }
        }

        [Serializable]
        public class StringOperatorIgnoreCaseSortedDictionaryType : SortedDictionary<string, Enumerations.BinaryOperator>
        {
            public StringOperatorIgnoreCaseSortedDictionaryType()
                : base(StringComparer.CurrentCultureIgnoreCase)
            {
            }
        }

        private readonly StringOperatorIgnoreCaseSortedDictionaryType binaryOperatorMap
            = new StringOperatorIgnoreCaseSortedDictionaryType { { "Add", Enumerations.BinaryOperator.Add } ,
                                                                 { "Subtract", Enumerations.BinaryOperator.Subtract } ,
                                                                 { "Multiply" , Enumerations.BinaryOperator.Multiply } ,
                                                                 { "Divide", Enumerations.BinaryOperator.Divide } };

        const string _applicationName = "Test Numeric";
    }
}
