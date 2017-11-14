@setlocal EnableDelayedExpansion

@set BuildSetDriveSpec=\\adeoc02\tms instances
@set BuildSetDirectoryName=Development

@call :GetAllTMSVersions

@set OriginalTMSMotorVersion=!TMSMotorVersion!
@set OriginalTMSHouseholdVersion=!TMSHouseholdVersion!
@set OriginalTMSPossessionsVersion=!TMSPossessionsVersion!
@set OriginalTMSIncomeVersion=!TMSIncomeVersion!

@echo.
@echo Original TMS Motor Version is "!OriginalTMSMotorVersion!"
@echo Original TMS Household Version is "!OriginalTMSHouseholdVersion!"
@echo Original TMS Possessions Version is "!OriginalTMSPossessionsVersion!"
@echo Original TMS Income Version is "!OriginalTMSIncomeVersion!"
@echo.

:PollLoopTop
@call :GetAllTMSVersions
@echo Pickup TMS Motor Version is "!TMSMotorVersion!"
@echo Pickup TMS Household Version is "!TMSHouseholdVersion!"
@echo Pickup TMS Possessions Version is "!TMSPossessionsVersion!"
@echo Pickup TMS Income Version is "!TMSIncomeVersion!"
@if not "!TMSMotorVersion!" == "!OriginalTMSMotorVersion!" goto PollLoopExit
@if not "!TMSHouseholdVersion!" == "!OriginalTMSHouseholdVersion!" goto PollLoopExit
@if not "!TMSPossessionsVersion!" == "!OriginalTMSPossessionsVersion!" goto PollLoopExit
@if not "!TMSIncomeVersion!" == "!OriginalTMSIncomeVersion!" goto PollLoopExit
@echo Waiting for one or more TMS Versions to change ...
@sleep 10
@goto PollLoopTop

:PollLoopExit

@echo.
@echo One or more TMS Versions have been updated
@echo.
@if not "!TMSMotorVersion!" == "!OriginalTMSMotorVersion!" echo Motor TMS has changed to "!TMSMotorVersion!"
@if not "!TMSHouseholdVersion!" == "!OriginalTMSHouseholdVersion!" echo Household TMS has changed to "!TMSHouseholdVersion!"
@if not "!TMSPossessionsVersion!" == "!OriginalTMSPossessionsVersion!" echo Possessions TMS has changed to "!TMSPossessionsVersion!"
@if not "!TMSIncomeVersion!" == "!OriginalTMSIncomeVersion!" echo Income TMS has changed to "!TMSIncomeVersion!"

@goto :EOF

:GetAllTMSVersions
@rem ***********************************************************************************
@rem
@rem Get all TMS versions
@rem
@rem ***********************************************************************************

@set TMSRiskType=Motor
@call :GetTMSVersion
@set TMSMotorVersion=%MajorVersion%.%MinorVersion%

@set TMSRiskType=Property
@call :GetTMSVersion
@set TMSHouseholdVersion=%MajorVersion%.%MinorVersion%

@set TMSRiskType=Possessions
@call :GetTMSVersion
@set TMSPossessionsVersion=%MajorVersion%.%MinorVersion%

@set TMSRiskType=Income
@call :GetTMSVersion
@set TMSIncomeVersion=%MajorVersion%.%MinorVersion%

@goto :EOF

:GetTMSVersion
@rem ***********************************************************************************
@rem
@rem Get the version of the specified TMS
@rem
@rem ***********************************************************************************
@set MajorVersion=??
@set MinorVersion=???
@if not exist "!BuildSetDriveSpec!\!BuildSetDirectoryName!\tms\%TMSRiskType%\tmsver.dat" goto SkipGetTMSVersion
@for /f "tokens=1-6* delims=[.] " %%a in ('type "!BuildSetDriveSpec!\!BuildSetDirectoryName!\tms\%TMSRiskType%\tmsver.dat"') do @set Token1=%%a& @set Token2=%%b& @set Token3=%%c& set @Token4=%%d& @set Token5=%%e& @set Token6=%%f
@set MajorVersion=%Token5%
@set MinorVersion=%Token6%

:SkipGetTMSVersion
@goto :EOF
