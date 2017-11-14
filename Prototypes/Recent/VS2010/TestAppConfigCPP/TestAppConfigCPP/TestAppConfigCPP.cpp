// TestAppConfigCPP.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
#include <string>
#include <iomanip>
#include <set>
using namespace std ;

#include <time.h>

#using <mscorlib.dll>

// *************************************************************************************
// *** Ensure that there is also a "Reference" to "System" in the Project References ***
// *************************************************************************************
using namespace System ;
using namespace System::IO ;
using namespace System::Configuration ;

ref class ECCLRBuildSetDetails
{
public :
    ECCLRBuildSetDetails( String ^ strBuildSetSpecification ) : m_bAreValid( false )
    {
        try
        {
            m_strOriginalBuildSetSpecification = strBuildSetSpecification ;
            m_strBuildSetListPathAndFilename = System::Configuration::ConfigurationManager::AppSettings[ m_strBuildSetListFullFilePathConfigKey ] ;
            if ( ! File::Exists( m_strBuildSetListPathAndFilename ) )
            {
                Console::WriteLine( "BuildSetDetails constructor : Build Set List File \"{0}\" does not exist" , m_strBuildSetListPathAndFilename ) ;
            }
            else
            {
                StreamReader ^ buildSetListFileStream = gcnew StreamReader( m_strBuildSetListPathAndFilename ) ;
                System::String ^ buildSetLine = nullptr ;
                int intLineNumber = 0 ;
                while ( ( buildSetLine = buildSetListFileStream->ReadLine() ) != nullptr )
                {
                    intLineNumber += 1 ;
                    if ( buildSetLine->Substring( 0 , 1 ) != %String( "#" ) )
                    {
                        // Not a comment line

                        // Console::WriteLine( "{0} : {1} : \"{2}\"" , m_strBuildSetListPathAndFilename , intLineNumber , buildSetLine ) ;

                        String ^ delimiterString = " " ;
                        array<Char> ^ delimiterCharArray = delimiterString->ToCharArray() ;
                        array<String ^> ^ buildSetDetail = buildSetLine->Split( delimiterCharArray , StringSplitOptions::RemoveEmptyEntries ) ;

                        System::String ^ buildSetIdentifier = buildSetDetail[0] ;
                        System::String ^ buildSetNickname = buildSetDetail[1] ;
                        System::String ^ buildSetSecondaryIdentifier = buildSetDetail[2] ;
                        System::String ^ buildSetCode = buildSetDetail[3] ;
                        System::String ^ buildSetSourceChangeControlType = buildSetDetail[4] ;
                        System::String ^ buildSetServerName = buildSetDetail[5] ;
                        System::String ^ buildSetServerDriveSpecifier = buildSetDetail[6] ;
                        System::String ^ buildSetServerDirectoryName = buildSetDetail[7] ;
                        System::String ^ buildSetOedEnvName = nullptr ;
                        if ( buildSetDetail->Length >= 9 )
                        {
                            buildSetOedEnvName = buildSetDetail[8] ;
                        }

                        if (    ( strBuildSetSpecification == buildSetIdentifier )
                             || ( String::Compare( strBuildSetSpecification , buildSetNickname , true /* ignore case */ ) == 0 )
                           )
                        {
                            // Console::WriteLine( "Found \"{0}\" = \"{1}\" \"{2}\"" , strBuildSetSpecification , buildSetDetail[0] , buildSetDetail[1] ) ;
                            if ( buildSetServerName->ToLower() != CurrentBuildServerName()->ToLower() )
                            {
                                Console::WriteLine( "{0} Build Set \"{1}\" is resident on Build Server \"{2}\" and not on this Build Server \"{3}\"" ,
                                                        System::DateTime::Now ,
                                                        buildSetIdentifier ,
                                                        buildSetServerName->ToUpper() ,
                                                        CurrentBuildServerName()->ToUpper() ) ;
                            }
                            else
                            {
                                m_strIdentifier = buildSetIdentifier ;
                                m_strNickname = buildSetNickname ;
                                m_strSecondaryIdentifier = buildSetSecondaryIdentifier ;
                                m_strCode = buildSetCode ;
                                m_strSourceChangeControlType = buildSetSourceChangeControlType ;
                                m_strBuildSetServerName = buildSetServerName ;
                                m_strBuildSetServerDriveSpecifier = buildSetServerDriveSpecifier ;
                                m_strBuildSetServerDirectoryName = buildSetServerDirectoryName ;
                                // The OEDIPUS Environment might not be specified and hence can be Nothing
                                m_strOedipusEnvironmentName = buildSetOedEnvName ;
                                m_bAreValid = true ;
                                // Display() ;
                            }
                        }

                    } // Not a comment line
                }
                buildSetListFileStream->Close() ;
            }
        }
        catch ( System::Exception ^ exception )
        {
            Console::WriteLine( "ECCLRBuildSetDetails constructor : Exception = \"{0}\"" , exception->ToString() ) ;
        }
    } // ECCLRBuildSetDetails constructor

    void Display( void )
    {
        Display( "ECCLRBuildSetDetails" ) ;
    }

    void Display( System::String ^ strDescription )
    {
        if ( ! m_bAreValid )
        {
            Console::WriteLine( %System::String( "{0} {1} : Original Build Set specification \"{2}\" is not known" ) ,
                                    System::DateTime::Now , strDescription , m_strOriginalBuildSetSpecification ) ;
        }
        else
        {
            Console::WriteLine( "{0} {1} : \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\" \"{8}\" \"{9}\" \"{10}\" \"{11}\"" ,
                                System::DateTime::Now ,
                                strDescription ,
                                Identifier() ,
                                Nickname() ,
                                SecondaryIdentifier() ,
                                Code() ,
                                SourceChangeControlType() ,
                                BuildSetServerName() ,
                                BuildSetServerDriveSpecifier() ,
                                BuildSetServerDirectoryName() ,
                                ( OedipusEnvironmentName() == nullptr ) ? "(unspecified)" : OedipusEnvironmentName() ,
                                BuildSetRootPath() ) ;
        }
    }

    bool AreValid( void )
    {
        return m_bAreValid ;
    }

    System::String ^ BuildSetListPathAndFilename( void )
    {
        // Always return this value
        return m_strBuildSetListPathAndFilename ;
    }

    System::String ^ Identifier( void )
    {
        System::String ^ strIdentifier = nullptr ;
        if ( m_bAreValid )
        {
            strIdentifier = m_strIdentifier ;
        }
        return strIdentifier ;
    }

    System::String ^ Nickname( void )
    {
        System::String ^ strNickname = nullptr ;
        if ( m_bAreValid )
        {
            strNickname = m_strNickname ;
        }
        return strNickname ;
    }

    System::String ^ SecondaryIdentifier( void )
    {
        System::String ^ strSecondaryIdentifier = nullptr ;
        if ( m_bAreValid )
        {
            strSecondaryIdentifier = m_strSecondaryIdentifier ;
        }
        return strSecondaryIdentifier ;
    }

    System::String ^ Code( void )
    {
        return m_strCode ;
        System::String ^ strCode = nullptr ;
        if ( m_bAreValid )
        {
            strCode = m_strCode ;
        }
        return strCode ;
    }

    System::String ^ SourceChangeControlType( void )
    {
        System::String ^ strSourceChangeControlType = nullptr ;
        if ( m_bAreValid )
        {
            strSourceChangeControlType = m_strSourceChangeControlType ;
        }
        return strSourceChangeControlType ;
    }

    System::String ^ BuildSetServerName( void )
    {
        System::String ^ strBuildSetServerName = nullptr ;
        if ( m_bAreValid )
        {
            strBuildSetServerName = m_strBuildSetServerName ;
        }
        return strBuildSetServerName ;
    }

    System::String ^ BuildSetServerDriveSpecifier( void )
    {
        System::String ^ strBuildSetServerDriveSpecifier = nullptr ;
        if ( m_bAreValid )
        {
            strBuildSetServerDriveSpecifier = m_strBuildSetServerDriveSpecifier ;
        }
        return strBuildSetServerDriveSpecifier ;
    }

    System::String ^ BuildSetServerDirectoryName( void )
    {
        System::String ^ strBuildSetServerDirectoryName = nullptr ;
        if ( m_bAreValid )
        {
            strBuildSetServerDirectoryName = m_strBuildSetServerDirectoryName ;
        }
        return strBuildSetServerDirectoryName ;
    }

    System::String ^ BuildSetRootPath( void )
    {
        if ( String::IsNullOrEmpty( m_strBuildSetRootPath ) )
        {
            if (    ( String::IsNullOrEmpty( m_strBuildSetServerDriveSpecifier ) )
                 || ( String::IsNullOrEmpty( m_strBuildSetServerDirectoryName ) )
               )
            {
                m_strBuildSetRootPath = nullptr ;
            }
            else
            {
                String ^ trimString = "" ;
                trimString += Path::DirectorySeparatorChar ;
                array<Char> ^ trimCharArray = trimString->ToCharArray() ;
                m_strBuildSetRootPath = Path::Combine( m_strBuildSetServerDriveSpecifier->Trim( trimCharArray ) + Path::DirectorySeparatorChar ,
                                                       m_strBuildSetServerDirectoryName->Trim( trimCharArray ) + Path::DirectorySeparatorChar ) ;
            }
        }
        return m_strBuildSetRootPath ;
    }

    System::String ^ OedipusEnvironmentName()
    {
        return m_strOedipusEnvironmentName ;
    }

private:

    System::String ^ CurrentBuildServerName( void )
    {
        // return System::Net::Dns::GetHostName() ;
        return "ADEBS04" ;
    }

    bool m_bAreValid ;
    System::String ^ m_strOriginalBuildSetSpecification ;

    System::String ^ m_strIdentifier ;
    System::String ^ m_strNickname ;
    System::String ^ m_strSecondaryIdentifier ;
    System::String ^ m_strCode ;
    System::String ^ m_strSourceChangeControlType ;
    System::String ^ m_strBuildSetServerName ;
    System::String ^ m_strBuildSetServerDriveSpecifier ;
    System::String ^ m_strBuildSetServerDirectoryName ;
    System::String ^ m_strOedipusEnvironmentName ;

    static System::String ^ m_strBuildSetListFullFilePathConfigKey = "BuildSetListFullFilePath" ;
    System::String ^ m_strBuildSetListPathAndFilename ;
    System::String ^ m_strBuildSetRootPath ;

} ; // ECCLRBuildSetDetails

class ECBuildSetDetails
{
public:
    ECBuildSetDetails( const std::string & strBuildSetSpecification ) ;

    void Display( void ) const ;
    void Display( ostream & ostr , const string & strDescription ) const ;

    bool AreValid( void ) const ;
    std::string Identifier( void ) const ;
    std::string Nickname( void ) const ;
    std::string SecondaryIdentifier( void ) const ;
    std::string Code( void ) const ;
    std::string SourceChangeControlType( void ) const ;
    std::string BuildSetServerName( void ) const ;
    std::string BuildSetServerDriveSpecifier( void ) const ;
    std::string BuildSetServerDirectoryName( void ) const ;
    std::string OedipusEnvironmentName( void ) const ;
    std::string BuildSetRootPath( void ) const ;

private:
    std::string MarshalStringToStdString( const String ^ strclr ) ;

    bool m_bAreValid ;
    std::string m_strOriginalBuildSetSpecification ;
    std::string m_strIdentifier ;
    std::string m_strNickname ;
    std::string m_strSecondaryIdentifier ;
    std::string m_strCode ;
    std::string m_strSourceChangeControlType ;
    std::string m_strBuildSetServerName ;
    std::string m_strBuildSetServerDriveSpecifier ;
    std::string m_strBuildSetServerDirectoryName ;
    std::string m_strOedipusEnvironmentName ;

    std::string m_strBuildSetRootPath ;

} ; // ECBuildSetDetails

ECBuildSetDetails::ECBuildSetDetails( const std::string & strBuildSetSpecification ) : m_bAreValid( false )
{
    m_strOriginalBuildSetSpecification = strBuildSetSpecification ;

    ECCLRBuildSetDetails ^ pBuildSetDetailsCLR = gcnew ECCLRBuildSetDetails( %System::String( strBuildSetSpecification.c_str() ) ) ;

    m_bAreValid = pBuildSetDetailsCLR->AreValid() ;
    m_strIdentifier = MarshalStringToStdString( pBuildSetDetailsCLR->Identifier() ) ;
    m_strNickname = MarshalStringToStdString( pBuildSetDetailsCLR->Nickname() ) ;
    m_strSecondaryIdentifier = MarshalStringToStdString( pBuildSetDetailsCLR->SecondaryIdentifier() ) ;
    m_strCode = MarshalStringToStdString( pBuildSetDetailsCLR->Code() ) ;
    m_strSourceChangeControlType = MarshalStringToStdString( pBuildSetDetailsCLR->SourceChangeControlType() ) ;
    m_strBuildSetServerName = MarshalStringToStdString( pBuildSetDetailsCLR->BuildSetServerName() ) ;
    m_strBuildSetServerDriveSpecifier = MarshalStringToStdString( pBuildSetDetailsCLR->BuildSetServerDriveSpecifier() ) ;
    m_strBuildSetServerDirectoryName = MarshalStringToStdString( pBuildSetDetailsCLR->BuildSetServerDirectoryName() ) ;
    m_strOedipusEnvironmentName = MarshalStringToStdString( pBuildSetDetailsCLR->OedipusEnvironmentName() ) ;

    m_strBuildSetRootPath = MarshalStringToStdString( pBuildSetDetailsCLR->BuildSetRootPath() ) ;

} // ECBuildSetDetails::ECBuildSetDetails

void ECBuildSetDetails::Display( void ) const
{
    Display( cout , "ECBuildSetDetails" ) ;
} // ECBuildSetDetails::Display

void ECBuildSetDetails::Display( ostream & ostr , const string & strDescription ) const
{
    const int cintYearBase = 1900 ;

    time_t tmtCurrent = 0 ;
    struct tm tmCurrent = { 0 } ;

    // Get the current time as seconds since time zero
    time( &tmtCurrent ) ;
    localtime_s( &tmCurrent , &tmtCurrent ) ;

    ostr << tmCurrent.tm_year + cintYearBase
         << "/" << setfill('0') << setw( 2 ) << tmCurrent.tm_mon+1
         << "/" << setfill('0') << setw( 2 ) << tmCurrent.tm_mday
         << " " << setfill('0') << setw( 2 ) << tmCurrent.tm_hour
         << ":" << setfill('0') << setw( 2 ) << tmCurrent.tm_min
         << ":" << setfill('0') << setw( 2 ) << tmCurrent.tm_sec
         << setfill(' ') << setw( 0 ) ;

    if ( ! m_bAreValid )
    {
        ostr << " " << strDescription << " : Original Build Set specification \"" << m_strOriginalBuildSetSpecification << "\" is not known" << endl ;
    }
    else
    {
        ostr << " " << strDescription << " :" ;
        ostr << " Identifier = \"" << this->Identifier() << "\"" ;
        ostr << " Nickname = \"" << this->Nickname() << "\"" ;
        ostr << " SecondaryIdentifier = \"" << this->SecondaryIdentifier() << "\"" ;
        ostr << " Code = \"" << this->Code() << "\"" ;
        ostr << " SourceChangeControlType = \"" << this->SourceChangeControlType() ;
        ostr << " BuildSetServerName = \"" << this->BuildSetServerName() << "\"" ;
        ostr << " BuildSetServerDriveSpecifier = \"" << this->BuildSetServerDriveSpecifier() << "\"" ;
        ostr << " BuildSetServerDirectoryName = \"" << this->BuildSetServerDirectoryName() << "\"" ;
        ostr << " OedipusEnvironmentName = \"" << this->OedipusEnvironmentName() << "\"" ;
        ostr << " BuildSetRootPath = \"" << this->BuildSetRootPath() << "\"" ;
        ostr << endl ;
    }
} // ECBuildSetDetails::Display

std::string ECBuildSetDetails::MarshalStringToStdString( const String ^ strclr )
{
    std::string str ;

    if ( strclr != nullptr )
    {
        using namespace Runtime::InteropServices;

        // Get a pointer to the CLR data contained in the CLR string
        const char* pchStringCLR = (const char*)(Marshal::StringToHGlobalAnsi(const_cast<System::String^>(strclr))).ToPointer();

        // Construct a std::string from the CLR data
        str = pchStringCLR ;

        // Release the CLR data
        Marshal::FreeHGlobal(IntPtr((void*)pchStringCLR));
    }

    return str ;

} // ECBuildSetDetails::MarshalStringToStdString

bool ECBuildSetDetails::AreValid( void ) const
{
    return m_bAreValid ;
}

std::string ECBuildSetDetails::Identifier( void ) const
{
    return m_strIdentifier ;
}

std::string ECBuildSetDetails::Nickname( void ) const
{
    return m_strNickname ;
}

std::string ECBuildSetDetails::SecondaryIdentifier( void ) const
{
    return m_strSecondaryIdentifier ;
}

std::string ECBuildSetDetails::Code( void ) const
{
    return m_strCode ;
}

std::string ECBuildSetDetails::SourceChangeControlType( void ) const
{
    return m_strSourceChangeControlType ;
}

std::string ECBuildSetDetails::BuildSetServerName( void ) const
{
    return m_strBuildSetServerName ;
}

std::string ECBuildSetDetails::BuildSetServerDriveSpecifier( void ) const 
{
    return m_strBuildSetServerDriveSpecifier ;
}

std::string ECBuildSetDetails::BuildSetServerDirectoryName( void ) const
{
    return m_strBuildSetServerDirectoryName ;
}

std::string ECBuildSetDetails::OedipusEnvironmentName( void ) const
{
    return m_strOedipusEnvironmentName ;
}

std::string ECBuildSetDetails::BuildSetRootPath( void ) const
{
    return m_strBuildSetRootPath ;
}

int _tmain(int argc, _TCHAR* argv[])
{
    String ^ key = gcnew String( "BuildSetListFullFilePath" ) ;

    String ^ path = System::Configuration::ConfigurationManager::AppSettings[ key ] ;

    Console::WriteLine( "\"{0}\" = \"{1}\"" , key , path ) ;

    ECCLRBuildSetDetails ^ buildSetDetailsCLR1 = gcnew ECCLRBuildSetDetails( "it" ) ;
    buildSetDetailsCLR1->Display( "CLR it" ) ;
    ECCLRBuildSetDetails ^ buildSetDetailsCLR2 = gcnew ECCLRBuildSetDetails( "IntegrationTest" ) ;
    buildSetDetailsCLR1->Display( "CLR IntegrationTest" ) ;
    ECCLRBuildSetDetails ^ buildSetDetailsCLR3 = gcnew ECCLRBuildSetDetails( "poop" ) ;
    buildSetDetailsCLR3->Display( "CLR should be invalid" ) ;

    ECBuildSetDetails buildSetDetails1( "at" ) ;
    buildSetDetails1.Display( cout , "C++ it" ) ;
    ECBuildSetDetails buildSetDetails2( "AcceptanceTest" ) ;
    buildSetDetails2.Display( cout , "C++ AcceptanceTest" ) ;

    ECBuildSetDetails invalid( "poop" ) ;
    invalid.Display( cout , "C++ should be invalid" ) ;

    set<string> set1 ;
    set<string> set2 ;
    set1.insert( set2.begin() , set2.end() ) ;
    cout << "Set items is " << set1.size() << endl ;

	return 0;
}

