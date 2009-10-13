using System;
using System.Collections.Generic;
using System.Text;

namespace jQuery.ScriptManager
{
    public class ScriptBlock
    {
        private string _scriptBlock;
        public string JSScriptBlock
        {
            get
            {
                return _scriptBlock;
            }
            set
            {
                _scriptBlock = value;
            }
        }

        public ScriptBlock()
        {
        }

        public ScriptBlock(string Script)
        {
            _scriptBlock = Script;
        }
    }
}
