/*************************************************************************/
/*                                                                       */
/*            COPYRIGHT ENDSLEIGH INSURANCE SERVICES LTD 2013            */
/*                                                                       */
/*                          NON-DELIVERABLE                              */
/*                                                                       */
/*************************************************************************/
/*                                                                       */
/*  PROJECT     : SDA                                                    */
/*                                                                       */
/*  LANGUAGE    : C++                                                    */
/*                                                                       */
/*  FILE NAME   : eisEndProcess.cpp                                      */
/*                                                                       */
/*  ENVIRONMENT : Microsoft Visual C++                                   */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  FILE FUNCTION   : End any process with the specified path element    */
/*                    in its executable path                             */
/*                                                                       */
/*  EXECUTABLE TYPE : EXE                                                */
/*                                                                       */
/*  SPECIFICATION   : None                                               */
/*                                                                       */
/*  RELATED DOCUMENTATION : None                                         */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  ABSTRACT : End any process with the specified path element in its    */
/*             executable path                                           */
/*                                                                       */
/*  AUTHOR   : Chris Cornelius      CREATION DATE: 10-Oct-2013           */
/*                                                                       */
/*-----------------------------------------------------------------------*/
/*                                                                       */
/*  BUILD INFORMATION : Microsoft Visual Studio                          */
/*                                                                       */
/*  EXECUTABLE NAME   : eisEndProcess.exe                                */
/*                                                                       */
/*  ENTRY POINTS      : main                                             */
/*                                                                       */
/*************************************************************************/
/*                                                                       */
/* PVCS SECTION :                                                        */
/* ~~~~~~~~~~~~~~
   PVCS FILENAME: $Logfile:   $
   PVCS REVISION: $Revision:  $

   $Log$

/*************************************************************************/

#include "stdafx.h"

#include <windows.h>

#include <psapi.h>

#include <string>
#include <iostream>
#include <iomanip>
#include <set>
using namespace std ;

#include <process.h>

bool g_bDebugOutput = false ;

const char * SystemErrorMessage( LPSTR pszMessage ,
                                 ULONG culMessageSize ,
                                 HRESULT hr )
{
    void *pMsgBuffer = NULL ;

    ::FormatMessage( 
        FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM ,
        NULL ,
        hr ,
        MAKELANGID( LANG_NEUTRAL , SUBLANG_DEFAULT ) ,
        (LPTSTR) &pMsgBuffer ,
        0 ,
        NULL );

    strncpy_s( pszMessage , culMessageSize , static_cast<const char *>( pMsgBuffer ) , _TRUNCATE );
    pszMessage[ culMessageSize-1 ] = '\0' ;

    char * pch = &pszMessage[ strlen( pszMessage )-1 ] ;
    while (  ( pch > pszMessage ) && ( ( *pch == '\n' ) || ( *pch == '\r' ) ) )
    {
        *pch-- = '\0' ;
    }

    // Free the buffer
    LocalFree( pMsgBuffer );

    return pszMessage ;

} // SystemErrorMessage

BOOL SetPrivilege( HANDLE hToken ,
                   LPCTSTR lpszPrivilege ,
                   BOOL bEnablePrivilege ) 
{
    TOKEN_PRIVILEGES tp = { 0 } ;
    LUID luid = { 0 };

    if ( ! LookupPrivilegeValue( NULL ,           // lookup privilege on local system
                                 lpszPrivilege,   // privilege to lookup 
                                 &luid ) )        // receives LUID of privilege
    {
        DWORD dwError = GetLastError();
        char szErrorMessage[ 200 ] ;
        cout << "LookupPrivilegeValue returned error " << dwError << " = \""
             << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
             << "\"" << endl ;
        return FALSE; 
    }

    tp.PrivilegeCount = 1;
    tp.Privileges[0].Luid = luid;
    if (bEnablePrivilege)
        tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
    else
        tp.Privileges[0].Attributes = 0;

    if ( ! AdjustTokenPrivileges( hToken ,
                                  FALSE, 
                                  &tp, 
                                  sizeof(TOKEN_PRIVILEGES), 
                                  (PTOKEN_PRIVILEGES) NULL, 
                                  (PDWORD) NULL)
       )
    { 
        DWORD dwError = GetLastError();
        char szErrorMessage[ 200 ] ;
        cout << "AdjustTokenPrivileges returned error " << dwError << " = \""
             << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
             << "\"" << endl ;
        return FALSE; 
    } 

    if (GetLastError() == ERROR_NOT_ALL_ASSIGNED)

    {
          printf("The token does not have the specified privilege. \n");
          return FALSE;
    } 

    return TRUE;
}

void ShowThreadPrivilegeCollection( TOKEN_PRIVILEGES *pTokenPrivilegesInfo )
{
    char    szErrorMessage[ 200 ] ;
    CHAR    strName[ 200 ] ;
    DWORD   dwNameSize = sizeof( strName ) ;
    CHAR    strDisplayName[ 200 ] ;
    DWORD   dwDisplayNameSize = sizeof( strDisplayName );
    DWORD   dwLangId = 0 ;

    wcout << L"Thread has " << pTokenPrivilegesInfo->PrivilegeCount << L" privileges" << endl ;

    for( int uintPrivId = 0 ;
         uintPrivId < pTokenPrivilegesInfo->PrivilegeCount ;
         ++uintPrivId
       )
    {
        BOOL bSuccess = FALSE ;

        dwNameSize = sizeof( strName ) ;
        dwDisplayNameSize = sizeof( strDisplayName ) ;
        bSuccess = LookupPrivilegeName(
                        NULL ,
                        &pTokenPrivilegesInfo->Privileges[uintPrivId].Luid ,
                        strName ,
                        &dwNameSize ) ;
        if ( bSuccess )
            bSuccess = LookupPrivilegeDisplayName(
                                NULL ,
                                strName ,
                                strDisplayName ,
                                &dwDisplayNameSize ,
                                &dwLangId );
        if ( ! bSuccess )
        {
            DWORD dwError = GetLastError();
            SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError );
            cout << "LookupPrivilegeDisplayName returned error " << dwError << " = \""
                 << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
                 << "\"" << endl ;
        }
        else
        {
            wcout << L" " << strDisplayName << L" = " << strName << L" = " ;
            if ( pTokenPrivilegesInfo->Privileges[uintPrivId].Attributes & SE_PRIVILEGE_ENABLED_BY_DEFAULT )
                wcout << L" (EnByDef)" ;
            else
                wcout << L" (not EnByDef)" ;
            if ( pTokenPrivilegesInfo->Privileges[uintPrivId].Attributes & SE_PRIVILEGE_ENABLED )
                wcout << L" (Enabled)" ;
            else
                wcout << L" (not Enabled)" ;
            if ( pTokenPrivilegesInfo->Privileges[uintPrivId].Attributes & SE_PRIVILEGE_USED_FOR_ACCESS )
                wcout << L" (Used)" ;
            else
                wcout << L" (not Used)" ;

            wcout << endl ;

        }

    } // for

} // ShowThreadPrivilegeCollection

void ShowAllThreadPrivileges( void )
{
    char    szErrorMessage[ 200 ] ;
    BOOL    bSuccess = FALSE ;
    HANDLE  hToken = INVALID_HANDLE_VALUE ;

    if ( OpenThreadToken( GetCurrentThread(),
                          TOKEN_QUERY,
                          TRUE,
                          &hToken )
       )
    {
        // Got Thread Token
        TOKEN_PRIVILEGES *pTokenPrivilegesInfo = NULL ;
        DWORD dwLengthNeeded = 0;

        // First see how much buffer space we need
        if (     ( GetTokenInformation( hToken,
                                        TokenPrivileges,
                                        NULL,
                                        0,
                                        &dwLengthNeeded ) == 0
                 )
              && ( GetLastError() == ERROR_INSUFFICIENT_BUFFER )
           )
        {
            // Allocate a large enough buffer for the token info

            DWORD dwSize = 0 ;

            if ( ( pTokenPrivilegesInfo = reinterpret_cast<TOKEN_PRIVILEGES *>(malloc( dwLengthNeeded ))) != NULL )
            {
                // Allocated space for Token Information

                if ( GetTokenInformation( hToken,
                                          TokenPrivileges,
                                          pTokenPrivilegesInfo,
                                          dwLengthNeeded,
                                          &dwLengthNeeded )
                   )
                {
                    // Got Token Information

                    ShowThreadPrivilegeCollection( pTokenPrivilegesInfo ) ;

                } // Got Token Information

                free( pTokenPrivilegesInfo );

            }

        } // Allocate a large enough buffer for the token info

        CloseHandle( hToken );

    }
    else
    {
        DWORD dwError = GetLastError();
        cout << "OpenThreadToken returned error " << dwError << " = \""
                << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
                << "\"" << endl ;
    }

} // ShowAllThreadPrivileges

bool EnableHighThreadPrivilege( void )
{
    bool bEnabled = false ;

    if ( g_bDebugOutput )
    {
        cout << "EnableHighThreadPrivilege : Begin" << endl ;
    }

    if ( ! ImpersonateSelf( SecurityImpersonation ) )
    {
        DWORD dwError = GetLastError();
        char szErrorMessage[ 200 ] ;
        cout << "ImpersonateSelf returned error " << dwError << " = \""
             << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
             << "\"" << endl ;
        exit(1);
    }

    if ( g_bDebugOutput )
    {
        cout << "EnableHighThreadPrivilege : Before changing privileges" << endl ;
        ShowAllThreadPrivileges();
    }

    HANDLE hThreadToken = INVALID_HANDLE_VALUE ;
    if ( ! OpenThreadToken( GetCurrentThread(),
			  			    TOKEN_QUERY | TOKEN_ADJUST_PRIVILEGES,
						    TRUE,
						    &hThreadToken )
       )
    {
        DWORD dwError = GetLastError();
        char szErrorMessage[ 200 ] ;
        cout << "OpenThreadToken returned error " << dwError << " = \""
             << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
             << "\"" << endl ;
        exit(1);
    }
    else
    {
        const char *pcszPrivilegeName = SE_DEBUG_NAME ;
        if ( ! SetPrivilege( hThreadToken , pcszPrivilegeName , TRUE ) )
        {
            cout << "SetPrivilege \"" << pcszPrivilegeName << " failed" << endl ;
        }
        else
        {
            if ( g_bDebugOutput )
            {
                cout << "SetPrivilege \"" << pcszPrivilegeName << " succeeded" << endl ;
            }
            bEnabled = true ;
        }

        CloseHandle( hThreadToken ) ;
    }

    if ( g_bDebugOutput )
    {
        cout << "EnableHighThreadPrivilege : After changing privileges" << endl ;
        ShowAllThreadPrivileges();
        cout << "EnableHighThreadPrivilege : End with " << (bEnabled?"TRUE":"FALSE") << endl ;
    }

    return bEnabled ;

} // EnableHighThreadPrivilege

const char * DayName( DWORD dwDayOfWeek )
{
    const char * pcszDayName = NULL ;

    switch ( dwDayOfWeek )
    {
    case 0  : pcszDayName = "Sunday"    ; break ;
    case 1  : pcszDayName = "Monday"    ; break ;
    case 2  : pcszDayName = "Tuesday"   ; break ;
    case 3  : pcszDayName = "Wednesday" ; break ;
    case 4  : pcszDayName = "Thursday"  ; break ;
    case 5  : pcszDayName = "Friday"    ; break ;
    case 6  : pcszDayName = "Saturday"  ; break ;
    default : pcszDayName = "Unknown"   ; break ;
    }

    return pcszDayName ;

} // DayName

string UpperCaseOf( const string & strSource )
{
    char szUpper[ MAX_PATH ] = { "" } ;
    strncpy_s( szUpper , _countof(szUpper) , strSource.c_str() , _TRUNCATE ) ;
    _strupr_s( szUpper , _countof( szUpper ) ) ;
    return string( szUpper ) ;
} // UpperCaseOf

class ECServicesSet
{
        class ECServiceDetails
        {
        public:
            ECServiceDetails( const char *pcszServicePath )
            {
                char szServicePath[ _MAX_PATH ] = { "" } ;
                if ( GetLongPathName( (LPSTR)pcszServicePath , szServicePath , sizeof( szServicePath ) ) )
                    m_strServicePath = szServicePath ;
                else
                    m_strServicePath = pcszServicePath ;
            }

            ECServiceDetails( const char *pcszServiceName , const char * pcszServicePath , const char * pcszDisplayName )
            {
                m_strServiceName = pcszServiceName ;
                char szServicePath[ _MAX_PATH ] = { "" } ;
                if ( GetLongPathName( (LPSTR)pcszServicePath , szServicePath , sizeof( szServicePath ) ) )
                    m_strServicePath = szServicePath ;
                else
                    m_strServicePath = pcszServicePath ;
                m_strDisplayName = pcszDisplayName ;
            }

            string ServiceName( void ) const { return m_strServiceName ; }
            string ServicePath( void ) const { return m_strServicePath ; }
            string displayName( void ) const { return m_strDisplayName ; }

        private:
            string m_strServiceName ;
            string m_strServicePath ;
            string m_strDisplayName ;
        } ;

        struct ECServiceDetailsCaseInsensitiveServicePathCompare
        {
            bool operator()( const ECServiceDetails & sd1 ,
                             const ECServiceDetails & sd2 ) const
            {
                int intCompare = _strcmpi( sd1.ServicePath().c_str() , sd2.ServicePath().c_str() ) ;
                return intCompare < 0 ;
            }
        } ;

        void GetSetOfServices( void ) ;

    public:

        typedef set< ECServiceDetails , ECServiceDetailsCaseInsensitiveServicePathCompare > ECServiceDetailsSet ;

        ECServiceDetailsSet m_ServiceDetailsSet ;

        ECServicesSet() ;
        ~ECServicesSet() ;

        ECServiceDetailsSet ServiceDetailsSet( void ) const { return m_ServiceDetailsSet; }

        bool ECServicesSet::ProcessIsAService( const char * pcszProcessName ) ;

        void Display( int intIndent )
        {
            for ( ECServiceDetailsSet::const_iterator it = m_ServiceDetailsSet.begin() ; it != m_ServiceDetailsSet.end() ; ++it )
            {
                cout << setw( intIndent ) << "" ;
                cout << (*it).ServiceName() ;
                cout << setw( 0 ) ;
                cout << " = \"" << (*it).ServicePath() << "\"" << endl ;
            } // for it
        }

} ; // ECServicesSet

// Default constructor
ECServicesSet::ECServicesSet()
{

    GetSetOfServices();

} // Default constructor

// Destructor
ECServicesSet::~ECServicesSet()
{
} // Destructor

void ECServicesSet::GetSetOfServices( void )
{
    DWORD dwTotalServiceCount = 0 ;

    SC_HANDLE hSCM = OpenSCManager( NULL , NULL , SC_MANAGER_ENUMERATE_SERVICE );
    if ( hSCM == NULL )
    {
        DWORD dwError = GetLastError();
        char szErrorMessage[ 200 ] ;
        cout << "OpenSCManager returned error " << dwError << " = \""
             << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
             << "\"" << endl ;
    }
    else
    {
        // Service Manager Open
        DWORD dwError = 0 ;
        DWORD dwBytesNeeded = 0 ;
        DWORD dwServiceCount = 0 ;
        DWORD dwResumeHandle = 0 ;
        BOOL bSuccess = FALSE ;

        // Find out how much memory is required to receive all Service Statuses in one go
        EnumServicesStatus( hSCM ,
                            SERVICE_WIN32 ,
                            SERVICE_STATE_ALL ,
                            NULL ,
                            0 ,
                            &dwBytesNeeded ,
                            &dwServiceCount ,
                            &dwResumeHandle ) ;
        // Ensure the next EnumServicesStatus begins from the beginning
        dwResumeHandle = 0 ;

        ENUM_SERVICE_STATUS *pess = NULL ;
        DWORD dwServiceStatusSize = dwBytesNeeded ;

        pess = (ENUM_SERVICE_STATUS *)calloc( 1 , dwServiceStatusSize );
        if ( pess == NULL )
        {
            cout << "calloc for ENUM_SERVICE_STATUS of " << dwServiceStatusSize << " bytes failed" << endl ;
        }
        else
        {
            // Allocated buffer for ENUM_SERVICE_STATUS
            QUERY_SERVICE_CONFIG *pqsc ;
            DWORD dwServiceConfigSize = sizeof( QUERY_SERVICE_CONFIG ) + 5*MAX_PATH ;
            DWORD dwBytesNeeded = 0 ;

            pqsc = (QUERY_SERVICE_CONFIG *)calloc( 1 , dwServiceConfigSize ) ;
            if ( pqsc == NULL )
            {
                cout << "calloc for QUERY_SERVICE_CONFIG of " << dwServiceConfigSize << " bytes failed" << endl ;
            }
            else
            {
                // Allocated buffer for Serivce Configuration

                bSuccess = EnumServicesStatus( hSCM ,
                                               SERVICE_WIN32 ,
                                               SERVICE_STATE_ALL ,
                                               pess ,
                                               dwServiceStatusSize ,
                                               &dwBytesNeeded ,
                                               &dwServiceCount ,
                                               &dwResumeHandle ) ;
                if ( ! bSuccess )
                {
                    char szErrorMessage[ 200 ] ;
                    dwError = GetLastError();
                    cout << "EnumServicesStatus returned error " << dwError << " = \""
                         << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
                         << "\"" << endl ;
                }
                else if ( dwServiceCount )
                {
                    // Got all Service Details

                    DWORD dwServiceId = 0 ;
                    ENUM_SERVICE_STATUS *pessThis = NULL ;

                    // Get the details for each Service
                    for ( dwServiceId = 0 , pessThis = pess ;
                          dwServiceId < dwServiceCount ;
                          ++dwServiceId , ++pessThis
                        )
                    {

                        // Open the Service to get it's image name
                        SC_HANDLE hService = OpenService( hSCM , pessThis->lpServiceName , SERVICE_QUERY_CONFIG );
                        if ( hService == NULL )
                        {
                            DWORD dwError = GetLastError();

                            char szErrorMessage[ 200 ] ;

                            cout << "OpenService returned error " << dwError << " = \""
                                 << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
                                 << "\"" << endl ;
                        }
                        else
                        {
                            // Opened Service

                            if ( ! QueryServiceConfig( hService ,
                                                       pqsc ,
                                                       dwServiceConfigSize , 
                                                       &dwBytesNeeded )
                               )
                            {
                                DWORD dwError = GetLastError();

                                char szErrorMessage[ 200 ] ;

                                cout << "QueryServiceConfig returned error " << dwError << " = \""
                                     << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
                                     << "\"" << endl ;
                            }
                            else
                            {
                                // Got Service Configuration

                                ECServiceDetails ServiceDetails( pessThis->lpServiceName , pqsc->lpBinaryPathName , pessThis->lpDisplayName ) ;

                                m_ServiceDetailsSet.insert( ServiceDetails );
                                dwTotalServiceCount += 1 ;

                                // cout << "Service " << dwTotalServiceCount << " = " << pessThis->lpDisplayName << " : " << pessThis->lpServiceName << " = " << ServiceDetails.szServicePath << endl ;

                            } // Got Service Configuration

                            CloseServiceHandle( hService ) ;

                        } // Opened Service

                    } // for dwServiceId

                } // Got all Service Details

                free( pqsc ) ;
                pqsc = NULL ;

            } // Allocated buffer for Serivce Configuration

            free( pess ) ;
            pess = NULL ;

        } // Allocated buffer for ENUM_SERVICE_STATUS

        CloseServiceHandle( hSCM );

    } // Service Manager Open

} // ECServicesSet::GetSetOfServices

bool ECServicesSet::ProcessIsAService( const char * pcszProcessName )
{
    bool bIsService = false ;

    // Only need the Service Path
    ECServiceDetails ServiceDetailsThis( pcszProcessName ) ;

    ECServiceDetailsSet::const_iterator it = m_ServiceDetailsSet.find( ServiceDetailsThis ) ;
    // Is a member if the group is in the set of groups
    bIsService = ( it != m_ServiceDetailsSet.end() ) ;

    return bIsService ;

} // ECServicesSet::ProcessIsAService

class ECProcessDetail
{
public:
    ECProcessDetail( const char *pcszFullPath , DWORD dwProcessId )
    {
        char szFullPath[ _MAX_PATH ] = { "" } ;
        if ( GetLongPathName( (LPSTR)pcszFullPath , szFullPath , sizeof( szFullPath ) ) )
            m_strFullPath = szFullPath ;
        else
            m_strFullPath = pcszFullPath ;
        m_dwProcessId = dwProcessId ;
    }

    string FullPath( void ) const
    {
        return m_strFullPath ;
    }

    string FullPathUpperCase( void ) const
    {
        return UpperCaseOf( m_strFullPath ) ;
    }

    DWORD ProcessId( void ) const
    {
        return m_dwProcessId ;
    }

private:
    string m_strFullPath ;
    DWORD m_dwProcessId ;

} ; // ECProcessDetail

class ECAllProcessDetailSet
{
public:
    ECAllProcessDetailSet()
    {
        if ( g_bDebugOutput )
        {
            cout << "ECAllProcessDetailSet()" << endl ;
        }

        DWORD adwProcessId[ 4000 ] = { 0 } ;
        DWORD cbProcessIdArraySize = sizeof( adwProcessId ) ;

        if ( ! EnumProcesses( &adwProcessId[0] , 
                              sizeof( adwProcessId ) ,
                              &cbProcessIdArraySize )
           )
        {
            char szErrorMessage[ 200 ] ;

            DWORD dwError = GetLastError();

            cout << "EnumProcesses returned error " << dwError << " = \""
                 << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
                 << "\"" << endl ;
        }
        else
        {
            // Got process identifiers

            // Make sure we know this process
            DWORD dwThisProcessId = _getpid();

            DWORD dwProcessIdCount = 0 ;

            dwProcessIdCount = cbProcessIdArraySize/sizeof( adwProcessId[0] ) ;

            cout << "ECAllProcessDetailSet : " << dwProcessIdCount << " processes identified" << endl ;

            EnableHighThreadPrivilege();

            for ( DWORD dwProcessIdIndex = 0 ;
                  dwProcessIdIndex < dwProcessIdCount ;
                  ++dwProcessIdIndex
                )
            {

                DWORD dwProcessId = adwProcessId[ dwProcessIdIndex ] ;

                if ( dwProcessId == 0 )
                {
                    if ( g_bDebugOutput )
                    {
                        cout << dwProcessIdIndex << " : Skipping Process " << dwProcessId << " = System Idle Process" << endl ;
                    }
                }
                else
                {
                    // Not the System Idle Process

                    if ( g_bDebugOutput )
                    {
                        cout << dwProcessIdIndex << " : Opening Process Id " << dwProcessId << endl ;
                    }

                    HANDLE hProcess = OpenProcess( /* PROCESS_ALL_ACCESS */
                                                   PROCESS_QUERY_INFORMATION
                                                   | PROCESS_VM_READ
                                                   /* | PROCESS_QUERY_LIMITED_INFORMATION */
                                                   /* | PROCESS_SET_INFORMATION */
                                                   | PROCESS_TERMINATE
                                                   ,
                                                   FALSE ,
                                                   dwProcessId
                                                 ) ;
                    if ( hProcess == NULL )
                    {
                        // OpenProcess failed

                        if ( g_bDebugOutput )
                        {
                            cout << "Failed to open Process " << dwProcessId << endl ;
                        }

                        DWORD dwError = GetLastError();

                        if ( dwError != ERROR_ACCESS_DENIED )
                        {
                            // Only bother displaying errors that are NOT Access Denied
                            char szErrorMessage[ 200 ] ;
                            cout << "OpenProcess returned error " << dwError << " = \""
                                 << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
                                 << "\"" << endl ;

                        } // Only bother displaying errors that are NOT Access Denied

                    } // OpenProcess failed
                    else
                    {
                        // Opened process

                        if ( g_bDebugOutput )
                        {
                            cout << dwProcessIdIndex << " : Opened Process " << dwProcessId << endl ;
                        }

                        char szShortModuleName[ MAX_PATH ] = { "" } ;
                        char szEXEShortName[ MAX_PATH ] = { "" } ;
                        bool bSystemProcess = false ;

                        if ( ! GetModuleFileNameEx( hProcess ,
                                                    NULL ,
                                                    szShortModuleName ,
                                                    sizeof( szShortModuleName ) )
                            )
                        {
                            DWORD dwError = GetLastError();
                            // Check that the process is not a System Process for which ERROR_PARTIAL_COPY is returned
                            if ( dwError == ERROR_PARTIAL_COPY )
                                bSystemProcess = true ;
                            else
                            {
                                char szErrorMessage[ 200 ] ;
                                cout << "GetModuleFileNameEx returned error " << dwError << " = \""
                                        << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
                                        << "\"" << endl ;
                            }

                        }
                        else
                        {
                            // Got module filename in device form

                            if ( g_bDebugOutput )
                            {
                                cout << "Module filename \"" << szShortModuleName << "\"" << endl ;
                            }

                            // Find the colon
                            char * pszColon = strchr( szShortModuleName , ':' ) ;

                            if ( pszColon == NULL )
                                strncpy_s( szEXEShortName , _countof( szEXEShortName ) , szShortModuleName , _TRUNCATE ) ;
                            else
                            {
                                char *pszFilename = pszColon-1 ;
                                strncpy_s( szEXEShortName , _countof( szEXEShortName ) , pszFilename , _TRUNCATE ) ;
                            }

                        } // Got module filename in device form

                        if ( ! bSystemProcess )
                        {
                            // Not a system process

                            if ( *szEXEShortName == '\0' )
                                cout << "Could not determine EXE name for Process Id " << dwProcessId << endl ;
                            else
                            {
                                // Got EXE name

                                ECProcessDetail processDetail( szEXEShortName , dwProcessId ) ;

                                m_ProcessDetailSet.insert( processDetail ) ;

                            } // Got EXE name

                            if ( ! CloseHandle( hProcess ) )
                            {
                                char szErrorMessage[ 200 ] ;
    
                                DWORD dwError = GetLastError();

                                cout << "CloseHandle returned error " << dwError << " = \""
                                     << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
                                     << "\"" << endl ;
                            }
                            else if ( g_bDebugOutput )
                            {
                                cout << dwProcessIdIndex << " : Closed Process " << dwProcessId << endl ;
                            }

                        } // Not a system process

                    } // Opened process

                } // Not the System Idle Process

            } // for dwProcessIdIndex

            cout << "ECAllProcessDetailSet : " << m_ProcessDetailSet.size() << " processes accessible" << endl ;

        } // Got process identifiers

    } // ECAllProcessDetailSet default constructor

    void EndProcessPathFamily( const char *pcszProcessFamilyPath )
    {
        cout << endl ;
        cout << "Ending Process Family Path \"" << pcszProcessFamilyPath << "\"" << endl ;
        cout << endl ;

        string strProcessFamilyPathUpper = UpperCaseOf( string( pcszProcessFamilyPath ) ) ;

        ECServicesSet ServiceSet ;

        // Make sure we know this process
        DWORD dwThisProcessId = _getpid();

        if ( g_bDebugOutput )
        {
            cout << "Process count is " << m_ProcessDetailSet.size() << endl ;
        }

        for ( ECProcessDetailSet::const_iterator itProcess = m_ProcessDetailSet.begin() ;
              itProcess != m_ProcessDetailSet.end() ;
              ++itProcess
            )
        {
            if ( g_bDebugOutput )
            {
                cout << "Process \"" << (*itProcess).FullPath() << "\"" << endl ;
            }

            HANDLE hProcess = OpenProcess( /* PROCESS_ALL_ACCESS */
                                            PROCESS_QUERY_INFORMATION
                                            | PROCESS_VM_READ
                                            /* | PROCESS_QUERY_LIMITED_INFORMATION */
                                            /* | PROCESS_SET_INFORMATION */
                                            | PROCESS_TERMINATE
                                            ,
                                            FALSE ,
                                            (*itProcess).ProcessId()
                                            ) ;
            if ( hProcess == NULL )
            {
                // OpenProcess failed

                DWORD dwError = GetLastError();

                // if ( dwError != ERROR_ACCESS_DENIED )
                {
                    // Only bother displaying errors that are NOT Access Denied
                    char szErrorMessage[ 200 ] ;

                    cout << "EndProcessPathFamily : OpenProcess returned error " << dwError << " = \""
                            << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
                            << "\"" << endl ;

                } // Only bother displaying errors that are NOT Access Denied

            } // OpenProcess failed
            else
            {
                // Opened process

                string strThisFullPathUpperCase = (*itProcess).FullPathUpperCase() ;
                const char * pcszThisProcessFamilyPath = strstr( strThisFullPathUpperCase.c_str() , strProcessFamilyPathUpper.c_str() ) ;

                if (    ( pcszProcessFamilyPath != NULL )
                     && ( pcszThisProcessFamilyPath != NULL  )
                   )
                {
                    // This process is in the selected process family

                    cout << (*itProcess).FullPath() << endl ;

                    if ( dwThisProcessId == (*itProcess).ProcessId() )
                        // Don't terminate this process
                        cout << "    This is the current process - Not terminating" << endl ;
                    else if ( ServiceSet.ProcessIsAService( (*itProcess).FullPath().c_str() ) )
                    {
                        cout << "    \"" << (*itProcess).FullPath() << "\" is a Service - Not Terminating" << endl ;
                    }
                    else
                    {
                        // Terminate the process

                        cout << "    **** Terminating \"" << (*itProcess).FullPath() << "\"" ;
                        if ( TerminateProcess( hProcess , 3 ) != 0 )
                            cout << " - Succeeded" ;
                        else
                            cout << " - Failed" ;
                        cout << endl ;

                    } // Terminate the process

                } // This process is in the selected process family

                if ( ! CloseHandle( hProcess ) )
                {
                    char szErrorMessage[ 200 ] ;
    
                    DWORD dwError = GetLastError();

                    cout << "CloseHandle returned error " << dwError << " = \""
                            << SystemErrorMessage( szErrorMessage , sizeof( szErrorMessage ) , dwError )
                            << "\"" << endl ;
                }

            } // Opened Process

        } // for itProcess

        cout << endl ;
        cout << "Finished ending Process Family Tree \"" << pcszProcessFamilyPath << "\"" << endl ;
    }

    private:

        struct ECProcessDetailCaseInsensitiveServicePathCompare
        {
            bool operator()( const ECProcessDetail & pdLeft ,
                             const ECProcessDetail & pdRight ) const
            {
                return _strcmpi( pdLeft.FullPath().c_str() , pdRight.FullPath().c_str() ) < 0 ;
            }
        } ;

        typedef set< ECProcessDetail , ECProcessDetailCaseInsensitiveServicePathCompare > ECProcessDetailSet ;

        ECProcessDetailSet m_ProcessDetailSet ;

} ; // ECAllProcessDetailSet

int _tmain(int argc, _TCHAR* argv[])
{
    ECServicesSet ServicesSet ;

    cout << "Service List" << endl ;
    ServicesSet.Display( 4 );

    ECAllProcessDetailSet processDetailSet ;

    if ( argc > 1 )
    {
        processDetailSet.EndProcessPathFamily( argv[1] ) ;
    }

	return 0;
} // _twinmain
