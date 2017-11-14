@rem @echo off
@setlocal EnableDelayedExpansion

@set BuildEnvironmentFilename=BuildSetList.cfg

@call :ProcessSelectedBuildSet "!BuildEnvironmentFilename!" "*"

@call :ProcessSelectedBuildSet "!BuildEnvironmentFilename!" "IntegrationTest2"
@call :ProcessSelectedBuildSet "!BuildEnvironmentFilename!" "IntegrationTest"
@call :ProcessSelectedBuildSet "!BuildEnvironmentFilename!" "bunnies"

@goto :EOF

:ProcessSelectedBuildSet
@rem ***********************************************************************************
@rem
@rem Process the specified Build Set
@rem    %1 = Filename containing all Build Set descriptions
@rem    %2 = The Build Set Identifier or Build Set Nickname for the Build Set to be
@rem         processed
@rem         If this parameter is * then all Build Sets will be processed
@rem
@rem ***********************************************************************************

@echo.
@echo ProcessSelectedBuildSet : Begin : Filename="%~1" BuildSet="%~2"

@set /a ProcessSelectedBuildSetLineNumber=0
@set FileLine=

:ProcessSelectedBuildSetLoopTop

    @call :ReadFileLine "%~1" !ProcessSelectedBuildSetLineNumber!
    @if not "!FileLine!" == "" (

        @rem @echo.
        @rem @echo Line "!ProcessSelectedBuildSetLineNumber!" = "!FileLine!"

        @if not "!FileLine:~0,1!" == "#" @(

            @for /f "tokens=1-10 delims== " %%a in ("!FileLine!") do @(
                @echo.
                @echo BuildSetIdentifier=%%a, BuildSetNickname=%%b, BuildSetSecondaryIdentifier=%%c BuildSetCode=%%d, BuildSetSourceChangeType=%%e, BuildServerName=%%f, BuildSetDriveSpec=%%g, BuildSetDirectory=%%h, BuildSetDplySrvLoc=%%i, BuildSetOEDEnvName=%%j

                @if "%~2" == "*" (
                    @rem Process this Build Set unconditionally
                    @call :ProcessBuildSet "%%a" "%%b" "%%c" "%%d" "%%e" "%%f" "%%g" "%%h" "%%i" "%%j"
                ) else (
                    @rem If the supplied case sensitive Build Set specification matches the Build Set Identifier then process it
                    @if "%%a" == "%~2" (
                        @call :ProcessBuildSet "%%a" "%%b" "%%c" "%%d" "%%e" "%%f" "%%g" "%%h" "%%i" "%%j" &goto ProcessSelectedBuildSetExit
                    ) else (
                        @rem If the supplied case insensitive Build Set specification matches the Build Set Nickname then process it
                        @if /i "%%b" == "%~2" @call :ProcessBuildSet "%%a" "%%b" "%%c" "%%d" "%%e" "%%f" "%%g" "%%h" "%%i" "%%j"&goto ProcessSelectedBuildSetExit
                    )
                )
            )
        )
        @set /a ProcessSelectedBuildSetLineNumber+=1
    )

@if not "!FileLine!" == "" goto ProcessSelectedBuildSetLoopTop

@if not "%~2" == "*" (
    @echo.
    @echo ProcessSelectedBuildSet : Unable to identify a Build Set with Build Set specification "%~2"
)

:ProcessSelectedBuildSetExit

@echo.
@echo ProcessSelectedBuildSet : End : Filename="%~1" BuildSet="%~2"

@goto :EOF

:ReadFileLine
@rem ***********************************************************************************
@rem
@rem Reads the specified line from the specified file
@rem    %1 = Filename
@rem    %2 = Integer line number
@rem
@rem ***********************************************************************************

@set /a ReadFileLineNumber=0
@set FileLine=

@for /f ^"usebackq^ eol^=^

^ delims^=^" %%a in (%~1) do @(
        @if "!ReadFileLineNumber!"=="%~2" set FileLine=%%a&goto :EOF
        @set /a ReadFileLineNumber+=1
)

@goto :EOF

:ProcessBuildSet
@rem
@rem Process the Build Set with the following parameters

set a1=%~1
set a2=%~2
set a3=%~3
set a4=%~4
set a5=%~5
set a6=%~6
set a7=%~7
set a8=%~8
set a9=%~9
shift
set a10=%~9

@echo.
@echo ProcessBuildSet : "%a1%" "%a2%" "%a3%" "%a4%" "%a5%" "%a6%" "%a7%" "%a8%" "%a9%" "%a10%"

@goto :EOF

:ShowUsage
@echo Usage: Filename LineIndex
