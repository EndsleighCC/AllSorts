@if "%1" == "" goto :EOF
eisnumbackup test.log
bin\debug\testregistry %1 > test.log
