using System;
using System.Collections.Generic;
using System.Text;

namespace jQuery.ScriptManager
{
    public class StartFunction
    {
        private string _functionName = String.Empty;
        public string FunctionName
        {
            get
            {
                return _functionName;
            }
            set
            {
                _functionName = value;
            }
        }

        public StartFunction()
        {
        }

        public StartFunction(string Start)
        {
            _functionName = Start;
        }
    }
}
