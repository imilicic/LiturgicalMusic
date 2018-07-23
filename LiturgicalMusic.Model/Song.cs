using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Model
{
    public class Song : ISong
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Template { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Source { get; set; }
        public string OtherInformation { get; set; }
        public List<IStanza> Stanzas { get; set; }
        public IComposer Composer { get; set; }
        public IComposer Arranger { get; set; }
        public List<IInstrumentalPart> InstrumentalParts { get; set; }
        public List<int> ThemeCategories { get; set; }
        public List<int> LiturgyCategories { get; set; }
    }
}
