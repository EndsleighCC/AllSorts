@setlocal
cd ..\Heritage

@rem Add the remote TFS Repository escaping with double percentage signs because this is a batch file
git remote add origin http://adetfs05:8080/tfs/Test/_git/Heritage

@rem Push the complete contents of all Branches to the TFS Repository and add tracking references
git push -u origin --all

@endlocal
