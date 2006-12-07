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
Imports WeifenLuo.WinFormsUI
Namespace VS2005Style
    Friend Class DockHelper
        Public Shared Function IsDockStateAutoHide(ByVal dockState As DockState) As Boolean
            If dockState = dockState.DockLeftAutoHide OrElse dockState = dockState.DockRightAutoHide OrElse dockState = dockState.DockTopAutoHide OrElse dockState = dockState.DockBottomAutoHide Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function IsDockStateDocked(ByVal dockState As DockState) As Boolean
            Return (dockState = dockState.DockLeft OrElse dockState = dockState.DockRight OrElse dockState = dockState.DockTop OrElse dockState = dockState.DockBottom)
        End Function
        Public Shared Function IsDockBottom(ByVal dockState As DockState) As Boolean
            If (dockState = dockState.DockBottom OrElse dockState = dockState.DockBottomAutoHide) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function IsDockLeft(ByVal dockState As DockState) As Boolean
            If (dockState = dockState.DockLeft OrElse dockState = dockState.DockLeftAutoHide) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function IsDockRight(ByVal dockState As DockState) As Boolean
            If (dockState = dockState.DockRight OrElse dockState = dockState.DockRightAutoHide) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function IsDockTop(ByVal dockState As DockState) As Boolean
            If (dockState = dockState.DockTop OrElse dockState = dockState.DockTopAutoHide) Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function IsDockStateValid(ByVal dockState As DockState, ByVal dockableAreas As DockAreas) As Boolean
            If ((dockableAreas And DockAreas.Float) = 0) AndAlso (dockState = dockState.Float) Then
                Return False
            ElseIf ((dockableAreas And DockAreas.Document) = 0) AndAlso (dockState = dockState.Document) Then
                Return False
            ElseIf ((dockableAreas And DockAreas.DockLeft) = 0) AndAlso (dockState = dockState.DockLeft OrElse dockState = dockState.DockLeftAutoHide) Then
                Return False
            ElseIf ((dockableAreas And DockAreas.DockRight) = 0) AndAlso (dockState = dockState.DockRight OrElse dockState = dockState.DockRightAutoHide) Then
                Return False
            ElseIf ((dockableAreas And DockAreas.DockTop) = 0) AndAlso (dockState = dockState.DockTop OrElse dockState = dockState.DockTopAutoHide) Then
                Return False
            ElseIf ((dockableAreas And DockAreas.DockBottom) = 0) AndAlso (dockState = dockState.DockBottom OrElse dockState = dockState.DockBottomAutoHide) Then
                Return False
            Else
                Return True
            End If
        End Function
        Public Shared Function IsDockWindowState(ByVal state As DockState) As Boolean
            If state = DockState.DockTop OrElse state = DockState.DockBottom OrElse state = DockState.DockLeft OrElse state = DockState.DockRight OrElse state = DockState.Document Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function IsValidRestoreState(ByVal state As DockState) As Boolean
            If state = DockState.DockLeft OrElse state = DockState.DockRight OrElse state = DockState.DockTop OrElse state = DockState.DockBottom OrElse state = DockState.Document Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Shared Function ToggleAutoHideState(ByVal state As DockState) As DockState
            If state = DockState.DockLeft Then
                Return DockState.DockLeftAutoHide
            ElseIf state = DockState.DockRight Then
                Return DockState.DockRightAutoHide
            ElseIf state = DockState.DockTop Then
                Return DockState.DockTopAutoHide
            ElseIf state = DockState.DockBottom Then
                Return DockState.DockBottomAutoHide
            ElseIf state = DockState.DockLeftAutoHide Then
                Return DockState.DockLeft
            ElseIf state = DockState.DockRightAutoHide Then
                Return DockState.DockRight
            ElseIf state = DockState.DockTopAutoHide Then
                Return DockState.DockTop
            ElseIf state = DockState.DockBottomAutoHide Then
                Return DockState.DockBottom
            Else
                Return state
            End If
        End Function
    End Class
End Namespace
