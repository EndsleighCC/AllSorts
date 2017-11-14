// TestStaticCpp.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
using namespace std ;

class ECOuterWrapperClass
{
public:
	ECOuterWrapperClass()
	{
		DoOuterSomething(); 
	}
private:
	void DoOuterSomething()
	{
		cout << "Done Outer Something" << endl ;
	}
} ;

static ECOuterWrapperClass g_OuterWrapperClass ;

class ECTestStaticConstruction
{
public:
	ECTestStaticConstruction()
	{
	}

private :
	class ECWrapperClass
	{
	public:
		ECWrapperClass()
		{
			DoSomething();
		}

	private:
		void DoSomething()
		{
			cout << "Something was done" << endl ;
		}
	} ;

	static ECWrapperClass m_wrapperClass ;
} ;

ECTestStaticConstruction::ECWrapperClass ECTestStaticConstruction::m_wrapperClass;

int _tmain(int argc, _TCHAR* argv[])
{
	ECTestStaticConstruction testStaticConstruction0 ;

	ECTestStaticConstruction *ptestStaticConstruction1 = new ECTestStaticConstruction() ;
	ECTestStaticConstruction *ptestStaticConstruction2 = new ECTestStaticConstruction() ;

	return 0;
}

