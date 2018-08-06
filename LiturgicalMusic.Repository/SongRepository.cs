using AutoMapper;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiturgicalMusic.Common;
using X.PagedList;
using AutoMapper.QueryableExtensions;

namespace LiturgicalMusic.Repository
{
    public class SongRepository : Repository<SongEntity>, ISongRepository
    {
        protected IMapper Mapper { get; private set; }

        public SongRepository(MusicContext context, IMapper mapper) : base(context)
        {
            this.Mapper = mapper;
        }

        public async Task<ISong> GetByIdAsync(int songId, IOptions options)
        {
            string include = SongHelper.CreateIncludeString(options);
            SongEntity songEntity = await base.GetByIdAsync(songId, include);

            return Mapper.Map<ISong>(songEntity);
        }

        public async Task<IPagedList<ISong>> GetAsync(IFilter filter, IOptions options, string orderBy, bool ascending, int pageNumber, int pageSize)
        {
            IQueryable<SongEntity> query;
            Func<IQueryable<SongEntity>, IOrderedQueryable<SongEntity>> order;
            string include = SongHelper.CreateIncludeString(options);

            switch (orderBy)
            {
                case "title":
                    if (ascending)
                    {
                        order = s => s.OrderBy(se => se.Title);
                    }
                    else
                    {
                        order = s => s.OrderByDescending(se => se.Title);
                    }
                    break;
                case "composer":
                    if (ascending)
                    {
                        order = s => s.OrderBy(se => se.Composer.Surname);
                    }
                    else
                    {
                        order = s => s.OrderByDescending(se => se.Composer.Surname);
                    }
                    break;
                case "arranger":
                    if (ascending)
                    {
                        order = s => s.OrderBy(se => se.Arranger.Surname);
                    }
                    else
                    {
                        order = s => s.OrderByDescending(se => se.Arranger.Surname);
                    }
                    break;
                default:
                    order = s => s.OrderBy(se => se.Title);
                    break;
            }

            if (filter.Title != null)
            {
                query = base.Get(s => s.Title.Contains(filter.Title), order, include);
            }
            else
            {
                query = base.Get(null, order, include);
            }

            IPagedList<SongEntity> songEntities = await query.ToPagedListAsync(pageNumber, pageSize);
            IPagedList<ISong> result = new StaticPagedList<ISong>(Mapper.Map<IEnumerable<ISong>>(songEntities), songEntities.GetMetaData());

            return result;
        }

        public async Task<ISong> InsertAsync(ISong song)
        {
            await SongHelper.CreatePdfAsync(song, Path.GetRandomFileName(), true);

            SongEntity songEntity = Mapper.Map<SongEntity>(song);

            if (song.Arranger != null)
            {
                songEntity.Arranger = null;
                songEntity.ArrangerId = song.Arranger.Id;
            }

            if (song.Composer != null)
            {
                songEntity.Composer = null;
                songEntity.ComposerId = song.Composer.Id;
            }

            return Mapper.Map<ISong>(await base.InsertAsync(songEntity));
        }

        public async Task<ISong> PreviewAsync(ISong song)
        {
            string songFileName = SongHelper.SongFileName(song);

            await SongHelper.CreatePdfAsync(song, SongHelper.Hash(songFileName), false);

            return song;
        }

        public async Task<ISong> UpdateAsync(ISong song)
        {
            IOptions options = new Options
            {
                Arranger = true,
                Composer = true,
                Stanzas = true,
                InstrumentalParts = true
            };
            string include = SongHelper.CreateIncludeString(options);
            SongEntity songDb = await base.GetByIdAsync(song.Id, include);
            SongEntity songEntity = Mapper.Map<SongEntity>(song);

            await SongHelper.UpdatePdfAsync(song, Path.GetRandomFileName(), SongHelper.SongFileName(Mapper.Map<ISong>(songDb)), true);

            songDb.Code = songEntity.Code;
            songDb.OtherInformations = songEntity.OtherInformations;
            songDb.Source = songEntity.Source;
            songDb.Template = songEntity.Template;
            songDb.Title = songEntity.Title;
            songDb.Type = songEntity.Type;

            if (songEntity.Arranger == null)
            {
                songDb.Arranger = null;
            }
            else
            {
                if (songDb.ArrangerId != songEntity.Arranger.Id)
                {
                    songDb.Arranger = null;
                    songDb.ArrangerId = songEntity.Arranger.Id;
                }
            }

            if (songEntity.Composer == null)
            {
                songDb.Composer = null;
            }
            else
            {
                if (songDb.ComposerId != songEntity.Composer.Id)
                {
                    songDb.Composer = null;
                    songDb.ComposerId = songEntity.Composer.Id;
                }
            }

            songDb = await base.UpdateAsync(songDb);

            return Mapper.Map<ISong>(songDb);
        }
    }
}
