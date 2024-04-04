Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web
Public Class HttpFileServer
    Implements IDisposable
    Public rootPath As String
    Private Const bufferSize As Integer = 1024 * 512
    '512KB
    Private ReadOnly http As HttpListener
    Public Sub New(ByVal rootPath As String)
        Me.rootPath = rootPath
        http = New HttpListener()
        http.Prefixes.Add("http://localhost:80/")
        http.Start()
        http.BeginGetContext(AddressOf requestWait, Nothing)
    End Sub
    Public Sub Dispose() Implements System.IDisposable.Dispose
        http.[Stop]()
    End Sub
    Private Sub requestWait(ByVal ar As IAsyncResult)
        If Not http.IsListening Then
            Return
        End If
        Dim c = http.EndGetContext(ar)
        http.BeginGetContext(AddressOf requestWait, Nothing)
        Dim url = tuneUrl(c.Request.RawUrl)
        Dim fullPath = IIf(String.IsNullOrEmpty(url), rootPath, Path.Combine(rootPath, url))
        If Directory.Exists(fullPath) Then
            returnDirContents(c, fullPath)
        ElseIf File.Exists(fullPath) Then
            returnFile(c, fullPath)
        Else
            return404(c)
        End If
    End Sub
    Private Sub returnDirContents(ByVal context As HttpListenerContext, ByVal dirPath As String)
        context.Response.ContentType = "text/html"
        context.Response.ContentEncoding = Encoding.UTF8
        Using sw = New StreamWriter(context.Response.OutputStream)
            sw.WriteLine("html")
            sw.WriteLine("head meta http-equiv='Content-Type' content='text/html; charset=utf-8/head'")
            sw.WriteLine("body ul")
            Dim dirs = Directory.GetDirectories(dirPath)
            For Each d As Object In dirs
                Dim link = d.Replace(rootPath, "").Replace("\"c, "/"c)
                sw.WriteLine("<li>&lt;DIR&gt; a href=" + link + " " + Path.GetFileName(d) + "/a  /li ")
            Next
            Dim files = Directory.GetFiles(dirPath)
            For Each f As Object In files
                Dim link = f.Replace(rootPath, "").Replace("\"c, "/"c)
                sw.WriteLine(" li <a href=" + link + " " + Path.GetFileName(f) + " /a  /li ")
            Next
            sw.WriteLine(" /ul  /body  /html ")
        End Using
        context.Response.OutputStream.Close()
    End Sub
    Private Shared Sub returnFile(ByVal context As HttpListenerContext, ByVal filePath As String)
        context.Response.ContentType = getcontentType(Path.GetExtension(filePath))
        Dim buffer = New Byte(bufferSize - 1) {}
        Using fs = File.OpenRead(filePath)
            context.Response.ContentLength64 = fs.Length
            Dim read As Integer
            While (InlineAssignHelper(read, fs.Read(buffer, 0, buffer.Length))) > 0
                context.Response.OutputStream.Write(buffer, 0, read)
            End While
        End Using
        context.Response.OutputStream.Close()
    End Sub
    Private Shared Sub return404(ByVal context As HttpListenerContext)
        context.Response.StatusCode = 404
        context.Response.Close()
    End Sub
    Private Shared Function tuneUrl(ByVal url As String) As String
        url = url.Replace("/"c, "\"c)
        url = encodeUTF8(url)
        url = url.Substring(1)
        Return url
    End Function

    Public Shared Function encodeUTF8(ByVal str As String)
        Dim utf8Encoding As New System.Text.UTF8Encoding
        Dim encodedString() As Byte
        encodedString = utf8Encoding.GetBytes(str)
        Return utf8Encoding.GetString(encodedString)
    End Function

    Private Shared Function getcontentType(ByVal extension As String) As String
        Select Case extension
            Case ".avi"
                Return "video/x-msvideo"
            Case ".css"
                Return "text/css"
            Case ".doc"
                Return "application/msword"
            Case ".gif"
                Return "image/gif"
            Case ".htm", ".html"
                Return "text/html"
            Case ".jpg", ".jpeg"
                Return "image/jpeg"
            Case ".js"
                Return "application/x-javascript"
            Case ".mp3"
                Return "audio/mpeg"
            Case ".png"
                Return "image/png"
            Case ".pdf"
                Return "application/pdf"
            Case ".ppt"
                Return "application/vnd.ms-powerpoint"
            Case ".zip"
                Return "application/zip"
            Case ".txt"
                Return "text/plain"
            Case Else
                Return "application/octet-stream"
        End Select
    End Function
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function
End Class

