using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Collections.ObjectModel;

namespace jQuery.ScriptManager
{
    [ParseChildren(true)]
    public class jQueryManager : WebControl
    {
        private ITemplate scriptTemplate;
        private LiteralControl scriptLiteral = new LiteralControl();

        private List<StartFunction> _readyFunctions = new List<StartFunction>();
        [Browsable(true), DefaultValue(null), PersistenceMode(PersistenceMode.InnerProperty)]
        public List<StartFunction> ReadyFunctions
        {
            get
            {
                return _readyFunctions;
            }
            set
            {
                _readyFunctions = value;
            }
        }

        private List<ScriptBlock> _otherFunctions;
        [Browsable(true), DefaultValue(null), PersistenceMode(PersistenceMode.InnerProperty)]
        public List<ScriptBlock> Scripts
        {
            get
            {
                return _otherFunctions;
            }
            set
            {
                _otherFunctions = value;
            }
        }

        //http://blog.iridescence.no/Posts/EnsuringaSingleInstanceofaControlonaPage.aspx
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page page = this.Page;
            if (page != null)
            {
                // check if an instance has already been added to the page
                if (page.Items.Contains(typeof(jQueryManager)))
                {
                    throw new InvalidOperationException("Only one instance of the jQueryManager control can be placed on a Page");
                }

                // add a reference to the Items collection for easy reference to the control; GetCurrent() will use this
                page.Items[typeof(jQueryManager)] = this;
            }

            if (scriptTemplate != null)
            {
                scriptTemplate.InstantiateIn(this);
                
            }

            Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);

            Page.ClientScript.RegisterClientScriptInclude("jqueryScript", Page.ClientScript.GetWebResourceUrl(this.GetType(), "com.intridea.presently.js.jquery-1.3.2.min.js"));
            Page.ClientScript.RegisterClientScriptInclude("jqueryUIScript", Page.ClientScript.GetWebResourceUrl(this.GetType(), "com.intridea.presently.js.jquery-ui-1.7.2.min.js"));
        }

        protected override void  AddParsedSubObject(object obj)
        {
            if (obj is LiteralControl)
            {
                RegisterStartFunction(((LiteralControl)obj).Text);
            }
            else
            {
                base.AddParsedSubObject(obj);
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            StringBuilder Start = new StringBuilder();

            if (Scripts != null)
            {
                foreach (ScriptBlock r in Scripts)
                    Start.Append(r.JSScriptBlock + Environment.NewLine);
            }

            if (ReadyFunctions != null)
            {
                Start.Append("$j(document).ready(function(){");

                foreach (StartFunction r in ReadyFunctions)
                    Start.Append(r.FunctionName + Environment.NewLine);

                Start.Append("});\n\n");
            }
            

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "JQuery", Start.ToString(), true);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// Gets the instance of the jQueryManager on the page
        /// </summary>
        /// <param name="page">Current Page</param>
        /// <returns>Instance of the jQueryManager</returns>
        public static jQueryManager GetCurrent(Page page)
        {
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }

            jQueryManager manager = page.Items[typeof(jQueryManager)] as jQueryManager;

            
            return manager;
        }

        /// <summary>
        /// Registers a script
        /// </summary>
        /// <param name="Script">The full contents of the script (function name(){ ... })</param>
        public void RegisterScript(string Script)
        {
            if (_otherFunctions == null)
                _otherFunctions = new List<ScriptBlock>();

            _otherFunctions.Add(new ScriptBlock(Script));
        }

        /// <summary>
        /// Registers the name of a function to call on $(document).ready
        /// </summary>
        /// <param name="FunctionName">The name of the function</param>
        public void RegisterStartFunction(string FunctionName)
        {
            if (_readyFunctions == null)
                _readyFunctions = new List<StartFunction>();

            Boolean pass = true;
            foreach (StartFunction reg in _readyFunctions)
            {
                if (reg.FunctionName == FunctionName)
                {
                    pass = false;
                }
            }

            if (pass)
            {
                _readyFunctions.Add(new StartFunction(FunctionName));
            }
        }
        

        [PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(TemplateControl))]
        public ITemplate ScriptTemplate
        {
            get { return scriptTemplate; }
            set { scriptTemplate = value; }
        }



    }

    

}
