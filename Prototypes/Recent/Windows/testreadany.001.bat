@rem @echo off
@setlocal EnableDelayedExpansion

@set BuildEnvironmentFilename=BuildSetList.config

@call :ProcessAllBuildSets !BuildEnvironmentFilename!

@call :ProcessSelectedBuildSet !BuildEnvironmentFilename! IntegrationTest2
@call :ProcessSelectedBuildSet !BuildEnvironmentFilename! IntegrationTest
@call :ProcessSelectedBuildSet !BuildEnvironmentFilename! bunnies

@goto :EOF

:ProcessAllBuildSets
@rem ***********************************************************************************
@rem
@rem Process all Build Sets
@rem    %1 = Filename containing all Build Set descriptions
@rem
@rem ***********************************************************************************

@echo.
@echo ProcessAllBuildSets : Begin

@set /a ProcessAllBuildSetsLineNumber=0
@set FileLine=

:ProcessAllBuildSetsLoopTop

    @call :ReadFileLine %1 !ProcessAllBuildSetsLineNumber!
    @if not "!FileLine!" == "" (

        @rem @echo.
        @rem @echo Line "!ProcessAllBuildSetsLineNumber!" = "!FileLine!"

        @if not "!FileLine:~0,1!" == "#" @(

            @for /f "tokens=1-6 delims== " %%a in ("!FileLine!") do @(
                @echo.
                @echo BuildSetIdentifier=%%a, BuildSetSecondaryIdentifier=%%b BuildSetCode=%%c, BuildSetSourceChangeType=%%d, BuildServer=%%e, BuildSetDirectory=%%f

                @call :ProcessBuildSet %%a %%b %%c %%d %%e %%f

            )
        )
        @set /a ProcessAllBuildSetsLineNumber+=1
    )

@if not "!FileLine!" == "" goto ProcessAllBuildSetsLoopTop

@echo.
@echo ProcessAllBuildSets : End

@goto :EOF

:ProcessSelectedBuildSet
@rem ***********************************************************************************
@rem
@rem Process the specified Build Set
@rem    %1 = Filename containing all Build Set descriptions
@rem    %2 = The Build Set Identifier for the Build Set to be processed
@rem
@rem ***********************************************************************************

@echo.
@echo ProcessSelectedBuildSet : Begin : Filename=%1 BuildSet=%2

@set /a ProcessSelectedBuildSetLineNumber=0
@set FileLine=

:ProcessSelectedBuildSetLoopTop

    @call :ReadFileLine %1 !ProcessSelectedBuildSetLineNumber!
    @if not "!FileLine!" == "" (

        @rem @echo.
        @rem @echo Line "!ProcessSelectedBuildSetLineNumber!" = "!FileLine!"

        @if not "!FileLine:~0,1!" == "#" @(

            @for /f "tokens=1-6 delims== " %%a in ("!FileLine!") do @(
                @echo.
                @echo BuildSetIdentifier=%%a, BuildSetSecondaryIdentifier=%%b BuildSetCode=%%c, BuildSetSourceChangeType=%%d, BuildServer=%%e, BuildSetDirectory=%%f

                @rem If the supplied Build Set matches then process it
                @if /i "%%a" == "%2" @call :ProcessBuildSet %%a %%b %%c %%d %%e %%f&goto ProcessSelectedBuildSetExit

            )
        )
        @set /a ProcessSelectedBuildSetLineNumber+=1
    )

@if not "!FileLine!" == "" goto ProcessSelectedBuildSetLoopTop

@echo.
@echo ProcessSelectedBuildSet : Unable to identify a Build Set with Build Set Identifier "%2"

:ProcessSelectedBuildSetExit

@echo.
@echo ProcessSelectedBuildSet : End : Filename=%1 BuildSet=%2

@goto :EOF

@rem
@rem Process the Build Set with the following parameters
@rem    BuildSetIdentifier
@rem    BuildSetSecondaryIdentifier
@rem    BuildSetCode
@rem    BuildSourceChangeType
@rem    BuildServer
@rem    BuildSetDirectory
:ProcessBuildSet

@echo.
@echo ProcessBuildSet : %1 %2 %3 %4 %5 %6

@goto :EOF

:ReadFileLine
@rem ***********************************************************************************
@rem
@rem From the specified file (parameter 1) reads the specified line (paremeter 2)
@rem
@rem ***********************************************************************************

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
