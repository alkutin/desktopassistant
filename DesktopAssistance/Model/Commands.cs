using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DesktopAssistance.Model
{
    [Serializable]
    [XmlRoot("Root")]
    public class Commands
    {

        [XmlArrayItem("Context")]
        public List<ContextItem> Contexts { get; set; }

        public class ContextItem
        {
            [XmlAttribute]
            public string Name { get; set; }

            [XmlArrayItem("Command")]
            public List<CommandItem> Commands { get; set; }

            public class CommandItem
            {
                [XmlAttribute]
                public string Title { get; set; }

                [XmlAttribute]
                public string Mask { get; set; }

                [XmlAttribute]
                public string Type { get; set; }

                [XmlAttribute]
                public string Value { get; set; }
            }
        }
    }
}
