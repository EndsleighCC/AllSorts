@setlocal EnableDelayedExpansion

set thing=

dir /s /ad /b ..\ > "%temp%\pathdir.tmp"
call :UpdatePathFrom "%temp%\pathdir.tmp"
del "%temp%\pathdir.tmp"

@echo.
@echo Finally thing="%thing%"

@goto :EOF

@rem
@rem Read the names of all the directories and add them to the path
@rem
:UpdatePathFrom

for /f ^"usebackq^ eol^=^

^ delims^=^" %%a in (%~1) do (
	@echo Before: thing="!thing!"
        @set thing=!thing!;%%a
	@echo After: thing="!thing!"
)

@goto :EOF
