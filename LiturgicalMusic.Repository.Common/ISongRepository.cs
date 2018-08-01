using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Common;

namespace LiturgicalMusic.Repository.Common
{
    public interface ISongRepository
    {
        Task<ISong> CreateSongAsync(ISong song);
        Task<ISong> GetSongByIdAsync(int songId);
        Task<ISong> PreviewSongAsync(ISong song);
        Task<List<ISong>> SearchSongsAsync(IFilter filter);
        Task<ISong> UpdateSongAsync(ISong song);
    }
}
