@echo off
@setlocal EnableDelayedExpansion

set BuildEnvironmentFilename=BuildSetList.config

call :ProcessAllBranches !BuildEnvironmentFilename!

@goto :EOF

@rem
@rem Process all Branches specified by the filename in parameter 1
@rem
:ProcessAllBranches

set /a LineNumber=0
set FileLine=

:LoopTop

    call :ReadLine !BuildEnvironmentFilename! !LineNumber!
    if not "!FileLine!" == "" (

        @rem @echo.
        @rem @echo Line "!LineNumber!" = "!FileLine!"

        if not "!FileLine:~0,1!" == "#" (

            for /f "tokens=1-6 delims== " %%a in ("!FileLine!") do (
                echo BuildSetIdentifier=%%a, BuildSetCode=%%b, SourceChangeType=%%c, BuildServer=%%d, Directory=%%e
            )
        )
        set /a LineNumber+=1
    )

if not "!FileLine!" == "" goto LoopTop

goto :EOF

@rem
@rem From the specified file (parameter 1) reads the specified line (paremeter 2)
@rem
:ReadLine

set /a counter=0
set FileLine=

for /f ^"usebackq^ eol^=^

^ delims^=^" %%a in (%1) do (
        if "!counter!"=="%2" set FileLine=%%a&goto :EOF
        set /a counter+=1
)

goto :EOF

:ShowUsage
@echo Usage: Filename LineIndex
