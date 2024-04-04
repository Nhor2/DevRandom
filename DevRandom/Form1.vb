Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.InteropServices
Imports DevRandom.Img2Wav
Imports Img2WavHelp
Imports QRCoder

Public Class Form1

    'DevRandom
    'lord.marte@gmail.com
    '18-03-2024
    'Crea continuamente numeri casuali

    Dim mouse_move As System.Drawing.Point

    'Il numero casuale
    Dim randx As Integer = 0
    Dim randx_file As String = Application.StartupPath & "\devrandom.txt"
    Dim randx_wav As String = Application.StartupPath & "\devrandom.wav"
    Dim randx_img As String = Application.StartupPath & "\devrandom.png"
    Dim randx_qr As String = Application.StartupPath & "\devrandom.png"
    Dim randx_www As String = Application.StartupPath & "\www\index.html"
    Dim randx_start As Integer = 0      'serve per capire che è l'inizio

    Dim cc_bmp As Bitmap = New Bitmap(512, 512)  '80 64

    Dim appDir As String = Application.StartupPath()
    Dim exeDir As String = Application.ExecutablePath()
    Dim machinename As String = Environment.MachineName
    Dim OpenCMD = CreateObject("wscript.shell")

    ' Environment variable names for default, process, user, and machine targets.
    'Attualmente Windows 10, 11 non funziona.
    Dim defaultEnvVar As String = NameOf(defaultEnvVar)
    Dim processEnvVar As String = NameOf(processEnvVar)
    Dim userEnvVar As String = NameOf(userEnvVar)
    Dim machineEnvVar As String = NameOf(machineEnvVar)
    Dim dft As String = NameOf(dft)
    Dim process As String = NameOf(process)
    Dim user As String = NameOf(user)
    Dim machine As String = NameOf(machine)




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Start()

        Dim r As New Random
        randx = r.Next()
    End Sub

    Private Sub window_header_Paint(sender As Object, e As PaintEventArgs) Handles window_header.Paint
        'Bordo panel
        ControlPaint.DrawBorder(e.Graphics, Me.ClientRectangle, Color.Silver, ButtonBorderStyle.Solid)
    End Sub

    Private Sub window_header_MouseMove(sender As Object, e As MouseEventArgs) Handles window_header.MouseMove
        If (e.Button = Windows.Forms.MouseButtons.Left) Then
            Dim mposition As Point
            mposition = Control.MousePosition
            mposition.Offset(mouse_move.X, mouse_move.Y)
            Me.Location = mposition
        End If

    End Sub

    Private Sub window_header_MouseDown(sender As Object, e As MouseEventArgs) Handles window_header.MouseDown
        mouse_move = New Point(-e.X, -e.Y)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'max
        If Me.WindowState = FormWindowState.Normal Then
            Me.WindowState = FormWindowState.Maximized
        Else
            Me.WindowState = FormWindowState.Normal
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'min
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'close
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'titolo
        Dim mill As String = DateAndTime.Now.Millisecond & Strings.StrReverse(DateAndTime.Now.Millisecond)
        Label2.Text = mill
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Timer2.Start()
        'sempre
        TimerPNG.Start()

        System.Threading.Thread.Sleep(1000)

        'App
        If CheckBox1.Checked = True Then
            TimerClip.Start()
        Else
            CheckBox1.Enabled = False
        End If

        'File
        If CheckBox2.Checked = True Then
            TimerFile.Start()
        Else
            CheckBox2.Enabled = False
        End If

        'Finestra
        If CheckBox3.Checked = True Then
            TimerWin.Start()
        Else
            CheckBox3.Enabled = False
        End If

        'Wav
        If CheckBox4.Checked = True Then
            TimerWav.Start()
        Else
            CheckBox4.Enabled = False
        End If

        'Img
        If CheckBox5.Checked = True Then

        Else
            CheckBox5.Enabled = False
        End If

        'QR
        If CheckBox6.Checked = True Then
            TimerQR.Start()
        Else
            CheckBox6.Enabled = False
        End If

        'www
        If CheckBox7.Checked = True Then
            TimerWeb.Start()
        Else
            CheckBox7.Enabled = False
        End If

        Button4.Enabled = False

        randx_start = 1
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        'value e visualizzazione
        Dim value = 0

        Dim r As New Random
        randx = r.Next()

        'set [<variable>=[<string>]]    'Attualmente non funziona e non posso usarla
        ' Check whether the environment variable exists.
        ' Dim value = Environment.GetEnvironmentVariable("RANDX")
        ' If necessary, create it.
        'If value Is Nothing Then
        'Environment.SetEnvironmentVariable("RANDX", randx, EnvironmentVariableTarget.User)

        ' Now retrieve it.
        'value = Environment.GetEnvironmentVariable("RANDX")
        'Else
        'Environment.SetEnvironmentVariable("RANDX", randx, EnvironmentVariableTarget.User)

        ' Now retrieve it.
        'value = Environment.GetEnvironmentVariable("RANDX")
        'End If

        If CheckBox1.Checked = True Then
            Label3.Text = Clipboard.GetText()
        End If

        'randx
        Label4.Text = randx

        PictureBox1.Refresh()
    End Sub

    Private Sub TimerClip_Tick(sender As Object, e As EventArgs) Handles TimerClip.Tick
        'Appunti
        Clipboard.SetText(randx.ToString)
    End Sub

    Private Sub TimerFile_Tick(sender As Object, e As EventArgs) Handles TimerFile.Tick
        'File
        If File.Exists(randx_file) Then
            If FileLen(randx_file) > 500000 Then
                My.Computer.FileSystem.DeleteFile(randx_file)
            End If
        End If

        My.Computer.FileSystem.WriteAllText(randx_file, randx.ToString & vbCrLf, True)
    End Sub

    Private Sub TimerWin_Tick(sender As Object, e As EventArgs) Handles TimerWin.Tick
        'Finestra
        Form2.randx2 = randx
    End Sub

    Private Sub TimerWav_Tick(sender As Object, e As EventArgs) Handles TimerWav.Tick
        'Wav
        'Dim core As Img2Wav.Core_Img2Wav = New Img2Wav.Core_Img2Wav()  

        'Vengono usate le classi derivate in C# per eliminare Overflow, Img2WavHelp
        Dim core As Img2WavHelp.Img2Wav.Core_Img2Wav = New Img2WavHelp.Img2Wav.Core_Img2Wav()
        If PictureBox1.Image IsNot Nothing Then
            core.InputBitmap = CType(PictureBox1.Image, Bitmap)
            core.Start()

            core.OutputWav.WriteFile(randx_wav)
        End If
    End Sub

    Private Sub TimerPNG_Tick(sender As Object, e As EventArgs) Handles TimerPNG.Tick
        'Img
        Dim AxA1 = CInt(Strings.Mid(randx.ToString, 1, 2))
        Dim AxA2 = CInt(Strings.Mid(randx.ToString, 3, 2))
        Dim AxA3 = CInt(Strings.Mid(randx.ToString, 5, 2))

        Dim cc As Color = Color.FromArgb(AxA1, AxA2, AxA3)

        Using gfx As Graphics = Graphics.FromImage(cc_bmp)
            Using brush As SolidBrush = New SolidBrush(cc)
                Using penx As Pen = New Pen(cc, 4)
                    gfx.Clear(Color.Black)
                    For i = 1 To (4)
                        gfx.DrawLine(penx, 256, AxA1 * Rnd(), AxA2 * Rnd(), 448)
                        gfx.DrawLine(penx, AxA2 * Rnd(), 448, AxA3 * Rnd(), 64)
                        gfx.DrawLine(penx, 128, AxA1 * Rnd(), 256, AxA3 * Rnd())
                        gfx.DrawLine(penx, 448, AxA2 * Rnd(), 256, AxA1 * Rnd())
                    Next
                End Using
            End Using
        End Using

        'ImageConverter Class convert Image object to Byte Array.
        Dim bytes As Byte() = CType((New ImageConverter()).ConvertTo(cc_bmp, GetType(Byte())), Byte())

        'Convert Byte Array to Image and display in PictureBox.
        PictureBox1.Image = Image.FromStream(New MemoryStream(bytes))

        If CheckBox5.Checked = True Then
            If PictureBox1.Image IsNot Nothing Then
                PictureBox1.Image.Save(randx_img, ImageFormat.Png)
            End If
        End If
    End Sub

    Private Sub TimerQR_Tick(sender As Object, e As EventArgs) Handles TimerQR.Tick
        'QR
        Dim gen As New QRCodeGenerator
        Dim data = gen.CreateQrCode(randx.ToString, QRCodeGenerator.ECCLevel.Q)
        Dim code As New QRCode(data)
        PictureBox2.Image = code.GetGraphic(6)
        PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox2.Refresh()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        'STOP
        Timer2.Stop()
        randx = 0
        Label3.Text = "0000000"
        Label4.Text = "0000000"

        TimerClip.Stop()
        TimerFile.Stop()
        TimerPNG.Stop()
        TimerQR.Stop()
        TimerWav.Stop()
        TimerWin.Stop()

        CheckBox1.Checked = False
        CheckBox2.Checked = False
        CheckBox3.Checked = False
        CheckBox4.Checked = False
        CheckBox5.Checked = False
        CheckBox6.Checked = False

        CheckBox1.Enabled = True
        CheckBox2.Enabled = True
        CheckBox3.Enabled = True
        CheckBox4.Enabled = True
        CheckBox5.Enabled = True
        CheckBox6.Enabled = True

        Button4.Enabled = False
        Button6.Enabled = False
        Button5.Enabled = False

        randx_start = 0
        StopMe()
    End Sub

    Public Sub StopMe()
        Button4.Enabled = False
        Button6.Enabled = False
        Button5.Enabled = False
        randx_start = 0
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        'appunti
        If randx_start = 0 Then
            Clipboard.SetText(" ")
            TimerClip.Interval = CInt(TextBox1.Text)
            Button4.Enabled = True
            Button5.Enabled = True
        End If

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        'file
        If randx_start = 0 Then
            TimerFile.Interval = CInt(TextBox2.Text)
            SaveFileDialog1.Filter = "File Testo TXT|*.txt"
            SaveFileDialog1.FileName = "devrandom.txt"
            SaveFileDialog1.Title = "Salva il file DevRandom..."
            If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
                If SaveFileDialog1.FileName <> "" Then
                    randx_file = SaveFileDialog1.FileName

                    Button4.Enabled = True
                    Button5.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        'Form
        If randx_start = 0 Then
            TimerWin.Interval = CInt(TextBox3.Text)
            Button4.Enabled = True
            Button5.Enabled = True
            Button6.Enabled = True
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        'Form
        Form2.Show()
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        'wav
        If randx_start = 0 Then
            TimerWav.Interval = CInt(TextBox4.Text) + CInt(TextBox5.Text) 'Al file WAV serve l'immagine di PictureBox1
            SaveFileDialog1.Filter = "File Sonoro WAV|*.wav"
            SaveFileDialog1.FileName = "devrandom.wav"
            SaveFileDialog1.Title = "Salva il file DevRandom..."
            If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
                If SaveFileDialog1.FileName <> "" Then
                    randx_wav = SaveFileDialog1.FileName

                    Button4.Enabled = True
                    Button5.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        'Img
        If randx_start = 0 Then
            TimerPNG.Interval = CInt(TextBox5.Text)
            SaveFileDialog1.Filter = "File Immagine PNG|*.png"
            SaveFileDialog1.FileName = "devrandom.png"
            SaveFileDialog1.Title = "Salva il file DevRandom..."
            If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
                If SaveFileDialog1.FileName <> "" Then
                    randx_wav = SaveFileDialog1.FileName

                    Button4.Enabled = True
                    Button5.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        'QR
        If randx_start = 0 Then
            TimerQR.Interval = CInt(TextBox11.Text)

            SaveFileDialog1.Filter = "File Immagine QRCode|*.png"
            SaveFileDialog1.FileName = "devrandom.png"
            SaveFileDialog1.Title = "Salva il file DevRandom..."
            If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
                If SaveFileDialog1.FileName <> "" Then
                    randx_qr = SaveFileDialog1.FileName

                    Button4.Enabled = True
                    Button5.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub TimerWeb_Tick(sender As Object, e As EventArgs) Handles TimerWeb.Tick
        'WWW
        Dim html = "<html><head></head><body><h1>/dev/random = " & randx.ToString & "</h1></body></html>"
        My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\www\index.html", html, False)
    End Sub

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        If randx_start = 0 Then
            TimerWeb.Interval = CInt(TextBox6.Text)

            If System.IO.Directory.Exists(Application.StartupPath & "\www") Then
                Dim randhttp As New HttpFileServer(Application.StartupPath & "\www")
            Else
                System.IO.Directory.CreateDirectory(Application.StartupPath & "\www")
                Dim randhttp As New HttpFileServer(Application.StartupPath & "\www")
            End If

            Button4.Enabled = True
            Button5.Enabled = True
        End If
    End Sub
End Class