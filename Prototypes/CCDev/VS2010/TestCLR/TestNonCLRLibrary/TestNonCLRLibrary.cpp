// TestNonCLRLibrary.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

#include <string>
using namespace std;

#include <stdio.h>
#include <string.h>

#include "TestNonCLRLibrary.h"

int TestPerformFlatCalculation( const TEST_INPUT_DATA *pinputData , TEST_OUTPUT_DATA *poutputData )
{
    poutputData->dblResult = pinputData->intIntProperty;
    
    // Add some data to the output array
    for ( int intIndex = 0 ; intIndex < pinputData->intIntProperty ; ++intIndex )
    {
        char szValue[ 20 ] = { "" } ;

        sprintf( szValue , "%d" , intIndex );

        string text = string( "C++ Flat" ) + pinputData->szStringProperty + " " + szValue ;
        strncpy( poutputData->aszDescriptionData[intIndex] , text.c_str() , sizeof( poutputData->aszDescriptionData[intIndex] ) ) ;
    }

    return 0 ;

} // TestPerformCalculation
