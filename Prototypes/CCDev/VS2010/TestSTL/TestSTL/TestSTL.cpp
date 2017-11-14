// TestSTL.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
#include <set>

typedef std::set< int > CSimpleNumberSet ;

void TestSimpleNumberSet( void )
{
    CSimpleNumberSet SimpleNumberSet ;

    SimpleNumberSet.insert( 1 ) ;
    SimpleNumberSet.insert( 2 ) ;
    SimpleNumberSet.insert( 3 ) ;

    std::pair<CSimpleNumberSet::iterator,bool> InsertionResult
        = SimpleNumberSet.insert( 2 ) ;
    if ( ! InsertionResult.second )
    {
        // Trap duplicate/overlapping set members

        CSimpleNumberSet::iterator itDuplicate = InsertionResult.first ;

        std::cout << "Duplicate at " << (*itDuplicate) << std::endl ;

    } // Trap duplicate/overlapping set members

    SimpleNumberSet.insert( 3 ) ;
}
int _tmain(int argc, _TCHAR* argv[])
{
    TestSimpleNumberSet() ;

	return 0;
}

