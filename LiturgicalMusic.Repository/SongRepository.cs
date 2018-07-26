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
            // create lilypond file
            string fileName = Path.GetRandomFileName();
            string pathToWebAPI = @"E:\vs projects\LiturgicalMusic\LiturgicalMusic.WebAPI";
            string tempDir = String.Format(@"{0}\temp", pathToWebAPI);
            string srcDir = String.Format(@"{0}\src", pathToWebAPI);

            using (StreamWriter file = new StreamWriter(String.Format(@"{0}\{1}.ly", tempDir, fileName)))
            {
                await file.WriteLineAsync(Lilypond.CreateHeader(song));
                await file.WriteLineAsync(Lilypond.CreateVoices(song));
                await file.WriteLineAsync(Lilypond.CreateScore(song));
            }

            // create bat file and run ly file
            using (StreamWriter file = new StreamWriter(String.Format(@"{0}\{1}.bat", tempDir, fileName)))
            {
                file.WriteLine(String.Format("lilypond {0}.ly", fileName));
            }

            Process process = new Process();
            process.StartInfo.WorkingDirectory = tempDir;
            process.StartInfo.FileName = String.Format("{0}.bat", fileName);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();
            
            File.Delete(String.Format(@"{0}\{1}.ly", tempDir, fileName));
            File.Delete(String.Format(@"{0}\{1}.bat", tempDir, fileName));
            
            if (!File.Exists(String.Format(@"{0}\{1}.pdf", tempDir, fileName))) // PDF not created so something is wrong...
            {
                throw new Exception("Something went wrong!");
            }
            else
            {
                string songFileName = song.Title;

                if (song.Composer != null)
                {
                    songFileName += song.Composer.Name + song.Composer.Surname;
                }
                else if (song.Arranger != null)
                {
                    songFileName += song.Arranger.Name + song.Arranger.Surname;
                }
                
                string moveTo = String.Format(@"{0}\app\assets\pdf\{1}.pdf", srcDir, songFileName);

                if (File.Exists(moveTo))
                {
                    File.Delete(moveTo);
                }
                
                File.Move(String.Format(@"{0}\{1}.pdf", tempDir, fileName), moveTo);
                File.Delete(String.Format(@"{0}\{1}.pdf", tempDir, fileName));

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
    }
}
