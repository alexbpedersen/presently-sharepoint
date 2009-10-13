<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Config.ascx.cs" Inherits="com.intridea.presently, com.intridea.presently.Config, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c038812e8af423a1" %>

<table style="border-top-width: 0px; border-left-width: 0px; border-bottom-width: 0px;
    width: 100%; border-collapse: collapse; border-right-width: 0px" cellspacing="0"
    border="0">
	<tr>
		<td>
			<div class="UserSectionTitle">
				Presently Web Part Settings
            </div>
        </td>
    </tr>
	<tr>
		<td>
			<div class="UserSectionHead">
				<asp:Label ID="ErrorLabel" Style="color: red" runat="server" 
                    meta:resourcekey="ErrorLabelResource1"></asp:Label>
            </div>
            <div class="UserDottedLine" style="width: 100%">
            </div>
        </td>
    </tr>
	<tr>
		<td>
            <div class="UserSectionHead">
                Sub-Domain 
            </div>
            <div class="UserSectionBody">
                <div class="UserControlGroup">
                    <asp:TextBox ID="SubdomainTextBox" runat="server" 
                        meta:resourcekey="SubdomainTextBoxResource1"></asp:TextBox>
                
                </div>
            </div>
            <div class="UserDottedLine" style="width: 100%">
            </div>
        </td>
    </tr>
	<tr>
		<td>
            <div class="UserSectionHead">
                Username
            </div>
            <div class="UserSectionBody">
                <div class="UserControlGroup">
                    <asp:TextBox ID="UsernameTextBox" runat="server" 
                        meta:resourcekey="UsernameTextBoxResource1"></asp:TextBox>
                </div>
            </div>
            <div class="UserDottedLine" style="width: 100%">
            </div>
        </td>
    </tr>
 	<tr>
		<td>
            <div class="UserSectionHead">
                Password
            </div>
            <div class="UserSectionBody">
                <div class="UserControlGroup">
					<asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" 
                        meta:resourcekey="PasswordTextBoxResource1"></asp:TextBox>
                </div>
            </div>
            <div class="UserDottedLine" style="width: 100%">
            </div>
        </td>
    </tr>
</table>
                    
                
