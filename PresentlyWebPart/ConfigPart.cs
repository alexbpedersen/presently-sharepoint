using System;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;


namespace com.intridea.presently
{
    class ConfigPart : EditorPart
    {
        private PresentlyConfig uc;

        private PresentlyWebPart SelectedWebPart
        {
            get { return (PresentlyWebPart)WebPartToEdit; }
        }

        public override bool ApplyChanges()
        {
            EnsureChildControls();
            try
            {
                if (Page.IsPostBack)
                {
                    SelectedWebPart.Username = uc.Username;
                    SelectedWebPart.Url = uc.Url;
                    if (uc.Password.Length != 0)
                        SelectedWebPart.Password = uc.Password;
                    SelectedWebPart.RefreshRate = uc.RefreshRate;
                    SelectedWebPart.SettingModified = true;
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
            if (uc == null)
                uc = (PresentlyConfig)Page.LoadControl("~/_controltemplates/PresentlyConfig.ascx");
            uc.Username = SelectedWebPart.Username;
            uc.Password = SelectedWebPart.Password;
            uc.Url = SelectedWebPart.Url;
            uc.RefreshRate = SelectedWebPart.RefreshRate;

            Controls.Add(uc);

        }

        public override void SyncChanges()
        {
            EnsureChildControls();
            uc.Username = SelectedWebPart.Username;
            uc.Password = SelectedWebPart.Password;
            uc.Url = SelectedWebPart.Url;
            uc.RefreshRate = SelectedWebPart.RefreshRate;
        }
    }
}