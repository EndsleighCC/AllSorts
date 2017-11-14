// TestPremCalcData.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
using namespace std ;

#define EIS_WINNT

#define INCL_DSOS
#define INCL_DRQ

#include <eis.h>

#include <iomanip>
#include <locale>
#include <sstream>

template<class T>
std::string FormatWithCommas(T value)
{
    std::stringstream ss;
    ss.imbue(std::locale(""));
    ss << std::fixed << value;
    return ss.str();
}

//ostream& operator<<( ostream& ostr , size_t stValue )
//{
//    ostr << FormatWithCommas( stValue ) ;
//}

int _tmain(int argc, _TCHAR* argv[])
{
    cout << "Size of DRQ_MOT_MULTIPLE_SCHEME_PREMCALC_IN = " << FormatWithCommas( sizeof( DRQ_MOT_MULTIPLE_SCHEME_PREMCALC_IN ) ) << " bytes" << endl ;
    cout << "Size of DRQ_MOT_MULTIPLE_SCHEME_PREMCALC_OUT = " << FormatWithCommas( sizeof( DRQ_MOT_MULTIPLE_SCHEME_PREMCALC_OUT ) ) << " bytes" << endl ;

	return 0;
}

