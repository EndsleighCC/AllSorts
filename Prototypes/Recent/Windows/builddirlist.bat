@rem
@rem Ensure that
@rem     [HKLM\Software\Microsoft\Command Processor]
@rem  or [HKCU\Software\Microsoft\Command Processor]
@rem has the REG_DWORD value 1
@rem Using this construct
@rem     setlocal EnableDelayedExpansion
@rem will not work as this batch file would not be able to communicate
@rem its changes to the outer environment
@rem

@set DirectoryTreeTop=..\VS2015\MakeUnused

@dir /s /ad /b %DirectoryTreeTop% > "%temp%\pathdir.tmp"
@call :UpdatePathFrom "%temp%\pathdir.tmp"
@del "%temp%\pathdir.tmp"

@echo.
@echo Finally pathadd="%pathadd%"

@rem To augment the PATH remove @echo from the start of the following line
@echo PATH=%PATH%;"%pathadd%"

@goto :EOF

@rem ----------------------------------------------------------------
@rem
@rem Batch file function to read the names of all the sub-directories
@rem of the specified tree (parameter 1) and add them to the path
@rem
@rem ----------------------------------------------------------------
:UpdatePathFrom

@set pathadd=

@for /f ^"usebackq^ eol^=^

^ delims^=^" %%a in (%~1) do @(
	@rem @echo Before: pathadd="!pathadd!"
        @set pathadd=!pathadd!;%%a
	@rem @echo After: pathadd="!pathadd!"
)

@goto :EOF
