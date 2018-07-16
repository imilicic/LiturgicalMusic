using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Repository.Common;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Model.Common;
using AutoMapper;

namespace LiturgicalMusic.Repository
{
    public class ComposerRepository : IComposerRepository
    {
        protected IMapper Mapper { get; private set; }

        public ComposerRepository(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public IComposer CreateComposer(IComposer composer)
        {
            ComposerEntity composerEntity;

            using (var db = new MusicContext())
            {
                composerEntity = Mapper.Map<ComposerEntity>(composer);
                db.Composers.Add(composerEntity);
                db.SaveChanges();
            }
            return Mapper.Map<IComposer>(composerEntity);
        }
        public List<IComposer> GetAllComposers()
        {
            List<ComposerEntity> composerEntities;

            using (var db = new MusicContext())
            {
                composerEntities = db.Composers.ToList();
            }
            return Mapper.Map<List<IComposer>>(composerEntities);
        }
    }
}
