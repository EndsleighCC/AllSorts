
#define EIS_WINNT

#include "D:\Repos\HeritageI\EIS\Include\eis.h"
#include "D:\Repos\HeritageI\DSOS\Include\dsos.h" 

    typedef struct _DRQ_DOCUMENT_DETAILS
    {
        CHAR    szDocumentCode[DSOS_CB_DOC_CODE+1]
            EIS_COM_PATH(IID_IDocument, DocumentCode, "EMP=1");

        CHAR    szPropertyItemDescription[DSOS_CB_REQ_DOCS_DESC + 1]
            EIS_COM_PATH(IID_IDocument, PropertyItemDescription, "");

        CHAR    szCoverSectionType[DSOS_CB_HSE_COVER_TYPE + 1]
            EIS_COM_PATH(IID_IDocument, CoverSectionType, "LKP=LkpPrimaryCoverSectionCodeToECoverSectionType");

        CHAR    szReceivedOnDate[DSOS_CB_STRING_DATE+1]
            EIS_COM_PATH(IID_IDocument, ReceivedOnDate, "");

        CHAR    szReceivedByTechRef[DSOS_CB_TECH_REF_SIZE]
            EIS_COM_PATH(IID_IDocument, ReceivedByTechRef, "");

        CHAR    szReceivedByCentreCode[DSOS_CB_CENTRE_CODE_SIZE]
            EIS_COM_PATH(IID_IDocument, ReceivedByCentreCode, "");

        CHAR    szStageRequiredCode[DSOS_CB_STAGE_REQUIRED_CODE_SIZE]
            EIS_COM_PATH(IID_IDocument, StageRequiredType, "LKP=LkpStageRequiredCodeToEStageRequiredType");

        CHAR    szFailureActionCode[DSOS_CB_FAILURE_ACTION_CODE_SIZE]
            EIS_COM_PATH(IID_IDocument, FailureActionType, "LKP=LkpFailureActionCodeToEFailureActionType");

    } DRQ_DOCUMENT_DETAILS; /* docd */

    typedef struct _DRQ_REF_INFO_U232
        {
            CHAR    szRefMsgNo[3 + 1];
            LONG    lRefMsgSector;
            LONG    lLoadValue;
            CHAR    chLoadType;

        }DRQ_REF_INFO_U232;

        typedef struct _DRQ_ADJ_PREM_BREAKDOWN
        {
            LONG    lDSOSRuleNo    /* D */
                EIS_COM_PATH(IID_IPremiumBreakdown, RuleNo, "EMP=1");
            CHAR    szRuleTitle[15+1]
                EIS_COM_PATH(IID_IPremiumBreakdown, RuleTitle, "");
            DSOS_FLOAT    dfLoadDiscount  /* DP 2 */
                EIS_COM_PATH(IID_IPremiumBreakdown, LoadDiscValue, "");
            CHAR    chLoadDiscountInd
                EIS_COM_PATH(IID_IPremiumBreakdown, LoadDiscCode, "");
            CHAR    chSource;
                /* OSQUERY - ??? */
            DSOS_FLOAT    dfPremium       /* DP 2 */
                EIS_COM_PATH(IID_IPremiumBreakdown, ResultantPremium, "");

        }DRQ_ADJ_PREM_BREAKDOWN;

        typedef struct _DRQ_VEH_INFO_U232
        {
            LONG    lVehNumber
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitLong, PRM=0")
                EIS_COM_PATH(IID_IVehicle, EIS_PROP_NULL, "FNC=fncInitLong, PRM=1");
            CHAR    szType[3 + 1];
                /* This field is mapped by hand */
            CHAR    chVehStatus
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    szVehEffectiveDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitDateDDMMCCYY, PRM=1")
                EIS_COM_PATH(IID_IMotorPolicy, CertificateDateTime, "FNC=fncIPolicy")
                EIS_COM_PATH(IID_IVehiclePolicyEffectiveDate, CertificateDateTime, "FNC=fncIPolicy");

            CHAR    szVehExpiryDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitDateDDMMCCYY, PRM=1")
                EIS_COM_PATH(IID_IVehicle2, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitDateDDMMCCYY")
                EIS_COM_PATH(IID_INewMotorPolicy, CertificateDateTime, "FNC=fncIPolicy");

            CHAR    szVehCode[DSOS_CB_VEH_CODE + 1]
                EIS_COM_PATH(IID_IVehicle, EIS_PROP_NULL, "FNC=fncInitMultiChar, PRM='*'")
                EIS_COM_PATH(IID_IVehicleVGRGDesc2, ABICode, "FNC=fncMapSubString, PRM=4");
                /* Only used on Veh 2, last 4 chars of ABI Code */

            CHAR    szABIVehCode[8 + 1]
                EIS_COM_PATH(IID_IVGRGDescription,ABICode,"");
            CHAR    szVehYearManuf[4 + 1]                   /* ACM - increased by 2 bytes on 11-Apr-1995 */
                EIS_COM_PATH(IID_IVehicle,YearOfManufacture,"");
            LONG    lVehAge
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncCalculateVehicleAge");
            LONG    lVehValue
                EIS_COM_PATH(IID_IVehicle, Value, "");
            CHAR    szVehRegNo[DSOS_CB_VEH_REG + 1]
                EIS_COM_PATH(IID_IVehicle, RegistrationNumber, "");
            CHAR    chRHDFlag
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'")
                EIS_COM_PATH(IID_IVehicle, IsRightHandDrive, "LKP=LkpOneZeroCharToTriState");
            CHAR    szVehSeatingCapacity[2 + 1]
                EIS_COM_PATH(IID_IVehicle, NumberOfSeats, "FNC=fncPrefixNumberWithZeros");
            CHAR    szVehPurchaseDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitDateDDMMCCYY, PRM=1")
                EIS_COM_PATH(IID_IVehicle, PurchaseDate, "");
            CHAR    chGarageInd
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IVehicle, ParkingLocation, "");
            CHAR    chVehModifiedFlag
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'")
                EIS_COM_PATH(IID_IVehicle, IsVehicleModified, "LKP=LkpYesNoFlagToBoolean");
            CHAR    aszVehModDetails[4][DSOS_CB_VEH_MODS_TEXT + 1]
                EIS_COM_PATH(IID_IVehicle, ModificationsText, "");
            CHAR    chVehOwnedFlag
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'")
                EIS_COM_PATH(IID_IVehicle, OwnedByProposer, "LKP=LkpOneZeroCharToTriState");
            CHAR    chVehOwnedByWhom
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IVehicle, Owner, "");
            CHAR    chRegisteredToFlag
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'")
                EIS_COM_PATH(IID_IVehicle, RegisteredToProposer, "LKP=LkpOneZeroCharToTriState");
            CHAR    chRegisteredToWhom
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IVehicle, RegisteredParty, "");
            CHAR    chWhereRegistered
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IVehicle, RegistrationType, "");

            CHAR    aszRegisteredToDetails[4][DSOS_CB_VEH_REG_OWNER_DTLS+1]
                EIS_COM_PATH(IID_IVehicle,OwnedOrRegisteredText,"");

            LONG    lRecordedMileage
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitLong, PRM=0")
                EIS_COM_PATH(IID_IVehicle, EIS_PROP_NULL, "FNC=fncInitLong, PRM=1");
                /* MART_OSQUERY - always 1 ? */
            LONG    lAnnualMileage
                EIS_COM_PATH(IID_IVehicle,TotalAnnualMileage,"");
            LONG    lAnnualBusMileage
                EIS_COM_PATH(IID_IVehicle,BusinessMileage,"");
            CHAR    chFreeFormatDescription
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'")
                EIS_COM_PATH(IID_IVehicle, IsVehicleMakeModelDescriptionModified, "LKP=LkpOneZeroCharToBoolean");

            CHAR    szVehMake[DSOS_CB_VEH_MAKE + 1]
                EIS_COM_PATH(IID_IVGRGDescription, VehicleMake, "");
            CHAR    szVehModel[DSOS_CB_VEH_FULLDESC + 1]
                EIS_COM_PATH(IID_IVGRGDescription, EIS_PROP_NULL, "DIR=ecddPropertyGet,"
                                                                  "FNC=fncCreateFullModelDesc")
                EIS_COM_PATH(IID_IVGRGDescription, ShortModel, "DIR=ecddPropertyPut");
            CHAR    szVehClass[DSOS_CB_VEH_CLASS + 1]
                EIS_COM_PATH(IID_IVGRGDescription, ClassOfUse, "");
            CHAR    szVehBodyType[DSOS_CB_VEH_BODY_TYPE + 1]
                EIS_COM_PATH(IID_IVGRGDescription, BodyType, "");
            CHAR    szVehCubicCapacity[4 + 1]
                EIS_COM_PATH(IID_IVGRGDescription, EngineCapacity, "FNC=fncPrefixNumberWithZeros");
            CHAR    chSpecialUseageFlag
                /* EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'") */
                /* EIS_COM_PATH(IID_IVehicle, EIS_PROP_NULL, "FNC=fncCalcVehSpecUsageFlag, LKP=LkpNumberCharToNumber"); */
                EIS_COM_PATH_NULL;
            CHAR    aszSpecialUseageDet[4][DSOS_CB_VEH_MODS_TEXT + 1]
                /* EIS_COM_PATH(IID_IVehcile,SpecialUsageText,""); */
                EIS_COM_PATH_NULL;
            CHAR    chGoodsDeliveryFlag
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'")
                EIS_COM_PATH(IID_IVehicle, IsUsedForGoodsDelivery, "LKP=LkpOneZeroCharToBoolean");
            CHAR    chDespatchUseFlag
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'")
                EIS_COM_PATH(IID_IVehicle, IsUsedForDispatch, "LKP=LkpOneZeroCharToBoolean");
            CHAR    szSideCarType[2 + 1]
                EIS_COM_PATH(IID_IVehicle,SideCarCode,"LKP=LkpNumberCharToNumber");
            CHAR    chSideCarSide
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'")
                EIS_COM_PATH(IID_IVehicle,IsSideCarLHS,"LKP=LkpOneZeroCharToBoolean");
            CHAR    szFinancialInterest[15 + 1];
                /* MART_OSQUERY - ??? */
            CHAR    chUsedForCommuting
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "FNC=fncGetOverallCommutingUse")
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
                /* MART_OSQUERY - ??? */
            CHAR    szVehGroup[2 + 1]
                EIS_COM_PATH(IID_IPremcalcCoverDetail, VehicleGroup, "")
                EIS_COM_PATH(IID_IUPCSVehGroup, VehicleGroup, "");
            CHAR    szSecurityDevice[10 + 1]
                EIS_COM_PATH(IID_IVehicle,SecurityDevice,"");
            CHAR    szSecurityDeviceDescription[50 + 1]
                EIS_COM_PATH(IID_IVehicle, SecurityDeviceText, "");
                /*EIS_COM_PATH(IID_IVehicle, SecurityDevice, "FNC=fncSecurityDeviceText")*/
            CHAR    chImported
                EIS_COM_PATH(IID_IVehicle, Imported, "LKP=LkpOneZeroCharToTriState")
        
            /* recursive mappings */
                EIS_COM_PATH(IID_ISchemeResult, PrimaryCoverSection,
                    "KEY=IID_IPrimaryCS, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IPrimaryCS, UnkProductCoverSection,
                    "KEY=IID_IPremcalcCoverDetail, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IVehicle, VGRGDescription,
                    "KEY=IID_IVGRGDescription, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IVehicle2, VGRGDescription,
                    "KEY=IID_IVehicleVGRGDesc2, RCA=ecraContainerTypeIgnoreRepeatCount")

                /* Mappings for vehicle group */
                EIS_COM_PATH(IID_IVehPolicyVehGroup, SelectedSchemeResult,
                    "KEY=IID_ISSRVehGroup, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_ISSRVehGroup, PrimaryCoverSection,
                    "KEY=IID_IPCSVehGroup, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IPCSVehGroup, UnkProductCoverSection,
                    "KEY=IID_IUPCSVehGroup, RCA=ecraContainerTypeIgnoreRepeatCount");

        }DRQ_VEH_INFO_U232;


        typedef struct _DRQ_ADJ_ACCIDENT
        {
            CHAR    chStatus /* DP */
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    szDate[DSOS_CB_STRING_DATE+1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitDateDDMMCCYY")
                EIS_COM_PATH(IID_IAccident, IncidentDate, "");
            CHAR    szType[4+1]
                EIS_COM_PATH(IID_IAccident, AccidentTypeCode, "DIR=ecddPropertyPut, EMP=1")
                EIS_COM_PATH(IID_IAccident, Type, "DIR=ecddPropertyGet");
            CHAR    szFaultBonusInd[2+1]
                EIS_COM_PATH(IID_IAccident, FaultBonusIndicator, "EMP=1");
            LONG    lTotalCosts  /* DP */
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitLong, PRM=0")
                EIS_COM_PATH(IID_IAccident, DamageCosts, "");
            LONG    lThirdPartyCosts    /* DP */
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitLong, PRM=0");
                /* MART_OSQUERY - Needs mapping for EIS Claims */
            LONG    lInjuryCosts   /* DP */
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitLong, PRM=0");
                /* MART_OSQUERY - Needs mapping for EIS Claims */
            CHAR    szEndsleighClaimInd[2+1];
                /* MART_OSQUERY - Needs mapping for EIS Claims */
            CHAR    chOutstandingClaimInd
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
                /* MART_OSQUERY - Needs mapping for EIS Claims */
            CHAR    szAccidentDesc[30+1];
                /* MART_OSQUERY - Needs mapping for EIS Claims */
            CHAR    chAccQuoteStatus;                         /* ACM - Added 6-Apr-95 */
                /* MART_OSQUERY_NOW - A = Added, D = Deleted, ' ' = default */
        }DRQ_ADJ_ACCIDENT;

        typedef struct _DRQ_ADJ_BANS
        {
            CHAR    chStatus
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    szReason[4+1]
                EIS_COM_PATH(IID_IBan, Code, "EMP=1");
            CHAR    szStartDate[DSOS_CB_STRING_DATE+1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitDateDDMMCCYY")
                EIS_COM_PATH(IID_IBan, StartDate, "");
            LONG    lPeriod
                EIS_COM_PATH(IID_IBan, DurationInWeeks, "");
            CHAR    chBanQuoteStatus;                        /* ACM - Added 6-Apr-95 */

        }DRQ_ADJ_BANS;

        typedef struct _DRQ_ADJ_CONVICTIONS
        {
            CHAR    chStatus
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    chType
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IConviction, Pending, "LKP=LkpPendingFlagToTriState");
            CHAR    szCode[4+1]
                EIS_COM_PATH(IID_IConviction, ResultantConvictionCode, "EMP=1");
            CHAR    szDate[DSOS_CB_STRING_DATE+1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitDateDDMMCCYY")
                EIS_COM_PATH(IID_IConviction, IncidentDate, "");
            LONG    lFineImposed /* DP */
                EIS_COM_PATH(IID_IConviction, FineAmount, "");
            LONG    lPointsImposed
                EIS_COM_PATH(IID_IConviction, PointsImposed, "");
            CHAR    chConQuoteStatus;               /* ACM - Added 6-Apr-1995 */

        }DRQ_ADJ_CONVICTIONS;

        typedef struct _DRQ_DRIVER_DATA_U232
        {
            CHAR    chDriverStatus
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IDriver, Status, "LKP=LkpAdjustStatusCharToEDriverStatus");
            CHAR    szDriverEffectiveDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL,"FNC=fncInitDateDDMMCCYY")
                EIS_COM_PATH(IID_IDriver, EffectiveDate, "");
            CHAR    szDriverExpiryDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL,"FNC=fncInitDateDDMMCCYY")
                EIS_COM_PATH(IID_IDriver, ExpiryDate, "");
            CHAR    szTitle[DSOS_CB_TITLE + 1]
                EIS_COM_PATH(IID_IPerson,Title,"EMP=1");
            CHAR    szInitials[DSOS_CB_INITIALS + 1]
                EIS_COM_PATH(IID_IPerson,Initials,"");
            CHAR    szSurname[DSOS_CB_SURNAME + 1]
                EIS_COM_PATH(IID_IPerson,Surname,"");
            CHAR    szForename[DSOS_CB_LONG_FORENAME + 1]
                EIS_COM_PATH(IID_IPerson,Forename, "TRNC=1");     /* 1.427 */
            CHAR    chSexInd
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IPerson,Gender,"");
            CHAR    chMaritalStatus
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IPerson, MaritalStatus, "");
            CHAR    chSpouseInd
                EIS_COM_PATH(IID_IDriver, RelationshipToProposer, "");
                /* not sure about PartnerOfClient
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'")
                EIS_COM_PATH(IID_IPerson, PartnerOfClient, "LKP=LkpOneZeroCharToBoolean"); */
            CHAR    szDoB[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitDateDDMMCCYY")
                EIS_COM_PATH(IID_IPerson, DateOfBirth, "");
            CHAR    szLicenceType[2 + 1]
                EIS_COM_PATH(IID_IDriver, LicenceType, "");
            CHAR    szLicenceDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitDateDDMMCCYY")
                EIS_COM_PATH(IID_IDriver,DateTestPassed,"");
            LONG    lUKResidence
                EIS_COM_PATH(IID_IPerson, UKResidencyPeriodInMonths, "");
            LONG    lOSCAFullTimeOcc
                EIS_COM_PATH(IID_IPerson, FullTimeOccupation, "IIF=eciifOSCACode" )
                /* EIS_COM_PATH(IID_IPerson, Union, "DIR=ecddPropertyGet, IIF=eciifOSCACode, FNC=fncIgnoreUnsetItemInfo") */ ;
            LONG    lOSCAPartTimeOcc
                 EIS_COM_PATH(IID_IPerson, PartTimeOccupation, "IIF=eciifOSCACode" );
            CHAR    szFullTimeOccCode[3 + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterZero")
                EIS_COM_PATH(IID_IPerson, FullTimeOccupation, "");
            CHAR    szFullTimeOccSector[3 + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterZero")
                EIS_COM_PATH(IID_IPerson, FullTimeOccupation, "DIR=ecddPropertyGet, FNC=fncSetOccupationSectorFromOccupation");
            CHAR    szFullTimeOccUnion[2 + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterZero")
                EIS_COM_PATH(IID_IPerson, Union, "");
            CHAR    szFullTimeOccDesc[30 + 1]
                EIS_COM_PATH(IID_IPerson, EIS_PROP_NULL, "FNC=fncFullTimeOccupationText, PRM=1");
            CHAR    szPartTimeOccCode[3 + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterZero")
                EIS_COM_PATH(IID_IPerson, PartTimeOccupation, "FNC=fncIgnoreIfNULL");
            CHAR    szPartTimeOccSector[3 + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterZero")
                EIS_COM_PATH(IID_IPerson, PartTimeOccupation, "DIR=ecddPropertyGet, FNC=fncSetOccupationSectorFromOccupation");
            CHAR    szPartTimeOccUnion[2 + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterZero")
                EIS_COM_PATH(IID_IPerson, Union, "");
            CHAR    szPartTimeOccDesc[30 + 1]
                EIS_COM_PATH(IID_IPerson, EIS_PROP_NULL, "FNC=fncPartTimeOccupationText, PRM=1");
            CHAR    chCommuteFlag
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'")
                EIS_COM_PATH(IID_IDriver, CommutingUse, "LKP=LkpOneZeroCharToTriState");
            CHAR    chBusUseFlag
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'")
                EIS_COM_PATH(IID_IDriver, BusinessUse, "LKP=LkpYesNoFlagToTriState");
            CHAR    chPrevDecInd
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IDriver, PreviouslyDeclined, "");
            LONG    lDisabilitiesCount
                EIS_COM_PATH(IID_IDriver, MedicalConditions, "DIR=ecddPropertyGet, FNC=fncGetCollectionCount");

        #if defined(EIS_TRANSLATOR_INVOKED)
            CHAR    aszDisabDesc_1[4 + 1]
                EIS_COM_PATH(IID_IDriver, MedicalConditions,
                             "CIX=1, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IMedicalCondition_1, PRP=1")
                EIS_COM_PATH(IID_IMedicalCondition_1, Code, "EMP=1");
            CHAR    aszDisabDesc_2[4 + 1]
                EIS_COM_PATH(IID_IDriver, MedicalConditions,
                             "CIX=2, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IMedicalCondition_2, PRP=1")
                EIS_COM_PATH(IID_IMedicalCondition_2, Code, "EMP=1");
            CHAR    aszDisabDesc_3[4 + 1]
                EIS_COM_PATH(IID_IDriver, MedicalConditions,
                             "CIX=3, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IMedicalCondition_3, PRP=1")
                EIS_COM_PATH(IID_IMedicalCondition_3, Code, "EMP=1");
            CHAR    aszDisabDesc_4[4 + 1]
                EIS_COM_PATH(IID_IDriver, MedicalConditions,
                             "CIX=4, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IMedicalCondition_4, PRP=1")
                EIS_COM_PATH(IID_IMedicalCondition_4, Code, "EMP=1");
            CHAR    aszDisabDesc_5[4 + 1]
                EIS_COM_PATH(IID_IDriver, MedicalConditions,
                             "CIX=5, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IMedicalCondition_5, PRP=1")
                EIS_COM_PATH(IID_IMedicalCondition_5, Code, "EMP=1");
        #else
            CHAR    aszDisabDesc[5][4 + 1];
        #endif

            LONG    lAccidentCount
                EIS_COM_PATH(IID_IDriver, Accidents, "DIR=ecddPropertyGet, FNC=fncGetCollectionCount");
            LONG    lConvictionsCount
                EIS_COM_PATH(IID_IDriver, Convictions, "DIR=ecddPropertyGet, FNC=fncGetCollectionCount");
            LONG    lBansCount
                EIS_COM_PATH(IID_IDriver, Bans, "DIR=ecddPropertyGet, FNC=fncGetCollectionCount");

            CHAR    szTestPassedDate[DSOS_CB_STRING_DATE + 1]  /* ACM - added 12-Oct-94 */
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitDateDDMMCCYY")
                EIS_COM_PATH(IID_IDriver, DateTestPassed, "");
            LONG    lPeriodLicenceHeld                         /* ACM - added 12-Oct-94 */
                EIS_COM_PATH(IID_IDriver, PeriodLicenceHeldInMonths, "");

            CHAR    chGraduate                                                 
                EIS_COM_PATH(IID_IPerson, Graduate, "");
            CHAR    szEmployersBusinessCategory[DSOS_CB_BUSINESS_CATEGORY+1]   
                EIS_COM_PATH(IID_IPerson, EmployerBusiness, "");
            CHAR    szGraduationDate[DSOS_CB_STRING_DATE+1]
                EIS_COM_PATH(IID_IPerson, GraduationDate, "");
            CHAR    szEducationEstablishmentCode[DSOS_CB_EDU_ESTB_CODE+1]
                EIS_COM_PATH(IID_IStudentPerson, EducationalEstablishment, "FNC=fncIStudentPerson");
            CHAR    chTermTimeResidence                                        
                EIS_COM_PATH(IID_IStudentPerson, TermTimeResidence, "FNC=fncIStudentPerson");
            CHAR    szCourseStudied[DSOS_CB_COURSE_CODE+1]                                         
                EIS_COM_PATH(IID_IStudentPerson, CourseStudied, "FNC=fncIStudentPerson");
            CHAR    szNUSCardNumber[DSOS_CB_NUS_EXTRA_CARD+1]                                         
                EIS_COM_PATH(IID_IStudentPerson, NUSCardNumber, "FNC=fncIStudentPerson");
            CHAR    szTeachingLocation[DSOS_CB_TEACHING_LOCATION+1]                                         
                EIS_COM_PATH_NULL;
            CHAR    szTeachingEstablishment[DSOS_CB_EDU_ESTB_CODE+1]                                         
                EIS_COM_PATH_NULL;
            CHAR    szTeachingSubject[DSOS_CB_TEACHING_SUBJECT+1]                                         
                EIS_COM_PATH_NULL;
            CHAR    szDriveOtherVehicles[ DSOS_CB_DRIVE_OTHER_VEHICLES_SIZE ]
                EIS_COM_PATH(IID_IDriver, DriveOtherVehicles, "");

            DRQ_ADJ_ACCIDENT    aaaAccidents[9]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "KEY=IID_Initialise")
                EIS_COM_PATH(IID_IDriver, Accidents, "KEY=IID_IAccident");
            DRQ_ADJ_BANS        aabBans[5]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "KEY=IID_Initialise")
                EIS_COM_PATH(IID_IDriver, Bans, "KEY=IID_IBan");
            DRQ_ADJ_CONVICTIONS aacConvictions[9]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "KEY=IID_Initialise")
                EIS_COM_PATH(IID_IDriver, Convictions, "KEY=IID_IConviction")

                EIS_COM_PATH(IID_IPerson, StudentPerson, "RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IStudentPerson")
                EIS_COM_PATH(IID_IDriver, Person,
                    "KEY=IID_IPerson,RCA=ecraContainerTypeIgnoreRepeatCount");

        }DRQ_DRIVER_DATA_U232;

        typedef struct _DRQ_ADJ_QANDA
        {
            LONG    lLineNo;    /* D */
            CHAR    szQandA[DSOS_CB_LINE_LENGTH+1];

        }DRQ_ADJ_QANDA;

#define OSM_CB_COVER_OPTION_TYPE_LENGTH (4)
#define OSM_CB_COVER_OPTION_TYPE_SIZE OSM_CB_COVER_OPTION_TYPE_LENGTH+1

#if defined( EIS_PACKING_REQUIRED )
#pragma pack(1)
#endif
typedef EIS_PACKED struct _OSM_COVER_OPTION_SELECTION_DETAILS
{
    CHAR szTypeCode[ OSM_CB_COVER_OPTION_TYPE_SIZE ]
        EIS_COM_PATH( IID_ICoverSectionTranslator_PutGet , TypeCode , "EMP=1" )
        EIS_COM_PATH( IID_ICoverOptionRequestDetails , TypeCode , "EMP=1" ) ;
    double dblPremium
        EIS_COM_PATH( IID_ICoverSectionTranslator_PutGet , TotalPremium , "DIR=ecddPropertyPut" )
        EIS_COM_PATH( IID_ICoverSectionTranslator_PutGet , AgreedTotalPremium , "" )
        EIS_COM_PATH( IID_ICoverOptionRequestDetails , Premium , "" ) ;
    double dblCommissionRatePercent
        EIS_COM_PATH( IID_ICoverSectionTranslator_PutGet , SchemeCommissionRate, "DIR=ecddPropertyPut")
        EIS_COM_PATH( IID_ICoverSectionTranslator_PutGet , CompositeCommissionRate, "")
        EIS_COM_PATH( IID_ICoverOptionRequestDetails , CommissionRate , "" ) ;
    CHAR chHidden
        EIS_COM_PATH( IID_ICoverSectionTranslator_PutGet , Hidden , "LKP=LkpYesNoFlagToBoolean" ) ;
    CHAR chDisplayablePremium
        EIS_COM_PATH( IID_ICoverSectionTranslator_PutGet , DisplayablePremium , "LKP=LkpYesNoFlagToBoolean" ) ;
    LONG lSortIndex
        EIS_COM_PATH( IID_ICoverSectionTranslator_PutGet , SortIndex , "" ) ;
    CHAR chCoverSectionSelectionControl
        EIS_COM_PATH( IID_ICoverSectionTranslator_PutGet , CoverSectionSelectionControl , "LKP=LkpCoverSectionSelectionControl" )
        EIS_COM_PATH( IID_ICoverSectionTranslator_PutGet , CoverSectionStatus, "DIR=ecddPropertyPut,"
                                                                               "FNC=fncSetCoverSectionStatus,"
                                                                               "PRM=ecssAccept" ) ;
    /* Referee */
    CHAR chPrimaryCoverElement
        EIS_COM_PATH(IID_ICoverSectionTranslator_PutGet, PrimaryCoverElement, "LKP=LkpYesNoFlagToBoolean") ;
    
    CHAR chSelected
        EIS_COM_PATH( IID_ICoverSectionTranslator_PutGet , Selected , "LKP=LkpYesNoFlagToBoolean" )
        EIS_COM_PATH( IID_ICoverOptionRequestDetails , Requested , "LKP=LkpYesNoFlagToBoolean" )
       
        /* Recursive mappings */

        /* NB. The property 'Type' is not actually mapped to here.  It is used to get the */
        /*     ResolveBaseInterface mapping to run in the custom function. */
        EIS_COM_PATH( IID_ICoverSectionTranslator_CoverOptionArrayMember, Type,
                                                                          "RCA=ecraContainerTypeIgnoreRepeatCount,"
                                                                          "FNC=fncGetICoverSectionTranslator,"
                                                                          "KEY=IID_ICoverSectionTranslator_PutGet" ) ;

} OSM_COVER_OPTION_SELECTION_DETAILS ; /* covosd */

        typedef struct _DRQ_ADJUSTMENT_SAVE_IN
        {
            CHAR    szUserRef[DSOS_CB_USER_REFERENCE+1]
                EIS_COM_PATH(IID_IMotorAdjQuote, EIS_PROP_NULL, "FNC=fncInitUserRef");

            /* section 1 */
            CHAR    chTransactionStatus;
            CHAR    chTransactionSuccess;
            CHAR    chAcceptType
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'");
                /* MART_OSQUERY - ??? */

            CHAR    szDSOSCaseRef[12 + 1]
                EIS_COM_PATH(IID_IMotorAdjQuote, AdjQuoteNumber, "FNC=fncIAdjQuote");
            CHAR    chQuoteStatus
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
                /* MART_OSQUERY - ??? */
            CHAR    szScheme[DSOS_CB_SCHEME+1]
                EIS_COM_PATH(IID_IMotorPolicy, SchemeName, "FNC=fncIPolicy");
            LONG    lPolicyNumber
                EIS_COM_PATH(IID_IMotorPolicy, PolicyNumberOnly, "FNC=fncIPolicy");
            CHAR    szClientNumber[DSOS_CB_CLIENT_NUMBER + 1]
                EIS_COM_PATH(IID_IMotorPolicy, ClientNumber, "FNC=fncIPolicy");
            CHAR    szQuoteEffectiveDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_IMotorAdjQuote, QuotationDate, "FNC=fncIAdjQuote");
            CHAR    szQuoteEffectiveTime[DSOS_CB_TIME + 1]
                EIS_COM_PATH(IID_IMotorPolicy, CertificateDateTime, "FNC=fncMapCertificateDateTimeAsTime");
            CHAR    szExpiryDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitDateDDMMCCYY, PRM=1");
                /* MART_OSQUERY - Blank in DSOS */
                /*EIS_COM_PATH(IID_IMotorPolicy, RenewalDateTime, "FNC=fncIPolicy");*/
            CHAR    szQuoteEnteredDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_IMotorPolicy, EntryDateTime, "FNC=fncIPolicy");
            CHAR    szDateLastPremCalc[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_IMotorPolicy, EntryDateTime, "FNC=fncIPolicy");
            CHAR    chOutstandingRefFlag
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'");
                /* MART_OSQUERY - should be referal status */
            CHAR    chQuoteOnCoverFlag
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorAdjQuote, QuoteOnCover, "FNC=fncIAdjQuote, LKP=LkpYesNoFlagToBoolean");
            CHAR    chQuoteCoverNoted
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'");
                /* MART_OSQUERY - should be cover note status */

            CHAR    szUserRefForWaive[DSOS_CB_USER_REFERENCE + 1]
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, WaiveTechRef, "");
            CHAR    szUserRefForCommDisc[DSOS_CB_USER_REFERENCE + 1]
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, DiscountTechRef, "");
            CHAR    szUserRefForPremCalc[DSOS_CB_USER_REFERENCE + 1]
                EIS_COM_PATH(IID_IMotorPolicy, UserTechRef, "FNC=fncIPolicy");

            DRQ_DOCUMENT_DETAILS addDocumentDetails[DSOS_CB_DOCS_REQ]
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, RequiredDocuments, "KEY=IID_IDocument, PRP=1")
                EIS_COM_PATH(IID_ICoverSectionDup, RequiredDocuments, "KEY=IID_IDocument, PRP=1");

            CHAR    szPolicyInceptionDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_IOriginalMotorPolicy, CertificateDateTime, "FNC=fncIPolicy");
            CHAR    szOriginalPolicyIncepDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_IOriginalMotorPolicy, CertificateDateTime, "FNC=fncIPolicy");
            CHAR    szPolicyExpiryDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_IOriginalMotorPolicy, ExpiryDateTime, "FNC=fncIPolicy");
            CHAR    chPolicyInRenewal
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'");
                /* MART_OSQUERY - ??? */
            CHAR    szPolicyTerm[2 + 1]
                EIS_COM_PATH(IID_IMotorCover, PeriodOfCover, "");
            CHAR    chStdNonStdInd
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='S'");
                /* MART_OSQUERY - ??? */
            LONG    lAdjustmentNum
                EIS_COM_PATH(IID_IMotorPolicy, NumberOfAdjustmentsPerformed, "FNC=fncIPolicy");
            LONG    lClaimsCount;
                /* MART_OSQUERY - ??? */
            LONG    lActiveMainVehicle
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitLong, PRM=1");
                /* MART_OSQUERY - ??? */
            LONG    lVehicleRecordCount
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitLong, PRM=1");
                /* MART_OSQUERY - ??? */
            LONG    lGreenCardCount
                EIS_COM_PATH(IID_IGreencard, NumberOfGreencardsUsed, "");
            LONG    lGreenCardDaysCount
                EIS_COM_PATH(IID_IGreencard, DaysPreviouslyIssued, "");
            CHAR    chWindscreenExtFlag
                /*EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'")*/
                EIS_COM_PATH(IID_IMotorCover, WindscreenCover, "LKP=LkpYesNoFlagToTriState")
                EIS_COM_PATH(IID_IPremcalcCoverRestriction, WindscreenCoverAllowed, "LKP=LkpYesNoFlagToTriState");
            CHAR    szPostCode[DSOS_CB_POSTCODE + 1]
                EIS_COM_PATH(IID_IMotorCover, RiskPostcode, "FNC=fncSpecialFormatPostcode");
            CHAR    chRiskAddress
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");

            CHAR    szUseClass[DSOS_CB_CLASS_USE + 1]
                EIS_COM_PATH(IID_IMotorCover, ClassOfUse, "")
                EIS_COM_PATH(IID_IPremcalcCoverRestriction, EnforcedClassOfUse,
                                                "DIR=ecddPropertyGet,FNC=fncIgnoreUnsetItemInfo");
            CHAR    szPolicyCoverType[DSOS_CB_COVER_TYPE + 1]
                EIS_COM_PATH(IID_ICalculationType, CalculationLevel,"FNC=fncCoverTypeFromECalculationLevel");
            CHAR    szPolVolDriverRestrict[2 + 1]
                EIS_COM_PATH (IID_IMotorCover, VoluntaryDrivingRestriction, "")
                EIS_COM_PATH(IID_IPremcalcCoverRestriction, EnforcedDrivingRestriction,
                                                "DIR=ecddPropertyGet,FNC=fncIgnoreUnsetItemInfo");
            LONG    lPolExcessValue
                EIS_COM_PATH(IID_ICalculationType,VoluntaryExcess,"")
                EIS_COM_PATH(IID_IPremcalcCoverRestriction, EnforcedExcess,
                                                "DIR=ecddPropertyGet,FNC=fncIgnoreUnsetItemInfo");
            LONG    lNCDYears
                EIS_COM_PATH(IID_IDrivingExperience, NCDYearsClaimed, "")
                EIS_COM_PATH(IID_IPremcalcCoverRestriction, NCDYearsAllowed, "");
            CHAR    chHalfYearsNCD
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='0'");
                /* MART_OSQUERY - ??? */
            LONG    lNCDInUseFlag
                EIS_COM_PATH(IID_IDrivingExperience, NCDInUse, "LKP=LkpOneZeroNumToTriState") ;
            LONG    lMaxNCDWithIns;
                /* MART_OSQUERY - ??? */
            CHAR    szNCDEarntOn[2 + 1]
                EIS_COM_PATH(IID_IDrivingExperience, NoClaimsType, "");
            CHAR    chProtectedNCDFlag
                EIS_COM_PATH(IID_ICalculationType, ProtectedDiscount, "LKP=LkpYesNoFlagToTriState")
                EIS_COM_PATH(IID_IPremcalcCoverRestriction, ProtectedDiscountAllowed, "LKP=LkpYesNoFlagToBoolean");
            CHAR    szWhereNCDEarnt[2 + 1]
                EIS_COM_PATH(IID_IDrivingExperience, NoClaimsCountry, "");
            CHAR    chIntroDiscFlag
                EIS_COM_PATH (IID_INewPolCoverSection_Primary,IntroductoryNCDGiven, "LKP=LkpYesNoFlagToBoolean");
                /* EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'"); */
            LONG    lContinuousProtNCDYears;
                /* MART_OSQUERY - ??? */
            LONG    lLoyaltyYears
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, SchemeLoyaltyYears, "");
                /* MART_OSQUERY - ??? */
            CHAR    chAgreedValueInd
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'");
            CHAR    chMainDriverInd;
                /* EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='1'"); */
                
            LONG    lHighestRatedDriver
                EIS_COM_PATH(IID_IPremcalcCoverDetail, HighestRatedDriver, "");

            CHAR    aszTermsCodes[12][2 + 1]
                EIS_COM_PATH(IID_IAdjustmentResult, EIS_PROP_NULL, "FNC=fncOSCATermsCodes" );

            CHAR    szCertificateCode[DSOS_CB_COV_CERT_CODE + 1]
                EIS_COM_PATH(IID_IMotorAdjQuote, ModifiedCertCode, "");
            CHAR    szInsurersAreaCode[DSOS_CB_AREA_CODE + 1]
                EIS_COM_PATH(IID_IPremcalcCoverDetail, AreaCode, "");
            CHAR    chVIPClient;
                /* MART_OSQUERY - ??? */
            LONG    lNumberOfVehsInFamily
                EIS_COM_PATH(IID_IMotorCover, VehiclesInHousehold, "");

            CHAR    chSinglePolicyDocument
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'");
                /* MART_OSQUERY - ??? */
            CHAR    chInstallmentInd
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='R'");
                /* MART_OSQUERY - ??? */
            CHAR    chOutstandingClaims
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
                /* MART_OSQUERY - ??? */
            LONG    lMonthsVehOwned;
                /* MART_OSQUERY - ??? */
            CHAR    szNotes[50 + 1]
                EIS_COM_PATH(IID_IMotorAdjQuote, Notes, "FNC=fncIAdjQuote")
                EIS_COM_PATH(IID_IGreencard, Notes, "DIR=ecddPropertyGet, FNC=fncIgnoreIfNULL");
                /* EIS_COM_PATH(IID_IPayment, RefundNotes, "DIR=ecddPropertyGet, FNC=fncIgnoreIfNULL")  1.474
                EIS_COM_PATH(IID_IUnderwritingFactors, UWNotes, "FNC=fncIgnoreIfNULL") 1.474*/


            /* Net Rate Fields */
            float flSchemeCommissionRatePercent
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, SchemeCommissionRate, "");
            float flCommissionDiscountPercent
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, CommissionDiscountRate, "");
            float flCompositeCommissionRatePercent
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, CompositeCommissionRate, "") ;
            float flRiskOnlyCalcInsPremAmount
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, BasePremiumRiskOnly, "") ;
            float flCompositeRateCIPAmount
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, BasePremium, "");
            float flCommissionAmount
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, CommissionAmount, "") ;
            CHAR chSchemeRatingType /* Gross Rate or Net Rate */
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, SchemeBaseRateType, "LKP=LkpBaseRateTypeCharToEBaseRateType");

            /* windmill stuff abCompositeRateCIPAmount */   
            CHAR        chChildrenUnder16Flag
                EIS_COM_PATH(IID_IMotorPolicy, HaveChildrenUnder16, "FNC=fncIPolicy, LKP=LkpOneZeroCharToTriState");
            CHAR        chNonMotoringConvictionFlag
                EIS_COM_PATH(IID_IMotorCover, AnyNonMotoringConvictions, "LKP=LkpOneZeroCharToTriState");
            CHAR        chHomeOwnerFlag
                EIS_COM_PATH(IID_IMotorCover, HomeOwner, "LKP=LkpOneZeroCharToTriState");
            DSOS_FLOAT  dfExpiringAnnualisedCIP
                EIS_COM_PATH(IID_INewPolCoverSection_Primary, BasePremium, "") ;  
            CHAR        chAutoRenewalConsent
                EIS_COM_PATH(IID_IPayment,AutoRenewalConsent,"LKP=LkpYesNoFlagToTriState") ;

            /* Heisenberg */
            double dblClaimsHandlingExpenses
                EIS_COM_PATH(IID_IMotorPolicy, ClaimsHandlingExpenses, "");
            double dblVariableExpensesCurrency
                EIS_COM_PATH(IID_IMotorPolicy, VariableExpensesAmount, "");
            double dblVariableExpensesPercent
                EIS_COM_PATH(IID_IMotorPolicy, VariableExpensesPercent, "");
            double dblMarkupPercentage
                EIS_COM_PATH(IID_IMotorPolicy, Markup, "");
            CHAR   chPriceOptimisationAvailable
                EIS_COM_PATH(IID_IMotorPolicy, PriceOptimisation, "LKP=LkpPriceOptimisationAvailable");


            CHAR    chPreAdjPremCalcFlag
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            DSOS_FLOAT    dfChangeInProRataPremsInclIPT
                EIS_COM_PATH(IID_IMotorAdjQuote, ChangeInProRataPremsInclIPT, "");
            DSOS_FLOAT    dfPreAdjProrataAmount
                EIS_COM_PATH(IID_IMotorAdjQuote, PreAdjPremium, "");
            DSOS_FLOAT    dfRateCappedAmount
                EIS_COM_PATH_NULL; /* Rate Capping was never implemented */
            DSOS_FLOAT    dfPostAdjTruePremium /* Includes ULR */
                EIS_COM_PATH(IID_IPolSchemeResult,TotalPremium,"");
            DSOS_FLOAT    dfPostAdjProrataAmount
                EIS_COM_PATH(IID_IMotorAdjQuote, PostAdjPremium, "");
            DSOS_FLOAT    dfAdminFeePercentage /* The current level of FAF - whether or not it is active for this quote */
                EIS_COM_PATH(IID_IMotorAdjQuote, AdjustmentFee, "");
            DSOS_FLOAT    dfAvailablePremium
                EIS_COM_PATH_NULL; /* Rate Capping was never implemented */
            DSOS_FLOAT    dfAvailableCommission
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, FlatRateAdminFeeAmount, "")
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, VariableAdminFeeAmount, "FNC=fncAddAmountToProperty");
            CHAR    szAvailablePremiumDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitDateDDMMCCYY, PRM=1");
            DSOS_FLOAT    dfPremiumPaidToDate
                EIS_COM_PATH(IID_IOriginalMotorPolicy,CashPaidToDate,"FNC=fncIPolicy");
            CHAR    chCommissionDiscFlag /* Adjustment Modelling is always waive - never comm discounting */
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'");
            DSOS_FLOAT    dfCommissionDiscAmount /* Really Admin fee percentage */
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, AdminFeeRate, "");
            DSOS_FLOAT    dfTotalPremiumDuePaid
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, AgreedTotalPremium, "")
                EIS_COM_PATH(IID_IPaymentGreencard, RequiredPayment, "")
                EIS_COM_PATH(IID_IPaymentDup, RequiredPayment, "");
            DSOS_FLOAT    dfWaivedPremium
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, WaiveAmount, "");
            DSOS_FLOAT    dfCalcPremium
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, AgreedTotalPremium, "")
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, FlatRateAdminFeeAmount, "FNC=fncDeductAmountFromProperty")
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, VariableAdminFeeAmount, "FNC=fncDeductAmountFromProperty");
            DSOS_FLOAT    dfPreAdjTruePremium;
                /* SL3607 - This field should not be populated with PreAdjTruePremium after all as it is not        */
                /* quite as redundant as first appeared! The U232 server apears to write the contents of this field */
                /* to the database and PMF displays the value as 'Entered Premium' (in the same area used by        */
                /* Greencard to represent monies which are non-refundable in the event of a cancellation)           */
                /*EIS_COM_PATH(IID_IOriginalSchemeResult,TotalPremium,"");*/
            DSOS_FLOAT    dfAdminFee
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, VariableAdminFeeAmount, "")
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, FlatRateAdminFeeAmount, "FNC=fncAddAmountToProperty")
                EIS_COM_PATH(IID_IPaymentDup, RequiredPayment, "");
            CHAR    chULRActive
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='Y'");
                /* MART_OSQUERY - ??? */
            CHAR    chULRAddedThisQuote
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
                /* MART_OSQUERY - ??? */
            DSOS_FLOAT    dfULRGrossPremium
                EIS_COM_PATH(IID_ICoverSection_ULR,TotalPremium,"FNC=fncMapIfSelected");
            DSOS_FLOAT    dfULRCommission;
                /* MART_OSQUERY - ??? */
            CHAR    chDAPActive
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'");
                /* MART_OSQUERY - ??? */
            CHAR    chDAPAddedThisQuote
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
                /* MART_OSQUERY - ??? */
            DSOS_FLOAT    dfDAPGrossPremium;
                /* MART_OSQUERY - ??? */
            DSOS_FLOAT    dfDAPCommission /* The amount of FAF for this particular quote */
                EIS_COM_PATH(IID_IMotorAdjQuote, AdjustmentFee, "FNC=fncAbortIfAdjFeeSelected");
            CHAR    chCreditRisk;
                /* MART_OSQUERY - ??? */
            LONG    lLoad;
                /* MART_OSQUERY - ??? */
            LONG    lDiscount;
                /* MART_OSQUERY - ??? */

            CHAR    szCashSourceCode1[8 + 1]
                EIS_COM_PATH(IID_ISourceCode_1, GlobalSourceCode, "FNC=fncISourceCode, EMP=1");
            DSOS_FLOAT    dfAmntSourceCode1
                EIS_COM_PATH(IID_ISourceCode_1, ValueWithoutFee, "FNC=fncISourceCode");
            CHAR    chTypeSourceCode1
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_ISourceCode_1, TypeIndicatorChar, "FNC=fncISourceCode");

            CHAR    szCashSourceCode2[8 + 1]
                EIS_COM_PATH(IID_ISourceCode_2, GlobalSourceCode, "FNC=fncISourceCode, EMP=1");
            DSOS_FLOAT    dfAmntSourceCode2
                EIS_COM_PATH(IID_ISourceCode_2, ValueWithoutFee, "FNC=fncISourceCode");
            CHAR    chTypeSourceCode2
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_ISourceCode_2, TypeIndicatorChar, "FNC=fncISourceCode");

            CHAR    chManualCheqInd
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'")
                EIS_COM_PATH(IID_IPayment, RefundType, "LKP=LkpManualChequeCodeToERefundType");
            CHAR    szManualCheqReason[2 + 1]
                EIS_COM_PATH(IID_IPayment, RefundType, "DIR=ecddPropertyGet, FNC=fncManualChequeCodeToReason");
            CHAR    chInformPremiumCredit
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM='N'");
                /* MART_OSQUERY - ??? */
            CHAR    chCertificateIssued
                EIS_COM_PATH(IID_IMotorPolicy, DupCertificateIssued, "LKP=LkpYesNoFlagToBoolean");

        #if defined(EIS_TRANSLATOR_INVOKED)
            CHAR    szEndorsements_1[3 + 1]
                EIS_COM_PATH(IID_IMotorAdjQuote, Endorsements, "FNC=fncIAdjQuote, "
                                            "CIX=1, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IEndorsements_1, PRP=1")
                EIS_COM_PATH(IID_IEndorsements_1, EndorsementCode, "EMP=1");
            CHAR    szEndorsements_2[3 + 1]
                EIS_COM_PATH(IID_IMotorAdjQuote, Endorsements, "FNC=fncIAdjQuote, "
                                            "CIX=2, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IEndorsements_2, PRP=1")
                EIS_COM_PATH(IID_IEndorsements_2, EndorsementCode, "EMP=1");
            CHAR    szEndorsements_3[3 + 1]
                EIS_COM_PATH(IID_IMotorAdjQuote, Endorsements, "FNC=fncIAdjQuote, "
                                            "CIX=3, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IEndorsements_3, PRP=1")
                EIS_COM_PATH(IID_IEndorsements_3, EndorsementCode, "EMP=1");
            CHAR    szEndorsements_4[3 + 1]
                EIS_COM_PATH(IID_IMotorAdjQuote, Endorsements, "FNC=fncIAdjQuote, "
                                            "CIX=4, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IEndorsements_4, PRP=1")
                EIS_COM_PATH(IID_IEndorsements_4, EndorsementCode, "EMP=1");
            CHAR    szEndorsements_5[3 + 1]
                EIS_COM_PATH(IID_IMotorAdjQuote, Endorsements, "FNC=fncIAdjQuote, "
                                            "CIX=5, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IEndorsements_5, PRP=1")
                EIS_COM_PATH(IID_IEndorsements_5, EndorsementCode, "EMP=1");
            CHAR    szEndorsements_6[3 + 1]
                EIS_COM_PATH(IID_IMotorAdjQuote, Endorsements, "FNC=fncIAdjQuote, "
                                            "CIX=6, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_IEndorsements_6, PRP=1")
                EIS_COM_PATH(IID_IEndorsements_6, EndorsementCode, "EMP=1");
        #else
            CHAR    szEndorsements[6][3 + 1];
        #endif

            /* Adjustment quote referral messages */
            CHAR    szTechNotes[120 + 1]
                EIS_COM_PATH(IID_IUnderwritingFactors, UWNotes, "");
            CHAR    szReferralDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitDateDDMMCCYY, PRM=1")
                EIS_COM_PATH(IID_IUnderwritingFactors, StartDate, "");
            CHAR    chIntroForce
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    chProtDiscForce
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            LONG    lHighestRatedDriverForce;
            CHAR    szVolDriverRestForce[2 + 1];
            CHAR    chAgreedValueForce
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    szAreaCodeOveride[2 + 1]
                EIS_COM_PATH(IID_IUnderwritingFactors, AreaCodeOverride, "");
            CHAR    szVehGroupOveride[2 + 1]
                EIS_COM_PATH(IID_IUnderwritingFactors, VehicleGroupOverride,"FNC=fncAddVehicleGroupOverride");
            LONG    lDriveAgeOveride
                EIS_COM_PATH(IID_IUnderwritingFactors, DriverAgeOverride, "");
            LONG    lNCDOveride
                EIS_COM_PATH(IID_IUnderwritingFactors, NCDOverride, "");
            LONG    lVehAgeOveride
                EIS_COM_PATH(IID_IUnderwritingFactors, VehicleAgeOverride, "");
            LONG    lVehValueOveride
                EIS_COM_PATH(IID_IUnderwritingFactors, VehicleValueOverride, "");
            CHAR    chMaritalStatusOveride
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    aszAdditionalTermsCode[8][2 + 1];
            CHAR    aszDeletedTermsCode[8][2 + 1];

            DRQ_REF_INFO_U232      ariRefInfo[10];

            CHAR    szAddDocs[12 + 1];
            CHAR    chReferalInhibitFlag
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, CoverSectionStatus, "LKP=LkpInhibitCharToECoverSectionStatus");
            CHAR    szReferTechRef[4 + 1]
                EIS_COM_PATH(IID_IUnderwritingFactors, UWTechRef, "");

            DRQ_ADJ_PREM_BREAKDOWN apbPremiumBreakdown[45]
                EIS_COM_PATH(IID_IPolSchemeResult, PremiumBreakdowns, "KEY=IID_IPremiumBreakdown, PRP=1");

            /*  Green Card Details */
            LONG    lGCDaysCount
                EIS_COM_PATH(IID_IGreencard, NumberOfDaysRequested, "DIR=ecddPropertyGet");
            CHAR    szGCEffectiveDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitDateDDMMCCYY, PRM=1")
                EIS_COM_PATH(IID_IGreencard,EffectiveDate, "DIR=ecddPropertyGet");
            CHAR    szGCExpiryDate[DSOS_CB_STRING_DATE + 1]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitDateDDMMCCYY, PRM=1")
                EIS_COM_PATH(IID_IGreencard,ExpiryDate, "DIR=ecddPropertyGet");
            DSOS_FLOAT    dfGCPremium
                EIS_COM_PATH(IID_IGreencard, Premium, "DIR=ecddPropertyGet");
            DSOS_FLOAT    dfGCAdminFee
                EIS_COM_PATH(IID_IGreencard, AdminFee, "DIR=ecddPropertyGet");
            LONG    lVehicleNum
                /* this value is always = 1 */
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitLong, PRM=1");
            LONG    lDriverNum
                /* this value is always = 1 */
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "DIR=ecddPropertyGet, FNC=fncInitLong, PRM=1");
            CHAR    chCategory1
                /* car = 'A', m/c = 'B' */
                EIS_COM_PATH(IID_IVehicle, Type, "DIR=ecddPropertyGet, LKP=lkpVehTypeToGreencardCategory1");
            CHAR    chCategory2
                /* trailer = 'F', no trailer = ' ' */
                EIS_COM_PATH(IID_IGreencard, Trailer, "DIR=ecddPropertyGet, LKP=lkpTrailerToGreencardCategory2");
            CHAR    chBailBond
                EIS_COM_PATH(IID_IGreencard, BailBond, "DIR=ecddPropertyGet, LKP=LkpYesNoFlagToBoolean");
            CHAR    chEireOnly
                EIS_COM_PATH(IID_IGreencard, Countries, "DIR=ecddPropertyGet,LKP=lkpIrelandStatusFromCountryCode");
        #if defined(EIS_TRANSLATOR_INVOKED)
            CHAR    szCountryCodes[3+1]
                EIS_COM_PATH(IID_IGreencard, Countries, "DIR=ecddPropertyGet, FNC=fncConvertGCCountryToString");
            CHAR    aszCountryCodes[49][3+1];
        #else
            CHAR    aszCountryCodes[50][3+1];
        #endif

            /* section 2 */
        #if defined(EIS_TRANSLATOR_INVOKED)
            DRQ_VEH_INFO_U232 eVehInfo1
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "KEY=IID_Initialise")
                EIS_COM_PATH(IID_IVehiclePolicy1, SelectedSchemeResult, "KEY=IID_ISchemeResult")
                EIS_COM_PATH(IID_IVehiclePolicy1, EIS_PROP_NULL, "KEY=IID_IMotorPolicy")
                EIS_COM_PATH(IID_IVehiclePolicy1, Vehicle, "KEY=IID_IVehicle")
                EIS_COM_PATH(IID_IVehiclePolicy2, EIS_PROP_NULL, "KEY=IID_INewMotorPolicy")
                EIS_COM_PATH(IID_IVehiclePolicyVehGroup, EIS_PROP_NULL, "KEY=IID_IVehiclePolicyVehGroup");

            DRQ_VEH_INFO_U232 eVehInfo2
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "KEY=IID_Initialise")
                EIS_COM_PATH(IID_IVehiclePolicy2, SelectedSchemeResult, "KEY=IID_ISchemeResult")
                EIS_COM_PATH(IID_IVehiclePolicy2, EIS_PROP_NULL, "KEY=IID_IMotorPolicy")
                EIS_COM_PATH(IID_IVehiclePolicy2, Vehicle, "KEY=IID_IVehicle")
                EIS_COM_PATH(IID_IVehiclePolicy2, Vehicle, "KEY=IID_IVehicle2");

            DRQ_VEH_INFO_U232 eVehInfoSpare
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "KEY=IID_Initialise");
        #else
            DRQ_VEH_INFO_U232 aeVehInfo[3];
        #endif

            /* section 3 */
            DRQ_DRIVER_DATA_U232 ddDriverData[9]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "KEY=IID_Initialise")
                EIS_COM_PATH(IID_IMotorPolicy, Drivers, "KEY=IID_IDriver");

            /* section 4 */
            DRQ_ADJ_QANDA aqaQandA[48];
                /* MART_OSQUERY - ??? */

        #if defined(EIS_TRANSLATOR_INVOKED)

            CHAR    ch_szUnderWritingAnswers_0_chAirportParking
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=1, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_1_chAirportParkingDvr
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=1, FNC=fncSetUWDriverNo, LKP=LkpNumberCharToNumber");
            CHAR    ch_szUnderWritingAnswers_2_chLicenceExpiryDvr
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=2, FNC=fncSetUWDriverNo, LKP=LkpNumberCharToNumber");
            CHAR    ch_szUnderWritingAnswers_3_chLicenceExpiry
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=2, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_4_chFurnishedCaravanette
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=3, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_5_chGreenCardPeriod
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=4, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_6_chSecurityDevFlag
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=5, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_7_chVehExposure
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=6, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_8_chPreviousFire
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=7, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_9_chConcurrentIncident
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=8, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_10_chConcurrentIncidentDvr
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=8, FNC=fncSetUWDriverNo, LKP=LkpNumberCharToNumber");
            CHAR    ach_szUnderWritingAnswers_11_szEarnedLocationCode[ 2 ]
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IDrivingExperience, NoClaimsCountry, "");
            CHAR    ch_szUnderWritingAnswers_13_chBlank
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    ch_szUnderWritingAnswers_14_chAddlDvrOwnCar
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=10, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_15_chUnderAgeOwnCar
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=11, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_16_chYoungestDvrOwn2ndCar
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=12, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_17_chMOTAvailable
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=13, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_18_chPolicyAbroad
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=14, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_19_chDualControlled
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=15, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_20_chLicenceIssuedAbroad
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=16, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_21_chLicenceIssuedAbroadDvr
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=16, FNC=fncSetUWDriverNo, LKP=LkpNumberCharToNumber");
            CHAR    ch_szUnderWritingAnswers_22_chSixMonthResidency
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=17, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_23_chTemporaryBreak
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=18, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_24_chRegularDvr
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=19, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_25_chRegularDvrDvr
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=19, FNC=fncSetUWDriverNo, LKP=LkpNumberCharToNumber");
            CHAR    ch_szUnderWritingAnswers_26_chInnerCityUse
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=20, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_27_chResidencyIntention
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=21, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_28_chResidencyIntentionDvr
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=21, FNC=fncSetUWDriverNo, LKP=LkpNumberCharToNumber");
            CHAR    ch_szUnderWritingAnswers_29_chCarDerivedVan
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=22, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_30_chOffshoreOneMonth
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=23, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_31_chOffshoreOneMonthDvr
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=23, FNC=fncSetUWDriverNo, LKP=LkpNumberCharToNumber");
            CHAR    ch_szUnderWritingAnswers_32_chExistingEISPolicy
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=24, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_33_chDaytimeExposure
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=25, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_34_chPartnerLicence
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=26, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_35_chUnderRiskAgeDrive
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=27, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_36_chPreviousPDPolicy
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=28, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_37_chCarelessDrivingDvr
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=9, FNC=fncSetUWDriverNo, LKP=LkpNumberCharToNumber");
            CHAR    ch_szUnderWritingAnswers_38_chCarelessDriving
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=9 , FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_39_chTurboCycle
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=29, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_40_chInsSincePurchase
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=32, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_41_chRegularDvr2Yrs
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=33, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");

            CHAR    ch_szUnderWritingAnswers_42_chDvrResAtHome
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=34, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_43_chPassPlusTaken
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=35, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_44_chUseOfOtherCar
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=36, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");

            /* The function I ripped these mappings from (DSOS SvrAddUWAnsToDrq) specified that slots */
            /* 45 to 80 were spare, but the rest of the UWQ mappings have the following settings. At  */
            /* least these are my best guess - an extra field seems to have appeared from somewhere.  */

            CHAR    ch_szUnderWritingAnswers_45_chNCDOnOtherCarFlag
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=37, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_46_chPropertyType
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IUnusedQuote, PropertyType, "");
            CHAR    ch_szUnderWritingAnswers_47_chNatureOfTenure
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorCover, HomeOwner, "LKP=LkpHomeOwnerToNatureOfTenure");

            CHAR    ach_szUnderWritingAnswers_48to54_Spare[ 7 ]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitMultiChar, PRM=' '");

            CHAR    ch_szUnderWritingAnswers_55_chDriverIsNonSmoker
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=38, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");
            CHAR    ch_szUnderWritingAnswers_56_chDvlaAdvised
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "CIX=39, FNC=fncGetUWResponse, LKP=LkpYesNoFlagToTriState");

            CHAR    ach_szUnderWritingAnswers_57to80_Spare[ 24 ]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitMultiChar, PRM=' '");

            CHAR    ach_szUnderWritingAnswers_81_szLoyaltyToEisYears[ 2 ]
                EIS_COM_PATH(IID_IMotoCover, EISLoyaltyYears, "FNC=fncPrefixNumberWithZeros");
            CHAR    ach_szUnderWritingAnswers_83_szLoyaltyToInsYears[ 2 ]
                EIS_COM_PATH(IID_IAdjCoverSection_Primary, SchemeLoyaltyYears, "FNC=fncPrefixNumberWithZeros");

            CHAR    ch_szUnderWritingAnswers_85_chBlank
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    ch_szUnderWritingAnswers_86_chBlank
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");

            CHAR    ch_szUnderWritingAnswers_87_chUsualOccDvr
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL,"FNC=fncInitChar,PRM='1'");
            CHAR    ach_szUnderWritingAnswers_88_szUsualOcc[ 3 ]
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "FNC=fncUsualOccClientDriver");
            CHAR    ch_szUnderWritingAnswers_91_chBlank
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    ach_szUnderWritingAnswers_92_szUsualOccDesc[ 30 ]
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "FNC=fncMainDriverUsualOccupationText");
            CHAR    ch_szUnderWritingAnswers_122_chBlank
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    ach_szUnderWritingAnswers_123_szUsualOccSpouse[ 3 ]
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "FNC=fncUsualOccSpouseDriver");
            CHAR    ch_szUnderWritingAnswers_126_chBlank
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    ach_szUnderWritingAnswers_127_szUsualOccDescSpouse[ 30 ]
                EIS_COM_PATH(IID_IMotorPolicy, EIS_PROP_NULL, "FNC=fncSpouseDriverUsualOccupationText");
            CHAR    ch_szUnderWritingAnswers_157_chBlank
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    ach_szUnderWritingAnswers_158_szSecDev[ 10 ]
                EIS_COM_PATH(IID_IVehicle, SecurityDevice, "");
            CHAR    ch_szUnderWritingAnswers_168_chBlank
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    ach_szUnderWritingAnswers_169_szSecDevDesc[ 50 ]
                EIS_COM_PATH(IID_IVehicle, SecurityDeviceText, "");
            CHAR    ch_szUnderWritingAnswers_219_chBlank
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");

            CHAR    ch_szUnderWritingAnswers_220_vown_chOwnerCode
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IVehicle, Owner, "");
            CHAR    ch_szUnderWritingAnswers_221_chCreditPlanType
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '");
            CHAR    ach_szUnderWritingAnswers_222_achAccDK[ 9 ]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterZero");
            CHAR    ach_szUnderWritingAnswers_231_achConvDK[ 9 ]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterZero");
            CHAR    ch_szUnderWritingAnswers_240_chBanDK
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterZero");
            CHAR    ch_szUnderWritingAnswers_241_chPndConvDK
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterZero");
            CHAR    ch_szUnderWritingAnswers_242_chDisabDK
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterZero");

            CHAR    ch_szUnderWritingAnswers_243_chRegType
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IVehicle, RegistrationType, "");
            CHAR    ch_szUnderWritingAnswers_244_chRegToCode
                EIS_COM_PATH(IID_Initialise,EIS_PROP_NULL, "FNC=fncInitChar, PRM=' '")
                EIS_COM_PATH(IID_IVehicle, RegisteredParty, "");

            CHAR    ch_szUnderWritingAnswers_245_szVehOwnershipMonths[ 2 ]
                EIS_COM_PATH(IID_IUnusedQuote, LenVehOwnership, "FNC=fncPrefixNumberWithZeros");

            CHAR    ch_szUnderWritingAnswers_247_achDvrMarriedInd[ 8 ]
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitFieldToCharacterSpace");

            CHAR    ch_szUnderWritingAnswers_255_chNull
                EIS_COM_PATH(IID_Initialise, EIS_PROP_NULL, "FNC=fncInitChar,PRM=0") ;

        #else
            CHAR    szUnderWritingAnswers[256 + 1] ;
        #endif

            OSM_COVER_OPTION_SELECTION_DETAILS acovosd[ DSOS_MAX_PREMCALC_COVER_OPTION_COUNT ]
                EIS_COM_PATH(IID_IAdjustmentResult, AdditionalCoverSections,
                                                    "KEY=IID_ICoverSectionTranslator_CoverOptionArrayMember, PRP=1")

                /* Recursive mappings */

                EIS_COM_PATH(IID_IMotorPolicy,Vehicle,
                    "KEY=IID_IVehicle,RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IMotorPolicy,Drivers,
                    "KEY=IID_IDriver,RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IMotorPolicy,Cover,
                    "KEY=IID_IMotorCover,,RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IMotorPolicy,Greencard,
                    "KEY=IID_IGreencard,RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IMotorPolicy,SelectedCalculationType,
                    "KEY=IID_ISelectedCalculationType,RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IMotorPolicy,UnderwritingQuestions,
                    "KEY=IID_IUnderwritingQuestions,RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IMotorAdjQuote, Payments,
                    "FNC=fncIAdjQuote, CIX=1, KEY=IID_IPayment, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IPayment, SourceCodes,
                    "CIX=1, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_ISourceCode_1, PRP=1")
                EIS_COM_PATH(IID_IPayment, SourceCodes,
                    "CIX=2, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_ISourceCode_2, PRP=1")
                EIS_COM_PATH(IID_IPaymentGreencard, SourceCodes,
                    "CIX=1, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_ISourceCode_1, PRP=1")
                EIS_COM_PATH(IID_IPaymentGreencard, SourceCodes,
                    "CIX=2, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_ISourceCode_2, PRP=1")
                EIS_COM_PATH(IID_IPaymentDup, SourceCodes,
                    "CIX=1, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_ISourceCode_1, PRP=1")
                EIS_COM_PATH(IID_IPaymentDup, SourceCodes,
                    "CIX=2, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_ISourceCode_2, PRP=1")
                EIS_COM_PATH(IID_IPaymentDup, SchemeResults,
                    "CIX=1, RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_ISchemeResultDup, PRP=1")
                EIS_COM_PATH(IID_ISchemeResultDup, PrimaryCoverSection,
                    "RCA=ecraContainerTypeIgnoreRepeatCount, KEY=IID_ICoverSectionDup")

                /* Current Motor Policy mappings */
                EIS_COM_PATH(IID_IMotorPolicy, SelectedCalculationType,
                    "KEY=IID_ICalculationType,RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IMotorAdjQuote, CurrentMotorPolicy,
                    "KEY=IID_IMotorPolicy, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IMotorAdjQuote, OriginalMotorPolicy,
                    "KEY=IID_IOriginalMotorPolicy, RCA=ecraContainerTypeIgnoreRepeatCount")

                /* Original Motor Policy mappings */
                EIS_COM_PATH(IID_IOriginalMotorPolicy, SelectedSchemeResult,
                    "KEY=IID_IOriginalSchemeResult, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IOriginalSchemeResult, AdditionalCoverSections,
                     "RCA=ecraContainerTypeIgnoreRepeatCount, FNC=fncGetULRCoverSection, KEY=IID_IOriginalCoverSection_ULR")
                EIS_COM_PATH(IID_IOriginalSchemeResult, AdditionalCoverSections,
                    "RCA=ecraContainerTypeIgnoreRepeatCount, FNC=fncGetULRCoverSection, KEY=IID_ICoverSection_ULR")

                EIS_COM_PATH(IID_IMotorAdjQuote, AdjustmentResult,
                    "KEY=IID_IAdjustmentResult, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IAdjustmentResult, PrimaryCoverSection,
                    "KEY=IID_IAdjCoverSection_Primary, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IMotorPolicy, SelectedSchemeResult,
                    "KEY=IID_IPolSchemeResult, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IPolSchemeResult, PrimaryCoverSection,
                    "KEY=IID_INewPolCoverSection_Primary, RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_INewPolCoverSection_Primary, UnkProductCoverSection,
                    "FNC=fncCreatePremcalcCoverDetail,RCA=ecraContainerTypeIgnoreRepeatCount,KEY=IID_IPremcalcCoverDetail")
                EIS_COM_PATH(IID_IMotorCover,DrivingExperience,
                    "KEY=IID_IDrivingExperience,RCA=ecraContainerTypeIgnoreRepeatCount")
                EIS_COM_PATH(IID_IMotorPolicy,UnderwritingFactors,
                    "RCA=ecraContainerTypeIgnoreRepeatCount,KEY=IID_IUnderwritingFactors")

                EIS_COM_PATH(IID_IPolSchemeResult, UnkProductSchemeResult,
                             "FNC=fncCreatePremcalcCoverRestriction,"
                             "RCA=ecraContainerTypeIgnoreRepeatCount,"
                             "KEY=IID_IPremcalcCoverRestriction");

        } DRQ_ADJUSTMENT_SAVE_IN ;
