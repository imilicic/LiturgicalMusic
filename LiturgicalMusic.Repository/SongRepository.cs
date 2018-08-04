using AutoMapper;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Model;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using LiturgicalMusic.Common;
using System.Data.Entity.Migrations;

namespace LiturgicalMusic.Repository
{
    public class SongRepository : GenericRepository<SongEntity>, ISongRepository, IDisposable
    {
        protected IMapper Mapper { get; private set; }
        private bool disposed = false;

        public SongRepository(MusicContext context, IMapper mapper) : base(context)
        {
            this.Mapper = mapper;
        }

        public async Task<ISong> GetByIdAsync(int songId, IOptions options)
        {
            string include = SongHelper.CreateIncludeString(options);
            SongEntity songEntity = await base.GetByIdAsync(songId, include);

            return Mapper.Map<ISong>(songEntity);
        }

        public async Task<List<ISong>> GetAsync(IFilter filter, IOptions options)
        {
            List<SongEntity> songEntities;
            string include = SongHelper.CreateIncludeString(options);
            Func<IQueryable<SongEntity>, IOrderedQueryable<SongEntity>> orderByTitle = s => s.OrderBy(se => se.Title);

            if (filter.Title != null)
            {
                songEntities = await base.GetAsync(s => s.Title.Contains(filter.Title), orderByTitle, include);
            }
            else
            {
                songEntities = await base.GetAsync(null, orderByTitle, include);
            }

            return Mapper.Map<List<ISong>>(songEntities);
        }

        public async Task<ISong> InsertAsync(ISong song)
        {
            await SongHelper.CreatePdfAsync(song, Path.GetRandomFileName(), true);

            SongEntity songEntity = Mapper.Map<SongEntity>(song);

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

            return Mapper.Map<ISong>(await base.InsertAsync(songEntity));
        }

        public async Task<ISong> PreviewAsync(ISong song)
        {
            string songFileName = SongHelper.SongFileName(song);

            await SongHelper.CreatePdfAsync(song, SongHelper.Hash(songFileName), false);

            return song;
        }

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
            SongEntity songDb = await base.GetByIdAsync(song.Id, include);
            SongEntity songEntity = Mapper.Map<SongEntity>(song);

            await SongHelper.UpdatePdfAsync(song, Path.GetRandomFileName(), SongHelper.SongFileName(Mapper.Map<ISong>(songDb)), true);

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

            SongEntity result = await base.UpdateAsync(songDb);

            return Mapper.Map<ISong>(result);

            // CRUD Instrumental part
            //List<InstrumentalPartEntity> parts = Context.InstrumentalParts.Where(p => p.SongId.Equals(songEntity.Id)).ToList();

            //foreach (InstrumentalPartEntity part in parts)
            //{
            //    if (songEntity.InstrumentalParts.SingleOrDefault(p => p.Position.Equals(part.Position)) == null)
            //    {
            //        Context.InstrumentalParts.Remove(part);
            //    }
            //}

            //foreach (InstrumentalPartEntity part in songEntity.InstrumentalParts)
            //{
            //    InstrumentalPartEntity dbPart = await Context.InstrumentalParts.SingleOrDefaultAsync(p => p.Id.Equals(part.Id));

            //    if (dbPart != null)
            //    {
            //        dbPart.Code = part.Code;
            //        dbPart.Position = part.Position;
            //        dbPart.Template = part.Template;
            //        dbPart.Type = part.Type;
            //    }
            //    else
            //    {
            //        part.SongId = songEntity.Id;
            //        Context.InstrumentalParts.Add(part);
            //    }
            //}

            //// CRUD stanzas
            //List<StanzaEntity> stanzas = await Context.Stanzas.Where(s => s.SongId.Equals(songEntity.Id)).ToListAsync();

            //foreach (StanzaEntity stanza in stanzas)
            //{
            //    if (songEntity.Stanzas.SingleOrDefault(s => s.Id.Equals(stanza.Id)) == null)
            //    {
            //        Context.Stanzas.Remove(stanza);
            //    }
            //}

            //foreach (StanzaEntity stanza in songEntity.Stanzas)
            //{
            //    StanzaEntity dbStanza = await Context.Stanzas.SingleOrDefaultAsync(s => s.Id.Equals(stanza.Id));

            //    if (dbStanza != null)
            //    {
            //        dbStanza.Text = stanza.Text;
            //    }
            //    else
            //    {
            //        stanza.SongId = songEntity.Id;
            //        Context.Stanzas.Add(stanza);
            //    }
            //}
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
