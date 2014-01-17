Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports System.Globalization

Module TimeServer

    Sub Main()
        Dim now As Date
        Dim strDateLine As String
        Dim ASCII As Encoding = Encoding.ASCII

        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture

        Try
            Dim tcpl As New TcpListener(IPAddress.Any, 14)

            tcpl.Start()

            Console.WriteLine("Waiting for clients to connect")
            Console.WriteLine("Press ctrl+c to quit...")

            While (True)
                Dim s As Socket = tcpl.AcceptSocket()

                now = DateTime.Now
                strDateLine = now.ToShortDateString() + " " + now.ToLongTimeString()

                Dim byteDateLine() As Byte = ASCII.GetBytes(strDateLine.ToCharArray())
                s.Send(byteDateLine, byteDateLine.Length, SocketFlags.None)
                s.Close()
                Console.WriteLine("Sent {0}", strDateLine)
            End While
        Catch socketError As SocketException
            If (socketError.ErrorCode) = 10048 Then
                Console.WriteLine("Connection to this port failed. There is another server listening on this port.")
            End If
        End Try
    End Sub

End Module
