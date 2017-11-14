// TestDouble.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <stdio.h>

#include <iostream>
using namespace std ;

void TestRounding( double dblValue )
{
    printf( "printf rounded value of %.3f to 2 decimal places is %.2f\n" , dblValue , dblValue ) ;

    char szRoundedValue[ 200 ] = { "" } ;

    sprintf( szRoundedValue , "sprintf rounded value of %.3f to 2 decimal places is %.2f\n" , dblValue , dblValue ) ;
    printf( "%s" , szRoundedValue ) ;

}

void TestReading( void )
{
    char szValueExponentialNotation[ 50 ] = { "1.03e-5" } ;
    double dblValue = 0.0 ;

    if ( sscanf( szValueExponentialNotation , "%lg" , &dblValue ) == 1 )
    {
        cout << "Value of \"" << szValueExponentialNotation << "\" = " << dblValue << endl ;
    }
    else
    {
        cout << "Failed to convert \"" << szValueExponentialNotation << "\"" << endl ;
    }

    char szValueSimpleNotation[ 50 ] = "3.1415926535898" ;

    if ( sscanf( szValueSimpleNotation , "%lg" , &dblValue ) == 1 )
    {
        cout << "Value of \"" << szValueSimpleNotation << "\" = " << dblValue << endl ;
    }
    else
    {
        cout << "Failed to convert \"" << szValueSimpleNotation << "\"" << endl ;
    }

}

int _tmain(int argc, _TCHAR* argv[])
{

    // TestRounding( 10.125 ) ;
    // TestRounding( 10.124 ) ;

    TestReading() ;

	return 0;
}

