Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace VS2005Style
    Public Class DrawHelper
        Public Shared bshift As Integer = 8
        Public Shared Sub DrawTab(ByVal g As Graphics, ByVal r As Rectangle, ByVal corner As Corners, ByVal gradient As GradientType, ByVal darkColor As Color, ByVal lightColor As Color, _
         ByVal edgeColor As Color, ByVal closed As Boolean)
            'dims
            Dim points As Point()
            Dim path As GraphicsPath
            Dim region As Region
            Dim linearBrush As LinearGradientBrush
            Dim brush As Brush = Nothing
            Dim pen As Pen
            r.Inflate(-1, -1)
            'set brushes
            Select Case gradient
                Case GradientType.Flat
                    brush = New SolidBrush(darkColor)
                    Exit Select
                Case GradientType.Linear
                    brush = New LinearGradientBrush(r, darkColor, lightColor, LinearGradientMode.Vertical)
                    Exit Select
                Case GradientType.Bell
                    linearBrush = New LinearGradientBrush(r, darkColor, lightColor, LinearGradientMode.Vertical)
                    linearBrush.SetSigmaBellShape(0.17F, 0.67F)
                    brush = linearBrush
                    Exit Select
            End Select
            pen = New Pen(edgeColor, 1)
            'generic points
            points = New Point(11) {New Point(r.Left, r.Bottom), _
                                    New Point(r.Left, r.Bottom - bshift), _
                                    New Point(r.Left, r.Top + bshift), _
                                    New Point(r.Left, r.Top), _
                                    New Point(r.Left + bshift, r.Top), _
                                    New Point(r.Right - bshift, r.Top), _
                                    New Point(r.Right, r.Top), _
                                    New Point(r.Right, r.Top + bshift), _
                                    New Point(r.Right, r.Bottom - bshift), _
                                    New Point(r.Right, r.Bottom), _
                                    New Point(r.Right - bshift, r.Bottom), _
                                    New Point(r.Left + bshift, r.Bottom)}

            path = New GraphicsPath()
            Select Case corner
                Case Corners.LeftBottom
                    path.AddLine(points(3), points(1))
                    path.AddBezier(points(1), points(0), points(0), points(11))
                    path.AddLine(points(11), points(9))
                    path.AddLine(points(9), points(6))
                    path.AddLine(points(6), points(3))
                    region = New Region(path)
                    g.FillRegion(brush, region)
                    g.DrawLine(pen, points(3), points(1))
                    g.DrawBezier(pen, points(1), points(0), points(0), points(11))
                    g.DrawLine(pen, points(11), points(9))
                    g.DrawLine(pen, points(9), points(6))
                    If closed Then
                        g.DrawLine(pen, points(6), points(3))
                    End If
                    Exit Select
                Case Corners.LeftTop
                    path.AddLine(points(0), points(2))
                    path.AddBezier(points(2), points(3), points(3), points(4))
                    path.AddLine(points(4), points(6))
                    path.AddLine(points(6), points(9))
                    path.AddLine(points(9), points(0))
                    region = New Region(path)
                    g.FillRegion(brush, region)
                    g.DrawLine(pen, points(0), points(2))
                    g.DrawBezier(pen, points(2), points(3), points(3), points(4))
                    g.DrawLine(pen, points(4), points(6))
                    g.DrawLine(pen, points(6), points(9))
                    If closed Then
                        g.DrawLine(pen, points(9), points(0))
                    End If
                    Exit Select
                Case Corners.Bottom

                    path.AddLine(points(1), points(3))
                    path.AddBezier(points(1), points(0), points(0), points(11))
                    path.AddLine(points(11), points(10))
                    path.AddBezier(points(10), points(9), points(9), points(8))
                    path.AddLine(points(8), points(6))
                    path.AddLine(points(6), points(3))
                    region = New Region(path)
                    g.FillRegion(brush, region)

                    g.DrawLine(pen, points(1), points(3))
                    g.DrawBezier(pen, points(1), points(0), points(0), points(11))
                    g.DrawLine(pen, points(11), points(10))
                    g.DrawBezier(pen, points(10), points(9), points(9), points(8))
                    g.DrawLine(pen, points(8), points(6))

                    If closed Then
                        g.DrawLine(pen, points(6), points(3))
                    End If

                    Exit Select

                Case Corners.Top
                    path.AddLine(points(0), points(2))
                    path.AddBezier(points(2), points(3), points(3), points(4))
                    path.AddLine(points(4), points(5))
                    path.AddBezier(points(5), points(6), points(6), points(7))
                    path.AddLine(points(7), points(9))
                    path.AddLine(points(9), points(0))
                    region = New Region(path)
                    g.FillRegion(brush, region)

                    g.DrawLine(pen, points(0), points(2))
                    g.DrawBezier(pen, points(2), points(3), points(3), points(4))
                    g.DrawLine(pen, points(4), points(5))
                    g.DrawBezier(pen, points(5), points(6), points(6), points(7))
                    g.DrawLine(pen, points(7), points(9))

                    If closed Then
                        g.DrawLine(pen, points(9), points(0))
                    End If

                    Exit Select

                Case Corners.RightBottom
                    path.AddLine(points(3), points(0))
                    path.AddLine(points(0), points(10))
                    path.AddBezier(points(10), points(9), points(9), points(8))
                    path.AddLine(points(8), points(6))
                    path.AddLine(points(6), points(3))
                    region = New Region(path)
                    g.FillRegion(brush, region)
                    g.DrawLine(pen, points(3), points(0))
                    g.DrawLine(pen, points(0), points(10))
                    g.DrawBezier(pen, points(10), points(9), points(9), points(8))
                    g.DrawLine(pen, points(8), points(6))
                    If closed Then
                        g.DrawLine(pen, points(6), points(3))
                    End If
                    Exit Select
                Case Corners.RightTop
                    path.AddLine(points(0), points(3))
                    path.AddLine(points(3), points(5))
                    path.AddBezier(points(5), points(6), points(6), points(7))
                    path.AddLine(points(7), points(9))
                    path.AddLine(points(9), points(0))
                    region = New Region(path)
                    g.FillRegion(brush, region)
                    g.DrawLine(pen, points(0), points(3))
                    g.DrawLine(pen, points(3), points(5))
                    g.DrawBezier(pen, points(5), points(6), points(6), points(7))
                    g.DrawLine(pen, points(7), points(9))
                    If closed Then
                        g.DrawLine(pen, points(9), points(0))
                    End If
                    Exit Select
            End Select
        End Sub
        Public Shared Sub DrawDocumentTab(ByVal g As Graphics, ByVal rect As Rectangle, ByVal backColorBegin As Color, ByVal backColorEnd As Color, ByVal edgeColor As Color, ByVal tabType As TabDrawType, ByVal closed As Boolean)
            Dim path As GraphicsPath
            Dim region As Region
            Dim brush As Brush = Nothing
            Dim pen As Pen
            brush = New LinearGradientBrush(rect, backColorBegin, backColorEnd, LinearGradientMode.Vertical)
            pen = New Pen(edgeColor, 1.0F)
            path = New GraphicsPath()

            If tabType = TabDrawType.First Then
                path.AddLine(rect.Left + 1, rect.Bottom + 1, rect.Left + rect.Height, rect.Top + 2)
                path.AddLine(rect.Left + rect.Height + 4, rect.Top, rect.Right - 3, rect.Top)
                path.AddLine(rect.Right - 1, rect.Top + 2, rect.Right - 1, rect.Bottom + 1)
            Else
                If tabType = TabDrawType.Active Then
                    path.AddLine(rect.Left + 1, rect.Bottom + 1, rect.Left + rect.Height, rect.Top + 2)
                    path.AddLine(rect.Left + rect.Height + 4, rect.Top, rect.Right - 3, rect.Top)
                    path.AddLine(rect.Right - 1, rect.Top + 2, rect.Right - 1, rect.Bottom + 1)
                Else
                    path.AddLine(rect.Left, rect.Top + 6, rect.Left + 4, rect.Top + 2)
                    path.AddLine(rect.Left + 8, rect.Top, rect.Right - 3, rect.Top)
                    path.AddLine(rect.Right - 1, rect.Top + 2, rect.Right - 1, rect.Bottom + 1)
                    path.AddLine(rect.Right - 1, rect.Bottom + 1, rect.Left, rect.Bottom + 1)
                End If
            End If
            region = New Region(path)
            g.FillRegion(brush, region)
            g.DrawPath(pen, path)
        End Sub
    End Class
    Public Enum Corners
        RightTop
        LeftTop
        LeftBottom
        RightBottom
        Bottom
        Top
    End Enum
    Public Enum TabDrawType
        First
        Active
        Inactive
    End Enum
    Public Enum GradientType
        Flat
        Linear
        Bell
    End Enum
End Namespace
