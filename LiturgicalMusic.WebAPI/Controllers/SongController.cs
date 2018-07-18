using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using System.Net.Http;
using System.Net;

namespace LiturgicalMusic.WebAPI.Controllers
{
    [RoutePrefix("api/songs")]
    public class SongController : ApiController
    {
        protected IMapper Mapper { get; private set; }
        protected ISongService Service { get; private set; }
        public SongController()
        {
        }

        public SongController(IMapper mapper, ISongService service)
        {
            this.Mapper = mapper;
            this.Service = service;
        }

        [HttpGet]
        [Route("search")]
        public HttpResponseMessage GetAllSongs()
        {
            List<SongModel> result = Mapper.Map<List<SongModel>>(Service.GetAllSongs());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        public class SongModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int Template { get; set; }
            public string Type { get; set; }
            public string Code { get; set; }
            public string Source { get; set; }
            public string OtherInformation { get; set; }
            public List<IStanza> Stanzas { get; set; }
            public IComposer Composer { get; set; }
            public IComposer Arranger { get; set; }
            public List<IInstrumentalPart> InstrumentalParts { get; set; }
            public List<int> ThemeCategories { get; set; }
            public List<int> LiturgyCategories { get; set; }
        }

        public class StanzaModel
        {
            public int Number { get; set; }
            public string Text { get; set; }
        }

        public class InstrumentalPartModel
        {
            public int Id { get; set; }
            public string Position { get; set; }
            public string Type { get; set; }
            public string Code { get; set; }
            public int Template { get; set; }
        }
    }
}