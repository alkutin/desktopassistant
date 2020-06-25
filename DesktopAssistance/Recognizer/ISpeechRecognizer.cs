using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAssistance.Recognizer
{
    public interface ISpeechRecognizer
    {
        void Init(params string[] phrases);
        void Talk(string message);

        event Action<string, float, string[]> OnSpeech;
    }
}
