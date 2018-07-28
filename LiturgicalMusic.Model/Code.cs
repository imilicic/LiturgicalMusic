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
        public string Soprano { get; set; }
        public string Alto { get; set; }
        public string Tenor { get; set; }
        public string Bass { get; set; }
        public string SopranoRelative { get; set; }
        public string AltoRelative { get; set; }
        public string TenorRelative { get; set; }
        public string BassRelative { get; set; }
    }
}
