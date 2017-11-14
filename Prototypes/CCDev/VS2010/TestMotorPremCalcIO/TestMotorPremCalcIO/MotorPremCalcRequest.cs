using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TestMotorPremCalcIO
{

    [DataContract]
    public class MotorPremCalcRequest // DRQ_PACE_IN
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MotorPremCalcRequest()
        {
            PremCalcBehaviouralParameters = new PremCalcBehaviouralParameters();
            CaseContextData = new CaseContextData();
            DiaryData = new DiaryData();
            ClientData = new ClientData();
            DriverDataCollection = new DriverDataCollection();
            VehicleDetails = new VehicleDetails();
            CoverDetails = new CoverDetails();
            Acceptance = new Acceptance();
            PremiumModellingData = new PremiumModellingData();
            ReferralData = new ReferralData();
            AdditionalUnderwritingData = new AdditionalUnderwritingData();
            AdditionalUnderwritingQuestionData = new AdditionalUnderwritingQuestionData();
        }

        #endregion Constructors

        #region DSOS_PREMCALC_PARAMS
        [DataMember]
        public PremCalcBehaviouralParameters PremCalcBehaviouralParameters { get; set; } // DSOS_PREMCALC_PARAMS
        #endregion

        #region RAWDATA

        [DataMember]
        public CaseContextData CaseContextData { get; set; } // CONTROL_SECTION

        [DataMember]
        public DiaryData DiaryData { get; set; } // DIARY_WORK_AREA

        [DataMember]
        public ClientData ClientData { get; set; } // CLIENT_DATA_CAPTURE

        [DataMember]
        public DriverDataCollection DriverDataCollection { get; set; }// DRIVER[]

        [DataMember]
        public VehicleDetails VehicleDetails { get; set; } // VEHICLE

        [DataMember]
        public CoverDetails CoverDetails { get; set; } // COVER_DETAILS

        [DataMember]
        public Acceptance Acceptance { get; set; } // ACCEPTANCE

        [DataMember]
        public PremiumModellingData PremiumModellingData { get; set; } // PREMIUM_MODELLING

        [DataMember]
        public ReferralData ReferralData { get; set; } // REFERRAL

        [DataMember]
        public AdditionalUnderwritingData AdditionalUnderwritingData { get; set; } // TMS_RETURN_DATA

        [DataMember]
        public AdditionalUnderwritingQuestionData AdditionalUnderwritingQuestionData { get; set; } // ADD_UW_QUESTIONS

        #endregion RAWDATA
    }

    public enum TriStateEnum
    {
        Unknown,
        Active,
        Inactive
    }

    public enum InputDataSourceEnum
    {
        Unknown,
        SalesBranch,
        ISeries,
        Web,
        Agent
    }

    public enum PremCalcPhaseEnum
    {
        None,
        PreAdjustment,
        PostAdjustment
    }

    [DataContract]
    public class PremCalcBehaviouralParameters // DSOS_PREMCALC_PARAMS
    {
        public PremCalcBehaviouralParameters()
        {
            SchemeList = new StringCollection();
        }

        // DSOS_PREMCALC_PARAMS::szInsurer[MQRD_CB_SCHEME_CODE + 1]
        [DataMember]
        public string SchemeOrSchemeListCode { get; set; }

        // DSOS_PREMCALC_PARAMS::scodlSchemeCodeList
        public StringCollection SchemeList { get; set; }

        // DSOS_PREMCALC_PARAMS::sVariant
        [DataMember]
        public bool IsVariant { get; set; }

        // DSOS_PREMCALC_PARAMS::sUWLevel
        [DataMember]
        public TriStateEnum IsSecondLevelUnderwriting { get; set; }

        // DSOS_PREMCALC_PARAMS::sReviewAddUWQ
        [DataMember]
        public bool ReviewAdditionalUnderwritingQuestions { get; set; }
    }

    [DataContract]
    public class StringCollection : Collection<string>
    {
        public StringCollection() : base()
        {
        }
    }

    [DataContract]
    public class CaseContextData // CONTROL_SECTION
    {
        public CaseContextData()
        {
        }

        // CONTROL_SECTION::edistInputSourceType
        [DataMember]
        public InputDataSourceEnum InputDataSource { get; set; }

        // CONTROL_SECTION::epcpPhase
        [DataMember]
        public PremCalcPhaseEnum PremCalcPhase { get; set; }

        // CONTROL_SECTION::szHostAppVersion
        [DataMember]
        public string RequestingSystemApplicationVersion { get; set; }

        // CONTROL_SECTION::centre_code[5]
        [DataMember]
        public string BranchCode { get; set; }

        // CONTROL_SECTION::trans_type[3]
        [DataMember]
        public string SystemTransactionCode { get; set; }

        // CONTROL_SECTION::dattimRequest
        [DataMember]
        public DateTime RequestDateTime { get; set; }

        // CONTROL_SECTION::blue_bag_trans_type
        // Whether Introductory NCD was given when the case was created
        [DataMember]
        public bool IntroductoryNcdGiven { get; set; }

        // CONTROL_SECTION::quote_no [SZ_QUOTE+1]
        [DataMember]
        public string QuoteNumber { get; set; }

        // CONTROL_SECTION::quote_base_quote_no [SZ_QUOTE+1]
        [DataMember]
        public string BaseQuoteNumber { get; set; }

        // CONTROL_SECTION::quote_scheme[ DSOS_CB_SCHEME_CODE_SIZE ]
        [DataMember]
        public string PreviousQuoteSchemeCode { get; set; }

        // CONTROL_SECTION::lPreviousQuoteDate
        [DataMember]
        public DateTime PreviousQuoteDateTime { get; set; }

        // CONTROL_SECTION::policy_bus_type ('C' or 'M')
        [DataMember]
        public string PolicyBusinessType { get; set; }

        // CONTROL_SECTION::policy_status
        // TODO: Investigate. Looks like it's always a space
        [DataMember]
        public string PolicyStatus { get; set; }

        // CONTROL_SECTION::client_code
        [DataMember]
        public string ClientNumber { get; set; }

        // CONTROL_SECTION::prem_calc_level
        [DataMember]
        public int OriginalNoClaimsDiscountYearsClaimed { get; set; }

        // CONTROL_SECTION::chQuoteType
        [DataMember]
        public string QuoteSystemTransactionTypeCode { get; set; }

        // CONTROL_SECTION::szCurrentScheme
        // Scheme Code of the Policy Number. For quotes that involved existing policies such as Adjustments and Renewals
        [DataMember]
        public string CurrentPolicySchemeCode { get; set; }

        // CONTROL_SECTION::szCurrentPolicyNum
        // Numeric part of Policy Number
        [DataMember]
        public string CurrentPolicyNumber { get; set; }

        // CONTROL_SECTION::chCurrentPremCredAccount
        [DataMember]
        public TriStateEnum HasCurrentPremiumCreditAccount { get; set; }

        // CONTROL_SECTION::chEligibleForPremCred
        [DataMember]
        public TriStateEnum IsEligibleForPremiumCredit { get; set; }

        // CONTROL_SECTION::RenewalDate
        [DataMember]
        public DateTime RenewalDateTime { get; set; }

        // CONTROL_SECTION::flSuspensionCreditAmount
        [DataMember]
        public double SuspensionCreditAmount { get; set; }

        // CONTROL_SECTION::chSpeculativeQuoteFlag
        [DataMember]
        public bool IsSpeculativeQuote { get; set; }

        // CONTROL_SECTION::StartOfCurrentTermDate
        [DataMember]
        public DateTime StartOfCurrentTermDateTime { get; set; }

        // CONTROL_SECTION::EndOfCurrentTermDate
        [DataMember]
        public DateTime EndOfCurrentTermDateTime { get; set; }

        // CONTROL_SECTION::OriginalInceptionDate
        [DataMember]
        public DateTime OriginalInceptionDate { get; set; }

        // CONTROL_SECTION::LastAdjustmentDate
        [DataMember]
        public DateTime LastAdjustmentDateTime { get; set; }

        // CONTROL_SECTION::chHostPDFlag
        [DataMember]
        public TriStateEnum HostPolicyHasProtectedDiscount { get; set; }

        // CONTROL_SECTION::chLongTermGreenCardFlag
        [DataMember]
        public TriStateEnum HostHasLongTermGreenCard { get; set; }

        // CONTROL_SECTION::flFAPAtLastAdjustment
        [DataMember]
        public double AnnualPremiumAtLastAdjustment { get; set; }

        // CONTROL_SECTION::szPreviousPolicyTermPaymentType[ DSOS_CB_PYMT_METHOD + 1 ]
        [DataMember]
        public string PreviousPolicyTermPaymentMethodCode { get; set; }

        // CONTROL_SECTION::dfPreviousPolicyTermCommissionRate
        [DataMember]
        public double PreviousPolicyTermCommissionRatePercent { get; set; }

        // CONTROL_SECTION::dfPreviousPolicyTermCalculatedInsurancePremium
        [DataMember]
        public double PreviousPolicyTermCalculatedInsurancePremium { get; set; }

        // CONTROL_SECTION::dfPreviousPolicyTermAdminFee
        [DataMember]
        public double PreviousPolicyTermAdministrationFee { get; set; }

        // CONTROL_SECTION::lPreviousPolicyDurationMonths
        [DataMember]
        public int PreviousPolicyDurationMonths { get; set; }

        // CONTROL_SECTION::szIntroducer[ OSM_CB_INTRODUCER_CODE_SIZE ]
        [DataMember]
        public string IntroducerCode { get; set; }

        // CONTROL_SECTION::szContactRoute[8+1]
        [DataMember]
        public string ContactRouteCode { get; set; }

        // CONTROL_SECTION::szContactSource[8+1]
        [DataMember]
        public string ContactSourceCode { get; set; }

        // CONTROL_SECTION::szMarketingCampaign[8+1]
        [DataMember]
        public string MarketingCampaignCode { get; set; }

        // CONTROL_SECTION::lAgentCategory
        [DataMember]
        public int AgentCategory { get; set; }

        // CONTROL_SECTION::fAgentIsAggregator
        [DataMember]
        public bool AgentIsAgregator { get; set; }

        // CONTROL_SECTION::dblRenewalPricingAdjustmentPercent
        [DataMember]
        public double RenewalPricingAdjustmentPercent { get; set; }

        // CONTROL_SECTION::lNewBusinessLeadTimeInDays
        [DataMember]
        public int NewBusinessLeadTimeDayCount { get; set; }

        // CONTROL_SECTION::dfExpiringPolicyAnnualisedCalculatedInsurancePremium
        [DataMember]
        public double ExpiringPolicyAnnualisedCalculatedInsurancePremium { get; set; }

        // CONTROL_SECTION::szCUEReference[ DSOS_CB_CUE_REFERENCE_SIZE ]
        [DataMember]
        public string ClaimsUnderwritingExchangeRefererence { get; set; }

        // CONTROL_SECTION::bIsRenewalSelect
        [DataMember]
        public bool IsRenewalSelect { get; set; }
    }

    [DataContract]
    public class DiaryData // DIARY_WORK_AREA
    {
        // DIARY_WORK_AREA::type[5]
        [DataMember]
        public string Type { get; set; }

        // DIARY_WORK_AREA::date
        [DataMember]
        public DateTime DateTime { get; set; }

        // DIARY_WORK_AREA::auto_act_date
        [DataMember]
        public DateTime AutoActivationDate { get; set; }

        // DIARY_WORK_AREA::note[71]
        [DataMember]
        public string Note { get; set; }
    }

    [DataContract]
    public class AddressLineCollection : Collection<string>
    {
        public AddressLineCollection()
            : base()
        {
        }
    }

    [DataContract]
    public class ClientData // CLIENT_DATA_CAPTURE
    {
        public ClientData()
        {
            ClientAddressLineCollection = new AddressLineCollection();
            RiskAddressLineCollection = new AddressLineCollection();
        }

        // CLIENT_DATA_CAPTURE::title [SZ_TITLE+1]
        [DataMember]
        public string Title { get; set; }

        // CLIENT_DATA_CAPTURE::forename [DSOS_CB_LONG_FORENAME+1]
        [DataMember]
        public string Forename { get; set; }

        // CLIENT_DATA_CAPTURE::initials [SZ_INITS+1]
        [DataMember]
        public string Initials { get; set; }

        // CLIENT_DATA_CAPTURE::surname [SZ_SNAME+1]
        [DataMember]
        public string Surname { get; set; }

        // CLIENT_DATA_CAPTURE::chGender
        [DataMember]
        public string GenderCode { get; set; }

        // CLIENT_DATA_CAPTURE::mar_status
        // TODO: PremCalc Code mapping from ItemList
        [DataMember]
        public string MaritalStatusCode { get; set; }

        // CLIENT_DATA_CAPTURE::DoB
        [DataMember]
        public DateTime DateOfBirth { get; set; }

        // CLIENT_DATA_CAPTURE::addr_lineX [SZ_ADDR1+1] * 4
        [DataMember]
        public AddressLineCollection ClientAddressLineCollection { get; set; }

        // CLIENT_DATA_CAPTURE::postcode [SZ_PCODE+1]
        [DataMember]
        public string ClientPostcode { get; set; }

        // CLIENT_DATA_CAPTURE::risk_addr_lineX [SZ_ADDR1+1] * 4
        [DataMember]
        public AddressLineCollection RiskAddressLineCollection { get; set; }

        // CLIENT_DATA_CAPTURE::risk_postcode [SZ_PCODE+1]
        [DataMember]
        public string RiskPostcode { get; set; }

        // CLIENT_DATA_CAPTURE::szEMailAddress[ OSM_EMAIL_ADDRESS_SIZE ]
        [DataMember]
        public string ClientEmailAddress { get; set; }

        // CLIENT_DATA_CAPTURE::code [SZ_CLIENTCODE+1]
        [DataMember]
        public string ClientNumber { get; set; }

        // CLIENT_DATA_CAPTURE::szEducationEstablishmentCode[DSOS_CB_EDU_ESTB_CODE+1]
        [DataMember]
        public string EducationEstablishmentCode { get; set; }

        // CLIENT_DATA_CAPTURE::grad_date
        [DataMember]
        public DateTime GraduationDate { get; set; }

        // CLIENT_DATA_CAPTURE::bad_debt_ind (used on the iSeries)
        [DataMember]
        public bool IsBadDebtRisk { get; set; }

        // CLIENT_DATA_CAPTURE::chGraduate
        [DataMember]
        public bool IsGraduate { get; set; }

        // CLIENT_DATA_CAPTURE::szEmployersBusinessCategory[DSOS_CB_BUSINESS_CATEGORY+1]
        [DataMember]
        public string EmployerBusinessCategory { get; set; }

        // CLIENT_DATA_CAPTURE::chFullTimeOccupationCategory
        [DataMember]
        public string FullTimeOccupationCategory { get; set; }

        // CLIENT_DATA_CAPTURE::szCourseStudiedCode[DSOS_CB_COURSE_CODE+1]
        [DataMember]
        public string CourseStudiedCode { get; set; }

        // CLIENT_DATA_CAPTURE::szNUSCardNumber[DSOS_CB_NUS_EXTRA_CARD+1]
        [DataMember]
        public string NusCardNumber { get; set; }

        // CLIENT_DATA_CAPTURE::bNUSExtraCardHeld
        [DataMember]
        public bool NusExtraCardHeld { get; set; }

        // CLIENT_DATA_CAPTURE::bStudentPossessionsPolicyEverHeld
        [DataMember]
        public bool StudentPossesssionsPolicyEverHeld { get; set; }

        // CLIENT_DATA_CAPTURE::szTeachingLocation[DSOS_CB_TEACHING_LOCATION+1]
        [DataMember]
        public string TeachingLocationCode { get; set; }

        // CLIENT_DATA_CAPTURE::szTeachingEstablishment[DSOS_CB_EDU_ESTB_CODE+1]
        [DataMember]
        public string TeachingEstablishmentCode { get; set; }

        // CLIENT_DATA_CAPTURE::szTeachingSubject[DSOS_CB_TEACHING_SUBJECT+1]
        [DataMember]
        public string TeachingSubjectCode { get; set; }

        // CLIENT_DATA_CAPTURE::chHasChildrenUnderAge16
        [DataMember]
        public bool HasChildrenUnderAge16 { get; set; }

        // CLIENT_DATA_CAPTURE::chAnyDriversWithCriminalConvictions
        [DataMember]
        public bool DriversExistWithCriminalConvictions { get; set; }

    }

    [DataContract]
    public class AccidentDetail // ACC_LOSS
    {
        // ACC_LOSS::date
        [DataMember]
        public string Type { get; set; }

        // ACC_LOSS::type[5]
        [DataMember]
        public DateTime Date { get; set; }

        // ACC_LOSS::time_ago (as a TimeSpan)
        public MotorUtility.LargeTimeSpan TimeAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date , coverStartDateOrPolicyStartDateOrAdjustmentstartDate );
        }

        // ACC_LOSS::time_ago (as months)
        public int MonthsAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date, coverStartDateOrPolicyStartDateOrAdjustmentstartDate).MonthCount;
        }

        // ACC_LOSS::damage_costs
        [DataMember]
        public int DamageCost { get; set; }

        // ACC_LOSS::inj_costs
        [DataMember]
        public int InjuryCost { get; set; }

        // ACC_LOSS::FB_inds[3]
        [DataMember]
        public string DaultBonusIndicator { get; set; }

    }

    [DataContract]
    public class AccidentDataCollection : Collection<AccidentDetail>
    {
        public AccidentDataCollection() : base()
        {
        }
    }

    [DataContract]
    public class ConvictionDetail // CONV_DETAILS
    {
        // CONV_DETAILS::type[ DSOS_CB_CONVICTION_CODE_SIZE ]
        [DataMember]
        public string Type { get; set; }

        // CONV_DETAILS::date
        [DataMember]
        public DateTime Date { get; set; }

        // CONV_DETAILS::chtype_ind
        [DataMember]
        public TriStateEnum Pending { get; set; }

        // CONV_DETAILS::time_ago
        public MotorUtility.LargeTimeSpan TimeAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date, coverStartDateOrPolicyStartDateOrAdjustmentstartDate);
        }

        // CONV_DETAILS::time_ago
        public int MonthsAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date, coverStartDateOrPolicyStartDateOrAdjustmentstartDate).MonthCount;
        }

        // CONV_DETAILS::lDvlaPoints
        [DataMember]
        public int DvlaPointCount { get; set; }

        // CONV_DETAILS::lFineAmountPounds
        [DataMember]
        public int FineAmountPounds { get; set; }

    }

    [DataContract]
    public class ConvictionDetailCollection : Collection<ConvictionDetail>
    {
        public ConvictionDetailCollection() : base()
        {
        }
    }

    [DataContract]
    public class BanDetail // BAN_DETAILS
    {
        // BAN_DETAILS::reason_type[ DSOS_CB_BAN_CODE_SIZE ]
        [DataMember]
        public string ReasonCode { get; set; }

        // BAN_DETAILS::start_date
        [DataMember]
        public DateTime Date { get; set; }

        // BAN_DETAILS::time_ago
        public MotorUtility.LargeTimeSpan TimeAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date, coverStartDateOrPolicyStartDateOrAdjustmentstartDate);
        }

        // BAN_DETAILS::time_ago
        public int MonthsAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date, coverStartDateOrPolicyStartDateOrAdjustmentstartDate).MonthCount;
        }

        // BAN_DETAILS::period
        [DataMember]
        public int DurationWeekCount { get; set; }
    }

    [DataContract]
    public class BanDetailCollection : Collection<BanDetail>
    {
        public BanDetailCollection() : base()
        {
        }
    }

    [DataContract]
    public class DisabilityDetail // DISABILITIES
    {
        // DISABILITIES::type[5]
        [DataMember]
        public string Type { get; set; }
    }

    [DataContract]
    public class DisabilityCollection : Collection<DisabilityDetail>
    {
        public DisabilityCollection() : base()
        {
        }
    }

    [DataContract]
    public class OccupationDetail
    {
        [DataMember]
        public string DSOSCode { get; set; }

        [DataMember]
        public string OSCACode { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Sector { get; set; }

        [DataMember]
        public string UnionCode { get; set; }
    }

    [DataContract]
    public class DriverData // DRIVER
    {
        public DriverData()
        {
            AccidentDataCollection = new AccidentDataCollection();
            ConvictionDetailCollection = new ConvictionDetailCollection();
            BanDetailCollection = new BanDetailCollection();
            DisabilityCollection = new DisabilityCollection();
            FullTimeOccupation = new OccupationDetail();
            PartTimeOccupation = new OccupationDetail();
        }

        // DRIVER::no
        // Driver Number 1 is the Proposer. There is no Driver zero i.e. this is not a Driver Index
        [DataMember]
        public int Number { get; set; }

        // DRIVER::status_ind
        // TODO: PremCalc Code mapping from ItemList Code
        [DataMember]
        public string StatusCode { get; set; }

        // DRIVER::dateDriverEffectiveDate
        [DataMember]
        public DateTime EffectiveDate { get; set; }

        // DRIVER::chGender
        // TODO: PremCalc Code mapping from ItemList Code
        [DataMember]
        public string GenderCode { get; set; }

        // DRIVER::title [SZ_TITLE+1]
        [DataMember]
        public string Title { get; set; }

        // DRIVER::forename [DSOS_CB_LONG_FORENAME+1]
        [DataMember]
        public string Forename { get; set; }

        // DRIVER::initials [SZ_INITS+1]
        [DataMember]
        public string Initials { get; set; }

        // DRIVER::surname [SZ_SNAME+1]
        [DataMember]
        public string Surname { get; set; }

        // DRIVER::mar_status
        // TODO: PremCalc Code mapping from ItemList Code
        [DataMember]
        public string MaritalStatusCode { get; set; }

        // DRIVER::spouse_ind
        [DataMember]
        public bool IsSpouseOfProposer { get; set; }

        // DRIVER::DoB
        [DataMember]
        public DateTime BirthDate { get; set; }

        // DRIVER::age_at_incept
        public int Age(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return MotorUtility.YearCountBetween(BirthDate, coverStartDateOrPolicyStartDateOrAdjustmentstartDate);
        }

        // DRIVER::lic_type[3]
        // TODO: PremCalc Code Mapping from ItemList Code
        [DataMember]
        public string LicenceTypeCode { get; set; }

        // DRIVER::lic_date
        // DRIVER::date_UK_test
        [DataMember]
        public DateTime LicenceDateTestPassed { get; set; }

        // DRIVER::per_lic_held
        // DRIVER::test_passed_ago
        public int TestPassedMonthsAgo( DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return MotorUtility.MonthCountBetween(coverStartDateOrPolicyStartDateOrAdjustmentstartDate,LicenceDateTestPassed);
        }

        // DRIVER::UK_res
        [DataMember]
        public int UkResidencePeriodMonths { get; set; }

        // DRIVER::achDSOS_FT_occn_code[ 3 ]
        // DRIVER::achDSOS_FT_occn_sector[ 3 ]
        // DRIVER::DSOS_FT_union [ 2 + 1 ]
        // DRIVER::DSOS_FT_occn_desc [SZ_OCCNDESC+1]
        // DRIVER::OSCA_FT_occn_code[3]
        [DataMember]
        public OccupationDetail FullTimeOccupation { get; set; }

        // DRIVER::DSOS_PT_occn_code [SZ_OCCNCODE+1]
        // DRIVER::DSOS_PT_occn_desc [SZ_OCCNDESC+1]
        // DRIVER::OSCA_PT_occn_code[3]
        [DataMember]
        public OccupationDetail PartTimeOccupation { get; set; }

        // DRIVER::commute_flag
        [DataMember]
        public TriStateEnum UsedForCommuting { get; set; }

        // DRIVER::chbus_use_flag
        [DataMember]
        public TriStateEnum BusinessUse { get; set; }

        // DRIVER::chprevious_decline_flag
        // TODO: PremCalc Code mapping from ItemList Code
        [DataMember]
        public string PreviousDriverDeclineReasonCode { get; set; }

        // DRIVER::chTermTimeResidence
        // TODO: PremCalc Code mapping from ItemList Code
        [DataMember]
        public string TermTimeResidenceCode { get; set; }

        // DRIVER::szUseOfOtherVehicles[ DSOS_CB_DRIVE_OTHER_VEHICLES_SIZE ]
        // TODO: PremCalc Code mapping from ItemList Code
        [DataMember]
        public string HasUseOFOtherVehiclesCode { get; set; }

        // DRIVER::chRelationshipToProposer
        // TODO: PremCalc Code mapping from ItemList Code
        [DataMember]
        public string RelationshipToProposerCode { get; set; }

        // DRIVER::szEmployerBusinessCategory[ DSOS_CB_BUSINESS_CATEGORY+1 ]
        // TODO: PremCalc Code mapping from ItemList Code
        [DataMember]
        public string EmployerBusinessCategoryCode { get; set; }

        #region Driver Collections

        // DRIVER::acc_loss[ DSOS_MAX_DRIVER_ACCIDENT_COUNT ]
        [DataMember]
        public AccidentDataCollection AccidentDataCollection { get; set; }

        // DRIVER::conv[ DSOS_MAX_DRIVER_CONVICTION_COUNT ]
        [DataMember]
        public ConvictionDetailCollection ConvictionDetailCollection { get; set; }

        // DRIVER::ban[ DSOS_MAX_DRIVER_BAN_COUNT ]
        [DataMember]
        public BanDetailCollection BanDetailCollection { get; set; }

        // DRIVER::disab[ DSOS_MAX_DRIVER_DISABILITY_COUNT ]
        [DataMember]
        public DisabilityCollection DisabilityCollection { get; set; }

        #endregion Driver Collections
    }

    [DataContract]
    public class DriverDataCollection : Collection<DriverData> // DRIVER[]
    {
        public DriverDataCollection() : base()
        {
        }

        // From ADD_UW_QUESTIONS::sMainDriverNumber
        public int MainDriverNumber { get; set; }
    }

    [DataContract]
    public class VehicleModification
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class VehicleModificationCollection : Collection<VehicleModification>
    {
        public VehicleModificationCollection() : base()
        {
        }
    }

    [DataContract]
    public class VehicleDetails // VEHICLE
    {
        public VehicleDetails()
        {
            VehicleModificationCollection = new VehicleModificationCollection();
            OwnedOrRegisteredTextCollection = new StringCollection();
        }

        // VEHICLE::code[9]
        [DataMember]
        public string AbiCode { get; set; }

        // VEHICLE::make[15]
        [DataMember]
        public string MakeDescription { get; set; }

        // VEHICLE::model[11]
        [DataMember]
        public string ShortModelDescription { get; set; }

        // VEHICLE::szFullVehicleDesc[30]
        [DataMember]
        public string FullModelDescription { get; set; }

        #region DRQ_VEHICLE_CLASS_DETAILS

        // VEHICLE::szClass[5] => DRQ_VEHICLE_CLASS_DETAILS

        // DRQ_VEHICLE_CLASS_DETAILS::chDoorCountDigit
        [DataMember]
        public int DoorCount { get; set; }

        // DRQ_VEHICLE_CLASS_DETAILS::chBodyType
        [DataMember]
        public string SimpleBodyTypeCode { get; set; }

        // DRQ_VEHICLE_CLASS_DETAILS::chFuelType
        [DataMember]
        public string FuelTypeCode { get; set; }

        // DRQ_VEHICLE_CLASS_DETAILS::chTransmissionType
        [DataMember]
        public string TransmissionType { get; set; }

        #endregion DRQ_VEHICLE_CLASS_DETAILS

        // VEHICLE::eng_cc[5]
        [DataMember]
        public int EngineCapacityCubicCentimetres { get; set; }

        // VEHICLE::year_of_manuf[5]
        [DataMember]
        public int YearOfManufacture { get; set; }

        // VEHICLE::age
        public int Age( DateTime endDate )
        {
            return endDate.Year - YearOfManufacture;
        }

        // VEHICLE::body_type[3]
        // TODO: PremCalc Code mapping from ItelList Code
        [DataMember]
        public string BodyTypeCode { get; set; }

        // VEHICLE::seating_cap[3]
        [DataMember]
        public int SeatingCapacity { get; set; }

        // VEHICLE::RHD_flag
        [DataMember]
        public bool IsRightHandDrive { get; set; }

        // VEHICLE::sidecar_type[3]
        [DataMember]
        public string SideCarStyleCode { get; set; }

        // VEHICLE::sidecar_side
        [DataMember]
        public bool SideCarIsLeftSide { get; set; }

        // VEHICLE::despatch_use_flag
        [DataMember]
        public bool IsUsedForDispatch { get; set; }

        // VEHICLE::mods_flag
        [DataMember]
        public bool IsModified { get; set; }

        // VEHICLE::aszModificationsText[ 4 ][32+1]
        // VEHICLE::aszModificationsCode[ 5 ][7+1]
        [DataMember]
        public VehicleModificationCollection VehicleModificationCollection { get; set; }

        // VEHICLE::value
        [DataMember]
        public int Value { get; set; }

        // VEHICLE::purch_date
        [DataMember]
        public DateTime PurchaseDate { get; set; }

        // VEHICLE::goodsdel_usage_flag
        [DataMember]
        public bool IsUsedForGoodDelivery { get; set; }

        // VEHICLE::owned_by_prop_flag
        [DataMember]
        public TriStateEnum IsOwnedByProposer { get; set; }

        // VEHICLE::reg_to_prop_flag
        [DataMember]
        public TriStateEnum IsRegisteredToProposer { get; set; }

        // VEHICLE::aszOtherRegOwnerDetails[4][DSOS_CB_VEH_REG_OWNER_DTLS+1]
        [DataMember]
        public StringCollection OwnedOrRegisteredTextCollection { get; set; } // TODO: Unknown

        // VEHICLE::tot_ann_mileage
        [DataMember]
        public int AnnualMileage { get; set; }

        // VEHICLE::park_garage_type
        [DataMember]
        public string ParkingLocationCode { get; set; }

        // VEHICLE::reg_no [SZ_VEHREG+1]
        [DataMember]
        public string RegistrationMark { get; set; }

        // VEHICLE::chImported
        [DataMember]
        public TriStateEnum IsImported { get; set; }
    }

    [DataContract]
    public class CoverOptionSelectionRequestDetail // OSM_COVER_OPTION_SELECTION_REQUEST_DETAILS
    {
        // TODO: ItemList Code required
        [DataMember]
        public string TypeCode { get; set; }

        [DataMember]
        public bool Requested { get; set; }

        [DataMember]
        public double Premium { get; set; }

        [DataMember]
        public double CommissionRatePercentage { get; set; }
    }

    [DataContract]
    public class CoverOptionSelectionRequestDetailCollection : Collection<CoverOptionSelectionRequestDetail>
    {
        public CoverOptionSelectionRequestDetailCollection() : base()
        {
        }
    }

    [DataContract]
    public class CoverDetails // COVER_DETAILS
    {
        public CoverDetails()
        {
            QuoteCoverOptionCollection = new CoverOptionSelectionRequestDetailCollection();
            CurrentPolicyCoverOptionCollection = new CoverOptionSelectionRequestDetailCollection();
        }

        // COVER_DETAILS::type[5]
        // TODO: ItemList Code required
        // "COMP", "TPFT", "TPO"
        [DataMember]
        public string CoverTypeCode { get; set; }

        // COVER_DETAILS::VDR_code[3]
        // TODO: ItemList Code required
        [DataMember]
        public string VoluntaryDrivingRestrictionCode { get; set; }

        // COVER_DETAILS::use_postcode [SZ_PCODE+1]
        // TODO: ItemList Code required
        [DataMember]
        public string UsePostcode { get; set; }

        // COVER_DETAILS::achNumberOfCars[1]
        [DataMember]
        public int CarCountInHousehold { get; set; }

        // COVER_DETAILS::start_date
        [DataMember]
        public DateTime StartDate { get; set; }

        // COVER_DETAILS::lRequestedPolicyDurationMonths
        [DataMember]
        public int RequestedPeriodOfCoverMonthCount { get; set; }

        // COVER_DETAILS::class_of_use[4]
        // TODO: ItemList Code required
        [DataMember]
        public string ClassOfUseCode { get; set; }

        // COVER_DETAILS::veh_total_bus_mileage
        [DataMember]
        public string BusinessMileage { get; set; }

        // COVER_DETAILS::veh_spec_usage_flag
        // EIS_COM_PATH (IID_IVehicle, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncCalcVehSpecUsageFlag");
        [DataMember]
        public bool SpecialUsageRequired { get; set; }

        // COVER_DETAILS::achNCD[1]
        [DataMember]
        public int NoClaimsDiscountSpecifiedYears { get; set; }

        // COVER_DETAILS::szNCDEarnedVehicleType[ DSOS_CB_NCD_EARNED_VEHICLE_TYPE_SIZE ]
        // TODO: ItemList Code required
        [DataMember]
        public string NoClaimsDiscountEarnedVehicleType { get; set; }

        // COVER_DETAILS::NCD_in_use_flag
        [DataMember]
        public bool NoClaimsDiscountUsedOnAnotherPolicy { get; set; }

        // COVER_DETAILS::VEX
        [DataMember]
        public double VoluntaryExcess { get; set; }

        // COVER_DETAILS::comp_premium
        [DataMember]
        public double CompetitionPremium { get; set; }

        // COVER_DETAILS::chWindscreenFlag
        [DataMember]
        public TriStateEnum WindscreenCoverRequired { get; set; }

        // COVER_DETAILS::PD
        [DataMember]
        public TriStateEnum ProtectedNoClaimsDiscountRequired { get; set; }

        // COVER_DETAILS::szUsualPaymentMethod[ DSOS_CB_USUAL_PAYMENT_METHOD_SIZE ]
        // TODO: ItemList Code required
        [DataMember]
        public string UsualPaymentMethodCode { get; set; }

        // COVER_DETAILS::szPreviousInsurerAndPaymentMethod[ DSOS_CB_PREVIOUS_INSURER_AND_PAYMENT_METHOD_SIZE ]
        [DataMember]
        public string PreviousInsurerAndPaymentMethodCode { get; set; }

        // COVER_DETAILS::acovosrdQuote[ DSOS_MAX_PREMCALC_COVER_OPTION_COUNT ]
        [DataMember]
        public CoverOptionSelectionRequestDetailCollection QuoteCoverOptionCollection { get; set; }

        // COVER_DETAILS::acovosrdCurrentPolicy[ DSOS_MAX_PREMCALC_COVER_OPTION_COUNT ]
        [DataMember]
        public CoverOptionSelectionRequestDetailCollection CurrentPolicyCoverOptionCollection { get; set; }

    }

    [DataContract]
    public class Acceptance // ACCEPTANCE
    {
        // ACCEPTANCE::cover_ULR_flag
        [DataMember]
        public TriStateEnum UninsuredLossRecoveryRequested { get; set; }

        // ACCEPTANCE::cover_DAP_flag
        [DataMember]
        public TriStateEnum DriverAccidentPlanRequested { get; set; }

        // ACCEPTANCE::cover_PPI_flag
        [DataMember]
        public TriStateEnum PaymentProtectionInsuranceRequested { get; set; }
    }

    [DataContract]
    public class PremiumModellingData // PREMIUM_MODELLING
    {
        // PREMIUM_MODELLING::flPreviousQuoteSchemeCompositeCommissionRatePercent
        [DataMember]
        public double PreviousQuoteSchemeCompositeCommissionRatePercent { get; set; }

    }

    [DataContract]
    public class ReferLoadOrDiscountDriverData // REFR_LOAD_DISCS
    {
        // REFR_LOAD_DISCS::load_value
        [DataMember]
        public double AmountPound { get; set; }

        // REFR_LOAD_DISCS::load_type
        [DataMember]
        public int DriverNumber { get; set; }
    }

    // Index is zero based Driver Index as opposed to the 1 based Driver Number
    [DataContract]
    public class ReferLoadOrDiscountDriverDataCollection : Collection<ReferLoadOrDiscountDriverData>
    {
        public ReferLoadOrDiscountDriverDataCollection()
            : base()
        {
        }
    }


    [DataContract]
    public class ReferralData // REFERRAL
    {
        public ReferralData()
        {
            Notes = new StringCollection();
            AddTermCollection = new StringCollection();
            RemoveTermCollection = new StringCollection();
            ReferLoadOrDiscountDriverDataCollection = new ReferLoadOrDiscountDriverDataCollection();
            AddRequiredDocumentCollection = new StringCollection();
        }

        // REFERRAL::user_ref[5]
        [DataMember]
        public string UserReference { get; set; }

        // REFERRAL::notes[121]
        [DataMember]
        public StringCollection Notes { get; set; }

        // REFERRAL::date
        [DataMember]
        public DateTime StartDate { get; set; }

        // REFERRAL::add_terms[8][3]
        [DataMember]
        public StringCollection AddTermCollection { get; set; }

        // REFERRAL::delete_terms[8][3]
        [DataMember]
        public StringCollection RemoveTermCollection { get; set; }

        // REFERRAL::oride_area_code[3]
        [DataMember]
        public string OverrideAreaCode { get; set; }

        // REFERRAL::oride_veh_group[3]
        [DataMember]
        public string OverrideVehicleGroup { get; set; }

        // REFERRAL::oride_veh_age
        [DataMember]
        public int OverrideVehicleAge { get; set; }

        // REFERRAL::oride_veh_value
        [DataMember]
        public double OverrideVehicleValue { get; set; }

        // REFERRAL::oride_NCD
        [DataMember]
        public int OverrideNoClaimsDiscount { get; set; }

        #region Override Driver Data

            // REFERRAL::oride_driver_no
            [DataMember]
            public int OverrideDriverNumber { get; set; }

            // REFERRAL::oride_driver_no_age
            [DataMember]
            public int OverrideDriverNumberAge { get; set; }

            // REFERRAL::oride_driver_no_mar_status[2]
            // TODO: PremCalc Code mapping from ItemList
            [DataMember]
            public string OverrideDriverNumberMaritalStatusCode { get; set; }

        #endregion Override Driver Data

        // REFERRAL::REFR_LOAD_DISCS load_disc_1/load_discs[9]
        // Indexed by the zero based Driver Index referring to 1 based Driver Number
        [DataMember]
        public ReferLoadOrDiscountDriverDataCollection ReferLoadOrDiscountDriverDataCollection { get; set; }

        // REFERRAL::aachRequiredDocumentCode[3][5]
        [DataMember]
        public StringCollection AddRequiredDocumentCollection { get; set; }
    }

    [DataContract]
    public class AdditionalUnderwritingData // TMS_RETURN_DATA
    {
        // TMS_RETURN_DATA::cover_intro_disc
        [DataMember]
        public bool IntroductoryNoClaimsDiscountGivenPreviously { get; set; }
    }

    [DataContract]
    public class AdditionalUnderwritingQuestionData // ADD_UW_QUESTIONS
    {
        public AdditionalUnderwritingQuestionData()
        {
            DriverMaritalTypeCodeCollection = new StringCollection();
        }

        // Additional Underwriting Question 1
        // ADD_UW_QUESTIONS::Airport_Parking_Flag
        [DataMember]
        public TriStateEnum AirportParkingRequired { get; set; }

        // ADD_UW_QUESTIONS::Airport_Parking_Driver
        [DataMember]
        public int AirportParkingDriverNumber { get; set; }

        // Additional Underwriting Question 2
        // ADD_UW_QUESTIONS::Licence_Expiry_Flag
        [DataMember]
        public TriStateEnum DriverFullUkLicenceWithAtLeast3YearsRemaining { get; set; }

        // ADD_UW_QUESTIONS::Licence_Expiry_Driver
        [DataMember]
        public int DriverFullUkLicenceWithAtLeast3YearsRemainingDriverNumber { get; set; }

        // Additional Underwriting Question 3
        // ADD_UW_QUESTIONS::Furnished_Caravanette_Flag
        [DataMember]
        public TriStateEnum CarvanetteIsFurnished { get; set; }

        // Additional Underwriting Question 4
        // ADD_UW_QUESTIONS::Green_Card_Period_Flag
        [DataMember]
        public TriStateEnum ForeignTravelPeiodWillExceedOneMonth { get; set; }

        // Additional Underwriting Question 5
        // ADD_UW_QUESTIONS::Security_Device_Flag
        [DataMember]
        public TriStateEnum ApprovedAntiTheftDeviceFitted { get; set; }

        // Additional Underwriting Question 6
        // ADD_UW_QUESTIONS::Vehicle_Exposure_Flag
        [DataMember]
        public TriStateEnum VehicleParkedInSamePlaceAsPreviousTheft { get; set; }

        // Additional Underwriting Question 7
        // ADD_UW_QUESTIONS::Previous_Fire_Flag
        [DataMember]
        public TriStateEnum VehicleIsTheSubjectOfAPreviousFireClaim { get; set; }

        // Additional Underwriting Question 8
        // ADD_UW_QUESTIONS::Concurrent_Incident_Flag
        [DataMember]
        public TriStateEnum DriverConvictionsOnSameDateAroseFromTheSameIncident { get; set; }

        // ADD_UW_QUESTIONS::Concurrent_Incident_Driver
        [DataMember]
        public int DriverConvictionsOnSameDateAroseFromTheSameIncidentDriverNumber { get; set; }

        // Additional Underwriting Question 9
        // ADD_UW_QUESTIONS::Careless_Driving_Flag
        [DataMember]
        public TriStateEnum DriverCarelessDrivingConvictionsAroseFromAccidents { get; set; }

        // ADD_UW_QUESTIONS::Careless_Driving_Driver
        [DataMember]
        public int DriverCarelessDrivingConvictionsAroseFromAccidentsDriverNumber { get; set; }

        // Additional Underwriting Question 10
        // ADD_UW_QUESTIONS::Additional_Driver_Car_Flag
        [DataMember]
        public TriStateEnum AdditionalDriversOwnCarsOfTheirOwn { get; set; }

        // Additional Underwriting Question 11
        // ADD_UW_QUESTIONS::Under21_Car_Flag : wrong variable name
        [DataMember]
        public TriStateEnum DriversUnder25OwnOrHaveVehiclesRegistreredInTheirName { get; set; }

        // Additional Underwriting Question 12
        // ADD_UW_QUESTIONS::Youngest_Driver_Car_Flag
        [DataMember]
        public TriStateEnum SecondCarOwnedAndRegisteredByYoungestDriver { get; set; }

        // Additional Underwriting Question 13
        // ADD_UW_QUESTIONS::MOT_Available_Flag
        [DataMember]
        public TriStateEnum VehicleMotCertificateDatedWithinLast3MonthsIsAvailable { get; set; }

        // Additional Underwriting Question 14
        // ADD_UW_QUESTIONS::Policy_Abroad_Flag
        [DataMember]
        public TriStateEnum VehicleAbroadHadPolicyInDriverName { get; set; }

        // Additional Underwriting Question 15
        // ADD_UW_QUESTIONS::Dual_Controlled_Flag
        [DataMember]
        public TriStateEnum VehicleIsDualControl { get; set; }

        // Additional Underwriting Question 16
        // ADD_UW_QUESTIONS::Licence_Issued_Abroad_Flag
        [DataMember]
        public TriStateEnum DriverLicenceIsFromUsaCanadaNewZealandAustraliaHongKongOrWesternEurope { get; set; }

        // ADD_UW_QUESTIONS::Licence_Issued_Abroad_Driver
        [DataMember]
        public int DriverLicenceIsFromUsaCanadaNewZealandAustraliaHongKongOrWesternEuropeDriverNumber { get; set; }

        // Additional Underwriting Question 17
        // ADD_UW_QUESTIONS::Six_Month_Residency_Flag
        [DataMember]
        public TriStateEnum IsBritishSubjectWithAtLeast6MonthsResidence { get; set; }

        // Additional Underwriting Question 18
        // ADD_UW_QUESTIONS::Temporary_Break_Flag
        [DataMember]
        public TriStateEnum Has4YearsRegularDrivingExperienceAndLessThan1YearAbroad { get; set; }

        // Additional Underwriting Question 19
        // ADD_UW_QUESTIONS::Regular_Driver_Flag
        [DataMember]
        public TriStateEnum HasRegularDrivingExperienceDuringResidency { get; set; }

        // ADD_UW_QUESTIONS::Regular_Driver_Driver
        [DataMember]
        public int HasRegularDrivingExperienceDuringResidencyDriverNumber { get; set; }

        // Additional Underwriting Question 20
        // ADD_UW_QUESTIONS::Inner_City_Use_Flag
        [DataMember]
        public TriStateEnum VehicleUsedInInnerCity { get; set; }

        // Additional Underwriting Question 21
        // ADD_UW_QUESTIONS::Residency_Intention_Flag
        [DataMember]
        public TriStateEnum DriverIntentionToRemainInUkForAtLeast1Year { get; set; }

        // ADD_UW_QUESTIONS::Residency_Intention_Driver
        [DataMember]
        public int DriverIntentionToRemainInUkForAtLeast1YearDriverNumber { get; set; }

        // Additional Underwriting Question 22
        // ADD_UW_QUESTIONS::Car_Derived_Van_Flag
        [DataMember]
        public TriStateEnum NoClaimsDiscountWasEarnedOnCarDerivedVan { get; set; }

        // Additional Underwriting Question 23
        // ADD_UW_QUESTIONS::Offshore_One_Month_Flag
        [DataMember]
        public TriStateEnum DriverSpentOver1MonthOffshoreForOccupation { get; set; }

        // ADD_UW_QUESTIONS::Offshore_One_Month_Driver
        [DataMember]
        public int DriverSpentOver1MonthOffshoreForOccupationDriverNumber { get; set; }

        // Additional Underwriting Question 24
        // ADD_UW_QUESTIONS::Existing_EIS_Policy_Flag
        [DataMember]
        public TriStateEnum HasExistingEndsleighMotorPolicy { get; set; }

        // Additional Underwriting Question 25
        // ADD_UW_QUESTIONS::Daytime_Exposure_Flag
        [DataMember]
        public TriStateEnum VehicleExposedInStreetOrPublicCarParkDuringDay { get; set; }

        // Additional Underwriting Question 26
        // ADD_UW_QUESTIONS::Partner_Licence_Flag
        [DataMember]
        public TriStateEnum PartnerHoldsLicenceToDriveVehicle { get; set; }

        // Additional Underwriting Question 27
        // ADD_UW_QUESTIONS::Under_Twentyfive_Flag
        [DataMember]
        public TriStateEnum PersonsUnder25WillBeAbleToDriveVehicle { get; set; }

        // Additional Underwriting Question 28
        // ADD_UW_QUESTIONS::Previous_PD_Policy_Flag
        [DataMember]
        public TriStateEnum NoClaimsDiscountProtectionOnPreviousPolicy { get; set; }

        // Additional Underwriting Question 29
        // ADD_UW_QUESTIONS::Turbo_Cycle_Flag
        [DataMember]
        public TriStateEnum MotorCycleIsTurboCharged { get; set; }

        // Additional Underwriting Question 32
        // ADD_UW_QUESTIONS::Insured_Since_Purchase_Flag
        [DataMember]
        public TriStateEnum VehicleInsuredByImmediateFamilySincePurchase { get; set; }

        // Additional Underwriting Question 33
        // ADD_UW_QUESTIONS::Regular_Driver_2Years_Flag
        [DataMember]
        public TriStateEnum PoposerHasDrivenRegularlyForThePast2Years { get; set; }

        // Additional Underwriting Question 34
        // ADD_UW_QUESTIONS::Driver_Res_At_Home_Flag
        [DataMember]
        public TriStateEnum AdditionalDriversUnder25LiveAtHomeWithParents { get; set; }

        // Additional Underwriting Question 35
        // ADD_UW_QUESTIONS::Pass_Plus_Taken_Flag
        [DataMember]
        public TriStateEnum CompletedPassPlusInLast2Years { get; set; }

        // Additional Underwriting Question 36
        // ADD_UW_QUESTIONS::Use_Of_Other_Car_Flag
        [DataMember]
        public TriStateEnum ProposerOrSpouseHasUseOfAnotherVehicle { get; set; }

        // Additional Underwriting Question 37
        // ADD_UW_QUESTIONS::NCD_On_Other_Car_Flag
        [DataMember]
        public TriStateEnum ProposerOrSpouseHaveAtLeast4YearsNcdOnAnotherVehicle { get; set; }

        // Additional Underwriting Question 38
        // ADD_UW_QUESTIONS::chDriverIsNonSmoker
        [DataMember]
        public TriStateEnum ProposerIsNonSmoker { get; set; }

        // Additional Underwriting Question 39
        // ADD_UW_QUESTIONS::chDvlaAdvised
        [DataMember]
        public TriStateEnum DvlaHasBeenInformedOfDisabilityAndHasAuthorisedDriving { get; set; }

        // ADD_UW_QUESTIONS::szNCDEarnedCountry
        [DataMember]
        public string NoClaimsDiscountEarnedCountryCode { get; set; }

        // TODO: ItemList Code required
        // ADD_UW_QUESTIONS::chNatureOfTenure
        [DataMember]
        public string NatureOfTenure { get; set; }

        // ADD_UW_QUESTIONS::lGraduationDate
        [DataMember]
        public DateTime GraduationDate { get; set; }

        // ADD_UW_QUESTIONS::sLoyaltyToEisYears
        [DataMember]
        public int LoyaltyToEisYearCount { get; set; }

        // ADD_UW_QUESTIONS::sLoyaltyToInsYears
        [DataMember]
        public int LoyaltyToInsurerYearCount { get; set; }

        // TODO: Unknown. Defaulted to character value of 1 = true (not '1')
        // ADD_UW_QUESTIONS::chUsualOccDrv
        [DataMember]
        public bool IsPoposerUsualOccupation { get; set; }

        // TODO: ItemList Code required
        // ADD_UW_QUESTIONS::szUsualOcc
        [DataMember]
        public string ProposerUsualOccupationCode { get; set; }

        // TODO: ItemList Code required
        // ADD_UW_QUESTIONS::UsualOcc1
        [DataMember]
        public string SpouseUsualOccupationCode { get; set; }

        // TODO: ItemList Code required
        // ADD_UW_QUESTIONS::SecDev
        [DataMember]
        public string VehicleSecurityDeviceCode { get; set; }

        // TODO: ItemList Code required
        // ADD_UW_QUESTIONS::veh_owned_by_who
        [DataMember]
        public string VehicleOwnerCode { get; set; }

        // TODO: ItemList Code required
        // ADD_UW_QUESTIONS::veh_type_of_reg
        [DataMember]
        public string VehicleRegistrationTypeCode { get; set; }

        // TODO: ItemList Code required
        // ADD_UW_QUESTIONS::veh_reg_to_who
        [DataMember]
        public string VehicleRegisteredPartyCode { get; set; }

        // ADD_UW_QUESTIONS::length_veh_ownership
        [DataMember]
        public int VehicleOwnedMonthCount { get; set; }

        // ADD_UW_QUESTIONS::driver_married_ind
        [DataMember]
        public StringCollection DriverMaritalTypeCodeCollection { get; set; }

        // ADD_UW_QUESTIONS::lEISLoyaltyMonths
        [DataMember]
        public int LoyaltyToEisMonthCount { get; set; }
    }
}
