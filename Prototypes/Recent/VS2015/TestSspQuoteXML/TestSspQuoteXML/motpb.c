/*************************************************************************/
/*                                                                       */
/*            COPYRIGHT ENDSLEIGH INSURANCE SERVICES LTD 2017            */
/*                                                                       */
/*                          NON-DELIVERABLE                              */
/*                                                                       */
/*************************************************************************/
/*                                                                       */
/*  PROJECT     : TECH                                                   */
/*                                                                       */
/*  LANGUAGE    : C                                                      */
/*                                                                       */
/*  FILE NAME   : MOTPB.C                                                */
/*                                                                       */
/*  ENVIRONMENT : Microsoft Visual Studio                                */
/*                IBM OS/400 ILE-C                                       */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/* AS/400 Pre-Compiler Directives                                        */
/* ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~                                        */
/*:T TECH - TECHMOT Motor PACE                                           */
/*:O CRTCMOD                                                             */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  FILE FUNCTION   : This source file contains the functions to         */
/*                    manipulate the premium breakdown                   */
/*                                                                       */
/*                                                                       */
/*  EXECUTABLE TYPE : DLL                                                */
/*                                                                       */
/*  SPECIFICATION   :                                                    */
/*                                                                       */
/*                                                                       */
/*  RELATED DOCUMENTATION :                                              */
/*                                                                       */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  AUTHOR   : N. Rutter            CREATION DATE:  4-Sep-1997           */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  BUILD INFORMATION : EISMAKE.CMD 32-Bit                               */
/*                                                                       */
/*  EXECUTABLE NAME   : TECHMOT.DLL                                      */
/*                                                                       */
/*  ENTRY POINTS      :                                                  */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  MANUAL MODIFICATION RECORD :                                         */
/*                                                                       */
/*  DATE        ISSUE.VERSION   DEVELOPER       DESCRIPTION              */
/*                                                                       */
/*   4-Sep-1997 0.01            NR              File created.            */
/*                                                                       */
/*************************************************************************/
/*                                                                       */
/* PVCS SECTION :                                                        */
/* ~~~~~~~~~~~~~~
   PVCS FILENAME: $Logfile:   Z:\TECH\Code\MOT\motpb.c  $
   PVCS REVISION: $Revision:   1.30.1.0  $

   $Log:   Z:\TECH\Code\MOT\motpb.c  $
 * 
 *    Rev 1.30.1.0   Nov 08 2017 11:41:20   corc1
 * 
 *  CR3753 – FCART RFC07
 * 
 *  Modified the return value of
 *  MotPremiumBreakdownAddCoverOption so that it correctly
 *  represents the action and added more debug output
 * 
 *    Rev 1.30   Mar 19 2015 14:20:40   corc1
 * 
 *  RQ013814
 * 
 *  Modified MotAddPremiumBreakdownEntryDebug
 *  to use a platform dependent Pound Sign definition
 * 
 *    Rev 1.29   Dec 10 2013 17:01:34   corc1
 * 
 *  CR3253
 * 
 *  Modified the implementation of MotPremiumBreakdownAddCoverOption
 *  to receive, for the speicified Cover Option, the current
 *  value of the Cover Selection Control that is relevant to
 *  the Premium Breakdown
 * 
 *    Rev 1.28   May 01 2013 11:27:02   corc1
 * 
 *  IMS10800
 * 
 *  Modified conditional compilation logic in
 *  MotAddPremiumBreakdownEntryDebug to cope more simply
 *  with development environments other than VS2010
 * 
 *    Rev 1.27   Apr 30 2013 17:02:44   corc1
 * 
 *  IMS10800
 * 
 *  Modified MotAddPremiumBreakdownEntryDebug to
 *  conditionally compile different code for calls to "isdigit"
 *  in Visual Studio 6 and on the iSeries compared with
 *  Visual Studios later than 6
 * 
 *    Rev 1.26   Apr 30 2013 16:25:48   corc1
 * 
 *  IMS10800
 * 
 *  Modified MotAddPremiumBreakdownEntryDebug to
 *  conditionally compile different code for calls to "isdigit"
 *  in Visual Studio 6 and Visual Studios later than 6
 * 
 *    Rev 1.25   Dec 10 2012 11:48:22   corc1
 * 
 *  IMS10800
 * 
 *  Merged with r1.23.1.4
 * 
 *  Corrected use of isdigit to use an unsigned char
 * 
 *    Rev 1.23.1.4   Sep 18 2012 11:33:54   andm1
 *  - IMS10828 - Jupiter - Add some debug output to 
 *    MotApplyCapOrCollar.
 * 
 *    Rev 1.23.1.3   Sep 14 2012 12:14:52   andm1
 *  - IMS10828 - Jupiter - housekeeping MotApplyCapOrCollar.
 * 
 *    Rev 1.23.1.2   Aug 22 2012 09:16:58   andm1
 *  - Project Jupiter - Dynamic Premium Cap/Collar - Modifications.
 * 
 *    Rev 1.23.1.1   Aug 21 2012 16:02:52   andm1
 *  - Project Jupiter - Dynamic Premium Cap/Collar - Amendments.
 * 
 *    Rev 1.23.1.0   Aug 21 2012 11:47:40   andm1
 *  - Project Jupiter - Dynamic Premium Cap/Collar.
 *    Implement MotApplyCapOrCollar and modify 
 *    MotApplyLoad to make use of the same.
 * 
 *    Rev 1.23   Nov 16 2011 12:28:22   corc1
 * 
 *  IMS10789 : Project Windmill
 * 
 *  Merged with r1.22.1.0
 * 
 *    Rev 1.22.1.0   Oct 05 2011 09:41:12   corc1
 * 
 *  IMS10789
 * 
 *  - Added implementation of MotAddUniqueInformationalMessageNumber
 *  - Modified MotAddDeclineMessageWithContext and MotAddReferMessageWithContext
 *    to use DSOS_MAX_PREMCALC_UNDERWRITING_MESSAGE_COUNT
 * 
 *    Rev 1.22   May 16 2011 16:28:00   corc1
 * 
 *  IMS10779
 * 
 *  Modified PceAddEntirePremiumBreakdownEntry and MotAddRuleDetailsToPB
 *  to set the Rule Invocation Number
 * 
 *    Rev 1.21   Mar 18 2011 11:39:30   corc1
 * 
 *  IMS10761 : Project Pitstop
 * 
 *  Merged with r1.20.1.0
 * 
 *    Rev 1.20.1.0   Mar 14 2011 15:44:28   corc1
 * 
 *  IMS10761
 * 
 *  Added implementation of MotPremiumBreakdownAddCoverOption
 * 
 *    Rev 1.20   Dec 15 2009 20:27:44   corc1
 * 
 *  IMS10637
 * 
 *  - Added implementation of PceSetRuleDescriptionOfPremiumBreakdownEntry
 *  - Modified PceAddEntirePremiumBreakdownEntry and
 *    MotAddRuleDetailsToPB to use
 *    PceSetRuleDescriptionOfPremiumBreakdownEntry
 * 
 *    Rev 1.19   Dec 15 2009 17:10:32   corc1
 * 
 *  IMS10637
 * 
 *  - Modified MotApplyLoad to check for a PremCalc Workspace
 *    not having been supplied
 *  - Modified PceAddEntirePremiumBreakdownEntry and MotAddRuleDetailsToPB
 *    to cope with Rule Numbers that are out of range of those supplied by TMS
 * 
 *    Rev 1.18   Sep 03 2009 15:17:58   majj1
 * 
 *  IMS9625
 * 
 *  Add support for constant  
 *  PCE_CH_PREM_BREAKDOWN_ROW_TYPE_
 *  FACTOR_ALPHA in MotApplyLoad.
 * 
 *    Rev 1.17   Jun 08 2009 16:11:54   corc1
 * 
 *  IMS9625
 * 
 *  - Modified MotApplyLoad to accept the Rule Number as a parameter
 *  - Modified MotPoundsToPB, MotPercentToPB and MotFactorToPB to accept
 *    the Rule Number as a parameter rather than assume the currently
 *    executing Rule
 *  - Where possible replaced use of the Premium Breakdown value
 *    resultant_prem with PCE_FINAL_PREMIUM
 *  - Modified MotAddRuleDetailsToPB to not add Scheme Debug and move
 *    the Scheme Debug processing to a separate function
 *    MotAddPremiumBreakdownEntryDebug
 *  - Tidied up PceAddPremiumBreakdownEntry to use a pointer to the
 *    Premium Breakdown Entry rather than use multiple index calculations.
 *    Also modified it to call MotAddPremiumBreakdownEntryDebug
 *  - Modified PceAddEntirePremiumBreakdownEntry to call
 *    MotAddPremiumBreakdownEntryDebug
 * 
 *    Rev 1.16   Jun 05 2009 16:45:46   corc1
 * 
 *  IMS9625
 * 
 *  Added implementation for PceAddEntirePremiumBreakdownEntry
 * 
 *    Rev 1.15   May 20 2009 15:04:22   corc1
 * 
 *  IMS10547
 * 
 *  Replaced MotAddDeclineMessage and MotAddReferMessage
 *  with MotAddDeclineMessageWithContext and MotAddReferMessageWithContext
 *  and removed hardcoded array sizes
 * 
 *    Rev 1.14   Mar 23 2009 13:54:30   majj1
 * 
 *  IMS9625
 * 
 *  Moved 1.13.1.1 over to the tip.
 *  Changed MotApplyLoad to check that Factor loads are greater 
 *  than 0 and not equal to 1.
 * 
 *    Rev 1.13.1.1   Dec 12 2007 15:51:58   corc1
 * 
 *  IMS9625
 * 
 *  Updated MotApplyLoad to cope properly with negative points
 * 
 *    Rev 1.13.1.0   Jun 06 2006 09:36:18   corc1
 * 
 *  Modified a number of Premium Breakdown functions to cope
 *  with being supplied a Pound Sign that originally comes from
 *  Code Page 850. On an iSeries this Pound Sign will not have
 *  the character value for the Code Page 850 Pound Sign (0x9c)
 *  but will have been converted to some other character (0x04 it
 *  seems)
 * 
 *    Rev 1.13   May 31 2006 14:24:24   corc1
 * 
 *  Modified MotAddRuleDetailsToPB to display as a digit a
 *  Premium Breakdown Entry Type that is numeric
 * 
 *    Rev 1.12   May 26 2006 14:13:46   corc1
 * 
 *  Modified MotAddRuleDetailsToPB to output more debugging
 *  details about the entry being added to Premium Breakdown
 * 
 *    Rev 1.11   Jul 21 2005 12:38:10   faum1
 * With PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POINTS
 * we are now rounding the float in case the cast looses too 
 * much in the conversion.
 * 
 *    Rev 1.10   Dec 08 2004 17:34:42   corc1
 * 
 *  Updated MotAddRuleDetailsToPB to generate debug 
 *  information that indicates the new value of the premium
 * 
 *    Rev 1.9   Dec 07 2004 11:51:48   corc1
 * 
 *  - Changed use of PCE_CH_CALC_STATUS_DECLINE and
 *    PCE_CH_CALC_STATUS_REFER to MOT_CH_CALC_STATUS_DECLINE
 *    and MOT_CH_CALC_STATUS_REFER respectively
 *  - Updated PceAddPremiumBreakdownEntry to output the
 *    Rule Number in its error processing
 * 
 *    Rev 1.8   Dec 06 2004 16:52:02   corc1
 * 
 *  Complaince (Net Rates) Project changes
 * 
 *  Made many changes to make use of the new Compliance
 *  (Net Rate) fields in the PremCalc output data
 *  structures. Probably the most important changes have
 *  now been added through Effect 327 and Cause 463.
 *  OnEndScheme now checks that the Composite Endsleigh
 *  Commission Rate for the Scheme has been set
 * 
 *    Rev 1.7   Jul 10 2001 15:25:08   CorC1
 * 
 *  - Added documentation to MotPercentToPB and reformatted
 *    internals of the function to ensure that compiler warnings
 *    are not generated
 *  - Modified MotApplyLoad to not generate a warning
 *    on the manipulation of ppwfWorkSpace->deferred_points
 *
 *    Rev 1.6   Apr 17 2000 12:39:06   RutN1
 * Fixed a potential access violation in 'PceConvertLoadPercentage'
 *
 *    Rev 1.5   Aug 20 1999 11:16:28   RutN1
 * Changed hardwired occurences of 9999.99 to use new constant
 * MOT_DECLINE_PREMIUM_VALUE
 *
 *    Rev 1.4   09 Dec 1997 12:41:42   RutN1
 * Port to AS/400
 *
 *    Rev 1.3   18 Nov 1997 09:37:28   RutN1
 * Fixes to bring into line with DSOSPCE
 *
 *    Rev 1.2   22 Oct 1997 16:57:08   RutN1
 * Changed many of the API's to have the PF_HANDLE as the first paramater
 *
 *    Rev 1.1   16 Oct 1997 10:41:50   RutN1
 *  - Added functions:
 *        PceAddPremiumBreakdownEntry
 *        PceCalculatePremiumAfterLoad
 *        PceConvertLoadPercentage
 *
 *    Rev 1.0   01 Oct 1997 13:42:18   CorC1
 *
 *  Initial revision before full calculation implemented.

*/
/*************************************************************************/

/* .page .contents */

/*
+-------------------------------------------------------------------------+
| EXTERNAL DATA and FUNCTION PROTOTYPES and GLOBAL VARIABLES:             |
+-------------------------------------------------------------------------+
|   This section details/includes the Header Files for all those          |
|   Modules whose Global Data or Functions are referenced by this source  |
|   (including the required system includes)                              |
+-------------------------------------------------------------------------+
*/

#include "mot.h"

/*
+-------------------------------------------------------------------------+
| PREPROCESSOR DEFINITIONS:                                               |
+-------------------------------------------------------------------------+
|   This section details #define declarations for the private             |
|   modules whose Global Data or Functions are referenced by this source  |
+-------------------------------------------------------------------------+
*/


/*
+-------------------------------------------------------------------------+
| TYPE DEFINITIONS:                                                       |
+-------------------------------------------------------------------------+
|   This section details typedef declarations for all the private         |
|   types for this source                                                 |
+-------------------------------------------------------------------------+
*/


/*
+-------------------------------------------------------------------------+
| FUNCTION PROTOTYPES:                                                    |
+-------------------------------------------------------------------------+
|   This section details prototype declarations for all the LOCAL         |
|   functions of of this source.                                          |
+-------------------------------------------------------------------------+
*/


/*
+-------------------------------------------------------------------------+
| GLOBAL DATA DECLARATIONS                                                |
+-------------------------------------------------------------------------+
|   This section details the global data used by all the source files     |
|   that comprise the <mod> module.                                       |
|                                                                         |
+-------------------------------------------------------------------------+
*/

/*
+-------------------------------------------------------------------------+
| KNOWN Wall WARNINGS:                                                    |
+-------------------------------------------------------------------------+
|   This section details known warnings that will be generated by the     |
|   IBM C/C++ Tools Version 2.01 compiler with the /Wall switch.          |
|                                                                         |
+-------------------------------------------------------------------------+
*/

/* none */

/* ======================== End of Module Header ======================= */

#if 0
/*
.page .section MotRoundFloatPremiumWithSignToNearestPenny
+-------------+ MotRoundFloatPremiumWithSignToNearestPenny +--------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  MotRoundFloatPremiumWithSignToNearestPenny                             |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|   Round the supplied value to the nearest penny according to the sign   |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   flValue [in]                                                          |
|       The value to be rounded to the nearest penny                      |
|                                                                         |
| RETURN VALUE:                                                           |
|   The supplied value to the nearest penny according to the sign         |
|                                                                         |
| LAST MODIFIED DATE: 14-Mar-2011                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
static float MotRoundFloatPremiumWithSignToNearestPenny( float flValue )
{
    double dblRoundedValue = 0.0 ;
    
    if ( flValue >= 0.0 )
        /* Zero or positive */
        dblRoundedValue = floor( ( flValue * EIS_DBL_POUNDS_PENCE_CONVERSION_FACTOR + 0.5 ) )
                                    / EIS_DBL_POUNDS_PENCE_CONVERSION_FACTOR ;
    else
        /* Negative */
        dblRoundedValue = ceil( ( flValue * EIS_DBL_POUNDS_PENCE_CONVERSION_FACTOR - 0.5 ) )
                                    / EIS_DBL_POUNDS_PENCE_CONVERSION_FACTOR ;

    return (float)dblRoundedValue ;

} /* MotRoundFloatPremiumWithSignToNearestPenny */
#endif

/*
.page .section MotPoundsToPB
+---------------------------+ MotPoundsToPB +-----------------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  MotPoundsToPB                                                          |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|   This function adds a monetary amount entry to the premium breakdown.  |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   hpf [in]                                                              |
|       Premium Calculation framework handle                              |
|   ulRuleNumber [in]                                                     |
|       The number of the Rule for which the change in premium is         |
|       relevant                                                          |
|   flPremiumChange [in]                                                  |
|       The premium increment that is to be added to the current premium  |
|   psCount [in/out]                                                      |
|       A pointer to a variable containing the number of members of the   |
|       Premium Breakdown Array. The value to which this points will be   |
|       modified by this function                                         |
|   pirInsurerResponse [in/out]                                           |
|       A pointer to the Scheme Result that is relevant to the Premium    |
|       Breakdown Array                                                   |
|   pir1IndividualResponse [in/out]                                       |
|       A pointer to the Individual Scheme Response which includes the    |
|       Premium Breakdown Array                                           |
|                                                                         |
| RETURN VALUE:                                                           |
|   EIS_OK                                                                |
|                                                                         |
| LAST MODIFIED DATE: 08-Jun-2009                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
static USHORT MotPoundsToPB( PF_HANDLE hpf ,
                             ULONG ulRuleNumber ,
                             float flPremiumChange ,
                             SHORT *psCount ,
                             INS_RESPONSE *pirInsurerResponse ,
                             INDIVIDUAL_RESPONSE *pir1IndividualResponse )
{
    /* Only perform the change if it is non-zero */
    if ( ! EIS_DBL_IS_EFFECTIVELY_ZERO( flPremiumChange , EIS_DBL_ABS_EFFECTIVE_ZERO ) )
    {
        float flOldPremium = (float)PCE_FINAL_PREMIUM ;

        PCE_FINAL_PREMIUM += flPremiumChange ;

        /* Only add the Premium Breakdown if the new premium is different */
        if ( ! EIS_DBL_IS_EQUAL( PCE_FINAL_PREMIUM , flOldPremium , EIS_DBL_ABS_COMPARE_PRECISION ) )
        {
            PceAddEntirePremiumBreakdownEntry( hpf ,
                                               ulRuleNumber ,
                                               (DSOS_DOUBLE)flPremiumChange ,
                                               PCE_FINAL_PREMIUM ,
                                               PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POUNDS ,
                                               pir1IndividualResponse ,
                                               psCount ) ;
        }
    }

    return EIS_OK ;

} /* MotPoundsToPB */

/*
.page .section MotPercentToPB
+---------------------------+ MotPercentToPB +----------------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|   MotPercentToPB                                                        |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|                                                                         |
|   This function applies a "Percentage Load" to the premium and if the   |
|   "new" premium is different from the "old", the "new" premium becomes  |
|   the premium and a corresponding Premium Breakdown Entry is generated. |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|                                                                         |
|   hpf [in]                                                              |
|       Premium Calculation framework handle                              |
|   ulRuleNumber [in]                                                     |
|       The number of the Rule for which the change in premium is         |
|       relevant                                                          |
|   flLoadPercent [in]                                                    |
|       The Load Percentage to be applied to the premium that has been    |
|       generated so far RELATIVE TO 100. Note that the value must be the |
|       percentage by which the premium should be increased. Hence a      |
|       value that is:                                                    |
|           >0   : will increase the premium by the specified percentage  |
|           zero : will produce no change to the premium                  |
|           <0   : will decrease the premium by the specified percentage  |
|       Note that a number of the tables used by effects that use this    |
|       function supply data where a Load of 100 means "make no change"   |
|       (i.e. apply zero load)                                            |
|   psCount [in/out]                                                      |
|       A pointer to an value containing the number of entries in the     |
|       Premium Breakdown Array. If this function does anything, the      |
|       value to which this points will increase by 1                     |
|   pirInsurerResponse [in/out]                                           |
|       A pointer to the generalised Insurer Response for the calling     |
|       Premium Calculation                                               |
|   pir1IndividualResponse [in/out]                                       |
|       A pointer to the specific Insurer Response for the calling        |
|       Premium Calculation                                               |
|                                                                         |
| RETURN VALUE:                                                           |
|                                                                         |
|   Zero                                                                  |
|                                                                         |
| LAST MODIFIED DATE: 08-Jun-2009                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
static USHORT MotPercentToPB( PF_HANDLE hpf ,
                              ULONG ulRuleNumber ,
                              float flLoadPercent ,
                              SHORT *psCount ,
                              INS_RESPONSE *pirInsurerResponse ,
                              INDIVIDUAL_RESPONSE *pir1IndividualResponse )
{

    /* Only perform the change if it is non-zero */
    if ( ! EIS_DBL_IS_EFFECTIVELY_ZERO( flLoadPercent , EIS_DBL_ABS_EFFECTIVE_ZERO ) )
    {
        /* A non-zero load value has been supplied */
        float flOldPremium = PCE_FINAL_PREMIUM ;
        float flNewPremium = 0.0 ;

        flNewPremium = (float)( ( flOldPremium * ( EIS_DBL_PERCENT_DIVISOR + flLoadPercent ) ) / 100.0 ) ;

        if ( ! EIS_DBL_IS_EQUAL( flNewPremium , flOldPremium , EIS_DBL_ABS_COMPARE_PRECISION ) )
        {
            /* The premium has changed */

            PCE_FINAL_PREMIUM = flNewPremium ;

            PceAddEntirePremiumBreakdownEntry( hpf ,
                                               ulRuleNumber ,
                                               (DSOS_DOUBLE) flLoadPercent ,
                                               flNewPremium  ,
                                               PCE_CH_PREM_BREAKDOWN_ROW_TYPE_PERCENT ,
                                               pir1IndividualResponse ,
                                               psCount ) ;

        } /* The premium has changed */

    } /* A non-zero load value has been supplied */

    return 0 ;

} /* MotPercentToPB */

/*
.page .section MotPointsToPB
+---------------------------+ MotPointsToPB +-----------------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|   MotPointsToPB                                                         |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|   The number of points added is equal to the difference between         |
|   the original points total specified and the new total.                |
|                                                                         |
|   It does NOT alter the deferred points total                           |
|       ( ppwfWorkSpace->deferred_points )                                |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   pts     - The new pts total                                           |
|   pts_b4  - The original pts total                                      |
|                                                                         |
| RETURN VALUE:                                                           |
|   0                                                                     |
|                                                                         |
| LAST MODIFIED DATE: 06-Dec-2004                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/

USHORT MotPointsToPB( PF_HANDLE           hpf ,
                      SHORT               pts ,
                      SHORT               pts_b4 ,
                      SHORT               *psCount ,
                      INDIVIDUAL_RESPONSE *pir1IndividualResponse ,
                      CHAR                chType )
{
    if (pts != pts_b4)      /* add details to pb structure */
    {
        PceAddPremiumBreakdownEntry ( hpf ,
                                      (DSOS_DOUBLE) ( pts-pts_b4 ) ,
                                      pir1IndividualResponse->pb[*psCount].resultant_prem ,
                                      chType ,
                                      pir1IndividualResponse ,
                                      psCount ) ;
    }

    return 0 ;

} /* MotPointsToPB */

/*
.page .section MotFactorToPB
+---------------------------+ MotFactorToPB +-----------------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|   MotFactorToPB                                                         |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|   This function adds a factored amount entry to the Premium Breakdown   |
|   Array                                                                 |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|                                                                         |
|   hpf [in]                                                              |
|       Premium Calculation framework handle                              |
|   ulRuleNumber [in]                                                     |
|       The number of the Rule for which the change in premium is         |
|       relevant                                                          |
|   flFactor [in]                                                         |
|       The Load Factor to be applied to the premium.                     |
|       The value will operate as follows:                                |
|           >0   : will increase the premium by the specified factor      |
|           zero : will produce no change to the premium                  |
|           <0   : will decrease the premium by the specified factor      |
|   psCount [in/out]                                                      |
|       A pointer to an value containing the number of entries in the     |
|       Premium Breakdown Array. If this function does anything, the      |
|       value to which this points will increase by 1                     |
|   pirInsurerResponse [in/out]                                           |
|       A pointer to the generalised Insurer Response for the calling     |
|       Premium Calculation                                               |
|   pir1IndividualResponse [in/out]                                       |
|       A pointer to the specific Insurer Response for the calling        |
|       Premium Calculation                                               |
|                                                                         |
| RETURN VALUE:                                                           |
|   None                                                                  |
|                                                                         |
| LAST MODIFIED DATE: 08-Jun-2009                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
static VOID MotFactorToPB( PF_HANDLE hpf ,
                           ULONG ulRuleNumber ,
                           float flFactor ,
                           SHORT *psCount ,
                           INS_RESPONSE *pirInsurerResponse ,
                           INDIVIDUAL_RESPONSE *pir1IndividualResponse )
{
    float flPremBefore = PCE_FINAL_PREMIUM ;

    PCE_FINAL_PREMIUM = flFactor * flPremBefore;

    PceAddEntirePremiumBreakdownEntry( hpf ,
                                       ulRuleNumber ,
                                       (DSOS_DOUBLE) flFactor ,
                                       PCE_FINAL_PREMIUM ,
                                       PCE_CH_PREM_BREAKDOWN_ROW_TYPE_FACTOR ,
                                       pir1IndividualResponse ,
                                       psCount ) ;

} /* MotFactorToPB */

/*
.page .section MotApplyCapOrCollar
+----------------------+ MotApplyCapOrCollar +----------------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  USHORT MotApplyCapOrCollar                                             |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|                                                                         |
|   Checks if Capping is in operation and actions accordingly             |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   hpf [in]                                                              |
|       Premium Calculation framework handle                              |
|   flLoad [in]                                                           |
|       The value of the load to be applied regardless of its type        |
|   chLoadType [in]                                                       |
|       The type of load to be applied. This must be one of the defined   |
|       character constants that begin PCE_CH_PREM                        |
|   ulRuleNumber [in]                                                     |
|       The number of the Rule for which the change in premium is         |
|       relevant                                                          |
|   pirInsurerResponse [in/out]                                           |
|       A pointer to the output data for the Scheme                       |
|   pir1IndividualResponse [in/out]                                       |
|       A pointer to the Individual Scheme Response which includes the    |
|       Premium Breakdown Array                                           |
|   psCount [in/out]                                                      |
|       A pointer to a variable containing the number of members of the   |
|       Premium Breakdown Array. The value to which this points will be   |
|       modified by this function                                         |
|   ppwfWorkSpace [in/out]                                                |
|       A pointer to the Workspace for the PremCalc corresponding to the  |
|       supplied PremCalc Framework Handle                                |
|                                                                         |
| RETURN VALUE:                                                           |
|   EIS_ERROR value without the severity                                  |
|                                                                         |
| LAST MODIFIED DATE: 18-Sep-2012                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
static USHORT MotApplyCapOrCollar( PF_HANDLE hpf , float flOldPremium, float flNewPremium , INS_RESPONSE *pirInsurerResponse ) 
{
    DRQ_PPACE_OUT ppaceOut = NULL ;

    PceGlobalDataGetUserData ( hpf , emudPACEOut , (PVOID)&ppaceOut ) ;
    
    /* Is CAP switched on? */
    if ( PceGlobalDataGetDynamicPremiumCapIsSet( hpf , NULL ) )
    {

        BOOL                                    bDynamicPremiumIsSet = FALSE ;
        PCE_EDYNAMIC_MAXIMUM_PREMIUM_ACTION     edmaxpaDynamicPremiumAction = edmaxpaUnknown ;
        double                                  dblDynamicPremiumThresholdAmount = 0.0 ;
        LONG                                    lDynamicPremiumMessageNumber = 0;

        PceGlobalDataGetDynamicPremiumCap( hpf , 
                                           &bDynamicPremiumIsSet ,
                                           &edmaxpaDynamicPremiumAction ,
                                           &dblDynamicPremiumThresholdAmount ,
                                           &lDynamicPremiumMessageNumber );

        /* Will the new premium exceed the cap threshold? */
        if ( flNewPremium > dblDynamicPremiumThresholdAmount )
        {

            switch ( edmaxpaDynamicPremiumAction ) 
            {
                case edmaxpaCap :
                    PceDebugSchemePrintf( hpf , NULL , "PCE_FINAL_PREMIUM capped at %f" , PCE_FINAL_PREMIUM ) ;
                    break;
                case edmaxpaDecline :
                    MotAddDeclineMessageWithContext( hpf ,
                                                     (SHORT)lDynamicPremiumMessageNumber ,
                                                     pirInsurerResponse ,
                                                     &ppaceOut->amnt.prem_msg ) ;
                    PceDebugSchemePrintf( hpf , 
                                          NULL , 
                                          "%f > %ld (CAP threshold)." ,
                                          flNewPremium, 
                                          dblDynamicPremiumThresholdAmount ) ;
                    break;
                default :
                    PceDebugSchemePrintf( hpf , 
                                          NULL ,
                                          "MotApplyLoad: Unrecognised edmaxpaDynamicPremiumAction '%ld'" , 
                                          edmaxpaDynamicPremiumAction ) ;
                    break;

            }
        } /* Will the new premium exceed the cap threshold? */
        else
        { /* New premium does not exceed the cap threshold */
            PCE_FINAL_PREMIUM = flNewPremium ;
        }
    } /* Is CAP switched on? */
    else
    { /* No Cap in operation */
        PCE_FINAL_PREMIUM = flNewPremium ;
    }
    

    /* Is COLLAR switched on? */
    if ( PceGlobalDataGetDynamicPremiumCollarIsSet( hpf , NULL ) )
    {
        BOOL                                    bDynamicPremiumIsSet = FALSE ;
        PCE_EDYNAMIC_MINIMUM_PREMIUM_ACTION     edminpaDynamicPremiumAction = edminpaUnknown ;
        double                                  dblDynamicPremiumThresholdAmount = 0.0 ;
        LONG                                    lDynamicPremiumMessageNumber = 0 ;

        PceGlobalDataGetDynamicPremiumCollar( hpf ,
                                              &bDynamicPremiumIsSet ,
                                              &edminpaDynamicPremiumAction ,
                                              &dblDynamicPremiumThresholdAmount ,
                                              &lDynamicPremiumMessageNumber );

        /* Will the new premium fall below the collar threshold? */
        if ( flNewPremium < dblDynamicPremiumThresholdAmount)
        {
            switch ( edminpaDynamicPremiumAction  ) 
            {
                case edminpaCollar :
                    PceDebugSchemePrintf( hpf , NULL , "PCE_FINAL_PREMIUM collared at %f" , PCE_FINAL_PREMIUM ) ;
                    break;
                case edminpaDecline :
                    MotAddDeclineMessageWithContext( hpf ,
                                                     (SHORT)lDynamicPremiumMessageNumber ,
                                                     pirInsurerResponse ,
                                                     &ppaceOut->amnt.prem_msg ) ;
                    PceDebugSchemePrintf( hpf ,
                                          NULL , 
                                          "%f > %ld (COLLAR threshold)." ,
                                          flNewPremium,
                                          dblDynamicPremiumThresholdAmount ) ;
                    break;
                default :
                    PceDebugSchemePrintf( hpf,
                                          NULL,
                                          "MotApplyLoad: Unrecognised edminpaDynamicPremiumAction '%ld'" ,
                                          edminpaDynamicPremiumAction ) ;
                    break;

            }
        } /* Will the new premium fall below the collar threshold? */
        else
        { /* New premium does not fall below collar threshold */
            PCE_FINAL_PREMIUM = flNewPremium ;
        }
    } /* Is COLLAR switched on? */
    else
    {
        PCE_FINAL_PREMIUM = flNewPremium ;
    }

    return EIS_OK ;

} /* MotApplyCapOrCollar */

/*
.page .section MotApplyLoad
+----------------------------+ MotApplyLoad +-----------------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  USHORT MotApplyLoad                                                    |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|                                                                         |
|   Applies the appropriate load, either pounds, percent, points or       |
|   factor to the premium breakdowns.                                     |
|                                                                         |
|                                   NOTE                                  |
|   This function should *not* normally be used to apply driver demerit   |
|   points mainly because, normally, the Premium Breakdown does not       |
|   contain points. Typically points management through the Premium       |
|   Breakdown is only done early in the PremCalc before the currency      |
|   Premium Breakdown is required.                                        |
|   This function only updates the deferred points total and does not     |
|   manage the demerit points for the individual drivers at all.          |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   hpf [in]                                                              |
|       Premium Calculation framework handle                              |
|   flLoad [in]                                                           |
|       The value of the load to be applied regardless of its type        |
|   chLoadType [in]                                                       |
|       The type of load to be applied. This must be one of the defined   |
|       character constants that begin PCE_CH_PREM                        |
|   ulRuleNumber [in]                                                     |
|       The number of the Rule for which the change in premium is         |
|       relevant                                                          |
|   pirInsurerResponse [in/out]                                           |
|       A pointer to the output data for the Scheme                       |
|   pir1IndividualResponse [in/out]                                       |
|       A pointer to the Individual Scheme Response which includes the    |
|       Premium Breakdown Array                                           |
|   psCount [in/out]                                                      |
|       A pointer to a variable containing the number of members of the   |
|       Premium Breakdown Array. The value to which this points will be   |
|       modified by this function                                         |
|   ppwfWorkSpace [in/out]                                                |
|       A pointer to the Workspace for the PremCalc corresponding to the  |
|       supplied PremCalc Framework Handle                                |
|                                                                         |
| RETURN VALUE:                                                           |
|   EIS_ERROR value without the severity                                  |
|                                                                         |
| LAST MODIFIED DATE: 15-Dec-2009                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/

USHORT MotApplyLoad( PF_HANDLE hpf ,
                     float flLoad ,
                     CHAR chLoadType ,
                     ULONG ulRuleNumber ,
                     INS_RESPONSE *pirInsurerResponse ,
                     INDIVIDUAL_RESPONSE *pir1IndividualResponse ,
                     SHORT *psCount ,
                     PWF *ppwfWorkSpace )
{
    USHORT usError = 0 ;

    /*
    **  Only apply loads to non-declined schemes.
    */
    if ( PCE_CALC_STATUS != MOT_CH_CALC_STATUS_DECLINE )
    {
        /* Scheme has not Declined yet */

        switch( chLoadType )
        {
            case CHAR_POUND_TMS : /* CC : For now allow incorrect Code Page Pound sign too */
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POUNDS :

                /* Only perform the change if it is non-zero */
                if ( ! EIS_DBL_IS_EFFECTIVELY_ZERO( flLoad , EIS_DBL_ABS_EFFECTIVE_ZERO ) )
                {

                    float flOldPremium = (float)PCE_FINAL_PREMIUM ;
                    float flNewPremium = 0.0 ;

                    flNewPremium = flOldPremium + flLoad ;

                    usError = MotApplyCapOrCollar( hpf ,
                                                   flOldPremium , 
                                                   flNewPremium , 
                                                   pirInsurerResponse ) ;
                    

                    if ( ! EIS_DBL_IS_EQUAL( flNewPremium , flOldPremium , EIS_DBL_ABS_COMPARE_PRECISION ) )
                    {
                        /* Only add the Premium Breakdown if the new premium is different */
                        PceAddEntirePremiumBreakdownEntry( hpf ,
                                                           ulRuleNumber ,
                                                           (DSOS_DOUBLE)flLoad ,
                                                           PCE_FINAL_PREMIUM ,
                                                           PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POUNDS ,
                                                           pir1IndividualResponse ,
                                                           psCount ) ;
                    } /* Only add the Premium Breakdown if the new premium is different */
                }
                
                break;

            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_PERCENT :

                /* Only perform the change if it is non-zero */
                if ( ! EIS_DBL_IS_EFFECTIVELY_ZERO( flLoad , EIS_DBL_ABS_EFFECTIVE_ZERO ) )
                {
                    /* A non-zero load value has been supplied */

                    float flOldPremium = PCE_FINAL_PREMIUM ;
                    float flNewPremium = 0.0 ;

                    flNewPremium = (float)( ( flOldPremium * ( EIS_DBL_PERCENT_DIVISOR + flLoad ) ) / 100.0 ) ;
                    
                    usError = MotApplyCapOrCollar( hpf ,
                                                   flOldPremium , 
                                                   flNewPremium , 
                                                   pirInsurerResponse ) ;

                    if ( ! EIS_DBL_IS_EQUAL( flNewPremium , flOldPremium , EIS_DBL_ABS_COMPARE_PRECISION ) )
                    {
                        /* Only add the Premium Breakdown if the new premium is different */

                        PCE_FINAL_PREMIUM = flNewPremium ;

                        PceAddEntirePremiumBreakdownEntry( hpf ,
                                                           ulRuleNumber ,
                                                           (DSOS_DOUBLE) flLoad ,
                                                           PCE_FINAL_PREMIUM  ,
                                                           chLoadType ,
                                                           pir1IndividualResponse ,
                                                           psCount ) ;

                    } /* Only add the Premium Breakdown if the new premium is different */

                } /* A non-zero load value has been supplied */

                break;
            
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_FACTOR_ALPHA :
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_FACTOR :
                
                if (    ( EIS_DBL_IS_GREATER_THAN( flLoad ,
                                                   0.0 , EIS_DBL_ABS_COMPARE_PRECISION )
                        ) 
                     && ( ! EIS_DBL_IS_EQUAL( flLoad ,
                                              1.0 , EIS_DBL_ABS_COMPARE_PRECISION )
                        )
                   )
                {
                    /* A factor greater than zero but less than 1.0 has been supplied */

                    float flOldPremium = PCE_FINAL_PREMIUM ;
                    float flNewPremium = 0.0 ;

                    flNewPremium = flLoad * flOldPremium;
                    
                    usError = MotApplyCapOrCollar( hpf ,
                                                   flOldPremium , 
                                                   flNewPremium , 
                                                   pirInsurerResponse ) ;


                    PceAddEntirePremiumBreakdownEntry( hpf ,
                                                       ulRuleNumber ,
                                                       (DSOS_DOUBLE) flLoad ,
                                                       PCE_FINAL_PREMIUM ,
                                                       PCE_CH_PREM_BREAKDOWN_ROW_TYPE_FACTOR ,
                                                       pir1IndividualResponse ,
                                                       psCount ) ;
                } /* A factor greater than zero but less than 1.0 has been supplied */
                break;

            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POINTS :

                if ( ppwfWorkSpace != NULL )
                {
                    /* Increment deferred points total rounding according to the sign of the load */

                    if ( flLoad < 0.0 )
                        ppwfWorkSpace->deferred_points = ppwfWorkSpace->deferred_points + (SHORT)( flLoad - 0.5 ) ;
                    else
                        ppwfWorkSpace->deferred_points = ppwfWorkSpace->deferred_points + (SHORT)( flLoad + 0.5 ) ;

                } /* Increment deferred points total rounding according to the sign of the load */

                if ( ! EIS_DBL_IS_EQUAL( flLoad ,
                                            0.0 , EIS_DBL_ABS_COMPARE_PRECISION ) ) /* add details to pb structure */
                {
                    PceAddPremiumBreakdownEntry ( hpf ,
                                                  (DSOS_DOUBLE) ( flLoad ) ,
                                                  pir1IndividualResponse->pb[*psCount].resultant_prem ,
                                                  chLoadType ,
                                                  pir1IndividualResponse ,
                                                  psCount ) ;
                }

                break;

            

        default:
                KerDebugPrintf( "MotApplyLoad: Unrecognised load type '%c'" , chLoadType ) ;
                break;

        } /* switch( chLoadType ) */

    } /* Scheme has not Declined yet */

    return usError ;

} /* MotApplyLoad */

/*
.page .section MotAddPremiumBreakdownEntryDebug
+------------------+ MotAddPremiumBreakdownEntryDebug +-------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|                                                                         |
|   PceAddPremiumBreakdownEntry                                           |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|                                                                         |
|   Add an element to the Premium Breakdown Array assuming that the Rule  |
|   that is currently executing in the PremCalc Framework is the relevant |
|   Rule                                                                  |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   hpf [in]                                                              |
|       Premium Calculation Framework Handle                              |
|   ppbEntry [in]                                                         |
|       A pointer to the Premium Breakdown Entry for which Debug is to be |
|       generated                                                         |
|                                                                         |
| RETURN VALUE:                                                           |
|                                                                         |
|   VOID                                                                  |
|                                                                         |
| LAST MODIFIED DATE: 19-Mar-2015                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
static void MotAddPremiumBreakdownEntryDebug( PF_HANDLE hpf , const PB *ppbEntry )
{
    PCE_PLOG_DATA ppldLogData = NULL ;

    PceGlobalDataGetLogData ( hpf , &ppldLogData ) ;

    if ( ( ppldLogData != NULL ) && ( ppldLogData->pdbDebug.fEnabled ) )
    {
        /* For Scheme Debugging purposes output any change to the Premium */

        char chPremiumBreakdownEntryType = ppbEntry->load_disc_type ;

        /* Numeric change types are used when applying */
        /* Demerit Points to the Premium Breakdown */
        #if defined(_MSC_VER) && (_MSC_VER>=1300)
        /* Visual Studio versions greater than 6 */
        if ( ! isdigit( (unsigned char)chPremiumBreakdownEntryType ) )
        #else
        /* Visual Studio 6 and iSeries */
        if ( ! isdigit( chPremiumBreakdownEntryType ) )
        #endif
        {
            /* The type is not numeric */

            switch ( chPremiumBreakdownEntryType )
            {
            case CHAR_POUND_TMS :
                /* CC : For now allow incorrect Code Page Pound sign too */
                chPremiumBreakdownEntryType = CHAR_POUND ;
                break ;
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POUNDS :
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POINTS :
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_PERCENT :
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_FACTOR :
                break ;
            default :
                chPremiumBreakdownEntryType = '?' ;
                break ;
            } /* switch ( chPremiumBreakdownEntryType ) */

        } /* The type is not numeric */

        PceDebugSchemePrintf( hpf , ppldLogData ,
                                "Premium changed by %c with %f to %f" ,
                                    chPremiumBreakdownEntryType ,
                                    ppbEntry->load_disc_value_signed ,
                                    ppbEntry->resultant_prem ) ;

    } /* For Scheme Debugging purposes output any change to the Premium */

} /* MotAddPremiumBreakdownEntryDebug */

/*
.page .section PceAddPremiumBreakdownEntry
+--------------------+ PceAddPremiumBreakdownEntry +----------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|                                                                         |
|   PceAddPremiumBreakdownEntry                                           |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|                                                                         |
|   Add an element to the Premium Breakdown Array assuming that the Rule  |
|   that is currently executing in the PremCalc Framework is the relevant |
|   Rule                                                                  |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   hpf [in]                                                              |
|       Premium Calculation Framework Handle                              |
|   ddLoad [in]                                                           |
|       The load value to be added to the Premium Breakdown               |
|   ddNewPremium [in]                                                     |
|       The premium that is the result of the specified load              |
|   chType [in]                                                           |
|       The type of load that the entry represents. This should be one    |
|       of the values specified by the constants whose names begin with   |
|       PCE_CH_PREM_BREAKDOWN_ROW_TYPE                                    |
|   pirInsurerResults [in/out]                                            |
|       A pointer to the "responses" for the current Scheme which         |
|       contains the Premium Breakdown Array                              |
|   psPremiumBreakdownRowCount [in/out]                                   |
|       A pointer to the value that contains the number of valid entries  |
|       in the premium breakdown                                          |
|                                                                         |
| RETURN VALUE:                                                           |
|                                                                         |
|   eerError                                                              |
|                                                                         |
| LAST MODIFIED DATE: 08-Jun-2009                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
EIS_ERROR EIS_LOCENTRY PceAddPremiumBreakdownEntry(
                            PF_HANDLE           hpf ,
                            DSOS_DOUBLE         ddLoad ,
                            DSOS_DOUBLE         ddNewPremium ,
                            CHAR                chType ,
                            INDIVIDUAL_RESPONSE *pirInsurerResults ,
                            SHORT               *psPremiumBreakdownRowCount )
{
    EIS_ERROR eerError = EIS_NO_ERROR;

    if ( *psPremiumBreakdownRowCount < DSOS_MAX_PREMIUM_BREAKDOWN_ROW_COUNT )
    {
        /* Breakdown array is not full */

        PB *ppbEntry = &pirInsurerResults->pb[*psPremiumBreakdownRowCount] ;

        ppbEntry->load_disc_value_signed = (float) ddLoad;

        ppbEntry->load_disc_type = chType;
        ppbEntry->resultant_prem = (float) ddNewPremium;

        MotAddRuleDetailsToPB( hpf ,
                               ppbEntry ) ;

        MotAddPremiumBreakdownEntryDebug( hpf , ppbEntry ) ;

        /* One more Premium Breakdown Entry */
        *psPremiumBreakdownRowCount += 1 ;

        if ( *psPremiumBreakdownRowCount < DSOS_MAX_PREMIUM_BREAKDOWN_ROW_COUNT )
            /* The next resultant premium value must be set to the last. */
            /* This appears to be because of bad coding a very long time */
            /* ago where references were made to the entry after the     */
            /* element at *psPremiumBreakdownRowCount-1 */
            pirInsurerResults->pb[*psPremiumBreakdownRowCount].resultant_prem = (float) ddNewPremium;

    }   /* Breakdown array is not full */
    else
    {
        /* Breakdown array is full */
        ULONG ulRuleNumber = 0 ;

        PceGlobalDataGetRule( hpf , &ulRuleNumber ) ;

        KerLogError( EIS_ERROR_REF( eerError ) ,
                     eenumPCEBreakdownArrayFull ,
                     esevDefault ,
                     edspNoDisplay ,
                     1 ,
                     ulRuleNumber );

    } /* Breakdown array is full */

    return eerError;

} /* PceAddPremiumBreakdownEntry */

/*
.page .section PceSetRuleDescriptionOfPremiumBreakdownEntry
+-----------+ PceSetRuleDescriptionOfPremiumBreakdownEntry +--------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|                                                                         |
|   PceSetRuleDescriptionOfPremiumBreakdownEntry                          |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|                                                                         |
|   Set the Rule Description member of a Premium Breakdown Entry          |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   ulRuleNumber [in]                                                     |
|       The Rule Number associated with the Premium Breakdown Entry       |
|   ppbEntry [in/out]                                                     |
|       A pointer to the Premium Breakdown Entry into which the Rule      |
|       Description is to be placed                                       |
|                                                                         |
| RETURN VALUE:                                                           |
|   VOID                                                                  |
|                                                                         |
| LAST MODIFIED DATE: 15-Dec-2009                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
static void PceSetRuleDescriptionOfPremiumBreakdownEntry( ULONG ulRuleNumber , PB *ppbEntry )
{
    if ( ulRuleNumber <= GenGetRuleCount() )
    {
        /* Rule Title is available from TMS */

        KitStrNCpy( ppbEntry->Rule_title ,
                    _apszRuleDescription[ ulRuleNumber ] ,
                    sizeof(ppbEntry->Rule_title) ) ;

    } /* Rule Title is available from TMS */
    else
    {
        /* Rule Title is not available from TMS */

        /* If the Rule Number is larger than the largest */
        /* in TMS then assume the PremCalc Framework is */
        /* generating Cover Option (AddOn) Modelling (for now) */
        KitStrNCpy( ppbEntry->Rule_title ,
                    "AddOn Modelling" ,
                    sizeof( ppbEntry->Rule_title ) ) ;

    } /* Rule Title is not available from TMS */

} /* PceSetRuleDescriptionOfPremiumBreakdownEntry */

/*
.page .section PceAddEntirePremiumBreakdownEntry
+-----------------+ PceAddEntirePremiumBreakdownEntry +-------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|                                                                         |
|   PceAddEntirePremiumBreakdownEntry                                     |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|                                                                         |
|   Add an entire Premium Breakdown Entry to the Premium Breakdown Array  |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   hpf [in]                                                              |
|       Premium Calculation Framework Handle                              |
|   ulRuleNumber                                                          |
|       The Rule Number associated with the Premium Breakdown Entry       |
|   ddLoad [in]                                                           |
|       The load value to be added to the Premium Breakdown               |
|   ddNewPremium [in]                                                     |
|       The premium that is the result of the specified load              |
|   chType [in]                                                           |
|       The type of load that the entry represents. This should be one    |
|       of the values specified by the constants whose names begin with   |
|       PCE_CH_PREM_BREAKDOWN_ROW_TYPE                                    |
|   pirInsurerResults [in/out]                                            |
|       A pointer to the "responses" for the current Scheme which         |
|       contains the Premium Breakdown Array                              |
|   psPremiumBreakdownRowCount [in/out]                                   |
|       A pointer to the value that contains the number of valid entries  |
|       in the premium breakdown                                          |
|                                                                         |
| RETURN VALUE:                                                           |
|   eerError                                                              |
|                                                                         |
| LAST MODIFIED DATE: 16-May-2009                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
EIS_ERROR EIS_LOCENTRY PceAddEntirePremiumBreakdownEntry(
                                        PF_HANDLE           hpf ,
                                        ULONG               ulRuleNumber ,
                                        DSOS_DOUBLE         ddLoad ,
                                        DSOS_DOUBLE         ddNewPremium ,
                                        CHAR                chType ,
                                        INDIVIDUAL_RESPONSE *pirInsurerResults ,
                                        SHORT               *psPremiumBreakdownRowCount )
{
    EIS_ERROR eerError = EIS_NO_ERROR;

    if ( *psPremiumBreakdownRowCount < DSOS_MAX_PREMIUM_BREAKDOWN_ROW_COUNT )
    {
        /* Breakdown array is not full */

        PB *ppbEntry = &pirInsurerResults->pb[*psPremiumBreakdownRowCount] ;

        ppbEntry->load_disc_value_signed = (float) ddLoad;

        ppbEntry->load_disc_type = chType;
        ppbEntry->resultant_prem = (float) ddNewPremium;

        ppbEntry->Rule_no = (SHORT) ulRuleNumber ;

        ppbEntry->sRuleInvocationNumber = (SHORT)PceGlobalDataGetRuleInvocationNumber( hpf , NULL ) ;

        PceSetRuleDescriptionOfPremiumBreakdownEntry( ulRuleNumber , ppbEntry ) ;

        MotAddPremiumBreakdownEntryDebug( hpf , ppbEntry ) ;

        /* One more Premium Breakdown Entry */
        *psPremiumBreakdownRowCount += 1 ;

        if ( *psPremiumBreakdownRowCount < DSOS_MAX_PREMIUM_BREAKDOWN_ROW_COUNT )
            /* The next resultant premium value must be set to the last. */
            /* This appears to be because of bad coding a very long time */
            /* ago where references were made to the entry after the     */
            /* element at *psPremiumBreakdownRowCount-1 */
            pirInsurerResults->pb[*psPremiumBreakdownRowCount].resultant_prem = (float) ddNewPremium;

    }   /* Breakdown array is not full */
    else
    {
        /* Breakdown array is full */
        ULONG ulRuleNumber = 0 ;

        PceGlobalDataGetRule( hpf , &ulRuleNumber ) ;

        KerLogError( EIS_ERROR_REF( eerError ) ,
                     eenumPCEBreakdownArrayFull ,
                     esevDefault ,
                     edspNoDisplay ,
                     1 ,
                     ulRuleNumber );

    } /* Breakdown array is full */

    return eerError;

} /* PceAddEntirePremiumBreakdownEntry */

/*
.page .section PceCalculatePremiumAfterLoad
+--------------------+ PceCalculatePremiumAfterLoad +---------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  PceCalculatePremiumAfterLoad                                           |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|                                                                         |
|   Given a premium, a load value (treated as a discount if -ve), and a   |
|   load type, calculates the premium that would result if that           |
|   load/discount were to be applied.                                     |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|                                                                         |
|   ddCurrentPremium - The value of the premium on which the calculation  |
|                      is to be applied                                   |
|                                                                         |
|   ddLoadValue      - The percentage/points/pounds/factor load (or       |
|                      count if -ve) to be applied.                       |
|                                                                         |
|   chLoadType       - The type of load must be either 'œ', '%' , 'P' or  |
|                      '*' or an error will be logged.                    |
|                                                                         |
|   pddResultantPremium - a pointer to a DSOS_DOUBLE value into which     |
|                         will be copied the calculated value of the      |
|                         premium after the load/discount has been        |
|                         applied.                                        |
|                                                                         |
| RETURN VALUE:                                                           |
|                                                                         |
|   If the calculation was completed successfully the function will       |
|   return EIS_NO_ERROR. Otherwise a non-zero return code will be         |
|   returned and an error logged.                                         |
|                                                                         |
| LAST MODIFIED DATE: 06-Dec-2006                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/

EIS_ERROR EIS_EXPENTRY PceCalculatePremiumAfterLoad ( DSOS_DOUBLE ddCurrentPremium     ,
                                                        DSOS_DOUBLE ddLoadValue          ,
                                                        CHAR        chLoadType           ,
                                                        DSOS_DOUBLE *pddResultantPremium )
{
    EIS_ERROR eerError    = EIS_NO_ERROR ;

    DSOS_DOUBLE ddMultiplier ;

    if ( pddResultantPremium == NULL )
    {
        KerLogError( EIS_ERROR_REF( eerError ),
                     eenumPCEInvalidFunctionParameter,
                     esevSevere,
                     edspNoDisplay,
                     2  ,
                     "PceCalculatePremiumAfterDiscount",
                     "pddResultantPremium == NULL" );

    }
    else
    {
        switch ( chLoadType )
        {
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_PERCENT :
                /*
                **  Convert the percentage value to a simple multiplier.
                **  ie 5% becomes 1.05 & -%5 becomes 0.95
                */
                ddMultiplier = 1.0 + (ddLoadValue / 100.0) ;
                *pddResultantPremium = ddCurrentPremium * ddMultiplier ;
                break ;

            case CHAR_POUND_TMS : /* CC : For now allow incorrect Code Page Pound sign too */
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POUNDS :
                *pddResultantPremium = ddCurrentPremium + ddLoadValue ;
                break ;

            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POINTS :
                *pddResultantPremium = ddCurrentPremium + ddLoadValue ;
                break ;

            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_FACTOR :
                ddMultiplier = ddLoadValue / 100.0 ;
                *pddResultantPremium = ddCurrentPremium * ddMultiplier ;
                break ;

            default:

                KerLogError( EIS_ERROR_REF( eerError ),
                             eenumPCEInvalidFunctionParameter,
                             esevSevere,
                             edspNoDisplay,
                             2  ,
                             "PceCalculatePremiumAfterDiscount",
                             "chLoadType != '£', '%', 'P' or '*'" );
                break ;
        }
    }

    return eerError ;

} /* PceCalculatePremiumAfterLoad */

/*
.page .section PceConvertLoadPercentage
+----------------------+ PceConvertLoadPercentage +-----------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  EIS_ERROR EIS_EXPENTRY PceConvertLoadPercentage (                      |
|                                   DSOS_DOUBLE ddCurrentPremium ,        |
|                                   DSOS_DOUBLE ddPercentageLoad ,        |
|                                   CHAR        chLoadType ,              |
|                                   DSOS_DOUBLE *pddResultantLoad )       |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|   Passed a premium, a percentage load and load type will calculate      |
|   the value of that load type which when applied will result in a       |
|   load equivalent to the percentage load.                               |
|                                                                         |
|   NOTE: This function will work for discounts, simply pass a negative   |
|         ddPercentageLoad value.                                         |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   ddCurrentPremium - The current value of the premium.                  |
|                                                                         |
|   ddPercentageLoad - The percentage value to base the calculation on    |
|                                                                         |
|   chLoadType       - The units to convert to.                           |
|                                                                         |
|   pddResultantLoad - Buffer to hold the result.                         |
|                                                                         |
| RETURN VALUE:                                                           |
|                                                                         |
|   If the function was successful EIS_NO_ERROR is returned. Otherwise    |
|   a non-zero error code is returned and an error message logged.        |
|   Possible return codes are:                                            |
|      eenuPCEInvalidFunctionParameter - If a parameter passed is invalid |
|                                                                         |
| LAST MODIFIED DATE: 06-Jun-2006                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/

EIS_ERROR EIS_EXPENTRY PceConvertLoadPercentage ( DSOS_DOUBLE ddCurrentPremium  ,
                                                    DSOS_DOUBLE ddPercentageLoad  ,
                                                    CHAR        chLoadType        ,
                                                    DSOS_DOUBLE *pddResultantLoad )
{
    EIS_ERROR  eerError = EIS_NO_ERROR;

    if ( pddResultantLoad == NULL )
    {
        KerLogError( EIS_ERROR_REF( eerError ),
                     eenumPCEInvalidFunctionParameter,
                     esevSevere,
                     edspNoDisplay,
                     2  ,
                     "PceConvertLoadPercentage",
                     "pddResultantLoad == NULL" ) ;
    }
    else
    {
        switch ( chLoadType )
        {
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_PERCENT :  /* No conversion to be done */
                *pddResultantLoad = ddPercentageLoad ;
                break ;
            case CHAR_POUND_TMS : /* CC : For now allow incorrect Code Page Pound sign too */
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POUNDS :
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POINTS :
                *pddResultantLoad = (ddPercentageLoad/100) * ddCurrentPremium ;
                break ;
            case PCE_CH_PREM_BREAKDOWN_ROW_TYPE_FACTOR :
                *pddResultantLoad = 1 + (ddPercentageLoad/100) ;
                break ;
            default :
                *pddResultantLoad = 0.0 ;
                KerLogError( EIS_ERROR_REF( eerError ),
                             eenumPCEInvalidFunctionParameter,
                             esevSevere,
                             edspNoDisplay,
                             2  ,
                             "PceConvertLoadPercentage",
                             "chLoadType is invalid" ) ;
                break ;
        } /* End switch ( chLoadType ) */
    }

    return( eerError );
} /* PceConvertLoadPercentage */

/*
.page .section MotAddRuleDetailsToPB
+-----------------------+ MotAddRuleDetailsToPB +-------------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  MotAddRuleDetailsToPB                                                  |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|   By reference to the PremCalc Framework, adds the details of the       |
|   currently executing Rule to the supplied Premium Breakdown Array      |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   hpf [in]                                                              |
|       Premium Calculation framework handle                              |
|                                                                         |
|   ppb [out]                                                             |
|       A pointer to the Premium Breakdown entry for which Rule Details   |
|       are to be generated                                               |
|                                                                         |
| RETURN VALUE:                                                           |
|   VOID                                                                  |
|                                                                         |
| LAST MODIFIED DATE: 16-May-2011                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
void MotAddRuleDetailsToPB( PF_HANDLE hpf , PB *ppbEntry )
{
    ULONG ulRuleNumber = 0 ;

    PceGlobalDataGetRule( hpf , &ulRuleNumber ) ;

    ppbEntry->Rule_no = (SHORT) ulRuleNumber ;

    ppbEntry->sRuleInvocationNumber = (SHORT)PceGlobalDataGetRuleInvocationNumber( hpf , NULL ) ;

    PceSetRuleDescriptionOfPremiumBreakdownEntry( ulRuleNumber , ppbEntry ) ;

} /* MotAddRuleDetailsToPB */

/*
.page .section MessageExists
+---------------------------+ MessageExists +-----------------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  MessageExists                                                          |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|   Checks the Premium Message Array for the presence of the specified    |
|   Message of the specified Type                                         |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|                                                                         |
|   chMessageType [in]                                                    |
|       The Type of the Message for which to check                        |
|   sMessageNumber [in]                                                   |
|       The Message Number for which to check                             |
|   pirInsurerResponse [in]                                               |
|       The Scheme Result whose Messages are to be checked                |
|   sMessageCount [in]                                                    |
|       The number of messages that exist in the Message Array            |
|                                                                         |
| RETURN VALUE:                                                           |
|   FALSE = The specified message was not found in the supplied Message   |
|           Array                                                         |
|   TRUE  = The specified message was found in the supplied Message Array |
|                                                                         |
| LAST MODIFIED DATE: 05-Oct-2011                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
static BOOL MessageExists( CHAR chMessageType ,
                           SHORT sMessageNumber ,
                           const INS_RESPONSE *pirInsurerResponse ,
                           SHORT sMessageCount )
{
    LONG lIndex = 0 ;
    BOOL bFound = FALSE ;

    /*
    **  Check for duplicates
    */
    for ( lIndex = 0 ;
             ( ! bFound )
          && ( lIndex < sMessageCount ) ;
          lIndex++
        )
    {
        if (    ( pirInsurerResponse->prem_msg[lIndex].msg_no == sMessageNumber )
             && ( pirInsurerResponse->prem_msg[lIndex].type == chMessageType )
           )
        {
            bFound = TRUE ;
        }
    }

    return bFound ;

} /* MessageExists */

/*
.page .section MotAddUniqueInformationalMessageNumber
+--------------+ MotAddUniqueInformationalMessageNumber +-----------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  MotAddUniqueInformationalMessageNumber                                 |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|  Adds an Informational message to the Premium Breakdown                 |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   hpf [in]                                                              |
|       Premium Calculation framework handle                              |
|   sMessage [in]                                                         |
|       The Informational Message Number to be added                      |
|   pirInsurerResponse [in/out]                                           |
|       A pointer to the Scheme Result that contains the Message Array    |
|   psMessageCount [in/out]                                               |
|       A pointer to the count of messages in the Message Array that will |
|       be updated by this function                                       |
|                                                                         |
| RETURN VALUE:                                                           |
|   VOID                                                                  |
|                                                                         |
| LAST MODIFIED DATE: 20-May-2009                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
BOOL MotAddUniqueInformationalMessageNumber( PF_HANDLE hpf ,
                                             SHORT sMessageNumber ,
                                             INS_RESPONSE *pirInsurerResponse ,
                                             SHORT *psMessageCount )
{
    BOOL bMessageAdded = FALSE ;

    if ( ! MessageExists( MOT_CH_CALC_STATUS_INFORMATIONAL ,
                          sMessageNumber ,
                          pirInsurerResponse ,
                          *psMessageCount
                        )
       )
    {
        /* Message not currently present */

        if ( *psMessageCount >= DSOS_MAX_PREMCALC_UNDERWRITING_MESSAGE_COUNT )
            PceDebugSchemePrintf( hpf , NULL ,
                                    "Underwriting Message array is full at %ld. Informational Message Number %ld not added" ,
                                        *psMessageCount ,
                                        sMessageNumber ) ;
        else
        {
            /* Add the message */

            ULONG ulMessageIndex = *psMessageCount ;

            PceDebugSchemePrintf( hpf , NULL , "Add Informational Message Number %ld" , sMessageNumber ) ;

            pirInsurerResponse->prem_msg[ ulMessageIndex ].msg_no = sMessageNumber ;
            pirInsurerResponse->prem_msg[ ulMessageIndex ].type = MOT_CH_CALC_STATUS_INFORMATIONAL ;

            /* One more message added */
            *psMessageCount = (SHORT)( ulMessageIndex + 1 ) ;

            bMessageAdded = TRUE ;

        } /* Add the message */

    } /* Message not currently present */

    return bMessageAdded ;

} /* MotAddUniqueInformationalMessageNumber */

/*
.page .section MotAddDeclineMessageWithContext
+------------------+ MotAddDeclineMessageWithContext +--------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  MotAddDeclineMessageWithContext                                        |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|  Adds a Decline message to the Premium Breakdown                        |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   hpf [in]                                                              |
|       Premium Calculation framework handle                              |
|   sMessage [in]                                                         |
|       The Message Number to be added                                    |
|   pirInsurerResponse [in/out]                                           |
|       A pointer to the Scheme Result that contains the Message Array    |
|   psMessageCount [in/out]                                               |
|       A pointer to the count of messages in the Message Array that will |
|       be updated by this function                                       |
|                                                                         |
| RETURN VALUE:                                                           |
|   VOID                                                                  |
|                                                                         |
| LAST MODIFIED DATE: 05-Oct-2011                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
BOOL MotAddDeclineMessageWithContext( PF_HANDLE hpf ,
                                      SHORT sMessage ,
                                      INS_RESPONSE *pirInsurerResponse ,
                                      SHORT *psMessageCount )
{
    BOOL bMessageAdded = FALSE ;

    if ( ! MessageExists( MOT_CH_CALC_STATUS_DECLINE ,
                          sMessage ,
                          pirInsurerResponse ,
                          *psMessageCount
                        )
       )
    {
        /* This message is not in the array already */

        ULONG ulMessageIndex = *psMessageCount ;

        PceDebugSchemePrintf( hpf , NULL , "Add Decline Message Number %ld" , sMessage ) ;

        /* Make sure that the array does not overflow with this Decline Message */
        if ( ulMessageIndex >= DSOS_MAX_PREMCALC_UNDERWRITING_MESSAGE_COUNT )
            /* The index must be the last valid location in the array */
            ulMessageIndex = DSOS_MAX_PREMCALC_UNDERWRITING_MESSAGE_COUNT - 1 ;

        pirInsurerResponse->prem_msg[ ulMessageIndex ].msg_no = sMessage ;
        pirInsurerResponse->prem_msg[ ulMessageIndex ].type = MOT_CH_CALC_STATUS_DECLINE ;

        /* One more message added */
        *psMessageCount = (SHORT)( ulMessageIndex + 1 ) ;

        PCE_CALC_STATUS = MOT_CH_CALC_STATUS_DECLINE ;
        PCE_FINAL_PREMIUM = MOT_DECLINE_PREMIUM_VALUE ;

        bMessageAdded = TRUE ;

    } /* This message is not in the array already */

    return bMessageAdded ;

} /* MotAddDeclineMessageWithContext */

/*
.page .section MotAddReferMessageWithContext
+-------------------+ MotAddReferMessageWithContext +---------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  MotAddReferMessageWithContext                                          |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|  Adds a Refer message to the Premium Breakdown                          |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|   hpf [in]                                                              |
|       Premium Calculation framework handle                              |
|   sMessage [in]                                                         |
|       The Message Number to be added                                    |
|   pirInsurerResponse [in/out]                                           |
|       A pointer to the Scheme Result that contains the Message Array    |
|   psMessageCount [in/out]                                               |
|       A pointer to the count of messages in the Message Array that will |
|       be updated by this function                                       |
|                                                                         |
| RETURN VALUE:                                                           |
|   FALSE = The Message was not added because it already exists in the    |
|           PremCalc result                                               |
|   TRUE  = The Message was added                                         |
|                                                                         |
| LAST MODIFIED DATE: 05-Oct-2011                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
BOOL MotAddReferMessageWithContext( PF_HANDLE hpf ,
                                    SHORT sMessage ,
                                    INS_RESPONSE *pirInsurerResponse ,
                                    SHORT *psMessageCount )
{
    BOOL bMessageAdded = FALSE ;

    if ( ! MessageExists( MOT_CH_CALC_STATUS_REFER ,
                          sMessage ,
                          pirInsurerResponse ,
                          *psMessageCount
                        )
       )
    {
        /* This message is not in the array already */

        ULONG ulMessageIndex = *psMessageCount ;

        PceDebugSchemePrintf( hpf , NULL , "Add Refer Message Number %ld" , sMessage ) ;

        /* Make sure that the array does not overflow with this Refer Message */
        if ( ulMessageIndex >= DSOS_MAX_PREMCALC_UNDERWRITING_MESSAGE_COUNT )
            /* The index must be the last valid location in the array */
            ulMessageIndex = DSOS_MAX_PREMCALC_UNDERWRITING_MESSAGE_COUNT - 1 ;

        pirInsurerResponse->prem_msg[ ulMessageIndex ].msg_no = sMessage ;
        pirInsurerResponse->prem_msg[ ulMessageIndex ].type = MOT_CH_CALC_STATUS_REFER ;

        /* One more message added */
        *psMessageCount = (SHORT)( ulMessageIndex + 1 ) ;

        /*
        **  Never change a Decline to a Refer.
        **  If the case has already been declined don't touch the calculation status.
        */
        if ( PCE_CALC_STATUS != MOT_CH_CALC_STATUS_DECLINE )
            PCE_CALC_STATUS = MOT_CH_CALC_STATUS_REFER ;

        bMessageAdded = TRUE ;

    } /* This message is not in the array already */

    return bMessageAdded ;

} /* MotAddReferMessageWithContext */

/*
.page .section MotPremiumBreakdownAddCoverOption
+-----------------+ MotPremiumBreakdownAddCoverOption +-------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|  MotPremiumBreakdownAddCoverOption                                      |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|   Adds a Cover Option to the Premium Breakdown                          |
|                                                                         |
|                                                                         |
|   Cover Option Premium Breakdown Considerations                         |
|                                                                         |
|   Adding Cover Option Details to the Premium Breakdown is absolutely    |
|   necessary for Cover Options with a "Selected" or "Compulsory" state   |
|   and a non-zero premium because PMD and the rest of the System assume  |
|   this behaviour. This is because it is assumed that the final premium  |
|   for a Scheme includes all "Selected" or "Compulsory" Cover Options.   |
|                                                                         |
|   Amongst the other considerations, only consider adding a Cover        |
|   Option to the Premium Breakdown if its premium is non-zero. This      |
|   reduces the potentially "unnecessary" number of Premium Breakdown     |
|   Entries that result from Cover Options that are really "Benefits" but |
|   unfortunately excludes those "proper" Cover Options that really do    |
|   have a zero premium under some conditions. This zero premium "proper" |
|   Cover Option limitation has been accepted by PMD.                     |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|                                                                         |
|   MOTOR_EFFECT_PARAMETERS [in/out]                                      |
|       The parameters that are passed to all Effects some of which will  |
|       be modified to generate a new premium                             |
|   pacovod [in]                                                          |
|       A pointer the Cover Option Details array for the PremCalc         |
|   ulCoverOptionDetailsMaxCount [in]                                     |
|       The count of the maximum number of items that may exist in the    |
|       array pointed to by pacovod                                       |
|   ecstType [in]                                                         |
|       The type of the Cover Option whose details are being added to the |
|       Premium Breakdown                                                 |
|   ecsconCurrent [in]                                                    |
|       The current Cover Section Selection Control value for the Cover   |
|       Option that pertains to the current Premium Breakdown             |
|                                                                         |
| RETURN VALUE:                                                           |
|   VOID                                                                  |
|                                                                         |
| LAST MODIFIED DATE: 13-Nov-2017                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/
BOOL MotPremiumBreakdownAddCoverOption( MOTOR_EFFECT_PARAMETERS ,
                                        const OSM_COVER_OPTION_DETAILS *pacovod ,
                                        ULONG ulCoverOptionDetailsMaxCount ,
                                        ECoverSectionType ecstType ,
                                        ECoverSectionSelectionControl ecsconCurrent )
{
    BOOL bAdded = FALSE ;

    EIS_ERROR eerError = EIS_NO_ERROR ;

    ECoverSectionSelectionControl ecsconCoverOption = ecsconUnknown ;

    eerError = PceGetCoverOptionPropertyCoverSectionSelectionControl( hpf ,
                                                                      ppldLogData ,
                                                                      PCE_VOL_COVER_OPTION_DETAILS_ARRAY ,
                                                                      PCE_VOL_COVER_OPTION_DETAILS_ARRAY_MAX_COUNT ,
                                                                      ecstType ,
                                                                      FALSE , /* Don't add if not already present */
                                                                      &ecsconCoverOption ) ;
    if (    ( EIS_ERROR_NUM( eerError ) == EIS_OK )
         && ( ecsconCurrent != ecsconCoverOption )
         && (    ( ecsconCurrent != ecsconUnknown )
              /* Only allow subtraction of premium that has been added previously */
              || (    ( ecsconCurrent == ecsconUnknown )
                   && ( ecsconCoverOption == ecsconDefaultSelected )
                 )
              || (    ( ecsconCurrent == ecsconUnknown )
                   && ( ecsconCoverOption == ecsconCompulsory )
                 )
            )
         && (    ( ecsconCurrent == ecsconDefaultDeselected ) || ( ecsconCurrent == ecsconForbidden )
              && ( ! ( ( ecsconCoverOption == ecsconDefaultDeselected ) || ( ecsconCoverOption == ecsconForbidden ) ) )
            )
         && (    ( ecsconCurrent == ecsconDefaultSelected ) || ( ecsconCurrent == ecsconCompulsory )
              && ( ! ( ( ecsconCoverOption == ecsconDefaultSelected ) || ( ecsconCoverOption == ecsconCompulsory ) ) )
            )
       )
    {
        /* Located the Cover Option */

        double dblCoverOptionPremium = 0.0 ;

        eerError = PceGetCoverOptionPropertyPremium( hpf ,
                                                     ppldLogData ,
                                                     (OSM_COVER_OPTION_DETAILS *)pacovod ,
                                                     ulCoverOptionDetailsMaxCount ,
                                                     ecstType ,
                                                     FALSE , /* Don't add if not already present */
                                                     &dblCoverOptionPremium ) ;

        if ( EIS_ERROR_NUM( eerError ) == EIS_OK )
        {
            /* Got Cover Option premium */

            CHAR szCoverOptionTMSCode[ PCE_CB_TABLE_ENTRY + 1 ] = { "" } ;
            PceGetTMSCoverSectionCode( ecstType , szCoverOptionTMSCode , sizeof( szCoverOptionTMSCode ) ) ;

            if ( EIS_DBL_IS_EFFECTIVELY_ZERO( dblCoverOptionPremium , EIS_DBL_ABS_EFFECTIVE_ZERO ) )
            {
                PceDebugSchemePrintf( hpf , ppldLogData ,
                    "MotPremiumBreakdownAddCoverOption : Cover Option \"%s\" has zero Premium"
                    " and will not need to be included in the Premium Breakdown",
                        szCoverOptionTMSCode ) ;
            }
            else
            {
                /* Non-zero Cover Option premium */

                if (    ( ecsconCoverOption == ecsconDefaultDeselected )
                     || ( ecsconCoverOption == ecsconForbidden )
                   )
                {
                    /* Ensure that the opposite of the Cover Option is added */
                    dblCoverOptionPremium = -dblCoverOptionPremium ;
                }

                PceDebugSchemePrintf( hpf , ppldLogData ,
                    "MotPremiumBreakdownAddCoverOption : Cover Option \"%s\" has non-zero Premium"
                    " and will be included in the Premium Breakdown with %f",
                        szCoverOptionTMSCode , dblCoverOptionPremium ) ;

                /* Add to the Premium Breakdown */
                PCE_APPLY_LOAD_TO_PB( dblCoverOptionPremium ,
                                        PCE_CH_PREM_BREAKDOWN_ROW_TYPE_POUNDS ) ;

            } /* Non-zero Cover Option premium */

            bAdded = TRUE ;

        } /* Got Cover Option premium */

    } /* Located the Cover Option */

    return bAdded ;

} /* MotPremiumBreakdownAddCoverOption */
