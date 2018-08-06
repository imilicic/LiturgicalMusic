using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Model.Common
{
    public interface ISong
    {
        int Id { get; set; }
        string Title { get; set; }
        IList<bool> Template { get; set; }
        string Type { get; set; }
        string Code { get; set; }
        string Source { get; set; }
        string OtherInformations { get; set; }
        IList<IStanza> Stanzas { get; set; }
        IComposer Composer { get; set; }
        IComposer Arranger { get; set; }
        IList<IInstrumentalPart> InstrumentalParts { get; set; }
        IList<int> ThemeCategories { get; set; }
        IList<int> LiturgyCategories { get; set; }
    }
}
