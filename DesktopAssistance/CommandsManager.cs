using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DesktopAssistance.Model;

namespace DesktopAssistance
{
    public class CommandsManager
    {
        private string _path;
        public Commands Commands { get; private set; }

        public bool Loaded { get; private set; }

        public CommandsManager()
        {
            Load();
        }

        public void Load()
        {
            _path = Path.GetFullPath(System.Configuration.ConfigurationManager.AppSettings["Commands"]);
            if (!File.Exists(_path))
            {
                Loaded = false;
                Commands = new Commands();
                return;
            }

            Commands = (Commands)new XmlSerializer(typeof (Commands)).Deserialize(File.OpenRead(_path));

            Loaded = true;
        }
    }
}
