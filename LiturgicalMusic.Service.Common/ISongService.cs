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
        ISong CreateSong();
        Task<ISong> GetSongByIdAsync(int songId, IOptions options);
        Task<List<ISong>> GetSongsAsync(IFilter filter, IOptions options);
        Task<ISong> InsertSongAsync(ISong song);
        Task<ISong> PreviewSongAsync(ISong song);
        Task<ISong> UpdateSongAsync(ISong song);
    }
}
