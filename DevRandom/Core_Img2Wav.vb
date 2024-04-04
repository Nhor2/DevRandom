Imports System
Imports System.Drawing
Imports WaveLibrary

Namespace Img2Wav


    Friend Class Core_Img2Wav

        Private Const MAX_DATA As Double = +50
        Private Const MIN_DATA As Double = -50

        Private Const NoInputBitmap As String = "No input bitmap"

        Public Property InputBitmap As Bitmap
        Public Property OutputWav As WaveFile

        Public Sub Start()
            Dim NumSamples = InputBitmap.Width * InputBitmap.Height

            Dim Samples = New Byte(NumSamples - 1) {}

            OutputWav = New WaveFile(1, 16, 44000)

            If InputBitmap Is Nothing Then Throw New Exception(NoInputBitmap)

            Dim data = New Double(InputBitmap.Height - 1) {}


            Dim w = 0
            For i = 0 To InputBitmap.Width - 1
                For j = 0 To InputBitmap.Height - 1

                    Dim C = InputBitmap.GetPixel(i, j)

                    data(j) = (C.R + C.G + C.B) / 3


                Next

                Img2Wav.FFT_Img2Wav.inverse(data)

                ' Ciclo per tutti i data
                For x = 0 To data.Length - 1
                    Dim d = MAX_DATA * data(x)
                    'Samples(w) = CByte(MAX_DATA * data(x))
                    Samples(w) = CType(d * 0.002, Byte)
                    w += 1
                Next


            Next

            OutputWav.SetData(Samples, NumSamples)

        End Sub
    End Class
End Namespace
