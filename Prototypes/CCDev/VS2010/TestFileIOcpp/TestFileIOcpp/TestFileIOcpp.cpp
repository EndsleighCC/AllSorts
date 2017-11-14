// TestFileIOcpp.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
#include <iomanip>
#include <fstream>
#include <string>
using namespace std ;

#include <Windows.h>

void DisplayFileContents( string & strContext , string & strFilename )
{
    ifstream inFile ;

    inFile.open( strFilename.c_str() ) ;
    if ( ! inFile )
    {
        cout << strContext << " : Unable to open \"" << strFilename << "\" for input. Trying \"type\"" << endl ;
        system( ( string( "type " ) + strFilename ).c_str() ) ;
    }
    else
    {
        // NMAKE temporary Error Output file is open

        string strErrorLine ;
        while ( ! getline( inFile , strErrorLine ).eof() )
        {
            cout << strContext << " : " << strErrorLine << endl ;
        } // while

    } // NMAKE temporary Error Output file is open

} // DisplayFileContents

void TestGetLongPathName( void )
{
    #define MAX_LENGTH 1024

    WCHAR szModuleName[ MAX_LENGTH ] = { L"" } ;

    const WCHAR * pcszModuleName = L"C:\\PROGRA~1\\INSURA~1.NET\\INETBase\\BIN\\ECOMTR~1.EXE" ;

    // Convert to the long filename in order to load the library
    GetLongPathName( pcszModuleName , szModuleName , sizeof( szModuleName )/sizeof( szModuleName[0] ) ) ;

    wcsncpy( szModuleName , pcszModuleName , sizeof( szModuleName )/sizeof( szModuleName[0] ) ) ;

    GetLongPathName( szModuleName , szModuleName , sizeof( szModuleName )/sizeof( szModuleName[0] ) ) ;
}

static BOOL DirectoryExists(const char *pcszPath)
{
  DWORD dwAttrib = GetFileAttributesA( pcszPath ) ;

  return ( (dwAttrib != INVALID_FILE_ATTRIBUTES ) && (dwAttrib & FILE_ATTRIBUTE_DIRECTORY) ) ;

} // DirectoryExists

void TestDirectoryExists( const char * pcszPath )
{
	if ( DirectoryExists( pcszPath ) )
		cout << "Directory \"" << pcszPath << "\" exists" << endl ;
	else
		cout << "Directory \"" << pcszPath << "\" does not exist" << endl ;
}

#include <io.h>

void SearchDirectory( const string strFiles )
{
	string strDirectoryName = strFiles ;

	string::size_type stLastSlashIndex = strDirectoryName.find_last_of( '\\' ) ;
	if ( stLastSlashIndex != string::npos )
		// Delete the file specification to reveal the directory name with a trailing slash
		strDirectoryName.erase( stLastSlashIndex + 1 ) ;

	int intError = NO_ERROR ;
	LONG lHandle = 0 ;

	struct _finddata_t FindData = { 0 } ;

	lHandle = _findfirst( strFiles.c_str() , &FindData );
	if ( lHandle != -1 )
	{
		// Opened the directory search

		while ( intError == NO_ERROR )
		{
			if ( ! ( FindData.attrib & _A_SUBDIR ) )
			{
				// Process a file

				cout << "    \"" << strDirectoryName << FindData.name << "\"" << endl ;

			} // Process a file

			intError = _findnext( lHandle , &FindData ) ;

		} // while

		_findclose( lHandle ) ;

	} // Opened the directory search
	
} // SearchDirectory

bool GetFileInformation( const string & filename , _finddata_t *pFindData )
{
	bool bSuccess = false ;

	if ( pFindData != NULL )
		memset( pFindData , 0 , sizeof( *pFindData ) ) ;

	struct _finddata_t FindData = { 0 } ;

	int intError = NO_ERROR ;
	LONG lHandle = 0 ;

	lHandle = _findfirst( filename.c_str() , &FindData );
	if ( lHandle != -1 )
	{
		// Got the file information

		bSuccess = true ;
		if ( pFindData != NULL )
			memcpy( pFindData , &FindData , sizeof( *pFindData ) ) ;

		_findclose( lHandle ) ;

	} // Got the file information

	return bSuccess ;

} // FileInformation

bool FileExists( const string & filename )
{
	bool bExists = false ;

	bExists = GetFileInformation( filename.c_str() , NULL ) ;

	return bExists ;

} // FileExists

const char *g_reg =
"HKCR\r\n"
"{\r\n"
"	NoRemove AppID\r\n"
"	{\r\n"
"		{A0C24656-1A37-11D2-BC8C-4000044EBB78} = s 'ECOMKernel'\r\n"
"		'ECOMKernel.EXE'\r\n"
"		{\r\n"
"			val AppID = s {A0C24656-1A37-11D2-BC8C-4000044EBB78}\r\n"
"		}\r\n"
"	}\r\n"
"}\r\n"
"HKLM\r\n"
"{\r\n"
"   NoRemove 'System'\r\n"
"   {\r\n"
"      NoRemove 'CurrentControlSet'\r\n"
"      {\r\n"
"         NoRemove 'Services'\r\n"
"		 {\r\n"
"			NoRemove 'EventLog'\r\n"
"			{\r\n"
"				NoRemove 'Application'\r\n"
"				{\r\n"
"					ForceRemove 'ECOMKernel.Kernel'\r\n"
"					{\r\n"
"						val EventMessageFile = s 'C:\\ST04\\INETBase\\Code\\ECOMKernel\\Debug\\ECOMKernel.exe'\r\n"
"					}\r\n"
"				}\r\n"
"			}\r\n"
"		 }\r\n"
"      }\r\n"
"   }	\r\n"
"}\r\n"
"\r\n" ;

#define EIS_WINNT
#define KER_LOG_BUFFER_LENGTH 500
#define CHAR_NUL '\0'

typedef void *PF_HANDLE ;
typedef int PCE_PLOG_DATA ;

void PceDebugSchemePrintf( PF_HANDLE hpf ,
                           PCE_PLOG_DATA ppldLogData ,
                           const char *pcszFormat ,
                           ... )
{
    CHAR szDebugText[ KER_LOG_BUFFER_LENGTH ] = { "" };

    va_list pvaArgList ;

    va_start( pvaArgList , pcszFormat ) ;

    #if defined( EIS_WINNT )
    _vsnprintf( szDebugText , sizeof( szDebugText ) ,
    #else
    vsnprintf( szDebugText , sizeof( szDebugText ) ,
    #endif
        pcszFormat , pvaArgList ) ;

    /* Ensure it's NUL terminated */
    szDebugText[ sizeof( szDebugText ) - 1 ] = CHAR_NUL ;

    va_end( pvaArgList );

    printf( "%s\n" , szDebugText ) ;

} /* PceDebugSchemePrintf */

void TestPrintf( void )
{
    double dblCommissionAmountBefore = 10.0 ;
    double dblCIPBefore = 100.0 ;
    double dblCompositeCommissionRatePercentBefore = 10.0 ;

    double dblCommissionAmount = 8 ;
    double dblCompositeCalculatedInsurancePremium = 98.0 ;
    double dblCompositeCommissionRatePercent = 8.0 ;

    PceDebugSchemePrintf( NULL , 0 ,
        "Commission details change from £%4.2f/£%4.2f=%4.2f%% to £%4.2f/£%4.2f=%4.2f%%" ,
                                        dblCommissionAmountBefore ,
                                        dblCIPBefore ,
                                        dblCompositeCommissionRatePercentBefore ,
                                        dblCommissionAmount ,
                                        dblCompositeCalculatedInsurancePremium ,
                                        dblCompositeCommissionRatePercent ) ;

} // TestPrintf

int _tmain(int argc, _TCHAR* argv[])
{

    TestPrintf() ;

    exit( 0 ) ;

    //if ( argc > 1 )
    //{
    //    wstring wstrInput = argv[1] ;
    //    string strInput( wstrInput.begin() , wstrInput.end() ) ;
    //    DisplayFileContents( string( "main" ) , strInput ) ;
    //}

    //TestGetLongPathName();

	// TestDirectoryExists( "C:\\Program Files\\insurance.net" ) ;
	// TestDirectoryExists( "C:\\Program Files\\noninsurance.net" ) ;

	SearchDirectory( "o:\\Solutions\\Build\\Bin\\Release\\*" ) ;

	if ( FileExists( "o:\\Solutions\\Build\\Bin\\Release\\Endsleigh.Agent.Web.WebDeployment.msi" ) )
	{
		_finddata_t fileinfo1 = { 0 } ;

		if ( GetFileInformation( "o:\\Solutions\\Build\\Bin\\Release\\Endsleigh.Agent.Web.WebDeployment.msi" , &fileinfo1 ) )
		{
			_finddata_t fileinfo2 = { 0 } ;
			if ( GetFileInformation( "o:\\Package\\164.2\\2013-06-26-00.07\\release\\Endsleigh.Agent.Web.WebDeployment.msi" , &fileinfo2 ) )
			{
				if ( fileinfo1.time_access > fileinfo2.time_access )
				{
					cout << "Source is later than Package" << endl ;
				}
				else
				{
					cout << "Package is later than or equal to source" << endl ;
				}
			}
		}
	}

	return 0;
}

