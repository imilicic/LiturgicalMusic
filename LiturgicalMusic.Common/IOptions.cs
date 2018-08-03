using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Common
{
    public interface IOptions
    {
        bool Stanzas { get; set; }
        bool Composer { get; set; }
        bool Arranger { get; set; }
        bool InstrumentalParts { get; set; }
        bool ThemeCategories { get; set; }
        bool LiturgyCategories { get; set; }
    }
}
