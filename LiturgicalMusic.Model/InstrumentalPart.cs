using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Model
{
    public class InstrumentalPart : IInstrumentalPart
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public List<bool> Template { get; set; }
    }
}
