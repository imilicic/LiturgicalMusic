using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Model
{
    public class Code: ICode
    {
        public string Key { get; set; }
        public string Time { get; set; }
        public string OrganSoprano { get; set; }
        public string OrganAlto { get; set; }
        public string OrganTenor { get; set; }
        public string OrganBass { get; set; }
        public string OrganSopranoRelative { get; set; }
        public string OrganAltoRelative { get; set; }
        public string OrganTenorRelative { get; set; }
        public string OrganBassRelative { get; set; }
        public string VoiceSoprano { get; set; }
        public string VoiceAlto { get; set; }
        public string VoiceTenor { get; set; }
        public string VoiceBass { get; set; }
        public string VoiceSopranoRelative { get; set; }
        public string VoiceAltoRelative { get; set; }
        public string VoiceTenorRelative { get; set; }
        public string VoiceBassRelative { get; set; }
    }
}
