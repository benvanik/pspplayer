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
Imports System.Runtime.InteropServices

Namespace VS2005Style.Win32
    Friend Class Gdi32
        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function CombineRgn(ByVal dest As IntPtr, ByVal src1 As IntPtr, ByVal src2 As IntPtr, ByVal flags As Integer) As Integer
        End Function
        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function CreateRectRgnIndirect(ByRef rect As Win32.RECT) As IntPtr
        End Function
        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function FillRgn(ByVal hDC As IntPtr, ByVal hrgn As IntPtr, ByVal hBrush As IntPtr) As Boolean
        End Function
        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetClipBox(ByVal hDC As IntPtr, ByRef rectBox As Win32.RECT) As Integer
        End Function
        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function SelectClipRgn(ByVal hDC As IntPtr, ByVal hRgn As IntPtr) As Integer
        End Function
        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function CreateBrushIndirect(ByRef brush As LOGBRUSH) As IntPtr
        End Function
        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function PatBlt(ByVal hDC As IntPtr, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal flags As UInt32) As Boolean
        End Function
        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function DeleteObject(ByVal hObject As IntPtr) As IntPtr
        End Function
        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function DeleteDC(ByVal hDC As IntPtr) As Boolean
        End Function
        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function SelectObject(ByVal hDC As IntPtr, ByVal hObject As IntPtr) As IntPtr
        End Function
        <DllImport("gdi32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function CreateCompatibleDC(ByVal hDC As IntPtr) As IntPtr
        End Function
    End Class
End Namespace
