using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Repository.Common;
using AutoMapper;
using LiturgicalMusic.Model.Common;
using System.Linq.Expressions;

namespace LiturgicalMusic.Repository
{
    public class StanzaRepository : GenericRepository<StanzaEntity>, IStanzaRepository, IDisposable
    {
        protected IMapper Mapper { get; private set; }

        public StanzaRepository(IMapper mapper, MusicContext context) : base(context)
        {
            this.Mapper = mapper;
        }

        public async Task DeleteAsync(int stanzaId)
        {
            await base.DeleteAsync(await base.GetByIdAsync(stanzaId));
        }

        public async Task<List<IStanza>> GetBySongAsync(int songId)
        {
            List<StanzaEntity> stanzas = await base.GetAsync(s => s.SongId.Equals(songId));

            return Mapper.Map<List<IStanza>>(stanzas);
        }

        public async Task<IStanza> InsertAsync(IStanza stanza, int songId)
        {
            StanzaEntity stanzaDb = Mapper.Map<StanzaEntity>(stanza);

            stanzaDb.SongId = songId;
            stanzaDb = await base.InsertAsync(stanzaDb);

            return Mapper.Map<IStanza>(stanzaDb);
        }

        public async Task<IStanza> UpdateAsync(IStanza stanza)
        {
            StanzaEntity stanzaDb = await base.GetByIdAsync(stanza.Id);

            Mapper.Map(stanza, stanzaDb);
            stanzaDb = await base.UpdateAsync(stanzaDb);

            return Mapper.Map<IStanza>(stanzaDb);
        }
    }
}
