Imports Microsoft.VisualBasic.PowerPacks
Public Class Form1

    Public Const GC As Double = 6.67384 * 10 ^ -11 '重力係數
    Public Const AU As Double = 149597870700 'm/AU
    'sm 觀察模式
    Public sm As Integer
    Dim c As Integer
    Public sw_List(10) As Button

    '繪圖輸出用變數
    Dim GUI As Bitmap
    Dim l9tmp As String
    Dim zoom As Double
    Dim viewX As Double
    Dim viewY As Double

    '天體浮點運算用物件
    Public sat(10) As Satellite
    Public Solar_Pl(10) As Planet

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AddOwnedForm(Form2)
        Me.AddOwnedForm(Form3)

        '建立衛星選擇按鈕
        Dim i As Integer
        For i = 0 To 9
            sw_List(i) = New Button
            sw_List(i).AutoSize = True
            sw_List(i).Width = 80
            sw_List(i).Height = 15
            sw_List(i).Font = New Font("新細明體", 12.25)
            sw_List(i).ForeColor = Color.Black
            sw_List(i).Text = "字"
            sw_List(i).Tag = i
            sw_List(i).Visible = False
            sw_List(i).Enabled = True

            sw_List(i).Location = New Point(113 + i * 95, 0)

            Me.Controls.Add(sw_List(i))
            AddHandler sw_List(i).Click, AddressOf Me.sw_List_Click
        Next
        sw_List(0).Enabled = False

        '各式參數初始化====================================================


        '衛星資料初始化
        For i = 0 To 9
            sat(i) = New Satellite
            sat(i).Name = "人造衛星"
            sat(i).Enable = False
            sat(i).Crashed = False
            sat(i).Vx = 0
            sat(i).Vy = 0
            sat(i).No = i
        Next

        '行星資料初始化
        For i = 0 To 9
            Solar_Pl(i) = New Planet
        Next
        Solar_Pl(0).X = 0 : Solar_Pl(0).Y = 0

        '初始座標為近拱點
        Solar_Pl(1).X = 46001009 '(km
        Solar_Pl(2).X = 107476170 '(km)
        Solar_Pl(3).X = 147098291 '(km)
        Solar_Pl(4).X = 206655215 '(km)
        Solar_Pl(5).X = 740679835 '(km)
        Solar_Pl(6).X = 1349823615 '(km)
        Solar_Pl(7).X = 2734998229 '(km)
        Solar_Pl(8).X = 4459753056 '(km)
        Solar_Pl(9).X = Solar_Pl(3).X + 363104 '(km)

        For i = 1 To 9
            Solar_Pl(i).X = Solar_Pl(i).X * 1000 ' km轉m
        Next

        'Y座標
        For i = 1 To 9
            Solar_Pl(i).Y = 0
        Next

        '初始速度為最大公轉速度
        For i = 1 To 9
            Solar_Pl(i).Vx = 0  '初始速度為90度之亮相 x分量為0
        Next
        Solar_Pl(1).Vy = 58980  '(m/s)
        Solar_Pl(2).Vy = 35260  '(m/s)
        Solar_Pl(3).Vy = 30290  '(m/s)
        Solar_Pl(4).Vy = 26500  '(m/s)
        Solar_Pl(5).Vy = 13720  '(m/s)
        Solar_Pl(6).Vy = 10180  '(m/s)
        Solar_Pl(7).Vy = 7110  '(m/s)
        Solar_Pl(8).Vy = 5500  '(m/s)
        Solar_Pl(9).Vy = Solar_Pl(3).Vy + 1076  '(m/s)

        '天體名稱
        Solar_Pl(0).Name = "太陽"
        Solar_Pl(1).Name = "水星"
        Solar_Pl(2).Name = "金星"
        Solar_Pl(3).Name = "地球"
        Solar_Pl(4).Name = "火星"
        Solar_Pl(5).Name = "木星"
        Solar_Pl(6).Name = "土星"
        Solar_Pl(7).Name = "天王星"
        Solar_Pl(8).Name = "海王星"
        Solar_Pl(9).Name = "月球"

        '半徑
        Solar_Pl(0).r = 696000   'km
        Solar_Pl(1).r = 2439.7
        Solar_Pl(2).r = 6052
        Solar_Pl(3).r = 6378
        Solar_Pl(4).r = 3397
        Solar_Pl(5).r = 71492
        Solar_Pl(6).r = 60268
        Solar_Pl(7).r = 25559
        Solar_Pl(8).r = 24764
        Solar_Pl(9).r = 1738.14
        For i = 0 To 9
            Solar_Pl(i).r = Solar_Pl(i).r * 1000    'm
        Next

        '真實比例
        '平均公轉半徑   1AU = 149,597,871km
        'Solar_Pl(1).d = 0.3871
        'Solar_Pl(2).d = 0.7233
        'Solar_Pl(3).d = 1
        'Solar_Pl(4).d = 1.5237
        'Solar_Pl(5).d = 5.2026
        'Solar_Pl(6).d = 9.5549
        'Solar_Pl(7).d = 19.2184
        'Solar_Pl(8).d = 30.1104
        'Solar_Pl(9).d = 0.0027
        'For i = 1 To 9
        '    Solar_Pl(i).d = Solar_Pl(i).d * AU  'm
        'Next

        '公轉週期
        'Solar_Pl(1).cr = 87.97
        'Solar_Pl(2).cr = 225
        'Solar_Pl(3).cr = 365.24
        'Solar_Pl(4).cr = 687
        'Solar_Pl(5).cr = 365.24 * 11.86
        'Solar_Pl(6).cr = 365.24 * 29.46
        'Solar_Pl(7).cr = 365.24 * 84
        'Solar_Pl(8).cr = 365.24 * 164
        'Solar_Pl(9).cr = 29

        'For i = 1 To 9
        '    Solar_Pl(i).cr = Solar_Pl(9I).cr * 24 * 60 * 60 '將公轉單位由天改成小時 分鐘 秒
        'Next

        '質量
        Solar_Pl(0).m = 333400   '太陽
        Solar_Pl(1).m = 0.055 '水星
        Solar_Pl(2).m = 0.815 '金星
        Solar_Pl(3).m = 1 '地球 5.9742×10^24 kg
        Solar_Pl(4).m = 0.107 '火星
        Solar_Pl(5).m = 317.832 '木星
        Solar_Pl(6).m = 95.16 '土星
        Solar_Pl(7).m = 14.54 '天王星
        Solar_Pl(8).m = 17.15 '海王星
        Solar_Pl(9).m = 0.0123 '月球

        For i = 0 To 9
            Solar_Pl(i).m = Solar_Pl(i).m * 5.9742 * 10 ^ 24  '地球比例 轉 kg
        Next

        '================================================================

        'GUI視窗顯示設定
        DoubleBuffered = True
        PictureBox1.Height = Me.Height - (PictureBox1.Top + 2)
        PictureBox1.Width = Me.Width - (PictureBox1.Left + 2)
        Button2.Top = Me.Height - (Button2.Height + 2)
        TextBox1.Top = Me.Height - (TextBox1.Height + 2)
        zoom = 0.001
        GUI = New Bitmap(PictureBox1.Width, PictureBox1.Height)
        sm = 10
        viewX = 0 : viewY = 0


    End Sub
    '運算用timer
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        For i = 1 To (Val(Label6.Text))
            Mathteacher()
        Next

    End Sub

    '浮點運算用
    Sub Mathteacher()
        Dim Fx, Fy As Double


        '衛星重力
        For j = 0 To 9
            '判斷使否是在使用中
            If sat(j).Enable Then
                If Not sat(j).Crashed Then '沒有墜地
                    '推進加速度演算
                    If sm \ 10 = 3 And j = (sm Mod 30) Then  '處於衛星操作介面
                        Dim Burn_Rate As Double
                        Burn_Rate = sat(j).SlaveT_Sat.Burn_Rate * (100 - Form3.VScrollBar1.Value) / 100 '可控的燃燒速率
                        If sat(j).SlaveT_Sat.Fuel = 0 Then
                            '沒燃料啦!
                        ElseIf sat(j).SlaveT_Sat.Fuel < Burn_Rate Then
                            sat(j).Vx = sat(j).Vx + ((sat(j).SlaveT_Sat.Sp * sat(j).SlaveT_Sat.Fuel) / sat(j).mT) * Math.Cos(Form3.Angle_F) '燃料轉換成加速度
                            sat(j).Vy = sat(j).Vy + ((sat(j).SlaveT_Sat.Sp * sat(j).SlaveT_Sat.Fuel) / sat(j).mT) * Math.Sin(Form3.Angle_F)
                            sat(j).SlaveT_Sat.Fuel = 0
                        Else
                            sat(j).Vx = sat(j).Vx + ((sat(j).SlaveT_Sat.Sp * Burn_Rate) / sat(j).mT) * Math.Cos(Form3.Angle_F) '燃料轉換成加速度
                            sat(j).Vy = sat(j).Vy + ((sat(j).SlaveT_Sat.Sp * Burn_Rate) / sat(j).mT) * Math.Sin(Form3.Angle_F)
                            sat(j).SlaveT_Sat.Fuel = sat(j).SlaveT_Sat.Fuel - Burn_Rate    '燃燒燃料
                        End If
                    End If
                    ' 遵從太陽系重力原則
                    If sat(j).Master Then '判斷是否為被動連接衛星(連接了領導者)
                        sat(j).Vx = sat(j).MasterT_Sat.Vx
                        sat(j).Vy = sat(j).MasterT_Sat.Vy
                        sat(j).X = sat(j).MasterT_Sat.X
                        sat(j).Y = sat(j).MasterT_Sat.Y
                    Else    '普通衛星/主動衛星 遵從物理運算
                        Fx = 0 : Fy = 0
                        For i = 0 To 9
                            Fx = Fx + (GC * sat(j).mT * Solar_Pl(i).m / ((sat(j).X - Solar_Pl(i).X) ^ 2 + (sat(j).Y - Solar_Pl(i).Y) ^ 2)) * Math.Cos(Math.Atan2(Solar_Pl(i).Y - sat(j).Y, Solar_Pl(i).X - sat(j).X)) '* Math.Cos(Math.Atan((y(10) - Solar_Pl(i).Y) / (x(10) - Solar_Pl(i).X)))
                            Fy = Fy + (GC * sat(j).mT * Solar_Pl(i).m / ((sat(j).X - Solar_Pl(i).X) ^ 2 + (sat(j).Y - Solar_Pl(i).Y) ^ 2)) * Math.Sin(Math.Atan2(Solar_Pl(i).Y - sat(j).Y, Solar_Pl(i).X - sat(j).X))
                        Next
                        sat(j).push(Fx, Fy)
                        Fx = 0 : Fy = 0
                        sat(j).X = sat(j).X + sat(j).Vx
                        sat(j).Y = sat(j).Y + sat(j).Vy
                    End If
                End If
            End If
        Next

        c = c + 1
        '行星重力
        Fx = 0 : Fy = 0
        For j = 1 To 9 '1~9號星球
            For i = 0 To 9 'j星球受到了哪些作用力
                If i <> j Then
                    Fx = Fx + (GC * Solar_Pl(j).m * Solar_Pl(i).m / ((Solar_Pl(j).X - Solar_Pl(i).X) ^ 2 + (Solar_Pl(j).Y - Solar_Pl(i).Y) ^ 2)) * Math.Cos(Math.Atan2(Solar_Pl(i).Y - Solar_Pl(j).Y, Solar_Pl(i).X - Solar_Pl(j).X))
                    Fy = Fy + (GC * Solar_Pl(j).m * Solar_Pl(i).m / ((Solar_Pl(j).X - Solar_Pl(i).X) ^ 2 + (Solar_Pl(j).Y - Solar_Pl(i).Y) ^ 2)) * Math.Sin(Math.Atan2(Solar_Pl(i).Y - Solar_Pl(j).Y, Solar_Pl(i).X - Solar_Pl(j).X))
                End If
            Next

            Solar_Pl(j).push(Fx, Fy)
            Fx = 0 : Fy = 0
            Solar_Pl(j).X = Solar_Pl(j).X + Solar_Pl(j).Vx
            Solar_Pl(j).Y = Solar_Pl(j).Y + Solar_Pl(j).Vy
        Next

        For j = 0 To 9
            '判斷使否是在使用中
            If sat(j).Enable Then
                '判斷是否墜毀 
                If Not sat(j).Crashed Then
                    For i = 0 To 9
                        If (((sat(j).X - Solar_Pl(i).X) ^ 2 + (sat(j).Y - Solar_Pl(i).Y) ^ 2) < Solar_Pl(i).r ^ 2) Then
                            sat(j).Crashed = True
                            sat(j).Crashed_In = i
                            sat(j).Crashed_Angle = Math.Atan2(sat(j).Y - Solar_Pl(sat(j).Crashed_In).Y, sat(j).X - Solar_Pl(sat(j).Crashed_In).X)
                            sat(j).Vx = 0 : sat(j).Vy = 0
                        End If
                    Next
                End If

                If sat(j).Crashed Then '墜毀後跟著行星移動的物理運算
                    sat(j).X = Solar_Pl(sat(j).Crashed_In).X + Math.Cos(sat(j).Crashed_Angle) * (Solar_Pl(sat(j).Crashed_In).r)
                    sat(j).Y = Solar_Pl(sat(j).Crashed_In).Y + Math.Sin(sat(j).Crashed_Angle) * (Solar_Pl(sat(j).Crashed_In).r)
                End If

            End If
        Next
    End Sub


    'Layout 用Timer
    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        '數據輸出
        l9tmp = "SatelliteLaunchGame - " & "自從太陽系誕生後已經經過 第" & Int(c / 86164.09) & "個地球天||" & Int((c / 60 / 60) Mod 24) & "地球時||" & Int((c / 60) Mod 60) & "地球分||" & Int(c Mod 60) & "地球秒||" & vbNewLine
        'l9tmp += "地球速度:" & (Solar_Pl(3).Vx ^ 2 + Solar_Pl(3).Vy ^ 2) ^ 0.5 & "m/s" & vbNewLine
        'l9tmp += "操作模式:" & sm & vbNewLine
        'l9tmp += "放大倍率:" & zoom & vbNewLine
        'For i = 0 To 9
        '    l9tmp += i & "號 人造衛星是否可用" & sat(i).Enable & vbNewLine
        'Next
        For i = 0 To 9
            If sat(i).Enable And sm Mod 30 = i Then
                l9tmp += "_________________" & i & "號 人造衛星參數_________________" & vbNewLine
                l9tmp += sat(i).X & "||" & sat(i).Y & vbNewLine
                l9tmp += "人造衛星名稱:" & sat(i).Name & vbNewLine
                l9tmp += "人造衛星是否墜毀" & sat(i).Crashed & vbNewLine
                If sat(i).Crashed Then
                    l9tmp += "人造衛星墜毀於:" & Solar_Pl(sat(i).Crashed_In).Name & vbNewLine
                End If
                l9tmp += "剩餘燃料:" & sat(i).Fuel & "(kg)" & vbNewLine
                If sat(i).Slave Then
                    l9tmp += "推進級剩餘燃料:" & sat(i).SlaveT_Sat.Fuel & "(kg)" & vbNewLine
                End If
                'l9tmp += "人造衛星速度:" & (sat(i).Vx ^ 2 + sat(i).Vy ^ 2) ^ 0.5 & "m/s" & vbNewLine
                'l9tmp += "衛星與地心的距離" & ((sat(i).X - Solar_Pl(3).X) ^ 2 + (sat(i).Y - Solar_Pl(3).Y) ^ 2) ^ 0.5 & vbNewLine '(m)
                'l9tmp += "衛星相對於地面的高度:" & ((sat(i).X - Solar_Pl(3).X) ^ 2 + (sat(i).Y - Solar_Pl(3).Y) ^ 2) ^ 0.5 - Solar_Pl(3).r & vbNewLine '(m)
                'l9tmp += "人造衛星相對速度:" & ((sat(i).Vx - Solar_Pl(3).Vx) ^ 2 + (sat(i).Vy - Solar_Pl(3).Vy) ^ 2) ^ 0.5 & "m/s" & vbNewLine
                l9tmp += "是否有領導者:" & sat(i).Master & vbNewLine
                l9tmp += "是否有追隨者:" & sat(i).Slave & vbNewLine
                l9tmp += "總質量:" & sat(i).mT & "(kg)" & vbNewLine
            End If
        Next

        '依照操作模式決定繪圖方式
        If sm \ 10 = 1 Then
            viewX = 0 : viewY = 0
            draw_GUI()
        ElseIf sm \ 10 = 2 Then
            viewX = Solar_Pl(3).X : viewY = Solar_Pl(3).Y
            draw_GUI()
        ElseIf sm \ 10 = 3 Then
            viewX = sat(sm Mod 30).X : viewY = sat(sm Mod 30).Y
            draw_GUI()
        End If
    End Sub

    Sub draw_GUI()
        '開始繪圖================================================
        Dim drawFont As New Font("Arial", 9)
        Dim drawBrush As New SolidBrush(Color.Black)
        Dim drawpen As New Pen(drawBrush)
        Dim g As Graphics
        g = Graphics.FromImage(GUI)
        g.Clear(Color.FromArgb(32, 32, 32))
        '繪製圓形(線框無填滿)
        'drawpen.Color = Color.Yellow
        'e.Graphics.DrawEllipse(drawpen, Me.Width \ 2, Me.Height \ 2, 10, 10)

        '繪製太陽
        If (Solar_Pl(0).r * 2 / AU * zoom > 2) Then
            drawBrush.Color = Color.FromArgb(255, 248, 202)
            Dim R, x, y As Integer
            R = Int(Solar_Pl(0).r * 2 / AU * zoom)
            x = Int((Solar_Pl(0).X - viewX) / AU * zoom)
            y = -Int((Solar_Pl(0).Y - viewY) / AU * zoom)
            g.FillEllipse(drawBrush, PictureBox1.Width \ 2 + x - R \ 2, PictureBox1.Height \ 2 + y - R \ 2, R, R)
        End If
        '繪製行星
        drawBrush.Color = Color.FromArgb(151, 255, 255)
        For i = 1 To 9
            Dim R, x, y As Long
            R = Int(Solar_Pl(i).r * 2 / AU * zoom)
            x = Int((Solar_Pl(i).X - viewX) / AU * zoom)
            y = -Int((Solar_Pl(i).Y - viewY) / AU * zoom)
            g.FillEllipse(drawBrush, PictureBox1.Width \ 2 + x - R \ 2, PictureBox1.Height \ 2 + y - R \ 2, R, R)
        Next
        '繪製行星指標
        If CheckBox1.Checked Then
            For i = 0 To 9
                If Math.Abs((Solar_Pl(i).X - viewX) / AU * zoom) < PictureBox1.Width / 2 And Math.Abs((Solar_Pl(i).Y - viewY) / AU * zoom) < PictureBox1.Height / 2 Then
                    Dim x, y As Integer
                    x = Int((Solar_Pl(i).X - viewX) / AU * zoom)
                    y = -Int((Solar_Pl(i).Y - viewY) / AU * zoom)
                    g.DrawImageUnscaled(New Bitmap(Image.FromFile("Tilesets/frame0.png")), New Point(PictureBox1.Width \ 2 + x - 64 \ 2, PictureBox1.Height \ 2 + y - 64 \ 2))
                End If
            Next
        End If
        '繪製行星名稱
        If CheckBox2.Checked Then
            drawBrush.Color = Color.FromArgb(64, 225, 225)
            drawFont = New Font("華康儷細黑", 12)
            For i = 0 To 9
                If Math.Abs((Solar_Pl(i).X - viewX) / AU * zoom) < PictureBox1.Width / 2 And Math.Abs((Solar_Pl(i).Y - viewY) / AU * zoom) < PictureBox1.Height / 2 Then
                    Dim x, y As Integer
                    x = Int((Solar_Pl(i).X - viewX) / AU * zoom)
                    y = -Int((Solar_Pl(i).Y - viewY) / AU * zoom)
                    If CheckBox1.Checked Then
                        g.DrawString(Solar_Pl(i).Name, drawFont, drawBrush, PictureBox1.Width \ 2 + x - 32, PictureBox1.Height \ 2 + y + 32)
                    Else
                        g.DrawString(Solar_Pl(i).Name, drawFont, drawBrush, PictureBox1.Width \ 2 + x, PictureBox1.Height \ 2 + y)
                    End If
                End If
            Next
        End If

        '繪製衛星指標
        If CheckBox3.Checked Then
            For i = 0 To 9
                If sat(i).Enable And Math.Abs((sat(i).X - viewX) / AU * zoom) < PictureBox1.Width / 2 And Math.Abs((sat(i).Y - viewY) / AU * zoom) < PictureBox1.Height / 2 Then
                    Dim x, y As Integer
                    x = Int((sat(i).X - viewX) / AU * zoom)
                    y = -Int((sat(i).Y - viewY) / AU * zoom)

                    If Not sat(i).Master Then
                        If i = (sm Mod 30) Then
                            g.DrawImageUnscaled(New Bitmap(Image.FromFile("Tilesets/frame1.png")), New Point(PictureBox1.Width \ 2 + x - 64 \ 2, PictureBox1.Height \ 2 + y - 64 \ 2))
                        ElseIf sat(i).Slave Then
                            If sat(i).Slave_Sat.No = sm Mod 30 Then
                                g.DrawImageUnscaled(New Bitmap(Image.FromFile("Tilesets/frame1.png")), New Point(PictureBox1.Width \ 2 + x - 64 \ 2, PictureBox1.Height \ 2 + y - 64 \ 2))
                            End If
                        Else
                            g.DrawImageUnscaled(New Bitmap(Image.FromFile("Tilesets/frame0.png")), New Point(PictureBox1.Width \ 2 + x - 64 \ 2, PictureBox1.Height \ 2 + y - 64 \ 2))
                        End If
                    End If
                End If
            Next
        End If
        '繪製衛星名稱
        If CheckBox4.Checked Then
            For i = 0 To 9
                If sat(i).Enable And Math.Abs((sat(i).X - viewX) / AU * zoom) < PictureBox1.Width / 2 And Math.Abs((sat(i).Y - viewY) / AU * zoom) < PictureBox1.Height / 2 Then
                    drawFont = New Font("華康儷細黑", 12)
                    Dim x, y As Integer
                    x = Int((sat(i).X - viewX) / AU * zoom)
                    y = -Int((sat(i).Y - viewY) / AU * zoom)
                    If Not sat(i).Master Then
                        If i = (sm Mod 30) Then
                            drawBrush.Color = Color.FromArgb(245, 56, 22)
                            If CheckBox3.Checked Then
                                g.DrawString(sat(i).Name, drawFont, drawBrush, PictureBox1.Width \ 2 + x - 32, PictureBox1.Height \ 2 + y + 32)
                            Else
                                g.DrawString(sat(i).Name, drawFont, drawBrush, PictureBox1.Width \ 2 + x, PictureBox1.Height \ 2 + y)
                            End If
                        ElseIf sat(i).Slave Then
                            If CheckBox3.Checked Then
                                g.DrawString(sat(i).Name, drawFont, drawBrush, PictureBox1.Width \ 2 + x - 32, PictureBox1.Height \ 2 + y + 32)
                            Else
                                g.DrawString(sat(i).Name, drawFont, drawBrush, PictureBox1.Width \ 2 + x, PictureBox1.Height \ 2 + y)
                            End If
                        Else
                            drawBrush.Color = Color.FromArgb(64, 225, 225)
                            If CheckBox3.Checked Then
                                g.DrawString(sat(i).Name, drawFont, drawBrush, PictureBox1.Width \ 2 + x - 32, PictureBox1.Height \ 2 + y + 32)
                            Else
                                g.DrawString(sat(i).Name, drawFont, drawBrush, PictureBox1.Width \ 2 + x, PictureBox1.Height \ 2 + y)
                            End If
                        End If
                    End If
                End If
            Next
        End If

        drawBrush.Color = Color.FromArgb(64, 225, 225)
        'e.Graphics.DrawString(l9tmp, drawFont, drawBrush, 1, 1)
        drawFont = New Font("Arial", 9)
        g.DrawString(l9tmp, drawFont, drawBrush, 1, 1)

        PictureBox1.Image = GUI
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'End
        Form3.Dispose()
        Form2.Dispose()
        Me.Dispose()
    End Sub

    Private Sub RadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged, RadioButton3.CheckedChanged
        sm = CType(sender, RadioButton).Tag * 10 + sm Mod 10
        If sm \ 10 = 1 Then
            Button1.Enabled = False
            For i = 0 To 9
                sw_List(i).Visible = False
            Next
            Form3.Visible = False
        ElseIf sm \ 10 = 2 Then
            Button1.Enabled = True
            For i = 0 To 9
                sw_List(i).Visible = False
            Next
            Form3.Visible = False
        ElseIf sm \ 10 = 3 Then
            Button1.Enabled = False
            sat_b_reflash()

        End If
        Button1.Enabled = RadioButton3.Checked
        Button3.Enabled = RadioButton2.Checked
    End Sub

    Sub sat_b_reflash()
        Dim k, j As Integer
        k = 113 : j = 113
        For i = 0 To 9
            If sat(i).Enable Then
                sw_List(i).Width = 80
                sw_List(i).Height = 15
                sw_List(i).Visible = True
                '按鈕重繪 master衛星和slave衛星分開
                If Not sat(i).Master Then
                    sw_List(i).Top = 0
                    sw_List(i).Left = k
                    k = k + sw_List(i).Width
                Else
                    sw_List(i).Top = 26
                    sw_List(i).Left = j
                    j = j + sw_List(i).Width
                End If
            Else
                sw_List(i).Visible = False
            End If

        Next
    End Sub

    Public Sub launch_rocket(ByVal name As String, ByVal m As Double, ByVal fuel As Double, ByVal SP As Double, ByVal BR As Double, ByVal Rm As Double, ByVal Rfuel As Double, ByVal RSP As Double, ByVal RBR As Double, ByVal Angle As Double)

        Dim i, j, k As Integer
        i = 0
        While (sat(i).Enable)
            i = i + 1
        End While
        j = i
        sat(j).Enable = True '衛星

        i = 0
        While (sat(i).Enable)
            i = i + 1
        End While
        k = i
        sat(k).Enable = True '運載火箭

        sat(j).Name = name  '名稱
        sat(j).Crashed = False
        sat(j).m = m        '質量
        sat(j).Fuel = fuel  '燃料
        sat(j).Sp = SP      '比衝
        sat(j).Burn_Rate = BR   '燃燒速度

        sat(k).Name = name & "的運載火箭"
        sat(k).Crashed = False
        sat(k).m = Rm
        sat(k).Fuel = Rfuel
        sat(k).Sp = RSP
        sat(k).Burn_Rate = RBR
        '建立連接
        sat(j).connect(sat(k))


        '從地表發射
        sat(j).X = Solar_Pl(3).X + Math.Cos(Angle) * (Solar_Pl(3).r + 10) '地面1公尺高
        sat(j).Y = Solar_Pl(3).Y + Math.Sin(Angle) * (Solar_Pl(3).r + 10)
        sat(k).X = sat(j).X
        sat(k).Y = sat(j).Y
        '地球本身的自帶力
        'Ax = 0 : Ay = 0
        sat(j).Vx = Solar_Pl(3).Vx  '地球平均速度(km/hr) / 一天文單位(km)   /km/s
        sat(j).Vy = Solar_Pl(3).Vy
        '發射所給予的加速度

        sm = 30 + j
        sw_List(j).Text = sat(j).Name
        sw_List(k).Text = sat(k).Name
        button_reflash(j)
        RadioButton3.Enabled = True
        Button1.Enabled = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Form3.Visible = True
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Form3.Visible = False
        Form2.Visible = True
    End Sub
    Private Sub sw_List_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '設定顯示介面模式
        sm = 30 + CType(sender, Button).Tag
        '重設按鈕顯示狀態
        For i = 0 To 9
            sw_List(i).Enabled = True
        Next
        CType(sender, Button).Enabled = False
    End Sub
    '按鈕重繪
    Sub button_reflash(ByVal h As Integer)
        For i = 0 To 9
            sw_List(i).Enabled = True
        Next
        sw_List(h).Enabled = False
    End Sub

    '滾條
    '縮時倍率
    Private Sub VScrollBar3_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles VScrollBar3.Scroll
        Label6.Text = 100 - VScrollBar3.Value
    End Sub
    '比例縮放
    Private Sub VScrollBar4_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles VScrollBar4.Scroll
        Label8.Text = 1000 - VScrollBar4.Value
        zoom = Val(Label8.Text) ^ 2.5 / (1001 - Val(Label8.Text))
    End Sub

End Class
