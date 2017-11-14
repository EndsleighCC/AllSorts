// TestOSVersionCPP.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
#include <string>
using namespace std ;

#include <windows.h>

int _tmain(int argc, _TCHAR* argv[])
{
    OSVERSIONINFO osversioninfo = { 0 } ;
    osversioninfo.dwOSVersionInfoSize = sizeof( osversioninfo ) ;
    GetVersionEx( &osversioninfo );
    char szPlatformId[ 1000 ] = { "" } ;
    switch ( osversioninfo.dwPlatformId )
    {
    case VER_PLATFORM_WIN32_NT : strncpy_s( szPlatformId , sizeof( szPlatformId ) , "Win32" , _TRUNCATE ); break ;
    default : sprintf_s( szPlatformId , sizeof( szPlatformId ) , "[%u]" , osversioninfo.dwPlatformId ); break ;
    }

    OSVERSIONINFOW osversioninfow = { 0 } ;
    osversioninfow.dwOSVersionInfoSize = sizeof( osversioninfow ) ;
    GetVersionExW( &osversioninfow ) ;

    wstring csdversion = osversioninfo.szCSDVersion ;

    wcout << L"Major Version = " << osversioninfo.dwMajorVersion
          << L", Minor Version = " << osversioninfo.dwMinorVersion
          << L", Build Number = " << osversioninfo.dwBuildNumber
          << L", Platform Id = " << osversioninfo.dwPlatformId
          << L", CSD Info = \"" << csdversion.c_str() << "\""
          << endl ;

	return 0;
}

