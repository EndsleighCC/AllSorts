// This is the main DLL file.

#include "stdafx.h"

#include <string>
using namespace std;

#include <string.h>

#include "TestCLRCPlusPlus.h"

#include "..\TestNonCLRLibrary\TestNonCLRLibrary.h"

namespace OuterNamespace { namespace InnerNamespace
{
    int MultiplyByTwo( int intValue )
    {
        return 2 * intValue ;
    }
} }

namespace TestCLRCPlusPlus
{

    int TestCLRCPlusPlusClass::Unity( void )
    {
        return 1 ;
    }

    int TestCLRCPlusPlusClass::Twice( int intValue )
    {
        return OuterNamespace::InnerNamespace::MultiplyByTwo( intValue ) ;
    }

    int TestCLRCPlusPlusClass::Multiply( int value, int multiplier )
    {
        TestCLRCommonClass ^ testCLRCommonClass = gcnew TestCLRCommonClass( 1 ) ;
        return value * multiplier ;
    }

    int TestCLRCPlusPlusClass::Multiply( TestCLRCommonClass ^ testCLRCommonClass , int multiplier )
    {
        int calculatedValue = testCLRCommonClass->IntegerValue * multiplier ;
        return calculatedValue ;
    }

    System::String ^ TestCLRCPlusPlusClass::Message()
    {
        return gcnew System::String( "Hi there from C++" ) ;
    }

    TestCLRCommonOutput ^ TestCLRCPlusPlusClass::PerformFunction( TestCLRCommonInput ^ inputData )
    {
        TestCLRCommonOutput ^ outputData = gcnew TestCLRCommonOutput();

        outputData->Result = inputData->IntProperty;

        // Add some members to the Collection (List<string>)
        for ( int index = 0 ; index < inputData->IntProperty ; ++index )
        {
            System::String ^ text = gcnew System::String( %System::String("C++ RETURN ") + inputData->StringProperty + %System::String(" ") + System::Convert::ToString( index ) ) ;
            outputData->DescriptionData->Add( text ) ;
        }

        return outputData ;
    }

    void TestCLRCPlusPlusClass::PerformFunction( TestCLRCommonInput ^ inputData , [Runtime::InteropServices::Out] TestCLRCommonOutput ^ %outputData )
    {
        outputData = gcnew TestCLRCommonOutput();

        outputData->Result = inputData->IntProperty;

        // Add some members to the Collection (List<string>)
        for ( int index = 0 ; index < inputData->IntProperty ; ++index )
        {
            System::String ^ text = gcnew System::String( %System::String("C++ OUT ") + inputData->StringProperty + %System::String(" ") + System::Convert::ToString( index ) ) ;
            outputData->DescriptionData->Add( text ) ;
        }
    }

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

    void TestCLRCPlusPlusClass::PerformFlatCalculation( TestCLRCommonInput ^ inputData , [Runtime::InteropServices::Out] TestCLRCommonOutput ^ %outputData )
    {
        TEST_INPUT_DATA flatInputData = { 0 } ;
        TEST_OUTPUT_DATA flatOutputData = { 0 } ;

        flatInputData.intIntProperty = inputData->IntProperty ;
        string stringProperty = MarshalStdStringFromSystemString( inputData->StringProperty ) ;
        strncpy( flatInputData.szStringProperty , stringProperty.c_str() , sizeof( flatInputData.szStringProperty ) ) ;

        int intError = TestPerformFlatCalculation( &flatInputData , &flatOutputData ) ;
    }

} // namespace TestCLRCPlusPlus