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
        protected IMapper Mapper { get; private set; }
        protected ISongService Service { get; private set; }

        public SongController() { }

        public SongController(IMapper mapper, ISongService service)
        {
            this.Mapper = mapper;
            this.Service = service;
        }

        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateAsync([FromBody] SongModel song)
        {
            ISong newSong = Service.Create();

            Mapper.Map(song, newSong);

            ISong resultSong = await Service.InsertAsync(newSong);
            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<SongModel>(resultSong));
        }

        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> GetAsync([FromUri] FilterModel filter, string orderBy = "title", bool ascending = true, int pageNumber = 1, int pageSize = 20)
        {
            IOptions options = new Options()
            {
                Arranger = true,
                Composer = true
            };

            IPagedList<ISong> result = await Service.GetAsync(Mapper.Map<IFilter>(filter), options, orderBy, ascending, pageNumber, pageSize);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);

            response.Headers.Add("Song-count", result.TotalItemCount.ToString());

            return response;
        }

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

        [HttpPost]
        [Route("preview")]
        public async Task<HttpResponseMessage> PreviewAsync([FromBody] SongModel song)
        {
            ISong s = await Service.PreviewAsync(Mapper.Map<ISong>(song));
            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<SongModel>(s));
        }

        [HttpPut]
        [Route("create")]
        public async Task<HttpResponseMessage> UpdateAsync([FromBody] SongModel song)
        {
            ISong resultSong = await Service.UpdateAsync(Mapper.Map<ISong>(song));

            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<SongModel>(resultSong));
        }

        public class ComposerModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
        }

        public class FilterModel
        {
            public string Title { get; set; }
        }

        public class InstrumentalPartModel
        {
            public int Id { get; set; }
            public string Position { get; set; }
            public string Type { get; set; }
            public string Code { get; set; }
            public IList<bool> Template { get; set; }
        }

        public class SongModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public IList<bool> Template { get; set; }
            public string Type { get; set; }
            public string Code { get; set; }
            public string Source { get; set; }
            public string OtherInformations { get; set; }
            public IList<StanzaModel> Stanzas { get; set; }
            public ComposerModel Composer { get; set; }
            public ComposerModel Arranger { get; set; }
            public IList<InstrumentalPartModel> InstrumentalParts { get; set; }
            public IList<int> ThemeCategories { get; set; }
            public IList<int> LiturgyCategories { get; set; }
        }

        public class StanzaModel
        {
            public int Id { get; set; }
            public int Number { get; set; }
            public string Text { get; set; }
        }
    }
}