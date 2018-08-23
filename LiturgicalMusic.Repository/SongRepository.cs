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
    public class SongRepository : ISongRepository
    {
        #region Properties
        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; private set; }

        /// <summary>
        /// Gets or sets the generic repository.
        /// </summary>
        /// <value>The repository.</value>
        protected IRepository<SongEntity> Repository { get; private set; }

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
        public SongRepository(IMapper mapper, IRepository<SongEntity> repository)
        {
            this.Mapper = mapper;
            this.Repository = repository;
            this.UnitOfWork = Repository.CreateUnitOfWork();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Deletes a song.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <returns></returns>
        public async Task DeleteAsync(int songId)
        {
            IOptions options = new Options()
            {
                Stanzas = true,
                InstrumentalParts = true
            };
            SongEntity songEntity = await Repository.GetById(songId, SongHelper.CreateIncludeString(options));
            SongHelper.DeletePdf(Mapper.Map<ISong>(songEntity));

            for (int i = songEntity.Stanzas.Count - 1; i >= 0; i--)
            {
                StanzaEntity stanza = songEntity.Stanzas.ElementAt(i);

                await UnitOfWork.DeleteAsync<StanzaEntity>(stanza.Id);
            }

            for (int i = songEntity.InstrumentalParts.Count - 1; i >= 0; i--)
            {
                InstrumentalPartEntity part = songEntity.InstrumentalParts.ElementAt(i);

                await UnitOfWork.DeleteAsync<InstrumentalPartEntity>(part.Id);
            }

            await UnitOfWork.DeleteAsync<SongEntity>(songId);
            await UnitOfWork.CommitAsync();
        }

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
            string include = SongHelper.CreateIncludeString(options);
            IQueryable<SongEntity> query = Repository.Get(include);
            IOrderedEnumerable<SongEntity> orderedQuery;
            Func<SongEntity, string> order;

            // filtering
            if (filter.Title != null)
            {
                query = query.Where(s => s.Title.Contains(filter.Title));
            }

            // sorting
            switch (sortingOptions.SortBy)
            {
                case "title":
                    order = s => s.Title;
                    break;
                case "composer":
                    order = s => s.Composer.Surname;
                    break;
                case "arranger":
                    order = s => s.Arranger.Surname;
                    break;
                default:
                    order = s => s.Title;
                    break;
            }

            if (sortingOptions.SortAscending)
            {
                orderedQuery = query.OrderBy(order);
            }
            else
            {
                orderedQuery = query.OrderByDescending(order);
            }

            // paging
            IPagedList<SongEntity> songEntities = await orderedQuery.ToPagedListAsync(pageOptions.PageNumber, pageOptions.PageSize);
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
            SongEntity songEntity = await Repository.GetById(songId, include);

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

            foreach (int liturgy in song.LiturgyCategories)
            {
                SongLiturgyEntity songLiturgy = new SongLiturgyEntity();
                songLiturgy.SongId = songEntity.Id;
                songLiturgy.LiturgyId = liturgy;
                await UnitOfWork.InsertAsync<SongLiturgyEntity>(songLiturgy);
            }

            foreach (int theme in song.ThemeCategories)
            {
                SongThemeEntity songTheme = new SongThemeEntity();
                songTheme.SongId = songEntity.Id;
                songTheme.ThemeId = theme;
                await UnitOfWork.InsertAsync<SongThemeEntity>(songTheme);
            }

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

            UnitOfWork.ClearLocal<SongEntity>();
            UnitOfWork.ClearLocal<StanzaEntity>();
            UnitOfWork.ClearLocal<InstrumentalPartEntity>();
            UnitOfWork.ClearLocal<ComposerEntity>();

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
            SongEntity songDb = await Repository.GetById(song.Id, include);
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

            UnitOfWork.ClearLocal<SongEntity>();
            UnitOfWork.ClearLocal<StanzaEntity>();
            UnitOfWork.ClearLocal<InstrumentalPartEntity>();
            UnitOfWork.ClearLocal<ComposerEntity>();

            return Mapper.Map<ISong>(songDb);
        }
        #endregion Methods
    }
}
