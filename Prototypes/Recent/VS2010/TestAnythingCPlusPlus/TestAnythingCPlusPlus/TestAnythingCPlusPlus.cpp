// TestAnythingCPlusPlus.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <string>
using namespace std ;

int _tmain(int argc, _TCHAR* argv[])
{
    string str = "0123456789" ;

    string *pstr1 = &str ;

    str = str.erase( 4 ) ;

    string *pstr2 = &str ;

	return 0;
}

