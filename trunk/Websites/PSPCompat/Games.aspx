<%@ Page AutoEventWireup="true" CodeFile="Games.aspx.cs" EnableViewState="false" Inherits="Games" Language="C#" MasterPageFile="~/MasterPage.master" Title="PSP Compatability Database : Games" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div id="ctxtnav" class="nav">
		<ul>
			<li><a href="?">All</a></li><li><a href="?letter=@">0-9/etc</a></li><li><a href="?letter=a">A</a></li><li><a href="?letter=b">B</a></li><li><a href="?letter=c">C</a></li><li><a href="?letter=d">D</a></li><li><a href="?letter=e">E</a></li><li><a href="?letter=f">F</a></li><li><a href="?letter=g">G</a></li><li><a href="?letter=h">H</a></li><li><a href="?letter=i">I</a></li><li><a href="?letter=j">J</a></li><li><a href="?letter=k">K</a></li><li><a href="?letter=l">L</a></li><li><a href="?letter=m">M</a></li><li><a href="?letter=n">N</a></li><li><a href="?letter=o">O</a></li><li><a href="?letter=p">P</a></li><li><a href="?letter=q">Q</a></li><li><a href="?letter=r">R</a></li><li><a href="?letter=s">S</a></li><li><a href="?letter=t">T</a></li><li><a href="?letter=u">U</a></li><li><a href="?letter=v">V</a></li><li><a href="?letter=w">W</a></li><li><a href="?letter=x">X</a></li><li><a href="?letter=y">Y</a></li><li><a href="?letter=z">Z</a></li><li class="last"><a href="?recent=true">Recent Changes</a></li></ul>
		<!--<br style="line-height:0.4" />
		<ul>
			<li class="last"><a href="/pspdev/wiki/WikiStart?action=history">Recent Changes</a></li>
		</ul>
		<hr />-->
	</div>
	<div id="content">
		<asp:Label ID="InfoLabel" runat="server" Text="Label" Visible="False"></asp:Label>
		<asp:Literal ID="AdminToolsLiteral" runat="server"><br /><a href="Games.aspx?action=ShowPendingApproval">Pending Approval</a> | </asp:Literal><asp:LinkButton ID="ApproveAllButton" runat="server" OnClick="ApproveAllButton_Click">Approve All</asp:LinkButton> <asp:LinkButton ID="ApproveSelectedButton" runat="server" OnClick="ApproveSelectedButton_Click">Approve/Delete Selected</asp:LinkButton>
		<br />
		<br />
		<asp:Table ID="gamesTable" runat="server">
		</asp:Table>
	</div>
</asp:Content>
