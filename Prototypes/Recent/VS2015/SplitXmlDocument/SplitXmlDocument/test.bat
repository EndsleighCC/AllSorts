eisNumBackup Logs\quoteBatches.log
eisNumBackup Logs\quoteBatches.err
bin\Debug\SplitXmlDocument.exe %1 %2 XmlDocuments\AGG09102017.xml 1>Logs\quoteBatches.log 2>Logs\quoteBatches.err
@if not errorlevel 1 (@echo Split Succeeded) else (@echo Split Failed)
