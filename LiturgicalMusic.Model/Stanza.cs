using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Model
{
    public class Stanza : IStanza
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }
}
