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
        protected ISongRepository SongRepository { get; private set; }
        protected IStanzaRepository StanzaRepository { get; private set; }
        protected IInstrumentalPartRepository InstrumentalPartRepository { get; private set; }
        protected IMapper Mapper { get; private set; }

        public SongService(ISongRepository songRepository,
            IStanzaRepository stanzaRepository,
            IInstrumentalPartRepository instrumentalPartRepository,
            IMapper mapper)
        {
            this.SongRepository = songRepository;
            this.StanzaRepository = stanzaRepository;
            this.InstrumentalPartRepository = instrumentalPartRepository;
            this.Mapper = mapper;
        }

        public ISong Create()
        {
            return new Song();
        }

        public async Task<ISong> GetByIdAsync(int songId, IOptions options)
        {
            return await SongRepository.GetByIdAsync(songId, options);
        }

        public async Task<List<ISong>> GetAsync(IFilter filter, IOptions options)
        {
            return await SongRepository.GetAsync(filter, options);
        }

        public async Task<ISong> InsertAsync(ISong song)
        {
            return await SongRepository.InsertAsync(song);
        }

        public async Task<ISong> PreviewAsync(ISong song)
        {
            return await SongRepository.PreviewAsync(song);
        }

        public async Task<ISong> UpdateAsync(ISong song)
        {
            // cud stanzas
            List<IStanza> stanzas = await StanzaRepository.GetBySongAsync(song.Id);

            foreach (IStanza stanza in stanzas)
            {
                if (song.Stanzas.SingleOrDefault(s => s.Id.Equals(stanza.Id)) == null) {
                    await StanzaRepository.DeleteAsync(stanza.Id);
                }
            }

            foreach (IStanza stanza in song.Stanzas)
            {
                if (stanzas.SingleOrDefault(s => s.Id.Equals(stanza.Id)) == null)
                {
                    await StanzaRepository.InsertAsync(stanza, song.Id);
                }
                else
                {
                    await StanzaRepository.UpdateAsync(stanza);
                }
            }

            // cud instrumental parts
            List<IInstrumentalPart> instrumentalParts = await InstrumentalPartRepository.GetBySongAsync(song.Id);

            foreach (IInstrumentalPart part in instrumentalParts)
            {
                if (song.InstrumentalParts.SingleOrDefault(p => p.Id.Equals(part.Id)) == null)
                {
                    await InstrumentalPartRepository.DeleteAsync(part.Id);
                }
            }

            foreach (IInstrumentalPart part in song.InstrumentalParts)
            {
                if (instrumentalParts.SingleOrDefault(p => p.Id.Equals(part.Id)) == null)
                {
                    await InstrumentalPartRepository.InsertAsync(part, song.Id);
                }
                else
                {
                    await InstrumentalPartRepository.UpdateAsync(part);
                }
            }

            // update song
            return await SongRepository.UpdateAsync(song);
        }
    }
}
