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
Imports WeifenLuo.WinFormsUI
Imports System.Drawing.Drawing2D
Namespace VS2005Style
    <ToolboxItem(False)> _
    Public Class DockPaneCaptionFromBase
        Inherits DockPaneCaptionBase
#Region "consts"
        Private Const _TextGapTop As Integer = 1
        Private Const _TextGapBottom As Integer = 2
        Private Const _TextGapLeft As Integer = 3
        Private Const _TextGapRight As Integer = 3
        Private Const _ButtonGapTop As Integer = 2
        Private Const _ButtonGapBottom As Integer = 2
        Private Const _ButtonGapBetween As Integer = 4
        Private Const _ButtonGapLeft As Integer = 1
        Private Const _ButtonGapRight As Integer = 2
        Private Const _ButtonMargin As Integer = 2
        Private Const _ResourceImageCloseEnabled As String = "DockPaneCaption.CloseEnabled.bmp"
        Private Const _ResourceImageCloseDisabled As String = "DockPaneCaption.CloseDisabled.bmp"
        Private Const _ResourceImageOptionsEnabled As String = "DockPaneCaption.OptionsEnabled.bmp"
        Private Const _ResourceImageOptionsDisabled As String = "DockPaneCaption.OptionsDisabled.bmp"
        Private Const _ResourceImageAutoHideYes As String = "DockPaneCaption.AutoHideYes.bmp"
        Private Const _ResourceImageAutoHideNo As String = "DockPaneCaption.AutoHideNo.bmp"
        Private Const _ResourceToolTipClose As String = "DockPaneCaption_ToolTipClose"
        Private Const _ResourceToolTipAutoHide As String = "DockPaneCaption_ToolTipAutoHide"
        Private Const _ResourceToolTipOptions As String = "DockPaneCaption_ToolTipOptions"
#End Region
        Private m_buttonClose As PopupButton
        Private m_buttonAutoHide As PopupButton
        Private m_buttonOptions As PopupButton

        Protected Friend Sub New(ByVal pane As DockPane)
            MyBase.New(pane)
            SuspendLayout()
            Font = SystemInformation.MenuFont

            m_buttonClose = New PopupButton(ImageCloseEnabled, ImageCloseDisabled)
            m_buttonClose.ActiveBackColorGradientBegin = Color.FromArgb(59, 128, 237)
            m_buttonClose.ActiveBackColorGradientEnd = Color.FromArgb(49, 106, 197)
            m_buttonClose.InactiveBackColorGradientBegin = Color.FromArgb(204, 199, 186)
            m_buttonClose.InactiveBackColorGradientEnd = Color.FromArgb(204, 199, 186)

            m_buttonAutoHide = New PopupButton
            m_buttonAutoHide.ActiveBackColorGradientBegin = Color.FromArgb(59, 128, 237)
            m_buttonAutoHide.ActiveBackColorGradientEnd = Color.FromArgb(49, 106, 197)
            m_buttonAutoHide.InactiveBackColorGradientBegin = Color.FromArgb(204, 199, 186)
            m_buttonAutoHide.InactiveBackColorGradientEnd = Color.FromArgb(204, 199, 186)

            m_buttonOptions = New PopupButton(ImageOptionsEnabled, ImageOptionsDisabled)
            m_buttonOptions.ActiveBackColorGradientBegin = Color.FromArgb(59, 128, 237)
            m_buttonOptions.ActiveBackColorGradientEnd = Color.FromArgb(49, 106, 197)
            m_buttonOptions.InactiveBackColorGradientBegin = Color.FromArgb(204, 199, 186)
            m_buttonOptions.InactiveBackColorGradientEnd = Color.FromArgb(204, 199, 186)

            m_buttonClose.ToolTipText = ToolTipClose
            m_buttonClose.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            AddHandler m_buttonClose.Click, AddressOf Close_Click

            m_buttonAutoHide.ToolTipText = ToolTipAutoHide
            m_buttonAutoHide.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            AddHandler m_buttonAutoHide.Click, AddressOf AutoHide_Click

            m_buttonOptions.ToolTipText = ToolTipOptions
            m_buttonOptions.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            AddHandler m_buttonOptions.Click, AddressOf Options_Click

            Controls.AddRange(New Control() {m_buttonClose, m_buttonAutoHide, m_buttonOptions})
            ResumeLayout()
        End Sub
#Region "Customizable Properties"
        Protected Overridable ReadOnly Property TextGapTop() As Integer
            Get
                Return _TextGapTop
            End Get
        End Property
        Protected Overridable ReadOnly Property TextGapBottom() As Integer
            Get
                Return _TextGapBottom
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
        Protected Overridable ReadOnly Property ButtonGapTop() As Integer
            Get
                Return _ButtonGapTop
            End Get
        End Property
        Protected Overridable ReadOnly Property ButtonGapBottom() As Integer
            Get
                Return _ButtonGapBottom
            End Get
        End Property
        Protected Overridable ReadOnly Property ButtonGapLeft() As Integer
            Get
                Return _ButtonGapLeft
            End Get
        End Property
        Protected Overridable ReadOnly Property ButtonGapRight() As Integer
            Get
                Return _ButtonGapRight
            End Get
        End Property
        Protected Overridable ReadOnly Property ButtonGapBetween() As Integer
            Get
                Return _ButtonGapBetween
            End Get
        End Property

        Private Shared _imageOptionsEnabled As Image = Nothing
        Protected Overridable ReadOnly Property ImageOptionsEnabled() As Image
            Get
                If _imageOptionsEnabled Is Nothing Then
                    _imageOptionsEnabled = My.Resources.DockPaneCaption_OptionsEnabled
                End If
                Return _imageOptionsEnabled
            End Get
        End Property
        Private Shared _imageOptionsDisabled As Image = Nothing
        Protected Overridable ReadOnly Property ImageOptionsDisabled() As Image
            Get
                If _imageOptionsDisabled Is Nothing Then
                    _imageOptionsDisabled = My.Resources.DockPaneCaption_OptionsDisabled
                End If
                Return _imageOptionsDisabled
            End Get
        End Property

        Private Shared _imageCloseEnabled As Image = Nothing
        Protected Overridable ReadOnly Property ImageCloseEnabled() As Image
            Get
                If _imageCloseEnabled Is Nothing Then
                    _imageCloseEnabled = My.Resources.DockPaneCaption_CloseEnabled
                End If
                Return _imageCloseEnabled
            End Get
        End Property
        Private Shared _imageCloseDisabled As Image = Nothing
        Protected Overridable ReadOnly Property ImageCloseDisabled() As Image
            Get
                If _imageCloseDisabled Is Nothing Then
                    _imageCloseDisabled = My.Resources.DockPaneCaption_CloseDisabled
                End If
                Return _imageCloseDisabled
            End Get
        End Property
        Private Shared _imageAutoHideYes As Image = Nothing
        Protected Overridable ReadOnly Property ImageAutoHideYes() As Image
            Get
                If _imageAutoHideYes Is Nothing Then
                    _imageAutoHideYes = My.Resources.DockPaneCaption_AutoHideYes
                End If
                Return _imageAutoHideYes
            End Get
        End Property
        Private Shared _imageAutoHideNo As Image = Nothing
        Protected Overridable ReadOnly Property ImageAutoHideNo() As Image
            Get
                If _imageAutoHideNo Is Nothing Then
                    _imageAutoHideNo = My.Resources.DockPaneCaption_AutoHideNo
                End If
                Return _imageAutoHideNo
            End Get
        End Property

        Private Shared _toolTipClose As String = Nothing
        Protected Overridable ReadOnly Property ToolTipClose() As String
            Get
                If _toolTipClose Is Nothing Then
                    _toolTipClose = My.Resources.DockPaneCaption_ToolTipClose
                End If
                Return _toolTipClose
            End Get
        End Property
        Private Shared _toolTipAutoHide As String = Nothing
        Protected Overridable ReadOnly Property ToolTipAutoHide() As String
            Get
                If _toolTipAutoHide Is Nothing Then
                    _toolTipAutoHide = My.Resources.DockPaneCaption_ToolTipAutoHide
                End If
                Return _toolTipAutoHide
            End Get
        End Property
        Private Shared _toolTipOptions As String = Nothing
        Protected Overridable ReadOnly Property ToolTipOptions() As String
            Get
                If _toolTipOptions Is Nothing Then
                    _toolTipOptions = My.Resources.DockPaneCaption_ToolTipOptions
                End If
                Return _toolTipOptions
            End Get
        End Property

        Protected Overridable ReadOnly Property ActiveBackColor() As Color
            Get
                Return Color.FromArgb(59, 128, 237)
            End Get
        End Property
        Protected Overridable ReadOnly Property InactiveBackColor() As Color
            Get
                Return Color.FromArgb(204, 199, 186)
            End Get
        End Property
        Protected Overridable ReadOnly Property ActiveTextColor() As Color
            Get
                Return SystemColors.ActiveCaptionText
            End Get
        End Property
        Protected Overridable ReadOnly Property InactiveTextColor() As Color
            Get
                Return SystemColors.ControlText
            End Get
        End Property
        Protected Overridable ReadOnly Property InactiveBorderColor() As Color
            Get
                Return SystemColors.GrayText
            End Get
        End Property
        Protected Overridable ReadOnly Property ActiveButtonBorderColor() As Color
            Get
                Return Color.FromArgb(60, 90, 170)
            End Get
        End Property
        Protected Overridable ReadOnly Property InactiveButtonBorderColor() As Color
            Get
                Return Color.FromArgb(140, 134, 123)
            End Get
        End Property
        Private Shared _textStringFormat As StringFormat = Nothing
        Protected Overridable ReadOnly Property TextStringFormat() As StringFormat
            Get
                If _textStringFormat Is Nothing Then
                    _textStringFormat = New StringFormat()
                    _textStringFormat.Trimming = StringTrimming.EllipsisCharacter
                    _textStringFormat.LineAlignment = StringAlignment.Center
                    _textStringFormat.FormatFlags = StringFormatFlags.NoWrap
                End If
                Return _textStringFormat
            End Get
        End Property
#End Region
        Protected Overloads Overrides Function MeasureHeight() As Integer
            Dim height As Integer = Font.Height + TextGapTop + TextGapBottom
            If height < (ImageCloseEnabled.Height + ButtonGapTop + ButtonGapBottom) Then
                height = ImageCloseEnabled.Height + ButtonGapTop + ButtonGapBottom
            End If
            Return height
        End Function
        Protected Overloads Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            MyBase.OnPaint(e)
            If DockPane.IsActivated Then
                Using brush As LinearGradientBrush = New LinearGradientBrush(ClientRectangle, Color.FromArgb(59, 128, 237), Color.FromArgb(49, 106, 197), LinearGradientMode.Vertical)
                    e.Graphics.FillRectangle(brush, ClientRectangle)
                End Using
            End If
            DrawCaption(e.Graphics)
        End Sub
        Private Sub DrawCaption(ByVal g As Graphics)
            If DockPane.IsActivated Then
                BackColor = ActiveBackColor
            Else
                BackColor = InactiveBackColor
            End If

            Dim rectCaption As Rectangle = ClientRectangle
            If Not DockPane.IsActivated Then
                Using pen As Pen = New Pen(InactiveBorderColor)
                    g.DrawLine(pen, rectCaption.X + 1, rectCaption.Y, rectCaption.X + rectCaption.Width - 2, rectCaption.Y)
                    g.DrawLine(pen, rectCaption.X + 1, rectCaption.Y + rectCaption.Height - 1, rectCaption.X + rectCaption.Width - 2, rectCaption.Y + rectCaption.Height - 1)
                    g.DrawLine(pen, rectCaption.X, rectCaption.Y + 1, rectCaption.X, rectCaption.Y + rectCaption.Height - 2)
                    g.DrawLine(pen, rectCaption.X + rectCaption.Width - 1, rectCaption.Y + 1, rectCaption.X + rectCaption.Width - 1, rectCaption.Y + rectCaption.Height - 2)
                End Using
            End If

            Dim rectCaptionText As Rectangle = rectCaption
            rectCaptionText.X += TextGapLeft
            rectCaptionText.Width = rectCaption.Width - ButtonGapRight - ButtonGapLeft - ButtonGapBetween - 3 * m_buttonClose.Width - TextGapLeft - TextGapRight
            rectCaptionText.Y += TextGapTop
            rectCaptionText.Height -= TextGapTop + TextGapBottom

            Dim brush As Brush
            If DockPane.IsActivated Then
                brush = New SolidBrush(ActiveTextColor)
            Else
                brush = New SolidBrush(InactiveTextColor)
            End If
            Using brush
                g.DrawString(DockPane.CaptionText, Font, brush, rectCaptionText, TextStringFormat)
            End Using

        End Sub
        Protected Overloads Overrides Sub OnLayout(ByVal levent As LayoutEventArgs)
            ' set the size and location for close and auto-hide buttons
            Dim rectCaption As Rectangle = ClientRectangle
            Dim buttonWidth As Integer = ImageCloseEnabled.Width
            Dim buttonHeight As Integer = ImageCloseEnabled.Height
            Dim height As Integer = rectCaption.Height - ButtonGapTop - ButtonGapBottom
            If buttonHeight < height Then
                buttonWidth = buttonWidth * (height / buttonHeight)
                buttonHeight = height
            End If

            m_buttonClose.SuspendLayout()
            m_buttonAutoHide.SuspendLayout()
            m_buttonOptions.SuspendLayout()

            Dim buttonSize As Size = New Size(buttonWidth, buttonHeight)
            m_buttonAutoHide.Size = buttonSize
            m_buttonClose.Size = buttonSize
            m_buttonOptions.Size = buttonSize

            Dim x As Integer = rectCaption.X + rectCaption.Width - 1 - ButtonGapRight - m_buttonClose.Width
            Dim y As Integer = rectCaption.Y + ButtonGapTop
            Dim point As Point = New Point(x, y)
            m_buttonClose.Location = point
            point.Offset(-(m_buttonAutoHide.Width + ButtonGapBetween), 0)
            m_buttonAutoHide.Location = point
            point.Offset(-(m_buttonOptions.Width + ButtonGapBetween), 0)
            m_buttonOptions.Location = point

            m_buttonClose.ResumeLayout()
            m_buttonAutoHide.ResumeLayout()
            m_buttonOptions.ResumeLayout()
            MyBase.OnLayout(levent)
        End Sub
        Protected Overloads Overrides Sub OnRefreshChanges()
            SetButtons()
            Invalidate()
        End Sub

        Private m_contextmenu As ContextMenuStrip
        Private Sub SetButtons()
            If DockPane.ActiveContent IsNot Nothing Then
                m_buttonClose.Enabled = DockPane.ActiveContent.DockHandler.CloseButton
                m_contextmenu = DockPane.ActiveContent.DockHandler.TabPageContextMenuStrip
                If m_contextmenu IsNot Nothing Then
                    m_buttonOptions.Visible = True
                    If m_buttonOptions.ToolTipText = "" Then m_buttonOptions.ToolTipText = m_contextmenu.Text
                Else
                    m_buttonOptions.Visible = False
                End If
            Else
                m_buttonClose.Enabled = False
            End If

            m_buttonAutoHide.Visible = Not DockPane.IsFloat
            If DockPane.IsAutoHide Then
                m_buttonAutoHide.ImageEnabled = ImageAutoHideYes
            Else
                m_buttonAutoHide.ImageEnabled = ImageAutoHideNo
            End If

            m_buttonAutoHide.IsActivated = DockPane.IsActivated
            m_buttonClose.IsActivated = DockPane.IsActivated
            m_buttonOptions.IsActivated = DockPane.IsActivated

            If DockPane.IsActivated Then
                m_buttonAutoHide.ForeColor = ActiveTextColor
                m_buttonAutoHide.BorderColor = ActiveButtonBorderColor
            Else
                m_buttonAutoHide.ForeColor = InactiveTextColor
                m_buttonAutoHide.BorderColor = InactiveButtonBorderColor
            End If

            m_buttonClose.ForeColor = m_buttonAutoHide.ForeColor
            m_buttonClose.BorderColor = m_buttonAutoHide.BorderColor
            m_buttonOptions.ForeColor = m_buttonAutoHide.ForeColor
            m_buttonOptions.BorderColor = m_buttonAutoHide.BorderColor

        End Sub
        Private Sub Close_Click(ByVal sender As Object, ByVal e As EventArgs)
            DockPane.CloseActiveContent()
        End Sub
        Private Sub AutoHide_Click(ByVal sender As Object, ByVal e As EventArgs)
            DockPane.DockState = DockHelper.ToggleAutoHideState(DockPane.DockState)
            If Not DockPane.IsAutoHide Then
                DockPane.Activate()
            End If
        End Sub
        Private Sub Options_Click(ByVal sender As Object, ByVal e As EventArgs)
            If m_contextmenu IsNot Nothing Then
                Dim x As Integer = 0
                Dim y As Integer = m_buttonOptions.Location.Y + m_buttonOptions.Height
                m_contextmenu.Show(m_buttonOptions, New Point(x, y))
            End If
        End Sub
    End Class
End Namespace
