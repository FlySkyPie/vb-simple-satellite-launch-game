Public Class Satellite

    Public X, Y, Vx, Vy, m, Fuel, Sp, Burn_Rate As Double    'X,Y座標;加速度向量Vx,Vy;質量m ;燃料;比衝Specific impulse;燃燒率
    Public Name As String   '衛星名稱
    Public No As Integer    '衛星編號
    Public Enable As Boolean    '使否在使用中(已發射)
    Public Crashed As Boolean   '是否墜毀
    Public Crashed_In As Integer    '墜毀於哪顆行星
    Public Crashed_Angle As Double  '墜毀於何處

    '主動衛星為核心 被動衛星的參數必須跟著主動衛星跑
    Public Master As Boolean   '是否連接了主動衛星
    Public Slave As Boolean   '是否連接了被動衛星

    Public Master_Sat As Satellite
    Public Slave_Sat As Satellite

    Public Sub push(ByVal Fx As Double, ByVal Fy As Double)
        Dim Ax, Ay As Double
        Ax = Fx / mT() : Ay = Fy / mT()
        Vx = Vx + Ax : Vy = Vy + Ay
    End Sub

    Public Sub connect(ByRef consat As Satellite)
        If consat.Enable And consat.Enable Then
            '主動方連接參數
            Me.Slave = True '有追隨者衛星
            Me.Slave_Sat = consat   '設定追隨者衛星

            '被動方連接參數
            consat.Master = True    '有領導衛星
            consat.Master_Sat = Me  '設定領導衛星
        End If
    End Sub
    '註銷連接
    Public Sub disconnect()
        '被動方連接參數
        Me.Slave_Sat.Master = False
        Me.Slave_Sat.Master_Sat = Nothing   '釋放追隨者

        '主動方連接參數
        Me.Slave = False
        Me.Slave_Sat = Nothing
    End Sub

    Public Function mT() As Double
        If Me.Slave Then
            mT = m + Fuel + Me.Slave_Sat.mT
        Else
            mT = m + Fuel
        End If
    End Function
    Public Function MasterT_Sat() As Satellite

        If Master Then
            Return Me.Master_Sat
        Else
            Return Me
        End If
    End Function
    Public Function SlaveT_Sat() As Satellite
        If Slave Then
            Return Me.Slave_Sat
        Else
            Return Me
        End If
    End Function
    Public Sub release() '釋放衛星資源
        If Me.Slave Then
            Me.Slave_Sat.release()
        End If
        Me.Enable = False
        Me.Crashed = False
    End Sub


End Class
