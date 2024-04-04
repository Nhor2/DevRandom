Imports System

Namespace Img2Wav
    Friend Class FFT_Img2Wav
        Public Shared Function num_flops(N As Integer) As Double
            Dim Nd As Double = N
            Dim logN As Double = log2(N)

            Return (5.0 * Nd - 2) * logN + 2 * (Nd + 1)
        End Function


        ''' <summary>
        ''' Compute Fast Fourier Transform of (complex) data, in place.
        ''' </summary>
        Public Shared Sub transform(data As Double())
            transform_internal(data, -1)
        End Sub

        ''' <summary>
        ''' Compute Inverse Fast Fourier Transform of (complex) data, in place.
        ''' </summary>
        Public Shared Sub inverse(data As Double())
            transform_internal(data, +1)
            ' Normalize
            Dim nd = data.Length
            Dim n As Integer = nd / 2
            Dim norm = 1 / n
            For i = 0 To nd - 1
                data(i) *= norm
            Next
        End Sub

        ''' <summary>
        ''' Accuracy check on FFT of data. Make a copy of data, Compute the FFT, then
        ''' the inverse and compare to the original.  Returns the rms difference.
        ''' </summary>
        Public Shared Function test(data As Double()) As Double
            Dim nd = data.Length
            ' Make duplicate for comparison
            Dim copy = New Double(nd - 1) {}
            Array.Copy(data, 0, copy, 0, nd)
            ' Transform & invert
            transform(data)
            inverse(data)
            ' Compute RMS difference.
            Dim diff = 0.0
            For i = 0 To nd - 1
                Dim d = data(i) - copy(i)
                diff += d * d
            Next
            Return Math.Sqrt(diff / nd)
        End Function

        ''' <summary>
        ''' Make a random array of n (complex) elements. 
        ''' </summary>
        Public Shared Function makeRandom(n As Integer) As Double()
            Dim nd = 2 * n
            Dim data = New Double(nd - 1) {}
            Dim r As Random = New Random()
            For i = 0 To nd - 1
                data(i) = r.NextDouble()
            Next
            Return data
        End Function

        Protected Friend Shared Function log2(n As Integer) As Integer
            Dim log = 0
            Dim k = 1

            While k < n
                k *= 2
                log += 1
            End While
            If n <> 1 << log Then Throw New ApplicationException("FFT: Data length is not a power of 2!: " & n.ToString())
            Return log
        End Function

        Protected Friend Shared Sub transform_internal(data As Double(), direction As Integer)
            If data.Length = 0 Then Return
            Dim n As Integer = data.Length / 2
            If n = 1 Then Return
            ' Identity operation!
            Dim logn = log2(n)

            ' bit reverse the input data for decimation in time algorithm 
            bitreverse(data)

            ' apply fft recursion 
            ' this loop executed log2(N) times 
            Dim bit = 0, dual = 1

            While bit < logn
                Dim w_real = 1.0
                Dim w_imag = 0.0

                Dim theta = 2.0 * direction * Math.PI / (2.0 * dual)
                Dim s = Math.Sin(theta)
                Dim t = Math.Sin(theta / 2.0)
                Dim s2 = 2.0 * t * t

                ' a = 0 
                Dim b = 0

                While b < n
                    Dim i = 2 * b
                    Dim j = 2 * (b + dual)

                    Dim wd_real = data(j)
                    Dim wd_imag = data(j + 1)

                    data(j) = data(i) - wd_real
                    data(j + 1) = data(i + 1) - wd_imag
                    data(i) += wd_real
                    data(i + 1) += wd_imag
                    b += 2 * dual
                End While

                ' a = 1 .. (dual-1) 
                For a = 1 To dual - 1
                    ' trignometric recurrence for w-> exp(i theta) w 
                    If True Then
                        Dim tmp_real = w_real - s * w_imag - s2 * w_real
                        Dim tmp_imag = w_imag + s * w_real - s2 * w_imag
                        w_real = tmp_real
                        w_imag = tmp_imag
                    End If
                    b = 0

                    While b < n
                        Dim i = 2 * (b + a)
                        Dim j = 2 * (b + a + dual)

                        Dim z1_real = data(j)
                        Dim z1_imag = data(j + 1)

                        Dim wd_real = w_real * z1_real - w_imag * z1_imag
                        Dim wd_imag = w_real * z1_imag + w_imag * z1_real

                        data(j) = data(i) - wd_real
                        data(j + 1) = data(i + 1) - wd_imag
                        data(i) += wd_real
                        data(i + 1) += wd_imag
                        b += 2 * dual
                    End While
                Next

                bit += 1
                dual *= 2
            End While
        End Sub


        Protected Friend Shared Sub bitreverse(data As Double())
            ' This is the Goldrader bit-reversal algorithm 
            Dim n As Integer = data.Length / 2
            Dim nm1 = n - 1
            Dim i = 0
            Dim j = 0
            While i < nm1

                'int ii = 2*i;
                Dim ii = i << 1

                'int jj = 2*j;
                Dim jj = j << 1

                'int k = n / 2 ;
                Dim k = n >> 1

                If i < j Then
                    Dim tmp_real = data(ii)
                    Dim tmp_imag = data(ii + 1)
                    data(ii) = data(jj)
                    data(ii + 1) = data(jj + 1)
                    data(jj) = tmp_real
                    data(jj + 1) = tmp_imag
                End If

                While k <= j
                    'j = j - k ;
                    j -= k

                    'k = k / 2 ; 
                    k >>= 1
                End While
                j += k
                i += 1
            End While
        End Sub
    End Class
End Namespace
