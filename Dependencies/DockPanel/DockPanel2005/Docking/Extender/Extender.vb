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
Imports WeifenLuo.WinFormsUI
Namespace VS2005Style
    Public Class Extender

        Public Enum Schema
            [Default]
            Override
            FromBase
        End Enum

        Private Class DockPaneStripOverrideFactory
            Implements DockPanelExtender.IDockPaneStripFactory

            Public Function CreateDockPaneStrip(ByVal pane As WeifenLuo.WinFormsUI.DockPane) As WeifenLuo.WinFormsUI.DockPaneStripBase Implements WeifenLuo.WinFormsUI.DockPanelExtender.IDockPaneStripFactory.CreateDockPaneStrip
                Return New DockPaneStripOverride(pane)
            End Function
        End Class

        Private Class AutoHideStripOverrideFactory
            Implements DockPanelExtender.IAutoHideStripFactory

            Public Function CreateAutoHideStrip(ByVal panel As WeifenLuo.WinFormsUI.DockPanel) As WeifenLuo.WinFormsUI.AutoHideStripBase Implements WeifenLuo.WinFormsUI.DockPanelExtender.IAutoHideStripFactory.CreateAutoHideStrip
                Return New AutoHideStripOverride(panel)
            End Function
        End Class

        Private Class DockPaneCaptionFromBaseFactory
            Implements DockPanelExtender.IDockPaneCaptionFactory

            Public Function CreateDockPaneCaption(ByVal pane As WeifenLuo.WinFormsUI.DockPane) As WeifenLuo.WinFormsUI.DockPaneCaptionBase Implements WeifenLuo.WinFormsUI.DockPanelExtender.IDockPaneCaptionFactory.CreateDockPaneCaption
                Return New DockPaneCaptionFromBase(pane)
            End Function
        End Class

        Private Class DockPaneTabFromBaseFactory
            Implements DockPanelExtender.IDockPaneTabFactory

            Public Function CreateDockPaneTab(ByVal content As WeifenLuo.WinFormsUI.IDockContent) As WeifenLuo.WinFormsUI.DockPaneTab Implements WeifenLuo.WinFormsUI.DockPanelExtender.IDockPaneTabFactory.CreateDockPaneTab
                Return New DockPaneTabFromBase(content)
            End Function
        End Class

        Private Class DockPaneStripFromBaseFactory
            Implements DockPanelExtender.IDockPaneStripFactory

            Public Function CreateDockPaneStrip(ByVal pane As WeifenLuo.WinFormsUI.DockPane) As WeifenLuo.WinFormsUI.DockPaneStripBase Implements WeifenLuo.WinFormsUI.DockPanelExtender.IDockPaneStripFactory.CreateDockPaneStrip
                Return New DockPaneStripFromBase(pane)
            End Function
        End Class

        Private Class AutoHideTabFromBaseFactory
            Implements DockPanelExtender.IAutoHideTabFactory

            Public Function CreateAutoHideTab(ByVal content As WeifenLuo.WinFormsUI.IDockContent) As WeifenLuo.WinFormsUI.AutoHideTab Implements WeifenLuo.WinFormsUI.DockPanelExtender.IAutoHideTabFactory.CreateAutoHideTab
                Return New AutoHideTabFromBase(content)
            End Function
        End Class

        Private Class AutoHideStripFromBaseFactory
            Implements DockPanelExtender.IAutoHideStripFactory

            Public Function CreateAutoHideStrip(ByVal panel As WeifenLuo.WinFormsUI.DockPanel) As WeifenLuo.WinFormsUI.AutoHideStripBase Implements WeifenLuo.WinFormsUI.DockPanelExtender.IAutoHideStripFactory.CreateAutoHideStrip
                Return New AutoHideStripFromBase(panel)
            End Function
        End Class

        Public Shared Sub SetSchema(ByVal dockPanel As DockPanel, ByVal schema As Extender.Schema)
            If schema = Extender.Schema.[Default] Then
                dockPanel.Extender.AutoHideTabFactory = Nothing
                dockPanel.Extender.DockPaneTabFactory = Nothing
                dockPanel.Extender.AutoHideStripFactory = Nothing
                dockPanel.Extender.DockPaneCaptionFactory = Nothing
                dockPanel.Extender.DockPaneStripFactory = Nothing
            ElseIf schema = Extender.Schema.Override Then
                dockPanel.Extender.AutoHideTabFactory = Nothing
                dockPanel.Extender.DockPaneTabFactory = Nothing
                dockPanel.Extender.DockPaneCaptionFactory = Nothing
                dockPanel.Extender.AutoHideStripFactory = New AutoHideStripOverrideFactory()
                dockPanel.Extender.DockPaneStripFactory = New DockPaneStripOverrideFactory()
            ElseIf schema = Extender.Schema.FromBase Then
                dockPanel.Extender.AutoHideTabFactory = New AutoHideTabFromBaseFactory()
                dockPanel.Extender.DockPaneTabFactory = New DockPaneTabFromBaseFactory()
                dockPanel.Extender.AutoHideStripFactory = New AutoHideStripFromBaseFactory()
                dockPanel.Extender.DockPaneCaptionFactory = New DockPaneCaptionFromBaseFactory()
                dockPanel.Extender.DockPaneStripFactory = New DockPaneStripFromBaseFactory()
            End If
        End Sub

    End Class
End Namespace
