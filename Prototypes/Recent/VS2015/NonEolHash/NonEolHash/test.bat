bin\Debug\NonEolHash.exe test1.txt test2.txt test3.txt test4.txt
@if errorlevel 1 ( echo Something was different ) else (echo All the same)
bin\Debug\NonEolHash.exe test1.txt test3.txt
@if errorlevel 1 ( echo Something was different ) else (echo All the same)
