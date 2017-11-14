// eisCrLf.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
using namespace std ;

int _tmain(int argc, _TCHAR* argv[])
{
    int intError = 0 ;

    if ( argc < 2 )
    {
        cout << "eisCrLf filename { ... }" << endl ;
    }
    else
    {
        // Sufficient arguments

        for ( int intArgId = 1 ; intArgId < argc ; ++intArgId )
        {
            const wchar_t *pcszFileName = argv[intArgId] ;
            FILE * fileInput = NULL ;
            int intOpenError = _wfopen_s( &fileInput , pcszFileName , L"r" ) ;
            {
                // Input file open

                if ( intOpenError != 0 )
                {
                    wcout << L"Unable to open \"" << pcszFileName << L"\"" << endl ;
                }
                else
                {

                }

            } // Input file open

        } // for

    } // Sufficient arguments

	return intError ;

} // main
