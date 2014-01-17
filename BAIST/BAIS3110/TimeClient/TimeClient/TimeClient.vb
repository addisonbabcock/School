Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.IO

Module TimeClient

    Sub Main()
        Dim tcpc As New TcpClient()
        Dim read(35) As Byte
        Dim args As String() = Environment.GetCommandLineArgs()

        If (args.Length < 2) Then
            Console.WriteLine("Please specify a server name in the command line.")
            Exit Sub
        End If

        Dim server As String = args(1)

        Try
            Dns.GetHostEntry(server)
        Catch
            Console.WriteLine("Cannot find server: {0}", server)
            Exit Sub
        End Try

        tcpc.Connect(server, 14)

        Dim s As Stream
        Try
            s = tcpc.GetStream()
        Catch exc As InvalidOperationException
            Console.WriteLine("Cannot connect to server: {0}", server)
            Exit Sub
        End Try

        Dim bytes As Integer = s.Read(read, 0, read.Length)
        Dim time As String = Encoding.ASCII.GetString(read)

        Console.WriteLine("Received {0} bytes", bytes)
        Console.WriteLine("Current date and time is: {0}", time)

        tcpc.Close()

        Console.WriteLine("Press return to exit")
        Console.Read()
    End Sub

End Module
