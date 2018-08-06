using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository;
using LiturgicalMusic.Common;
using LiturgicalMusic.Model;
using AutoMapper;
using X.PagedList;

namespace LiturgicalMusic.Service
{
    public class SongService : ISongService
    {
        protected UnitOfWork UnitOfWork { get; private set; }
        protected IMapper Mapper { get; private set; }

        public SongService(UnitOfWork unitOfWork, IMapper mapper)
        {
            this.UnitOfWork = unitOfWork;
            this.Mapper = mapper;
        }

        public ISong Create()
        {
            return new Song();
        }

        public async Task<ISong> GetByIdAsync(int songId, IOptions options)
        {
            return await UnitOfWork.SongRepository.GetByIdAsync(songId, options);
        }

        public async Task<IPagedList<ISong>> GetAsync(IFilter filter, IOptions options, string orderBy, bool ascending, int pageNumber, int pageSize)
        {
            return await UnitOfWork.SongRepository.GetAsync(filter, options, orderBy, ascending, pageNumber, pageSize);
        }

        public async Task<ISong> InsertAsync(ISong song)
        {
            return await UnitOfWork.SongRepository.InsertAsync(song);
        }

        public async Task<ISong> PreviewAsync(ISong song)
        {
            return await UnitOfWork.SongRepository.PreviewAsync(song);
        }

        public async Task<ISong> UpdateAsync(ISong song)
        {
            // cud stanzas
            IList<IStanza> stanzas = await UnitOfWork.StanzaRepository.GetBySongAsync(song.Id);

            foreach (IStanza stanza in stanzas)
            {
                if (song.Stanzas.SingleOrDefault(s => s.Id.Equals(stanza.Id)) == null) {
                    await UnitOfWork.StanzaRepository.DeleteAsync(stanza.Id);
                }
            }

            foreach (IStanza stanza in song.Stanzas)
            {
                if (stanzas.SingleOrDefault(s => s.Id.Equals(stanza.Id)) == null)
                {
                    await UnitOfWork.StanzaRepository.InsertAsync(stanza, song.Id);
                }
                else
                {
                    await UnitOfWork.StanzaRepository.UpdateAsync(stanza);
                }
            }

            // cud instrumental parts
            IList<IInstrumentalPart> instrumentalParts = await UnitOfWork.InstrumentalPartRepository.GetBySongAsync(song.Id);

            foreach (IInstrumentalPart part in instrumentalParts)
            {
                if (song.InstrumentalParts.SingleOrDefault(p => p.Id.Equals(part.Id)) == null)
                {
                    await UnitOfWork.InstrumentalPartRepository.DeleteAsync(part.Id);
                }
            }

            foreach (IInstrumentalPart part in song.InstrumentalParts)
            {
                if (instrumentalParts.SingleOrDefault(p => p.Id.Equals(part.Id)) == null)
                {
                    await UnitOfWork.InstrumentalPartRepository.InsertAsync(part, song.Id);
                }
                else
                {
                    await UnitOfWork.InstrumentalPartRepository.UpdateAsync(part);
                }
            }

            // update song
            return await UnitOfWork.SongRepository.UpdateAsync(song);
        }
    }
}
