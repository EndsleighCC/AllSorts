// This is the main DLL file.

#include "stdafx.h"

#include <string>
using namespace std ;

#include "TestCPlusPlusCLRObject.h"

#include "TestCplusCplusCommon.h"

#include <string.h>

using namespace TestCPlusPlusCLRObject ;

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

//=========================[ MarshalNulTerminatedStringFromSystemString ]===========================
// Purpose:
//  Marshal a CLR String from the CLR Heap to a NUL terminated sring
//
// Parameters:
//
//	pszBuffer [in/out]
//		A pointer to a buffer to receive the NUL terminated string
//	stBufferSize [in]
//		The size in characters of the buffer pointed to by pszBuffer
//  strclr [in]
//      A handle to the CLR string whose contents us to be marshaled into the NUL terminated string
//
// Return value:
//      pszBuffer
//
// Exceptions:
//
//  None
//
//==================================================================================================
static char * MarshalNulTerminatedStringFromSystemString(	char * pszBuffer ,
                                                            size_t stBufferSize ,
                                                            const String ^ strclr )
{
    // Marshal to std::string
    string strStd = MarshalStdStringFromSystemString( strclr ) ;

    // Copy to the NUL terminated string buffer
    strncpy_s( pszBuffer ,
               stBufferSize ,
               strStd.c_str() ,
               _TRUNCATE ) ;

    return pszBuffer ;

} // MarshalNulTerminatedStringFromSystemString

System::IntPtr ^ ECTestCPlusPlusCLRObject::ProducePointerToCommonCplusCplusData( void )
{
    // Allocate a C++ data structure on the unmanaged C++ Heap
    CplusCplusCommonData *pcommonData = new CplusCplusCommonData ;

    // Put some data into the C++ data structure
    pcommonData->intValue = 17 ;
    strncpy( pcommonData->szValue , "Seventeen" , sizeof( pcommonData->szValue ) ) ;

    // Wrap the C++ unmanaged Heap pointer in a System::IntPtr
    return gcnew System::IntPtr( pcommonData ) ;
}

void ECTestCPlusPlusCLRObject::ConsumePointerToCommonCplusCplusData( System::IntPtr ^ intPtr )
{
    // Get the pointer to the C++ structure
    CplusCplusCommonData *pcommonData = (CplusCplusCommonData *)intPtr->ToPointer() ;

    // Because an integer is a Value Type it is possible to just use the C++ data structure member
    // i.e. System::Int32 is the same as a C++ int
    Console::WriteLine( "intValue = {0}" , pcommonData->intValue ) ;

    // To use Console::Writeline with a string, the string must be a System::String
    // System::String is a Reference Type which is not the same as a C++ NUL terminated string
    // System::String has a constructor that accepts a NUL terminated string (thank goodness)
    System::String ^ stringValue = gcnew System::String( pcommonData->szValue ) ;
    Console::WriteLine( "szValue = \"{0}\"" , stringValue ) ;
}
