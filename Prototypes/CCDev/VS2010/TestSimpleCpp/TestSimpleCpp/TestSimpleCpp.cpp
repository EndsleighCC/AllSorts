// TestSimpleCpp.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
using namespace std ;

#include <limits.h>

int _tmain(int argc, _TCHAR* argv[])
{
    int i = 5 ;

    int j = 0 ;

    // j = ++i++ ; // Does not compile
    j = (++i)++ ;

    cout << "i = " << i << " and j = " << j << endl ;

    cout << "INT_MAX = " << INT_MAX << endl ;

    int intValue = INT_MAX ; 
    int intOtherValue = intValue + 1 ;

    cout << "Value is " << intValue << " + 1 = " << intOtherValue << endl ;

	return 0;
}

