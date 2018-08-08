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

namespace LiturgicalMusic.Repository
{
    public class SongRepository : Repository<SongEntity>, ISongRepository
    {
        #region Properties
        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; private set; }

        /// <summary>
        /// Gets or sets the unit of work.
        /// </summary>
        /// <value>The unit of work.</value>
        protected IUnitOfWork UnitOfWork { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="SongRepository"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="uowFactory">The factory for unit of work.</param>
        public SongRepository(IMapper mapper, MusicContext dbContext, IUnitOfWorkFactory uowFactory): base(dbContext, uowFactory)
        {
            this.Mapper = mapper;
            this.UnitOfWork = CreateUnitOfWork();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Gets all songs filtered, ordered, using pages
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <param name="orderBy">The string represeting how to order songs.</param>
        /// <param name="ascending">Whether to order ascending or descending.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns></returns>
        public async Task<IPagedList<ISong>> GetAsync(IFilter filter, IOptions options, ISorting sortingOptions, IPaging pageOptions)
        {
            IQueryable<SongEntity> query;
            Func<IQueryable<SongEntity>, IOrderedQueryable<SongEntity>> order;
            string include = SongHelper.CreateIncludeString(options);

            switch (sortingOptions.SortBy)
            {
                case "title":
                    if (sortingOptions.SortAscending)
                    {
                        order = s => s.OrderBy(se => se.Title);
                    }
                    else
                    {
                        order = s => s.OrderByDescending(se => se.Title);
                    }
                    break;
                case "composer":
                    if (sortingOptions.SortAscending)
                    {
                        order = s => s.OrderBy(se => se.Composer.Surname);
                    }
                    else
                    {
                        order = s => s.OrderByDescending(se => se.Composer.Surname);
                    }
                    break;
                case "arranger":
                    if (sortingOptions.SortAscending)
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

            IPagedList<SongEntity> songEntities = await query.ToPagedListAsync(pageOptions.PageNumber, pageOptions.PageSize);
            IPagedList<ISong> result = new StaticPagedList<ISong>(Mapper.Map<IEnumerable<ISong>>(songEntities), songEntities.GetMetaData());

            return result;
        }

        /// <summary>
        /// Gets song by ID which contains certain options.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public async Task<ISong> GetByIdAsync(int songId, IOptions options)
        {
            string include = SongHelper.CreateIncludeString(options);
            SongEntity songEntity = await base.GetByIdAsync(songId, include);

            return Mapper.Map<ISong>(songEntity);
        }

        /// <summary>
        /// Inserts a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
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

            songEntity = await UnitOfWork.InsertAsync<SongEntity>(songEntity);
            await UnitOfWork.CommitAsync();

            return Mapper.Map<ISong>(songEntity);
        }

        /// <summary>
        /// Makes a preview of a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        public async Task<ISong> PreviewAsync(ISong song)
        {
            string songFileName = SongHelper.SongFileName(song);

            await SongHelper.CreatePdfAsync(song, SongHelper.Hash(songFileName), false);

            return song;
        }

        /// <summary>
        /// Updates a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
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

            // updating stanzas
            for (int i = songDb.Stanzas.Count - 1; i >= 0; i--)
            {
                StanzaEntity stanzaDb = songDb.Stanzas.ElementAt(i);

                if (songEntity.Stanzas.SingleOrDefault(s => s.Id.Equals(stanzaDb.Id)) == null)
                {
                    await UnitOfWork.DeleteAsync<StanzaEntity>(stanzaDb);
                }
            }

            foreach (StanzaEntity stanza in songEntity.Stanzas)
            {
                StanzaEntity stanzaDb = songDb.Stanzas.SingleOrDefault(s => s.Id.Equals(stanza.Id));

                if (stanzaDb == null)
                {
                    stanza.SongId = song.Id;
                    await UnitOfWork.InsertAsync<StanzaEntity>(stanza);
                }
                else
                {
                    stanzaDb.Number = stanza.Number;
                    stanzaDb.Text = stanza.Text;
                    await UnitOfWork.UpdateAsync<StanzaEntity>(stanzaDb);
                }
            }

            // updating instrumental parts
            for (int i = songDb.InstrumentalParts.Count - 1; i >= 0; i--)
            {
                InstrumentalPartEntity partDb = songDb.InstrumentalParts.ElementAt(i);

                if (songEntity.InstrumentalParts.SingleOrDefault(p => p.Id.Equals(partDb.Id)) == null)
                {
                    await UnitOfWork.DeleteAsync<InstrumentalPartEntity>(partDb);
                }
            }

            foreach (InstrumentalPartEntity part in songEntity.InstrumentalParts)
            {
                InstrumentalPartEntity partDb = songDb.InstrumentalParts.SingleOrDefault(p => p.Id.Equals(part.Id));

                if (partDb == null)
                {
                    part.SongId = song.Id;
                    await UnitOfWork.InsertAsync<InstrumentalPartEntity>(part);
                }
                else
                {
                    partDb.Code = part.Code;
                    partDb.Position = part.Position;
                    partDb.Template = part.Template;
                    partDb.Type = part.Type;
                    await UnitOfWork.UpdateAsync<InstrumentalPartEntity>(partDb);
                }
            }

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

            songDb = await UnitOfWork.UpdateAsync(songDb);
            await UnitOfWork.CommitAsync();

            return Mapper.Map<ISong>(songDb);
        }
        #endregion Methods
    }
}
