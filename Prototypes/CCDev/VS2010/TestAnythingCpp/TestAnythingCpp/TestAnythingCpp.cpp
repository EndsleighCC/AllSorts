// TestAnythingCpp.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#define _CRT_SECURE_NO_WARNINGS

#include <stdlib.h>
#include <string.h>

#include <iostream>
using namespace std ;

typedef unsigned long ULONG ;
#define KitStrNCpy( _pszDest , _pszSource , _ulSize ) strcpy( _pszDest , _pszSource )
#define KitStrNCat( _pszDest , _pszSource , _ulSize ) strcat( _pszDest , _pszSource )

void KitDebugFileName( char *pszDebugFileName , ULONG ulDebugFileNameSize )
{
    /* Debug filename */
    const char * pcszDebugFileName = "EISKIT.LOG" ;

    /* Locate the debug directory */
    const char *pcszEnvironmentVariableName = "EIS_INET_LOCATION" ;

    char szEnvironmentVariableValue[ 100 ] = { "" } ;

    size_t stSizeRequired = 0 ;

    getenv_s( &stSizeRequired , szEnvironmentVariableValue , sizeof( szEnvironmentVariableValue ) , pcszEnvironmentVariableName );

    if ( stSizeRequired < 1 )
    {
        /* Default */
        KitStrNCpy( pszDebugFileName , "C:\\Program Files\\insurance.net\\EIS\\LOG" , ulDebugFileNameSize ) ;
    }
    else
    {
        KitStrNCpy( pszDebugFileName , szEnvironmentVariableValue , ulDebugFileNameSize ) ;
    }

    if ( pszDebugFileName[ strlen( pszDebugFileName ) - 1 ] != '\\' )
        KitStrNCat( pszDebugFileName , "\\" , ulDebugFileNameSize ) ;
    KitStrNCat( pszDebugFileName , pcszDebugFileName , ulDebugFileNameSize ) ;

} /* KitDebugFileName */

typedef bool BOOL ;
#define FALSE false ;
#define TRUE true ;
#define EIS_EXPENTRY

BOOL EIS_EXPENTRY PceStringValueIsDouble( const char *pcszValue , double *pdblValue )
{
    BOOL bStringValueIsDouble = FALSE ;

    double dblValue = 0.0 ;

    bStringValueIsDouble = (BOOL)( sscanf( pcszValue , "%lf" , &dblValue ) == 1 ) ;
    if ( ( bStringValueIsDouble ) && ( pdblValue != NULL ) )
        *pdblValue = dblValue ;

    return bStringValueIsDouble ;

} /* PceStringValueIsDouble */

void TestIsDouble( const char *pcszValue )
{
    double dblValue = 0.0 ;

    if ( ! PceStringValueIsDouble( pcszValue , &dblValue ) )
    {
        cout << "Value \"" << pcszValue << "\" is non-numeric" << endl ;
    }
    else
    {
        cout << "Value \"" << pcszValue << "\" has numeric value " << dblValue << endl ;
    }

} // TestIsDouble

int _tmain(int argc, _TCHAR* argv[])
{
    const char szNonInteger[ 50 ] = { "Z50" } ;
    const char szInteger[ 50 ] = { "100" } ;
    const char szDouble[ 50 ] = { "3.1415926535898" } ;

    double dblValue = 0.0 ;

    TestIsDouble( szNonInteger ) ;
    TestIsDouble( szInteger ) ;
    TestIsDouble( szDouble ) ;

    exit( 0 ) ;

    char chMinus = '-' ;
    char chN = 'N' ;

    cout << "Minus is " << (int)chMinus << " and N is " << (int)chN << endl ;

    double THREE_MONTH_TERM = ( 365 / 4 ) ;
    double SIX_MONTH_TERM = ( 365 / 2 ) ;

    cout << "THREE_MONTH_TERM = " << THREE_MONTH_TERM << " [" << 365.0/4 << "]" << endl ;
    cout << "SIX_MONTH_TERM = " << SIX_MONTH_TERM << " [" << 365.0/2 << "]" << endl ;

    printf( "Value is %12.6f\n" , 3.1415926535898 ) ;
    printf( "Value is %.16f\n" , 3.1415926535898 ) ;

    exit(0);

    char szDebugFileName[ 300 ] = { "" } ;
    KitDebugFileName( szDebugFileName , sizeof( szDebugFileName ) ) ;

    cout << "Debug filename is \"" << szDebugFileName << "\"" << endl ;

	return 0;
}

