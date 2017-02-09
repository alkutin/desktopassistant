using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prebuilt
{
    public class PrebuiltBase
    {
        public EventHandler<string> OnMessage { get; set; }
        public string CommandText { get; set; }

        public PrebuiltBase(EventHandler<string> onMessage, string commandText)
        {
            OnMessage = onMessage;
            CommandText = commandText;
        }
    }
}
