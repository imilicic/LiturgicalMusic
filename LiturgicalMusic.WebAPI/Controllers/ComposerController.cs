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
        public async Task<HttpResponseMessage> GetComposersAsync()
        {
            List<IComposer> c = await Service.GetComposersAsync();
            List<ComposerModel> result = Mapper.Map<List<ComposerModel>>(c);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateComposerAsync([FromBody] ComposerModel composer)
        {
            IComposer newComposer = Service.CreateComposer();

            Mapper.Map(composer, newComposer);

            IComposer resultComposer = await Service.InsertComposerAsync(newComposer);
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
