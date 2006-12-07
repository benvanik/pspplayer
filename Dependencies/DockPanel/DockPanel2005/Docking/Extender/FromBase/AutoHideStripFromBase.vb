' *****************************************************************************
' 
'  Copyright 2004, Weifen Luo
'  All rights reserved. The software and associated documentation 
'  supplied hereunder are the proprietary information of Weifen Luo
'  and are supplied subject to licence terms.
' 
'  WinFormsUI Library Version 1.0
' *****************************************************************************
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports WeifenLuo.WinFormsUI

Namespace VS2005Style
    <ToolboxItem(False)> _
    Public Class AutoHideStripFromBase
        Inherits AutoHideStripBase
        Private Const _ImageHeight As Integer = 16
        Private Const _ImageWidth As Integer = 16
        Private Const _ImageGapTop As Integer = 2
        Private Const _ImageGapLeft As Integer = 4
        Private Const _ImageGapRight As Integer = 4
        Private Const _ImageGapBottom As Integer = 2
        Private Const _TextGapLeft As Integer = 4
        Private Const _TextGapRight As Integer = 10
        Private Const _TabGapTop As Integer = 3
        Private Const _TabGapLeft As Integer = 2
        Private Const _TabGapBetween As Integer = 10
        Private Shared _stringFormatTabHorizontal As StringFormat
        Private Shared _stringFormatTabVertical As StringFormat
        Private Shared _matrixIdentity As Matrix
        Private Shared _dockStates As DockState()
#Region "Customizable Properties"
        Protected Overridable ReadOnly Property StringFormatTabHorizontal() As StringFormat
            Get
                Return _stringFormatTabHorizontal
            End Get
        End Property
        Protected Overridable ReadOnly Property StringFormatTabVertical() As StringFormat
            Get
                Return _stringFormatTabVertical
            End Get
        End Property
        Protected Overridable ReadOnly Property ImageHeight() As Integer
            Get
                Return _ImageHeight
            End Get
        End Property
        Protected Overridable ReadOnly Property ImageWidth() As Integer
            Get
                Return _ImageWidth
            End Get
        End Property
        Protected Overridable ReadOnly Property ImageGapTop() As Integer
            Get
                Return _ImageGapTop
            End Get
        End Property
        Protected Overridable ReadOnly Property ImageGapLeft() As Integer
            Get
                Return _ImageGapLeft
            End Get
        End Property
        Protected Overridable ReadOnly Property ImageGapRight() As Integer
            Get
                Return _ImageGapRight
            End Get
        End Property
        Protected Overridable ReadOnly Property ImageGapBottom() As Integer
            Get
                Return _ImageGapBottom
            End Get
        End Property
        Protected Overridable ReadOnly Property TextGapLeft() As Integer
            Get
                Return _TextGapLeft
            End Get
        End Property
        Protected Overridable ReadOnly Property TextGapRight() As Integer
            Get
                Return _TextGapRight
            End Get
        End Property
        Protected Overridable ReadOnly Property TabGapTop() As Integer
            Get
                Return _TabGapTop
            End Get
        End Property
        Protected Overridable ReadOnly Property TabGapLeft() As Integer
            Get
                Return _TabGapLeft
            End Get
        End Property
        Protected Overridable ReadOnly Property TabGapBetween() As Integer
            Get
                Return _TabGapBetween
            End Get
        End Property
        Protected Overridable Sub BeginDrawTab()
        End Sub
        Protected Overridable Sub EndDrawTab()
        End Sub
        Protected Overridable ReadOnly Property BrushTabBackGround() As Brush
            Get
                Return SystemBrushes.Control
            End Get
        End Property
        Protected Overridable ReadOnly Property PenTabBorder() As Pen
            Get
                Return SystemPens.GrayText
            End Get
        End Property
        Protected Overridable ReadOnly Property BrushTabText() As Brush
            Get
                Return SystemBrushes.FromSystemColor(SystemColors.ControlDarkDark)
            End Get
        End Property
#End Region
        Private ReadOnly Property MatrixIdentity() As Matrix
            Get
                Return _matrixIdentity
            End Get
        End Property
        Private ReadOnly Property DockStates() As DockState()
            Get
                Return _dockStates
            End Get
        End Property
        Shared Sub New()
            _stringFormatTabHorizontal = New StringFormat()
            _stringFormatTabHorizontal.Alignment = StringAlignment.Near
            _stringFormatTabHorizontal.LineAlignment = StringAlignment.Center
            _stringFormatTabHorizontal.FormatFlags = StringFormatFlags.NoWrap
            _stringFormatTabVertical = New StringFormat()
            _stringFormatTabVertical.Alignment = StringAlignment.Near
            _stringFormatTabVertical.LineAlignment = StringAlignment.Center
            _stringFormatTabVertical.FormatFlags = StringFormatFlags.NoWrap Or StringFormatFlags.DirectionVertical
            _matrixIdentity = New Matrix()
            _dockStates = New DockState(4) {}
            _dockStates(0) = DockState.DockLeftAutoHide
            _dockStates(1) = DockState.DockRightAutoHide
            _dockStates(2) = DockState.DockTopAutoHide
            _dockStates(3) = DockState.DockBottomAutoHide
        End Sub
        Protected Friend Sub New(ByVal panel As DockPanel)
            MyBase.New(panel)
            SetStyle(ControlStyles.ResizeRedraw, True)
            SetStyle(ControlStyles.UserPaint, True)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            BackColor = Color.WhiteSmoke
        End Sub
        Protected Overloads Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            Dim g As Graphics = e.Graphics
            g.FillRectangle(BrushTabBackGround(), e.ClipRectangle)
            DrawTabStrip(g)
        End Sub
        Protected Overloads Overrides Sub OnLayout(ByVal levent As LayoutEventArgs)
            CalculateTabs()
            MyBase.OnLayout(levent)
        End Sub
        Private Sub DrawTabStrip(ByVal g As Graphics)
            DrawTabStrip(g, DockState.DockTopAutoHide)
            DrawTabStrip(g, DockState.DockBottomAutoHide)
            DrawTabStrip(g, DockState.DockLeftAutoHide)
            DrawTabStrip(g, DockState.DockRightAutoHide)
        End Sub
        Private Sub DrawTabStrip(ByVal g As Graphics, ByVal dockState As DockState)
            Dim rectTabStrip As Rectangle = GetLogicalTabStripRectangle(dockState)
            If rectTabStrip.IsEmpty Then
                Return
            End If
            Dim matrixIdentity As Matrix = g.Transform
            If dockState = dockState.DockLeftAutoHide OrElse dockState = dockState.DockRightAutoHide Then
                Dim matrixRotated As Matrix = New Matrix()
                matrixRotated.RotateAt(90, New PointF(CSng(rectTabStrip.X) + CSng(rectTabStrip.Height) / 2, CSng(rectTabStrip.Y) + CSng(rectTabStrip.Height) / 2))
                g.Transform = matrixRotated
            End If
            For Each pane As AutoHidePane In GetPanes(dockState)
                For Each tab As AutoHideTabFromBase In pane.Tabs
                    DrawTab(g, tab)
                Next
            Next
            g.Transform = matrixIdentity
        End Sub
        Private Sub CalculateTabs()
            CalculateTabs(DockState.DockTopAutoHide)
            CalculateTabs(DockState.DockBottomAutoHide)
            CalculateTabs(DockState.DockLeftAutoHide)
            CalculateTabs(DockState.DockRightAutoHide)
        End Sub
        Private Sub CalculateTabs(ByVal dockState As DockState)
            Dim rectTabStrip As Rectangle = GetLogicalTabStripRectangle(dockState)
            Dim imageHeight As Integer = rectTabStrip.Height - ImageGapTop - ImageGapBottom
            Dim imageWidth As Integer = imageWidth
            If imageHeight > Me.ImageHeight Then
                imageWidth = Me.ImageWidth * (imageHeight / Me.ImageHeight)
            End If
            Using g As Graphics = CreateGraphics()
                Dim x As Integer = TabGapLeft + rectTabStrip.X
                For Each pane As AutoHidePane In GetPanes(dockState)
                    Dim maxWidth As Integer = 0
                    For Each tab As AutoHideTabFromBase In pane.Tabs
                        Dim width As Integer = imageWidth + ImageGapLeft + ImageGapRight + CInt(g.MeasureString(tab.Content.DockHandler.TabText, Font).Width) + 1 + TextGapLeft + TextGapRight
                        If width > maxWidth Then
                            maxWidth = width
                        End If
                    Next
                    For Each tab As AutoHideTabFromBase In pane.Tabs
                        tab.TabX = x
                        If tab.Content Is pane.DockPane.ActiveContent Then
                            tab.TabWidth = maxWidth
                        Else
                            tab.TabWidth = imageWidth + ImageGapLeft + ImageGapRight
                        End If
                        x += tab.TabWidth
                    Next
                    x += TabGapBetween
                Next
            End Using
        End Sub
        Private Sub DrawTab(ByVal g As Graphics, ByVal tab As AutoHideTabFromBase)
            Dim rectTab As Rectangle = GetTabRectangle(tab)
            If rectTab.IsEmpty Then
                Return
            End If
            Dim dockState As DockState = tab.Content.DockHandler.DockState
            Dim content As IDockContent = tab.Content
            BeginDrawTab()
            Dim brushTabBackGround As Brush = Me.BrushTabBackGround
            Dim penTabBorder As Pen = Me.PenTabBorder
            Dim brushTabText As Brush = Me.BrushTabText

            g.SmoothingMode = SmoothingMode.AntiAlias
            If dockState = dockState.DockTopAutoHide OrElse dockState = dockState.DockRightAutoHide Then
                DrawHelper.DrawTab(g, rectTab, Corners.Bottom, GradientType.Flat, Color.FromArgb(244, 242, 232), Color.FromArgb(244, 242, 232), Color.FromArgb(172, 168, 153), True)
            Else
                DrawHelper.DrawTab(g, rectTab, Corners.Top, GradientType.Flat, Color.FromArgb(244, 242, 232), Color.FromArgb(244, 242, 232), Color.FromArgb(172, 168, 153), True)
            End If

            ' Set no rotate for drawing icon and text
            Dim matrixRotate As Matrix = g.Transform
            g.Transform = MatrixIdentity

            ' Draw the icon
            Dim rectImage As Rectangle = rectTab
            rectImage.X += ImageGapLeft
            rectImage.Y += ImageGapTop
            Dim imageHeight As Integer = rectTab.Height - ImageGapTop - ImageGapBottom
            Dim imageWidth As Integer = Me.ImageWidth
            If imageHeight > Me.ImageHeight Then
                imageWidth = Me.ImageWidth * (imageHeight / Me.ImageHeight)
            End If
            rectImage.Height = imageHeight
            rectImage.Width = imageWidth
            rectImage = GetTransformedRectangle(dockState, rectImage)
            g.DrawIcon(content.DockHandler.Icon, rectImage)

            ' Draw the text
            If content Is content.DockHandler.Pane.ActiveContent Then
                Dim rectText As Rectangle = rectTab
                rectText.X += ImageGapLeft + imageWidth + ImageGapRight + TextGapLeft
                rectText.Width -= ImageGapLeft + imageWidth + ImageGapRight + TextGapLeft
                rectText = GetTransformedRectangle(dockState, rectText)
                If dockState = dockState.DockLeftAutoHide OrElse dockState = dockState.DockRightAutoHide Then
                    g.DrawString(content.DockHandler.TabText, Font, brushTabText, rectText, StringFormatTabVertical)
                Else
                    g.DrawString(content.DockHandler.TabText, Font, brushTabText, rectText, StringFormatTabHorizontal)
                End If
            End If
            ' Set rotate back
            g.Transform = matrixRotate
            EndDrawTab()
        End Sub
        Private Function GetLogicalTabStripRectangle(ByVal dockState As DockState) As Rectangle
            Return GetLogicalTabStripRectangle(dockState, False)
        End Function
        Private Function GetLogicalTabStripRectangle(ByVal dockState As DockState, ByVal transformed As Boolean) As Rectangle
            If Not DockHelper.IsDockStateAutoHide(dockState) Then
                Return Rectangle.Empty
            End If
            Dim leftPanes As Integer = GetPanes(dockState.DockLeftAutoHide).Count
            Dim rightPanes As Integer = GetPanes(dockState.DockRightAutoHide).Count
            Dim topPanes As Integer = GetPanes(dockState.DockTopAutoHide).Count
            Dim bottomPanes As Integer = GetPanes(dockState.DockBottomAutoHide).Count
            Dim x As Integer, y As Integer, width As Integer, height As Integer
            height = MeasureHeight()
            If dockState = dockState.DockLeftAutoHide AndAlso leftPanes > 0 Then
                x = 0
                If topPanes = 0 Then
                    y = 0
                Else : y = height
                End If
                If topPanes = 0 Then
                    If bottomPanes = 0 Then
                        width = Me.Height
                    Else
                        width = Me.Height - height
                    End If
                Else
                    If bottomPanes = 0 Then
                        width = Me.Height - height
                    Else
                        width = Me.Height - height * 2
                    End If
                End If
            ElseIf dockState = dockState.DockRightAutoHide AndAlso rightPanes > 0 Then
                x = Me.Width - height
                If leftPanes <> 0 AndAlso x < height Then
                    x = height
                End If
                If topPanes = 0 Then
                    y = 0
                Else : y = height
                End If
                If topPanes = 0 Then
                    If bottomPanes = 0 Then
                        width = Me.Height
                    Else
                        width = Me.Height - height
                    End If
                Else
                    If bottomPanes = 0 Then
                        width = Me.Height - height
                    Else
                        width = Me.Height - height * 2
                    End If
                End If
            ElseIf dockState = dockState.DockTopAutoHide AndAlso topPanes > 0 Then
                If leftPanes = 0 Then
                    x = 0
                Else : x = height
                End If
                y = 0
                If leftPanes = 0 Then
                    If rightPanes = 0 Then
                        width = Me.Width
                    Else
                        width = Me.Width - height
                    End If
                Else
                    If rightPanes = 0 Then
                        width = Me.Width - height
                    Else
                        width = Me.Width - height * 2
                    End If
                End If
            ElseIf dockState = dockState.DockBottomAutoHide AndAlso bottomPanes > 0 Then
                If leftPanes = 0 Then
                    x = 0
                Else : x = height
                End If
                y = Me.Height - height
                If topPanes <> 0 AndAlso y < height Then
                    y = height
                End If
                If leftPanes = 0 Then
                    If rightPanes = 0 Then
                        width = Me.Width
                    Else
                        width = Me.Width - height
                    End If
                Else
                    If rightPanes = 0 Then
                        width = Me.Width - height
                    Else
                        width = Me.Width - height * 2
                    End If
                End If

            Else
                Return Rectangle.Empty
            End If
            If Not transformed Then
                Return New Rectangle(x, y, width, height)
            Else
                Return GetTransformedRectangle(dockState, New Rectangle(x, y, width, height))
            End If
        End Function
        Private Function GetTabRectangle(ByVal tab As AutoHideTabFromBase) As Rectangle
            Return GetTabRectangle(tab, False)
        End Function
        Private Function GetTabRectangle(ByVal tab As AutoHideTabFromBase, ByVal transformed As Boolean) As Rectangle
            Dim dockState As DockState = tab.Content.DockHandler.DockState
            Dim rectTabStrip As Rectangle = GetLogicalTabStripRectangle(dockState)
            If rectTabStrip.IsEmpty Then
                Return Rectangle.Empty
            End If
            Dim x As Integer = tab.TabX
            Dim y As Integer

            If (dockState = dockState.DockTopAutoHide OrElse dockState = dockState.DockRightAutoHide) Then
                y = rectTabStrip.Y
            Else
                y = rectTabStrip.Y + TabGapTop
            End If

            Dim width As Integer = (DirectCast(tab, AutoHideTabFromBase)).TabWidth
            Dim height As Integer = rectTabStrip.Height - TabGapTop
            If Not transformed Then
                Return New Rectangle(x, y, width, height)
            Else
                Return GetTransformedRectangle(dockState, New Rectangle(x, y, width, height))
            End If
        End Function
        Private Function GetTransformedRectangle(ByVal dockState As DockState, ByVal rect As Rectangle) As Rectangle
            If dockState <> dockState.DockLeftAutoHide AndAlso dockState <> dockState.DockRightAutoHide Then
                Return rect
            End If
            Dim pts As PointF() = New PointF(1) {}
            ' the center of the rectangle
            pts(0).X = CSng(rect.X) + CSng(rect.Width) / 2
            pts(0).Y = CSng(rect.Y) + CSng(rect.Height) / 2
            Dim rectTabStrip As Rectangle = GetLogicalTabStripRectangle(dockState)
            Dim matrix As Matrix = New Matrix()
            matrix.RotateAt(90, New PointF(CSng(rectTabStrip.X) + CSng(rectTabStrip.Height) / 2, CSng(rectTabStrip.Y) + CSng(rectTabStrip.Height) / 2))
            matrix.TransformPoints(pts)
            Return New Rectangle(CInt((pts(0).X - CSng(rect.Height) / 2 + 0.5F)), CInt((pts(0).Y - CSng(rect.Width) / 2 + 0.5F)), rect.Height, rect.Width)
        End Function
        Protected Overloads Overrides Function GetHitTest(ByVal ptMouse As Point) As IDockContent
            For Each state As DockState In DockStates
                Dim rectTabStrip As Rectangle = GetLogicalTabStripRectangle(state, True)
                If Not rectTabStrip.Contains(ptMouse) Then
                    Continue For
                End If
                For Each pane As AutoHidePane In GetPanes(state)
                    For Each tab As AutoHideTabFromBase In pane.Tabs
                        Dim rectTab As Rectangle = GetTabRectangle(tab, True)
                        rectTab.Intersect(rectTabStrip)
                        If rectTab.Contains(ptMouse) Then
                            Return tab.Content
                        End If
                    Next
                Next
            Next
            Return Nothing
        End Function
        Protected Overloads Overrides Function MeasureHeight() As Integer
            Return Math.Max(ImageGapBottom + ImageGapTop + ImageHeight, Font.Height) + TabGapTop
        End Function
        Protected Overloads Overrides Sub OnRefreshChanges()
            CalculateTabs()
            Invalidate()
        End Sub
    End Class
End Namespace
