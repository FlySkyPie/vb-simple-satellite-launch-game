Public Class Form2
    Dim mouseD As Boolean
    Dim Angle As Double
    Dim textbox(10) As TextBox
    Private Sub Form2_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Me.Visible = False
    End Sub
    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For i = 0 To 9
            textbox(i) = New TextBox

            textbox(i).ShortcutsEnabled = False
            textbox(i).Size = New Size(100, 22)
            textbox(i).TabIndex = 0

            Me.Controls.Add(textbox(i))
            If i <> 0 Then
                AddHandler textbox(i).KeyPress, AddressOf Me.TextBox_KeyPress
            End If

        Next
        textbox(0).Location = New Point(111, 6)
        textbox(1).Location = New Point(111, 34)
        textbox(2).Location = New Point(111, 62)
        textbox(3).Location = New Point(111, 90)
        textbox(4).Location = New Point(111, 118)
        textbox(5).Location = New Point(111, 182)
        textbox(6).Location = New Point(111, 210)
        textbox(7).Location = New Point(111, 238)
        textbox(8).Location = New Point(111, 266)
        textbox(9).Location = New Point(258, 130)
        textbox(9).Enabled = False

        textbox(0).Text = "人造衛星"
        textbox(1).Text = "83.6"    '衛星質量
        textbox(2).Text = "10"      '衛星燃料
        textbox(3).Text = "3050"    '引擎比衝
        textbox(4).Text = "10"      '燃燒速率

        textbox(5).Text = "38555"   '火箭質量
        textbox(6).Text = "29510"   '燃料
        textbox(7).Text = "2600"    '引擎比衝
        textbox(8).Text = "290"   '燃燒速率
        textbox(9).Text = "0"       '發射角

        PictureBox1_Draw(New Point(50, 0))
    End Sub

    Private Sub PictureBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        mouseD = True
        PictureBox1_Draw(e.Location)
    End Sub

    Private Sub PictureBox1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        If mouseD Then
            PictureBox1_Draw(e.Location)
        End If
    End Sub
    Sub PictureBox1_Draw(ByVal p As Point)
        Angle = Math.Atan2(50 - p.Y, p.X - 50)
        textbox(9).Text = CStr(Angle)
        Dim tmp As Bitmap
        Dim drawBrush As New SolidBrush(Color.FromArgb(151, 255, 255))
        Dim drawpen As New Pen(drawBrush)
        Dim g As Graphics
        Dim R, x, y As Integer
        tmp = New Bitmap(100, 100)
        g = Graphics.FromImage(tmp)
        R = 6
        x = 50 + 49 * Math.Cos(Angle)
        y = 50 - 49 * Math.Sin(Angle)
        g.DrawEllipse(drawpen, 0, 0, 100, 100)
        g.DrawLine(drawpen, New Point(50, 50), New Point(x, y))
        drawBrush.Color = Color.Red
        g.FillEllipse(drawBrush, x - R \ 2, y - R \ 2, R, R)
        PictureBox1.Image = tmp
    End Sub


    Private Sub PictureBox1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp
        mouseD = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        textbox(5).Text = "38555"
        textbox(6).Text = "29510"
        textbox(7).Text = "2600"
        textbox(8).Text = "174.6"
    End Sub

    Private Sub TextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'e.KeyChar == (Char)46 -----> .
        'e.KeyChar == (Char)48 ~ 57 -----> 0~9
        'e.KeyChar == (Char)8 -----------> Backpace
        'e.KeyChar == (Char)13-----------> Enter
        If e.KeyChar = Chr(46) Or e.KeyChar = Chr(48) Or e.KeyChar = Chr(49) Or e.KeyChar = Chr(50) Or e.KeyChar = Chr(51) Or e.KeyChar = Chr(52) Or e.KeyChar = Chr(53) Or e.KeyChar = Chr(54) Or e.KeyChar = Chr(55) Or e.KeyChar = Chr(56) Or e.KeyChar = Chr(57) Or e.KeyChar = Chr(13) Or e.KeyChar = Chr(8) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim tmp(11) As Double
        Dim checker As Boolean
        checker = True

        For i = 1 To 9

            If textbox(i).Text = "" Then
                checker = False
            Else
                tmp(i) = CDbl(textbox(i).Text)
            End If
        Next
        If textbox(0).Text = "" Then
            MsgBox("請設定衛星名稱!", 32, "錯誤")
        ElseIf checker Then

            Form1.launch_rocket(textbox(0).Text, tmp(1), tmp(2), tmp(3), tmp(4), tmp(5), tmp(6), tmp(7), tmp(8), tmp(9))
            Form1.RadioButton3.Checked = True
            Me.Visible = False
            Form3.Visible = True
            Form3.Angle_F = tmp(9)
            Form3.PictureBox2_Draw()
            Form3.VScrollBar1.Value = 0
        Else
            MsgBox("參數不可設定為0!", 32, "錯誤")
        End If

    End Sub
End Class