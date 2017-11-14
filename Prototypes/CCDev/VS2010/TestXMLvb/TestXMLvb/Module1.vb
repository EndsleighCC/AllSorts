Imports System.DirectoryServices
Imports System.Environment
Imports System.Xml
Imports System.IO
Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.Collections.Generic

Module Module1

    Sub TestXMLBuildEnvironment()

        Dim XMLBuildEnvironmentFile As System.Xml.XmlDocument
        Dim BuildEnvironmentLocation As String = "C:\ST04\Endsleigh\Build\Console\buildenvironment.xml"

        Dim SysBuild As String
        Dim BuildPath As String
        Dim PromotionLevel As String = "st04"
        Dim _AdditionalEnvironmentVariables As New Dictionary(Of String, String)

        XMLBuildEnvironmentFile = New Xml.XmlDocument
        XMLBuildEnvironmentFile.Load(BuildEnvironmentLocation)

        Dim BuildNodelist As Xml.XmlNodeList = XMLBuildEnvironmentFile.SelectNodes("/buildenvironment/server[@name='" & "adebs03" & "']")
        Dim BuildConfigPath As String = String.Empty

        'Get the sysbuild folder location.
        SysBuild = XMLBuildEnvironmentFile.SelectSingleNode("/buildenvironment/server[@name='" & "adebs03" & "']").Attributes.GetNamedItem("sysbuild").Value

        For Each xmlelement As Xml.XmlElement In BuildNodelist
            BuildPath = xmlelement.SelectSingleNode("promotion[@level='" & PromotionLevel & "']").Attributes.GetNamedItem("path").Value
            BuildConfigPath = System.IO.Path.Combine(BuildPath, xmlelement.SelectSingleNode("buildfile").Attributes.GetNamedItem("relativepath").Value)
            'VS2003Devenv = New Devenv(xmlelement.SelectSingleNode("devenv[@solutiontype='VS2003']"))
            'VS2005Devenv = New Devenv(xmlelement.SelectSingleNode("devenv[@solutiontype='VS2005']"))
            'VS2008Devenv = New Devenv(xmlelement.SelectSingleNode("devenv[@solutiontype='VS2008']"))
            'VS2010Devenv = New Devenv(xmlelement.SelectSingleNode("devenv[@solutiontype='VS2010']"))
            'MSBuild3_5 = New MSBuild(xmlelement.SelectSingleNode("msbuild[@solutiontype='MSBUILD3_5']"))
            'MSBuild4_0_30319 = New MSBuild(xmlelement.SelectSingleNode("msbuild[@solutiontype='MSBUILD4_0_30319']"))

            ' get environment details
            Dim EnvironmentNodeList As Xml.XmlNodeList = xmlelement.SelectNodes("environment")

            For Each xmlenvelement As Xml.XmlElement In EnvironmentNodeList
                Dim PathNodeList As Xml.XmlNodeList = xmlenvelement.SelectNodes("searchpath")

                For Each xmlpathelement As Xml.XmlElement In PathNodeList
                    Dim SearchPath As String = xmlpathelement.GetAttribute("path")

                    If Not xmlpathelement.GetAttribute("relative") Is Nothing Then
                        If xmlpathelement.GetAttribute("relative").ToLower = "true" Then
                            SearchPath = String.Format("{0}\{1}", BuildPath, SearchPath)
                        End If
                    End If

                    ' AppendPathForEnv(xmlenvelement.GetAttribute("name"), xmlpathelement.GetAttribute("type").ToLower, SearchPath)

                Next

                Dim envVarPathElementList As Xml.XmlNodeList = xmlenvelement.SelectNodes("environmentvariablepath")
                ' StreamOutput.WriteLine(Now & " Build Environment : Environment Variable Element Count is " & envVarPathElementList.Count)
                For Each envVarPathElement As Xml.XmlElement In envVarPathElementList
                    Dim envVarName As String = envVarPathElement.GetAttribute("name")
                    Dim envVarPath As String = envVarPathElement.GetAttribute("path")
                    If Not Regex.IsMatch(envVarPath, "[a-z]:.*", RegexOptions.IgnoreCase) Then
                        ' Supplied path is relative to the Promotion Group Build Path
                        envVarPath = Path.Combine(BuildPath, envVarPath)
                    End If
                    ' StreamOutput.WriteLine(Now & " Build Environment : Environment Variable """ & envVarName & """ = """ & envVarPath & """")
                    _AdditionalEnvironmentVariables.Add(envVarName, envVarPath)
                Next

                For Each envVarName As String In _AdditionalEnvironmentVariables.Keys
                    Dim envVarValue As String = _AdditionalEnvironmentVariables.Item(envVarName)
                Next

            Next
        Next

    End Sub

    Sub TestXMLBuildXML()

        Dim XMLSolutionBuildDocument As System.Xml.XmlDocument
        Dim BuildDocumentLocation As String = "C:\ST04\Master\Build\Build.net\build.xml"

        XMLSolutionBuildDocument = New Xml.XmlDocument
        XMLSolutionBuildDocument.Load(BuildDocumentLocation)

        Dim OuterSolutionNodelist As Xml.XmlNodeList = XMLSolutionBuildDocument.SelectNodes("/build/solutions[@config='" & "debug" & "']")
        ' Dim OuterSolutionNodelist As Xml.XmlNodeList = XMLSolutionBuildDocument.SelectNodes("/build/solutions[fn:matches(@config,'debug',i)]")

        For Each xmlSolutionListElement As Xml.XmlElement In OuterSolutionNodelist

            Dim SolutionList As Xml.XmlNodeList = xmlSolutionListElement.SelectNodes("solution")

            For Each solutionElement As Xml.XmlElement In SolutionList

                Dim solutionName = solutionElement.GetAttribute("name")
                Dim solutionTimeout As String = solutionElement.GetAttribute("maxbuildtimeminutes")
                If solutionTimeout Is Nothing Then
                    Console.WriteLine("Solution ""{0}"" has undefined timeout", solutionName)
                Else
                    Dim solutionBuildTimeoutMinutes As Integer
                    If Int32.TryParse(solutionTimeout, solutionBuildTimeoutMinutes) Then
                        Console.WriteLine("Solution ""{0}"" has timeout {1} minutes", solutionName, solutionTimeout)
                    Else
                        Console.WriteLine("Solution ""{0}"" is defaulted", solutionName)
                    End If
                End If
            Next

        Next

    End Sub

    Sub TestProcess()
        Dim processName As String = "notepad"

        Dim thisProcess As Process

        thisProcess = Process.Start(processName, "")

        Dim completed = thisProcess.WaitForExit(5000)

        Console.WriteLine("Process ""{0}"" {1}. Exit Code = {2}", processName, IIf(completed, "Completed", "Did Not Complete"), thisProcess.ExitCode)

        If Not completed Then
            Console.WriteLine("Process ""{0}"" did not complete", processName)
            thisProcess.Kill()
            thisProcess.WaitForExit()
            Console.WriteLine("Process ""{0}"" killed", processName)
        End If

    End Sub

    Sub TestEnvironmentVariable()

        Const EnvVarName1 = "COMPUTERNAME"
        Console.WriteLine("Environment Variable ""{0}"" = ""{1}""", EnvVarName1, Environment.GetEnvironmentVariable(EnvVarName1))

        Const EnvVarName2 = "OEDIPUS_SRC_REDIRECT"
        Dim EnvVarValue2 = Environment.GetEnvironmentVariable(EnvVarName2)
        If String.IsNullOrEmpty(EnvVarValue2) Then
            Console.WriteLine("Environment Variable ""{0}"" does not exist")
        Else
            Console.WriteLine("Environment Variable ""{0}"" = ""{1}""", EnvVarName2, EnvVarValue2)
        End If

    End Sub

    Sub TestFileWriting()

        Try

            Using fileStream As New FileStream("Test.txt", System.IO.FileMode.Append, FileAccess.Write, FileShare.Read)

                Using streamWriter As New StreamWriter(fileStream)

                    streamWriter.WriteLine("This is an output")

                End Using

            End Using

        Catch ex As Exception

        End Try

    End Sub

    Sub Main()

        ' Call TestProcess()

        ' Call TestXMLBuildXML()

        ' Call TestEnvironmentVariable()

        Call TestFileWriting()

    End Sub

End Module
