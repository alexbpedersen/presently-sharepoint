using System;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;


namespace com.intridea.presently
{
    class ConfigPart : EditorPart
    {
        private Config uc;

        private PresentlyWebPart SelectedWebPart
        {
            get { return (PresentlyWebPart)WebPartToEdit; }
        }

        public override bool ApplyChanges()
        {
            try
            {
                if (Page.IsPostBack)
                {
                    SelectedWebPart.Username = uc.Username;
                    SelectedWebPart.Url = uc.Subdomain;
                    SelectedWebPart.Password = uc.Password;

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            string ucPath = UserControlsVirtualPath() + "Config.ascx";
            uc = (Config)Page.LoadControl(ucPath);
            uc.Username = SelectedWebPart.Username;
            uc.Password = SelectedWebPart.Password;
            uc.Subdomain = SelectedWebPart.Url;

            Controls.Add(uc);

        }

        public override void SyncChanges()
        {
            uc.Username = SelectedWebPart.Username;
            uc.Password = SelectedWebPart.Password;
            uc.Subdomain = SelectedWebPart.Url;
        }



        /// <summary>
        /// Gets the path where user controls can be loaded from for this web part.
        /// This is a virtual path, site root relative, ending in a slash.
        /// </summary>
        /// <returns>String with path</returns>
        public string UserControlsVirtualPath()
        {
            SPWeb currentWeb = SPControl.GetContextWeb(Context);
            Type currentType = GetType();
            string classResourcePath = SPWebPartManager.GetClassResourcePath(currentWeb, currentType);

            int startIndex = classResourcePath.IndexOf("/wpresources/");
            if (startIndex < 0)
            {
                startIndex = classResourcePath.IndexOf("/_wpresources/");
            }
            if (startIndex < 0)
            {
                throw new Exception(String.Format("ClassResourcePath '{0}' should contain /wpresources/ or /_wpresources/", classResourcePath));
            }

            classResourcePath = classResourcePath.Substring(startIndex);

            classResourcePath = string.Concat(classResourcePath, "/com.intridea.presently/");

            return classResourcePath;
        }

    }
}