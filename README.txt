To Install Presently Web Part on your sharepoint server:
1) Copy the solutions package to your sharepoint server.

2) Run setup.bat command in the PresentlyWebPart/bin/Release directory to install Presently Web Part to your sharepoint server.

setup.bat /help
Usage:
setup.bat [/install or /uninstall][/weburl <url>][/siteurl <url>]
          [/help]

Options:
 /install or /uninstall
 Install specified Solution package (.wsp) to the SharePoint server
 or uninstall specified Solution from the SharePoint server.
 Default value: install
 /weburl
 Specify a web url of the SharePoint server.
 Default value: http://localhost/
 /siteurl
 Specify a site url of the SharePoint server.
 Default value: http://localhost/
 /help
 Show this information.
 
3) After the command is completed, login your sharepoint site, and click "Site Actions" > "Edit Page" link.
4) Click "Add A Web Part" link, add select "PresentlyWebPart Web Part" in the Miscellaneous section.
5) Once the PresentlyWebPart is added to your site, click "Modify Shared Web Part" link from the Web Part's drop down menu.
6) Enter Presently Server Url, user name and password. You can also change the auto-refresh rate. Click "Ok" to save the changes.
 