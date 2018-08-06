using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using LiturgicalMusic.Common;
using X.PagedList;

namespace LiturgicalMusic.WebAPI.Controllers
{
    [RoutePrefix("api/songs")]
    public class SongController : ApiController
    {
        #region Properties

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; private set; }

        /// <summary>
        /// Gets or sets song service.
        /// </summary>
        /// <value>The song service.</value>
        protected ISongService Service { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="SongController"/> class.
        /// </summary>
        public SongController() { }

        /// <summary>
        /// Initializes a new instance of <see cref="SongController"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="service">The song service.</param>
        public SongController(IMapper mapper, ISongService service)
        {
            this.Mapper = mapper;
            this.Service = service;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Creates and inserts a new song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateAsync([FromBody] SongModel song)
        {
            ISong newSong = Service.Create();

            Mapper.Map(song, newSong);

            ISong resultSong = await Service.InsertAsync(newSong);
            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<SongModel>(resultSong));
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
        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> GetAsync(string searchQuery, string orderBy = "title", bool ascending = true, int pageNumber = 1, int pageSize = 20)
        {
            IOptions options = new Options()
            {
                Arranger = true,
                Composer = true
            };
            IFilter filter = new Filter()
            {
                Title = searchQuery
            };

            IPagedList<ISong> result = await Service.GetAsync(filter, options, orderBy, ascending, pageNumber, pageSize);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);

            response.Headers.Add("Song-count", result.TotalItemCount.ToString());

            return response;
        }

        /// <summary>
        /// Gets song by ID which contains certain options.
        /// </summary>
        /// <param name="songId">The song ID.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> GetByIdAsync(int songId)
        {
            IOptions options = new Options()
            {
                Arranger = true,
                Composer = true,
                Stanzas = true,
                InstrumentalParts = true
            };

            ISong s = await Service.GetByIdAsync(songId, options);
            SongModel result = Mapper.Map<SongModel>(s);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Makes a preview of a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("preview")]
        public async Task<HttpResponseMessage> PreviewAsync([FromBody] SongModel song)
        {
            ISong s = await Service.PreviewAsync(Mapper.Map<ISong>(song));
            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<SongModel>(s));
        }

        /// <summary>
        /// Updates a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("create")]
        public async Task<HttpResponseMessage> UpdateAsync([FromBody] SongModel song)
        {
            ISong resultSong = await Service.UpdateAsync(Mapper.Map<ISong>(song));

            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<SongModel>(resultSong));
        }
        #endregion Methods

        #region Models
        public class ComposerModel
        {
            #region Properties

            /// <summary>
            ///  Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets composer name.
            /// </summary>
            /// <value>The composer name.</value>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets composer surname.
            /// </summary>
            /// <value>The composer surname.</value>
            public string Surname { get; set; }
            #endregion Properties
        }

        public class InstrumentalPartModel
        {
            #region Properties

            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets instrumental part position in song.
            /// </summary>
            /// <value><c>prelude</c>, <c>interlude</c> or <c>coda</c></value>
            public string Position { get; set; }

            /// <summary>
            /// Gets or sets type of instrumental part.
            /// </summary>
            /// <value><c>hymn</c> or <c>psalm</c></value>
            public string Type { get; set; }

            /// <summary>
            /// Gets or sets code of instrumental part.
            /// </summary>
            /// <value>The code.</value>
            public string Code { get; set; }

            /// <summary>
            /// Gets or sets template of instrumental part.
            /// </summary>
            /// <value>The template of instrumental part.</value>
            public IList<bool> Template { get; set; }
            #endregion Properties
        }

        public class SongModel
        {
            #region Properties
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets song title.
            /// </summary>
            /// <value>The song title.</value>
            public string Title { get; set; }

            /// <summary>
            /// Gets or sets song template.
            /// </summary>
            /// <value>The song template.</value>
            public IList<bool> Template { get; set; }

            /// <summary>
            /// Gets or sets the song type.
            /// </summary>
            /// <value><c>hymn</c> or <c>psalm</c></value>
            public string Type { get; set; }

            /// <summary>
            /// Gets or sets code.
            /// </summary>
            /// <value>The code.</value>
            public string Code { get; set; }

            /// <summary>
            /// Gets or sets source of a song.
            /// </summary>
            /// <value>The source.</value>
            public string Source { get; set; }

            /// <summary>
            /// Gets or sets other informations of a song.
            /// </summary>
            /// <value>The other informations.</value>
            public string OtherInformations { get; set; }

            /// <summary>
            /// Gets or sets stanzas.
            /// </summary>
            /// <value>The stanzas.</value>
            public IList<StanzaModel> Stanzas { get; set; }

            /// <summary>
            /// Gets or sets the composer.
            /// </summary>
            /// <value>The composer.</value>
            public ComposerModel Composer { get; set; }

            /// <summary>
            /// Gets or sets the arranger.
            /// </summary>
            /// <value>The arranger.</value>
            public ComposerModel Arranger { get; set; }

            /// <summary>
            /// Gets or sets instrumental parts.
            /// </summary>
            /// <value>The instrumental parts.</value>
            public IList<InstrumentalPartModel> InstrumentalParts { get; set; }

            /// <summary>
            /// Gets or sets theme categories.
            /// </summary>
            /// <value>The theme categories.</value>
            public IList<int> ThemeCategories { get; set; }

            /// <summary>
            /// Gets or sets liturgy categories.
            /// </summary>
            /// <value>The liturgy categories.</value>
            public IList<int> LiturgyCategories { get; set; }
            #endregion Properties
        }

        public class StanzaModel
        {
            #region Properties

            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets stanza number.
            /// </summary>
            /// <value>The number.</value>
            public int Number { get; set; }

            /// <summary>
            /// Gets or sets stanza text.
            /// </summary>
            /// <value>The text.</value>
            public string Text { get; set; }
            #endregion Properties
        }
        #endregion Models
    }
}