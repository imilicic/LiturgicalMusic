using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Common;

namespace LiturgicalMusic.Service.Common
{
    public interface ISongService
    {
        ISong Create();
        Task<ISong> GetByIdAsync(int songId, IOptions options);
        Task<List<ISong>> GetAsync(IFilter filter, IOptions options);
        Task<ISong> InsertAsync(ISong song);
        Task<ISong> PreviewAsync(ISong song);
        Task<ISong> UpdateAsync(ISong song);
    }
}
