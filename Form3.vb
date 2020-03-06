Public Class Form3
    Dim mouseD As Boolean
    Public Angle_F As Double
    Dim k As Integer
    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(Form1.Width - Me.Width, Form1.Height - Me.Height)
    End Sub
    Private Sub Form3_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Me.Visible = False
    End Sub

    Private Sub PictureBox2_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox2.MouseDown
        mouseD = True
        Angle_F = Math.Atan2(75 - e.Y, e.X - 75)
        PictureBox2_Draw()
    End Sub

    Private Sub PictureBox2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox2.MouseMove
        If mouseD Then
            Angle_F = Math.Atan2(75 - e.Y, e.X - 75)
            PictureBox2_Draw()
        End If
    End Sub
    Public Sub PictureBox2_Draw()
        TextBox1.Text = CStr(Angle_F)
        Dim tmp As Bitmap
        Dim drawBrush As New SolidBrush(Color.FromArgb(151, 255, 255))
        Dim drawpen As New Pen(drawBrush, 1)
        Dim g As Graphics
        Dim R, x, y As Integer
        tmp = New Bitmap(150, 150)
        g = Graphics.FromImage(tmp)
        R = 10
        x = 75 + 70 * Math.Cos(Angle_F)
        y = 75 - 70 * Math.Sin(Angle_F)
        'drawpen.Width = 1
        g.DrawEllipse(drawpen, 5, 5, 140, 140)

        g.DrawLine(drawpen, New Point(75, 75), New Point(x, y))
        g.FillEllipse(drawBrush, x - R \ 2, y - R \ 2, R, R)

        drawBrush.Color = Color.FromArgb(0, 128, 255)
        g.FillEllipse(drawBrush, 75 - R \ 2, 75 - R \ 2, R, R)

        PictureBox2.Image = tmp
    End Sub

    Private Sub PictureBox2_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox2.MouseUp
        mouseD = False
    End Sub

    Function draw_pic(ByVal Angle As Double) As Bitmap
        Dim tmp As Bitmap
        Dim drawBrush As New SolidBrush(Color.FromArgb(151, 255, 255))
        Dim drawpen As New Pen(drawBrush, 1)
        Dim g As Graphics
        Dim R, x, y As Integer
        tmp = New Bitmap(150, 150)
        g = Graphics.FromImage(tmp)
        R = 10
        x = 75 + 70 * Math.Cos(Angle)
        y = 75 - 70 * Math.Sin(Angle)
        'drawpen.Width = 1
        g.DrawEllipse(drawpen, 5, 5, 140, 140)

        g.DrawLine(drawpen, New Point(75, 75), New Point(x, y))
        g.FillEllipse(drawBrush, x - R \ 2, y - R \ 2, R, R)

        drawBrush.Color = Color.FromArgb(0, 128, 255)
        g.FillEllipse(drawBrush, 75 - R \ 2, 75 - R \ 2, R, R)

        Return tmp
    End Function

    Function draw_pic2(ByVal heigh As Double) As Bitmap
        Dim tmp As Bitmap
        Dim R, y As Integer
        R = 16
        Dim drawBrush As New SolidBrush(Color.FromArgb(151, 255, 255))
        Dim drawpen As New Pen(drawBrush, 4)
        Dim g As Graphics
        tmp = New Bitmap(30, 150)
        g = Graphics.FromImage(tmp)
        g.DrawLine(drawpen, New Point(15, 0), New Point(15, 150))

        If heigh / 1000 < 600 Then
            y = 150 - Int(heigh / 4000)
            drawBrush.Color = Color.FromArgb(0, 128, 255)
            g.FillEllipse(drawBrush, 15 - R \ 2, y - R \ 2, R, R)

        End If
        Return tmp
    End Function

    Private Sub VScrollBar1_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles VScrollBar1.Scroll
        Label6.Text = (100 - VScrollBar1.Value) & "%"
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Form1.sm \ 10 = 3 Then

            k = Form1.sm Mod 30
            '繪製太陽系相對速度 以及顯示文字
            If Form1.sat(k).Crashed Then
                PictureBox1.Image = draw_pic(Math.Atan2(Form1.Solar_Pl(Form1.sat(k).Crashed_In).Vy, Form1.Solar_Pl(Form1.sat(k).Crashed_In).Vx))
                Label5.Text = (Form1.Solar_Pl(Form1.sat(k).Crashed_In).Vx ^ 2 + Form1.Solar_Pl(Form1.sat(k).Crashed_In).Vy ^ 2) ^ 0.5 & "m/s"
            Else
                PictureBox1.Image = draw_pic(Math.Atan2(Form1.sat(k).Vy, Form1.sat(k).Vx))
                Label5.Text = (Form1.sat(k).Vx ^ 2 + Form1.sat(k).Vy ^ 2) ^ 0.5 & " m/s"
            End If

            Label7.Text = "與" & Form1.Solar_Pl(NumericUpDown1.Value).Name & "的相對速度"
            If Form1.sat(k).Crashed And Form1.sat(k).Crashed_In = NumericUpDown1.Value Then
                PictureBox3.Image = draw_pic(1.5707963267945)
                Label8.Text = "0 m/s"
            ElseIf Form1.sat(k).Crashed And Form1.sat(k).Crashed_In <> NumericUpDown1.Value Then
                PictureBox3.Image = draw_pic(Math.Atan2(Form1.Solar_Pl(Form1.sat(k).Crashed_In).Vy - Form1.Solar_Pl(NumericUpDown1.Value).Vy, Form1.Solar_Pl(Form1.sat(k).Crashed_In).Vx - Form1.Solar_Pl(NumericUpDown1.Value).Vx))
                Label8.Text = ((Form1.Solar_Pl(Form1.sat(k).Crashed_In).Vx - Form1.Solar_Pl(NumericUpDown1.Value).Vx) ^ 2 + (Form1.Solar_Pl(Form1.sat(k).Crashed_In).Vy - Form1.Solar_Pl(NumericUpDown1.Value).Vy) ^ 2) ^ 0.5 & " m/s"
            Else
                PictureBox3.Image = draw_pic(Math.Atan2(Form1.sat(k).Vy - Form1.Solar_Pl(NumericUpDown1.Value).Vy, Form1.sat(k).Vx - Form1.Solar_Pl(NumericUpDown1.Value).Vx))
                Label8.Text = ((Form1.sat(k).Vx - Form1.Solar_Pl(NumericUpDown1.Value).Vx) ^ 2 + (Form1.sat(k).Vy - Form1.Solar_Pl(NumericUpDown1.Value).Vy) ^ 2) ^ 0.5 & " m/s"
            End If

            PictureBox4.Image = draw_pic2(Fix(((Form1.sat(k).X - Form1.Solar_Pl(NumericUpDown1.Value).X) ^ 2 + (Form1.sat(k).Y - Form1.Solar_Pl(NumericUpDown1.Value).Y) ^ 2) ^ 0.5 - Form1.Solar_Pl(NumericUpDown1.Value).r))
            If Fix(((Form1.sat(k).X - Form1.Solar_Pl(NumericUpDown1.Value).X) ^ 2 + (Form1.sat(k).Y - Form1.Solar_Pl(NumericUpDown1.Value).Y) ^ 2) ^ 0.5 - Form1.Solar_Pl(NumericUpDown1.Value).r) > 1000 Then
                Label9.Text = Fix((((Form1.sat(k).X - Form1.Solar_Pl(NumericUpDown1.Value).X) ^ 2 + (Form1.sat(k).Y - Form1.Solar_Pl(NumericUpDown1.Value).Y) ^ 2) ^ 0.5 - Form1.Solar_Pl(NumericUpDown1.Value).r) / 100) / 10 & " km"
            Else
                Label9.Text = Fix(((Form1.sat(k).X - Form1.Solar_Pl(NumericUpDown1.Value).X) ^ 2 + (Form1.sat(k).Y - Form1.Solar_Pl(NumericUpDown1.Value).Y) ^ 2) ^ 0.5 - Form1.Solar_Pl(NumericUpDown1.Value).r) & " m"
            End If


            Button1.Enabled = Form1.sat(k).Slave


        End If
    End Sub

    'Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
    '    Label7.Text = "與" & Form1.Solar_Pl(NumericUpDown1.Value).Name & "的相對速度"
    'End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Form1.sat(k).disconnect()
        Form1.sat_b_reflash()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Form1.sat(k).release()
        Me.Visible = False
        Dim tmp As Boolean
        tmp = False
        For i = 0 To 9
            tmp = tmp Or Form1.sat(i).Enable
        Next
        If tmp Then
            For i = 0 To 9
                If Form1.sat(i).Enable Then
                    Form1.sm = 30 + i
                    Form1.sw_List(i).Enabled = False
                    Exit For
                End If
            Next
        End If
        If Not tmp Then
            Form1.RadioButton2.Checked = True
        End If

        Form1.RadioButton3.Enabled = tmp
        Form1.sat_b_reflash()

    End Sub
End Class