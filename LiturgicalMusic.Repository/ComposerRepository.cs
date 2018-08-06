using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Repository.Common;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Model.Common;
using AutoMapper;
using System.Data.Entity;

namespace LiturgicalMusic.Repository
{
    public class ComposerRepository : Repository<ComposerEntity>, IComposerRepository
    {
        protected IMapper Mapper { get; private set; }

        public ComposerRepository(MusicContext context, IMapper mapper) : base(context)
        {
            this.Mapper = mapper;
        }

        public async Task<IList<IComposer>> GetAsync()
        {
            return Mapper.Map<IList<IComposer>>(await base.Get(null, cs => cs.OrderBy(c => c.Surname)).ToListAsync());
        }

        public async Task<IComposer> GetByIdAsync(int composerId)
        {
            ComposerEntity composer = await base.GetByIdAsync(composerId);
            return Mapper.Map<IComposer>(composer);
        }

        public async Task<IComposer> InsertAsync(IComposer composer)
        {
            ComposerEntity composerEntity = Mapper.Map<ComposerEntity>(composer);

            return Mapper.Map<IComposer>(await base.InsertAsync(composerEntity));
        }
    }
}
