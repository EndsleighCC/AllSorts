// TestCLRConsole.cpp : main project file.

#include "stdafx.h"

using namespace System;
using namespace System::ComponentModel;

using namespace TestCSharpCLRObject;

using namespace TestCPlusPlusCLRObject ;

using namespace TestCSharpCLRStaticObject ;

int main(array<System::String ^> ^args)
{
    //Console::WriteLine(L"Hello World from C++. Press <ENTER>");
    //Console::ReadLine();

    try
    {
        try
        {
            //char *pch = 0 ;
            //char ch = *pch ;
            //int intTop = 1 ;
            //int intBottom = 0 ;
            //int intValue = intTop / intBottom ;
            // MotCalculatePremium
            //System::Diagnostics::Process^ myProc = gcnew System::Diagnostics::Process;
            ////Attempting to start a non-existing executable
            //myProc->StartInfo->FileName = "c:\nonexist.exe";
            ////Start the application and assign it to the process component.
            //myProc->Start();

            char *pch = (char *)1 ;
            char ch = *pch ;

        }
        catch ( Win32Exception ^ wex )
        {
            Console::WriteLine( wex->Message );
            Console::WriteLine( wex->ErrorCode );
            Console::WriteLine( wex->NativeErrorCode );
            Console::WriteLine( wex->StackTrace );
            Console::WriteLine( wex->Source );
            Exception^ e = wex->GetBaseException();
            Console::WriteLine( wex->Message );
        }
        catch ( Exception ^ ex )
        {
            Console::WriteLine( ex->Message );
            Console::WriteLine( ex->StackTrace );
            Console::WriteLine( ex->Source );
            Exception^ e = ex->GetBaseException();
            Console::WriteLine( e->Message );
        }
    }
    catch ( ... )
    {
        Console::WriteLine( "Unexpected unhandled exception" ) ;
    }

    TestCSharpCLRClass ^ testCSharpCLRClass = gcnew TestCSharpCLRClass();
    Console::WriteLine( testCSharpCLRClass->HelloString() );
    Console::ReadLine();

    Console::WriteLine( testCSharpCLRClass->HelloStringProperty ) ;
    Console::ReadLine();

	Console::WriteLine();
	Console::WriteLine( "Static Method on non-static class returned \"{0}\"" , testCSharpCLRClass->GetStaticString() );

	Console::WriteLine();
	Console::WriteLine( "Static Method on static class returned \"{0}\"" , TestCSharpCLRStaticClass::GetStaticString() );

    ECTestCPlusPlusCLRObject ^ testCPlusPlusCLRObject = gcnew ECTestCPlusPlusCLRObject() ;

    System::IntPtr ^ intPtr = testCPlusPlusCLRObject->ProducePointerToCommonCplusCplusData() ;

    testCPlusPlusCLRObject->ConsumePointerToCommonCplusCplusData( intPtr ) ;

    return 0;
}
