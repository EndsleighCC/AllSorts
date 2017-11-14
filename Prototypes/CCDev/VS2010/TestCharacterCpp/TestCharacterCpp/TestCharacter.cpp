// Test Character conversions and display
// C. Cornelius 9th November 2012

#include <iostream>
using namespace std ;

#include <locale.h>

/***********************************************************/
/* Windows Code Page 1252 to/from EBCDIC conversion tables */
/***********************************************************/

/****************************************************************************/
/* These tables are written with hexadecimal values rather than displayable */
/* characters to avoid problems with display Code Page conversions.         */
/*                                                                          */
/* The character to be converted is used as the index into the appropriate  */
/* conversion array.                                                        */
/* For example, upper case ASCII 'A' (0x41) indexes into the ASCII to       */
/* EBCDIC table to generate uppercase EBCDIC 'A' 0xc1                       */
/****************************************************************************/

static const int g_aintASCIIToEBCDICTable[] =
{
    /*       0    1    2    3    4    5    6    7    8    9    A    B    C    D    E    F */
    /* 0 */ 0x00,0x01,0x02,0x03,0x37,0x2D,0x2E,0x2F,0x16,0x05,0x25,0x0B,0x0C,0x0D,0x0E,0x0F,
    /* 1 */ 0x10,0x11,0x12,0x13,0x3C,0x3D,0x32,0x26,0x18,0x19,0x3F,0x27,0x22,0x1D,0x35,0x1F,
    /* 2 */ 0x40,0x5A,0x7F,0x7B,0x4A,0x6C,0x50,0x7D,0x4D,0x5D,0x5C,0x4E,0x6B,0x60,0x4B,0x61,
    /* 3 */ 0xF0,0xF1,0xF2,0xF3,0xF4,0xF5,0xF6,0xF7,0xF8,0xF9,0x7A,0x5E,0x4C,0x7E,0x6E,0x6F,
    /* 4 */ 0x7C,0xC1,0xC2,0xC3,0xC4,0xC5,0xC6,0xC7,0xC8,0xC9,0xD1,0xD2,0xD3,0xD4,0xD5,0xD6,
    /* 5 */ 0xD7,0xD8,0xD9,0xE2,0xE3,0xE4,0xE5,0xE6,0xE7,0xE8,0xE9,0xAD,0xE0,0xBD,0x5F,0x6D,
    /* 6 */ 0x79,0x81,0x82,0x83,0x84,0x85,0x86,0x87,0x88,0x89,0x91,0x92,0x93,0x94,0x95,0x96,
    /* 7 */ 0x97,0x98,0x99,0xA2,0xA3,0xA4,0xA5,0xA6,0xA7,0xA8,0xA9,0xC0,0x6A,0xD0,0xA1,0x07,
    /* 8 */ 0x68,0xDC,0x51,0x42,0x43,0x44,0x47,0x48,0x52,0x53,0x54,0x57,0x56,0x58,0x63,0x67,
    /* 9 */ 0x71,0x9C,0x9E,0xCB,0xCC,0xCD,0xDB,0xDD,0xDF,0xEC,0xFC,0xB0,0x5B,0xB2,0xBF,0xB4,
    /* A */ 0x45,0x55,0xCE,0x5B,0x49,0x69,0x9A,0x9B,0xAB,0xE1,0x04,0xB8,0xB7,0xAA,0x8A,0x8B,
    /* B */ 0x06,0x08,0x09,0x0A,0x14,0x15,0x17,0x1A,0x1B,0x1C,0x1E,0x20,0x21,0x23,0x24,0x28,
    /* C */ 0x29,0x2A,0x2B,0x2C,0x30,0x31,0x33,0x34,0x36,0x38,0x39,0x3A,0x3B,0x3E,0x41,0x46,
    /* D */ 0x4F,0x62,0x64,0x65,0x66,0x72,0x73,0x74,0x75,0x76,0x77,0x78,0x80,0x8C,0x8D,0x8E,
    /* E */ 0x9D,0x59,0x9F,0xAC,0xAE,0xAF,0xA0,0xEB,0xED,0xEE,0xEF,0xFA,0xFB,0x70,0xFD,0xFE,
    /* F */ 0xB1,0x8F,0xB3,0xB5,0xB6,0xB9,0xBA,0xBB,0x90,0xBC,0xBE,0xCA,0xCF,0xEA,0xDA,0xFF
};

static const int g_aintEBCDICToASCIITable[] =
{
    /*       0    1    2    3    4    5    6    7    8    9    A    B    C    D    E    F */
    /* 0 */ 0x00,0x01,0x02,0x03,0xAA,0x09,0xB0,0x7F,0xB1,0xB2,0xB3,0x0B,0x0C,0x0D,0x0E,0x0F,
    /* 1 */ 0x10,0x11,0x12,0x13,0xB4,0xB5,0x08,0xB6,0x18,0x19,0xB7,0xB8,0xB9,0x1D,0xBA,0x1F,
    /* 2 */ 0xBB,0xBC,0x1C,0xBD,0xBE,0x0A,0x17,0x1B,0xBF,0xC0,0xC1,0xC2,0xC3,0x05,0x06,0x07,
    /* 3 */ 0xC4,0xC5,0x16,0xC6,0xC7,0x1E,0xC8,0x04,0xC9,0xCA,0xCB,0xCC,0x14,0x15,0xCD,0x1A,
    /* 4 */ 0x20,0xCE,0x83,0x84,0x85,0xA0,0xCF,0x86,0x87,0xA4,0x24,0x2E,0x3C,0x28,0x2B,0xD0,
    /* 5 */ 0x26,0x82,0x88,0x89,0x8A,0xA1,0x8C,0x8B,0x8D,0xE1,0x21,0xA3,0x2A,0x29,0x3B,0x5E,
    /* 6 */ 0x2D,0x2F,0xD1,0x8E,0xD2,0xD3,0xD4,0x8F,0x80,0xA5,0x7C,0x2C,0x25,0x5F,0x3E,0x3F,
    /* 7 */ 0xED,0x90,0xD5,0xD6,0xD7,0xD8,0xD9,0xDA,0xDB,0x60,0x3A,0x23,0x40,0x27,0x3D,0x22,
    /* 8 */ 0xDC,0x61,0x62,0x63,0x64,0x65,0x66,0x67,0x68,0x69,0xAE,0xAF,0xDD,0xDE,0xDF,0xF1,
    /* 9 */ 0xF8,0x6A,0x6B,0x6C,0x6D,0x6E,0x6F,0x70,0x71,0x72,0xA6,0xA7,0x91,0xE0,0x92,0xE2,
    /* A */ 0xE6,0x7E,0x73,0x74,0x75,0x76,0x77,0x78,0x79,0x7A,0xAD,0xA8,0xE3,0x5B,0xE4,0xE5,
    /* B */ 0x9B,0xF0,0x9D,0xF2,0x9F,0xF3,0xF4,0xAC,0xAB,0xF5,0xF6,0xF7,0xF9,0x5D,0xFA,0x9E,
    /* C */ 0x7B,0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0xFB,0x93,0x94,0x95,0xA2,0xFC,
    /* D */ 0x7D,0x4A,0x4B,0x4C,0x4D,0x4E,0x4F,0x50,0x51,0x52,0xFE,0x96,0x81,0x97,0xA3,0x98,
    /* E */ 0x5C,0xA9,0x53,0x54,0x55,0x56,0x57,0x58,0x59,0x5A,0xFD,0xE7,0x99,0xE8,0xE9,0xEA,
    /* F */ 0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0xEB,0xEC,0x9A,0xEE,0xEF,0xFF
} ;

/*
.page .section OsmCharacterConvert
+-------------------+ OsmCharacterConvert +-------------------------------+
| # header #
|
| FUNCTION DEFINITION:
*/

#define CHAR_POUND               ('£')
#define CHAR_POUND_CP1252        ('£')    /* Code Page 1252 : Dec=163 , Octal=243 , Hex=0xa3 */
#define CHAR_POUND_CP850         ('\234') /* Code Page 850  : Dec=156 , Octal=234 , Hex=0x9c */
#define CHAR_POUND_TMS           ('œ')    /* EIS OS/2 Configuration uses Code Page 850. This */
                                          /* character has been placed here as a literal so  */
                                          /* that it will be converted on transfer to the    */
                                          /* iSeries in exactly the same way as any literal  */
                                          /* Pound signs in TMS Rule parameters. */

#define EIS_ENUM_MAX_32BIT      0x7FFFFFFF

typedef enum _EIS_EDATA_REP
{

    edrUnspecified              , /* Zero on all platforms */

    edrASCII  = 16777217        , /* = 0x01000001 which is a byte-wise palindrome and hence is the same for all platforms */
    edrEBCDIC = 33554434        , /* = 0x02000002 which is a byte-wise palindrome and hence is the same for all platforms */

    edrEnd = EIS_ENUM_MAX_32BIT   /* Some platforms only have 32-bit enumerations */

} EIS_EDATA_REP;  /* edr */

typedef void VOID ;
typedef char CHAR ;
typedef unsigned char UCHAR ;
typedef UCHAR * PUCHAR ;
typedef unsigned long ULONG ;

VOID OsmCharacterConvert( EIS_EDATA_REP edrConvertTo,
                          PUCHAR puch,
                          ULONG ulSize )

/*
| FUNCTIONAL DESCRIPTION:
|   Converts an array of characters between ASCII and EBCDIC.
|
| FORMAL PARAMETERS:
|
|   edrConvertTo            Indicates TO which representation the
|                           characters are to be converted
|   puch                    Pointer to the first character in the array.
|   ulSize                  The number of characters in the array.
|
| RETURN VALUE:
|
|   VOID
|
| LAST MODIFIED DATE: 22-Feb-1999
|
|  # header-end #
+-------------------------------------------------------------------------+
*/
{
    /*------------------------------------------*/
    /* Repeat for all characters to convert:    */
    /* Look up the character in the appropriate */
    /* conversion table and replace with the    */
    /* character in the new representation.     */
    /*------------------------------------------*/

    if ( edrConvertTo == edrEBCDIC )
    {
        /* Convert ASCII to EBCDIC */

        for ( /* puch = puch */ ; ulSize-- ; puch++ )
            *puch = (char)g_aintASCIIToEBCDICTable[ *puch ] ;

    } /* Convert ASCII to EBCDIC */
    else
    {
        /* Convert EBCDIC to ASCII */

        for ( /* puch = puch */ ; ulSize-- ; puch++ )
            *puch = (char)g_aintEBCDICToASCIITable[ *puch ] ;

    } /* Convert EBCDIC to ASCII */

} /* OsmCharacterConvert() */



void CheckForSymmetricalConversion( void )
{
    int intChar ;

    cout << "Unsigned comparison" << endl ;

    const char * pcszLocale = setlocale( LC_ALL , NULL ) ;

    for ( intChar = 155 ; intChar < 256 ; ++intChar )
    {
        UCHAR uchStart = (unsigned char)intChar ;
        UCHAR uchEBCDIC = (unsigned char)intChar ;
        UCHAR uchEnd = (unsigned char)intChar ;

        // Convert to EBCDIC
        OsmCharacterConvert( edrEBCDIC , &uchEBCDIC , (ULONG)sizeof( uchEBCDIC ) );
        uchEnd = uchEBCDIC ;
        // Convert back to ASCII
        OsmCharacterConvert( edrASCII , &uchEnd , (ULONG)sizeof( uchEnd ) );

        if ( uchStart != uchEnd )
        {
            const unsigned char uchOS2Pound = CHAR_POUND_TMS ;
            const unsigned char uchWinPoundSign = CHAR_POUND ;
            const char * pcszOS2PoundText = "" ;
            const char * pcszWinPoundText = "" ;

            if ( uchStart == (unsigned char)CHAR_POUND_TMS )
                pcszOS2PoundText = "OS/2 Pound Sign = " ;

            if ( uchEnd = (unsigned char)CHAR_POUND )
                pcszWinPoundText = " = Windows Pound Sign" ;

            cout << pcszOS2PoundText ;
            cout << (int)intChar ;

            const char *pcszThisLocale1 = setlocale( LC_ALL , NULL ) ;
            const char *pcszThisLocale2 = setlocale( LC_ALL , NULL ) ;

            cout << " = 0x" ;
            cout << hex << intChar << dec ;
            cout << " = " << uchStart << " returned EBCDIC " ;
            cout << (unsigned int)uchEBCDIC << " = 0x" << hex << (unsigned int)uchEBCDIC << dec ;
            cout << " as " << (unsigned int)uchEnd << " = 0x" << hex << (unsigned int)uchEnd  << dec ;
            cout << " = " << uchEnd << pcszWinPoundText << endl ;
        }

        if ( strcmpi( pcszLocale , "English_United Kingdom.1252" ) != 0 )
            cout << "Changed!!" << endl ;

    } // for intChar

    cout << "Signed comparison" << endl ;

    for ( intChar = 0 ; intChar < 256 ; ++intChar )
    {
        CHAR chStart = (char)intChar ;
        CHAR chEBCDIC = (char)intChar ;
        CHAR chEnd = (char)intChar ;

        // Convert to EBCDIC
        OsmCharacterConvert( edrEBCDIC , (unsigned char *)&chEBCDIC , (ULONG)sizeof( chEBCDIC ) );
        chEnd = chEBCDIC ;
        // Convert back to ASCII
        OsmCharacterConvert( edrASCII , (unsigned char *)&chEnd , (ULONG)sizeof( chEnd ) );

        if ( chStart != chEnd )
        {
            const char uchOS2Pound = CHAR_POUND_TMS ;
            const char uchWinPoundSign = CHAR_POUND ;
            const char * pcszOS2PoundText = "" ;
            const char * pcszWinPoundText = "" ;

            if ( chStart == CHAR_POUND_TMS )
                pcszOS2PoundText = "OS/2 Pound Sign = " ;

            if ( chEnd = CHAR_POUND )
                pcszWinPoundText = " = Windows Pound Sign" ;

            cout << pcszOS2PoundText << intChar << " = 0x" << hex << intChar << dec
                 << " = " << chStart << " returned EBCDIC "
                 << (int)chEBCDIC << " = 0x" << hex << (int)chEBCDIC << dec
                 << " as " << (int)chEnd << " = 0x" << hex << (int)chEnd  << dec
                 << " = " << chEnd << pcszWinPoundText << endl ;
        }

    } // for intChar

} // CheckForSymmetricalConversion

int main( const int cintArgCount , const char * apcszArgValue[] )
{
    int intError = 0 ;

    const char *pcszLocale = setlocale( LC_ALL , "" ) ;

    CheckForSymmetricalConversion() ;

    return intError ;

} // main
