eisNumBackup test.ut.log
eisNumBackup test.ut.err
bin\Debug\CreateFeatureBranches D:\Repos\HeritageAT AcceptanceTest w:\Master\RelDoc\4-50-01-05\User_Test.ChangeSets.txt u:\ 1>test.ut.log 2>test.ut.err
@if errorlevel 1 (echo AcceptanceTest Feature branches were created successfully) else (echo AcceptanceTest Feature branches failed to be created)
