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
using System.Data.Entity;

namespace LiturgicalMusic.Repository
{
    public class StanzaRepository : Repository<StanzaEntity>, IStanzaRepository
    {
        protected IMapper Mapper { get; private set; }

        public StanzaRepository(MusicContext context, IMapper mapper) : base(context)
        {
            this.Mapper = mapper;
        }

        public async Task DeleteAsync(int stanzaId)
        {
            await base.DeleteAsync(await base.GetByIdAsync(stanzaId));
        }

        public async Task<IList<IStanza>> GetBySongAsync(int songId)
        {
            IList<StanzaEntity> stanzas = await base.Get(s => s.SongId.Equals(songId)).ToListAsync();

            return Mapper.Map<IList<IStanza>>(stanzas);
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