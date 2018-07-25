﻿using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using LiturgicalMusic.Service.Common;
using LiturgicalMusic.Model.Common;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

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
        public async Task<HttpResponseMessage> GetAllSongsAsync()
        {
            List<ISong> s = await Service.GetAllSongsAsync();
            List<SongModel> result = Mapper.Map<List<SongModel>>(s);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> GetSongByIdAsync(int songId)
        {
            ISong s = await Service.GetSongByIdAsync(songId);
            SongModel result = Mapper.Map<SongModel>(s);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateSong([FromBody] SongModel song)
        {
            ISong resultSong = await Service.CreateSongAsync(Mapper.Map<ISong>(song));
            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<SongModel>(resultSong));
        }

        public class SongModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Template { get; set; }
            public string Type { get; set; }
            public string Code { get; set; }
            public string Source { get; set; }
            public string OtherInformation { get; set; }
            public List<StanzaModel> Stanzas { get; set; }
            public ComposerModel Composer { get; set; }
            public ComposerModel Arranger { get; set; }
            public List<InstrumentalPartModel> InstrumentalParts { get; set; }
            public List<int> ThemeCategories { get; set; }
            public List<int> LiturgyCategories { get; set; }
        }

        public class ComposerModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
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