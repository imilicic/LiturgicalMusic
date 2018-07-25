using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Model.Common
{
    public interface ICode
    {
        string Key { get; set; }
        string Time { get; set; }
        string OrganSoprano { get; set; }
        string OrganAlto { get; set; }
        string OrganTenor { get; set; }
        string OrganBass { get; set; }
        string OrganSopranoRelative { get; set; }
        string OrganAltoRelative { get; set; }
        string OrganTenorRelative { get; set; }
        string OrganBassRelative { get; set; }
        string VoiceSoprano { get; set; }
        string VoiceAlto { get; set; }
        string VoiceTenor { get; set; }
        string VoiceBass { get; set; }
        string VoiceSopranoRelative { get; set; }
        string VoiceAltoRelative { get; set; }
        string VoiceTenorRelative { get; set; }
        string VoiceBassRelative { get; set; }
    }
}
