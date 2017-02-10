using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DesktopAssistance.Model;

namespace DesktopAssistance
{
    public class RunEngine
    {
        private readonly CommandsManager _commands;
        private readonly EventHandler<string> _onShowMessageEventHandler;

        public string CurrentContextName { get; private set; }

        public RunEngine(CommandsManager commands, EventHandler<string> onShowMessageEventHandler)
        {
            _commands = commands;
            _onShowMessageEventHandler = onShowMessageEventHandler;

            Debug.WriteLine(string.Join(", ", new Type[] {typeof(Prebuilt.Desktop.Windows), typeof(Prebuilt.Context.Management) }.Select(s => s.Name)));
        }

        public void RunCommand(string text)
        {
            var contexts = _commands.Commands.Contexts.Where(w => w.Name == "Main" || w.Name == CurrentContextName);
            var command = contexts.SelectMany(s => s.Commands).Where(w => Regex.IsMatch(text, w.Mask)).FirstOrDefault();
            if (command == null)
            {
                //_onShowMessageEventHandler(this, "Unknown command: " + text);
                ShowAllCommands();
                return;
            }

            CurrentContextName = contexts.First(w => w.Commands.Contains(command)).Name;

            switch (command.Type)
            {
                case "Method":
                    InvokeMethod(command.Value, text);
                    break;
            }
        }

        public List<string> GetAllCommands()
        {
            return _commands.Commands.Contexts.Where(w => w.Name == "Main" || w.Name == CurrentContextName)
                .SelectMany(s => s.Commands)
                .Select(sc => sc.Title).ToList();
        }

        public void ShowAllCommands()
        {
            _onShowMessageEventHandler(this, string.Join(", ", GetAllCommands()));
        }

        private void InvokeMethod(string value, string text)
        {
            var idx = value.LastIndexOf('.') + 1;
            var methodName = value.Substring(idx, value.Length - idx);
            var className = value.Substring(0, idx - 1);

            var objectType = Type.GetType(className + ", Prebuilt");
            var objectInstance = objectType.GetConstructor(new []{typeof(EventHandler<string>), typeof(string) }).Invoke(new object[] {_onShowMessageEventHandler, text});
            objectType.GetMethod(methodName).Invoke(objectInstance, null);
        }
    }
}
