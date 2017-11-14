Imports System.IO

Module Module1

    Public Sub Main(ByVal args() As String)

        Console.WriteLine("Count of arguments is {0}", args.Length)

        For Each arg As String In args

            Dim argAsIs As String = arg

            Dim charsToTrim() As Char = {Path.DirectorySeparatorChar}

            Dim thisArg1 As String = arg.Trim(New Char() {"\"c})
            Dim thisArg2 As String = arg.Trim(New Char() {Path.DirectorySeparatorChar})
            Dim thisArg3 As String = arg.Trim(charsToTrim)

            Console.WriteLine("Argument trimmed chars is ""{0}""", thisArg1)
            Console.WriteLine("Argument trimmed string is ""{0}""", thisArg2)
            Console.WriteLine("Argument trimmed array of char is ""{0}""", thisArg3)

        Next

    End Sub

End Module
