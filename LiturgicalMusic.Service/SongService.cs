using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository.Common;
using LiturgicalMusic.Common;
using LiturgicalMusic.Model;
using AutoMapper;

namespace LiturgicalMusic.Service
{
    public class SongService : ISongService
    {
        protected ISongRepository Repository { get; private set; }
        protected IMapper Mapper { get; private set; }

        public SongService(ISongRepository repository, IMapper mapper)
        {
            this.Repository = repository;
            this.Mapper = mapper;
        }

        public ISong CreateSong()
        {
            return new Song();
        }

        public async Task<ISong> GetSongByIdAsync(int songId, IOptions options)
        {
            return await Repository.GetSongByIdAsync(songId, options);
        }

        public async Task<List<ISong>> GetSongsAsync(IFilter filter, IOptions options)
        {
            return await Repository.GetSongsAsync(filter, options);
        }

        public async Task<ISong> InsertSongAsync(ISong song)
        {
            return await Repository.InsertSongAsync(song);
        }

        public async Task<ISong> PreviewSongAsync(ISong song)
        {
            return await Repository.PreviewSongAsync(song);
        }

        public async Task<ISong> UpdateSongAsync(ISong song)
        {
            return await Repository.UpdateSongAsync(song);
        }
    }
}
