using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TestMonthlyPayments
{
    class Program
    {
        public class CoverSection : IComparable<CoverSection>
        {
            #region Constructors

            public CoverSection(string coverSectionCode, double premium)
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

            /// <summary>
            /// Calculate the monthly payment on this Cover Section
            /// </summary>
            /// <param name="instalmentCount">
            /// The count of instalments
            /// </param>
            /// <param name="loanInterestPercentage">
            /// The loan interest in percent
            /// </param>
            /// <param name="nonDepositFraction">
            /// The fraction of the total PREMIUM loan NOT represented by the deposit
            /// i.e. 1 - ( deposit / [total PREMIUM] )
            /// </param>
            public void CalculateMonthlyPayment( int instalmentCount , double loanInterestPercentage , double nonDepositFraction )
            {
                _installmentCount = instalmentCount;
                _loanInterestPercentage = loanInterestPercentage;
                _monthlyPayment =
                    NumericalOperation.PennyRound(_premium*nonDepositFraction*(1.0+loanInterestPercentage/NumericalOperation.PercentageDivisor)/instalmentCount);
            }

            /// <summary>
            /// Monthly payment NOT including any multiple payment correction
            /// </summary>
            public double MonthlyPayment
            {
                get { return _monthlyPayment; }
            }

            /// <summary>
            /// Calculate the total annual payment INCLUDING any multiple payment correction
            /// </summary>
            public double AnnualPayment
            {
                get { return _monthlyPayment * _installmentCount ; }
            }

            /// <summary>
            /// Definition of IComparable<CoverSection>
            /// </summary>
            /// <param name="otherCoverSection">
            /// The Cover Section with which the Code of "this" Cover Section should be compared
            /// </param>
            /// <returns></returns>
            public int CompareTo(CoverSection otherCoverSection)
            {
                return String.Compare(this.Code, otherCoverSection.Code, /* ignore case */ true);
            }

            #endregion Public members

            #region Private members

            private string _coverSectionCode = null;
            private double _premium = 0.0;

            private int _installmentCount = 0;
            private double _loanInterestPercentage = 0.0;
            private double _monthlyPayment = 0.0;
            private double _multiplePaymentCorrection = 0.0;

            #endregion

        } // CoverSection

        /// <summary>
        /// A Sorted Set of Cover Section Objects that will be ordered according to the
        /// CoverSection.IComparable interface i.e. in increasing order of Cover Section Code
        /// </summary>
        public class CoverSectionContainer : SortedSet<CoverSection>
        {
            public CoverSectionContainer(): base()
            {
            }
        }

        public class SchemeResult
        {
            #region Constructors

            public SchemeResult(string schemeCode, CoverSection primaryCoverSection)
            {
                _schemeCode = schemeCode;
                _primaryCoverSection = primaryCoverSection;
            }

            #endregion Constructors

            #region Public members

            public string Code
            {
                get { return _schemeCode; }
            }

            public CoverSection PrimaryCoverSection { get { return _primaryCoverSection; }}

            public void AddCoverSection(CoverSection coverSection)
            {
                // Make it clear that a Cover Section can only appear once
                if (_additionalCoverSectionContainer.Contains(coverSection))
                {
                    throw new ArgumentException(String.Format("The Cover Section Collection already contains a Cover Section with Code \"{0}\"",coverSection.Code));
                }
                else
                {
                    _additionalCoverSectionContainer.Add(coverSection);
                }
            }

            public CoverSectionContainer AdditionalCoverSectionContainer
            {
                get { return _additionalCoverSectionContainer; }
                set { _additionalCoverSectionContainer = value; }
            }

            public CoverSection AdditionalCoverSection(string code)
            {
                return _additionalCoverSectionContainer.Where( cs => String.Compare(cs.Code,code,/*ignore case*/ true) == 0 ).FirstOrDefault();
            }

            public double TotalPremium
            {
                get { return _primaryCoverSection.Premium + TotalAdditionalCoverSectionPremium; }
            }

            public double TotalAdditionalCoverSectionPremium
            {
                get
                {
                    double totalAdditionalCoverSectionPremium = _additionalCoverSectionContainer.Sum(cs => cs.Premium);
                    return totalAdditionalCoverSectionPremium;
                }
            }

            public double TotalAdditionalCoverSectionMonthlyPayments
            {
                get
                {
                    double totalAdditionalCoverSectionMonthlyPayments = _additionalCoverSectionContainer.Sum(cs => cs.MonthlyPayment);
                    return totalAdditionalCoverSectionMonthlyPayments;
                }
            }

            public double TotalAnnualPayment 
            {
                get
                {
                    double totalAnnualPayment = PrimaryCoverSection.AnnualPayment + _additionalCoverSectionContainer.Sum(cs => cs.AnnualPayment);
                    return totalAnnualPayment;
                }
            }

            public double Deposit
            {
                get { return _deposit + _multiplePaymentCorrection; }
            }

            public double MonthlyPayment
            {
                get { return _monthlyPayment; }
            }

            public int InstalmentCount
            {
                get { return _instalmentCount;  }
            }

            public double TotalCalculatedLoan
            {
                get { return _totalCalculatedLoan; }
            }

            public double TotalPaymentDifferenceFromCalculatedLoan 
            {
                get { return _totalPaymentDifferenceFromCalculatedLoan; }
            }

            /// <summary>
            /// The correction that must be make to the deposit to ensure that the sum
            ///  of payments on all Cover Section (including the Primary Cover Section)
            /// plus deposit is equal to the total loan.
            /// This is necessary because of the rounding/truncation that occurs on the penny
            /// values on each Cover Section.
            /// </summary>
            public double MultiplePaymentCorrection
            {
                get { return _multiplePaymentCorrection; }
                set { _multiplePaymentCorrection = value; }
            }

            public void CalculateMonthlyPaymentPremiums(int instalmentCount)
            {
                _instalmentCount = instalmentCount;
                if (instalmentCount == 12 /* a year */)
                {
                    // No deposit at Renewal
                    _deposit = 0.0;
                }
                else
                {
                    // Standard deposit calculation
                    _deposit =
                        NumericalOperation.PennyTruncate(TotalPremium*
                                                         (_depositPercentage/NumericalOperation.PercentageDivisor)) + _depositFixed;
                }

                double remainingPrinciple = TotalPremium - _deposit;
                _totalCalculatedLoan = NumericalOperation.PennyRound(remainingPrinciple * (1.0 + _interestPercentage / NumericalOperation.PercentageDivisor));

                // Round the monthly payment
                // Note that due to the necessary rounding to a penny the total of all the payments may not equal exactly the calculated loan
                _monthlyPayment = NumericalOperation.PennyRound(_totalCalculatedLoan / instalmentCount);

                // The actual amount paid
                double monthlyPaymentTotal = _monthlyPayment*instalmentCount;

                // Check the difference between the total of the payments and the loan calculation
                _totalPaymentDifferenceFromCalculatedLoan = monthlyPaymentTotal - _totalCalculatedLoan;

                // Adjust the payments according to the deposit
                double nonDepositFraction = 1.0 - _deposit/TotalPremium;
                PrimaryCoverSection.CalculateMonthlyPayment(instalmentCount, _interestPercentage, nonDepositFraction);
                SetAdditionalCoverSectionPayments(instalmentCount, nonDepositFraction);

                // Generate total annual payment values
                double totalAdditionalCoverSectionAnnualPayment = TotalAdditionalCoverSectionMonthlyPayments * instalmentCount;
                double totalPrimaryCoverSectionAnnualPayment = PrimaryCoverSection.MonthlyPayment*instalmentCount;
                // Calculate the difference between the total of the penny rounded payments and what the total should really be
                double multiplePaymentCorrection = monthlyPaymentTotal
                                                - (totalPrimaryCoverSectionAnnualPayment + totalAdditionalCoverSectionAnnualPayment);
                // Put into the Deposit the difference between the total of the individual payments and what the total should have been
                _multiplePaymentCorrection = multiplePaymentCorrection;

            } // CalculateMonthlyPaymentPremiums

            #endregion Public members

            #region Private members

            private void SetAdditionalCoverSectionPayments(int instalmentCount, double nonDepositFraction)
            {
                foreach (CoverSection coverSection in _additionalCoverSectionContainer)
                {
                    coverSection.CalculateMonthlyPayment(instalmentCount, _interestPercentage, nonDepositFraction);
                }
            }

            private const double _depositPercentage = 100.0/12.0;
            private const double _depositFixed = 6.0;

            private double _deposit = 0.0;
            private double _multiplePaymentCorrection = 0.0;
            private double _monthlyPayment = 0.0;
            private int _instalmentCount = 0;

            private double _totalCalculatedLoan = 0.0;
            private double _totalPaymentDifferenceFromCalculatedLoan = 0.0;

            private const double _interestPercentage = 9.0;

            private string _schemeCode = null; // Supplied by the caller at Construction
            private CoverSection _primaryCoverSection = null; // Supplied by the caller at Construction

            private CoverSectionContainer _additionalCoverSectionContainer = new CoverSectionContainer();

            #endregion Private members

        } // SchemeResult

        static void TestPayments(SchemeResult schemeResult , int instalmentCount)
        {
            schemeResult.CalculateMonthlyPaymentPremiums(instalmentCount);

            Console.WriteLine();
            Console.WriteLine("Total premium = {0}",schemeResult.TotalPremium.ToString("F2"));
            Console.WriteLine("Number of instalments = {0}", schemeResult.InstalmentCount);
            Console.WriteLine("Deposit = {0}", schemeResult.Deposit.ToString("F2"));
            Console.WriteLine("Total calculated loan = {0}", schemeResult.TotalCalculatedLoan.ToString("F2"));
            Console.WriteLine("Monthly payment = {0}", schemeResult.MonthlyPayment.ToString("F2"));
            Console.WriteLine("Total payments = {0}", schemeResult.MonthlyPayment * schemeResult.InstalmentCount);
            Console.WriteLine("Total Cover Section Annual Payments = {0}", schemeResult.TotalAnnualPayment.ToString("F2"));
            Console.WriteLine("Total payments including deposit = {0}", (schemeResult.TotalAnnualPayment + schemeResult.Deposit).ToString("F2"));
            Console.WriteLine("Difference of Total payments (including deposit) and Premium is {0}",
                                (schemeResult.TotalAnnualPayment + schemeResult.Deposit - schemeResult.TotalPremium).ToString("F2"));
            Console.WriteLine();
            Console.WriteLine("Primary Cover Section {0} : Full Premium = {1}, Payment per month = {2}",
                                    schemeResult.PrimaryCoverSection.Code,
                                    schemeResult.PrimaryCoverSection.Premium.ToString("F2"),
                                    schemeResult.PrimaryCoverSection.MonthlyPayment.ToString("F2"));
            Console.WriteLine("Total payment difference from calculated loan = {0}",schemeResult.TotalPaymentDifferenceFromCalculatedLoan.ToString("F2"));
            double totalCoverSectionPayment = schemeResult.PrimaryCoverSection.MonthlyPayment*instalmentCount;
            foreach (CoverSection coverSection in schemeResult.AdditionalCoverSectionContainer)
            {
                Console.WriteLine("    Cover Section {0} : Full Premium = {1}, Payment per month = {2}",
                                            coverSection.Code,
                                            coverSection.Premium.ToString("F2"),
                                            coverSection.MonthlyPayment.ToString("F2"));
                totalCoverSectionPayment += coverSection.MonthlyPayment * instalmentCount;
            }
            double totalPaymentsWithMultiplePaymentCorrection = totalCoverSectionPayment + schemeResult.MultiplePaymentCorrection;
            Console.WriteLine("Total Cover Section payment made = {0}. Correction = {1}. Total = {2}",
                                totalCoverSectionPayment,
                                schemeResult.MultiplePaymentCorrection.ToString("F2"),
                                totalPaymentsWithMultiplePaymentCorrection.ToString("F2"));
            if (Math.Abs(totalPaymentsWithMultiplePaymentCorrection - (schemeResult.MonthlyPayment * schemeResult.InstalmentCount)) < 1.0e-6)
            {
                Console.WriteLine("     Corrected total of Cover Section Payments agrees with total payments");
            }
            else
            {
                Console.WriteLine("**** Corrected total of Cover Section Payments {0} does not agree with total payments {1}",
                    totalPaymentsWithMultiplePaymentCorrection, schemeResult.MonthlyPayment * schemeResult.InstalmentCount);
            }
            Console.WriteLine("Total Cover Section payment plus deposit = {0}", schemeResult.Deposit + schemeResult.TotalAnnualPayment);

        } // TestPayments

        static void Main(string[] args)
        {
            SchemeResult schemeResult = new SchemeResult("ABC", new CoverSection("PRIMARY", 100.0));

            schemeResult.AddCoverSection(new CoverSection("COV1", 10.0));
            schemeResult.AddCoverSection(new CoverSection("COV2", 20.0));
            schemeResult.AddCoverSection(new CoverSection("COV3", 30.0));

            //// Test duplicate Cover Section exception
            //try
            //{
            //    schemeResult.AddCoverSection(new CoverSection("COV3", 40.0));
            //}
            //catch (Exception eek)
            //{
            //    Console.WriteLine(eek.Message);
            //}

            Console.WriteLine();
            Console.WriteLine("Scheme {0} : Premium {1}", schemeResult.Code, schemeResult.TotalPremium);
            Console.WriteLine();
            Console.WriteLine("Primary Cover Section {0} : Full Premium = {1}", schemeResult.PrimaryCoverSection.Code, schemeResult.PrimaryCoverSection.Premium);
            foreach (CoverSection coverSectionOriginal in schemeResult.AdditionalCoverSectionContainer)
            {
                Console.WriteLine("    Additional Cover Section {0} : Full Premium {1}",coverSectionOriginal.Code,coverSectionOriginal.Premium);
            }

            TestPayments(schemeResult , 11);
            TestPayments(schemeResult , 12);
        }
    }
}
