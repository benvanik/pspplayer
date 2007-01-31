<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AuthenticationControl.ascx.cs" Inherits="AuthenticationControl" EnableViewState="false" %>
<div style="height: 45px">
	<asp:Panel ID="PreAuthPanel" runat="server">
		<div style="text-align: right;">
			<span style="font-size: 10pt">User: </span>
			<asp:TextBox ID="UsernameTextBox" runat="server" AccessKey="u" AutoCompleteType="Email" Font-Size="Small" Width="77px"></asp:TextBox>
			<span style="font-size: 10pt">Password: </span>
			<asp:TextBox ID="PasswordTextBox" runat="server" Font-Size="Small" TextMode="Password" Width="77px"></asp:TextBox>
			<asp:ImageButton ID="LoginButton" runat="server" ImageUrl="~/Resources/Login.png" AlternateText="Sign In" OnClick="LoginButton_Click" /></div>
	</asp:Panel>
	<asp:Panel ID="PostAuthPanel" runat="server">
		<div style="text-align: right;">
			<asp:Label ID="UserInfoLabel" runat="server" Text="Logged In"></asp:Label>&nbsp;
			<asp:LinkButton ID="SignOutButton" runat="server" AccessKey="l" OnClick="SignOutButton_Click">Sign Out</asp:LinkButton></div>
	</asp:Panel>
</div>
