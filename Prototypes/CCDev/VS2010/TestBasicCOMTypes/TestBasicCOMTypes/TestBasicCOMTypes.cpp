
#include <iostream>
using namespace std;

#include <Windows.h>

#include <atlbase.h>
#include <atlcom.h>
#include <comutil.h>

int main( const int cintArgCount , const char * apcszArgValue[] )
{
    int intError = 0 ;

    _variant_t variant ;

    _bstr_t bstrt = "Hi there" ;

    variant = bstrt ;

    wcout << L"Variant contains " << variant.vt << L" and pointer is \"" << hex << variant.bstrVal << L"\" = \"" << (_bstr_t)variant << L"\"" << endl ;

    return intError ;

} // main
