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

namespace LiturgicalMusic.Repository
{
    public class SongRepository : ISongRepository
    {
        protected IMapper Mapper { get; private set; }

        public SongRepository(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public async Task<ISong> CreateSongAsync(ISong song)
        {
            string pathToWebAPI = @"E:\vs projects\LiturgicalMusic\LiturgicalMusic.WebAPI";
            string tempDir = String.Format(@"{0}\temp", pathToWebAPI);
            string srcDir = String.Format(@"{0}\src", pathToWebAPI);
            string fileName = Path.GetRandomFileName();
            string filePath = await Lilypond.CreateFileAsync(song, tempDir, fileName, true);

            if (!File.Exists(filePath)) // PDF not created so something is wrong...
            {
                throw new Exception("Something went wrong!");
            }
            
            string songFileName = song.Title;

            if (song.Composer != null)
            {
                songFileName += song.Composer.Name + song.Composer.Surname;
            }
            else if (song.Arranger != null)
            {
                songFileName += song.Arranger.Name + song.Arranger.Surname;
            }

            if (File.Exists(String.Format(@"{0}\{1}.ly", tempDir, songFileName.GetHashCode().ToString())))
            {
                File.Delete(String.Format(@"{0}\{1}.ly", tempDir, songFileName.GetHashCode().ToString()));
                File.Delete(String.Format(@"{0}\{1}.bat", tempDir, songFileName.GetHashCode().ToString()));
            }

            string moveTo = String.Format(@"{0}\app\assets\pdf\{1}.pdf", srcDir, songFileName);

            if (File.Exists(moveTo))
            {
                File.Delete(moveTo);
            }
                
            File.Move(filePath, moveTo);
            File.Delete(filePath);

            // DB
            SongEntity songEntity;

            using (var db = new MusicContext())
            {
                songEntity = Mapper.Map<SongEntity>(song);

                if (songEntity.Composer != null)
                {
                    ComposerEntity composerEntity = await db.Composers.SingleOrDefaultAsync(c => c.Id.Equals(songEntity.Composer.Id));

                    songEntity.Composer = composerEntity;
                }

                if (songEntity.Arranger != null)
                {
                    ComposerEntity arrangerEntity = await db.Composers.SingleOrDefaultAsync(c => c.Id.Equals(songEntity.Arranger.Id));

                    songEntity.Arranger = arrangerEntity;
                }

                if (songEntity.LiturgyCategories.Count() > 0)
                {
                    List<int> liturgyIds = song.LiturgyCategories;
                    songEntity.LiturgyCategories = await db.LiturgyCategories.Where(l => liturgyIds.Contains(l.Id)).ToListAsync();
                }

                if (songEntity.ThemeCategories.Count() > 0)
                {
                    List<int> themeIds = song.ThemeCategories;
                    songEntity.ThemeCategories = await db.ThemeCategories.Where(l => themeIds.Contains(l.Id)).ToListAsync();
                }

                db.Songs.Add(songEntity);
                await db.SaveChangesAsync();
            }

            return Mapper.Map<ISong>(songEntity);
        }

        public async Task<List<ISong>> GetAllSongsAsync()
        {
            List<SongEntity> songEntities;

            using (var db = new MusicContext())
            {
                songEntities = await db.Songs
                    .Include("Composer")
                    .Include("Arranger")
                    .Include("ThemeCategories")
                    .Include("LiturgyCategories")
                    .Include("InstrumentalParts")
                    .Include("Stanzas")
                    .ToListAsync();
            }

            return Mapper.Map<List<ISong>>(songEntities);
        }

        public async Task<ISong> GetSongByIdAsync(int songId)
        {
            SongEntity songEntity;

            using (var db = new MusicContext())
            {
                songEntity = await db.Songs
                    .Include("Composer")
                    .Include("Arranger")
                    .Include("ThemeCategories")
                    .Include("LiturgyCategories")
                    .Include("InstrumentalParts")
                    .Include("Stanzas")
                    .SingleOrDefaultAsync(s => s.Id.Equals(songId));
            }

            return Mapper.Map<ISong>(songEntity);
        }

        public async Task<ISong> PreviewSongAsync(ISong song)
        {
            string pathToWebAPI = @"E:\vs projects\LiturgicalMusic\LiturgicalMusic.WebAPI";
            string tempDir = String.Format(@"{0}\temp", pathToWebAPI);
            string srcDir = String.Format(@"{0}\src", pathToWebAPI);
            string songFileName = song.Title;
            string fileName;

            if (song.Composer != null)
            {
                songFileName += song.Composer.Name + song.Composer.Surname;
            }
            else if (song.Arranger != null)
            {
                songFileName += song.Arranger.Name + song.Arranger.Surname;
            }

            fileName = songFileName.GetHashCode().ToString();

            string filePath = await Lilypond.CreateFileAsync(song, tempDir, fileName, false);

            if (!File.Exists(filePath)) // PDF not created so something is wrong...
            {
                throw new Exception("Something went wrong!");
            }

            string moveTo = String.Format(@"{0}\app\assets\pdf\{1}.pdf", srcDir, songFileName);

            if (File.Exists(moveTo))
            {
                File.Delete(moveTo);
            }

            File.Move(filePath, moveTo);

            return song;
        }

        public async Task<ISong> UpdateSongAsync(ISong song)
        {
            string pathToWebAPI = @"E:\vs projects\LiturgicalMusic\LiturgicalMusic.WebAPI";
            string tempDir = String.Format(@"{0}\temp", pathToWebAPI);
            string srcDir = String.Format(@"{0}\src", pathToWebAPI);
            string fileName = Path.GetRandomFileName();
            string filePath = await Lilypond.CreateFileAsync(song, tempDir, fileName, true);

            if (!File.Exists(filePath)) // PDF not created so something is wrong...
            {
                throw new Exception("Something went wrong!");
            }

            string songFileName = song.Title;

            if (song.Composer != null)
            {
                songFileName += song.Composer.Name + song.Composer.Surname;
            }
            else if (song.Arranger != null)
            {
                songFileName += song.Arranger.Name + song.Arranger.Surname;
            }

            if (File.Exists(String.Format(@"{0}\{1}.ly", tempDir, songFileName.GetHashCode().ToString())))
            {
                File.Delete(String.Format(@"{0}\{1}.ly", tempDir, songFileName.GetHashCode().ToString()));
                File.Delete(String.Format(@"{0}\{1}.bat", tempDir, songFileName.GetHashCode().ToString()));
            }

            string moveTo = String.Format(@"{0}\app\assets\pdf\{1}.pdf", srcDir, songFileName);

            if (File.Exists(moveTo))
            {
                File.Delete(moveTo);
            }

            File.Move(filePath, moveTo);
            File.Delete(filePath);

            // DB
            SongEntity songEntity;

            using (var db = new MusicContext())
            {
                songEntity = Mapper.Map<SongEntity>(song);

                // CRUD Instrumental part
                List<InstrumentalPartEntity> parts = db.InstrumentalParts.Where(p => p.SongId.Equals(songEntity.Id)).ToList();

                foreach (InstrumentalPartEntity part in parts)
                {
                    if (songEntity.InstrumentalParts.SingleOrDefault(p => p.Position.Equals(part.Position)) == null)
                    {
                        db.InstrumentalParts.Remove(part);
                    }
                }

                foreach (InstrumentalPartEntity part in songEntity.InstrumentalParts)
                {
                    InstrumentalPartEntity dbPart = await db.InstrumentalParts.SingleOrDefaultAsync(p => p.Id.Equals(part.Id));

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
                        db.InstrumentalParts.Add(part);
                    }
                }

                // CRUD stanzas
                List<StanzaEntity> stanzas = await db.Stanzas.Where(s => s.SongId.Equals(songEntity.Id)).ToListAsync();

                foreach (StanzaEntity stanza in stanzas)
                {
                    if (songEntity.Stanzas.SingleOrDefault(s => s.Id.Equals(stanza.Id)) == null)
                    {
                        db.Stanzas.Remove(stanza);
                    }
                }

                foreach (StanzaEntity stanza in songEntity.Stanzas)
                {
                    StanzaEntity dbStanza = await db.Stanzas.SingleOrDefaultAsync(s => s.Id.Equals(stanza.Id));

                    if (dbStanza != null)
                    {
                        dbStanza.Text = stanza.Text;
                    }
                    else
                    {
                        stanza.SongId = songEntity.Id;
                        db.Stanzas.Add(stanza);
                    }
                }

                // UPDATE song
                SongEntity dbSong = await db.Songs.SingleOrDefaultAsync(s => s.Id.Equals(songEntity.Id));

                dbSong.ArrangerId = songEntity.ArrangerId;
                dbSong.Code = songEntity.Code;
                dbSong.ComposerId = songEntity.ComposerId;
                dbSong.OtherInformations = songEntity.OtherInformations;
                dbSong.OtherParts = songEntity.OtherParts;
                dbSong.Source = songEntity.Source;
                dbSong.Template = songEntity.Template;
                dbSong.Title = songEntity.Title;
                dbSong.Type = songEntity.Type;

                await db.SaveChangesAsync();
            }

            return Mapper.Map<ISong>(songEntity);
        }
    }
}
