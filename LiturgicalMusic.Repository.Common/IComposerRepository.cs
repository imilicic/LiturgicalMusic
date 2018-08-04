using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Repository.Common
{
    public interface IComposerRepository
    {
        Task<IComposer> InsertAsync(IComposer composer);
        Task<List<IComposer>> GetAsync();
        Task<IComposer> GetByIdAsync(int composerId);
    }
}
