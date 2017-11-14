@if "%~1" == "" goto :EOF

@echo.
@echo Ensure that the Git branch from which "%~1" emmanates has been checked out before pressing enter
@pause

@set GitRepositoryName=Heritage
@set GitRepositoryDirectory=d:\Repos\%GitRepositoryName%T
@set UseExistingGitRepositorySwitch=/e

eisNumBackup Logs\MigrationOneReport.log
eisNumBackup Logs\MigrationOneReport.err

PvcsToGitMigrate\PvcsToGitMigrate\bin\Debug\PvcsToGitMigrate.exe w:\Master\RelDoc "%GitRepositoryDirectory%" %UseExistingGitRepositorySwitch% /i:%~1 %~2 %~3 %~4 %~5 1>>Logs\MigrationOneReport.log 2>>Logs\MigrationOneReport.err
