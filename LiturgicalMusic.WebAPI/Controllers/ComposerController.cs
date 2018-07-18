using AutoMapper;
using LiturgicalMusic.Service.Common;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.WebAPI.Controllers
{
    [RoutePrefix("api/composers")]
    public class ComposerController : ApiController
    {
        protected IComposerService Service { get; private set; }
        protected IMapper Mapper { get; private set; }

        public ComposerController()
        {
        }

        public ComposerController(IMapper mapper, IComposerService service)
        {
            this.Mapper = mapper;
            this.Service = service;
        }

        [HttpGet]
        [Route("get")]
        public HttpResponseMessage GetAllComposers()
        {
            List<ComposerModel> result = Mapper.Map<List<ComposerModel>>(Service.GetAllComposers());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage CreateComposer([FromBody] ComposerModel composer)
        {
            var result = Service.CreateComposer(Mapper.Map<IComposer>(composer));
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        public class ComposerModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
        }
    }
}
