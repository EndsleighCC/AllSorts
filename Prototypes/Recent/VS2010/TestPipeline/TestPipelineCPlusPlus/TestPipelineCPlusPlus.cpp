// TestPipelineCPlusPlus.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
#include <string>
using namespace std ;

using namespace System;
using namespace System::Collections::ObjectModel;

using namespace System::Management::Automation ;
using namespace System::Management::Automation::Runspaces ;

ref class TestPowershell
{
public :
    TestPowershell(String ^ commandFilePathAndName)
    {
        _powershell = PowerShell::Create();
        _powershell->AddCommand( commandFilePathAndName );
    }

    void AddParameter(String ^ parameter)
    {
        _powershell->AddParameter( nullptr , parameter );
    }

    Collection<PSObject ^> ^Execute()
    {
        Console::WriteLine("Running script from Powershell Object");
        Collection<PSObject ^> ^powershellResults = _powershell->Invoke();
        System::Text::StringBuilder ^resultsMessage = gcnew System::Text::StringBuilder();
        for each (PSObject ^powershellResult in powershellResults)
        {
            resultsMessage->Append(powershellResult->ToString());
        }
        Console::WriteLine("Results are:");
        Console::WriteLine(resultsMessage);
        return powershellResults;
    }

    delegate void MyDelegate(System::Object ^ sender, DataAddedEventArgs ^ e);

    Collection<String^> ^ExecuteAsynchronously()
    {
        Console::WriteLine("Running script asynchronously from Powershell Object");

        PSDataCollection<PSObject^> ^outputCollection = gcnew PSDataCollection<PSObject^>();

        // outputCollection->DataAdded += &TestPowershell::Output_DataAdded ;
        outputCollection->DataAdded += gcnew MyDelegate( this , &TestPowershell::Output_DataAdded ) ;

        _powershell->Streams->Error->DataAdded += &TestPowershell::Error_DataAdded ;

        IAsyncResult ^result = _powershell->BeginInvoke<PSObject^, PSObject^>(nullptr, outputCollection);

        while (!result->IsCompleted)
        {
            Console::WriteLine("Waiting for asynchonous result");
            System::Threading::Thread::Sleep(2000);
        }

        Console::WriteLine("Asynchronous Execution ends");

        Collection<String^> ^outputResults = gcnew Collection<String^>();

        Console::WriteLine("Results are:");
        for each (PSObject ^outputItem in outputCollection)
        {
            String ^outputLine = outputItem->BaseObject->ToString();
                    
            outputResults->Add(outputLine);
            // Line should already contain an End-Of-Line character
            Console::Write(outputLine);
        }
        return outputResults;
    }

    void Output_DataAdded(System::Object ^ sender, DataAddedEventArgs ^ e)
    {
        PSDataCollection<PSObject ^> ^outputCollection = (PSDataCollection<PSObject^>^)(sender);
        PSObject ^psObject = outputCollection[e->Index];
        // Line should already contain an End-Of-Line character
        Console::Write("Output: {0}",psObject->BaseObject->ToString());
    }

    void Error_DataAdded(System::Object ^ sender, DataAddedEventArgs ^ e)
    {
        PSDataCollection<ErrorRecord^> ^errorCollection = (PSDataCollection<ErrorRecord^>^)sender;
        ErrorRecord ^errorRecord = errorCollection[e->Index];
        Console::WriteLine("Error: {0}", errorRecord->ToString());
    }

private:
    PowerShell ^ _powershell ;
} ; // TestPowershell

int _tmain(int argc, _TCHAR* argv[])
{
    String ^ scriptName = "testrun.ps1";
    String ^ currentDirectory = Environment::CurrentDirectory ;
    String ^ scriptFullFilename = System::IO::Path::Combine(Environment::CurrentDirectory, gcnew String( "..\\TestPipeline\\" ) , scriptName);
    scriptFullFilename = System::IO::Path::GetFullPath(scriptFullFilename );
    if (System::IO::File::Exists(scriptFullFilename))
    {
        TestPowershell ^testPowershell = gcnew TestPowershell(scriptFullFilename);
        testPowershell->AddParameter("From the powershell object");
        testPowershell->Execute();
    }
    else
    {
        Console::WriteLine("Script file \"{0}\" does not exist", scriptFullFilename);
    }

	return 0;
}

