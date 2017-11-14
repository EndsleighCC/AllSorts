@echo off
@rem ------------------------------------------------------------------------
@rem
@rem  ShowVersions.bat - Display the System Version and TMS versions
@rem
@rem  Parameters:
@rem
@rem   The path from the versions are to be extracted
@rem
@rem ------------------------------------------------------------------------
@rem AUTHOR   : C. Cornelius                      CREATION DATE: 31-Jul-2017
@rem ------------------------------------------------------------------------
@rem PVCS SECTION :                                                        
@rem ~~~~~~~~~~~~~~
@rem
@rem PVCS FILENAME: $Logfile:   Z:\sda\Cmd\ShowVersions.bat  $
@rem PVCS REVISION: $Revision:   1.0  $
@rem
@rem $Log:   Z:\sda\Cmd\ShowVersions.bat  $
@rem 
@rem    Rev 1.0   Jul 31 2017 11:36:14   corc1
@rem 
@rem  inet OS Transition
@rem 
@rem  Initial revision
@rem
@rem ------------------------------------------------------------------------
@echo on

@if "%~1" == "" call :ShowUsage&goto :EOF

@setlocal EnableDelayedExpansion

@echo.
@echo Specified path is "%~1"

@call :GetSystemVersion "%~1"
@echo.
@echo System Version is "%SystemVersion%"

@call :GetInternalVersion "%~1"
@echo.
@echo Internal Version is "%InternalVersion%"

@call :GetTmsVersions "%~1"
@echo.
@echo TMS Motor Version "%TmsMotorVersion%"
@echo TMS Household Version "%TmsHouseholdVersion%"
@echo TMS Possessions Version "%TmsPossessionsVersion%"
@echo TMS Income Version "%TmsIncomeVersion%"

@endlocal

@goto :EOF

:ShowUsage
@rem ***********************************************************************************
@rem
@rem Show usage
@rem
@rem ***********************************************************************************

@echo.
@echo Usage: ShowVersions path
@echo.
@echo Function: Display the System Version, Internal Version and all TMS versions in the specified path

@goto :EOF

:GetSystemVersion
@rem ***********************************************************************************
@rem
@rem Get the System Version from the System Version File
@rem
@rem %1 = The root directory of the Source Volume
@rem
@rem ***********************************************************************************

@set SystemVersionFilename=%~1\INETBase\Code\ECOMINETVersion\ECOMINETVersion.rc

@set SystemVersion=

@FOR /f "eol=; tokens=1-3" %%a in (%SystemVersionFilename%) do @call :ProcessSystemVersionLine %%a %%b %%c

@goto :EOF

:ProcessSystemVersionLine
@rem ***********************************************************************************
@rem
@rem Processs the token associated with a line from the System Control File
@rem    %1 = First token on the file line
@rem    %2 = Second token from the file line
@rem    %3 = Third token from the file line
@rem
@rem ***********************************************************************************

@if "%~1" == "VALUE" (
    if "%~2" == "ProductVersion" (
        @set SystemVersion=%~3
    )
)

@goto :EOF

:GetInternalVersion
@rem ***********************************************************************************
@rem
@rem Get the Internal Version from the System Version File
@rem
@rem %1 = The root directory of the Source Volume
@rem
@rem ***********************************************************************************

@set InternalVersionFilename=%~1\INETBase\Code\ECOMINETVersion\ECOMINETVersion.rc

@set SystemVersion=

@FOR /f "eol=; tokens=1-3" %%a in (%InternalVersionFilename%) do @call :ProcessInternalVersionLine %%a %%b %%c

@goto :EOF

:ProcessInternalVersionLine
@rem ***********************************************************************************
@rem
@rem Processs the token associated with a line from the Version Control File
@rem    %1 = First token on the file line
@rem    %2 = Second token from the file line
@rem    %3 = Third token from the file line
@rem
@rem ***********************************************************************************

@if "%~1" == "VALUE" (
    if "%~2" == "SpecialBuild" (
        @set InternalVersion=%~3
    )
)

@goto :EOF

:GetTmsVersions
@rem ***********************************************************************************
@rem
@rem Get the TMS Versions from the appropriate places
@rem
@rem %1 = The root directory of the Source Volume
@rem
@rem ***********************************************************************************

@rem Motor
@call :GetTmsVersion "%~1\TMS\Motor\TMSVER.DAT"
@set TmsMotorVersion=%TmsVersion%

@rem Household
@call :GetTmsVersion "%~1\TMS\Property\TMSVER.DAT"
@set TmsHouseholdVersion=%TmsVersion%

@rem Possessions
@call :GetTmsVersion "%~1\TMS\Possessions\TMSVER.DAT"
@set TmsPossessionsVersion=%TmsVersion%

@rem Income
@call :GetTmsVersion "%~1\TMS\Income\TMSVER.DAT"
@set TmsIncomeVersion=%TmsVersion%

@goto :EOF

:GetTmsVersion
@rem ***********************************************************************************
@rem
@rem Get the TMS Version from the specified file
@rem
@rem ***********************************************************************************

@set /p VersionText=<"%~1"

@for /f "tokens=5-6 delims=." %%a in ("%VersionText%") do @(
    set TmsMajorVersion=%%a
    set TmsMinorVersion=%%b
)
@set TmsVersion=%TmsMajorVersion%.%TmsMinorVersion%

@goto :EOF
