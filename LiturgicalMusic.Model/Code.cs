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
        public string Voice { get; set; }
        public string VoiceRelative { get; set; }
    }
}
