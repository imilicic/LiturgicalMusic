using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Service.Common
{
    public interface ISongService
    {
        Task<ISong> CreateSongAsync(ISong song);
        Task<List<ISong>> GetAllSongsAsync();
        Task<ISong> GetSongByIdAsync(int songId);
        Task<ISong> PreviewSongAsync(ISong song);
    }
}
