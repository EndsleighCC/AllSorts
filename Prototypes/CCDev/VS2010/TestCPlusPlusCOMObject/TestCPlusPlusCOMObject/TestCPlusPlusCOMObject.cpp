// TestCPlusPlusCOMObject.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <atlbase.h>

#include <atlcom.h>

#define EIS_WINNT

#include <INETBaseC.h>
#include <INETBaseC_i.c>
//
//#include <INETBaseT.h>
//#include <INETBaseT_i.c>

#include <ECComBSTR.hpp>

// #import "C:\ST04\INETBase\TLB\INETBaseC.tlb"

using namespace System;
using namespace System::Runtime;
using namespace System::Runtime::InteropServices ;

using namespace TestCSharpObject;

#include <iostream>
#include <iomanip>
using namespace std ;

// #pragma unmanaged

// #include <unknwn.h>

//IUnknown ^ GetFromSimplePointer( void )
//{
//    IUnknown ^ pIUnknownManaged ;
//
//    CComPtr<IUnknown> *pIUnknown ;
//
//    cout << "Calling CoCreateInstance" << endl ;
//
//    HRESULT hr = S_OK ;
//    hr = CoCreateInstance(  CLSID_Kernel ,
//                            NULL ,
//                            CLSCTX_LOCAL_SERVER ,
//                            IID_IUnknown ,
//                            reinterpret_cast< void ** >( &pIUnknown ) );
//    if ( SUCCEEDED( hr ) )
//    {
//        // I want to be able to do this:
//        // System::Object ^ pIUnknownObject = gcnew System::Object( pIUnknown ) ;
//
//        // pIUnknownManaged = pIUnknown ;
//
//        System::Object ^ unknown = System::Runtime::InteropServices::Marshal::GetObjectForIUnknown( IntPtr( pIUnknown ) ) ;
//
//        // pIUnknownManaged = (IUnknown ^)unknown ;
//
//        // INETBaseLib::IUnknown ^ pIUnknownManaged1 = gcnew INETBaseLib::IUnknown( pIUnknown ) ;
//    }
//
//    return pIUnknownManaged ;
//} // GetFromSimplePointer

// #pragma managed

//==============================[ MarshalStdStringFromSystemString ]================================
// Purpose:
//  Marshal a CLR String from the CLR Heap to a std:string
//
// Parameters:
//
//  strclr [in]
//      A handle to the CLR string to be marshaled into the std::string
//
// Return value:
//      A std:string containing the contents of the CLR String
//
// Exceptions:
//
//  None
//
//==================================================================================================

static string MarshalStdStringFromSystemString( const String ^ strclr )
{
	string str ;

	if ( strclr != nullptr )
	{
		// System String is not null

		using namespace Runtime::InteropServices;

		// Get a pointer to the CLR data contained in the CLR string
		const char* pchStringCLR = (const char*)(Marshal::StringToHGlobalAnsi(const_cast<System::String^>(strclr))).ToPointer();

		// Construct a std::string from the CLR data
		str = pchStringCLR ;

		// Release the CLR data
		Marshal::FreeHGlobal(IntPtr((void*)pchStringCLR));

	} // System String is not null

	return str ;

} // MarshalStdStringFromSystemString

void TestSchemeResultWithCComPtr( void )
{
    CComPtr<IUnknown> pIUnknownSchemeResult = NULL ;
    System::Object ^ objSchemeResult ;

    cout << "Calling CoCreateInstance on SchemeResult" << endl ;

    HRESULT hr = S_OK ;
    hr = pIUnknownSchemeResult.CoCreateInstance( CLSID_SchemeResult , NULL , CLSCTX_ALL ) ;
    if ( SUCCEEDED( hr ) )
    {
        cout << "Getting Scheme Result System.Object" << endl ;
        // Marshal into a .NET System.Object that contains a COM Object
        // objSchemeResult = System::Runtime::InteropServices::Marshal::GetObjectForIUnknown( IntPtr( pIUnknownSchemeResult ) ) ;
        objSchemeResult = Marshal::GetObjectForIUnknown( IntPtr( pIUnknownSchemeResult ) ) ;
        cout << "Got Scheme Result Object" << endl ;
    }
    else
    {
        cout << "Failed to create Scheme Result" << endl ;
        exit(1);
    }

    cout << "C++ Casting from Managed SchemeResult System.Object to ISchemeResult" << endl ;

    INETBaseLib::ISchemeResult ^ schemeResult = (INETBaseLib::ISchemeResult ^)objSchemeResult;

    cout << "C++ Initially assigning the SchemeResult::SchemeName to \"XXX\"" << endl ;
    schemeResult->SchemeName = "XXX" ;

    TestCSharpClass ^ testCSharpClass = gcnew TestCSharpClass() ;

    testCSharpClass->ReferenceSchemeResult( schemeResult , "XYZ" ) ;

    string strSchemeName = MarshalStdStringFromSystemString( schemeResult->SchemeName ) ;

    cout << "C++ Scheme Name is \"" << strSchemeName.c_str() << "\"" << endl ;

    // Now look into the original unmanaged COM Scheme Result
    CComPtr<ISchemeResult> pISchemeResult ;

    cout << "C++ querying for IID_ISchemeResult" << endl ;
    hr = pIUnknownSchemeResult->QueryInterface( &pISchemeResult ) ;
    if ( SUCCEEDED( hr ) )
    {

        if ( pISchemeResult == NULL )
            cout << "Scheme Result pointer is NULL" << endl ;
        else
        {
            ECComBSTR bstreSchemeName ;

            cout << "C++ getting SchemeName Property" << endl ;

            hr = pISchemeResult->get_SchemeName( &bstreSchemeName ) ;

            wcout << L"C++ COM Scheme Name is \"" << (BSTR)bstreSchemeName << L"\"" << endl ;
        }
    }

}
//
//void TestSchemeResult( void )
//{
//    // CComPtr<IUnknown> pIUnknown = NULL ;
//    IUnknown *pIUnknown = NULL ;
//    System::Object ^ objSchemeResult ;
//
//    cout << "Calling CoCreateInstance on SchemeResult" << endl ;
//
//    HRESULT hr = S_OK ;
//    hr = CoCreateInstance(  CLSID_SchemeResult ,
//                            NULL ,
//                            CLSCTX_ALL ,
//                            IID_IUnknown ,
//                            reinterpret_cast< void ** >( &pIUnknown ) );
//    // hr = pIUnknown->CoCreateInstance( CLSID_SchemeResult , NULL , CLSCTX_ALL ) ;
//    if ( SUCCEEDED( hr ) )
//    {
//        cout << "Getting Scheme Result System.Object" << endl ;
//        // Marshal into a .NET System.Object that contains a COM Object
//        // objSchemeResult = System::Runtime::InteropServices::Marshal::GetObjectForIUnknown( IntPtr( pIUnknown ) ) ;
//        objSchemeResult = Marshal::GetObjectForIUnknown( IntPtr( pIUnknown ) ) ;
//        cout << "Got Scheme Result Object" << endl ;
//    }
//    else
//    {
//        cout << "Failed to create Scheme Result" << endl ;
//        exit(1);
//    }
//
//    cout << "Casting from SchemeResult System.Object to ISchemeResult" << endl ;
//
//    INETBaseLib::ISchemeResult ^ schemeResult = (INETBaseLib::ISchemeResult ^)objSchemeResult;
//
//    cout << "Assigning the SchemeResult::SchemeName" << endl ;
//    schemeResult->SchemeName = "XYZ" ;
//
//    string strSchemeName = MarshalStdStringFromSystemString( schemeResult->SchemeName ) ;
//
//    cout << "C++ Scheme Name is \"" << strSchemeName.c_str() << "\"" << endl ;
//
//    // CComPtr<ISchemeResult> pISchemeResult ;
//    ISchemeResult *pISchemeResult = NULL ;
//
//    cout << "C++ querying for IID_ISchemeResult" << endl ;
//    hr = pIUnknown->QueryInterface( IID_ISchemeResult , (void**)&pISchemeResult ) ;
//    if ( SUCCEEDED( hr ) )
//    {
//
//        if ( pISchemeResult == NULL )
//            cout << "Scheme Result pointer is NULL" << endl ;
//        else
//        {
//            ECComBSTR bstreSchemeName ;
//
//            cout << "C++ getting SchemeName Property" << endl ;
//
//            hr = pISchemeResult->get_SchemeName( &bstreSchemeName ) ;
//
//            wcout << L"COM Scheme Name is \"" << (BSTR)bstreSchemeName << L"\"" << endl ;
//        }
//    }
//
//}

int _tmain(int argc, _TCHAR* argv[])
{
    HRESULT hr = S_OK ;
    cout << "Begin" << endl ;
    // hr = CoInitialize(NULL);
    if ( FAILED( hr ) )
    {
        cout << "CoInitialize failed" << endl ;
    }
    else
    {
        // CComPtr<INETBaseLib::IDiagnostics> pIDiagnostics ;

        // Type ^ typeKernel = INETBaseLib::KernelClass::typeid;
        Type ^ typeIDiagnostics = INETBaseLib::IDiagnostics::typeid ;

        // cout << "Calling Type::GetTypeFromProgID" << endl ;

        // Type ^ typeKernel = Type::GetTypeFromProgID("ECOMKernel.Kernel");

        // cout << "Calling Activator::CreateInstance" << endl ;

        // System::Object ^ objKernel = Activator::CreateInstance(typeKernel);

        CComPtr<IUnknown> *pIUnknown ;
        System::Object ^ objKernel ;

        cout << "Calling CoCreateInstance" << endl ;

        HRESULT hr = S_OK ;
        hr = CoCreateInstance(  CLSID_Kernel ,
                                NULL ,
                                CLSCTX_LOCAL_SERVER ,
                                IID_IUnknown ,
                                reinterpret_cast< void ** >( &pIUnknown ) );
        if ( SUCCEEDED( hr ) )
        {
            cout << "Getting Kernel Object" << endl ;
            // Marshal into a .NET System.Object that contains a COM Object
            // objKernel = System::Runtime::InteropServices::Marshal::GetObjectForIUnknown( IntPtr( pIUnknown ) ) ;
            objKernel = Marshal::GetObjectForIUnknown( IntPtr( pIUnknown ) ) ;
            cout << "Got Kernel Object" << endl ;
        }
        else
        {
            cout << "Failed to create Kernel" << endl ;
            exit(1);
        }

        cout << "Casting from Kernel to IDiagnostics" << endl ;

        INETBaseLib::IDiagnostics ^ diagnostics = (INETBaseLib::IDiagnostics ^)objKernel;

        diagnostics->Trace( %System::String( "Hi there from C++ only" ) ) ;

        // INETBaseLib::IDiagnostics ^ diagnosticsFromCOM = GetFromSimplePointer() ;

        TestCSharpClass ^ testCSharpClass = gcnew TestCSharpClass() ;

        testCSharpClass->Output( %System::String("Calling OutputDiagnostics" ) ) ;
        // Use the IDiagnostics interface by passing it through as a parameter
        // which is just what will be required for our Motor Quote
        testCSharpClass->OutputDiagnostics( diagnostics , %System::String("Hello from C++ through COM and .NET" ) ) ;

        // TestSchemeResult() ;
        TestSchemeResultWithCComPtr() ;

        //hr = CoCreateInstance(  CLSID_Kernel ,
        //                        NULL ,
        //                        CLSCTX_LOCAL_SERVER ,
        //                        IID_IDiagnostics ,
        //                        reinterpret_cast< void ** >( &pIDiagnostics ) );
        //if ( SUCCEEDED( hr ) )
        //{
        //    ECComBSTR bstre = "Hi there" ;

        //    pIDiagnostics->Trace( (BSTR)bstre ) ;
        //}
    
    }

    // CoUninitialize();

    cout << "End" << endl ;

	return 0;
}

