using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Model.Common
{
    public interface ICode
    {
        string Soprano { get; set; }
        string Alto { get; set; }
        string Tenor { get; set; }
        string Bass { get; set; }
        string SopranoRelative { get; set; }
        string AltoRelative { get; set; }
        string TenorRelative { get; set; }
        string BassRelative { get; set; }
    }
}
