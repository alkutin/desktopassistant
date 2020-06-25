using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace DesktopAssistance.Recognizer
{
    public class AzureSpeechRecognizer : ISpeechRecognizer
    {
        private SpeechRecognizer _recognizer;
        private System.Speech.Synthesis.SpeechSynthesizer _synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();


        public event Action<string, float, string[]> OnSpeech;

        public async void Init(params string[] phrases)
        {
            // var model = KeywordRecognitionModel;
            System.Diagnostics.Debug.WriteLine(typeof(SpeechConfig).FullName);
            var config = SpeechConfig.FromSubscription("e2bd9edf-125b-4b71-88b4-b065250f8004", "eastus");
            _recognizer = new SpeechRecognizer(config);
            _recognizer.Recognized += _recognizer_Recognized;
            //await _recognizer.StartContinuousRecognitionAsync();
            await _recognizer.RecognizeOnceAsync();//.ConfigureAwait(false);
            
        }

        private void _recognizer_Recognized(object sender, SpeechRecognitionEventArgs e)
        {
            if (OnSpeech != null)
                OnSpeech.Invoke(e.Result.Text, 1, new string[0]);
        }

        public async void Talk(string message)
        {            
            _synthesizer.Speak(message);

            await _recognizer.RecognizeOnceAsync().ConfigureAwait(false);
        }
    }
}
