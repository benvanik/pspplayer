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
    Friend Class User32
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function AnimateWindow(ByVal hWnd As IntPtr, ByVal dwTime As UInt32, ByVal dwFlags As FlagsAnimateWindow) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function DragDetect(ByVal hWnd As IntPtr, ByVal pt As VS2005Style.Win32.POINT) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetSysColorBrush(ByVal index As Integer) As IntPtr
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function InvalidateRect(ByVal hWnd As IntPtr, ByRef rect As RECT, ByVal [erase] As Boolean) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function LoadCursor(ByVal hInstance As IntPtr, ByVal cursor As UInt32) As IntPtr
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function SetCursor(ByVal hCursor As IntPtr) As IntPtr
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetFocus() As IntPtr
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function SetFocus(ByVal hWnd As IntPtr) As IntPtr
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function ReleaseCapture() As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function WaitMessage() As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function TranslateMessage(ByRef msg As MSG) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function DispatchMessage(ByRef msg As MSG) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function PostMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As UInt32, ByVal lParam As UInt32) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As UInt32, ByVal lParam As UInt32) As UInt32
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As UInt32
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetMessage(ByRef msg As MSG, ByVal hWnd As Integer, ByVal wFilterMin As UInt32, ByVal wFilterMax As UInt32) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function PeekMessage(ByRef msg As MSG, ByVal hWnd As Integer, ByVal wFilterMin As UInt32, ByVal wFilterMax As UInt32, ByVal wFlag As UInt32) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function BeginPaint(ByVal hWnd As IntPtr, ByRef ps As PAINTSTRUCT) As IntPtr
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function EndPaint(ByVal hWnd As IntPtr, ByRef ps As PAINTSTRUCT) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetDC(ByVal hWnd As IntPtr) As IntPtr
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetWindowDC(ByVal hWnd As IntPtr) As IntPtr
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function ReleaseDC(ByVal hWnd As IntPtr, ByVal hDC As IntPtr) As Integer
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal cmdShow As Short) As Integer
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function MoveWindow(ByVal hWnd As IntPtr, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal repaint As Boolean) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, _
         ByVal flags As FlagsSetWindowPos) As Integer
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function UpdateLayeredWindow(ByVal hwnd As IntPtr, ByVal hdcDst As IntPtr, ByRef pptDst As VS2005Style.Win32.POINT, ByRef psize As VS2005Style.Win32.SIZE, ByVal hdcSrc As IntPtr, ByRef pprSrc As VS2005Style.Win32.POINT, _
         ByVal crKey As Int32, ByRef pblend As BLENDFUNCTION, ByVal dwFlags As Int32) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetWindowRect(ByVal hWnd As IntPtr, ByRef rect As RECT) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function ClientToScreen(ByVal hWnd As IntPtr, ByRef pt As VS2005Style.Win32.POINT) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function ScreenToClient(ByVal hWnd As IntPtr, ByRef pt As VS2005Style.Win32.POINT) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function TrackMouseEvent(ByRef tme As TRACKMOUSEEVENTS) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function SetWindowRgn(ByVal hWnd As IntPtr, ByVal hRgn As IntPtr, ByVal redraw As Boolean) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetKeyState(ByVal virtKey As Integer) As UShort
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function GetParent(ByVal hWnd As IntPtr) As IntPtr
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function DrawFocusRect(ByVal hWnd As IntPtr, ByRef rect As RECT) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function HideCaret(ByVal hWnd As IntPtr) As Boolean
        End Function
        <DllImport("User32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function ShowCaret(ByVal hWnd As IntPtr) As Boolean
        End Function
        <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function SystemParametersInfo(ByVal uAction As SystemParametersInfoActions, ByVal uParam As UInt32, ByRef lpvParam As UInt32, ByVal fuWinIni As UInt32) As Boolean
        End Function
        <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function WindowFromPoint(ByVal point As VS2005Style.Win32.POINT) As IntPtr
        End Function
    End Class
End Namespace
