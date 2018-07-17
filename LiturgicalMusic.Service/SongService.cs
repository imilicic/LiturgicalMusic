using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository.Common;

namespace LiturgicalMusic.Service
{
    public class SongService : ISongService
    {
        protected ISongRepository Repository { get; private set; }

        public SongService(ISongRepository repository)
        {
            this.Repository = repository;
        }

        public ISong CreateSong(ISong song)
        {
            return Repository.CreateSong(song);
        }

        public List<ISong> GetAllSongs()
        {
            return Repository.GetAllSongs();
        }

        public ISong GetSongById(int songId)
        {
            return Repository.GetSongById(songId);
        }
    }
}
