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
    public class ComposerRepository : IComposerRepository
    {
        protected IMapper Mapper { get; private set; }

        public ComposerRepository(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public async Task<IComposer> InsertComposerAsync(IComposer composer)
        {
            ComposerEntity composerEntity;

            using (var db = new MusicContext())
            {
                composerEntity = Mapper.Map<ComposerEntity>(composer);
                db.Composers.Add(composerEntity);
                await db.SaveChangesAsync();
            }
            return Mapper.Map<IComposer>(composerEntity);
        }
        public async Task<List<IComposer>> GetComposersAsync()
        {
            List<ComposerEntity> composerEntities;

            using (var db = new MusicContext())
            {
                composerEntities = await db.Composers.OrderBy(c => c.Surname).ToListAsync();
            }
            return Mapper.Map<List<IComposer>>(composerEntities);
        }
    }
}
