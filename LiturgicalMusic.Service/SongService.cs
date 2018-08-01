using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository.Common;
using LiturgicalMusic.Common;

namespace LiturgicalMusic.Service
{
    public class SongService : ISongService
    {
        protected ISongRepository Repository { get; private set; }

        public SongService(ISongRepository repository)
        {
            this.Repository = repository;
        }

        public async Task<ISong> CreateSongAsync(ISong song)
        {
            ISong s = await Repository.CreateSongAsync(song);
            return s;
        }

        public async Task<ISong> GetSongByIdAsync(int songId)
        {
            ISong s = await Repository.GetSongByIdAsync(songId);
            return s;
        }

        public async Task<ISong> PreviewSongAsync(ISong song)
        {
            ISong s = await Repository.PreviewSongAsync(song);
            return s;
        }

        public async Task<List<ISong>> SearchSongsAsync(IFilter filter)
        {
            List<ISong> s = await Repository.SearchSongsAsync(filter);
            return s;
        }

        public async Task<ISong> UpdateSongAsync(ISong song)
        {
            ISong s = await Repository.UpdateSongAsync(song);
            return s;
        }
    }
}
