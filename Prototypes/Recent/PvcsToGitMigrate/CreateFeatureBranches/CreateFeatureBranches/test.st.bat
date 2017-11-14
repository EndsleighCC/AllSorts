eisNumBackup test.st.log
eisNumBackup test.st.err
bin\Debug\CreateFeatureBranches D:\Repos\HeritageAT IntegrationTest w:\Master\RelDoc\4-50-01-05\System_Test.ChangeSets.txt g:\ 1>test.st.log 2>test.st.err
@if errorlevel 1 (echo IntegrationTest Feature branches were created successfully) else (echo IntegrationTest Feature branches failed to be created)
