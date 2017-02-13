using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prebuilt.Context
{
    public class Management : PrebuiltBase
    {
        public Management(EventHandler<string> onMessage, string commandText) : base(onMessage, commandText)
        {
        }

        public void VoiceOn()
        {
            OnMessage(this, "Voice listening On");
            ContextState.Instance.VoiceEnabled = true;
        }

        public void VoiceOff()
        {
            OnMessage(this, "Voice listening Off");
            ContextState.Instance.VoiceEnabled = false;
        }
    }
}
