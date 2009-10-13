using System;

using System.Web.UI;

namespace com.intridea.presently
{
    public partial class Config : UserControl
    {
        /// <summary>
        /// ErrorLabel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label ErrorLabel;

        /// <summary>
        /// UsernameTextBox control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox UsernameTextBox;

        /// <summary>
        /// PasswordTextBox control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox PasswordTextBox;
        /// <summary>
        /// PasswordTextBox control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox SubdomainTextBox;

        private string _subdomain;
        private string _username;
        private string _password;

        public string Subdomain
        {
            get
            {
                return SubdomainTextBox.Text;
            }
            set
            {
                _subdomain = value;
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
            SubdomainTextBox.Text = _subdomain;
            UsernameTextBox.Text = _username;
            PasswordTextBox.Text = _password;

        }
    }
}