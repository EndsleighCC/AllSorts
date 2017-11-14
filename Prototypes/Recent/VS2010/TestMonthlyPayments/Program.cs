//**********************************************************************
//        COPYRIGHT ENDSLEIGH INSURANCE SERVICES LIMITED 2015
//**********************************************************************
//   PROJECT         :   PErsonal
//   LANGUAGE        :   C#
//   FILENAME        :   Program.cs
//   ENVIRONMENT     :   Microsoft Visual Studio
//**********************************************************************
//   FILE FUNCTION   :   Main source for a prototype application for
//                       calculating payment plan details
//   EXECUTABLE TYPE :   EXE
//   SPECIFICATION   :   None
//
//   RELATED DOCUMENTATION : None
//
//**********************************************************************
//   ABSTRACT        :   Traditionally, the calculation of payment plans
//                       has been liberally "sprinkled" through various
//                       sources around the system. This application
//                       is a prototype for the proper and possibly
//                       single location for payment plan calculations.
//                       
//   AUTHOR          :   C. Cornelius        CREATION DATE : 23-Jun-2015
//
//**********************************************************************
//   BUILD INFORMATION   :   Endsleigh Build System
//   EXECUTABLE NAME     :   TestMonthlyPayments.exe
//   MAIN ENTRY POINTS   :   Main
//
//   EVENTS              :   None
//
//**********************************************************************
//   PVCS SECTION:
//   ~~~~~~~~~~~~~
//   PVCS FILENAME: $Logfile$
//   PVCS REVISION: $Revision$
//
//   $Log$
// 
//**********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMonthlyPayments
{
    class Program
    {
        public class PaymentCoverSection : IComparable<PaymentCoverSection>
        {
            #region Constructors

            public PaymentCoverSection(string coverSectionCode, double premium)
            {
                _coverSectionCode = coverSectionCode;
                _premium = premium;
            }

            #endregion Constructors

            #region Public members

            public string Code
            {
                get { return _coverSectionCode; }
            }

            public double Premium
            {
                get { return _premium; }
            }

            public void SetAdditionalCoverSectionMonthlyPaymentPlanDepositDetails(
                            int installmentCount, double schemeResultTotalDeposit, double schemeResultTotalPremium)
            {
                _installmentCount = installmentCount;
                _uncorrectedDeposit = NumericalOperation.PennyRound(schemeResultTotalDeposit * _premium / schemeResultTotalPremium);
                _depositCorrection = 0.0;
            }

            public double UncorrectedDeposit
            {
                set { _uncorrectedDeposit = value; }
                get { return _uncorrectedDeposit; }
            }

            public double DepositCorrection
            {
                set { _depositCorrection = value; }
                get { return _depositCorrection; }
            }

            public double Deposit
            {
                get { return _uncorrectedDeposit + _depositCorrection; }
            }

            public int InstallmentCount
            {
                set { _installmentCount = value; }
                get { return _installmentCount; }
            }

            /// <summary>
            /// Calculate the monthly payment by an Additional Cover Section
            /// </summary>
            /// <param name="schemeResultMonthlyPayment">The total value of a single monthly payment including that for the Primary Cover Section</param>
            /// <param name="schemeResultTotalPremium">The total value of all the Cover Sections including the Primary Cover Section</param>
            public void SetAdditionalCoverSectionMonthlyPaymentDetails(double schemeResultMonthlyPayment, double schemeResultTotalPremium)
            {
                _uncorrectedMonthlyPayment =
                    NumericalOperation.PennyRound(schemeResultMonthlyPayment * _premium / schemeResultTotalPremium);
                // Reset
                _monthlyPaymentCorrection = 0.0;
            }

            /// <summary>
            /// The value of monthly payment that does not allow for the
            /// indivisibility of the total payment by the number of payments
            /// </summary>
            public double UncorrectedMonthlyPayment
            {
                set { _uncorrectedMonthlyPayment = value; }
                get { return _uncorrectedMonthlyPayment; }
            }

            /// <summary>
            /// The value that must be added to each monthly payment to get the total payment
            /// closer to the actual required payment. The two may not be the same due to
            /// rounding or truncation
            /// </summary>
            public double MonthlyPaymentCorrection
            {
                set { _monthlyPaymentCorrection = value; }
                get { return _monthlyPaymentCorrection; }
            }

            public double MonthlyPayment
            {
                get { return _uncorrectedMonthlyPayment + _monthlyPaymentCorrection; }
            }

            /// <summary>
            /// Calculate the total annual payment
            /// </summary>
            public double AnnualPayment
            {
                get { return MonthlyPayment * _installmentCount ; }
            }

            /// <summary>
            /// Definition of IComparable<PaymentCoverSection>
            /// </summary>
            /// <param name="otherPaymentCoverSection">
            /// The Cover Section with which the Code of "this" Cover Section should be compared
            /// </param>
            /// <returns></returns>
            public int CompareTo(PaymentCoverSection otherPaymentCoverSection)
            {
                return String.Compare(this.Code, otherPaymentCoverSection.Code, /* ignore case */ true);
            }

            #endregion Public members

            #region Private members

            private string _coverSectionCode = null;
            private double _premium = 0.0;

            private int _installmentCount = 0;
            private double _uncorrectedDeposit = 0.0;
            private double _depositCorrection = 0.0;

            private double _uncorrectedMonthlyPayment = 0.0;
            private double _monthlyPaymentCorrection = 0.0;

            #endregion

        } // PaymentCoverSection

        /// <summary>
        /// A Sorted Set of Cover Section Objects that will be ordered according to the
        /// PaymentCoverSection.IComparable interface i.e. in increasing order of Cover Section Code
        /// </summary>
        public class PaymentCoverSectionContainer : SortedSet<PaymentCoverSection>
        {
            public PaymentCoverSectionContainer(): base()
            {
            }
        }

        public class PaymentSchemeResult
        {
            #region Constructors

            public PaymentSchemeResult(string schemeCode, PaymentCoverSection primaryPaymentCoverSection)
            {
                _schemeCode = schemeCode;
                _primaryPaymentCoverSection = primaryPaymentCoverSection;
            }

            #endregion Constructors

            #region Public members

            public string Code
            {
                get { return _schemeCode; }
            }

            public PaymentCoverSection PrimaryPaymentCoverSection { get { return _primaryPaymentCoverSection; }}

            public void AddCoverSection(PaymentCoverSection paymentCoverSection)
            {
                // Make it clear that a Cover Section can only appear once
                if (_additionalPaymentCoverSectionContainer.Contains(paymentCoverSection))
                {
                    throw new ArgumentException(String.Format("The Cover Section Collection already contains a Cover Section with Code \"{0}\"",paymentCoverSection.Code));
                }
                else
                {
                    _additionalPaymentCoverSectionContainer.Add(paymentCoverSection);
                }
            }

            public PaymentCoverSectionContainer AdditionalPaymentCoverSectionContainer
            {
                get { return _additionalPaymentCoverSectionContainer; }
                set { _additionalPaymentCoverSectionContainer = value; }
            }

            public PaymentCoverSection AdditionalCoverSection(string code)
            {
                return _additionalPaymentCoverSectionContainer.Where( cs => String.Compare(cs.Code,code,/*ignore case*/ true) == 0 ).FirstOrDefault();
            }

            public double TotalPremium
            {
                get { return _primaryPaymentCoverSection.Premium + TotalAdditionalCoverSectionPremium; }
            }

            public double TotalAdditionalCoverSectionPremium
            {
                get
                {
                    double totalAdditionalCoverSectionPremium = _additionalPaymentCoverSectionContainer.Sum(cs => cs.Premium);
                    return totalAdditionalCoverSectionPremium;
                }
            }

            public double TotalAdditionalCoverSectionMonthlyPayments
            {
                get
                {
                    double totalAdditionalCoverSectionMonthlyPayments = _additionalPaymentCoverSectionContainer.Sum(cs => cs.MonthlyPayment);
                    return totalAdditionalCoverSectionMonthlyPayments;
                }
            }

            public double TotalAdditionalCoverSectionAnnualPayments
            {
                get
                {
                    double totalAdditionalCoverSectionAnnualPayments = _additionalPaymentCoverSectionContainer.Sum(cs => cs.AnnualPayment);
                    return totalAdditionalCoverSectionAnnualPayments;
                }
            }

            public double TotalAnnualPayment 
            {
                get
                {
                    double totalAnnualPayment = PrimaryPaymentCoverSection.AnnualPayment + TotalAdditionalCoverSectionAnnualPayments;
                    return totalAnnualPayment;
                }
            }

            public double TotalAdditionalCoverSectionDeposits
            {
                get
                {
                    double totalAdditionalCoverSectionDeposits = _additionalPaymentCoverSectionContainer.Sum(cs => cs.Deposit);
                    return totalAdditionalCoverSectionDeposits;
                }
            }

            public double DepositPercentage
            {
                get { return _depositPercentage; }
            }

            public double DepositFixedAmount
            {
                get { return _depositFixedAmount; }
            }

            public double Deposit
            {
                get
                {
                    double deposit = PrimaryPaymentCoverSection.Deposit + TotalAdditionalCoverSectionDeposits;
                    return deposit ;
                }
            }

            public double MonthlyPayment
            {
                get { return _monthlyPayment; }
            }

            public int InstallmentCount
            {
                get { return _installmentCount;  }
            }

            public double TotalCalculatedLoan
            {
                get { return _totalCalculatedLoan; }
            }

            public double TotalOfDepositAndAnnualPayments
            {
                get
                {
                    double total = Deposit + TotalAnnualPayment;
                    return total ;
                }
            }

            /// <summary>
            /// A Property available to check the difference between the calculated loan
            /// and the total of the actual payments.
            /// A positive value means that the total of the payments are greater than the loan calculation.
            /// A negative value means that the total of the payments are less than the loan calculation.
            /// </summary>
            public double TotalPaymentDifferenceFromCalculatedLoan 
            {
                get { return _totalPaymentDifferenceFromCalculatedLoan; }
            }

            /// <summary>
            /// The correction that must be make to the deposit to ensure that the sum
            /// of payments on all Cover Section (including the Primary Cover Section)
            /// plus deposit is equal to the total loan.
            /// This is necessary because of the rounding/truncation that occurs on the penny
            /// values on each Cover Section.
            /// </summary>
            public double MultiplePaymentDifference
            {
                get { return _multiplePaymentDifference; }
                set { _multiplePaymentDifference = value; }
            }

            public void CalculateMonthlyPaymentPremiums(bool depositRequired, int installmentCount)
            {
                // Simple Endsleigh style payment validation
                if ( depositRequired )
                {
                    // New Business

                    if (installmentCount != 11)
                    {
                        throw new ArgumentException(String.Format("Cases where a deposit is required must be New Business and have 11 payments"));
                    }

                } // New Business
                else
                {
                    // Renewals

                    if (installmentCount != 12)
                    {
                        throw new ArgumentException(String.Format("Cases where a deposit is NOT required must be Renewals and have 12 payments"));
                    }

                } // Renewals

                _installmentCount = installmentCount;

                if (depositRequired)
                {
                    // Standard deposit calculation for New Business
                    double depositTotal =
                        NumericalOperation.PennyTruncate(TotalPremium*
                                                         (_depositPercentage/NumericalOperation.PercentageDivisor)) + _depositFixedAmount;
                    foreach (PaymentCoverSection additionalCoverSection in _additionalPaymentCoverSectionContainer)
                    {
                        additionalCoverSection.SetAdditionalCoverSectionMonthlyPaymentPlanDepositDetails(installmentCount,depositTotal,TotalPremium);
                    }

                    PrimaryPaymentCoverSection.InstallmentCount = installmentCount;
                    // Use the remainder as the Primary Cover Section Deposit
                    PrimaryPaymentCoverSection.UncorrectedDeposit = depositTotal - TotalAdditionalCoverSectionDeposits;
                    PrimaryPaymentCoverSection.DepositCorrection = 0.0;

                } // Standard deposit calculation for New Business
                else
                {
                    // No deposit at Renewal
                    foreach (PaymentCoverSection additionalCoverSection in _additionalPaymentCoverSectionContainer)
                    {
                        additionalCoverSection.SetAdditionalCoverSectionMonthlyPaymentPlanDepositDetails(installmentCount, 0.0, TotalPremium);
                    }

                    PrimaryPaymentCoverSection.InstallmentCount = installmentCount;
                    PrimaryPaymentCoverSection.UncorrectedDeposit = 0.0;
                    PrimaryPaymentCoverSection.DepositCorrection = 0.0;
                }

                double loanPrincipal = TotalPremium - Deposit;
                _totalCalculatedLoan = NumericalOperation.PennyRound(loanPrincipal * (1.0 + _interestPercentage / NumericalOperation.PercentageDivisor));

                // Round the monthly payment
                // Note that due to the necessary rounding to a penny the total of all the payments may not equal exactly the calculated loan
                _monthlyPayment = NumericalOperation.PennyRound(_totalCalculatedLoan / installmentCount);

                // The actual amount paid
                double monthlyPaymentTotal = _monthlyPayment*installmentCount;

                // Retain the difference between the total of the payments and the loan calculation
                _totalPaymentDifferenceFromCalculatedLoan = _totalCalculatedLoan - monthlyPaymentTotal ;

                if ( depositRequired )
                {
                    // When there is a non-zero deposit correct the total payment
                    // to equal the calculated loan by changing the value of the deposit
                    PrimaryPaymentCoverSection.DepositCorrection = _totalPaymentDifferenceFromCalculatedLoan;
                }

                // Distribute the payments across the Cover Sections according to the premium for each Cover Section
                foreach (PaymentCoverSection additionalCoverSection in _additionalPaymentCoverSectionContainer)
                {
                    additionalCoverSection.SetAdditionalCoverSectionMonthlyPaymentDetails(_monthlyPayment,TotalPremium);
                }
                // Use the remainder as the Primary Cover Section Monthly Payment
                PrimaryPaymentCoverSection.UncorrectedMonthlyPayment = _monthlyPayment - TotalAdditionalCoverSectionMonthlyPayments;

                // Calculate the difference between the total of the penny rounded or truncated payments and what the total payments should be really
                // This should be very close to zero
                double multiplePaymentDifference = monthlyPaymentTotal - TotalAnnualPayment;
                _multiplePaymentDifference = multiplePaymentDifference;

            } // CalculateMonthlyPaymentPremiums

            public void Test(string testName , bool depositRequired, int installmentCount)
            {
                this.CalculateMonthlyPaymentPremiums(depositRequired, installmentCount);

                Console.WriteLine();
                string title = String.Format("Test is for \"{0}\"", testName);
                Console.WriteLine(title);
                Console.WriteLine(new String('~',title.Length));
                Console.WriteLine();
                Console.WriteLine("Total premium = {0}", this.TotalPremium.ToString("F2"));
                Console.WriteLine("Number of instalments = {0}", this.InstallmentCount);
                Console.WriteLine("Deposit Percentage = {0}%", this.DepositPercentage.ToString("F14"));
                Console.WriteLine("Deposit Fixed Amount = {0}", this.DepositFixedAmount.ToString("F2"));
                Console.WriteLine("Deposit Amount = {0}", this.Deposit.ToString("F2"));
                Console.WriteLine("Total calculated loan = {0}", this.TotalCalculatedLoan.ToString("F2"));
                Console.WriteLine("Monthly payment = {0}", this.MonthlyPayment.ToString("F2"));
                Console.WriteLine("Total payments = {0}", this.MonthlyPayment * this.InstallmentCount);
                Console.WriteLine("Total Cover Section Annual Payments = {0}", this.TotalAnnualPayment.ToString("F2"));
                Console.WriteLine("Total payments including deposit = {0}", (this.TotalAnnualPayment + this.Deposit).ToString("F2"));
                Console.WriteLine("Difference of Total payments (including deposit) and Premium is {0}",
                                    (this.TotalAnnualPayment + this.Deposit - this.TotalPremium).ToString("F2"));
                Console.WriteLine();
                Console.WriteLine("Primary Cover Section {0}", this.PrimaryPaymentCoverSection.Code);
                Console.WriteLine("    Full Premium = {0}",this.PrimaryPaymentCoverSection.Premium.ToString("F2"));
                Console.WriteLine("    Monthly payment = {0}, Monthly payment correction {1}",
                                            this.PrimaryPaymentCoverSection.MonthlyPayment.ToString("F2"),
                                            this.PrimaryPaymentCoverSection.MonthlyPaymentCorrection.ToString("F2"));
                Console.WriteLine("    Monthly deposit = {0}, Monthly deposit correction = {1}",
                                        this.PrimaryPaymentCoverSection.Deposit.ToString("F2"),
                                        this.PrimaryPaymentCoverSection.DepositCorrection.ToString("F2"));
                Console.WriteLine("Total payment difference from calculated loan = {0}", this.TotalPaymentDifferenceFromCalculatedLoan.ToString("F2"));
                if (Math.Abs(this.TotalPaymentDifferenceFromCalculatedLoan) < NumericalOperation.SmallPositive)
                {
                    Console.WriteLine("    *** The total of the payments agrees exactly with the calculated loan");
                }
                else if (this.TotalPaymentDifferenceFromCalculatedLoan < 0.0)
                {
                    Console.WriteLine("    +++ The total of the payments is greater than the calculated loan");
                }
                else
                {
                    Console.WriteLine("    --- The total of the payments is less than the calculated loan");
                }
                double totalCoverSectionPayment = this.PrimaryPaymentCoverSection.MonthlyPayment * installmentCount;
                foreach (PaymentCoverSection coverSection in this.AdditionalPaymentCoverSectionContainer)
                {
                    Console.WriteLine("    Cover Section {0} :", coverSection.Code);
                    Console.WriteLine("                  Full Premium = {0}",coverSection.Premium.ToString("F2"));
                    Console.WriteLine("                  Payment contribution per month = {0}", coverSection.MonthlyPayment.ToString("F2"));
                    Console.WriteLine("                  Deposit contribution per month = {0}", coverSection.Deposit.ToString("F2"));
                    totalCoverSectionPayment += coverSection.MonthlyPayment * installmentCount;
                }
                double totalPaymentsWithMultiplePaymentDifference = totalCoverSectionPayment + this.MultiplePaymentDifference;
                Console.WriteLine("Total Cover Section payment made = {0}. Difference = {1}. Total = {2}",
                                    totalCoverSectionPayment,
                                    this.MultiplePaymentDifference.ToString("F2"),
                                    totalPaymentsWithMultiplePaymentDifference.ToString("F2"));
                if (Math.Abs(totalPaymentsWithMultiplePaymentDifference - (this.MonthlyPayment * this.InstallmentCount)) < NumericalOperation.SmallPositive)
                {
                    Console.WriteLine("     Corrected total of Cover Section Payments agrees with total payments");
                }
                else
                {
                    Console.WriteLine("**** Corrected total of Cover Section Payments {0} does not agree with total payments {1}",
                        totalPaymentsWithMultiplePaymentDifference, this.MonthlyPayment * this.InstallmentCount);
                }
                if (Math.Abs(this.MultiplePaymentDifference) < NumericalOperation.SmallPositive)
                {
                    Console.WriteLine("    *** There is no discrepency between the total of the payments and what should be the payment total");
                }
                else if (this.MultiplePaymentDifference < 0.0)
                {
                    Console.WriteLine("    --- The total of the payments is less than what should be the payment total by {0}",
                        this.MultiplePaymentDifference.ToString("F2"));
                }
                else
                {
                    Console.WriteLine("    +++ The total of the payments is greater than what should be the payment total by {0}",
                        this.MultiplePaymentDifference.ToString("F2"));
                }
                Console.WriteLine("Total Cover Section payment plus deposit = {0}", this.TotalOfDepositAndAnnualPayments);
                Console.WriteLine("Total excess cost of product is {0}", (this.TotalOfDepositAndAnnualPayments - this.TotalPremium).ToString("F2"));
                // It is necessary to add the difference to the deposit because the deposit has already been corrected to include it
                double paymentDifferenceFromCalc = 0.0;
                if (this.Deposit > NumericalOperation.SmallPositive)
                {
                    // When the deposit is non-zero it will have been corrected by the difference from the calculated loan
                    paymentDifferenceFromCalc = this.TotalOfDepositAndAnnualPayments -
                                                (this.TotalCalculatedLoan + this.Deposit - this.TotalPaymentDifferenceFromCalculatedLoan);
                }
                else
                {
                    // When the deposit is zero it will NOT have been corrected by the difference from the calculated loan
                    paymentDifferenceFromCalc = this.TotalOfDepositAndAnnualPayments - (this.TotalCalculatedLoan + this.Deposit);
                }
                if ((Math.Abs(paymentDifferenceFromCalc)) < NumericalOperation.SmallPositive)
                {
                    Console.WriteLine("    *** Payments+Deposit is more or less the same as CalculatedLoan+Deposit");
                }
                else if (paymentDifferenceFromCalc < 0.0)
                {
                    Console.WriteLine("    --- Payments+Deposit is less than CalculatedLoan+Deposit by {0}", Math.Abs(paymentDifferenceFromCalc).ToString("F2"));
                }
                else
                {
                    Console.WriteLine("    +++ Payments+Deposit is greater than CalculatedLoan+Deposit by {0}", Math.Abs(paymentDifferenceFromCalc).ToString("F2"));
                }
                
            }

            #endregion Public members

            #region Private members

            private const double _depositPercentage = 100.0/12.0; // 8.3 recurring %
            private const double _depositFixedAmount = 6.0; // Pounds Sterling

            private double _monthlyPayment = 0.0;
            private int _installmentCount = 0;

            // The difference between the calculated total repayment and the total of the Cover Section payments 
            private double _multiplePaymentDifference = 0.0;

            private double _totalCalculatedLoan = 0.0;
            private double _totalPaymentDifferenceFromCalculatedLoan = 0.0;

            private const double _interestPercentage = 9.0;

            private string _schemeCode = null; // Supplied by the caller at Construction
            private PaymentCoverSection _primaryPaymentCoverSection = null; // Supplied by the caller at Construction

            private PaymentCoverSectionContainer _additionalPaymentCoverSectionContainer = new PaymentCoverSectionContainer();

            #endregion Private members

        } // PaymentSchemeResult

        public static void Test(string testName, PaymentSchemeResult paymentSchemeResult)
        {
            try
            {
                paymentSchemeResult.AddCoverSection(new PaymentCoverSection("COV1", 10.0));
                paymentSchemeResult.AddCoverSection(new PaymentCoverSection("COV2", 20.0));
                paymentSchemeResult.AddCoverSection(new PaymentCoverSection("COV3", 30.0));
                paymentSchemeResult.AddCoverSection(new PaymentCoverSection("COV4", 90.0));

                Console.WriteLine();
                Console.WriteLine("Scheme {0} : Premium {1}", paymentSchemeResult.Code, paymentSchemeResult.TotalPremium.ToString("F2"));
                Console.WriteLine();
                Console.WriteLine("Primary Cover Section {0} : Full Premium = {1}", paymentSchemeResult.PrimaryPaymentCoverSection.Code, paymentSchemeResult.PrimaryPaymentCoverSection.Premium);
                foreach (PaymentCoverSection coverSectionOriginal in paymentSchemeResult.AdditionalPaymentCoverSectionContainer)
                {
                    Console.WriteLine("    Additional Cover Section {0} : Full Premium {1}", coverSectionOriginal.Code, coverSectionOriginal.Premium);
                }

                // New Business always has 11 equal payments
                paymentSchemeResult.Test(testName + " : New Business", true, 11);
                // Renewals always has 12 equal payments
                paymentSchemeResult.Test(testName + " : Renewal", false, 12);

            }
            catch (ArgumentException eek)
            {
                // A Duplicate Cover Section (Code) Exception
                Console.WriteLine(eek.Message);
            }
            catch (Exception eek)
            {
                // General Exception
                Console.WriteLine("Unexpected exception \"{0}\"", eek.Message);
            }
        }

        static void Main(string[] args)
        {
            PaymentSchemeResult paymentSchemeResultWithPrimary = new PaymentSchemeResult("XYZ", new PaymentCoverSection("PRIMARY", 500.0));
            Test("Normal Primary Cover Section",paymentSchemeResultWithPrimary);

            PaymentSchemeResult paymentSchemeResultWithZeroPrimary = new PaymentSchemeResult("TUV", new PaymentCoverSection("PRIMARY", 0.0));
            Test("Zero Premium Primary Cover Section",paymentSchemeResultWithZeroPrimary);
        }
    }
}
