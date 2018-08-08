using AutoMapper;
using LiturgicalMusic.Service.Common;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiturgicalMusic.Model.Common;
using System.Threading.Tasks;

namespace LiturgicalMusic.WebAPI.Controllers
{
    [RoutePrefix("api/composers")]
    public class ComposerController : ApiController
    {
        #region Properties

        /// <summary>
        /// Gets or sets the composer service.
        /// </summary>
        /// <value>The composer service.</value>
        protected IComposerService Service { get; private set; }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; private set; }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="ComposerController"/> class.
        /// </summary>
        public ComposerController() { }

        /// <summary>
        /// Initializes new instance of <see cref="ComposerController"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="service">The composer service.</param>
        public ComposerController(IMapper mapper, IComposerService service)
        {
            this.Mapper = mapper;
            this.Service = service;
        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets all composers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> GetAsync()
        {
            IList<IComposer> c = await Service.GetAsync();
            IList<ComposerModel> result = Mapper.Map<IList<ComposerModel>>(c);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Gets composer by ID.
        /// </summary>
        /// <param name="composerId">The composer ID.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> GetByIdAsync(int composerId)
        {
            IComposer composer = await Service.GetByIdAsync(composerId);
            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<ComposerModel>(composer));
        }

        /// <summary>
        /// Inserts and creates a composer.
        /// </summary>
        /// <param name="composer">The composer.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateAsync([FromBody] ComposerModel composer)
        {
            IComposer newComposer = Service.Create();

            Mapper.Map(composer, newComposer);

            IComposer resultComposer = await Service.InsertAsync(newComposer);
            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<ComposerModel>(resultComposer));
        }
        #endregion Methods

        #region Models
        public class ComposerModel
        {
            #region Properties

            /// <summary>
            /// Gets or sets the identifier.
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
        #endregion Models
    }
}
