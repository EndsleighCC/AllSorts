@rem @echo off
@setlocal EnableDelayedExpansion

@set BuildEnvironmentFilename=BuildSetList.config

@call :ProcessAllBuildSets !BuildEnvironmentFilename!

@call :ProcessSelectedBuildSet !BuildEnvironmentFilename! IntegrationTest2
@call :ProcessSelectedBuildSet !BuildEnvironmentFilename! System_Test

@goto :EOF

@rem
@rem Process all Build Sets specified by the filename in parameter 1
@rem
:ProcessAllBuildSets

@echo.
@echo ProcessAllBuildSets : Begin

@set /a ProcessAllBuildSetsLineNumber=0
@set FileLine=

:ProcessAllBuildSetsLoopTop

    @call :ReadFileLine !BuildEnvironmentFilename! !ProcessAllBuildSetsLineNumber!
    @if not "!FileLine!" == "" (

        @rem @echo.
        @rem @echo Line "!ProcessAllBuildSetsLineNumber!" = "!FileLine!"

        @if not "!FileLine:~0,1!" == "#" @(

            @for /f "tokens=1-6 delims== " %%a in ("!FileLine!") do @(
                @echo.
                @echo BuildSetIdentifier=%%a, BuildSetCode=%%b, BuildSetSourceChangeType=%%c, BuildServer=%%d, BuildSetDirectory=%%e

                @call :ProcessBuildSet %%a %%b %%c %%d %%e

            )
        )
        @set /a ProcessAllBuildSetsLineNumber+=1
    )

@if not "!FileLine!" == "" goto ProcessAllBuildSetsLoopTop

@echo.
@echo ProcessAllBuildSets : End

@goto :EOF

@rem
@rem From the filename (parameter 1) process the specified Build Set (parameter 2)
@rem
:ProcessSelectedBuildSet

@echo.
@echo ProcessSelectedBuildSet : Begin : Filename=%1 BuildSet=%2

@set /a ProcessSelectedBuildSetLineNumber=0
@set FileLine=

:ProcessSelectedBuildSetLoopTop

    @call :ReadFileLine !BuildEnvironmentFilename! !ProcessSelectedBuildSetLineNumber!
    @if not "!FileLine!" == "" (

        @rem @echo.
        @rem @echo Line "!ProcessSelectedBuildSetLineNumber!" = "!FileLine!"

        @if not "!FileLine:~0,1!" == "#" @(

            @for /f "tokens=1-6 delims== " %%a in ("!FileLine!") do @(
                @rem echo BuildSetIdentifier=%%a, BuildSetCode=%%b, BuildSetSourceChangeType=%%c, BuildServer=%%d, BuildSetDirectory=%%e

                @rem If the supplied Build Set matches then process it
                @if /i "%%a" == "%2" @call :ProcessBuildSet %%a %%b %%c %%d %%e&goto ProcessSelectedBuildSetExit

            )
        )
        @set /a ProcessSelectedBuildSetLineNumber+=1
    )

@if not "!FileLine!" == "" goto ProcessSelectedBuildSetLoopTop

:ProcessSelectedBuildSetExit

@echo.
@echo ProcessSelectedBuildSet : End : Filename=%1 BuildSet=%2

@goto :EOF

@rem
@rem Process the Build Set with the following parameters
@rem    BuildSetIdentifier
@rem    BuildSetCode
@rem    BuildSourceChangeType
@rem    BuildServer
@rem    BuildSetDirectory
:ProcessBuildSet

@echo.
@echo ProcessBuildSet : %1 %2 %3 %4 %5

@goto :EOF

@rem
@rem From the specified file (parameter 1) reads the specified line (paremeter 2)
@rem
:ReadFileLine

@set /a ReadFileLineNumber=0
@set FileLine=

@for /f ^"usebackq^ eol^=^

^ delims^=^" %%a in (%1) do @(
        @if "!ReadFileLineNumber!"=="%2" set FileLine=%%a&goto :EOF
        @set /a ReadFileLineNumber+=1
)

@goto :EOF

:ShowUsage
@echo Usage: Filename LineIndex
