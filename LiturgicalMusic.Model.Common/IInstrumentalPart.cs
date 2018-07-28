using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Model.Common
{
    public interface IInstrumentalPart
    {
        int Id { get; set; }
        List<bool> Template { get; set; }
        string Position { get; set; }
        string Type { get; set; }
        string Code { get; set; }
    }
}
