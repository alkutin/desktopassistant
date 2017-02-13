using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prebuilt.Context
{
    public class ContextState
    {
        public static readonly ContextState Instance = new ContextState();

        public bool VoiceEnabled { get; set; }
    }
}
