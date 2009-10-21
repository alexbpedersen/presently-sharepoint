<%@ Control Language="C#" AutoEventWireup="true" Inherits="com.intridea.presently.PresentlyConfig, PresentlyWebPart, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d49903b17ff8148c" %>

<table style="border-top-width: 0px; border-left-width: 0px; border-bottom-width: 0px;
    width: 100%; border-collapse: collapse; border-right-width: 0px" cellspacing="0"
    border="0">
	<tr>
         <td >

               <asp:RequiredFieldValidator
                    ID="Value1RequiredValidator"
                    ControlToValidate="UrlTextBox"
                    ErrorMessage="Please enter Url.<br />"
                    Display="Dynamic"
                    runat="server"/>
            </td>

    </tr>
	<tr>
         <td >

               <asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1"
                    ControlToValidate="UsernameTextBox"
                    ErrorMessage="Please enter User Name.<br />"
                    Display="Dynamic"
                    runat="server"/>
            </td>

    </tr>
	<tr>
		<td>
            <div class="UserSectionHead">
                Url
            </div>
            <div class="UserSectionBody">
                <div class="UserControlGroup">
                    <asp:TextBox ID="UrlTextBox" runat="server" 
                        meta:resourcekey="UrlTextBoxResource1"></asp:TextBox>
                
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
 	<tr>
		<td>
            <div class="UserSectionHead">
                Auto-Refresh Rate
            </div>
            <div class="UserSectionBody">
                <div class="UserControlGroup">
					<asp:DropDownList ID="RefreshDropDown" runat="server"  
                        meta:resourcekey="RefreshDropDownResource1">
                          <asp:ListItem Value="60"> 1 min </asp:ListItem>
                          <asp:ListItem Value="180"> 3 mins </asp:ListItem>
                          <asp:ListItem Value="300"> 5 mins </asp:ListItem>
                          <asp:ListItem Value="600"> 10 mins </asp:ListItem>
                        </asp:DropDownList>
                </div>
            </div>
            <div class="UserDottedLine" style="width: 100%">
            </div>
        </td>
    </tr>    
</table>
                    
                
