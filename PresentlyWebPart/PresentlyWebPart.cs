using System;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Serialization;
using Microsoft.SharePoint.WebPartPages;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Text;
using TwitterLib;
using jQuery.ScriptManager;

namespace com.intridea.presently
{
    [Guid("6541136b-b6ba-49b9-bf67-1a55eb5e9e75")]
    public class PresentlyWebPart : Microsoft.SharePoint.WebPartPages.WebPart
    {
        Literal lit;
        TextBox input;
        String _username;
        String _password;
        String _url;
        TwitterService _twitterService;
        Button update;

        protected jQueryManager jqueryManager = null;
        private int refreshInterval = 60;
        UpdatePanel refreshBox;
        Timer timer;
        ScriptManager scriptHandler;
        bool property_modified = false;

        [Personalizable(PersonalizationScope.User), Category("Settings"), WebDisplayName("RefreshRate"), WebDescription("Use this property to set the refresh rate")]
        public int RefreshRate
        {
            get { return refreshInterval; }
            set { refreshInterval = value; }
        }
        [WebPartStorage(Storage.Personal)]
        public bool SettingModified
        {
            get { return property_modified; }
            set { property_modified = value;}
        }
        //[Browsable(false), Category("Miscellaneous"), WebPartStorage(Storage.Shared), XmlElement(ElementName = "Scope")]
        [Personalizable(PersonalizationScope.User), Category("Settings"), WebDisplayName("Username"), WebDescription("Use this property to set the user whose public timeline will be displayed")]
        public String Username
        {
            get { return _username; }
            set { _username = value; }
        }

        //[Browsable(false), Category("Miscellaneous"), WebPartStorage(Storage.Shared), XmlElement(ElementName = "Scope")]
        [Personalizable(PersonalizationScope.User), Category("Security"), WebDisplayName("Password"), WebDescription("Use this property to set the user password")]
        [PasswordPropertyText(true)]
        public String Password
        {
            get { return _password; }
            set { _password = value;}
        }
        //[Browsable(false), Category("Miscellaneous"), WebPartStorage(Storage.Shared), XmlElement(ElementName = "Scope")]
        [Personalizable(PersonalizationScope.User), Category("Settings"), WebDisplayName("Url"), WebDescription("Use this property to set the domain name")]
        public String Url
        {
            get { return _url; }
            set { _url = value;}
        }

        protected override void OnInit(EventArgs e)
        {
            //find our JQueryManager, if it doesn't exist add one to the page
            jqueryManager = jQueryManager.GetCurrent(Page);
            if (jqueryManager == null)
            {
                jqueryManager = new jQueryManager();
                Page.Controls.Add(jqueryManager);
            }
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("Lightbox"))
                Page.ClientScript.RegisterClientScriptInclude("Lightbox", Page.ClientScript.GetWebResourceUrl(this.GetType(), "com.intridea.presently.js.jquery.lightbox-0.5.js"));
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("Presently"))
                Page.ClientScript.RegisterClientScriptInclude("Presently", Page.ClientScript.GetWebResourceUrl(this.GetType(), "com.intridea.presently.js.presently.js"));
            CssRegistration.Register(Page.ClientScript.GetWebResourceUrl(this.GetType(), "com.intridea.presently.css.jquery.lightbox-0.5.css"));
            CssRegistration.Register(Page.ClientScript.GetWebResourceUrl(this.GetType(), "com.intridea.presently.css.presently.css"));

            scriptHandler = ScriptManager.GetCurrent(Page);
            if (scriptHandler == null)
            {
                scriptHandler = new ScriptManager();
                scriptHandler.ID = "scriptHandler";
                this.Controls.Add(scriptHandler);
            }

            Literal div = new Literal();
            div.Text = "<div class='main_div'>";
            this.Controls.Add(div);
            if (refreshBox == null)
                refreshBox = new UpdatePanel();
            refreshBox.ID = this.ID + "refreshBox";
            refreshBox.UpdateMode = UpdatePanelUpdateMode.Conditional;
            refreshBox.ChildrenAsTriggers = true; 
            div = new Literal();
            div.Text = "<div id='big_box_update' class='update_box with_sidebar'>";
            refreshBox.ContentTemplateContainer.Controls.Add(div);
            input = new TextBox();
            input.CssClass = "presently_update_box";
            input.ID = "update_text";
            input.Rows = 3;
            input.TextMode = TextBoxMode.MultiLine;
            refreshBox.ContentTemplateContainer.Controls.Add(input);
            update = new Button();
            update.Text = "Update";
            update.ID = "big_box_submit";
            update.CssClass = "presently_update_submit";
            update.Click += new EventHandler(this.submit_Click);
            refreshBox.ContentTemplateContainer.Controls.Add(update);

            div = new Literal();
            div.Text = "</div>";
            refreshBox.ContentTemplateContainer.Controls.Add(div);
            div = new Literal();
            div.Text = "<div class='loading_div'> <img ALIGN=ABSMIDDLE src='/resources/PresentlyImages/PresentlyImages/loading.gif'/> &nbsp;&nbsp;Loading ... </div>";
            refreshBox.ContentTemplateContainer.Controls.Add(div);

            div = new Literal();
            div.Text = "<div class='twitterTimeline'> ";
            refreshBox.ContentTemplateContainer.Controls.Add(div);
            if (lit == null)
            {
                lit = new Literal();
                lit.Text = "";
            }
            refreshBox.ContentTemplateContainer.Controls.Add(lit);
            div = new Literal();
            div.Text = "</div>";
            refreshBox.ContentTemplateContainer.Controls.Add(div);
            //The ScriptManager control must be added first.
            //refreshBox.ContentTemplateContainer.Controls.Add(timer);
            this.Controls.Add(refreshBox);

            div = new Literal();
            div.Text = "</div>";
            this.Controls.Add(div);

            SPSite mySite = SPContext.Current.Site;
            SPWeb myWeb = SPContext.Current.Web;
            CreateList(mySite, myWeb);
            
            base.OnInit(e);


        }

        public PresentlyWebPart()
        {
            _twitterService = new TwitterService(this);
            this.AllowEdit=true;
        }
        public PresentlyWebPart(String user, String password, String subdomain)
        {
            this.Username = user;
            this.Password = password;
            this.Url = subdomain;
            _twitterService = new TwitterService(this);
            this.AllowEdit = true;            
        }
        protected override void OnPreRender(EventArgs e)
        {
            if (_twitterService == null)
                _twitterService = new TwitterService(this);
            else if (property_modified)
            {
                _twitterService.updateLogins(this.Username, this.Password, this.Url);
                lit.Text = GetTweets();
                timer.Interval = refreshInterval * 1000;

                property_modified = false;
            }            
            base.OnPreRender(e);
        }
        protected override void CreateChildControls() 
        {

            if (timer == null)
                timer = new Timer();
            timer.ID = this.ID + "timer";
            timer.Interval = refreshInterval * 1000;
            timer.Tick += new EventHandler<EventArgs>(this.TimerHandler);
            //this.Controls.Add(timer);

            if (update == null)
                update = new Button();
            update.Text = "Update";
            update.ID = "big_box_submit";
            update.CssClass = "presently_update_submit";
            update.Click += new EventHandler(this.submit_Click);
            if (refreshBox == null)
                refreshBox = new UpdatePanel();
            refreshBox.ContentTemplateContainer.Controls.Add(timer);

            //EnsurePostBack();

            if (_twitterService == null)
                _twitterService = new TwitterService(this);
            else {
                _twitterService.updateLogins(this.Username, this.Password, this.Url);
                property_modified = false;
            }
            //HtmlGenericControl stylesheet = new HtmlGenericControl("style");
            //stylesheet.InnerHtml = Constants.Styles;
            //this.Controls.Add(stylesheet);

            if (lit == null)
            {
                lit = new Literal();
                lit.Text = "";
            }
            if (!_twitterService.isConfigured())
                lit.Text = "<br/>Please provide presently URL and User/Password in the settings.<br/>" + lit.Text;
            /*else if (!Page.IsPostBack)
                lit.Text = GetTweets();
            */
            if (refreshBox.Triggers != null)
            {
                refreshBox.Triggers.Clear();
                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                trigger.ControlID = timer.ID;
                trigger.EventName = "Tick";
                refreshBox.Triggers.Add(trigger);
                trigger = new AsyncPostBackTrigger();
                trigger.ControlID = update.ID;
                trigger.EventName = "Click";
                refreshBox.Triggers.Add(trigger);
            }

        }
        private void submit_Click(object sender, EventArgs e)
        {
            // Update the label string.
            if (input.Text != String.Empty)
            {
                _twitterService.SendTweet(input.Text);
            }
            input.Text = "";
            this.lit.Text = GetTweets();
            LiteralControl lc = new LiteralControl();
            lc.ID = "refreshBoxId";
            scriptHandler.RegisterDataItem(lc, this.refreshBox.ClientID);
        }

        private void StoreAttachments(TweetCollection tc)
        {
            SPSite mySite = SPContext.Current.Site;
            SPWeb myWeb = SPContext.Current.Web;
            SPList _myList = CreateList(mySite, myWeb);

            foreach (Tweet tweet in tc)
            if (tweet.Assets != null) {
                foreach (Attachment att in tweet.Assets)
                {
                    SPQuery oQuery = new SPQuery();
                    oQuery.Query = 
                        "<Where>"+
                            "<And>"+
                                "<Eq><FieldRef Name='DocumentId'/>" +
                                   "<Value Type='Number'>"+att.Id+"</Value></Eq>" +
                                "<Eq><FieldRef Name='PresentlyUrl'/>" +
                                    "<Value Type='Text'>" + this.Url + "</Value></Eq>"+
                            "</And>"+
                        "</Where>";
                    SPListItemCollection collListItems = _myList.GetItems(oQuery);
                    if (collListItems.Count == 0)
                    {
                        SPListItem myListItem = _myList.Items.Add();
                        myListItem["DocumentId"] = att.Id;
                        myListItem["Title"] = att.FileName;
                        myListItem["URL"] = att.Url;
                        myListItem["Size"] = att.Size;
                        myListItem["MediaType"] = att.ContentType;
                        myListItem["PresentlyUrl"] = this.Url;
                        if (att.ContentType.StartsWith("image"))
                        {
                            myListItem["Preview"] = att.Url.Replace("/original/", "/stream_multi_thumb/");
                        }
                        myListItem["DateCreated"] = att.DateCreated;
                        myWeb.AllowUnsafeUpdates = true;
                        myListItem.Update();
                        myWeb.AllowUnsafeUpdates = false;
                    }
                }
            }
        }


        public override object WebBrowsableObject
        {
            get { return this; }
        }


        public override EditorPartCollection CreateEditorParts()
        {

            ConfigPart tp = new ConfigPart();
            tp.ID = this.ID + "ConnectionSettingsEditor";
            tp.Title = "Connection Settings";
            tp.ChromeState = PartChromeState.Normal;
            
            List<EditorPart> editors = new List<EditorPart>();
            
            /*PropertyGridEditorPart tp = new PropertyGridEditorPart();
            tp.ID = this.ID + "setting_editor";
            tp.Title = "Connection Settings";
            */
            editors.Add(tp);

            EditorPartCollection result = new EditorPartCollection(editors);
            
            return result;
        }

        public String GetTweets()
        {
            lock (typeof(PresentlyWebPart))
            {
                try
                {

                    TweetCollection tweets = _twitterService.GetTweets();
                    StoreAttachments(tweets);
                    return TweetBuilder.buildTweets(tweets);
                }
                catch (Exception err)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div>" + err.Message + "</div>" + "<div>" + err.StackTrace + "</div>");
                    return sb.ToString();
                }
            }
        }


        private void TimerHandler(object sender, EventArgs eventArgs)
        {
            this.lit.Text = GetTweets();
            LiteralControl lc = new LiteralControl();
            lc.ID = "refreshBoxId";
            scriptHandler.RegisterDataItem(lc, this.refreshBox.ClientID);
        }

        private void EnsurePostBack()
        {
            if (Page.IsPostBack && Page.IsAsync)
                return;
            string fixupScript = @"
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function BeginRequestHandler(sender, args)
            {
                $j('.loading_div').show();
            }

            function EndRequestHandler(sender, args)
            {
             $j('.loading_div').hide();
             update_links();
             highlight_tweets();
            }
            ";
             ScriptManager.RegisterStartupScript(this, typeof(PresentlyWebPart), "UpdatePanelPostBack", fixupScript, true);
        }

        private SPList CreateList(SPSite site, SPWeb web)
        {
            SPList list = null;
            try
            {
                list = web.Lists["Presently Documents"];
            }
            catch
            {
                list = null;
            }
            try
            {
                if ((list == null))
                {
                    web.AllowUnsafeUpdates = true;
                    SPListTemplateCollection customListTemplates = site.GetCustomListTemplates(web); //create the connection library using the uploaded list template
                    SPListTemplate listTemplate = customListTemplates["Presently Document Template"];
                    Guid guid = web.Lists.Add("Presently Documents", "A custom list to store presently documents", listTemplate);
                    SPList presentlyList = web.Lists.GetList(guid, false);
                    presentlyList.OnQuickLaunch = true;
                    presentlyList.Update();
                    web.AllowUnsafeUpdates = false;
                    return presentlyList;
                }
                return list;
            }
            catch (Exception err)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<div>" + err.Message + "</div>" + "<div>" + err.StackTrace + "</div>");
                Literal error = new Literal();
                error.Text = sb.ToString();
                this.Controls.Add(error);
                return null;
            }
        }

        private void EnsurePanelFix()
        {
            if (this.Page.Form != null)
            {
                string fixupScript = @"_spBodyOnLoadFunctionNames.push(""_initFormActionAjax"");

             function _initFormActionAjax()
             {
               if (_spEscapedFormAction == document.forms[0].action)
               {
                 document.forms[0]._initialAction = document.forms[0].action;
               }
             }

             RestoreToOriginalFormAction = function()
             {
               if (_spOriginalFormAction != null)
               {
                 RestoreToOriginalFormActionCore();
                 document.forms[0]._initialAction = document.forms[0].action;
               }
             }
            var RestoreToOriginalFormActionCore = RestoreToOriginalFormAction;

            function RunThisAfterEachAsyncPostback()
            {
             update_links();
             highlight_tweets();
            }
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(RunThisAfterEachAsyncPostback);
            // runs each time postback is initiated by any update panel on the page
            ";                
            ScriptManager.RegisterStartupScript(Page, typeof(PresentlyWebPart), "UpdatePanelFixup", fixupScript, true);
            }
        }
    }

}
