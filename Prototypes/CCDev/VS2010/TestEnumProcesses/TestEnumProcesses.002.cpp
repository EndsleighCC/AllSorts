// Test Program to enumerate all processes in the system
// C. Cornelius 22-Jan-2000

#define _WIN32_WINNT 0x400

#include <windows.h>

#include <psapi.h>

#include <iostream>

using namespace std ;

#include <process.h>

#define STRNCPY( szDest , szSrc , cbDestSize ) \
                ( strncpy( szDest , szSrc , cbDestSize ) , szDest[ cbDestSize-1 ] = '\0' )
#define STRNCAT( szDest , szSrc , cbDestSize ) \
                ( strncat( szDest , szSrc , ( ((int)cbDestSize-strlen(szSrc)-1)<=0 \
                  ? 0 \
                  : ((int)cbDestSize-strlen(szSrc)-1) ) ) , szDest[ cbDestSize-1 ] = '\0' )

const char * ErrorMessage( LPSTR pszMessage ,
                           ULONG culMessageSize ,
                           HRESULT hr )
{
    void    *pMsgBuffer ;

    ::FormatMessage( 
        FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM ,
        NULL ,
        hr ,
        MAKELANGID( LANG_NEUTRAL , SUBLANG_DEFAULT ) ,
        (LPTSTR) &pMsgBuffer ,
        0 ,
        NULL );

    STRNCPY( pszMessage , static_cast<const char *>( pMsgBuffer ) , culMessageSize );
    pszMessage[ culMessageSize-1 ] = '\0' ;

    char * pch = &pszMessage[ strlen( pszMessage )-1 ] ;
    while (  ( pch > pszMessage ) && ( ( *pch == '\n' ) || ( *pch == '\r' ) ) )
    {
        *pch-- = '\0' ;
    }

    // Free the buffer
    LocalFree( pMsgBuffer );

    return pszMessage ;

} // ErrorMessage

#if 0
/*
.page .section ExpandToLongFileName
+-----------------------+ ExpandToLongFileName +--------------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|                                                                         |
|   ExpandToLongFileName                                                  |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|                                                                         |
|   Under certain conditions Windows NT will maintain the running process |
|   name in an 8.3 file name with tilda (~) characters and numbers        |
|   distinguishing different 8.3 equivalents of a long file name. This    |
|   function expands the 8.3 format running process name into the full    |
|   name.                                                                 |
|                                                                         |
|   This function performs disk directory management to expand the        |
|   running process name so that it should not be called during DLL       |
|   instance initialisation unless serialisation is in operation.         |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|                                                                         |
| pcszShortFileName [in]                                                  |
|                                                                         |
|   A pointer to a buffer containing the short file name for which the    |
|   long filename is required.                                            |
|                                                                         |
| pszLongFileName [out]                                                   |
|                                                                         |
|   A pointer to a buffer to receive the long file name                   |
|                                                                         |
| intLongFileNameSize [in]                                                |
|                                                                         |
|   The size in bytes of the supplied buffer.                             |
|                                                                         |
| RETURN VALUE:                                                           |
|                                                                         |
| EIS_ERROR                                                               |
|                                                                         |
| LAST MODIFIED DATE: 14-Feb-2001                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/

#include <io.h>

void ExpandToLongFileName( const char *pcszShortFileName ,
                           char * const pszLongFileName ,
                           const int intLongFileNameSize )
{
    char szShortFileNameBuffer[ MAX_PATH ] = { 0 } ;
    const char * pcszNameWithSlash = strchr( pcszShortFileName , '\\' );
    const char * pcszName = NULL ;

    memset( pszLongFileName , 0 , intLongFileNameSize );
    memcpy( pszLongFileName , pcszShortFileName , pcszNameWithSlash-pcszShortFileName );

    // Skip the backslash and point to the start of the second part of the path
    pcszNameWithSlash = strchr( pcszNameWithSlash+1 , '\\' );
    pcszName = pcszNameWithSlash + 1 ;
    while ( pcszName != NULL )
    {
        // Got the next path component

        int intShortFileNameLength = 0 ;

        if ( pcszNameWithSlash != NULL )
        {
            // Next path element

            // Skip the backslash and point to the start of the next part of the path
            pcszName = pcszNameWithSlash + 1 ;
            intShortFileNameLength = pcszName - pcszShortFileName - 1 ;

        } // Next path element
        else
            // The last path element i.e. the name
            intShortFileNameLength = strlen( pcszShortFileName ) ;

        // Look for the long file name corresponding to these left hand
        // parts of the short filename
        strncpy( szShortFileNameBuffer , pcszShortFileName , intShortFileNameLength );
        szShortFileNameBuffer[ intShortFileNameLength ] = '\0' ;

        long  lHandle ;
        struct  _finddata_t FindData ;

        lHandle = _findfirst( szShortFileNameBuffer , &FindData );
        if ( lHandle != -1 )
        {
            // Found a match

            STRNCAT( pszLongFileName , "\\" , intLongFileNameSize );
            STRNCAT( pszLongFileName , FindData.name , intLongFileNameSize );

            _findclose( lHandle );

        } // Found a match

        if ( pcszNameWithSlash != NULL )
            // Only the name left to process. Look after the initial slash
            pcszNameWithSlash = strchr( ++pcszNameWithSlash , '\\' );
        else
            // No more components of the path to process
            pcszName = NULL ;

    } // Got the next path component

    if ( *pszLongFileName == '\0' )
    {
        // Could not find the file so return the Short Name
        cout << "Setting long filename to \"" << pcszShortFileName << "\"" << endl ;
        STRNCPY( pszLongFileName , pcszShortFileName , intLongFileNameSize );
    }

} // ExpandToLongFileName
#endif

/*
.page .section ExpandToLongFileName
+-----------------------+ ExpandToLongFileName +--------------------------+
| # header #                                                              |
|                                                                         |
| FUNCTION NAME:                                                          |
|                                                                         |
|   ExpandToLongFileName                                                  |
|                                                                         |
| FUNCTIONAL DESCRIPTION:                                                 |
|                                                                         |
|   Under certain conditions Windows NT will maintain the name of a       |
|   file in an 8.3 format with tilda (~) characters, numbers and          |
|   semi-arbitrary letters distinguishing different 8.3 equivalents of    |
|   a long file name. This function expands the 8.3 format filename       |
|   into the full "long" filename.                                        |
|                                                                         |
|   This function performs disk directory management to expand the        |
|   filename so that it should not be called during DLL instance          |
|   initialisation unless serialisation is in operation.                  |
|                                                                         |
| FORMAL PARAMETERS:                                                      |
|                                                                         |
| pszLongFileName [out]                                                   |
|                                                                         |
|   A pointer to a buffer to receive the long file name                   |
|                                                                         |
| pcszShortFileName [in]                                                  |
|                                                                         |
|   A pointer to a buffer containing the short file name for which the    |
|   long filename is required.                                            |
|                                                                         |
| intLongFileNameSize [in]                                                |
|                                                                         |
|   The size in bytes of pszLongFileName                                  |
|                                                                         |
| RETURN VALUE:                                                           |
|                                                                         |
|   false = The file with the specified short filename does not exist and |
|           the returned long filename will be set to the same as the     |
|           short filename                                                |
|   true  = The file with the specified short filename exists and the     |
|           equivalent long filename will be returned                     |
|                                                                         |
| LAST MODIFIED DATE: 14-Feb-2001                                         |
|                                                                         |
|  # header-end #                                                         |
+-------------------------------------------------------------------------+
*/

#include <io.h>

bool ExpandToLongFileName( char * const pszLongFileName ,
                           const char *pcszShortFileName ,
                           const int intLongFileNameSize )
{
    bool bSuccess = true ;
    const char * pcszSpecialProcessPrefix = "??" ;
    const char * pcszSystemRootEnvironmentVar = "SystemRoot" ;
    const char * pcszBackslash = "\\" ;
    const char * pcszColon = ":" ;
    char szShortFileNameBuffer[ MAX_PATH ] = { "" } ;
    char szShortFileNameSearchName[ MAX_PATH ] = { "" } ;

    // Make sure the output starts off blank
    STRNCPY( pszLongFileName , "" , intLongFileNameSize );

    // Copy the name so that it can be parsed using strtok
    STRNCPY( szShortFileNameBuffer , pcszShortFileName , sizeof( szShortFileNameBuffer ) );

    char * pszNamePart = strtok( szShortFileNameBuffer , pcszBackslash );
    if ( pszNamePart == NULL )
    {
        // No parts of the name to parse so just use the one supplied
        long lHandle ;
        struct _finddata_t FindData ;

        lHandle = _findfirst( pcszShortFileName , &FindData );
        if ( lHandle != -1 )
        {
            // Found a match

            STRNCAT( pszLongFileName , FindData.name , intLongFileNameSize );

            _findclose( lHandle );

        } // Found a match
        else
        {
            // Part of the path does not exist so short filename is invalid

            bSuccess = false ;
            // Set it to the same as the short filename
            STRNCPY( pszLongFileName , pcszShortFileName , intLongFileNameSize );

        } // Part of the path does not exist so short filename is invalid

    } // No parts of the name to parse so just use the one supplied
    else
    {
        // There is a path to parse

        if ( strcmpi( pszNamePart , pcszSpecialProcessPrefix ) == 0 )
            // Skip the "special" prefix
            pszNamePart = strtok( NULL , pcszBackslash );
        else
            if ( strcmpi( pszNamePart , pcszSystemRootEnvironmentVar ) == 0 )
            {
                // Patch up the name since it contains a SystemRoot Environment variable
                const char * pcszSystemRootEnvironment = getenv( pcszSystemRootEnvironmentVar ) ;

                if ( pcszSystemRootEnvironment != NULL )
                {
                    // Rebuild the short name with the SystemRoot path replaced

                    // The actual name comes after the environment variable name
                    const char * pcszStartName = &pcszShortFileName[ (pszNamePart-szShortFileNameBuffer)+strlen(pszNamePart)+1 ] ;

                    // Copy in the SystemRoot path
                    STRNCPY( szShortFileNameBuffer , pcszSystemRootEnvironment , sizeof( szShortFileNameBuffer ) );
                    STRNCAT( szShortFileNameBuffer , pcszBackslash , sizeof( szShortFileNameBuffer ) );
                    // Concatenate the rest of the name
                    memcpy( &szShortFileNameBuffer[ strlen(szShortFileNameBuffer) ] ,
                            pcszStartName ,
                            strlen( pcszStartName )+1 ); // Include the terminating NUL

                    // Begin strtok again
                    pszNamePart = strtok( szShortFileNameBuffer , pcszBackslash );

                } // Rebuild the short name with the SystemRoot path replaced

            } // Patch up the name since it contains a SystemRoot Environment variable

        if (    ( pszNamePart != NULL )
             && ( strchr( pszNamePart , *pcszColon ) != NULL )
           )
        {
            // First part is a drive specifier so copy that in an go to the next part

            STRNCPY( pszLongFileName , pszNamePart , intLongFileNameSize );

            // Get the next part of the path
            pszNamePart = strtok( NULL , pcszBackslash );

        } // First part is a drive specifier so copy that in an go to the next part

        // Update the first search item
        STRNCPY( szShortFileNameSearchName , pszLongFileName , sizeof( szShortFileNameSearchName ) );

        while ( ( bSuccess ) && ( pszNamePart != NULL ) )
        {
            long lHandle ;
            struct _finddata_t FindData ;

            STRNCAT( szShortFileNameSearchName , pcszBackslash , sizeof( szShortFileNameSearchName ) );
            STRNCAT( szShortFileNameSearchName , pszNamePart , sizeof( szShortFileNameSearchName ) );

            lHandle = _findfirst( szShortFileNameSearchName , &FindData );
            if ( lHandle != -1 )
            {
                // Found a match

                STRNCAT( pszLongFileName , "\\" , intLongFileNameSize );
                STRNCAT( pszLongFileName , FindData.name , intLongFileNameSize );

                _findclose( lHandle );

            } // Found a match
            else
            {
                // Part of the path does not exist so short filename is invalid

                bSuccess = false ;
                // Set it to the same as the short filename
                STRNCPY( pszLongFileName , pcszShortFileName , intLongFileNameSize );

            } // Part of the path does not exist so short filename is invalid

            // Skip to the next part
            pszNamePart = strtok( NULL , pcszBackslash );

        } // while

    } // There is a path

    return bSuccess ;

} // ExpandToLongFileName

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

struct CServiceDetails
{
    char szServiceName[ MAX_PATH ] ;
    char szServicePath[ MAX_PATH ] ;
    char szDisplayName[ MAX_PATH ] ;
} ;

#include <set>

using namespace std ;

struct CServiceDetailsCaseInsensitiveServicePathCompare
{
    bool operator()( const CServiceDetails & sd1 ,
                     const CServiceDetails & sd2 ) const
    {
        return strcmpi( sd1.szServicePath , sd2.szServicePath ) < 0 ;
    }
} ;

typedef set< CServiceDetails , CServiceDetailsCaseInsensitiveServicePathCompare > CServiceDetailsSet ;

void GetListOfServices( CServiceDetailsSet & ServiceDetailsSet )
{
    CServiceDetails ServiceDetails ;
    DWORD dwTotalServiceCount = 0 ;

    SC_HANDLE hSCM = OpenSCManager( NULL , NULL , SC_MANAGER_ENUMERATE_SERVICE );
    if ( hSCM == NULL )
    {
        DWORD dwError = GetLastError();

        char szMessage[ 200 ] ;

        cout << "OpenSCManager returned error " << dwError << " = \""
             << ErrorMessage( szMessage , sizeof( szMessage ) , dwError )
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
                    char szMessage[ 200 ] ;

                    dwError = GetLastError();

                    cout << "EnumServicesStatus returned error " << dwError << " = \""
                         << ErrorMessage( szMessage , sizeof( szMessage ) , dwError )
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

                            char szMessage[ 200 ] ;

                            cout << "OpenService returned error " << dwError << " = \""
                                 << ErrorMessage( szMessage , sizeof( szMessage ) , dwError )
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

                                char szMessage[ 200 ] ;

                                cout << "QueryServiceConfig returned error " << dwError << " = \""
                                     << ErrorMessage( szMessage , sizeof( szMessage ) , dwError )
                                     << "\"" << endl ;
                            }
                            else
                            {
                                // Got Service Configuration

                                STRNCPY( ServiceDetails.szServiceName , pessThis->lpServiceName , sizeof( ServiceDetails.szServiceName ) );
                                ExpandToLongFileName( ServiceDetails.szServicePath , pqsc->lpBinaryPathName , sizeof( ServiceDetails.szServicePath ) );
                                STRNCPY( ServiceDetails.szDisplayName , pessThis->lpDisplayName , sizeof( ServiceDetails.szDisplayName ) );

                                ServiceDetailsSet.insert( ServiceDetails );
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

} // GetListOfServices

bool ProcessIsAService( const char * pcszProcessName , const CServiceDetailsSet & ServiceDetailsSet )
{
    bool bIsService = false ;

    CServiceDetails ServiceDetailsThis ;

    // Only need the Service Path
    STRNCPY( ServiceDetailsThis.szServicePath , pcszProcessName , sizeof( ServiceDetailsThis.szServicePath ) );

    CServiceDetailsSet::const_iterator it = ServiceDetailsSet.find( ServiceDetailsThis ) ;
    // Is a member if the group is in the set of groups
    bIsService = ( it != ServiceDetailsSet.end() ) ;

    return bIsService ;

} // ProcessIsAService

int main( const int cintArgCount , const char * apcszArgValue[] )
{
    int intError = 0 ;

    DWORD adwProcessId[ 4000 ] = { 0 } ;
    DWORD cbProcessIdCount = sizeof( adwProcessId ) ;
    const char * pcszProcessFamily = NULL ;
    char szProcessFamilyUpper[ MAX_PATH ] = { "" } ;
    const char * pcszAction = NULL ;

    if ( cintArgCount > 1 )
    {
        pcszProcessFamily = apcszArgValue[1] ;
        STRNCPY( szProcessFamilyUpper , pcszProcessFamily , sizeof( szProcessFamilyUpper ) );
        strupr( szProcessFamilyUpper );
        if ( cintArgCount > 2 )
            pcszAction = apcszArgValue[2] ;
    }

    CServiceDetailsSet ServiceDetailsSet ;

    GetListOfServices( ServiceDetailsSet );

    if ( ! EnumProcesses( &adwProcessId[0] , 
                          sizeof( adwProcessId ) ,
                          &cbProcessIdCount )
       )
    {
        char szMessage[ 200 ] ;

        DWORD dwError = GetLastError();

        cout << "EnumProcesses returned error " << dwError << " = \""
             << ErrorMessage( szMessage , sizeof( szMessage ) , dwError )
             << "\"" << endl ;
    }
    else
    {
        // Got process identifiers

        // Make sure we know this process
        DWORD dwThisProcessId = _getpid();

        DWORD dwProcessIdCount = 0 ;

        dwProcessIdCount = cbProcessIdCount/sizeof( adwProcessId[0] ) ;

        cout << "There are " << dwProcessIdCount << " processes running" << endl ;

        for ( DWORD dwProcessIdIndex = 0 ;
              dwProcessIdIndex < dwProcessIdCount ;
              ++dwProcessIdIndex
            )
        {

            DWORD dwProcessId = adwProcessId[ dwProcessIdIndex ] ;

            if ( dwProcessId == 0 )
            {
                cout << dwProcessIdIndex << " : Skipping Process " << dwProcessId << " = System Idle Process" << endl ;
            }
            else
            {
                // Not the System Idle Process

                #if 0
                cout << dwProcessIdIndex << " : Opening Process " << dwProcessId << endl ;
                #endif

                HANDLE hProcess = OpenProcess( PROCESS_ALL_ACCESS
                                               /* PROCESS_QUERY_INFORMATION
                                               | PROCESS_SET_INFORMATION
                                               | PROCESS_TERMINATE */
                                               ,
                                               FALSE ,
                                               dwProcessId
                                             ) ;
                if ( hProcess == NULL )
                {
                    // OpenProcess failed

                    DWORD dwError = GetLastError();

                    if ( dwError != ERROR_ACCESS_DENIED )
                    {
                        // Only bother displaying errors that are NOT Access Denied
                        char szMessage[ 200 ] ;

                        cout << "OpenProcess returned error " << dwError << " = \""
                             << ErrorMessage( szMessage , sizeof( szMessage ) , dwError )
                             << "\"" << endl ;

                    } // Only bother displaying errors that are NOT Access Denied

                } // OpenProcess failed
                else
                {
                    // Opened process

                    #if 0
                    cout << dwProcessIdIndex << " : Opened Process " << dwProcessId << endl ;
                    #endif

                    HMODULE ahmModule[ 4000 ] = { 0 } ;
                    DWORD cbModuleCount = sizeof( ahmModule ) ;

                    if ( ! EnumProcessModules( hProcess ,
                                               ahmModule ,
                                               sizeof( ahmModule ) ,
                                               &cbModuleCount )
                       )
                    {
                        // EnumProcessModules failed

                        DWORD dwError = GetLastError();

                        if ( dwError != ERROR_ACCESS_DENIED )
                        {
                            // Only bother displaying errors that are NOT Access Denied

                            char szMessage[ 200 ] ;

                            cout << dwProcessIdIndex << " : EnumProcessModules(" << dwProcessId << "," << cbModuleCount << ") returned error " << dwError
                                 << " = \""
                                 << ErrorMessage( szMessage , sizeof( szMessage ) , dwError )
                                 << "\"" << endl ;

                        } // Only bother displaying errors that are NOT Access Denied

                    } // EnumProcessModules failed
                    else
                    {
                        // Got Module Handles for this process

                        const char * pcszExtensionEXE = ".EXE" ;
                        const int cintExtensionEXELength = sizeof( ".EXE" ) - 1 ;

                        DWORD dwModuleCount = cbModuleCount/sizeof( ahmModule[0] ) ;
                        char szShortModuleName[ MAX_PATH ] = { "" } ;

                        char szEXEShortName[ MAX_PATH ] = { "" } ;

                        // Find the EXE module first assuming that the first EXE encountered in this
                        // search is the actual EXE rather than an EXE that has been loaded as a resource
                        for ( DWORD dwModuleId = 0 ;
                                 ( *szEXEShortName == '\0' )
                              && ( dwModuleId < dwModuleCount ) ;
                              ++dwModuleId
                            )
                        {
                            HMODULE hmModule = ahmModule[ dwModuleId ] ;

                            if ( ! GetModuleFileNameEx( hProcess ,
                                                        hmModule ,
                                                        szShortModuleName ,
                                                        sizeof( szShortModuleName ) )
                               )
                            {
                                char szMessage[ 200 ] ;
    
                                DWORD dwError = GetLastError();

                                cout << "GetModuleFileName returned error " << dwError << " = \""
                                     << ErrorMessage( szMessage , sizeof( szMessage ) , dwError )
                                     << "\"" << endl ;

                            }
                            else
                            {
                                // Got module filename

                                int cintShortModuleNameLength = strlen( szShortModuleName ) ;

                                if (    ( cintShortModuleNameLength > cintExtensionEXELength ) 
                                     && ( strcmpi( &szShortModuleName[ cintShortModuleNameLength-cintExtensionEXELength ] , pcszExtensionEXE ) == 0 )
                                   )
                                   // It's an EXE name
                                   STRNCPY( szEXEShortName , szShortModuleName , sizeof( szEXEShortName ) );

                            } // Got module filename

                        } // for dwModuleId

                        if ( *szEXEShortName == '\0' )
                            cout << "Could not determine EXE name for Process Id " << dwProcessId << endl ;
                        else
                        {
                            // Got EXE name

                            char szEXELongName[ 2*MAX_PATH ] = { "" } ;
                            if ( ! ExpandToLongFileName( szEXELongName , szEXEShortName , sizeof( szEXELongName ) ) )
                            {
                                cout << "Long Name of \"" << szEXEShortName << "\" could not be determined" << endl ; 
                                for ( int intCharId = 0 ;
                                         ( intCharId <= 10 )
                                      && ( intCharId < strlen( szEXEShortName ) ) ;
                                      ++intCharId
                                    )
                                {
                                    cout << (unsigned int)szEXEShortName[ intCharId ] << "='" << szEXEShortName[ intCharId ] << "'" << " " ;
                                }
                                cout << endl ;
                            }

                            char szEXELongNameUpper[ MAX_PATH ] = { "" } ;

                            STRNCPY( szEXELongNameUpper , szEXELongName , sizeof( szEXELongNameUpper ) ) ;
                            strupr( szEXELongNameUpper );

                            if (    ( pcszProcessFamily == NULL )
                                 || ( strstr( szEXELongNameUpper , szProcessFamilyUpper ) != NULL )
                               )
                            {
                                // This process is in the selected process family

                                SIZE_T stMinimumWorkingSetSizeBytes = 0 ;
                                SIZE_T stMaximumWorkingSetSizeBytes = 0 ;
                                GetProcessWorkingSetSize( hProcess , &stMinimumWorkingSetSizeBytes , &stMaximumWorkingSetSizeBytes ) ;

                                FILETIME ftCreationTime = { 0 } ;
                                FILETIME ftExitTime = { 0 } ;
                                FILETIME ftKernelModeTime = { 0 } ;
                                FILETIME ftUserModeTime = { 0 } ;

                                GetProcessTimes( hProcess ,
                                                 &ftCreationTime ,
                                                 &ftExitTime ,
                                                 &ftKernelModeTime ,
                                                 &ftUserModeTime );

                                SYSTEMTIME stCreationTime = { 0 } ;
                                SYSTEMTIME stExitTime = { 0 } ;
                                ULARGE_INTEGER ulgKernelModeTime ;
                                ULARGE_INTEGER ulgUserModeTime ;

                                FileTimeToSystemTime( &ftCreationTime , &stCreationTime ) ;
                                FileTimeToSystemTime( &ftExitTime , &stExitTime ) ;
                                memcpy( &ulgKernelModeTime , &ftKernelModeTime , sizeof( ulgKernelModeTime ) );
                                memcpy( &ulgUserModeTime , &ftUserModeTime , sizeof( ulgUserModeTime ) );

                                cout << dwProcessIdIndex << " : " ;
                                if ( ProcessIsAService( szEXELongName , ServiceDetailsSet ) )
                                    cout << "Service " ;
                                else
                                    cout << "Process " ;
                                cout << szEXELongName << " (PID=" << dwProcessId << ") has "
                                     << dwModuleCount << " modules open." << endl ;
                                cout << "    Started " << stCreationTime.wHour 
                                                       << ":" << stCreationTime.wMinute 
                                                       << ":" << stCreationTime.wSecond
                                                       << "." << stCreationTime.wMilliseconds
                                                       << " on "
                                                       << DayName( stCreationTime.wDayOfWeek )
                                                       << " the "
                                                       << stCreationTime.wDay
                                                       << "-" << stCreationTime.wMonth
                                                       << "-" << stCreationTime.wYear
                                                       << endl ;
                                double dblKernelModeTimeSecs = (ulgKernelModeTime.HighPart*((double)(unsigned long)UINT_MAX+1) + (double)(unsigned long)ulgKernelModeTime.LowPart)*100e-9 ;
                                double dblUserModeTimeSecs = (ulgUserModeTime.HighPart*((double)(unsigned long)UINT_MAX+1) + (double)(unsigned long)ulgUserModeTime.LowPart)*100e-9 ;
                                cout << "    Kernel Mode Time = " << dblKernelModeTimeSecs << " seconds" << endl ;
                                cout << "    User Mode Time   = " << dblUserModeTimeSecs << " seconds" << endl ;
                                cout << "    Min Size = " << stMinimumWorkingSetSizeBytes << " Bytes."
                                     << " Max Size = " << stMaximumWorkingSetSizeBytes << " Bytes." << endl ;

                                if (    ( pcszAction != NULL )
                                     && ( strcmpi( pcszAction , "terminate" ) == 0 )
                                   )
                                {
                                    // Check whether to terminate the process

                                    if ( dwThisProcessId == dwProcessId )
                                        // Don't terminate this process
                                        cout << "    This is the current process - Not terminating" << endl ;
                                    else if ( ProcessIsAService( szEXELongName , ServiceDetailsSet ) )
                                    {
                                        cout << "    This is a Service - Not Terminating" << endl ;
                                    }
                                    else
                                    {
                                        // Terminate the process

                                        cout << "    **** Terminating" << endl ;
                                        TerminateProcess( hProcess , 3 ) ;

                                    } // Terminate the process

                                } // Check whether to terminate the process
                                else
                                {
                                    // No action so just display the module details

                                    for ( DWORD dwModuleId = 0 ;
                                          dwModuleId < dwModuleCount ;
                                          ++dwModuleId
                                        )
                                    {
                                        HMODULE hmModule = ahmModule[ dwModuleId ] ;

                                        if ( ! GetModuleFileNameEx( hProcess ,
                                                                    hmModule ,
                                                                    szShortModuleName ,
                                                                    sizeof( szShortModuleName ) )
                                           )
                                        {
                                            char szMessage[ 200 ] ;
    
                                            DWORD dwError = GetLastError();

                                            cout << "GetModuleFileName returned error " << dwError << " = \""
                                                 << ErrorMessage( szMessage , sizeof( szMessage ) , dwError )
                                                 << "\"" << endl ;

                                        }
                                        else
                                        {
                                            // Got module filename

                                            char szLongModuleName[ 400 ] ;
                                            ExpandToLongFileName( szLongModuleName , szShortModuleName , sizeof( szLongModuleName ) );

                                            cout << "    " ;
                                            cout.width(2) ;
                                            cout << dwModuleId << " (" << hmModule << ") : " << szLongModuleName << endl;
                                            if ( ( dwModuleId == 0 ) && ( dwThisProcessId == dwProcessId ) )
                                                // Indicate that this is this process - but only once
                                                cout << "    This is the current process" << endl ;

                                        } // Got module filename

                                    } // for dwModuleId

                                } // No action so just display the module details

                            } // This process is in the selected process family

                        } // Got EXE name

                    } // Got Module Handles for this process

                    if ( ! CloseHandle( hProcess ) )
                    {
                        char szMessage[ 200 ] ;
    
                        DWORD dwError = GetLastError();

                        cout << "CloseHandle returned error " << dwError << " = \""
                             << ErrorMessage( szMessage , sizeof( szMessage ) , dwError )
                             << "\"" << endl ;
                    }
                    #if 0
                    else
                    {
                        cout << dwProcessIdIndex << " : Closed Process " << dwProcessId << endl ;
                    }
                    #endif

                } // Opened process

            } // Not the System Idle Process

        } // for dwProcessIdIndex

    } // Got process identifiers

    return intError ;

} // main
