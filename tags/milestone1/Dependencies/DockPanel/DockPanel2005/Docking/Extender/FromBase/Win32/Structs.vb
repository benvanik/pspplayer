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
Imports System.Runtime.InteropServices
Namespace VS2005Style.Win32
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure MSG
        Public hwnd As IntPtr
        Public message As Integer
        Public wParam As IntPtr
        Public lParam As IntPtr
        Public time As Integer
        Public pt_x As Integer
        Public pt_y As Integer
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure PAINTSTRUCT
        Public hdc As IntPtr
        Public fErase As Integer
        Public rcPaint As Rectangle
        Public fRestore As Integer
        Public fIncUpdate As Integer
        Public Reserved1 As Integer
        Public Reserved2 As Integer
        Public Reserved3 As Integer
        Public Reserved4 As Integer
        Public Reserved5 As Integer
        Public Reserved6 As Integer
        Public Reserved7 As Integer
        Public Reserved8 As Integer
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
        Public Overloads Overrides Function ToString() As String
            Return "{left=" + left.ToString() + ", " + "top=" + top.ToString() + ", " + "right=" + right.ToString() + ", " + "bottom=" + bottom.ToString() + "}"
        End Function
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure POINT
        Public x As Integer
        Public y As Integer
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure SIZE
        Public cx As Integer
        Public cy As Integer
    End Structure
    <StructLayout(LayoutKind.Sequential, Pack:=1)> _
    Friend Structure BLENDFUNCTION
        Public BlendOp As Byte
        Public BlendFlags As Byte
        Public SourceConstantAlpha As Byte
        Public AlphaFormat As Byte
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure TRACKMOUSEEVENTS
        Public Const TME_HOVER As UInt32 = 1
        Public Const TME_LEAVE As UInt32 = 2
        Public Const TME_NONCLIENT As UInt32 = 16
        Public Const TME_QUERY As UInt32 = 1073741824
        Public Const TME_CANCEL As UInt32 = 2147483648
        Public Const HOVER_DEFAULT As UInt32 = 4294967295
        Private cbSize As UInt32
        Private dwFlags As UInt32
        Private hWnd As IntPtr
        Private dwHoverTime As UInt32
        Public Sub New(ByVal dwFlags As UInt32, ByVal hWnd As IntPtr, ByVal dwHoverTime As UInt32)
            cbSize = 16
            Me.dwFlags = dwFlags
            Me.hWnd = hWnd
            Me.dwHoverTime = dwHoverTime
        End Sub
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure LOGBRUSH
        Public lbStyle As UInt32
        Public lbColor As UInt32
        Public lbHatch As UInt32
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure NCCALCSIZE_PARAMS
        Public rgrc1 As RECT
        Public rgrc2 As RECT
        Public rgrc3 As RECT
        Private lppos As IntPtr
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure CWPRETSTRUCT
        Public lResult As Integer
        Public lParam As Integer
        Public wParam As Integer
        Public message As Integer
        Public hwnd As IntPtr
    End Structure
End Namespace
