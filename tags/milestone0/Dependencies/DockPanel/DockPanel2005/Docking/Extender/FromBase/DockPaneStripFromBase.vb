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
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.ComponentModel
Imports WeifenLuo.WinFormsUI

Namespace VS2005Style

    <ToolboxItem(False)> _
    Public Class DockPaneStripFromBase
        Inherits DockPaneStripBase

#Region "Private Consts"
        Private Const _ToolWindowStripGapLeft As Integer = 4
        Private Const _ToolWindowStripGapRight As Integer = 3
        Private Const _ToolWindowImageHeight As Integer = 16
        Private Const _ToolWindowImageWidth As Integer = 16
        Private Const _ToolWindowImageGapTop As Integer = 2
        Private Const _ToolWindowImageGapBottom As Integer = 1
        Private Const _ToolWindowImageGapLeft As Integer = 5
        Private Const _ToolWindowImageGapRight As Integer = 2
        Private Const _ToolWindowTextGapRight As Integer = 1
        Private Const _ToolWindowTabSeperatorGapTop As Integer = 3
        Private Const _ToolWindowTabSeperatorGapBottom As Integer = 3
        Private Const _DocumentToolWindowTabMinHeight As Integer = 24
        Private Const _DocumentTabMinHeight As Integer = 20
        Private Const _DocumentTabMaxWidth As Integer = 200
        Private Const _DocumentButtonGapTop As Integer = 3
        Private Const _DocumentButtonGapBottom As Integer = 4
        Private Const _DocumentButtonGapBetween As Integer = 5
        Private Const _DocumentButtonGapRight As Integer = 3
        Private Const _DocumentTabGapTop As Integer = 3
        Private Const _DocumentTabGapLeft As Integer = 3
        Private Const _DocumentTabGapRight As Integer = 1
        Private Const _DocumentTabOverlap As Integer = 14
        Private Const _DocumentTextExtraHeight As Integer = 3
        Private Const _DocumentTextExtraWidth As Integer = 24
        Private Const _DocumentIconGapLeft As Integer = 6
        Private Const _DocumentIconHeight As Integer = 14
        Private Const _DocumentIconWidth As Integer = 15
        Private Const _ResourceImageCloseEnabled As String = "DockPaneStrip.CloseEnabled.bmp"
        Private Const _ResourceImageCloseDisabled As String = "DockPaneStrip.CloseDisabled.bmp"
        Private Const _ResourceImageOptionsEnabled As String = "DockPaneStrip.OptionsEnabled.bmp"
        Private Const _ResourceImageOptionsDisabled As String = "DockPaneStrip.OptionsDisabled.bmp"
        Private Const _ResourceImageOverflowEnabled As String = "DockPaneStrip.OverflowEnabled.bmp"
        Private Const _ResourceImageOverflowDisabled As String = "DockPaneStrip.OverflowDisabled.bmp"
        Private Const _ResourceToolTipClose As String = "DockPaneStrip_ToolTipClose"
        Private Const _ResourceToolTipOptions As String = "DockPaneStrip_ToolTipOptions"

#End Region

#Region "Private Variables"
        Private m_offsetX As Integer = 0
        Private m_buttonClose As PopupButton
        Private m_buttonOptions As PopupButton
        Private m_components As IContainer
        Private m_toolTip As ToolTip
#End Region

#Region "Customizable Properties"
        Protected Overridable ReadOnly Property ToolWindowStripGapLeft() As Integer
            Get
                Return _ToolWindowStripGapLeft
            End Get
        End Property
        Protected Overridable ReadOnly Property ToolWindowStripGapRight() As Integer
            Get
                Return _ToolWindowStripGapRight
            End Get
        End Property
        Protected Overridable ReadOnly Property ToolWindowImageHeight() As Integer
            Get
                Return _ToolWindowImageHeight
            End Get
        End Property
        Protected Overridable ReadOnly Property ToolWindowImageWidth() As Integer
            Get
                Return _ToolWindowImageWidth
            End Get
        End Property
        Protected Overridable ReadOnly Property ToolWindowImageGapTop() As Integer
            Get
                Return _ToolWindowImageGapTop
            End Get
        End Property
        Protected Overridable ReadOnly Property ToolWindowImageGapBottom() As Integer
            Get
                Return _ToolWindowImageGapBottom
            End Get
        End Property
        Protected Overridable ReadOnly Property ToolWindowImageGapLeft() As Integer
            Get
                Return _ToolWindowImageGapLeft
            End Get
        End Property
        Protected Overridable ReadOnly Property ToolWindowImageGapRight() As Integer
            Get
                Return _ToolWindowImageGapRight
            End Get
        End Property
        Protected Overridable ReadOnly Property ToolWindowTextGapRight() As Integer
            Get
                Return _ToolWindowTextGapRight
            End Get
        End Property
        Protected Overridable ReadOnly Property ToolWindowTabSeperatorGapTop() As Integer
            Get
                Return _ToolWindowTabSeperatorGapTop
            End Get
        End Property
        Protected Overridable ReadOnly Property ToolWindowTabSeperatorGapBottom() As Integer
            Get
                Return _ToolWindowTabSeperatorGapBottom
            End Get
        End Property
        Private Shared _imageCloseEnabled As Image = Nothing
        Protected Overridable ReadOnly Property ImageCloseEnabled() As Image
            Get
                If _imageCloseEnabled Is Nothing Then
                    _imageCloseEnabled = My.Resources.DockPaneStrip_CloseEnabled
                End If
                Return _imageCloseEnabled
            End Get
        End Property
        Private Shared _imageCloseDisabled As Image = Nothing
        Protected Overridable ReadOnly Property ImageCloseDisabled() As Image
            Get
                If _imageCloseDisabled Is Nothing Then
                    _imageCloseDisabled = My.Resources.DockPaneStrip_CloseDisabled
                End If
                Return _imageCloseDisabled
            End Get
        End Property
        Private Shared _imageOptionsEnabled As Image = Nothing
        Protected Overridable ReadOnly Property ImageOptionsEnabled() As Image
            Get
                If _imageOptionsEnabled Is Nothing Then
                    _imageOptionsEnabled = My.Resources.DockPaneStrip_OptionsEnabled
                End If
                Return _imageOptionsEnabled
            End Get
        End Property
        Private Shared _imageOptionsDisabled As Image = Nothing
        Protected Overridable ReadOnly Property ImageOptionsDisabled() As Image
            Get
                If _imageOptionsDisabled Is Nothing Then
                    _imageOptionsDisabled = My.Resources.DockPaneStrip_OptionsDisabled
                End If
                Return _imageOptionsDisabled
            End Get
        End Property
        Private Shared _imageOverflowEnabled As Image = Nothing
        Protected Overridable ReadOnly Property ImageOverflowEnabled() As Image
            Get
                If _imageOverflowEnabled Is Nothing Then
                    _imageOverflowEnabled = My.Resources.DockPaneStrip_OverflowEnabled
                End If
                Return _imageOverflowEnabled
            End Get
        End Property
        Private Shared _imageOverflowDisabled As Image = Nothing
        Protected Overridable ReadOnly Property ImageOverflowDisabled() As Image
            Get
                If _imageOverflowDisabled Is Nothing Then
                    _imageOverflowDisabled = My.Resources.DockPaneStrip_OverflowDisabled
                End If
                Return _imageOverflowDisabled
            End Get
        End Property
        Private Shared _toolTipClose As String = Nothing
        Protected Overridable ReadOnly Property ToolTipClose() As String
            Get
                If _toolTipClose Is Nothing Then
                    _toolTipClose = My.Resources.DockPaneStrip_ToolTipClose
                End If
                Return _toolTipClose
            End Get
        End Property
        Private Shared _toolTipOptions As String = Nothing
        Protected Overridable ReadOnly Property ToolTipOptions() As String
            Get
                If _toolTipOptions Is Nothing Then
                    _toolTipOptions = My.Resources.DockPaneStrip_ToolTipOptions
                End If
                Return _toolTipOptions
            End Get
        End Property

        Private Shared _toolWindowTextStringFormat As StringFormat = Nothing
        Protected Overridable ReadOnly Property ToolWindowTextStringFormat() As StringFormat
            Get
                If _toolWindowTextStringFormat Is Nothing Then
                    _toolWindowTextStringFormat = New StringFormat(StringFormat.GenericTypographic)
                    _toolWindowTextStringFormat.Trimming = StringTrimming.EllipsisCharacter
                    _toolWindowTextStringFormat.LineAlignment = StringAlignment.Center
                    _toolWindowTextStringFormat.FormatFlags = StringFormatFlags.NoWrap
                End If
                Return _toolWindowTextStringFormat
            End Get
        End Property
        Private Shared _documentTextStringFormat As StringFormat = Nothing
        Public Shared ReadOnly Property DocumentTextStringFormat() As StringFormat
            Get
                If _documentTextStringFormat Is Nothing Then
                    _documentTextStringFormat = New StringFormat(StringFormat.GenericTypographic)
                    _documentTextStringFormat.Alignment = StringAlignment.Center
                    _documentTextStringFormat.Trimming = StringTrimming.EllipsisPath
                    _documentTextStringFormat.LineAlignment = StringAlignment.Center
                    _documentTextStringFormat.FormatFlags = StringFormatFlags.NoWrap
                End If
                Return _documentTextStringFormat
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentToolWindowTabMinHeight() As Integer
            Get
                Return _DocumentToolWindowTabMinHeight
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentTabMinHeight() As Integer
            Get
                Return _DocumentTabMinHeight
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentTabMaxWidth() As Integer
            Get
                Return _DocumentTabMaxWidth
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentButtonGapTop() As Integer
            Get
                Return _DocumentButtonGapTop
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentButtonGapBottom() As Integer
            Get
                Return _DocumentButtonGapBottom
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentButtonGapBetween() As Integer
            Get
                Return _DocumentButtonGapBetween
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentButtonGapRight() As Integer
            Get
                Return _DocumentButtonGapRight
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentTabGapTop() As Integer
            Get
                Return _DocumentTabGapTop
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentTabGapLeft() As Integer
            Get
                Return _DocumentTabGapLeft
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentTabGapRight() As Integer
            Get
                Return _DocumentTabGapRight
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentTextExtraHeight() As Integer
            Get
                Return _DocumentTextExtraHeight
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentTextExtraWidth() As Integer
            Get
                Return _DocumentTextExtraWidth
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentIconGapLeft() As Integer
            Get
                Return _DocumentIconGapLeft
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentIconWidth() As Integer
            Get
                Return _DocumentIconWidth
            End Get
        End Property
        Protected Overridable ReadOnly Property DocumentIconHeight() As Integer
            Get
                Return _DocumentIconHeight
            End Get
        End Property
        Protected Overridable Sub OnBeginDrawTabStrip(ByVal appearance As DockPane.AppearanceStyle)
        End Sub
        Protected Overridable Sub OnEndDrawTabStrip(ByVal appearance As DockPane.AppearanceStyle)
        End Sub
        Protected Overridable Sub OnBeginDrawTab(ByVal appearance As DockPane.AppearanceStyle)
        End Sub
        Protected Overridable Sub OnEndDrawTab(ByVal appearance As DockPane.AppearanceStyle)
        End Sub
        Protected Overridable ReadOnly Property OutlineInnerPen() As Pen
            Get
                Return SystemPens.ControlText
            End Get
        End Property
        Protected Overridable ReadOnly Property OutlineOuterPen() As Pen
            Get
                Return New Pen(Color.FromArgb(127, 157, 185))
            End Get
        End Property
        Protected Overridable ReadOnly Property ActiveBackBrush() As Brush
            Get
                Return SystemBrushes.Control
            End Get
        End Property
        Protected Overridable ReadOnly Property ActiveTextBrush() As Brush
            Get
                Return SystemBrushes.ControlText
            End Get
        End Property
        Protected Overridable ReadOnly Property TabSeperatorPen() As Pen
            Get
                Return SystemPens.GrayText
            End Get
        End Property
        Protected Overridable ReadOnly Property InactiveTextBrush() As Brush
            Get
                Return SystemBrushes.FromSystemColor(SystemColors.ControlDarkDark)
            End Get
        End Property
#End Region

#Region "New & Dispose Methods"
        Protected Friend Sub New(ByVal pane As DockPane)
            MyBase.New(pane)

            SetStyle(ControlStyles.ResizeRedraw, True)
            SetStyle(ControlStyles.UserPaint, True)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            SetStyle(ControlStyles.SupportsTransparentBackColor, True)

            SuspendLayout()
            Font = SystemInformation.MenuFont
            BackColor = Color.FromArgb(228, 226, 213)

            m_components = New Container()
            m_toolTip = New ToolTip(Components)

            m_buttonClose = New PopupButton(ImageCloseEnabled, ImageCloseDisabled)
            m_buttonClose.IsActivated = True
            m_buttonClose.ActiveBackColorGradientBegin = Color.FromArgb(228, 226, 213)
            m_buttonClose.ActiveBackColorGradientEnd = Color.FromArgb(228, 226, 213)
            m_buttonClose.ToolTipText = ToolTipClose
            m_buttonClose.Anchor = AnchorStyles.Top Or AnchorStyles.Right

            m_buttonOptions = New PopupButton(ImageOptionsEnabled, ImageOptionsDisabled)
            m_buttonOptions.IsActivated = True
            m_buttonOptions.ActiveBackColorGradientBegin = Color.FromArgb(228, 226, 213)
            m_buttonOptions.ActiveBackColorGradientEnd = Color.FromArgb(228, 226, 213)
            m_buttonOptions.ToolTipText = ToolTipOptions
            m_buttonOptions.Anchor = AnchorStyles.Top Or AnchorStyles.Right

            AddHandler m_buttonClose.Click, AddressOf Close_Click
            AddHandler m_buttonOptions.Click, AddressOf Options_Click
            Controls.AddRange(New Control() {m_buttonClose, m_buttonOptions})
            ResumeLayout()

        End Sub

        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                Components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region

#Region "(Measure Height) Private Methods"
        Protected Overloads Overrides Function MeasureHeight() As Integer
            If Appearance = WeifenLuo.WinFormsUI.DockPane.AppearanceStyle.ToolWindow Then
                Return MeasureHeight_ToolWindow()
            Else
                Return MeasureHeight_Document()
            End If
        End Function

        Private Function MeasureHeight_ToolWindow() As Integer
            If DockPane.IsAutoHide OrElse Tabs.Count <= 1 Then
                Return 0
            End If
            Dim height As Integer = Math.Max(Font.Height, ToolWindowImageHeight) + ToolWindowImageGapTop + ToolWindowImageGapBottom
            If height < DocumentToolWindowTabMinHeight Then
                height = DocumentToolWindowTabMinHeight
            End If
            Return height
        End Function

        Private Function MeasureHeight_Document() As Integer
            Dim height As Integer = Math.Max(Font.Height + DocumentTabGapTop + DocumentTextExtraHeight, ImageCloseEnabled.Height + DocumentButtonGapTop + DocumentButtonGapBottom)
            If height < DocumentTabMinHeight Then
                height = DocumentTabMinHeight
            End If
            Return height
        End Function

#End Region

#Region "(OnPaint & OnRefreshChanges) Protected Methods"
        Protected Overloads Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            MyBase.OnPaint(e)

            Dim rect As Rectangle = TabsRectangle
            If Appearance = WeifenLuo.WinFormsUI.DockPane.AppearanceStyle.Document Then
                rect.X -= DocumentTabGapLeft
                rect.Width += DocumentTabGapLeft
                Using brush As LinearGradientBrush = New LinearGradientBrush(rect, Color.FromArgb(228, 226, 213), Color.FromArgb(228, 226, 213), LinearGradientMode.Horizontal)
                    e.Graphics.FillRectangle(brush, rect)
                End Using
            Else
                Using brush As LinearGradientBrush = New LinearGradientBrush(rect, Color.FromArgb(231, 231, 218), Color.FromArgb(231, 231, 218), LinearGradientMode.Horizontal)
                    e.Graphics.FillRectangle(brush, rect)
                End Using
            End If
            DrawTabStrip(e.Graphics)
        End Sub

        Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
            MyBase.OnResize(e)

            Dim count As Integer = Tabs.Count
            Dim tabrect As Rectangle = TabsRectangle

            ' Resize to a bigger window
            If count > 1 Then
                If OffsetX < 0 AndAlso GetTabRectangle(count - 1).Right < tabrect.Right Then
                    OffsetX += (tabrect.Right - GetTabRectangle(count - 1).Right)
                    If DockPane.DockPanel.ShowDocumentIcon Then
                        OffsetX += DocumentIconWidth
                    End If
                    If OffsetX > 0 Then OffsetX = 0
                    OnRefreshChanges()
                End If
            End If

            'Resize to a smaller window
            Dim content As IDockContent
            For i As Integer = 0 To count - 1
                content = Tabs(i).Content
                If content Is DockPane.ActiveContent Then
                    If Not tabrect.Contains(GetTabRectangle(i)) Then
                        EnsureTabVisible(content)
                    End If
                End If
            Next

        End Sub

        Protected Overloads Overrides Sub OnRefreshChanges()
            CalculateTabs()
            SetInertButtons()
            Invalidate()
        End Sub
#End Region

#Region "(GetOutlinePath) Private Method"
        Protected Overloads Overrides Function GetOutlinePath(ByVal index As Integer) As GraphicsPath
            Dim pts As Point() = New Point(7) {}
            If Appearance = WeifenLuo.WinFormsUI.DockPane.AppearanceStyle.Document Then
                Dim rectTab As Rectangle = GetTabRectangle(index)
                rectTab.Intersect(TabsRectangle)
                Dim y As Integer = DockPane.PointToClient(PointToScreen(New Point(0, rectTab.Bottom))).Y
                Dim rectPaneClient As Rectangle = DockPane.ClientRectangle
                pts(0) = DockPane.PointToScreen(New Point(rectPaneClient.Left, y))
                pts(1) = PointToScreen(New Point(rectTab.Left, rectTab.Bottom))
                pts(2) = PointToScreen(New Point(rectTab.Left, rectTab.Top))
                pts(3) = PointToScreen(New Point(rectTab.Right + _DocumentTabOverlap, rectTab.Top))
                pts(4) = PointToScreen(New Point(rectTab.Right + _DocumentTabOverlap, rectTab.Bottom))
                pts(5) = DockPane.PointToScreen(New Point(rectPaneClient.Right, y))
                pts(6) = DockPane.PointToScreen(New Point(rectPaneClient.Right, rectPaneClient.Bottom))
                pts(7) = DockPane.PointToScreen(New Point(rectPaneClient.Left, rectPaneClient.Bottom))
            Else
                Dim rectTab As Rectangle = GetTabRectangle(index)
                rectTab.Intersect(TabsRectangle)
                Dim y As Integer = DockPane.PointToClient(PointToScreen(New Point(0, rectTab.Top))).Y + 1
                Dim rectPaneClient As Rectangle = DockPane.ClientRectangle
                pts(0) = DockPane.PointToScreen(New Point(rectPaneClient.Left, rectPaneClient.Top))
                pts(1) = DockPane.PointToScreen(New Point(rectPaneClient.Right, rectPaneClient.Top))
                pts(2) = DockPane.PointToScreen(New Point(rectPaneClient.Right, y))
                pts(3) = PointToScreen(New Point(rectTab.Right + 1, rectTab.Top))
                pts(4) = PointToScreen(New Point(rectTab.Right + 1, rectTab.Bottom))
                pts(5) = PointToScreen(New Point(rectTab.Left + 1, rectTab.Bottom))
                pts(6) = PointToScreen(New Point(rectTab.Left + 1, rectTab.Top))
                pts(7) = DockPane.PointToScreen(New Point(rectPaneClient.Left, y))
            End If
            Dim path As GraphicsPath = New GraphicsPath()
            path.AddLines(pts)
            Return path
        End Function
#End Region

#Region "(Calculate Tabs) Private Methods"
        Private Sub CalculateTabs()
            If Appearance = WeifenLuo.WinFormsUI.DockPane.AppearanceStyle.ToolWindow Then
                CalculateTabs_ToolWindow()
            Else
                CalculateTabs_Document()
            End If
        End Sub

        Private Sub CalculateTabs_ToolWindow()
            If Tabs.Count <= 1 OrElse DockPane.IsAutoHide Then
                Return
            End If
            Dim rectTabStrip As Rectangle = ClientRectangle
            ' Calculate tab widths
            Dim countTabs As Integer = Tabs.Count
            For Each tab As DockPaneTabFromBase In Tabs
                tab.MaxWidth = GetTabOriginalWidth(Tabs.IndexOf(tab))
                tab.Flag = False
            Next
            ' Set tab whose max width less than average width
            Dim anyWidthWithinAverage As Boolean = True
            Dim totalWidth As Integer = rectTabStrip.Width - ToolWindowStripGapLeft - ToolWindowStripGapRight
            Dim totalAllocatedWidth As Integer = 0
            Dim averageWidth As Integer = totalWidth / countTabs
            Dim remainedTabs As Integer = countTabs
            anyWidthWithinAverage = True
            While anyWidthWithinAverage AndAlso remainedTabs > 0
                anyWidthWithinAverage = False
                For Each tab As DockPaneTabFromBase In Tabs
                    If tab.Flag Then
                        Continue For
                    End If
                    If tab.MaxWidth <= averageWidth Then
                        tab.Flag = True
                        tab.TabWidth = tab.MaxWidth
                        totalAllocatedWidth += tab.TabWidth
                        anyWidthWithinAverage = True
                        remainedTabs -= 1
                    End If
                Next
                If remainedTabs <> 0 Then
                    averageWidth = (totalWidth - totalAllocatedWidth) / remainedTabs
                End If
            End While
            ' If any tab width not set yet, set it to the average width
            If remainedTabs > 0 Then
                Dim roundUpWidth As Integer = (totalWidth - totalAllocatedWidth) - (averageWidth * remainedTabs)
                For Each tab As DockPaneTabFromBase In Tabs
                    If tab.Flag Then
                        Continue For
                    End If
                    tab.Flag = True
                    If roundUpWidth > 0 Then
                        tab.TabWidth = averageWidth + 1
                        roundUpWidth -= 1
                    Else
                        tab.TabWidth = averageWidth
                    End If
                Next
            End If
            ' Set the X position of the tabs
            Dim x As Integer = rectTabStrip.X + ToolWindowStripGapLeft
            For Each tab As DockPaneTabFromBase In Tabs
                tab.TabX = x
                x += tab.TabWidth
            Next
        End Sub

        Private Sub CalculateTabs_Document()
            Dim countTabs As Integer = Tabs.Count
            If countTabs = 0 Then
                Return
            End If
            Dim rectTabStrip As Rectangle = ClientRectangle
            Dim x As Integer = rectTabStrip.X + DocumentTabGapLeft + OffsetX
            For Each tab As DockPaneTabFromBase In Tabs
                tab.TabX = x
                tab.TabWidth = Math.Min(GetTabOriginalWidth(Tabs.IndexOf(tab)), DocumentTabMaxWidth)
                x += tab.TabWidth
            Next
        End Sub
#End Region

#Region "(GetTabOriginalWidth) Private Methods"
        Protected Overridable Function GetTabOriginalWidth(ByVal index As Integer) As Integer
            If Appearance = WeifenLuo.WinFormsUI.DockPane.AppearanceStyle.ToolWindow Then
                Return GetTabOriginalWidth_ToolWindow(index)
            Else
                Return GetTabOriginalWidth_Document(index)
            End If
        End Function

        Private Function GetTabOriginalWidth_ToolWindow(ByVal index As Integer) As Integer
            Dim content As IDockContent = Tabs(index).Content
            Using g As Graphics = CreateGraphics()
                Dim sizeString As SizeF = g.MeasureString(content.DockHandler.TabText, Font)
                Return ToolWindowImageWidth + CInt(sizeString.Width) + 1 + ToolWindowImageGapLeft + ToolWindowImageGapRight + ToolWindowTextGapRight
            End Using
        End Function

        Private Function GetTabOriginalWidth_Document(ByVal index As Integer) As Integer
            Dim content As IDockContent = Tabs(index).Content
            Using g As Graphics = CreateGraphics()
                Dim sizeText As SizeF
                'If content Is DockPane.ActiveContent AndAlso DockPane.IsActiveDocumentPane Then
                Using boldFont As Font = New Font(Me.Font, FontStyle.Bold)
                    sizeText = g.MeasureString(content.DockHandler.TabText, boldFont, DocumentTabMaxWidth, DocumentTextStringFormat)
                End Using
                'Else
                '    sizeText = g.MeasureString(content.DockHandler.TabText, Font, DocumentTabMaxWidth, DocumentTextStringFormat)
                'End If
                If DockPane.DockPanel.ShowDocumentIcon Then
                    Return CInt(sizeText.Width) + 1 + DocumentTextExtraWidth + DocumentIconWidth + DocumentIconGapLeft
                Else
                    Return CInt(sizeText.Width) + 1 + DocumentTextExtraWidth
                End If

            End Using
        End Function

#End Region

#Region "(DrawTabStrip) Private Methods"
        Protected Overridable Sub DrawTabStrip(ByVal g As Graphics)
            OnBeginDrawTabStrip(Appearance)
            If Appearance = WeifenLuo.WinFormsUI.DockPane.AppearanceStyle.Document Then
                DrawTabStrip_Document(g)
            Else
                DrawTabStrip_ToolWindow(g)
            End If
            OnEndDrawTabStrip(Appearance)
        End Sub

        Private Sub DrawTabStrip_Document(ByVal g As Graphics)
            Dim count As Integer = Tabs.Count
            If count = 0 Then
                Return
            End If

            g.DrawLine(OutlineOuterPen, ClientRectangle.Left, ClientRectangle.Bottom - 1, ClientRectangle.Right, ClientRectangle.Bottom - 1)

            ' Draw the tabs
            Dim rectTabs As Rectangle = TabsRectangle
            Dim rectTab As Rectangle = Rectangle.Empty
            g.SetClip(rectTabs, CombineMode.Replace)

            Dim j As Integer = 0
            For i As Integer = 0 To count - 1
                rectTab = GetTabRectangle(i)
                If rectTab.IntersectsWith(rectTabs) Then
                    DrawTab(g, Tabs(i).Content, rectTab, j)
                    j = j + 1
                End If
            Next
        End Sub

        Private Sub DrawTabStrip_ToolWindow(ByVal g As Graphics)

            ' TODO: Clean up and add properties for colors
            g.SmoothingMode = SmoothingMode.AntiAlias
            For i As Integer = 0 To Tabs.Count - 1
                Dim tabrect As Rectangle = GetTabRectangle(i)
                Dim rectIcon As Rectangle = New Rectangle(tabrect.X + ToolWindowImageGapLeft, tabrect.Y + tabrect.Height - 1 - ToolWindowImageGapBottom - ToolWindowImageHeight, ToolWindowImageWidth, ToolWindowImageHeight)
                Dim rectText As Rectangle = rectIcon

                rectText.X += rectIcon.Width + ToolWindowImageGapRight
                rectText.Width = tabrect.Width - rectIcon.Width - ToolWindowImageGapLeft - ToolWindowImageGapRight - ToolWindowTextGapRight

                If DockPane.ActiveContent Is Tabs(i).Content Then

                    ' color area as the tab
                    g.FillRectangle(New SolidBrush(Color.FromArgb(252, 252, 254)), ClientRectangle.X, ClientRectangle.Y - 1, ClientRectangle.Width - 1, tabrect.Y + 2)

                    DrawHelper.DrawTab(g, tabrect, Corners.Bottom, GradientType.Flat, Color.FromArgb(252, 252, 254), Color.FromArgb(252, 252, 254), Color.FromArgb(172, 168, 153), False)

                    ' line to the left
                    g.DrawLine(TabSeperatorPen, tabrect.X, tabrect.Y + 1, ClientRectangle.X, tabrect.Y + 1)

                    ' line to the right
                    g.DrawLine(TabSeperatorPen, tabrect.X + tabrect.Width, tabrect.Y + 1, ClientRectangle.Width, tabrect.Y + 1)

                    ' text
                    g.DrawString(Tabs(i).Content.DockHandler.TabText, Font, New SolidBrush(Color.Black), rectText, ToolWindowTextStringFormat)

                Else
                    If Tabs.IndexOf(DockPane.ActiveContent) <> Tabs.IndexOf(Tabs(i).Content) + 1 AndAlso _
                       Tabs.IndexOf(Tabs(i).Content) <> Tabs.Count - 1 Then
                        g.DrawLine(TabSeperatorPen, tabrect.X + tabrect.Width - 1, tabrect.Y + ToolWindowTabSeperatorGapTop, tabrect.X + tabrect.Width - 1, tabrect.Y + tabrect.Height - 1 - ToolWindowTabSeperatorGapBottom)
                    End If
                    g.DrawString(Tabs(i).Content.DockHandler.TabText, Font, InactiveTextBrush, rectText, ToolWindowTextStringFormat)
                End If

                If tabrect.Contains(rectIcon) Then
                    g.DrawIcon(Tabs(i).Content.DockHandler.Icon, rectIcon)
                End If
            Next
        End Sub
#End Region

#Region "(GetTabRectangle) Private Methods"
        Protected Overridable Function GetTabRectangle(ByVal index As Integer) As Rectangle
            If Appearance = WeifenLuo.WinFormsUI.DockPane.AppearanceStyle.ToolWindow Then
                Return GetTabRectangle_ToolWindow(index)
            Else
                Return GetTabRectangle_Document(index)
            End If
        End Function

        Private Function GetTabRectangle_ToolWindow(ByVal index As Integer) As Rectangle
            Dim rectTabStrip As Rectangle = ClientRectangle
            Dim tab As DockPaneTabFromBase = DirectCast((Tabs(index)), DockPaneTabFromBase)
            Return New Rectangle(tab.TabX, rectTabStrip.Y + 2, tab.TabWidth, rectTabStrip.Height - 3)
        End Function

        Private Function GetTabRectangle_Document(ByVal index As Integer) As Rectangle
            Dim rectTabStrip As Rectangle = ClientRectangle
            Dim tab As DockPaneTabFromBase = DirectCast(Tabs(index), DockPaneTabFromBase)
            Return New Rectangle(tab.TabX, rectTabStrip.Y + DocumentTabGapTop, tab.TabWidth, rectTabStrip.Height - DocumentTabGapTop)
        End Function
#End Region

#Region "(DrawTab) Private Methods"
        Private Sub DrawTab(ByVal g As Graphics, ByVal content As IDockContent, ByVal rect As Rectangle, ByVal index As Integer)
            OnBeginDrawTab(Appearance)
            If Appearance = WeifenLuo.WinFormsUI.DockPane.AppearanceStyle.ToolWindow Then
                DrawTab_ToolWindow(g, content, rect, index)
            Else
                DrawTab_Document(g, content, rect, index)
            End If
            OnEndDrawTab(Appearance)
        End Sub

        Private Sub DrawTab_ToolWindow(ByVal g As Graphics, ByVal content As IDockContent, ByVal rect As Rectangle, ByVal index As Integer)
            Dim rectIcon As Rectangle = New Rectangle(rect.X + ToolWindowImageGapLeft, rect.Y + rect.Height - 1 - ToolWindowImageGapBottom - ToolWindowImageHeight, ToolWindowImageWidth, ToolWindowImageHeight)
            Dim rectText As Rectangle = rectIcon
            rectText.X += rectIcon.Width + ToolWindowImageGapRight
            rectText.Width = rect.Width - rectIcon.Width - ToolWindowImageGapLeft - ToolWindowImageGapRight - ToolWindowTextGapRight

            g.SmoothingMode = SmoothingMode.AntiAlias
            If DockPane.ActiveContent Is content Then
                DrawHelper.DrawTab(g, rect, Corners.Bottom, GradientType.Flat, Color.LightBlue, Color.WhiteSmoke, Color.Gray, False)
                g.DrawString(content.DockHandler.TabText, Font, ActiveTextBrush, rectText, ToolWindowTextStringFormat)
            Else
                If Tabs.IndexOf(DockPane.ActiveContent) <> Tabs.IndexOf(content) + 1 Then
                    g.DrawLine(TabSeperatorPen, rect.X + rect.Width - 1, rect.Y + ToolWindowTabSeperatorGapTop, rect.X + rect.Width - 1, rect.Y + rect.Height - 1 - ToolWindowTabSeperatorGapBottom)
                End If
                g.DrawString(content.DockHandler.TabText, Font, InactiveTextBrush, rectText, ToolWindowTextStringFormat)
            End If
            If rect.Contains(rectIcon) Then
                g.DrawIcon(content.DockHandler.Icon, rectIcon)
            End If
        End Sub

        Private Sub DrawTab_Document(ByVal g As Graphics, ByVal content As IDockContent, ByVal rect As Rectangle, ByVal index As Integer)
            Dim rectText As Rectangle = rect
            rectText.X += DocumentTextExtraWidth / 2
            rectText.Width -= DocumentTextExtraWidth
            rectText.X += _DocumentTabOverlap

            If index = 0 Then
                rect.Width += _DocumentTabOverlap
            Else
                rect.X += _DocumentTabOverlap
            End If

            g.SmoothingMode = SmoothingMode.AntiAlias
            If DockPane.ActiveContent Is content Then

                If index = 0 Then
                    If DockPane.DockPanel.ShowDocumentIcon Then
                        rectText.X += DocumentIconGapLeft
                        rectText.Width -= DocumentIconGapLeft
                    End If
                Else
                    rect.X -= _DocumentTabOverlap
                    rect.Width += _DocumentTabOverlap
                    If DockPane.DockPanel.ShowDocumentIcon Then
                        rectText.X += DocumentIconGapLeft
                        rectText.Width -= DocumentIconGapLeft
                    End If
                End If

                ' Draw Tab & Text
                DrawHelper.DrawDocumentTab(g, rect, Color.White, Color.White, Color.FromArgb(127, 157, 185), TabDrawType.Active, True)
                If DockPane.IsActiveDocumentPane Then
                    Using boldFont As Font = New Font(Me.Font, FontStyle.Bold)
                        g.DrawString(content.DockHandler.TabText, boldFont, ActiveTextBrush, rectText, DocumentTextStringFormat)
                    End Using
                Else
                    g.DrawString(content.DockHandler.TabText, Font, InactiveTextBrush, rectText, DocumentTextStringFormat)
                End If

                ' Draw Icon
                If DockPane.DockPanel.ShowDocumentIcon Then
                    Dim icon As Icon = content.DockHandler.Icon
                    Dim rectIcon As Rectangle
                    If index = 0 Then
                        rectIcon = New Rectangle(rect.X + DocumentIconGapLeft + _DocumentTabOverlap, rectText.Y + (rect.Height - DocumentIconHeight) / 2, DocumentIconWidth, DocumentIconHeight)
                    Else
                        rectIcon = New Rectangle(rect.X + DocumentIconGapLeft + _DocumentTabOverlap, rectText.Y + (rect.Height - DocumentIconHeight) / 2, DocumentIconWidth, DocumentIconHeight)
                    End If
                    g.DrawIcon(content.DockHandler.Icon, rectIcon)
                End If
            Else
                If index = 0 Then
                    DrawHelper.DrawDocumentTab(g, rect, Color.FromArgb(254, 253, 253), Color.FromArgb(241, 239, 226), Color.FromArgb(172, 168, 153), TabDrawType.First, True)
                Else
                    DrawHelper.DrawDocumentTab(g, rect, Color.FromArgb(254, 253, 253), Color.FromArgb(241, 239, 226), Color.FromArgb(172, 168, 153), TabDrawType.Inactive, True)
                End If
                g.DrawLine(OutlineOuterPen, rect.X, ClientRectangle.Bottom - 1, rect.X + rect.Width, ClientRectangle.Bottom - 1)
                g.DrawString(content.DockHandler.TabText, Font, InactiveTextBrush, rectText, DocumentTextStringFormat)
            End If

        End Sub
#End Region

#Region "(Buttons Related) Private Methods"
        Private Sub SetInertButtons()
            ' Set the visibility of the inert buttons
            If DockPane.DockState = DockState.Document Then
                m_buttonClose.Visible = True
                m_buttonOptions.Visible = True
            Else
                m_buttonClose.Visible = False
                m_buttonOptions.Visible = False
            End If

            ' Enable/disable overflow button
            Dim count As Integer = Tabs.Count
            If count <> 0 Then
                Dim rectTabs As Rectangle = TabsRectangle
                If GetTabRectangle(count - 1).Right > rectTabs.Right OrElse GetTabRectangle(0).Left < rectTabs.Left Then
                    m_buttonOptions.ImageEnabled = ImageOverflowEnabled
                    m_buttonOptions.ImageDisabled = ImageOverflowDisabled
                Else
                    m_buttonOptions.ImageEnabled = ImageOptionsEnabled
                    m_buttonOptions.ImageDisabled = ImageOptionsDisabled
                End If
            End If

            ' Enable/disable close button
            If DockPane.ActiveContent Is Nothing Then
                m_buttonClose.Enabled = False
            Else
                m_buttonClose.Enabled = DockPane.ActiveContent.DockHandler.CloseButton
            End If
        End Sub

        Protected Overloads Overrides Sub OnLayout(ByVal levent As LayoutEventArgs)
            Dim rectTabStrip As Rectangle = ClientRectangle
            ' Set position and size of the buttons
            Dim buttonWidth As Integer = ImageCloseEnabled.Width
            Dim buttonHeight As Integer = ImageCloseEnabled.Height
            Dim height As Integer = rectTabStrip.Height - DocumentButtonGapTop - DocumentButtonGapBottom
            If buttonHeight < height Then
                buttonWidth = buttonWidth * (height / buttonHeight)
                buttonHeight = height
            End If
            Dim buttonSize As Size = New Size(buttonWidth, buttonHeight)
            m_buttonClose.Size = buttonSize
            m_buttonOptions.Size = buttonSize
            Dim x As Integer = rectTabStrip.X + rectTabStrip.Width - DocumentTabGapLeft - DocumentButtonGapRight - buttonWidth
            Dim y As Integer = rectTabStrip.Y + DocumentButtonGapTop
            m_buttonClose.Location = New Point(x, y)
            Dim point As Point = m_buttonClose.Location
            point.Offset(-(DocumentButtonGapBetween + buttonWidth), 0)
            m_buttonOptions.Location = point
            OnRefreshChanges()
            MyBase.OnLayout(levent)
        End Sub

        Private Sub Close_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim i As Integer, width As Integer = -1
            For i = 0 To Tabs.Count - 1
                If Tabs(i).Content Is DockPane.ActiveContent Then
                    width = GetTabRectangle(i).Width
                    Exit For
                End If
            Next
            DockPane.CloseActiveContent()
            If width > 0 AndAlso Tabs.Count > 0 AndAlso GetTabRectangle(0).X < 0 Then
                OffsetX += Math.Min(width, Math.Abs(GetTabRectangle(0).X)) + 4
                OnRefreshChanges()
            End If
        End Sub

        Private m_contextmenu As New ContextMenuStrip
        Private Sub Options_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim x As Integer = 0
            Dim y As Integer = m_buttonOptions.Location.Y + m_buttonOptions.Height

            m_contextmenu.Items.Clear()
            For Each content As IDockContent In DockPane.Contents
                Dim item As ToolStripMenuItem = m_contextmenu.Items.Add(content.DockHandler.TabText, content.DockHandler.Icon.ToBitmap)
                item.Tag = content
                AddHandler item.Click, AddressOf MenuItem_Click
            Next
            m_contextmenu.Show(m_buttonOptions, x, y)
        End Sub

        Private Sub MenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim item As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
            If item IsNot Nothing Then
                Dim content As IDockContent = item.Tag
                If content IsNot Nothing Then
                    EnsureTabVisible(content)
                    content.DockHandler.Activate()
                End If
            End If
        End Sub

        Protected Overloads Overrides Function GetHitTest(ByVal ptMouse As Point) As Integer
            Dim rectTabStrip As Rectangle = TabsRectangle
            For i As Integer = 0 To Tabs.Count - 1
                Dim rectTab As Rectangle = GetTabRectangle(i)
                rectTab.Intersect(rectTabStrip)
                If rectTab.Contains(ptMouse) Then
                    Return i
                End If
            Next
            Return -1
        End Function

        Protected Overloads Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
            Dim index As Integer = GetHitTest(PointToClient(Control.MousePosition))
            Dim toolTip As String = String.Empty
            MyBase.OnMouseMove(e)
            If index <> -1 Then
                Dim rectTab As Rectangle = GetTabRectangle(index)
                If Tabs(index).Content.DockHandler.ToolTipText IsNot Nothing Then
                    toolTip = Tabs(index).Content.DockHandler.ToolTipText
                ElseIf rectTab.Width < GetTabOriginalWidth(index) Then
                    toolTip = Tabs(index).Content.DockHandler.TabText
                End If
            End If
            If m_toolTip.GetToolTip(Me) <> toolTip Then
                m_toolTip.Active = False
                m_toolTip.SetToolTip(Me, toolTip)
                m_toolTip.Active = True
            End If
        End Sub
#End Region

#Region "Protected & Private Properties"
        Protected ReadOnly Property Components() As IContainer
            Get
                Return m_components
            End Get
        End Property

        Private Property OffsetX() As Integer
            Get
                Return m_offsetX
            End Get
            Set(ByVal value As Integer)
                m_offsetX = value
            End Set
        End Property

        Private ReadOnly Property TabsRectangle() As Rectangle
            Get
                If Appearance = WeifenLuo.WinFormsUI.DockPane.AppearanceStyle.ToolWindow Then
                    Return ClientRectangle
                End If
                Dim rectWindow As Rectangle = ClientRectangle
                Dim x As Integer = rectWindow.X
                Dim y As Integer = rectWindow.Y
                Dim width As Integer = rectWindow.Width
                Dim height As Integer = rectWindow.Height
                x += DocumentTabGapLeft
                width -= DocumentTabGapLeft + DocumentTabGapRight + DocumentButtonGapRight + m_buttonClose.Width * 2 + DocumentButtonGapBetween * 3
                Return New Rectangle(x, y, width, height)
            End Get
        End Property
#End Region

        Protected Overloads Overrides Sub EnsureTabVisible(ByVal content As IDockContent)
            If Appearance <> WeifenLuo.WinFormsUI.DockPane.AppearanceStyle.Document Then
                Return
            End If
            Dim rectTabStrip As Rectangle = TabsRectangle
            Dim rectTab As Rectangle = GetTabRectangle(Tabs.IndexOf(content))
            If (rectTab.Right + _DocumentTabOverlap) > rectTabStrip.Right Then
                OffsetX -= rectTab.Right - rectTabStrip.Right + _DocumentTabOverlap
                rectTab.X -= rectTab.Right - rectTabStrip.Right + _DocumentTabOverlap
            End If
            If rectTab.Left < rectTabStrip.Left Then
                OffsetX += rectTabStrip.Left - rectTab.Left
            End If
            OnRefreshChanges()
        End Sub

    End Class
End Namespace
