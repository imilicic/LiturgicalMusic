using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Service.Common
{
    public interface IComposerService
    {
        IComposer CreateComposer(IComposer composer);
        List<IComposer> GetAllComposers();
    }
}
