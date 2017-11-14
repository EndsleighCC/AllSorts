using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TestMotorPremCalcIO
{

    [DataContract]
    public class MotorPremCalcRequest
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

        [DataMember]
        public PremCalcBehaviouralParameters PremCalcBehaviouralParameters { get; set; } // DSOS_PREMCALC_PARAMS

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

    }

    public enum TriState
    {
        Unknown,
        Active,
        Inactive
    }

    [DataContract]
    public class PremCalcBehaviouralParameters // DSOS_PREMCALC_PARAMS
    {
        public PremCalcBehaviouralParameters()
        {
        }

        [DataMember]
        public bool IsVariant { get; set; }
        [DataMember]
        public bool IsSecondLevelUnderwriting { get; set; }
        [DataMember]
        public bool ReviewUnderwritingQuestions { get; set; }
    }

    public enum GenderEnum
    {
        Male,
        Female
    }

    public enum MaritalStatusEnum
    {
        Single,
        Married,
        Cohabiting
    }

    public class StringCollection : Collection<string>
    {
        public StringCollection() : base()
        {
        }
    }

    public class CaseContextData // CONTROL_SECTION
    {
        public CaseContextData()
        {
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
            None ,
            PreAdjustment ,
            PostAdjustment
        }

        public enum VehicleCategoryEnum
        {
            Car ,
            Motorcycle
        }

        // Unclear wht this means
        public enum PolicyStatusEnum
        {
            Unknown
        }

        public enum QuoteSystemTransactionTypeEnum
        {
            NewBusiness ,
            Adjustment ,
            HostQuote ,
            CrossQuote
        }

        public enum PreviousPolicyTermPaymentMethodEnum
        {
            // TODO: Unsure!!!!
            CreditCard ,
            Cash
        }

        [DataMember]
        public InputDataSourceEnum InputDataSource { get; set; }

        [DataMember]
        public PremCalcPhaseEnum PremCalcPhase { get; set; }

        [DataMember]
        public string RequestingSystemApplicationVersion { get; set; }

        [DataMember]
        public string BranchCode { get; set; }

        [DataMember]
        public string SystemTransactionCode { get; set; }

        [DataMember]
        public DateTime RequestDateTime { get; set; }

        // Whether Introductory NCD was given when the case was created
        [DataMember]
        public bool IntroductoryNcdGiven { get; set; }

        [DataMember]
        public string QuoteNumber { get; set; }

        [DataMember]
        public string BaseQuoteNumber { get; set; }

        [DataMember]
        public string PreviousQuoteSchemeCode { get; set; }

        [DataMember]
        public DateTime PreviousQuoteDateTime { get; set; }

        [DataMember]
        public VehicleCategoryEnum VehicleCategory { get; set; }

        [DataMember]
        public PolicyStatusEnum PolicyStatus { get; set; }

        [DataMember]
        public string ClientCode { get; set; }

        [DataMember]
        public int OriginalNcdYearsClaimed { get; set; }

        [DataMember]
        public QuoteSystemTransactionTypeEnum QuoteSystemTransactionType { get; set; }

        [DataMember]
        public string CurrentPolicySchemeCode { get; set; }

        [DataMember]
        public string CurrentPolicyNumber { get; set; }

        [DataMember]
        public TriState HasCurrentPremiumCreditAccount { get; set; }

        [DataMember]
        public TriState IsEligibleForPremiumCredit { get; set; }

        [DataMember]
        public DateTime RenewalDateTime { get; set; }

        [DataMember]
        public double SuspensionCreditAmount { get; set; }

        [DataMember]
        public bool IsSpeculativeQuote { get; set; }

        [DataMember]
        public DateTime StartOfCurrentTermDateTime { get; set; }

        [DataMember]
        public DateTime EndOfCurrentTermDateTime { get; set; }

        [DataMember]
        public DateTime OriginalInceptionDate { get; set; }

        [DataMember]
        public DateTime LastAdjustmentDateTime { get; set; }

        [DataMember]
        public TriState HostPolicyHasProtectedDiscount { get; set; }

        [DataMember]
        public TriState HostHasLongTermGreenCard { get; set; }

        [DataMember]
        public double AnnualPremiumAtLastAdjustment { get; set; }

        [DataMember]
        public bool SixMonthPolicy { get; set; }

        [DataMember]
        public PreviousPolicyTermPaymentMethodEnum PreviousPolicyTermPaymentMethod { get; set; }

        [DataMember]
        public double PreviousPolicyTermCommissionRatePercent { get; set; }

        [DataMember]
        public double PreviousPolicyTermCalculatedInsurancePremium { get; set; }

        [DataMember]
        public double PreviousPolicyTermAdministrationFee { get; set; }

        [DataMember]
        public int PreviousPolicyDurationMonths { get; set; }

        [DataMember]
        public string IntroducerCode { get; set; }

        [DataMember]
        public string ContactRouteCode { get; set; }

        [DataMember]
        public string ContactSourceCode { get; set; }

        [DataMember]
        public string MarketingCampaignCode { get; set; }

        [DataMember]
        public int AgentCategory { get; set; }

        [DataMember]
        public bool AgentIsAgregator { get; set; }

        [DataMember]
        public double RenewalPricingAdjustmentPercent { get; set; }

        [DataMember]
        public int NewBusinessLeadTimeDayCount { get; set; }

        [DataMember]
        public double ExpiringPolicyAnnualisedCalculatedInsurancePremium { get; set; }

        [DataMember]
        public string CueRefererence { get; set; }

        [DataMember]
        public bool IsRenewalSelect { get; set; }
    }

    public class DiaryData // DIARY_WORK_AREA
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public DateTime AutoActivationDate { get; set; }

        [DataMember]
        public string Note { get; set; }
    }

    public class AddressLineCollection : Collection<string>
    {
        public AddressLineCollection()
            : base()
        {
        }
    }

    public class ClientData // CLIENT_DATA_CAPTURE
    {
        public ClientData()
        {
            ClientAddressLineCollection = new AddressLineCollection();
            RiskAddressLineCollection = new AddressLineCollection();
        }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Forename { get; set; }

        [DataMember]
        public string Initials { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public GenderEnum Gender { get; set; }

        [DataMember]
        public MaritalStatusEnum MaritalStatus { get; set; }

        [DataMember]
        public DateTime DateOfBirth { get; set; }

        [DataMember]
        public AddressLineCollection ClientAddressLineCollection { get; set; }

        [DataMember]
        public string ClientPostcode { get; set; }

        [DataMember]
        public AddressLineCollection RiskAddressLineCollection { get; set; }

        [DataMember]
        public string RiskPostcode { get; set; }

        [DataMember]
        public string HomePhoneNumber { get; set; }

        [DataMember]
        public string WorkPhoneNumber { get; set; }

        [DataMember]
        public string ClientEmailAddress { get; set; }

        [DataMember]
        public string ClientNumber { get; set; }

        [DataMember]
        public string EducationEstablishmentCode { get; set; }

        [DataMember]
        public DateTime GraduationDate { get; set; }

        [DataMember]
        public bool IsBadDebtRisk { get; set; }

        [DataMember]
        public bool IsGraduate { get; set; }

        [DataMember]
        public string EmployerBusinessCategory { get; set; }

        [DataMember]
        public string FullTimeOccupationCategory { get; set; }

        [DataMember]
        public string CourseStudiedCode { get; set; }

        [DataMember]
        public string NusCardNumber { get; set; }

        [DataMember]
        public bool NusExtraCardHeld { get; set; }

        [DataMember]
        public bool StudentPossesssionsPolicyEverHeld { get; set; }

        [DataMember]
        public string TeachingLocationCode { get; set; }

        [DataMember]
        public string TeachingEstablishmentCode { get; set; }

        [DataMember]
        public string TeachingSubjectCode { get; set; }

        [DataMember]
        public bool HasChildrenUnderAge16 { get; set; }

        [DataMember]
        public bool DriversExistWithCriminalConvictions { get; set; }

    }

    [DataContract]
    public class AccidentDetail // ACC_LOSS
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        public MotorUtility.LargeTimeSpan TimeAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date , coverStartDateOrPolicyStartDateOrAdjustmentstartDate );
        }

        public int MonthsAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date, coverStartDateOrPolicyStartDateOrAdjustmentstartDate).MonthCount;
        }

        [DataMember]
        public int DamageCost { get; set; }

        [DataMember]
        public int InjuryCost { get; set; }

        [DataMember]
        public string DaultBonusIndicator { get; set; }

    }

    public class AccidentDataCollection : Collection<AccidentDetail>
    {
        public AccidentDataCollection() : base()
        {
        }
    }

    public class ConvictionDetail
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public TriState Pending { get; set; }

        public MotorUtility.LargeTimeSpan TimeAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date, coverStartDateOrPolicyStartDateOrAdjustmentstartDate);
        }

        public int MonthsAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date, coverStartDateOrPolicyStartDateOrAdjustmentstartDate).MonthCount;
        }

        [DataMember]
        public int DvlaPointCount { get; set; }

        [DataMember]
        public int FineAmountPounds { get; set; }

    }

    public class ConvictionDetailCollection : Collection<ConvictionDetail>
    {
        public ConvictionDetailCollection() : base()
        {
        }
    }

    public class BanDetail
    {
        [DataMember]
        public string Reason { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        public MotorUtility.LargeTimeSpan TimeAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date, coverStartDateOrPolicyStartDateOrAdjustmentstartDate);
        }

        public int MonthsAgo(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return new MotorUtility.LargeTimeSpan(Date, coverStartDateOrPolicyStartDateOrAdjustmentstartDate).MonthCount;
        }

        [DataMember]
        public int DurationWeekCount { get; set; }
    }

    public class BanDetailCollection : Collection<BanDetail>
    {
        public BanDetailCollection() : base()
        {
        }
    }

    public class DisabilityDetail
    {
        [DataMember]
        public string Type { get; set; }
    }

    public class DisabilityCollection : Collection<DisabilityDetail>
    {
        public DisabilityCollection() : base()
        {
        }
    }

    public class OccupationDetail
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Sector { get; set; }

        [DataMember]
        public string UnionCode { get; set; }
    }

    public class DriverData // DRIVER
    {
        public enum StatusEnum
        {
            Active ,
            Suspended ,
            Deleted
        }

        public enum LicenceTypeEnum
        {
            International ,
            FullUk ,
            ProvisionalUk ,
            Overseas ,
            FullEu ,
            ProvisionalEu ,
            CommonwealthIssued
        }

        public enum PreviousDriverDeclineReasonEnum
        {
            None ,
            DueToOccupation ,
            DueToAgeOfDriver ,
            DueToTypeOrClassOfVehicle ,
            Other
        }

        public enum TermTimeResidenceEnum
        {
            HallsOfResidenceOnOrOffCampus,
            SharedAccommodationRented,
            RentedAccommodationNonShared,
            SharedAccommodationProvidedByUniversityOrCollege,
            LivingAtHomeWithParentsOrGuardian,
            OwnHomeMortgagedOrOwnedOutright,
            NotKnownOrNotSpecified
        }

        public enum DriveOtherVehiclesEnum
        {
            NoAccessToOtherCars ,
            OwnAnotherCar ,
            NamedDriverOnAnotherPolicy ,
            CompanyCarIncludingSocialUse ,
            CompanyCarExcludingSocialUse ,
            OwnOrHaveUseOfAVan ,
            Other
        }

        public enum RelationshipToProposerEnum
        {
            MarriedOrCivilPartner ,
            Parent ,
            SonOrDaughter ,
            BrotherOrSister ,
            OtherFamilyMember ,
            BusinessPartner ,
            Employee ,
            Employer ,
            LinInPartner ,
            Other
        }

        public DriverData()
        {
            AccidentDataCollection = new AccidentDataCollection();
            ConvictionDetailCollection = new ConvictionDetailCollection();
            BanDetailCollection = new BanDetailCollection();
            DisabilityCollection = new DisabilityCollection();
            FullTimeOccupation = new OccupationDetail();
            PartTimeOccupation = new OccupationDetail();
        }

        // Driver Number 1 is the Proposer. There is no Driver zero i.e. this is not a Driver Index
        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public StatusEnum Status { get; set; }

        [DataMember]
        public DateTime EffectiveDate { get; set; }

        [DataMember]
        public GenderEnum Gender { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Forename { get; set; }

        [DataMember]
        public string Initials { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public MaritalStatusEnum MaritalStatus { get; set; }

        [DataMember]
        public bool IsSpouseOfProposer { get; set; }

        [DataMember]
        public DateTime BirthDate { get; set; }

        public int Age(DateTime coverStartDateOrPolicyStartDateOrAdjustmentstartDate)
        {
            return MotorUtility.YearCountBetween(BirthDate, coverStartDateOrPolicyStartDateOrAdjustmentstartDate);
        }

        [DataMember]
        public LicenceTypeEnum LicenceType { get; set; }

        [DataMember]
        public DateTime LicenceDateTestPassed { get; set; }

        public int TestPassedMonthsAgo { get { return 0 /* Months from Cover Start Date or Policy Start Date to LicenceDateTestPassed. See MotorUtility.MonthCountBetween */ ; } }

        [DataMember]
        public int UkResidencePeriodMonths { get; set; }

        [DataMember]
        public OccupationDetail FullTimeOccupation { get; set; }

        [DataMember]
        public OccupationDetail PartTimeOccupation { get; set; }

        [DataMember]
        public TriState Commuting { get; set; }

        [DataMember]
        public TriState BusinessUse { get; set; }

        [DataMember]
        public PreviousDriverDeclineReasonEnum PreviousDriverDeclineReason { get; set; }

        [DataMember]
        public TermTimeResidenceEnum TermTimeResidence { get; set; }

        [DataMember]
        public DriveOtherVehiclesEnum DriveOtherVehicles { get; set; }

        [DataMember]
        public RelationshipToProposerEnum RelationshipToProposer { get; set; }

        [DataMember]
        public string EmployerBusinessCategory { get; set; }

        #region Driver Collections

        [DataMember]
        public AccidentDataCollection AccidentDataCollection { get; set; }

        [DataMember]
        public ConvictionDetailCollection ConvictionDetailCollection { get; set; }

        [DataMember]
        public BanDetailCollection BanDetailCollection { get; set; }

        [DataMember]
        public DisabilityCollection DisabilityCollection { get; set; }

        #endregion Driver Collections
    }

    public class DriverDataCollection : Collection<DriverData> // DRIVER[]
    {
        public DriverDataCollection() : base()
        {
        }

        // From ADD_UW_QUESTIONS::sMainDriverNumber
        public int MainDriverNumber { get; set; }
    }

    public class VehicleModification
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    public class VehicleModificationCollection : Collection<VehicleModification>
    {
        public VehicleModificationCollection() : base()
        {
        }
    }

    public class VehicleDetails // VEHICLE
    {
        public enum SimpleBodyTypeEnum
        {
            Estate ,
            Cabriolet ,
            Other
        }

        public enum FuelTypeEnum
        {
            Petrol ,
            Diesel ,
            Electric ,
            Other
        }

        public enum TransmissionTypeEnum
        {
            Manual ,
            Automatic ,
            Other
        }

        public enum BodyTypeEnum
        {
            Saloon ,
            Hatchback ,
            Coupe ,
            Sports ,
            Van ,
            Estate ,
            Motorcycle ,
            CaravanetteManufacturer ,
            CaravanetteConverted ,
            Pickup ,
            Utility ,
            Beachbuggey ,
            Convertible ,
            Jeep ,
            LongWheelBaseLandrover
        }

        public enum SideCarStyleEnum
        {
            NeitherSide ,
            LeftSide ,
            RightSide ,
            SidewinderLeft ,
            SidewinderRight
        }

        public enum ParkingLocationEnum
        {
            Driveway ,
            Garage ,
            CarPort ,
            OnRoad ,
            InTheOpen ,
            PrivateCarPark ,
            PublicCarPark ,
            HallsOfResidence
        }

        public VehicleDetails()
        {
            VehicleModificationCollection = new VehicleModificationCollection();
            OwnedOrRegisteredText = new StringCollection();
        }

        [DataMember]
        public string AbiCode { get; set; }

        [DataMember]
        public string MakeDescription { get; set; }

        [DataMember]
        public string ShortModelDescription { get; set; }

        [DataMember]
        public string FullModelDescription { get; set; }

        [DataMember]
        public int DoorCount { get; set; }

        [DataMember]
        public SimpleBodyTypeEnum SimpleBodyType { get; set; }

        [DataMember]
        public FuelTypeEnum FuelType { get; set; }

        [DataMember]
        public TransmissionTypeEnum TransmissionType { get; set; }

        [DataMember]
        public int EngineCapacityCubicCentimetres { get; set; }

        [DataMember]
        public int YearOfManufacture { get; set; }

        public int Age( DateTime endDate )
        {
            return endDate.Year - YearOfManufacture;
        }

        [DataMember]
        public BodyTypeEnum BodyType { get; set; }

        [DataMember]
        public int SeatingCapacity { get; set; }

        [DataMember]
        public bool IsRightHandDrive { get; set; }

        [DataMember]
        public SideCarStyleEnum SideCarStyle { get; set; }

        [DataMember]
        public bool SideCarIsLeftSide { get; set; }

        [DataMember]
        public bool IsUsedForDispatch { get; set; }

        [DataMember]
        public bool IsModified { get; set; }

        [DataMember]
        public int Value { get; set; }

        [DataMember]
        public DateTime PurchaseDate { get; set; }

        [DataMember]
        public bool IsUsedForGoodDelivery { get; set; }

        [DataMember]
        public TriState IsOwnedByProposer { get; set; }

        [DataMember]
        public TriState IsRegisteredToProposer { get; set; }

        [DataMember]
        public StringCollection OwnedOrRegisteredText { get; set; } // TODO: What is this?

        [DataMember]
        public int TotalAnnualMileage { get; set; }

        [DataMember]
        public ParkingLocationEnum ParkingLocation { get; set; }

        [DataMember]
        public string RegistrationMark { get; set; }

        [DataMember]
        public bool IsImported { get; set; }

        [DataMember]
        public VehicleModificationCollection VehicleModificationCollection { get; set; }
    }

    public class CoverOptionSelectionRequestDetail
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

    public class CoverOptionSelectionRequestDetailCollection : Collection<CoverOptionSelectionRequestDetail>
    {
        public CoverOptionSelectionRequestDetailCollection() : base()
        {
        }
    }

    public class CoverDetails // COVER_DETAILS
    {
        public CoverDetails()
        {
            SpecialUsageDescriptionText = new StringCollection();
            QuoteCoverOptionCollection = new CoverOptionSelectionRequestDetailCollection();
            CurrentPolicyCoverOptionCollection = new CoverOptionSelectionRequestDetailCollection();
        }

        // TODO: ItemList Code required
        // "COMP", "TPFT", "TPO"
        [DataMember]
        public string CoverTypeCode { get; set; }

        // TODO: ItemList Code required
        [DataMember]
        public string VoluntaryDrivingRestrictionCode { get; set; }

        // TODO: ItemList Code required
        [DataMember]
        public string UsePostcode { get; set; }

        [DataMember]
        public int CarCountInHousehold { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public int RequestedPeriodOfCoverMonthCount { get; set; }

        // TODO: ItemList Code required
        [DataMember]
        public string ClassOfUseCode { get; set; }

        [DataMember]
        public string BusinessMileage { get; set; }

        // EIS_COM_PATH (IID_IVehicle, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncCalcVehSpecUsageFlag");
        [DataMember]
        public bool SpecialUsageRequired { get; set; }

        [DataMember]
        public StringCollection SpecialUsageDescriptionText { get; set; }

        [DataMember]
        public int NoClaimsDiscountSpecifiedYears { get; set; }

        // TODO: ItemList Code required
        [DataMember]
        public string NoClaimsDiscountEarnedVehicleType { get; set; }

        [DataMember]
        public bool NoClaimsDiscountUsedOnAnotherPolicy { get; set; }

        [DataMember]
        public double VoluntaryExcess { get; set; }

        [DataMember]
        public double CompetitionPremium { get; set; }

        [DataMember]
        public TriState WindscreenCoverRequired { get; set; }

        [DataMember]
        public TriState ProtectedNoClaimsDiscountRequired { get; set; }

        // TODO: ItemList Code required
        [DataMember]
        public string UsualPaymentMethodCode { get; set; }

        [DataMember]
        public string PreviousInsurerAndPaymentMethodCode { get; set; }

        [DataMember]
        public CoverOptionSelectionRequestDetailCollection QuoteCoverOptionCollection { get; set; }

        [DataMember]
        public CoverOptionSelectionRequestDetailCollection CurrentPolicyCoverOptionCollection { get; set; }

    }

    public class Acceptance // ACCEPTANCE
    {
        [DataMember]
        public TriState UninsuredLossRecoveryRequested { get; set; }

        [DataMember]
        public TriState DriverAccidentPlanRequested { get; set; }

        [DataMember]
        public TriState PaymentProtectionInsuranceRequested { get; set; }
    }

    public class PremiumModellingData // PREMIUM_MODELLING
    {
        // PREMIUM_MODELLING
        [DataMember]
        public double PreviousQuoteSchemeCompositeCommissionRatePercent { get; set; }

    }

    public class ReferLoadOrDiscountDriverData // REFR_LOAD_DISCS
    {
        [DataMember]
        public double AmountPound { get; set; }

        [DataMember]
        public int DriverNumber { get; set; }
    }

    // Index is zero based Driver Index
    public class ReferLoadOrDiscountDriverDataCollection : Collection<ReferLoadOrDiscountDriverData>
    {
        public ReferLoadOrDiscountDriverDataCollection()
            : base()
        {
        }
    }


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

        [DataMember]
        public string UserReference { get; set; }

        [DataMember]
        public StringCollection Notes { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public StringCollection AddTermCollection { get; set; }

        [DataMember]
        public StringCollection RemoveTermCollection { get; set; }

        [DataMember]
        public string OverrideAreaCode { get; set; }

        [DataMember]
        public string OverrideVehicleGroup { get; set; }

        [DataMember]
        public int OverrideVehicleAge { get; set; }

        [DataMember]
        public double OverrideVehicleValue { get; set; }

        [DataMember]
        public int OverrideNoClaimsDiscount { get; set; }

        [DataMember]
        public int OverrideDriverNumber { get; set; }

        [DataMember]
        public int OverrideDriverNumberAge { get; set; }

        // Indexed by the zero based Driver Index
        [DataMember]
        public ReferLoadOrDiscountDriverDataCollection ReferLoadOrDiscountDriverDataCollection { get; set; }

        [DataMember]
        public StringCollection AddRequiredDocumentCollection { get; set; }

        [DataMember]
        public MaritalStatusEnum OverrideDriverNumberMaritalStatus { get; set; }
    }

    public class AdditionalUnderwritingData // TMS_RETURN_DATA
    {
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
        public TriState AirportParkingRequired { get; set; }

        // ADD_UW_QUESTIONS::Airport_Parking_Driver
        [DataMember]
        public int AirportParkingDriverNumber { get; set; }

        // Additional Underwriting Question 2
        // ADD_UW_QUESTIONS::Licence_Expiry_Flag
        [DataMember]
        public TriState DriverFullUkLicenceWithAtLeast3YearsRemaining { get; set; }

        // ADD_UW_QUESTIONS::Licence_Expiry_Driver
        [DataMember]
        public int DriverFullUkLicenceWithAtLeast3YearsRemainingDriverNumber { get; set; }

        // Additional Underwriting Question 3
        // ADD_UW_QUESTIONS::Furnished_Caravanette_Flag
        [DataMember]
        public TriState CarvanetteIsFurnished { get; set; }

        // Additional Underwriting Question 4
        // ADD_UW_QUESTIONS::Green_Card_Period_Flag
        [DataMember]
        public TriState ForeignTravelPeiodWillExceedOneMonth { get; set; }

        // Additional Underwriting Question 5
        // ADD_UW_QUESTIONS::Security_Device_Flag
        [DataMember]
        public TriState ApprovedAntiTheftDeviceFitted { get; set; }

        // Additional Underwriting Question 6
        // ADD_UW_QUESTIONS::Vehicle_Exposure_Flag
        [DataMember]
        public TriState VehicleParkedInSamePlaceAsPreviousTheft { get; set; }

        // Additional Underwriting Question 7
        // ADD_UW_QUESTIONS::Previous_Fire_Flag
        [DataMember]
        public TriState VehicleIsTheSubjectOfAPreviousFireClaim { get; set; }

        // Additional Underwriting Question 8
        // ADD_UW_QUESTIONS::Concurrent_Incident_Flag
        [DataMember]
        public TriState DriverConvictionsOnSameDateAroseFromTheSameIncident { get; set; }

        // ADD_UW_QUESTIONS::Concurrent_Incident_Driver
        [DataMember]
        public int DriverConvictionsOnSameDateAroseFromTheSameIncidentDriverNumber { get; set; }

        // Additional Underwriting Question 9
        // ADD_UW_QUESTIONS::Careless_Driving_Flag
        [DataMember]
        public TriState DriverCarelessDrivingConvictionsAroseFromAccidents { get; set; }

        // ADD_UW_QUESTIONS::Careless_Driving_Driver
        [DataMember]
        public int DriverCarelessDrivingConvictionsAroseFromAccidentsDriverNumber { get; set; }

        // Additional Underwriting Question 10
        // ADD_UW_QUESTIONS::Additional_Driver_Car_Flag
        [DataMember]
        public TriState AdditionalDriversOwnCarsOfTheirOwn { get; set; }

        // Additional Underwriting Question 11
        // ADD_UW_QUESTIONS::Under21_Car_Flag : wrong variable name
        [DataMember]
        public TriState DriversUnder25OwnOrHaveVehiclesRegistreredInTheirName { get; set; }

        // Additional Underwriting Question 12
        // ADD_UW_QUESTIONS::Youngest_Driver_Car_Flag
        [DataMember]
        public TriState SecondCarOwnedAndRegisteredByYoungestDriver { get; set; }

        // Additional Underwriting Question 13
        // ADD_UW_QUESTIONS::MOT_Available_Flag
        [DataMember]
        public TriState VehicleMotCertificateDatedWithinLast3MonthsIsAvailable { get; set; }

        // Additional Underwriting Question 14
        // ADD_UW_QUESTIONS::Policy_Abroad_Flag
        [DataMember]
        public TriState VehicleAbroadHadPolicyInDriverName { get; set; }

        // Additional Underwriting Question 15
        // ADD_UW_QUESTIONS::Dual_Controlled_Flag
        [DataMember]
        public TriState VehicleIsDualControl { get; set; }

        // Additional Underwriting Question 16
        // ADD_UW_QUESTIONS::Licence_Issued_Abroad_Flag
        [DataMember]
        public TriState DriverLicenceIsFromUsaCanadaNewZealandAustraliaHongKongOrWesternEurope { get; set; }

        // ADD_UW_QUESTIONS::Licence_Issued_Abroad_Driver
        [DataMember]
        public int DriverLicenceIsFromUsaCanadaNewZealandAustraliaHongKongOrWesternEuropeDriverNumber { get; set; }

        // Additional Underwriting Question 17
        // ADD_UW_QUESTIONS::Six_Month_Residency_Flag
        [DataMember]
        public TriState IsBritishSubjectWithAtLeast6MonthsResidence { get; set; }

        // Additional Underwriting Question 18
        // ADD_UW_QUESTIONS::Temporary_Break_Flag
        [DataMember]
        public TriState Has4YearsRegularDrivingExperienceAndLessThan1YearAbroad { get; set; }

        // Additional Underwriting Question 19
        // ADD_UW_QUESTIONS::Regular_Driver_Flag
        [DataMember]
        public TriState HasRegularDrivingExperienceDuringResidency { get; set; }

        // ADD_UW_QUESTIONS::Regular_Driver_Driver
        [DataMember]
        public int HasRegularDrivingExperienceDuringResidencyDriverNumber { get; set; }

        // Additional Underwriting Question 20
        // ADD_UW_QUESTIONS::Inner_City_Use_Flag
        [DataMember]
        public TriState VehicleUsedInInnerCity { get; set; }

        // Additional Underwriting Question 21
        // ADD_UW_QUESTIONS::Residency_Intention_Flag
        [DataMember]
        public TriState DriverIntentionToRemainInUkForAtLeast1Year { get; set; }

        // ADD_UW_QUESTIONS::Residency_Intention_Driver
        [DataMember]
        public int DriverIntentionToRemainInUkForAtLeast1YearDriverNumber { get; set; }

        // Additional Underwriting Question 22
        // ADD_UW_QUESTIONS::Car_Derived_Van_Flag
        [DataMember]
        public TriState NoClaimsDiscountWasEarnedOnCarDerivedVan { get; set; }

        // Additional Underwriting Question 23
        // ADD_UW_QUESTIONS::Offshore_One_Month_Flag
        [DataMember]
        public TriState DriverSpentOver1MonthOffshoreForOccupation { get; set; }

        // ADD_UW_QUESTIONS::Offshore_One_Month_Driver
        [DataMember]
        public int DriverSpentOver1MonthOffshoreForOccupationDriverNumber { get; set; }

        // Additional Underwriting Question 24
        // ADD_UW_QUESTIONS::Existing_EIS_Policy_Flag
        [DataMember]
        public TriState HasExistingEndsleighMotorPolicy { get; set; }

        // Additional Underwriting Question 25
        // ADD_UW_QUESTIONS::Daytime_Exposure_Flag
        [DataMember]
        public TriState VehicleExposedInStreetOrPublicCarParkDuringDay { get; set; }

        // Additional Underwriting Question 26
        // ADD_UW_QUESTIONS::Partner_Licence_Flag
        [DataMember]
        public TriState PartnerHoldsLicenceToDriveVehicle { get; set; }

        // Additional Underwriting Question 27
        // ADD_UW_QUESTIONS::Under_Twentyfive_Flag
        [DataMember]
        public TriState PersonsUnder25WillBeAbleToDriveVehicle { get; set; }

        // Additional Underwriting Question 28
        // ADD_UW_QUESTIONS::Previous_PD_Policy_Flag
        [DataMember]
        public TriState NoClaimsDiscountProtectionOnPreviousPolicy { get; set; }

        // Additional Underwriting Question 29
        // ADD_UW_QUESTIONS::Turbo_Cycle_Flag
        [DataMember]
        public TriState MotorCycleIsTurboCharged { get; set; }

        // Additional Underwriting Question 32
        // ADD_UW_QUESTIONS::Insured_Since_Purchase_Flag
        [DataMember]
        public TriState VehicleInsuredByImmediateFamilySincePurchase { get; set; }

        // Additional Underwriting Question 33
        // ADD_UW_QUESTIONS::Regular_Driver_2Years_Flag
        [DataMember]
        public TriState PoposerHasDrivenRegularlyForThePast2Years { get; set; }

        // Additional Underwriting Question 34
        // ADD_UW_QUESTIONS::Driver_Res_At_Home_Flag
        [DataMember]
        public TriState AdditionalDriversUnder25LiveAtHomeWithParents { get; set; }

        // Additional Underwriting Question 35
        // ADD_UW_QUESTIONS::Pass_Plus_Taken_Flag
        [DataMember]
        public TriState CompletedPassPlusInLast2Years { get; set; }

        // Additional Underwriting Question 36
        // ADD_UW_QUESTIONS::Use_Of_Other_Car_Flag
        [DataMember]
        public TriState ProposerOrSpouseHasUseOfAnotherVehicle { get; set; }

        // Additional Underwriting Question 37
        // ADD_UW_QUESTIONS::NCD_On_Other_Car_Flag
        [DataMember]
        public TriState ProposerOrSpouseHaveAtLeast4YearsNcdOnAnotherVehicle { get; set; }

        // Additional Underwriting Question 38
        // ADD_UW_QUESTIONS::chDriverIsNonSmoker
        [DataMember]
        public TriState ProposerIsNonSmoker { get; set; }

        // Additional Underwriting Question 39
        // ADD_UW_QUESTIONS::chDvlaAdvised
        [DataMember]
        public TriState DvlaHasBeenInformedOfDisabilityAndHasAuthorisedDriving { get; set; }

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
