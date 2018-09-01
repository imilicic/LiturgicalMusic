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
            SongEntity songEntity = await Repository.GetById(songId);
            SongHelper.DeletePdf(Mapper.Map<ISong>(songEntity));

            using (IUnitOfWork unitOfWork = Repository.CreateUnitOfWork())
            {
                await unitOfWork.DeleteAsync<SongEntity>(songId);
                await unitOfWork.CommitAsync();
            }
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
        public async Task<IPagedList<ISong>> GetAsync(IFilter filter, string[] options, ISorting sortingOptions, IPaging pageOptions)
        {
            IQueryable<SongEntity> query = Repository.Get(options);
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
        public async Task<ISong> GetByIdAsync(int songId, string[] options)
        {
            SongEntity songEntity = await Repository.GetById(songId, options);

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

            using (IUnitOfWork unitOfWork = Repository.CreateUnitOfWork())
            {
                foreach (int liturgy in song.LiturgyCategories)
                {
                    SongLiturgyEntity songLiturgy = new SongLiturgyEntity();
                    songLiturgy.SongId = songEntity.Id;
                    songLiturgy.LiturgyId = liturgy;
                    await unitOfWork.InsertAsync<SongLiturgyEntity>(songLiturgy);
                }

                foreach (int theme in song.ThemeCategories)
                {
                    SongThemeEntity songTheme = new SongThemeEntity();
                    songTheme.SongId = songEntity.Id;
                    songTheme.ThemeId = theme;
                    await unitOfWork.InsertAsync<SongThemeEntity>(songTheme);
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

                songEntity = await unitOfWork.InsertAsync<SongEntity>(songEntity);
                await unitOfWork.CommitAsync();
            }

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
            string[] options = new String[4] { "Stanzas", "InstrumentalParts", "LiturgyCategories", "ThemeCategories" };
            SongEntity songEntity = Mapper.Map<SongEntity>(song);
            SongEntity songDb = await Repository.GetById(song.Id, options);

            await SongHelper.UpdatePdfAsync(song, Path.GetRandomFileName(), SongHelper.SongFileName(Mapper.Map<ISong>(songDb)), true);

            using (IUnitOfWork unitOfWork = Repository.CreateUnitOfWork())
            {
                // updating stanzas
                for (int i = songDb.Stanzas.Count - 1; i >= 0; i--)
                {
                    StanzaEntity stanzaDb = songDb.Stanzas.ElementAt(i);
                    StanzaEntity foundStanza = songEntity.Stanzas.SingleOrDefault(s => s.Id.Equals(stanzaDb.Id));

                    if (object.Equals(foundStanza, default(StanzaEntity)))
                    {
                        await unitOfWork.DeleteAsync<StanzaEntity>(stanzaDb);
                    }
                }

                foreach (StanzaEntity stanza in songEntity.Stanzas)
                {
                    if (stanza.Id == 0)
                    {
                        stanza.SongId = song.Id;
                        await unitOfWork.InsertAsync<StanzaEntity>(stanza);
                    }
                    else
                    {
                        StanzaEntity stanzaDb = songDb.Stanzas.SingleOrDefault(s => s.Id.Equals(stanza.Id));

                        stanzaDb.Number = stanza.Number;
                        stanzaDb.Text = stanza.Text;
                        await unitOfWork.UpdateAsync<StanzaEntity>(stanzaDb);
                    }
                }

                // updating instrumental parts
                for (int i = songDb.InstrumentalParts.Count - 1; i >= 0; i--)
                {
                    InstrumentalPartEntity partDb = songDb.InstrumentalParts.ElementAt(i);
                    InstrumentalPartEntity foundPart = songEntity.InstrumentalParts.SingleOrDefault(p => p.Id.Equals(partDb.Id));

                    if (object.Equals(foundPart, default(InstrumentalPartEntity)))
                    {
                        await unitOfWork.DeleteAsync<InstrumentalPartEntity>(partDb);
                    }
                }

                foreach (InstrumentalPartEntity part in songEntity.InstrumentalParts)
                {
                    if (part.Id == 0)
                    {
                        part.SongId = song.Id;
                        await unitOfWork.InsertAsync<InstrumentalPartEntity>(part);
                    }
                    else
                    {
                        InstrumentalPartEntity partDb = songDb.InstrumentalParts.SingleOrDefault(p => p.Id.Equals(part.Id));

                        partDb.Code = part.Code;
                        partDb.Position = part.Position;
                        partDb.Template = part.Template;
                        partDb.Type = part.Type;
                        await unitOfWork.UpdateAsync<InstrumentalPartEntity>(partDb);
                    }
                }

                // updating liturgy categories
                for (int i = songDb.LiturgyCategories.Count - 1; i >= 0; i--)
                {
                    SongLiturgyEntity songLiturgyDb = songDb.LiturgyCategories.ElementAt(i);
                    int foundLiturgy = song.LiturgyCategories.SingleOrDefault(l => l.Equals(songLiturgyDb.LiturgyId));

                    if (object.Equals(foundLiturgy, default(int)))
                    {
                        await unitOfWork.DeleteAsync<SongLiturgyEntity>(songLiturgyDb);
                    }
                }

                foreach (int liturgy in song.LiturgyCategories)
                {
                    SongLiturgyEntity songLiturgyDb = songDb.LiturgyCategories.SingleOrDefault(l => l.LiturgyId.Equals(liturgy));

                    if (songLiturgyDb == null)
                    {
                        SongLiturgyEntity songLiturgy = new SongLiturgyEntity
                        {
                            LiturgyId = liturgy,
                            SongId = song.Id
                        };

                        await unitOfWork.InsertAsync<SongLiturgyEntity>(songLiturgy);
                    }
                }

                // updating theme categories
                for (int i = songDb.ThemeCategories.Count - 1; i >= 0; i--)
                {
                    SongThemeEntity songThemeDb = songDb.ThemeCategories.ElementAt(i);
                    int foundTheme = song.LiturgyCategories.SingleOrDefault(t => t.Equals(songThemeDb.ThemeId));

                    if (object.Equals(foundTheme, default(int)))
                    {
                        await unitOfWork.DeleteAsync<SongThemeEntity>(songThemeDb);
                    }
                }

                foreach (int theme in song.ThemeCategories)
                {
                    SongThemeEntity songThemeDb = songDb.ThemeCategories.SingleOrDefault(l => l.ThemeId.Equals(theme));

                    if (songThemeDb == null)
                    {
                        SongThemeEntity songTheme = new SongThemeEntity
                        {
                            ThemeId = theme,
                            SongId = song.Id
                        };

                        await unitOfWork.InsertAsync<SongThemeEntity>(songTheme);
                    }
                }

                songDb.ArrangerId = null;
                songDb.Code = songEntity.Code;
                songDb.ComposerId = null;
                songDb.OtherInformations = songEntity.OtherInformations;
                songDb.Source = songEntity.Source;
                songDb.Template = songEntity.Template;
                songDb.Title = songEntity.Title;
                songDb.Type = songEntity.Type;

                if (songEntity.Arranger != null)
                {
                    songDb.ArrangerId = songEntity.Arranger.Id;
                }

                if (songEntity.Composer != null)
                {
                    songDb.ComposerId = songEntity.Composer.Id;
                }

                songDb = await unitOfWork.UpdateAsync(songDb);
                await unitOfWork.CommitAsync();
            }

            return Mapper.Map<ISong>(songDb);
        }
        #endregion Methods
    }
}
