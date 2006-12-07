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
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports System.ComponentModel

Namespace VS2005Style
    <ToolboxItem(False)> _
    Public Class PopupButton
        Inherits Button

        Private Enum RepeatClickStatus
            Disabled
            Started
            Repeating
            Stopped
        End Enum
        Private Class RepeatClickEventArgs
            Inherits EventArgs
            Private Shared _empty As RepeatClickEventArgs
            Shared Sub New()
                _empty = New RepeatClickEventArgs()
            End Sub
            Public Shared Shadows ReadOnly Property Empty() As RepeatClickEventArgs
                Get
                    Return _empty
                End Get
            End Property
        End Class

#Region "Private fields"
        Private components As IContainer = New Container()
        Private m_isActivated As Boolean = False
        Private m_borderWidth As Integer = 1
        Private m_mouseOver As Boolean = False
        Private m_mouseCapture As Boolean = False
        Private m_isPopup As Boolean = False
        Private m_imageEnabled As Image = Nothing
        Private m_imageDisabled As Image = Nothing
        Private m_imageIndexEnabled As Integer = -1
        Private m_imageIndexDisabled As Integer = -1
        Private m_monochrom As Boolean = True
        Private m_toolTip As ToolTip = Nothing
        Private m_toolTipText As String = ""
        Private m_borderColor As Color = Color.Empty
        Private m_activeGradientBegin As Color = Color.Empty
        Private m_activeGradientEnd As Color = Color.Empty
        Private m_inactiveGradientBegin As Color = Color.Empty
        Private m_inactiveGradientEnd As Color = Color.Empty
        Private m_clickStatus As RepeatClickStatus = RepeatClickStatus.Disabled
        Private m_repeatClickDelay As Integer = 500
        Private m_repeatClickInterval As Integer = 100
        Private m_timer As Timer
#End Region

#Region "Public Methods"
        Public Sub New()
            InternalConstruct(Nothing, Nothing)
        End Sub
        Public Sub New(ByVal c_imageEnabled As Image)
            InternalConstruct(c_imageEnabled, Nothing)
        End Sub
        Public Sub New(ByVal c_imageEnabled As Image, ByVal c_imageDisabled As Image)
            InternalConstruct(c_imageEnabled, c_imageDisabled)
        End Sub
#End Region

#Region "Private Methods"
        Private Sub InternalConstruct(ByVal c_imageEnabled As Image, ByVal c_imageDisabled As Image)
            ' Remember parameters
            Me.ImageEnabled = c_imageEnabled
            Me.ImageDisabled = c_imageDisabled
            ' Prevent drawing flicker by blitting from memory in WM_PAINT
            SetStyle(ControlStyles.ResizeRedraw, True)
            SetStyle(ControlStyles.UserPaint, True)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            ' Prevent base class from trying to generate double click events and
            ' so testing clicks against the double click time and rectangle. Getting
            ' rid of this allows the user to press then release button very quickly.
            'SetStyle(ControlStyles.StandardDoubleClick, false);
            ' Should not be allowed to select this control
            SetStyle(ControlStyles.Selectable, False)
            m_timer = New Timer()
            m_timer.Enabled = False
            AddHandler m_timer.Tick, AddressOf Timer_Tick
        End Sub
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub
        Private Function ShouldSerializeBorderColor() As Boolean
            Return (m_borderColor <> Color.Empty)
        End Function
        Private Function ShouldSerializeImageEnabled() As Boolean
            Return (m_imageEnabled IsNot Nothing)
        End Function
        Private Function ShouldSerializeImageDisabled() As Boolean
            Return (m_imageDisabled IsNot Nothing)
        End Function
        Private Sub DrawBackground(ByVal g As Graphics)
            If m_mouseOver Then
                If m_isActivated Then
                    Using brush As LinearGradientBrush = New LinearGradientBrush(ClientRectangle, Color.FromArgb(156, 182, 231), Color.FromArgb(156, 182, 231), LinearGradientMode.Vertical)
                        g.FillRectangle(brush, ClientRectangle)
                    End Using
                Else
                    Using brush As LinearGradientBrush = New LinearGradientBrush(ClientRectangle, Color.FromArgb(236, 233, 216), Color.FromArgb(236, 233, 216), LinearGradientMode.Vertical)
                        g.FillRectangle(brush, ClientRectangle)
                    End Using
                End If
            Else
                If m_isActivated Then
                    Using brush As LinearGradientBrush = New LinearGradientBrush(ClientRectangle, ActiveBackColorGradientBegin, ActiveBackColorGradientEnd, LinearGradientMode.Vertical)
                        g.FillRectangle(brush, ClientRectangle)
                    End Using
                Else
                    Using brush As LinearGradientBrush = New LinearGradientBrush(ClientRectangle, InactiveBackColorGradientBegin, InactiveBackColorGradientEnd, LinearGradientMode.Vertical)
                        g.FillRectangle(brush, ClientRectangle)
                    End Using
                End If
            End If
        End Sub
        Private Sub DrawImage(ByVal g As Graphics)
            Dim image As Image
            If Me.Enabled Then
                image = Me.ImageEnabled
            Else
                If ImageDisabled IsNot Nothing Then
                    image = Me.ImageDisabled
                Else
                    image = Me.ImageEnabled
                End If
            End If
            Dim imageAttr As ImageAttributes = Nothing
            If image Is Nothing Then
                Return
            End If
            If m_monochrom Then
                imageAttr = New ImageAttributes()
                ' transform the monochrom image
                ' white -> BackColor
                ' black -> ForeColor
                Dim myColorMap(1) As ColorMap
                myColorMap(0) = New ColorMap()
                myColorMap(0).OldColor = Color.White
                myColorMap(0).NewColor = Color.Transparent
                myColorMap(1) = New ColorMap()
                myColorMap(1).OldColor = Color.Black
                myColorMap(1).NewColor = Me.ForeColor
                imageAttr.SetRemapTable(myColorMap)
            End If
            Dim rect As Rectangle = New Rectangle(0, 0, image.Width, image.Height)
            If (Not Enabled) AndAlso (ImageDisabled Is Nothing) Then
                Using bitmapMono As Bitmap = New Bitmap(image, ClientRectangle.Size)
                    If imageAttr IsNot Nothing Then
                        Using gMono As Graphics = Graphics.FromImage(bitmapMono)
                            gMono.DrawImage(image, New Point(2) {New Point(0, 0), New Point(image.Width - 1, 0), New Point(0, image.Height - 1)}, rect, GraphicsUnit.Pixel, imageAttr)
                        End Using
                    End If
                    ControlPaint.DrawImageDisabled(g, bitmapMono, 0, 0, Me.BackColor)
                End Using
            Else
                ' Three points provided are upper-left, upper-right and 
                ' lower-left of the destination parallelogram. 
                Dim pts As Point() = New Point(2) {}
                If Enabled AndAlso m_mouseOver AndAlso m_mouseCapture Then
                    pts(0).X = 1
                    pts(0).Y = 1
                Else
                    pts(0).X = 0
                    pts(0).Y = 0
                End If
                pts(1).X = pts(0).X + ClientRectangle.Width
                pts(1).Y = pts(0).Y
                pts(2).X = pts(0).X
                pts(2).Y = pts(1).Y + ClientRectangle.Height
                If imageAttr Is Nothing Then
                    g.DrawImage(image, pts, rect, GraphicsUnit.Pixel)
                Else
                    g.DrawImage(image, pts, rect, GraphicsUnit.Pixel, imageAttr)
                End If
            End If
        End Sub
        Private Sub DrawText(ByVal g As Graphics)
            If Text = String.Empty Then
                Return
            End If
            Dim rect As Rectangle = ClientRectangle
            rect.X += BorderWidth
            rect.Y += BorderWidth
            rect.Width -= 2 * BorderWidth
            rect.Height -= 2 * BorderWidth
            Dim stringFormat As StringFormat = New StringFormat()
            If TextAlign = ContentAlignment.TopLeft Then
                stringFormat.Alignment = StringAlignment.Near
                stringFormat.LineAlignment = StringAlignment.Near
            ElseIf TextAlign = ContentAlignment.TopCenter Then
                stringFormat.Alignment = StringAlignment.Center
                stringFormat.LineAlignment = StringAlignment.Near
            ElseIf TextAlign = ContentAlignment.TopRight Then
                stringFormat.Alignment = StringAlignment.Far
                stringFormat.LineAlignment = StringAlignment.Near
            ElseIf TextAlign = ContentAlignment.MiddleLeft Then
                stringFormat.Alignment = StringAlignment.Near
                stringFormat.LineAlignment = StringAlignment.Center
            ElseIf TextAlign = ContentAlignment.MiddleCenter Then
                stringFormat.Alignment = StringAlignment.Center
                stringFormat.LineAlignment = StringAlignment.Center
            ElseIf TextAlign = ContentAlignment.MiddleRight Then
                stringFormat.Alignment = StringAlignment.Far
                stringFormat.LineAlignment = StringAlignment.Center
            ElseIf TextAlign = ContentAlignment.BottomLeft Then
                stringFormat.Alignment = StringAlignment.Near
                stringFormat.LineAlignment = StringAlignment.Far
            ElseIf TextAlign = ContentAlignment.BottomCenter Then
                stringFormat.Alignment = StringAlignment.Center
                stringFormat.LineAlignment = StringAlignment.Far
            ElseIf TextAlign = ContentAlignment.BottomRight Then
                stringFormat.Alignment = StringAlignment.Far
                stringFormat.LineAlignment = StringAlignment.Far
            End If
            Using brush As Brush = New SolidBrush(ForeColor)
                g.DrawString(Text, Font, brush, rect, stringFormat)
            End Using
        End Sub
        Private Sub DrawBorder(ByVal g As Graphics)
            Dim bs As ButtonBorderStyle
            ' Decide on the type of border to draw around image
            If Not Me.Enabled Then
                If IsPopup Then
                    bs = ButtonBorderStyle.Outset
                Else
                    bs = ButtonBorderStyle.Solid
                End If
            ElseIf m_mouseOver AndAlso m_mouseCapture Then
                bs = ButtonBorderStyle.Inset
            ElseIf IsPopup OrElse m_mouseOver Then
                If m_isActivated Then
                    BorderColor = Color.FromArgb(60, 90, 170)
                Else
                    BorderColor = Color.FromArgb(140, 134, 123)
                End If
                bs = ButtonBorderStyle.Solid
            Else
                bs = ButtonBorderStyle.Solid
            End If
            Dim colorLeftTop As Color
            Dim colorRightBottom As Color
            If bs = ButtonBorderStyle.Solid Then
                colorLeftTop = Me.BorderColor
                colorRightBottom = Me.BorderColor
            ElseIf bs = ButtonBorderStyle.Outset Then
                If m_borderColor.IsEmpty Then
                    colorLeftTop = Me.BackColor
                Else
                    colorLeftTop = m_borderColor
                End If
                colorRightBottom = Me.BackColor
            Else
                colorLeftTop = Me.BackColor
                If m_borderColor.IsEmpty Then
                    colorRightBottom = Me.BackColor
                Else
                    colorRightBottom = m_borderColor
                End If
            End If

            ControlPaint.DrawBorder(g, ClientRectangle, colorLeftTop, m_borderWidth, bs, colorLeftTop, _
             m_borderWidth, bs, colorRightBottom, m_borderWidth, bs, colorRightBottom, _
             m_borderWidth, bs)
        End Sub

#End Region

#Region "Properties"
        Public Property IsActivated() As Boolean
            Get
                Return m_isActivated
            End Get
            Set(ByVal value As Boolean)
                m_isActivated = value
            End Set
        End Property

        <Category("Appearance")> _
        Public Property ActiveBackColorGradientBegin() As System.Drawing.Color
            Get
                Return m_activeGradientBegin
            End Get
            Set(ByVal value As System.Drawing.Color)
                m_activeGradientBegin = value
            End Set
        End Property

        <Category("Appearance")> _
        Public Property ActiveBackColorGradientEnd() As System.Drawing.Color
            Get
                Return m_activeGradientEnd
            End Get
            Set(ByVal value As System.Drawing.Color)
                m_activeGradientEnd = value
            End Set
        End Property

        <Category("Appearance")> _
        Public Property InactiveBackColorGradientBegin() As System.Drawing.Color
            Get
                Return m_inactiveGradientBegin
            End Get
            Set(ByVal value As System.Drawing.Color)
                m_inactiveGradientBegin = value
            End Set
        End Property

        <Category("Appearance")> _
        Public Property InactiveBackColorGradientEnd() As System.Drawing.Color
            Get
                Return m_inactiveGradientEnd
            End Get
            Set(ByVal value As System.Drawing.Color)
                m_inactiveGradientEnd = value
            End Set
        End Property

        <Category("Appearance")> _
        Public Property BorderColor() As Color
            Get
                Return m_borderColor
            End Get
            Set(ByVal value As Color)
                If m_borderColor <> value Then
                    m_borderColor = value
                    Invalidate()
                End If
            End Set
        End Property
        <Category("Appearance")> _
        <DefaultValue(1)> _
        Public Property BorderWidth() As Integer
            Get
                Return m_borderWidth
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    value = 1
                End If
                If m_borderWidth <> value Then
                    m_borderWidth = value
                    Invalidate()
                End If
            End Set
        End Property
        <Category("Appearance")> _
        Public Property ImageEnabled() As Image
            Get
                If m_imageEnabled IsNot Nothing Then
                    Return m_imageEnabled
                End If
                Try
                    If ImageList Is Nothing OrElse ImageIndexEnabled = -1 Then
                        Return Nothing
                    Else
                        Return ImageList.Images(m_imageIndexEnabled)
                    End If
                Catch
                    Return Nothing
                End Try
            End Get
            Set(ByVal value As Image)
                If value IsNot m_imageEnabled Then
                    m_imageEnabled = value
                    Invalidate()
                End If
            End Set
        End Property
        <Category("Appearance")> _
        Public Property ImageDisabled() As Image
            Get
                If m_imageDisabled IsNot Nothing Then
                    Return m_imageDisabled
                End If
                Try
                    If ImageList Is Nothing OrElse ImageIndexDisabled = -1 Then
                        Return Nothing
                    Else
                        Return ImageList.Images(m_imageIndexDisabled)
                    End If
                Catch
                    Return Nothing
                End Try
            End Get
            Set(ByVal value As Image)
                If m_imageDisabled Is value Then
                    m_imageDisabled = value
                    Invalidate()
                End If
            End Set
        End Property
        <Category("Appearance")> _
        <DefaultValue(-1)> _
        <Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design", "System.Drawing.Design.UITypeEditor,System.Drawing")> _
        <TypeConverter(GetType(System.Windows.Forms.ImageIndexConverter))> _
        <RefreshProperties(RefreshProperties.Repaint)> _
        Public Property ImageIndexEnabled() As Integer
            Get
                Return m_imageIndexEnabled
            End Get
            Set(ByVal value As Integer)
                If m_imageIndexEnabled <> value Then
                    m_imageIndexEnabled = value
                    Invalidate()
                End If
            End Set
        End Property
        <Category("Appearance")> _
        <DefaultValue(-1)> _
        <Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design", "System.Drawing.Design.UITypeEditor,System.Drawing")> _
        <TypeConverter(GetType(System.Windows.Forms.ImageIndexConverter))> _
        <RefreshProperties(RefreshProperties.Repaint)> _
        Public Property ImageIndexDisabled() As Integer
            Get
                Return m_imageIndexDisabled
            End Get
            Set(ByVal value As Integer)
                If m_imageIndexDisabled <> value Then
                    m_imageIndexDisabled = value
                    Invalidate()
                End If
            End Set
        End Property
        <Category("Appearance")> _
        <DefaultValue(False)> _
        Public Property IsPopup() As Boolean
            Get
                Return m_isPopup
            End Get
            Set(ByVal value As Boolean)
                If m_isPopup <> value Then
                    m_isPopup = value
                    Invalidate()
                End If
            End Set
        End Property
        <Category("Appearance")> _
        <DefaultValue(True)> _
        Public Property Monochrome() As Boolean
            Get
                Return m_monochrom
            End Get
            Set(ByVal value As Boolean)
                If value <> m_monochrom Then
                    m_monochrom = value
                    Invalidate()
                End If
            End Set
        End Property
        <Category("Behavior")> _
        <DefaultValue(False)> _
        Public Property RepeatClick() As Boolean
            Get
                Return (ClickStatus <> RepeatClickStatus.Disabled)
            End Get
            Set(ByVal value As Boolean)
                ClickStatus = RepeatClickStatus.Stopped
            End Set
        End Property
        Private Property ClickStatus() As RepeatClickStatus
            Get
                Return m_clickStatus
            End Get
            Set(ByVal value As RepeatClickStatus)
                If m_clickStatus = value Then
                    Return
                End If
                m_clickStatus = value
                If ClickStatus = RepeatClickStatus.Started Then
                    Timer.Interval = RepeatClickDelay
                    Timer.Enabled = True
                ElseIf ClickStatus = RepeatClickStatus.Repeating Then
                    Timer.Interval = RepeatClickInterval
                Else
                    Timer.Enabled = False
                End If
            End Set
        End Property
        <Category("Behavior")> _
        <DefaultValue(500)> _
        Public Property RepeatClickDelay() As Integer
            Get
                Return m_repeatClickDelay
            End Get
            Set(ByVal value As Integer)
                m_repeatClickDelay = value
            End Set
        End Property
        <Category("Behavior")> _
        <DefaultValue(100)> _
        Public Property RepeatClickInterval() As Integer
            Get
                Return m_repeatClickInterval
            End Get
            Set(ByVal value As Integer)
                m_repeatClickInterval = value
            End Set
        End Property
        Private ReadOnly Property Timer() As Timer
            Get
                Return m_timer
            End Get
        End Property
        <Category("Appearance")> _
        <DefaultValue("")> _
        Public Property ToolTipText() As String
            Get
                Return m_toolTipText
            End Get
            Set(ByVal value As String)
                If m_toolTipText <> value Then
                    If m_toolTip Is Nothing Then
                        m_toolTip = New ToolTip(Me.components)
                    End If
                    m_toolTipText = value
                    m_toolTip.SetToolTip(Me, value)
                End If
            End Set
        End Property

#End Region

#Region "Events"
        Private Sub Timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
            If m_mouseCapture AndAlso m_mouseOver Then
                OnClick(RepeatClickEventArgs.Empty)
            End If
            If ClickStatus = RepeatClickStatus.Started Then
                ClickStatus = RepeatClickStatus.Repeating
            End If
        End Sub
        Protected Overloads Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            If e.Button <> Windows.Forms.MouseButtons.Left Then
                Return
            End If
            If m_mouseCapture = False OrElse m_mouseOver = False Then
                m_mouseCapture = True
                m_mouseOver = True
                'Redraw to show button state
                Invalidate()
            End If
            If RepeatClick Then
                OnClick(RepeatClickEventArgs.Empty)
                ClickStatus = RepeatClickStatus.Started
            End If
        End Sub
        Protected Overloads Overrides Sub OnClick(ByVal e As EventArgs)
            If RepeatClick AndAlso Not (TypeOf e Is RepeatClickEventArgs) Then
                Return
            End If
            MyBase.OnClick(e)
        End Sub
        Protected Overloads Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
            MyBase.OnMouseUp(e)
            If e.Button <> Windows.Forms.MouseButtons.Left Then
                Return
            End If
            If m_mouseOver = True OrElse m_mouseCapture = True Then
                m_mouseOver = False
                m_mouseCapture = False
                ' Redraw to show button state
                Invalidate()
            End If
            If RepeatClick Then
                ClickStatus = RepeatClickStatus.Stopped
            End If
        End Sub
        Protected Overloads Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
            MyBase.OnMouseMove(e)
            ' Is mouse point inside our client rectangle
            Dim over As Boolean = Me.ClientRectangle.Contains(New Point(e.X, e.Y))
            ' If entering the button area or leaving the button area...
            If over <> m_mouseOver Then
                ' Update state
                m_mouseOver = over
                ' Redraw to show button state
                Invalidate()
            End If
        End Sub
        Protected Overloads Overrides Sub OnMouseEnter(ByVal e As EventArgs)
            ' Update state to reflect mouse over the button area
            If Not m_mouseOver Then
                m_mouseOver = True
                ' Redraw to show button state
                Invalidate()
            End If
            MyBase.OnMouseEnter(e)
        End Sub
        Protected Overloads Overrides Sub OnMouseLeave(ByVal e As EventArgs)
            ' Update state to reflect mouse not over the button area
            If m_mouseOver Then
                m_mouseOver = False
                ' Redraw to show button state
                Invalidate()
            End If
            MyBase.OnMouseLeave(e)
        End Sub
        Protected Overloads Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            MyBase.OnPaint(e)
            DrawBackground(e.Graphics)
            DrawImage(e.Graphics)
            DrawText(e.Graphics)
            If m_mouseOver Or m_mouseCapture Then DrawBorder(e.Graphics)
        End Sub
        Protected Overloads Overrides Sub OnEnabledChanged(ByVal e As EventArgs)
            MyBase.OnEnabledChanged(e)
            If Enabled = False Then
                m_mouseOver = False
                m_mouseCapture = False
                If RepeatClick AndAlso ClickStatus <> RepeatClickStatus.Stopped Then
                    ClickStatus = RepeatClickStatus.Stopped
                End If
            End If
            Invalidate()
        End Sub
#End Region

        Private Sub InitializeComponent()
            Me.SuspendLayout()
            '
            'PopupButton
            '
            Me.AutoSize = True
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ResumeLayout(False)

        End Sub
    End Class
End Namespace
