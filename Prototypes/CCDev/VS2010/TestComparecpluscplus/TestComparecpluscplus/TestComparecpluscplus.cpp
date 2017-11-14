// TestComparecpluscplus.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>

using namespace std ;

#include <stdlib.h>
#include <string.h>

int _tmain(int argc, _TCHAR* argv[])
{
    // const char *pcszDate1 = "01.11.2015" ;
    // const char *pcszDate2 = "31.10.2015" ;

    const char *pcszDate1 = "2015.11.01" ;
    const char *pcszDate2 = "2015.10.31" ;

    int intCompare = _stricmp( pcszDate1 , pcszDate2 ) ;

    if ( intCompare == 0 )
    {
        cout << pcszDate1 << " is the same as " << pcszDate2 << endl ;
    }
    else if ( intCompare < 0 )
    {
        cout << pcszDate1 << " is less than " << pcszDate2 << endl ;
    }
    else
    {
        cout << pcszDate1 << " is greater than " << pcszDate2 << endl ;
    }

	return 0;
}

