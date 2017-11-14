// TestExceptionCPP.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
using namespace std ;

#include <eis.h>

#include <ECString.hpp>
#include <ECSehException.hpp>

void TestBasicException( void )
{
    try
    {
        int *pintAnything = NULL ;

        int intValue = *pintAnything ;

        cout << "Exception did not occur" << endl ;
    }
    catch ( ... )
    {
        cout << "Exception occurred" << endl ;
    }
} // TestBasicException

void TestStructuredException( void )
{
    ECAutoSehExceptionTranslator AutoSehExceptionTranslator ;

    try
    {
        int *pintAnything = NULL ;

        cout << "About to reference an invalid pointer" << endl ;
        int intValue = *pintAnything ;

        cout << "Exception did not occur" << endl ;
    }
    catch( ECSehException &sehException )
    {
        cout << "Exception occurred : " << sehException.ExceptionDescription().CStr() << endl ;
    } // catch
} // TestStructuredException

int _tmain(int argc, _TCHAR* argv[])
{
    // TestBasicException() ;
    TestStructuredException() ;

	return 0;
}

