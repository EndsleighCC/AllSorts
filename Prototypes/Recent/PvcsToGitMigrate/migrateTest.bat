@if "%1" == "" echo No Repository operation supplied&goto :EOF
@if /i "%1" == "push" goto GitPushOnly
@if /i "%1" == "existing" set UseExistingRepository=yes
@if /i "%1" == "new" set UseExistingRepository=no
@if "%UseExistingRepository%" == "" echo New or existing Repository not specified&goto :EOF

@rem tsleep until=06:30:00

eisNumBackup Logs\MigrationReportTest.log
eisNumBackup Logs\MigrationReportTest.err

@set GitRepositoryName=Heritage
@set GitRepositoryDirectory=d:\Repos\%GitRepositoryName%T
@set UseExistingGitRepositorySwitch=

@rem 
@if /i "%UseExistingRepository%" == "yes" set UseExistingGitRepositorySwitch=/e&goto SkipRemoveExistingGitRepository

@rem Delete any existing git Repository
rd %GitRepositoryDirectory% /s/q 1>>Logs\MigrationReportTest.log 2>>Logs\MigrationReportTest.err

@rem Create the directory to contain the git Repository
md %GitRepositoryDirectory%

:SkipRemoveExistingGitRepository

@rem Import the PVCS sources into a single new or existing Git Repository
@rem Pass parameters from the command line on to the utility in case they are additional switches
PvcsToGitMigrate\PvcsToGitMigrate\bin\Debug\PvcsToGitMigrate.exe w:\Master\RelDoc "%GitRepositoryDirectory%" %UseExistingGitRepositorySwitch% %~2 %~3 %~4 %~5 1>>Logs\MigrationReportTest.log 2>>Logs\MigrationReportTest.err
@if errorlevel 1 @echo Migration failed & goto :EOF

:GitPushOnly
@rem @echo Pushing to origin

@rem Operate inside the Git Repository

@setlocal
cd /d %GitRepositoryDirectory%

@rem Add the remote TFS Repository escaping with double percentage signs because this is a batch file
@rem Test:
@rem git remote add origin http://adetfs05:8080/tfs/Test/_git/%GitRepositoryName%
@rem Actual:
@rem git remote add origin http://adetfs05:8080/tfs/Endsleigh%%20Products/_git/%GitRepositoryName% | unix2dos 1>>..\PvcsToGitMigrate\Logs\MigrationReportTest.log 2>>..\PvcsToGitMigrate\Logs\MigrationReportTest.err

@rem Push the complete contents of all Branches to the TFS Repository and add tracking references
@rem git push -u origin --all | unix2dos 1>>..\PvcsToGitMigrate\Logs\MigrationReportTest.log 2>>..\PvcsToGitMigrate\Logs\MigrationReportTest.err

@endlocal
