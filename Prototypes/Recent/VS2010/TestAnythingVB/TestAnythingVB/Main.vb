Imports System.ServiceProcess

Module Main

    Sub Main()

        Dim totalFilesCopiedCount As Integer = 0

        Try
            Console.Out.WriteLine(Now & " CopyLatestVersions: Copying ends after " & totalFilesCopiedCount & " files")
        Catch ex As Exception
            Console.Out.WriteLine("Exception:{0}", ex.ToString())
        End Try

        Dim sstatus As System.ServiceProcess.ServiceControllerStatus = System.ServiceProcess.ServiceControllerStatus.Running

        Console.Out.WriteLine("Service status is " & [Enum].GetName(GetType(System.ServiceProcess.ServiceControllerStatus), sstatus))
        Console.Out.WriteLine("Service status is " & sstatus.ToString())

        Dim ServiceController As System.ServiceProcess.ServiceController = New System.ServiceProcess.ServiceController("Print Spooler")
        Console.Out.WriteLine("Service Control Status is {0}", ServiceController.Status)
        ServiceController.Stop()
        Try
            ServiceController.Stop()
        Catch ex As Exception
            Console.Out.WriteLine("Exception:{0}", ex.ToString())
        End Try
        ServiceController.WaitForStatus(ServiceProcess.ServiceControllerStatus.Stopped, System.TimeSpan.FromSeconds(60))
        Console.Out.WriteLine("Service Control Status is {0}", ServiceController.Status)
        ServiceController.Start()
        ServiceController.WaitForStatus(ServiceProcess.ServiceControllerStatus.Running, System.TimeSpan.FromSeconds(60))
        Console.Out.WriteLine("Service Control Status is {0}", ServiceController.Status)

        Dim x As Integer

        x = 1 Or 4

        x = x Or 1

    End Sub

End Module
