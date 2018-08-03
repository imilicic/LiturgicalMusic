using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Model.Common
{
    public interface ICode
    {
        string Voice { get; set; }
        string VoiceRelative { get; set; }
    }
}
