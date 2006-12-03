<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" Title="PSP Compatability Database : Games" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<table border="0" cellpadding="0" cellspacing="0" width="80%">
		<tr>
			<td>
				<div style="text-align: left;">
					<br />
					<asp:Label ID="MessageLabel" runat="server" Text="Label" Font-Bold="True" ForeColor="Firebrick"></asp:Label>
					<hr style="width: 100%; height: 1px; margin: 0;" />
					<asp:Label ID="DescriptionLabel" runat="server" Text="Label"></asp:Label><br />
					<br />
					<strong>More Information:</strong>
					<asp:Label ID="MoreInformationLabel" runat="server" Text="Label"></asp:Label><br />
					<br />
					<hr style="width: 100%; height: 1px; margin: 0;" />
					<strong>Common Solutions:</strong><br />
					Not yet implemented -_-
					<asp:Table ID="CauseTable" runat="server">
					</asp:Table>
					<br />
					<br />
				</div>
			</td>
		</tr>
	</table>
</asp:Content>
