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
    Friend Class AutoHideTabFromBase
        Inherits AutoHideTab
        Friend Sub New(ByVal content As IDockContent)
            MyBase.New(content)
        End Sub
        Private m_tabX As Integer = 0
        Protected Friend Property TabX() As Integer
            Get
                Return m_tabX
            End Get
            Set(ByVal value As Integer)
                m_tabX = value
            End Set
        End Property
        Private m_tabWidth As Integer = 0
        Protected Friend Property TabWidth() As Integer
            Get
                Return m_tabWidth
            End Get
            Set(ByVal value As Integer)
                m_tabWidth = value
            End Set
        End Property
    End Class
End Namespace
