@rem tsleep until=06:30:00
eisNumBackup Logs\comp.at.log
eisNumBackup Logs\comp.at.err
bin\Debug\CompareSourceVolumes "\\ADEBS03\SysPROD\Master\RelDoc" \\adebs02\SysUT00 d:\Repos\Heritage 1>Logs\comp.at.log 2>Logs\comp.at.err
