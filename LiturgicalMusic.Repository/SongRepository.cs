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
    public class SongRepository : ISongRepository, IDisposable
    {
        protected MusicContext Context { get; private set; }
        private bool disposed = false;
        protected IMapper Mapper { get; private set; }

        private const string PATH_TO_WEB_API = @"E:\vs projects\LiturgicalMusic\LiturgicalMusic.WebAPI";
        private const string PDF_ASSETS_DIR = @"app\assets\pdf";
        private const string SOURCE_DIR = "src";
        private const string TEMP_DIR = "temp";

        public SongRepository(MusicContext context, IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
        }

        private async Task CreateSongPdfAsync(ISong song, string fileName, bool deleteTempFiles)
        {
            string tempDir = String.Format(@"{0}\{1}", PATH_TO_WEB_API, TEMP_DIR);
            string srcDir = String.Format(@"{0}\{1}", PATH_TO_WEB_API, SOURCE_DIR);
            Lilypond lyGenerator = new Lilypond(song, tempDir, fileName, deleteTempFiles);
            string filePath = await lyGenerator.CreateFileAsync();
            string songFileName = SongFileName(song);

            if (!File.Exists(filePath)) // PDF not created so something is wrong...
            {
                throw new Exception("Something went wrong!");
            }


            if (deleteTempFiles)
            {
                if (File.Exists(String.Format(@"{0}\{1}.ly", tempDir, Hash(songFileName))))
                {
                    File.Delete(String.Format(@"{0}\{1}.ly", tempDir, Hash(songFileName)));
                    File.Delete(String.Format(@"{0}\{1}.bat", tempDir, Hash(songFileName)));
                }
            }

            string moveTo = String.Format(@"{0}\{1}\{2}.pdf", srcDir, PDF_ASSETS_DIR, songFileName);

            if (File.Exists(moveTo))
            {
                File.Delete(moveTo);
            }

            File.Move(filePath, moveTo);
            File.Delete(filePath);
        }

        public async Task<ISong> GetSongByIdAsync(int songId, IOptions options)
        {
            SongEntity songEntity = await Context.Songs.SingleOrDefaultAsync(s => s.Id.Equals(songId));

            if (options.Composer)
            {
                Context.Entry(songEntity).Reference(s => s.Composer).Load();
            }
            if (options.Arranger)
            {
                Context.Entry(songEntity).Reference(s => s.Arranger).Load();
            }
            if (options.Stanzas)
            {
                Context.Entry(songEntity).Collection(s => s.Stanzas).Load();
            }
            if (options.InstrumentalParts)
            {
                Context.Entry(songEntity).Collection(s => s.InstrumentalParts).Load();
            }

            return Mapper.Map<ISong>(songEntity);
        }

        public async Task<List<ISong>> GetSongsAsync(IFilter filter, IOptions options)
        {
            List<SongEntity> songEntities;

            if (filter.Title == null)
            {
                songEntities = await Context.Songs
                    .OrderBy(s => s.Title)
                    .ToListAsync();
            }
            else
            {
                songEntities = await Context.Songs
                    .Where(s => s.Title.Contains(filter.Title))
                    .OrderBy(s => s.Title)
                    .ToListAsync();
            }

            if (options.Composer)
            {
                songEntities.ForEach(song => 
                {
                    Context.Entry(song).Reference(s => s.Composer).Load();
                });
            }

            if (options.Arranger)
            {
                songEntities.ForEach(song =>
                {
                    Context.Entry(song).Reference(s => s.Arranger).Load();
                });
            }

            return Mapper.Map<List<ISong>>(songEntities);
        }

        private string SongFileName(ISong song)
        {
            string songFileName = song.Title;

            if (song.Composer != null)
            {
                songFileName = String.Concat(songFileName, song.Composer.Name, song.Composer.Surname);
            }
            else if (song.Arranger != null)
            {
                songFileName = String.Concat(songFileName, song.Arranger.Name, song.Arranger.Surname);
            }

            return songFileName;
        }

        private string Hash(string text)
        {
            int hash = text.GetHashCode();
            string result;

            if (hash < 0)
            {
                result = String.Concat("m", (-hash).ToString());
            }
            else
            {
                result = hash.ToString();
            }

            return result;
        }

        public async Task<ISong> InsertSongAsync(ISong song)
        {
            await CreateSongPdfAsync(song, Path.GetRandomFileName(), true);

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

            Context.Songs.Add(songEntity);
            await Context.SaveChangesAsync();

            return Mapper.Map<ISong>(songEntity);
        }

        public async Task<ISong> PreviewSongAsync(ISong song)
        {
            string songFileName = SongFileName(song);

            await CreateSongPdfAsync(song, Hash(songFileName), false);

            return song;
        }

        public async Task<ISong> UpdateSongAsync(ISong song)
        {
            await CreateSongPdfAsync(song, Path.GetRandomFileName(), true);

            SongEntity songEntity = Mapper.Map<SongEntity>(song);

            // CRUD Instrumental part
            List<InstrumentalPartEntity> parts = Context.InstrumentalParts.Where(p => p.SongId.Equals(songEntity.Id)).ToList();

            foreach (InstrumentalPartEntity part in parts)
            {
                if (songEntity.InstrumentalParts.SingleOrDefault(p => p.Position.Equals(part.Position)) == null)
                {
                    Context.InstrumentalParts.Remove(part);
                }
            }

            foreach (InstrumentalPartEntity part in songEntity.InstrumentalParts)
            {
                InstrumentalPartEntity dbPart = await Context.InstrumentalParts.SingleOrDefaultAsync(p => p.Id.Equals(part.Id));

                if (dbPart != null)
                {
                    dbPart.Code = part.Code;
                    dbPart.Position = part.Position;
                    dbPart.Template = part.Template;
                    dbPart.Type = part.Type;
                }
                else
                {
                    part.SongId = songEntity.Id;
                    Context.InstrumentalParts.Add(part);
                }
            }

            // CRUD stanzas
            List<StanzaEntity> stanzas = await Context.Stanzas.Where(s => s.SongId.Equals(songEntity.Id)).ToListAsync();

            foreach (StanzaEntity stanza in stanzas)
            {
                if (songEntity.Stanzas.SingleOrDefault(s => s.Id.Equals(stanza.Id)) == null)
                {
                    Context.Stanzas.Remove(stanza);
                }
            }

            foreach (StanzaEntity stanza in songEntity.Stanzas)
            {
                StanzaEntity dbStanza = await Context.Stanzas.SingleOrDefaultAsync(s => s.Id.Equals(stanza.Id));

                if (dbStanza != null)
                {
                    dbStanza.Text = stanza.Text;
                }
                else
                {
                    stanza.SongId = songEntity.Id;
                    Context.Stanzas.Add(stanza);
                }
            }
            
            Context.Songs.AddOrUpdate(songEntity);

            await Context.SaveChangesAsync();

            return Mapper.Map<ISong>(songEntity);
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
