
   typedef DSOS_PACKED struct _APPC_DOCUMENT_DETAILS
   {
       CHAR    achDocumentCode[DSOS_CB_DOC_CODE] ;
       CHAR    achReceivedOnDate[DSOS_CB_STRING_DATE];      
       CHAR    achReceivedByTechRef[DSOS_CB_TECH_REF_LENGTH];
       CHAR    achReceivedByCentreCode[DSOS_CB_CENTRE_CODE_LENGTH];
       CHAR    achStageRequiredCode[DSOS_CB_STAGE_REQUIRED_CODE_LENGTH];
       CHAR    achFailureActionCode[DSOS_CB_FAILURE_ACTION_CODE_LENGTH];

   } APPC_DOCUMENT_DETAILS ; /* docd */

   typedef DSOS_PACKED struct _APPC_ADJ_PREM_BREAKDOWN
   {
       BYTE    abDSOSRuleNo[4];    /* D */
       CHAR    achRuleTitle[15];
       BYTE    abLoadDiscount[4];  /* DP 2 */
       CHAR    chLoadDiscountInd;
       CHAR    chSource;
       BYTE    abPremium[4];       /* DP 2 */
   }
   APPC_ADJ_PREM_BREAKDOWN;

   typedef DSOS_PACKED struct _APPC_ADJ_QANDA
   {
       BYTE    abLineNo[2];    /* D */
       CHAR    achQandA[DSOS_CB_LINE_LENGTH];

   }
   APPC_ADJ_QANDA;

   typedef DSOS_PACKED struct _APPC_ADJ_ACCIDENT_U222
   {
       CHAR    chStatus;
       CHAR    achDate[DSOS_CB_STRING_DATE];
       CHAR    achType[4];
       CHAR    achFaultBonusInd[2];
       BYTE    abTotalCosts[3];        /* DP */
       BYTE    abThirdPartyCosts[3];   /* DP */
       BYTE    abInjuryCosts[3];       /* DP */
       CHAR    achEndsleighClaimInd[2];
       CHAR    chOutstandingClaimInd;
       CHAR    achAccidentDesc[30];
       CHAR    chAccQuoteStatus;                        /* ACM - Added 6-Apr-95 */

   }
   APPC_ADJ_ACCIDENT_U222;
   
   typedef DSOS_PACKED struct _APPC_ADJ_BANS_U222
   {
       CHAR    chStatus;
       CHAR    achReason[4];
       CHAR    achStartDate[DSOS_CB_STRING_DATE];
       BYTE    abPeriod[4];
       CHAR    chBanQuoteStatus;                        /* ACM - Added 6-Apr-95 */

   }
   APPC_ADJ_BANS_U222;

   typedef DSOS_PACKED struct _APPC_ADJ_CONVICTIONS_U222
   {
       CHAR    chStatus;
       CHAR    chType;
       CHAR    achCode[4];
       CHAR    achDate[DSOS_CB_STRING_DATE];
       BYTE    abFineImposed[3];               /* DP */
       BYTE    abPointsImposed[2];
       CHAR    chConQuoteStatus;                        /* ACM - Added 6-Apr-95 */

   }
   APPC_ADJ_CONVICTIONS_U222;

   /*  U232  */
   typedef APPC_ADJ_ACCIDENT_U222    APPC_ADJ_ACCIDENT_U232;
   typedef APPC_ADJ_BANS_U222        APPC_ADJ_BANS_U232;
   typedef APPC_ADJ_CONVICTIONS_U222 APPC_ADJ_CONVICTIONS_U232;

   typedef DSOS_PACKED struct _APPC_VEH_INFO_U232
   {
       BYTE    abVehNumber[2]; /* D */
       CHAR    achType[3];
       CHAR    chVehStatus;
       CHAR    achVehEffectiveDate[DSOS_CB_STRING_DATE];
       CHAR    achVehExpiryDate[DSOS_CB_STRING_DATE];
       CHAR    achVehCode[DSOS_CB_VEH_CODE];
       CHAR    achABIVehCode[8];
       CHAR    achVehYearManuf[4];                         /* ACM - increased by 2 bytes on 11-Apr-1995 */
       BYTE    abVehAge[2]; /* D */
       BYTE    abVehValue[3]; /* DP */
       CHAR    achVehRegNo[DSOS_CB_VEH_REG];
       CHAR    chRHDFlag;
       CHAR    achVehSeatingCapacity[2];
       CHAR    achVehPurchaseDate[DSOS_CB_STRING_DATE];
       CHAR    chGarageInd;
       CHAR    chVehModifiedFlag;
       CHAR    achVehModDetails[4][DSOS_CB_VEH_MODS_TEXT];
       CHAR    chVehOwnedFlag;
       CHAR    chVehOwnedByWhom;
       CHAR    chRegisteredToFlag;
       CHAR    chRegisteredToWhom;
       CHAR    chWhereRegistered;
       CHAR    achRegisteredToDetails[4][DSOS_CB_VEH_REG_OWNER_DTLS];
       BYTE    abRecordedMileage[4]; /* DP */
       BYTE    abAnnualMileage[3];  /* DP */
       BYTE    abAnnualBusMileage[3]; /* DP */
       CHAR    chFreeFormatDescription;                /* ACM - Added on 11-Apr-1995 */
       CHAR    achVehMake[DSOS_CB_VEH_MAKE];
       CHAR    achVehModel[DSOS_CB_VEH_FULLDESC];
       CHAR    achVehClass[DSOS_CB_VEH_CLASS];
       CHAR    achVehBodyType[DSOS_CB_VEH_BODY_TYPE];
       CHAR    achVehCubicCapacity[4];
       CHAR    chSpecialUseageFlag;
       CHAR    achSpecialUseageDet[4][DSOS_CB_VEH_MODS_TEXT];
       CHAR    chGoodsDeliveryFlag;
       CHAR    chDespatchUseFlag;
       CHAR    achSideCarType[2];
       CHAR    chSideCarSide;
       CHAR    achFinancialInterest[15];
       CHAR    chUsedForCommuting;
       CHAR    achVehGroup[2];
       CHAR    achSecurityDevice[10];
       CHAR    achSecurityDeviceDescription[50];
	   CHAR	   chImported;	
       CHAR    achFiller[88]; /* PB reduced filler by 1 byte and another 1 byte as seems to be the wrong size */

   }
   APPC_VEH_INFO_U232;

   typedef APPC_VEH_INFO_U232 *APPC_PVEH_INFO_U232;


   typedef DSOS_PACKED struct _APPC_DRIVER_DATA_U232
   {
       CHAR    chDriverStatus;
       CHAR    achDriverEffectiveDate[DSOS_CB_STRING_DATE];
       CHAR    achDriverExpiryDate[DSOS_CB_STRING_DATE];
       CHAR    achTitle[DSOS_CB_TITLE];
       CHAR    achInitials[DSOS_CB_INITIALS];
       CHAR    achSurname[DSOS_CB_SURNAME];
       CHAR    achForename[DSOS_CB_LONG_FORENAME];
       CHAR    chSexInd;
       CHAR    chMaritalStatus;
       CHAR    chSpouseInd;
       CHAR    achDoB[DSOS_CB_STRING_DATE];
       CHAR    achLicenceType[2];
       CHAR    achLicenceDate[DSOS_CB_STRING_DATE];
       BYTE    abUKResidence[3]; /* DP */
       BYTE    abOSCAFullTimeOcc[2]; /* D */
       BYTE    abOSCAPartTimeOcc[2]; /* D */
       CHAR    achFullTimeOccCode[3];
       CHAR    achFullTimeOccSector[3];
       CHAR    achFullTimeOccUnion[2];
       CHAR    achFullTimeOccDesc[30];
       CHAR    achPartTimeOccCode[3];
       CHAR    achPartTimeOccSector[3];
       CHAR    achPartTimeOccUnion[2];
       CHAR    achPartTimeOccDesc[30];
       CHAR    chCommuteFlag;
       CHAR    chBusUseFlag;
       CHAR    chPrevDecInd;
       BYTE    bDisabilitiesCount;     /* ACM - changed from indicator to count */
       CHAR    achDisabDesc[5][4];
       BYTE    bAccidentCount;
       BYTE    bConvictionsCount;
       BYTE    abBansCount[2];
       CHAR    achTestPassedDate[DSOS_CB_STRING_DATE];     /* ACM -  Added 12-Oct 94 */
       BYTE    abPeriodLicenceHeld[3];                     /* Packed ACM - Added 12-Oct 94 */
        
       CHAR    chGraduate;                                                 
       CHAR    achEmployersBusinessCategory[DSOS_CB_BUSINESS_CATEGORY]; 
       CHAR    achGraduationDate[DSOS_CB_STRING_DATE];
       CHAR    achEducationEstablishmentCode[DSOS_CB_EDU_ESTB_CODE];
       CHAR    chTermTimeResidence       ;                                 
       CHAR    achCourseStudied[DSOS_CB_COURSE_CODE]   ;                                      
       CHAR    achNUSCardNumber[DSOS_CB_NUS_EXTRA_CARD]     ;                                    
       CHAR    achTeachingLocation[DSOS_CB_TEACHING_LOCATION];                                     
       CHAR    achTeachingEstablishment[DSOS_CB_EDU_ESTB_CODE];                                
       CHAR    achTeachingSubject[DSOS_CB_TEACHING_SUBJECT];   
	   CHAR	   achDriveOtherVehicles[ DSOS_CB_DRIVE_OTHER_VEHICLES_LENGTH ];   	   

       APPC_ADJ_ACCIDENT_U232    aaaAccidents[9];
       APPC_ADJ_BANS_U232        aabBans[5];
       APPC_ADJ_CONVICTIONS_U232 aacConvictions[9];

   }
   APPC_DRIVER_DATA_U232;


   typedef DSOS_PACKED struct _APPC_REF_INFO_U232
   {
       CHAR    achRefMsgNo[3];
       BYTE    abRefMsgSector[2]; /* D */
       BYTE    abLoadValue[2];    /* DP */
       CHAR    chLoadType;

   }
   APPC_REF_INFO_U232;

   typedef APPC_REF_INFO_U232 *APPC_PREF_INFO_U232;


   typedef DSOS_PACKED struct _APPC_U232_C002
   {
       CHAR    achBufferID[DSOS_CB_TRANS_BUFFER_ID];
       CHAR    achTransactionID[DSOS_CB_TRANS_BUFFER_ID];
       CHAR    achNextBufferID[DSOS_CB_TRANS_BUFFER_ID];

   }
   APPC_U232_C002;

   typedef APPC_U232_C002 *APPC_PU232_C002;

   typedef DSOS_PACKED struct _APPC_U232_C003
   {
       CHAR    achBufferID[DSOS_CB_TRANS_BUFFER_ID];
       CHAR    achTransactionID[DSOS_CB_TRANS_BUFFER_ID];
       CHAR    achNextBufferID[DSOS_CB_TRANS_BUFFER_ID];
       CHAR    achAdjustmentID[3];

   }
   APPC_U232_C003;

   typedef APPC_U232_C003 *APPC_PU232_C003;

   typedef DSOS_PACKED struct _APPC_U232_A
   {
       CHAR    achTransactionID[DSOS_CB_TRANS_BUFFER_ID];
       CHAR    achNextBufferID[DSOS_CB_TRANS_BUFFER_ID];

       /* Basic Details */
       CHAR    chAcceptType;
       CHAR    achDSOSCaseRef[12];
       CHAR    chQuoteStatus;
       CHAR    achScheme[DSOS_CB_SCHEME];
       BYTE    abPolicyNumber[7];  /*Decimal */
       CHAR    achClientNumber[DSOS_CB_CLIENT_NUMBER];
       CHAR    achQuoteEffectiveDate[DSOS_CB_STRING_DATE];
       CHAR    achQuoteEffectiveTime[DSOS_CB_TIME];
       CHAR    achExpiryDate[DSOS_CB_STRING_DATE];
       CHAR    achQuoteEnteredDate[DSOS_CB_STRING_DATE];
       CHAR    achDateLastPremCalc[DSOS_CB_STRING_DATE];
       CHAR    chOutstandingRefFlag;
       CHAR    chQuoteOnCoverFlag;
       CHAR    chQuoteCoverNoted;
       CHAR    achUserRefForWaive[ 10 ];
       CHAR    achUserRefForCommDisc[ 10 ];
       CHAR    achUserRefForPremCalc[ 10 ];

       APPC_DOCUMENT_DETAILS addDocumentDetails[ DSOS_CB_DOCS_REQ ] ;

       CHAR    achPolicyInceptionDate[DSOS_CB_STRING_DATE];
       CHAR    achOriginalPolicyIncepDate[DSOS_CB_STRING_DATE];
       CHAR    achPolicyExpiryDate[DSOS_CB_STRING_DATE];
       CHAR    chPolicyInRenewal;
       CHAR    achPolicyTerm[2];
       CHAR    chStdNonStdInd;
       BYTE    abAdjustmentNum[2];
       BYTE    abClaimsCount[2];
       BYTE    abActiveMainVehicle[2];
       BYTE    abVehicleRecordCount[2]; /* D */
       BYTE    abGreenCardCount[2];     /* Decimal */
       BYTE    abGreenCardDaysCount[2]; /* Decimal Packed */
       CHAR    chWindscreenExtFlag;
       CHAR    achPostCode[DSOS_CB_POSTCODE];
       CHAR    chRiskAddress;
       CHAR    achUseClass[DSOS_CB_CLASS_USE];
       CHAR    achPolicyCoverType[DSOS_CB_COVER_TYPE];
       CHAR    achPolVolDriverRestrict[2];
       BYTE    abPolExcessValue[2]; /* DP */
       BYTE    abNCDYears[2];      /* D */
       CHAR    chHalfYearsNCD;
       BYTE    bNCDInUseFlag;
       BYTE    abMaxNCDWithIns[2]; /* D */
       CHAR    achNCDEarntOn[2];
       CHAR    chProtectedNCDFlag;
       CHAR    achWhereNCDEarnt[2];
       CHAR    chIntroDiscFlag;
       BYTE    bContinuousProtNCDYears;
       BYTE    abLoyaltyYears[2];
       CHAR    chAgreedValueInd;
       CHAR    chMainDriverInd;
       BYTE    bHighestRatedDriver;
       CHAR    achTermsCodes[12][2];
       CHAR    achCertificateCode[DSOS_CB_COV_CERT_CODE];
       CHAR    achInsurersAreaCode[DSOS_CB_AREA_CODE];
       CHAR    chVIPClient;
       BYTE    bNumberOfVehsInFamily;
       CHAR    chSinglePolicyDocument;
       CHAR    chInstallmentInd;
       CHAR    chOutstandingClaims;
       BYTE    abMonthsVehOwned[2]; /* DP */
       CHAR    achNotes[50];
       CHAR    chPreAdjPremCalcFlag;
       BYTE    abChangeInProRataPremsInclIPT[4];  /* DP 2 */
       BYTE    abPreAdjProrataAmount[4]; /* DP 2 */
       BYTE    abRateCappedAmount[4];   /* DP 2 */
       BYTE    abPostAdjTruePremium[4];  /* DP 2 */
       BYTE    abPostAdjProrataAmount[4]; /* DP 2 */
       BYTE    abAdminFeePercentage[3]; /* Packed decimal */
       BYTE    abAvailablePremium[4]; /* DP 2 */
       BYTE    abAvailableCommission[4]; /* DP 2 */
       CHAR    achAvailablePremiumDate[DSOS_CB_STRING_DATE]; /* DP */
       BYTE    abPremiumPaidToDate[5];         /* DP 2 */
       CHAR    chCommissionDiscFlag;
       BYTE    abCommissionDiscAmount[4]; /* DP 2 */
       BYTE    abWaivedPremium[4];         /* DP 2 */
       BYTE    abTotalPremiumDuePaid[4];     /* DP 2 */
       BYTE    abCalcPremium[4];           /* DP 2 */
       BYTE    abPreAdjTruePremium[4];     /* DP 2 */
       BYTE    abAdminFee[4];              /* DP 2 */
       CHAR    chULRActive;
       CHAR    chULRAddedThisQuote;
       BYTE    abULRGrossPremium[4];       /* DP 2 */
       BYTE    abULRCommission[4];        /* DP 2 */
       CHAR    chDAPActive;
       CHAR    chDAPAddedThisQuote;
       BYTE    abDAPGrossPremium[4];   /* DP 2 */
       BYTE    abDAPCommission[4];        /* DP 2 */
       CHAR    chCreditRisk;
       BYTE    abLoad[3];          /* D */
       BYTE    abDiscount[3];      /* D */
       CHAR    achCashSourceCode1[8];
       BYTE    abAmntSourceCode1[4]; /* DP 2 */
       CHAR    chTypeSourceCode1;
       CHAR    achCashSourceCode2[8];
       BYTE    abAmntSourceCode2[4]; /* DP 2 */
       CHAR    chTypeSourceCode2;
       CHAR    chManualCheqInd;
       CHAR    achManualCheqReason[2];
       CHAR    chInformPremiumCredit;
       CHAR    chCertificateIssued;

	    /* additional fields for windmill.*/
	   CHAR	   chChildrenUnder16Flag;
       CHAR	   chNonMotoringConvictionFlag;
	   CHAR	   chHomeOwnerFlag;
	   BYTE    abCompositeRateCIPAmount[4];                 /* Packed decimal */
	   BYTE	   abExpiringAnnualisedCIP[4];					/* Packed decimal */
       CHAR    chAutoRenewalConsent;

	   /* Heisenberg */
	   double dblClaimsHandlingExpenses;
	   double dblVariableExpensesCurrency;
	   double dblVariableExpensesPercent;
	   double dblMarkupPercentage;
	   CHAR   chPriceOptimisationAvailable;

       CHAR    achEndorsements[6][3];

       /* Adjustment quote referral messages */
       CHAR    achTechNotes[120];
       CHAR    achReferralDate[DSOS_CB_STRING_DATE]; /* DP */
       CHAR    chIntroForce;
       CHAR    chProtDiscForce;
       BYTE    bHighestRatedDriverForce; /* D */
       CHAR    achVolDriverRestForce[2];
       CHAR    chAgreedValueForce;
       CHAR    achAreaCodeOveride[2];
       CHAR    achVehGroupOveride[2];
       BYTE    abDriveAgeOveride[3]; /* DP */
       BYTE    abNCDOveride[2];
       BYTE    abVehAgeOveride[2];
       BYTE    abVehValueOveride[4];  /* DP */
       CHAR    chMaritalStatusOveride;
       CHAR    achAdditionalTermsCode[8][2];
       CHAR    achDeletedTermsCode[8][2];

       APPC_REF_INFO_U232      ariRefInfo[10];

       CHAR    achAddDocs[12];
       CHAR    chReferalInhibitFlag;
       CHAR    achReferTechRef[4];

       CHAR    achFiller1[8];

       APPC_ADJ_PREM_BREAKDOWN apbPremiumBreakdown[45];

       /*  Green Card Details */
       BYTE    abGCDaysCount[2];    /* DP */
       CHAR    achGCEffectiveDate[DSOS_CB_STRING_DATE];/* DP */
       CHAR    achGCExpiryDate[DSOS_CB_STRING_DATE];   /* DP */
       BYTE    abGCPremium[4];      /* DP 2 */
       BYTE    abGCAdminFee[4];     /* DP 2 */
       BYTE    abVehicleNum[2];    /* DP */
       BYTE    abDriverNum[2];     /* DP */

       CHAR    chCategory1;            /* Additional GC fields on 26-Jan-95 */
       CHAR    chCategory2;
       CHAR    chBailBond;
       CHAR    chEireOnly;
       CHAR    achCountryCodes[50][3];

        /* Net Rate Fields */
	    /* PB - net rate fields renmoved as not used at all by the Iseries and */
	    /* this would be trhe wrong place for these anyway.                    */
        /*BYTE    abSchemeCommissionRatePercent[4];            Packed decimal */
        /*BYTE    abCommissionDiscountPercent[4];              Packed decimal */
        /*BYTE    abCompositeCommissionRatePercent[4];         Packed decimal */
        /*BYTE    abRiskOnlyCalcInsPremAmount[4];              Packed decimal */
        /*BYTE    abCompositeRateCIPAmount[4];                 Packed decimal */
        /*BYTE    abCommissionAmount[4];                       Packed decimal */
        /*CHAR    chSchemeRatingType;                                           */    
        
        OSM_COVER_OPTION_SELECTION_DETAILS acovosd[ DSOS_MAX_PREMCALC_COVER_OPTION_COUNT ] ;
   }
   APPC_U232_A;

   typedef APPC_U232_A *APPC_PU232_A;

   typedef DSOS_PACKED struct _APPC_U232_B
   {
       CHAR    achTransactionID[DSOS_CB_TRANS_BUFFER_ID];
       CHAR    achNextBufferID[DSOS_CB_TRANS_BUFFER_ID];

       APPC_VEH_INFO_U232 aeVehInfo[3];

   }
   APPC_U232_B;

   typedef APPC_U232_B *APPC_PU232_B;

   typedef DSOS_PACKED struct _APPC_U232_C
   {
       CHAR    achTransactionID[DSOS_CB_TRANS_BUFFER_ID];
       CHAR    achNextBufferID[DSOS_CB_TRANS_BUFFER_ID];

       APPC_DRIVER_DATA_U232 ddDriverData[9];

   }
   APPC_U232_C;

   typedef APPC_U232_C *APPC_PU232_C;

   typedef DSOS_PACKED struct _APPC_U232_D
   {
       CHAR    achTransactionID[DSOS_CB_TRANS_BUFFER_ID];
       CHAR    achNextBufferID[DSOS_CB_TRANS_BUFFER_ID];

       APPC_ADJ_QANDA aqaQandA[48];
       CHAR    achUnderWritingAnswers[256];

   }
   APPC_U232_D;

   typedef APPC_U232_D *APPC_PU232_D;
