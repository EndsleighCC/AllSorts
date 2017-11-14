// TestHarness.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <atlbase.h>
#include <atlsafe.h>

#include <iostream>
using namespace std ;

// Declare a CComModule that is never used to allow #include of atlcom.h
// This allows use of COM Security which is somewhat easier to use than Win32 Security
CComModule _Module ;

#include "..\TestSafeArrayCOMObject_i.h"
#include "..\TestSafeArrayCOMObject_i.c"

void DisplayOneDimensionalSafeArrayOfLongs( CComSafeArray<long> & sal )
{
    cout << "Singly Dimensioned Safe Array of \"long\" of dimensions "
            << sal.GetDimensions()
            << " has "
            << sal.GetCount()
            << " elements with lower bound "
            << sal.GetLowerBound()
            << " and upper bound "
            << sal.GetUpperBound()
            << endl ;

    for ( int intIndex = sal.GetLowerBound() ;
            intIndex <= sal.GetUpperBound() ;
            ++intIndex
        )
    {
        cout << "sal[" << intIndex << "] = " << sal[intIndex] << endl ;
    }

} // DisplayOneDimensionalSafeArrayOfLongs

void DisplayTwoDimensionalSafeArrayOfLongs( CComSafeArray<long> & sal )
{
    long lDimension1Base = sal.GetLowerBound(0); ;
    long lDimension1Count = sal.GetCount(0) ;
    long lDimension2Base = sal.GetLowerBound(1) ;
    long lDimension2Count = sal.GetCount(1) ;

    cout << "Reading two dimensional Safe Array of \"long\"" << endl ;
    cout << "    Dimension 1 : Count=" << lDimension1Count << ", Base=" << lDimension1Base << endl ;
    cout << "    Dimension 2 : Count=" << lDimension2Count << ", Base=" << lDimension2Base << endl ;

    for ( long lDimension1Index = lDimension1Base ;
          lDimension1Index < ( lDimension1Base + lDimension1Count ) ;
          ++lDimension1Index
        )
    {
        for ( long lDimension2Index = lDimension2Base ;
              lDimension2Index < ( lDimension2Base + lDimension2Count ) ;
              ++lDimension2Index
            )
        {
            long rgIndElement[] = { lDimension1Index , lDimension2Index } ;
            long lValue = 0 ;
            HRESULT hrMultiDimGetAt = sal.MultiDimGetAt( rgIndElement , lValue ) ;
            if ( SUCCEEDED( hrMultiDimGetAt ) )
            {
                cout << "        GET sal[" << lDimension1Index << "][" << lDimension2Index << "] = " << lValue << endl ;
            }
            else
            {
                cout << "        Failed to get element sal[" << lDimension1Index << "][" << lDimension2Index << "]" << endl ;
            }
        }
    }

} // DisplayTwoDimensionalSafeArrayOfLongs

void DisplaySafeArrayOfLongs( CComSafeArray<long> & sal )
{
    switch ( sal.GetDimensions() )
    {
    case 1 :
        DisplayOneDimensionalSafeArrayOfLongs( sal ) ;
        break ;
    case 2 :
        DisplayTwoDimensionalSafeArrayOfLongs( sal ) ;
        break ;
    default :
        cout << "Safe Array of \"long\" of dimensions "
                << sal.GetDimensions()
                << " cannot be processed"
                << endl ;
    } // switch

} // DisplaySafeArrayOfLongs

void DisplayOneDimensionalSafeArrayOfBSTRs( CComSafeArray<BSTR> & sab )
{
    cout << "Singly Dimensioned Safe Array of \"BSTR\" of dimensions "
            << sab.GetDimensions()
            << " has "
            << sab.GetCount()
            << " elements with lower bound "
            << sab.GetLowerBound()
            << " and upper bound "
            << sab.GetUpperBound()
            << endl ;

    for ( int intIndex = sab.GetLowerBound() ;
            intIndex <= sab.GetUpperBound() ;
            ++intIndex
        )
    {
        wcout << L"    sab[" << intIndex << L"] = " << (WCHAR *)(sab[intIndex]) << endl ;
    }

} // DisplayOneDimensionalSafeArrayOfBSTRs

void DisplayTwoDimensionalSafeArrayOfBSTRs( CComSafeArray<BSTR> & sab )
{
} // DisplayTwoDimensionalSafeArrayOfBSTRs

void DisplaySafeArrayOfBSTRs( CComSafeArray<BSTR> & sab )
{
    switch ( sab.GetDimensions() )
    {
    case 1 :
        DisplayOneDimensionalSafeArrayOfBSTRs( sab ) ;
        break ;
    case 2 :
        DisplayTwoDimensionalSafeArrayOfBSTRs( sab ) ;
        break ;
    default :
        cout << "Safe Array of \"BSTR\" of dimensions "
                << sab.GetDimensions()
                << " cannot be processed"
                << endl ;
    } // switch

} // DisplaySafeArrayOfBSTRs

int _tmain(int argc, _TCHAR* argv[])
{
    HRESULT hr = S_OK ;

    hr = CoInitialize( NULL ) ;
    if ( SUCCEEDED( hr ) )
    {
        // CoInitialize called successfully

        {
            // Ensure that any COM object destructors are called before CoUninitialize is called

            CComPtr<ITestSafeArray> pITestSafeArray ;

            hr = CoCreateInstance( CLSID_TestSafeArray ,
                                   NULL ,
                                   CLSCTX_ALL ,
                                   IID_ITestSafeArray ,
                                   (void **)&pITestSafeArray ) ;
            if ( SUCCEEDED( hr ) )
            {
                // Got Safe Array COM Object

                cout << "Successfully instantiated Safe Array COM Object" << endl ;

                // hr = pITestSafeArray->Test( 17 ) ;

                SAFEARRAY *psa = NULL ;
                hr = pITestSafeArray->GetSafeArrayOfLongs( &psa ) ;
                if ( SUCCEEDED( hr ) )
                {
                    // Got Safe Array of long from COM Object

                    // *Copy* into a CComSafeArray object
                    CComSafeArray<long> sal( psa ) ;

                    cout << endl ;
                    cout << "Displaying COM Object new Safe Array of \"long\"" << endl ;
                    DisplaySafeArrayOfLongs( sal ) ;

                    // Explicitly destroy the primitive Safe Array
                    // and the Destructor will destroy the CComSafeArray
                    SafeArrayDestroy( psa ) ;
                    psa = NULL ;

                } // Got Safe Array of long from COM Object

                hr = pITestSafeArray->GetStoredSafeArrayOfLongs( &psa ) ;
                if ( SUCCEEDED( hr ) )
                {
                    // Got Safe Array of long from COM Object

                    // *Copy* into a CComSafeArray object
                    CComSafeArray<long> sal( psa ) ;

                    cout << endl ;
                    cout << "Displaying COM Object Copy of original Safe Array of \"long\"" << endl ;
                    DisplaySafeArrayOfLongs( sal ) ;

                    // Explicitly destroy the primitive Safe Array
                    // and the Destructor will destroy the CComSafeArray
                    SafeArrayDestroy( psa ) ;
                    psa = NULL ;

                } // Got Safe Array of long from COM Object

                hr = pITestSafeArray->GetSafeArrayOfBSTRs( &psa ) ;
                if ( SUCCEEDED( hr ) )
                {
                    // Got Safe Array of long from COM Object

                    // *Copy* into a CComSafeArray object
                    CComSafeArray<BSTR> sab( psa ) ;

                    cout << endl ;
                    cout << "Displaying COM Object new Safe Array of \"BSTR\"" << endl ;
                    DisplaySafeArrayOfBSTRs( sab ) ;

                    // Explicitly destroy the primitive Safe Array
                    // and the Destructor will destroy the CComSafeArray
                    SafeArrayDestroy( psa ) ;
                    psa = NULL ;

                } // Got Safe Array of long from COM Object

            } // Got Safe Array COM Object

        } // Ensure that any COM object destructors are called before CoUninitialize is called

        CoUninitialize() ;

    } // CoInitialize called successfully

	return 0;

} // main
