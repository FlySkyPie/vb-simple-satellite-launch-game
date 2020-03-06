Public Class Planet
    '簡化版太陽系用參數
    Public d, cr As Double  '平均半徑,公轉週期
    '接近精密太陽系用參數
    Public Vx, Vy, Ap, Pe As Double '速度向量  近拱點 ,遠拱點
    '共用參數
    Public X, Y, m, r As Double 'X,Y軸座標 ;質量;半徑
    '星體名稱
    Public Name As String

    Public Sub push(ByVal Fx As Double, ByVal Fy As Double)
        Dim Ax, Ay As Double
        Ax = Fx / m : Ay = Fy / m
        Vx = Vx + Ax : Vy = Vy + Ay
    End Sub
End Class
