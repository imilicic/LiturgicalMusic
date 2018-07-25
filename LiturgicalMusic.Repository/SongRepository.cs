using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.DAL;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Model;
using LiturgicalMusic.Repository.Common;
using AutoMapper;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace LiturgicalMusic.Repository
{
    public class SongRepository : ISongRepository
    {
        protected IMapper Mapper { get; private set; }

        public SongRepository(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public ISong CreateSong(ISong song)
        {
            // create lilypond file
            string fileName = Path.GetRandomFileName();
            string tempDir = @"E:\vs projects\LiturgicalMusic\LiturgicalMusic.WebAPI\temp";

            ICode code = JsonConvert.DeserializeObject<Code>(song.Code);
            List<bool> template = song.Template.Select(c => Convert.ToBoolean(Convert.ToInt16(c.ToString()))).ToList();
            List<bool> voiceTemplate = template.GetRange(0, 4);
            List<bool> organTemplate = template.GetRange(4, 4);

            using (StreamWriter file = new StreamWriter(tempDir + @"\" + fileName + ".ly"))
            {
                file.WriteLine(@"\include ""global-vkg.ily""");
                file.WriteLine(@"\header{");
                file.WriteLine(String.Format(@"title = ""{0}""", song.Title));
                file.WriteLine(String.Format(@"othersource = ""{0}""", song.OtherInformation));

                if (song.Composer != null)
                {
                    file.WriteLine(String.Format(@"composer = ""{0} {1}""", song.Composer.Name, song.Composer.Surname));
                }

                if (song.Arranger != null)
                {
                    file.WriteLine(String.Format(@"arranger = ""Harmonizacija: {0} {1}""", song.Arranger.Name, song.Arranger.Surname));
                }

                file.WriteLine(String.Format(@"source = ""{0}""", song.Source));
                file.WriteLine(@"tagline = """"");
                file.WriteLine(@"}");

                if (song.Type == "hymn")
                {
                    file.WriteLine(String.Format(@"keyTime = {{ \key {0} \time {1} }}", code.Key, code.Time));
                }

                if (voiceTemplate.Any(s => s)) // there is at least one human voice
                {
                    if (voiceTemplate[0])
                    {
                        file.WriteLine(String.Format(@"voiceS = \relative {0} {{", code.VoiceSopranoRelative));
                        file.WriteLine(code.VoiceSoprano);
                        file.WriteLine(@"}");
                    }
                    if (voiceTemplate[1])
                    {
                        file.WriteLine(String.Format(@"voiceA = \relative {0} {{", code.VoiceAltoRelative));
                        file.WriteLine(code.VoiceAlto);
                        file.WriteLine(@"}");
                    }
                    if (voiceTemplate[2])
                    {
                        file.WriteLine(String.Format(@"voiceT = \relative {0} {{", code.VoiceTenorRelative));
                        file.WriteLine(code.VoiceTenor);
                        file.WriteLine(@"}");
                    }
                    if (voiceTemplate[3])
                    {
                        file.WriteLine(String.Format(@"voiceB = \relative {0} {{", code.VoiceBassRelative));
                        file.WriteLine(code.VoiceBass);
                        file.WriteLine(@"}");
                    }
                }

                if (organTemplate.Any(s => s)) // there is at least one organ voice
                {
                    if (organTemplate[0])
                    {
                        file.WriteLine(String.Format(@"organS = \relative {0} {{", code.OrganSopranoRelative));
                        file.WriteLine(code.OrganSoprano);
                        file.WriteLine(@"}");
                    }
                    if (organTemplate[1])
                    {
                        file.WriteLine(String.Format(@"organA = \relative {0} {{", code.OrganAltoRelative));
                        file.WriteLine(code.OrganAlto);
                        file.WriteLine(@"}");
                    }
                    if (organTemplate[2])
                    {
                        file.WriteLine(String.Format(@"organT = \relative {0} {{", code.OrganTenorRelative));
                        file.WriteLine(code.OrganTenor);
                        file.WriteLine(@"}");
                    }
                    if (organTemplate[3])
                    {
                        file.WriteLine(String.Format(@"organB = \relative {0} {{", code.OrganBassRelative));
                        file.WriteLine(code.OrganBass);
                        file.WriteLine(@"}");
                    }
                }

                // lilypond main part
                file.WriteLine(@"\score {");

                int markVoice = -1;
                int markOrgan = -1;
                if (voiceTemplate.Any(s => s))
                {
                    markVoice = voiceTemplate.IndexOf(true) % 2;
                }
                else
                {
                    markOrgan = organTemplate.IndexOf(true) % 2;
                }

                if (voiceTemplate.Any(s => s) && organTemplate.Any(s => s)) // at least one voice in human voice and organ voice, ie. two separate staffs
                {
                    file.WriteLine(@"<<");
                }

                if (voiceTemplate.Any(s => s)) // part for voice
                {
                    if (voiceTemplate.GetRange(0, 2).Any(s => s) && voiceTemplate.GetRange(2, 2).Any(s => s)) // there is upper and lower parts, ie grandstaff must be used
                    {
                        LilypondWriteGrandStaff(file, voiceTemplate, "voice", markVoice);
                    }
                    else // use only one staff
                    {
                        List<List<string>> staffs = LilypondGetVoices(voiceTemplate, "voice");

                        if (staffs[0].Count() == 0)
                        {
                            LilypondWriteStaff(file, staffs[1], markVoice);
                        }
                        else
                        {
                            LilypondWriteStaff(file, staffs[0], markVoice);
                        }
                    }
                }

                if (organTemplate.Any(s => s)) // part for organ
                {
                    if (organTemplate.GetRange(0, 2).Any(s => s) && organTemplate.GetRange(2, 2).Any(s => s)) // there is upper and lower parts, ie grandstaff must be used
                    {
                        LilypondWriteGrandStaff(file, organTemplate, "organ", markOrgan);
                    }
                    else // use only one staff
                    {
                        List<List<string>> staffs = LilypondGetVoices(organTemplate, "organ");

                        if (staffs[0].Count() == 0)
                        {
                            LilypondWriteStaff(file, staffs[1], markOrgan);
                        }
                        else
                        {
                            LilypondWriteStaff(file, staffs[0], markOrgan);
                        }
                    }
                }

                if (voiceTemplate.Any(s => s) && organTemplate.Any(s => s)) // at least one voice in human voice and organ voice, ie. two separate staffs
                {
                    file.WriteLine(@">>");
                }

                file.WriteLine(@"}");
            }

            // create bat file and run ly file
            string batName = fileName + ".bat";

            using (StreamWriter file = new StreamWriter(tempDir + @"\" + batName))
            {
                file.WriteLine("lilypond " + fileName + ".ly");
            }

            Process process = new Process();
            process.StartInfo.WorkingDirectory = tempDir;
            process.StartInfo.FileName = batName;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();

            File.Delete(tempDir + @"\" + fileName + ".ly");
            File.Delete(tempDir + @"\" + fileName + ".bat");

            if (!File.Exists(tempDir + @"\" + fileName + ".pdf")) // PDF not created so something is wrong...
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

                string moveTo = String.Format(@"E:\vs projects\LiturgicalMusic\LiturgicalMusic.WebAPI\src\app\assets\pdf\{0}.pdf", songFileName);

                if (File.Exists(moveTo))
                {
                    File.Delete(moveTo);
                }

                File.Move(tempDir + @"\" + fileName + ".pdf", moveTo);
                File.Delete(tempDir + @"\" + fileName + ".pdf");

                // DB
                SongEntity songEntity;

                using (var db = new MusicContext())
                {
                    songEntity = Mapper.Map<SongEntity>(song);

                    if (songEntity.Composer != null)
                    {
                        ComposerEntity composerEntity = db.Composers.SingleOrDefault(c => c.Id.Equals(songEntity.Composer.Id));

                        songEntity.Composer = composerEntity;
                    }

                    if (songEntity.Arranger != null)
                    {
                        ComposerEntity arrangerEntity = db.Composers.SingleOrDefault(c => c.Id.Equals(songEntity.Arranger.Id));

                        songEntity.Arranger = arrangerEntity;
                    }

                    if (songEntity.LiturgyCategories.Count() > 0)
                    {
                        List<int> liturgyIds = song.LiturgyCategories;
                        songEntity.LiturgyCategories = db.LiturgyCategories.Where(l => liturgyIds.Contains(l.Id)).ToList();
                    }

                    if (songEntity.ThemeCategories.Count() > 0)
                    {
                        List<int> themeIds = song.ThemeCategories;
                        songEntity.ThemeCategories = db.ThemeCategories.Where(l => themeIds.Contains(l.Id)).ToList();
                    }

                    db.Songs.Add(songEntity);
                    db.SaveChanges();
                }
                return Mapper.Map<ISong>(songEntity);
            }
        }

        protected void LilypondWriteGrandStaff(StreamWriter file, List<bool> template, string instrument, int markedVoice = -1)
        {
            file.WriteLine(@"\new GrandStaff <<");

            List<List<string>> staffs = LilypondGetVoices(template, instrument);

            LilypondWriteStaff(file, staffs[0], markedVoice);
            LilypondWriteStaff(file, staffs[1]);

            file.WriteLine(@">>");
        }

        protected void LilypondWriteStaff(StreamWriter file, List<string> voices, int markedVoice = -1)
        {
            file.WriteLine(@"\new Staff <<");
            int i = 0;
            List<string> numbers = new List<string> { "One", "Two" };

            foreach (string voice in voices)
            {
                if (voices.Count() == 1)
                {
                    if (markedVoice == 0)
                    {
                        file.WriteLine(String.Format(@"\new Voice = ""voiceS"" << \{0} >>", voice));
                    }
                    else
                    {
                        file.WriteLine(String.Format(@"\new Voice << \{0} >>", voice));
                    }
                }
                else
                {
                    if (markedVoice == i)
                    {
                        file.WriteLine(String.Format(@"\new Voice = ""voiceS"" << \voice{0} \{1} >>", numbers[i], voice));
                    }
                    else
                    {
                        file.WriteLine(String.Format(@"\new Voice << \voice{0} \{1} >>", numbers[i], voice));
                    }
                    i++;
                }
            }

            file.WriteLine(@">>");
        }

        protected List<List<string>> LilypondGetVoices(List<bool> template, string instrument)
        {
            List<string> voices = new List<string> { "S", "A", "T", "B" };
            List<string> staff1 = new List<string> { };
            List<string> staff2 = new List<string> { };

            for (int i = 0; i < template.Count(); i++)
            {
                if (template[i])
                {
                    if (i <= 1)
                    {
                        staff1.Add(instrument + voices[i]);
                    }
                    else
                    {
                        staff2.Add(instrument + voices[i]);
                    }
                }
            }

            return new List<List<string>> { staff1, staff2 };
        }

        public List<ISong> GetAllSongs()
        {
            List<SongEntity> songEntities;

            using (var db = new MusicContext())
            {
                songEntities = db.Songs
                    .Include("Composer")
                    .Include("Arranger")
                    .Include("ThemeCategories")
                    .Include("LiturgyCategories")
                    .Include("InstrumentalParts")
                    .Include("Stanzas")
                    .ToList();
            }

            return Mapper.Map<List<ISong>>(songEntities);
        }

        public ISong GetSongById(int songId)
        {
            SongEntity songEntity;

            using (var db = new MusicContext())
            {
                songEntity = db.Songs
                    .Include("Composer")
                    .Include("Arranger")
                    .Include("ThemeCategories")
                    .Include("LiturgyCategories")
                    .Include("InstrumentalParts")
                    .Include("Stanzas")
                    .SingleOrDefault(s => s.Id.Equals(songId));
            }

            return Mapper.Map<ISong>(songEntity);
        }
    }
}
