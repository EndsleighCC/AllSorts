// TestCHAR.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
using namespace std ;

#include <stdlib.h>
#include <ctype.h>

#define CHAR_POUND        ('£')
#define CHAR_POUND_CP1252 ('£')    /* Code Page 1252 (Windows)   : Dec=163 , Octal=243 , Hex=0xa3 */
#define CHAR_POUND_CP850  ('\234') /* Code Page 850  (UK ASCII)  : Dec=156 , Octal=234 , Hex=0x9c */
#define CHAR_POUND_CP285  ('\133') /* Code Page 285  (UK EBCDIC) : Dec=91 ,  Octal=133 , Hex=0x5b */
#define CHAR_POUND_TMS    ('œ')    /* EIS OS/2 Configuration uses Code Page 850. This */
                                   /* character has been placed here as a literal so  */
                                   /* that it will be converted on transfer to the    */
                                   /* iSeries in exactly the same way as any literal  */
                                   /* Pound signs in TMS Rule parameters. */

#define CHAR_POUND_CP850_TO_CP1252_TO_CP285 ('\04')  /* The value that a Code Page 850 Pound Sign appears */
                                                     /* to become when it is included in a Code Page 850 */
                                                     /* file that is "treated" as Code Page 1252 and that */
                                                     /* entire file is converted from Code Page 1252 */
                                                     /* to Code Page 285 */

int _tmain(int argc, _TCHAR* argv[])
{
    char chPound = (char)0xa3 ;
    int intPound = 0xa3 ;

    int intPound285 = CHAR_POUND_CP285 ;

    int intPound285from1252 = CHAR_POUND_CP850_TO_CP1252_TO_CP285 ;

    int intPoundPlusOne = (unsigned char)chPound ;

    bool isChar = (intPoundPlusOne + 1) <= 256 ;

    if ( isChar )
        cout << "Character \"" << chPound << "\" is a character" << endl ;
    else
        cout << "Character \"" << chPound << "\" is NOT a character" << endl ;

    if ( isdigit( (unsigned char)chPound ) )
        cout << "Character \"" << chPound << "\" is a digit" << endl ;
    else
        cout << "Character \"" << chPound << "\" is NOT a digit" << endl ;

	return 0;
}

