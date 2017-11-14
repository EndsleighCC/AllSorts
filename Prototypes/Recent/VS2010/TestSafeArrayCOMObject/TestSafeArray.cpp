// TestSafeArray.cpp : Implementation of CTestSafeArray

#include "stdafx.h"
#include "TestSafeArray.h"

#include <iostream>
using namespace std ;

// CTestSafeArray

HRESULT CTestSafeArray::Test( /*[in]*/ int value )
{
    HRESULT hr = S_OK ;

    cout << "Passed in Integer was " << value << endl ;

    return hr ;
}

SAFEARRAY *CreateSinglyDimensionedSafeArrayOfLongs( void )
{
    // 1-D zero based Safe Array of long with 10 elements
    CComSafeArray<long> sal( 10 /* count */ , 0 /* lower bound (base) */ ) ;

    sal[0] = 0 ;
    sal[1] = 1 ;
    sal[2] = 2 ;
    sal[3] = 3 ;
    sal[4] = 4 ;
    sal[5] = 5 ;
    sal[6] = 6 ;
    sal[7] = 7 ;
    sal[8] = 8 ;
    sal[9] = 9 ;

    // Return the Detached Safe Array pointer that is contained inside
    // the CComSafeArray object and the Destructor will do nothing
    return sal.Detach() ;

} // CreateSinglyDimensionedSafeArrayOfLongs

SAFEARRAY *CreateDoublyDimensionedSafeArrayOfLongs( void )
{
    const long lDimension1Base = 0 ;
    const long lDimension1Count = 3 ;
    const long lDimension2Base = 0 ;
    const long lDimension2Count = 5 ;

    // 2-D array with all dimensions
    // Left zero based dimension has 3 elements
    CComSafeArrayBound bound1( lDimension1Count /* count */ , lDimension1Base /* lower bound (base) */ );
    // Right zero based dimension has 4 elements
    CComSafeArrayBound bound2( lDimension2Count /* count */ , lDimension2Base /* lower bound (base) */ );

    cout << "Creating two dimensional Safe Array of \"long\"" << endl ;
    cout << "    Dimension 1 : Count=" << bound1.GetCount() << ", Base=" << bound1.GetLowerBound() << endl ;
    cout << "    Dimension 2 : Count=" << bound2.GetCount() << ", Base=" << bound2.GetLowerBound() << endl ;

    // Equivalent C-style array indices would be [3][4]
    CComSafeArrayBound rgBounds[] = { bound1, bound2 };
    CComSafeArray<long> sal(rgBounds, sizeof(rgBounds)/sizeof(CComSafeArrayBound));

    for ( long lDimension1Index = lDimension1Base ;
          lDimension1Index < lDimension1Base + lDimension1Count ;
          ++lDimension1Index
        )
    {
        for ( long lDimension2Index = lDimension2Base ;
              lDimension2Index < lDimension2Base + lDimension2Count ;
              ++lDimension2Index
            )
        {
            long rgIndElement[] = { lDimension1Index , lDimension2Index } ;
            long lValue = lDimension1Index*10 + lDimension2Index ;
            HRESULT hrMultiDimSetAt = sal.MultiDimSetAt( &rgIndElement[0] , lValue ) ;
            if ( SUCCEEDED( hrMultiDimSetAt ) )
            {
                cout << "        SET sal[" << lDimension1Index << "][" << lDimension2Index << "] = " << lValue << endl ;
            }
            else
            {
                cout << "        Failed to set element sal[" << lDimension1Index << "][" << lDimension2Index << "]" << endl ;
            }
        }
    }

    // Return the Detached Safe Array pointer that is contained inside
    // the CComSafeArray object and the Destructor will do nothing
    return sal.Detach() ;

} // CreateDoublyDimensionedSafeArrayOfLongs

SAFEARRAY *CreateSinglyDimensionedSafeArrayOfBSTRs( void )
{
    // 1-D zero based Safe Array of long with 10 elements
    CComSafeArray<BSTR> sab( 10 /* count */ , 0 /* lower bound (base) */ ) ;

    sab[0] = SysAllocString(L"BSTR 0");
    sab[1] = SysAllocString(L"BSTR 1");
    sab[2] = SysAllocString(L"BSTR 2");
    sab[3] = SysAllocString(L"BSTR 3");
    sab[4] = SysAllocString(L"BSTR 4");
    sab[5] = SysAllocString(L"BSTR 5");
    sab[6] = SysAllocString(L"BSTR 6");
    sab[7] = SysAllocString(L"BSTR 7");
    sab[8] = SysAllocString(L"BSTR 8");
    sab[9] = SysAllocString(L"BSTR 9");

    // Return the Detached Safe Array pointer that is contained inside
    // the CComSafeArray object and the Destructor will do nothing
    return sab.Detach() ;

} // CreateSinglyDimensionedSafeArrayOfLongs

HRESULT CTestSafeArray::GetSafeArrayOfLongs( /*[out,retval]*/ SAFEARRAY **psa )
{
    HRESULT hr = S_OK ;

    // *psa = CreateSinglyDimensionedSafeArrayOfLongs() ;
    *psa = CreateDoublyDimensionedSafeArrayOfLongs() ;

    // Take an internal copy of the Safe Array that has been returned
    // before actually returning from this function
    m_sal.CopyFrom(*psa);

    return hr ;

} // CTestSafeArray::GetSafeArrayOfLongs

HRESULT CTestSafeArray::GetStoredSafeArrayOfLongs( /*[out,retval]*/ SAFEARRAY **psa )
{
    HRESULT hr = S_OK ;

    // Let CComSafeArray handle all the copying and lifetime issues
    CComSafeArray<long> sal( m_sal ) ;

    // Return the Detached Safe Array pointer that is contained inside
    // the CComSafeArray object and the Destructor will do nothing
    *psa = sal.Detach();

    return hr ;

} // GetStoredSafeArrayOfLongs

HRESULT CTestSafeArray::GetSafeArrayOfBSTRs( /*[out,retval]*/ SAFEARRAY **psa )
{
    HRESULT hr = S_OK ;

    *psa = CreateSinglyDimensionedSafeArrayOfBSTRs();

    return hr ;

} // CTestSafeArray::GetSafeArrayOfBSTRs
