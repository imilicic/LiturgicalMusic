using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Common
{
    public class Options : IOptions
    {
        public bool Stanzas { get; set; }
        public bool Composer { get; set; }
        public bool Arranger { get; set; }
        public bool InstrumentalParts { get; set; }
        public bool ThemeCategories { get; set; }
        public bool LiturgyCategories { get; set; }
    }
}
