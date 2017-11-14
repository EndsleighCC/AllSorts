bin\Debug\CompareSolutions.exe Endsleigh/Legacy/Build/Database/Endsleigh.Legacy.Build.Database.sln \\adebs03\SysPR00 \\adebs04\SysST00

bin\Debug\CompareSolutions.exe Endsleigh/Legacy/Build/Base/Endsleigh.Legacy.Build.Base.sln \\adebs03\SysPR00 \\adebs04\SysST00
@if errorlevel 1 (echo They are different) else (echo They are the same)
bin\Debug\CompareSolutions.exe Endsleigh/Legacy/Build/Base/Endsleigh.Legacy.Build.Base.sln \\adebs03\SysPR00 \\shnsdw26\ST00
