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
        IComposer Create();
        Task<IList<IComposer>> GetAsync();
        Task<IComposer> GetByIdAsync(int composerId);
        Task<IComposer> InsertAsync(IComposer composer);
    }
}
