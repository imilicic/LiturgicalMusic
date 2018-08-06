using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Model.Common;
using AutoMapper;
using LiturgicalMusic.Repository.Common;
using System.Data.Entity;

namespace LiturgicalMusic.Repository
{
    public class InstrumentalPartRepository : Repository<InstrumentalPartEntity>, IInstrumentalPartRepository
    {
        protected IMapper Mapper { get; private set; }

        public InstrumentalPartRepository(MusicContext context, IMapper mapper) : base(context)
        {
            this.Mapper = mapper;
        }

        public async Task DeleteAsync(int instrumentalPartId)
        {
            await base.DeleteAsync(await base.GetByIdAsync(instrumentalPartId));
        }

        public async Task<IList<IInstrumentalPart>> GetBySongAsync(int songId)
        {
            IList<InstrumentalPartEntity> instrumentalParts = await base.Get(p => p.SongId.Equals(songId)).ToListAsync();

            return Mapper.Map<IList<IInstrumentalPart>>(instrumentalParts);
        }

        public async Task<IInstrumentalPart> InsertAsync(IInstrumentalPart instrumentalPart, int songId)
        {
            InstrumentalPartEntity instrumentalPartDb = Mapper.Map<InstrumentalPartEntity>(instrumentalPart);

            instrumentalPartDb.SongId = songId;
            instrumentalPartDb = await base.InsertAsync(instrumentalPartDb);

            return Mapper.Map<IInstrumentalPart>(instrumentalPartDb);
        }

        public async Task<IInstrumentalPart> UpdateAsync(IInstrumentalPart instrumentalPart)
        {
            InstrumentalPartEntity instrumentalPartDb = await base.GetByIdAsync(instrumentalPart.Id);

            Mapper.Map(instrumentalPart, instrumentalPartDb);
            instrumentalPartDb = await base.UpdateAsync(instrumentalPartDb);

            return Mapper.Map<IInstrumentalPart>(instrumentalPartDb);
        }
    }
}
