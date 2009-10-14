using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Drawing;

namespace com.intridea.presently
{
    public partial class PresentlyConfig : System.Web.UI.UserControl
    {
        private string _url;
        private string _username;
        private string _password;

        public string Url
        {
            get
            {
                return UrlTextBox.Text;
            }
            set
            {
                _url = value;
            }
        }

        public string Username
        {
            get
            {
                return UsernameTextBox.Text;
            }
            set
            {
                _username = value;
            }
        }

        public string Password
        {
            get
            {
                return PasswordTextBox.Text;
            }
            set
            {
                _password = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void CreateChildControls()
        {
            UrlTextBox.Text = _url;
            UsernameTextBox.Text = _username;
            PasswordTextBox.Text = _password;
            base.CreateChildControls();

        }
    }
}