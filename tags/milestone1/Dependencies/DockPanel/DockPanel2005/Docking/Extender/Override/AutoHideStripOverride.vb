Imports System
Imports WeifenLuo.WinFormsUI
Imports System.Drawing
Imports System.Drawing.Drawing2D
Namespace VS2005Style
    Public Class AutoHideStripOverride
        Inherits AutoHideStripVS2003
        Protected Friend Sub New(ByVal dockPanel As DockPanel)
            MyBase.New(dockPanel)
            BackColor = Color.Yellow
        End Sub
    End Class
End Namespace