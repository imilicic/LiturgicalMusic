using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository.Common;
using AutoMapper;

namespace LiturgicalMusic.Repository
{
    public class SongRepository : ISongRepository
    {
        protected IMapper Mapper { get; private set; }

        public SongRepository(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public ISong CreateSong(ISong song)
        {
            SongEntity songEntity;

            using (var db = new MusicContext())
            {
                songEntity = Mapper.Map<SongEntity>(song);

                if (songEntity.Composer != null)
                {
                    ComposerEntity composerEntity = db.Composers.SingleOrDefault(c => c.Id.Equals(songEntity.Composer.Id));

                    songEntity.Composer = composerEntity;
                }

                if (songEntity.Arranger != null)
                {
                    ComposerEntity arrangerEntity = db.Composers.SingleOrDefault(c => c.Id.Equals(songEntity.Arranger.Id));

                    songEntity.Arranger = arrangerEntity;
                }

                if (songEntity.LiturgyCategories.Count() > 0)
                {
                    List<int> liturgyIds = song.LiturgyCategories;
                    songEntity.LiturgyCategories = db.LiturgyCategories.Where(l => liturgyIds.Contains(l.Id)).ToList();
                }

                if (songEntity.ThemeCategories.Count() > 0)
                {
                    List<int> themeIds = song.ThemeCategories;
                    songEntity.ThemeCategories = db.ThemeCategories.Where(l => themeIds.Contains(l.Id)).ToList();
                }

                db.Songs.Add(songEntity);
                db.SaveChanges();
            }
            return Mapper.Map<ISong>(songEntity);
        }

        public List<ISong> GetAllSongs()
        {
            List<SongEntity> songEntities;

            using (var db = new MusicContext())
            {
                songEntities = db.Songs
                    .Include("Composer")
                    .Include("Arranger")
                    .Include("ThemeCategories")
                    .Include("LiturgyCategories")
                    .Include("InstrumentalParts")
                    .Include("Stanzas")
                    .ToList();
            }

            return Mapper.Map<List<ISong>>(songEntities);
        }

        public ISong GetSongById(int songId)
        {
            SongEntity songEntity;

            using (var db = new MusicContext())
            {
                songEntity = db.Songs
                    .Include("Composer")
                    .Include("Arranger")
                    .Include("ThemeCategories")
                    .Include("LiturgyCategories")
                    .Include("InstrumentalParts")
                    .Include("Stanzas")
                    .SingleOrDefault(s => s.Id.Equals(songId));
            }

            return Mapper.Map<ISong>(songEntity);
        }
    }
}
