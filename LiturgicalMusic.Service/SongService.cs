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

        public ISong Create()
        {
            return new Song();
        }

        public async Task<ISong> GetByIdAsync(int songId, IOptions options)
        {
            return await Repository.GetByIdAsync(songId, options);
        }

        public async Task<List<ISong>> GetAsync(IFilter filter, IOptions options)
        {
            return await Repository.GetAsync(filter, options);
        }

        public async Task<ISong> InsertAsync(ISong song)
        {
            return await Repository.InsertAsync(song);
        }

        public async Task<ISong> PreviewAsync(ISong song)
        {
            return await Repository.PreviewAsync(song);
        }

        public async Task<ISong> UpdateAsync(ISong song)
        {
            return await Repository.UpdateAsync(song);
        }
    }
}
