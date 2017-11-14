// TestSafeArray.h : Declaration of the CTestSafeArray

#pragma once
#include "resource.h"       // main symbols



#include "TestSafeArrayCOMObject_i.h"

#include <atlsafe.h>

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CTestSafeArray

class ATL_NO_VTABLE CTestSafeArray :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CTestSafeArray, &CLSID_TestSafeArray>,
	public IDispatchImpl<ITestSafeArray, &IID_ITestSafeArray, &LIBID_TestSafeArrayCOMObjectLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CTestSafeArray()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_TESTSAFEARRAY)


BEGIN_COM_MAP(CTestSafeArray)
	COM_INTERFACE_ENTRY(ITestSafeArray)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:

    // ITestSafeArray
    STDMETHOD(Test)( /*[in]*/ int value ) ;

    STDMETHOD(GetSafeArrayOfLongs)( /*[out,retval]*/ SAFEARRAY **psa ) ;

    STDMETHOD(GetStoredSafeArrayOfLongs)( /*[out,retval]*/ SAFEARRAY **psa ) ;

    STDMETHOD(GetSafeArrayOfBSTRs)( /*[out,retval]*/ SAFEARRAY **psa ) ;

private:
    CComSafeArray<long> m_sal;

}; // CTestSafeArray

OBJECT_ENTRY_AUTO(__uuidof(TestSafeArray), CTestSafeArray)
