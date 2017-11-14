// Utility for trimming the output from a debug log

#include "stdafx.h"

#include <string>
#include <algorithm>
#include <iostream>
#include <fstream>
using namespace std ;

#include <stdio.h>

void TidyString( string & str )
{
    // IBMi output does not contain double quotes
    std::replace( str.begin() , str.end() , '\"' , ' ' ) ;
    // IBMi output includes tabs
    std::replace( str.begin() , str.end() , '\t' , ' ' ) ;
    // IBMi output does not have commas because it came from a CSV
    std::replace( str.begin() , str.end() , ',' , ' ' ) ;
    // Wretched Pound signs
    std::replace( str.begin() , str.end() , 'œ' , '£' ) ;
} // TidyString

void TrimEISDBGLOG( const string strInputFilename )
{
    ifstream inFile ;

    inFile.open( strInputFilename.c_str() ) ;
    if ( ! inFile )
    {
        cout << "Unable to open input file \"" << strInputFilename << "\"" << endl ;
    }
    else
    {
        // Stream is open

        string strFileLine ;
        while ( ! getline( inFile , strFileLine ).eof() )
        {
            string strDisplayLine ;
            // EISDBG.LOG is 
            string::size_type sstSeparator = strFileLine.find_first_of( ">" ) ;
            if ( sstSeparator != string::npos )
            {
                strDisplayLine = strFileLine.substr( sstSeparator + 2 /* "> " */ ) ;
            }
            else
            {
                strDisplayLine = strFileLine ;
            }

            TidyString( strDisplayLine ) ;

            cout << strDisplayLine << endl ;

        } // while

    } // Stream is open

} // TrimEISDBGLOG

void TrimDebugUpToCharacter( const string & strInputFilename , int intLeftTrimCount )
{
    ifstream inFile ;

    inFile.open( strInputFilename.c_str() ) ;
    if ( ! inFile )
    {
        cout << "Unable to open input file \"" << strInputFilename << "\"" << endl ;
    }
    else
    {
        string strFileLine ;
        while ( ! getline( inFile , strFileLine ).eof() )
        {
            string strDisplayLine = strFileLine.substr( intLeftTrimCount ) ;
            TidyString( strDisplayLine ) ;
            cout << strDisplayLine << endl ;
        } // while
    }

} // TrimDebugUpToCharacter

int _tmain(int argc, _TCHAR* argv[])
{
    int intError = 0 ;

    if ( argc <= 1 )
    {
        cout << "eisTrimDebug {/l:n} InputfileName" << endl ;
    }
    else
    {
        const char *pcszInputFilename = NULL ;

        enum EDebugType
        {
            edtEISDBG ,
            edtOther
        } ;
        
        EDebugType debugType = edtEISDBG ;

        int intLeftTrimCount = 0 ;

        int intArgId = 0 ;
        const char *pcszArgValue = NULL ;

        for ( intArgId = 1 , pcszArgValue = argv[ intArgId ] ;
              intArgId < argc ;
              ++intArgId , pcszArgValue = pcszArgValue = argv[ intArgId ]
            )
        {
            if ( *pcszArgValue == '/' )
            {
                // A switch

                switch ( tolower( pcszArgValue[1] ) )
                {
                case 'l' :
                    if ( pcszArgValue[2] == ':' )
                    {
                        intLeftTrimCount = atoi( &pcszArgValue[3] ) ;
                        debugType = edtOther ;
                        cerr << "Left trim count is " << intLeftTrimCount << " characters" << endl ;
                    }
                    else
                    {
                        cerr << "No left trim count supplied" << endl ;
                    }
                    break;
                default :
                    cerr << "Unknown switch \"" << pcszArgValue << "\"" << endl ;
                    break ;
                } // switch

            } // A switch
            else
            {
                // An argument

                if ( pcszInputFilename == NULL )
                {
                    pcszInputFilename = pcszArgValue ;
                    cerr << "Input filename is \"" << pcszInputFilename << "\"" << endl ;
                }

            } // An argument

        } // for intArgId

        if ( pcszInputFilename == NULL )
        {
            cout << "No input filename was specified" << endl ;
        }
        else
        {
            // Got an input filename

            switch ( debugType )
            {
            case edtEISDBG :
                TrimEISDBGLOG( pcszInputFilename ) ;
                break ;
            case edtOther :
                TrimDebugUpToCharacter( pcszInputFilename , intLeftTrimCount ) ;
                break ;
            } // switch

        } // Got an input filename

    }

	return intError ;

} // _tmain
