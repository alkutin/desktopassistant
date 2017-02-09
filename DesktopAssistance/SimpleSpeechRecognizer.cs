﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAssistance
{
    class SimpleSpeechRecognizer
    {
        private SpeechRecognitionEngine _recognitionEngine = new SpeechRecognitionEngine(CultureInfo.GetCultureInfo("en-US"));
        private GrammarBuilder _grammarBuilder = new GrammarBuilder();
        private DictationGrammar _dictationGrammar = new DictationGrammar();

        public event Action<string, float, string[]> OnSpeech;

        public void Init(params string[] phrases)
        {
            //_grammarBuilder.AppendWildcard();
            _recognitionEngine.SetInputToDefaultAudioDevice();//микрофон
            _grammarBuilder.Append(new Choices(phrases));//добавляем используемые фразы
            _recognitionEngine.UnloadAllGrammars();
            if (phrases.Any())
                _recognitionEngine.LoadGrammar(new Grammar(_grammarBuilder));//загружаем "грамматику"
            else _recognitionEngine.LoadGrammar(_dictationGrammar);
            _recognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);//событие речь распознана
            _recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);//начинаем распознование
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (OnSpeech != null)
                OnSpeech.Invoke(e.Result.Text, e.Result.Confidence, e.Result.Alternates.Where(w => w.Confidence > 0.3).Select(s => s.Text).ToArray());
        }
    }
}