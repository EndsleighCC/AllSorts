setlocal EnableDelayedExpansion

set value=%1

@if "%value%" == "fred" (
call :YesMessage
) else (
call :OtherMessage
)

@goto :EOF

:YesMessage

echo YesMessage : Value has the value "%value%" and is "fred"

@goto :EOF

:OtherMessage

echo OtherMessage : Value isn't "fred" and has the value "%value%"

@goto :EOF
