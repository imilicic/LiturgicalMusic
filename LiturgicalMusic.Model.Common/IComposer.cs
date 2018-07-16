using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Model.Common
{
    public interface IComposer
    {
        int Id { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
    }
}
