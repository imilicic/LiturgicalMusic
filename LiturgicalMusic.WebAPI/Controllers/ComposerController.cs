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
        public async Task<HttpResponseMessage> GetAsync()
        {
            IList<IComposer> c = await Service.GetAsync();
            IList<ComposerModel> result = Mapper.Map<IList<ComposerModel>>(c);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> GetByIdAsync(int composerId)
        {
            IComposer composer = await Service.GetByIdAsync(composerId);
            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<ComposerModel>(composer));
        }

        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateAsync([FromBody] ComposerModel composer)
        {
            IComposer newComposer = Service.Create();

            Mapper.Map(composer, newComposer);

            IComposer resultComposer = await Service.InsertAsync(newComposer);
            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<ComposerModel>(resultComposer));
        }

        public class ComposerModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
        }
    }
}
