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
        int Template { get; set; }
        string Type { get; set; }
        string Code { get; set; }
        string Source { get; set; }
        string OtherInformation { get; set; }
        List<IStanza> Stanzas { get; set; }
        IComposer Composer { get; set; }
        IComposer Arranger { get; set; }
        List<IInstrumentalPart> InstrumentalParts { get; set; }
        List<int> ThemeCategories { get; set; }
        List<int> LiturgyCategories { get; set; }
    }
}
