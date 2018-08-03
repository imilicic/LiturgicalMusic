using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Model.Common
{
    public interface IStanza
    {
        int Id { get; set; }
        int Number { get; set; }
        string Text { get; set; }
    }
}
