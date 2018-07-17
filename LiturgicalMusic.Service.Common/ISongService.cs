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
        ISong CreateSong(ISong song);
        List<ISong> GetAllSongs();
        ISong GetSongById(int songId);
    }
}
