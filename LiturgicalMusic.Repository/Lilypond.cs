using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model;
using LiturgicalMusic.Model.Common;
using Newtonsoft.Json;
using System.Diagnostics;

namespace LiturgicalMusic.Repository
{
    public class Lilypond
    {
        public static string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static async Task<string> CreateFileAsync(ISong song, string path, string fileName, bool deleteTempFiles)
        {
            dynamic code = JsonConvert.DeserializeObject(song.Code);

            ICode organCode = new Code
            {
                Soprano = code.OrganSoprano,
                Alto = code.OrganAlto,
                Tenor = code.OrganTenor,
                Bass = code.OrganBass,
                SopranoRelative = code.OrganSopranoRelative,
                AltoRelative = code.OrganAltoRelative,
                TenorRelative = code.OrganTenorRelative,
                BassRelative = code.OrganBassRelative
            };

            ICode voiceCode = new Code
            {
                Soprano = code.VoiceSoprano,
                Alto = code.VoiceAlto,
                Tenor = code.VoiceTenor,
                Bass = code.VoiceBass,
                SopranoRelative = code.VoiceSopranoRelative,
                AltoRelative = code.VoiceAltoRelative,
                TenorRelative = code.VoiceTenorRelative,
                BassRelative = code.VoiceBassRelative
            };

            IInstrumentalPart prelude = null, interlude = null, coda = null;

            if (song.InstrumentalParts.Count() > 0)
            {
                prelude = song.InstrumentalParts.Find(p => p.Position.Equals("prelude"));
                interlude = song.InstrumentalParts.Find(p => p.Position.Equals("interlude"));
                coda = song.InstrumentalParts.Find(p => p.Position.Equals("coda"));
            }

            string key = code.Key;
            string time = code.Time;

            using (StreamWriter file = new StreamWriter(String.Format(@"{0}\{1}.ly", path, fileName)))
            {
                await file.WriteLineAsync(CreateHeader(song, key, time));

                if (prelude != null)
                {
                    ICode preludeCode = JsonConvert.DeserializeObject<Code>(prelude.Code);
                    await file.WriteLineAsync(CreateVoices(preludeCode, prelude.Template, "prelude"));
                    await file.WriteLineAsync(CreatePartScore(prelude.Template, "prelude", true));
                }

                await file.WriteLineAsync(CreateVoices(voiceCode, song.Template.GetRange(0, 4), "voice"));
                await file.WriteLineAsync(CreateVoices(organCode, song.Template.GetRange(4, 4), "organ"));
                await file.WriteLineAsync(CreateLyrics(song.Stanzas));
                await file.WriteLineAsync(CreateMainScore(song));

                if (interlude != null)
                {
                    ICode interludeCode = JsonConvert.DeserializeObject<Code>(interlude.Code);
                    await file.WriteLineAsync(CreateVoices(interludeCode, interlude.Template, "interlude"));
                    await file.WriteLineAsync(CreatePartScore(interlude.Template, "interlude"));
                }

                if (coda != null)
                {
                    ICode codaCode = JsonConvert.DeserializeObject<Code>(coda.Code);
                    await file.WriteLineAsync(CreateVoices(codaCode, coda.Template, "coda"));
                    await file.WriteLineAsync(CreatePartScore(coda.Template, "coda"));
                }
            }

            using (StreamWriter file = new StreamWriter(String.Format(@"{0}\{1}.bat", path, fileName)))
            {
                file.WriteLine(String.Format(@"lilypond ""{0}.ly""", fileName));
            }

            Process process = new Process();
            process.StartInfo.WorkingDirectory = path;
            process.StartInfo.FileName = String.Format("{0}.bat", fileName);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();

            if (deleteTempFiles)
            {
                File.Delete(String.Format(@"{0}\{1}.ly", path, fileName));
                File.Delete(String.Format(@"{0}\{1}.bat", path, fileName));
            }

            return String.Format(@"{0}\{1}.pdf", path, fileName);
        }

        public static string CreateHeader(ISong song, string key = null, string time = null)
        {
            StringBuilder header = new StringBuilder();

            header.AppendLine(@"\include ""global-vkg.ily""");
            header.AppendLine(@"\pointAndClickOff");
            header.AppendLine(@"\header{");
            header.AppendLine(String.Format(@"title = ""{0}""", song.Title));
            header.AppendLine(String.Format(@"othersource = ""{0}""", song.OtherInformations));

            if (song.Composer != null)
            {
                header.AppendLine(String.Format(@"composer = ""{0} {1}""", song.Composer.Name, song.Composer.Surname));
            }

            if (song.Arranger != null)
            {
                header.AppendLine(String.Format(@"arranger = ""Harmonizacija: {0} {1}""", song.Arranger.Name, song.Arranger.Surname));
            }

            header.AppendLine(String.Format(@"source = ""{0}""", song.Source));
            header.AppendLine(@"tagline = """"");
            header.AppendLine(@"}");

            if (song.Type == "hymn")
            {
                header.AppendLine(String.Format(@"keyTime = {{ \key {0} \time {1} }}", key, time));
            }

            return header.ToString();
        }

        public static string CreateLyrics(List<IStanza> stanzas)
        {
            StringBuilder lyrics = new StringBuilder();

            stanzas.ForEach((IStanza stanza) => {
                lyrics.AppendLine(String.Format(@"lyrics{0} = \lyricmode {{", alphabet[stanza.Number]));
                lyrics.AppendLine(stanza.Text);
                lyrics.AppendLine("}");
            });

            return lyrics.ToString();
        }

        public static string CreateVoices(ICode code, List<bool> template, string instrument)
        {
            StringBuilder voices = new StringBuilder();

            if (template.Any(s => s)) // there is at least one voice
            {
                if (template[0])
                {
                    voices.AppendLine(String.Format(@"{0}S = \relative {1} {{", instrument, code.SopranoRelative));
                    voices.AppendLine(code.Soprano);
                    voices.AppendLine(@"}");
                }
                if (template[1])
                {
                    voices.AppendLine(String.Format(@"{0}A = \relative {1} {{", instrument, code.AltoRelative));
                    voices.AppendLine(code.Alto);
                    voices.AppendLine(@"}");
                }
                if (template[2])
                {
                    voices.AppendLine(String.Format(@"{0}T = \relative {1} {{", instrument, code.TenorRelative));
                    voices.AppendLine(code.Tenor);
                    voices.AppendLine(@"}");
                }
                if (template[3])
                {
                    voices.AppendLine(String.Format(@"{0}B = \relative {1} {{", instrument, code.BassRelative));
                    voices.AppendLine(code.Bass);
                    voices.AppendLine(@"}");
                }
            }

            return voices.ToString();
        }

        public static string CreateGrandStaff(List<bool> template, string instrument, List<IStanza> stanzas = null, int markedVoice = -1)
        {
            StringBuilder grandStaff = new StringBuilder();
            List<List<string>> staffs = GetVoices(template, instrument);

            grandStaff.AppendLine(@"\new GrandStaff <<");
            grandStaff.AppendLine(CreateStaff(staffs[0], "upper", markedVoice));

            if (markedVoice >= 0)
            {
                stanzas.ForEach((IStanza stanza) => {
                    grandStaff.AppendLine(String.Format(@"\new Lyrics \lyricsto ""voiceS"" \lyrics{0}", alphabet[stanza.Number]));
                });
            }

            grandStaff.AppendLine(CreateStaff(staffs[1], "lower"));
            grandStaff.AppendLine(@">>");

            return grandStaff.ToString();
        }

        public static string CreateStaff(List<string> voices, string position, int markedVoice = -1)
        {
            StringBuilder staff = new StringBuilder();
            int i = 0;
            List<string> numbers = new List<string> { "One", "Two" };

            staff.AppendLine(String.Format(@"\new Staff = ""{0}"" <<", position));

            foreach (string voice in voices)
            {
                if (voices.Count() == 1)
                {
                    if (markedVoice == 0)
                    {
                        staff.AppendLine(String.Format(@"\new Voice = ""voiceS"" << \{0} >>", voice));
                    }
                    else
                    {
                        staff.AppendLine(String.Format(@"\new Voice << \{0} >>", voice));
                    }
                }
                else
                {
                    if (markedVoice == i)
                    {
                        staff.AppendLine(String.Format(@"\new Voice = ""voiceS"" << \voice{0} \{1} >>", numbers[i], voice));
                    }
                    else
                    {
                        staff.AppendLine(String.Format(@"\new Voice << \voice{0} \{1} >>", numbers[i], voice));
                    }
                    i++;
                }
            }

            staff.AppendLine(">>");
            return staff.ToString();
        }

        public static string CreatePartScore(List<bool> template, string instrument, bool isPrelude = false)
        {
            StringBuilder score = new StringBuilder();

            if (isPrelude)
            {
                score.AppendLine(@"\markup \fill-line {");
                score.AppendLine(@"\hspace #1");
            }

            score.AppendLine(@"\score {");

            if (template.GetRange(0, 2).Any(s => s) && template.GetRange(2, 2).Any(s => s)) // there is upper and lower parts, ie grandstaff must be used
            {
                score.AppendLine(CreateGrandStaff(template, instrument));
            }
            else // use only one staff
            {
                List<List<string>> staffs = Lilypond.GetVoices(template, instrument);

                if (staffs[0].Count() == 0)
                {
                    score.AppendLine(CreateStaff(staffs[1], "lower"));
                }
                else
                {
                    score.AppendLine(CreateStaff(staffs[0], "upper"));
                }
            }

            if (isPrelude)
            {
                score.AppendLine(@"\layout {}");
                score.AppendLine(@"}");
            }

            score.AppendLine(@"}");

            return score.ToString();
        }

        public static string CreateMainScore(ISong song)
        {
            StringBuilder score = new StringBuilder();
            List<bool> voiceTemplate = song.Template.GetRange(0, 4);
            List<bool> organTemplate = song.Template.GetRange(4, 4);

            score.AppendLine(@"\score {");

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
                score.AppendLine(@"<<");
            }

            if (voiceTemplate.Any(s => s)) // part for voice
            {
                if (voiceTemplate.GetRange(0, 2).Any(s => s) && voiceTemplate.GetRange(2, 2).Any(s => s)) // there is upper and lower parts, ie grandstaff must be used
                {
                    score.AppendLine(CreateGrandStaff(voiceTemplate, "voice", song.Stanzas, markVoice));
                }
                else // use only one staff
                {
                    List<List<string>> staffs = Lilypond.GetVoices(voiceTemplate, "voice");

                    if (staffs[0].Count() == 0)
                    {
                        score.AppendLine(CreateStaff(staffs[1], "lower" , markVoice));
                    }
                    else
                    {
                        score.AppendLine(CreateStaff(staffs[0], "upper", markVoice));
                    }

                    song.Stanzas.ForEach((IStanza stanza) => {
                        score.AppendLine(String.Format(@"\new Lyrics \lyricsto ""voiceS"" \lyrics{0}", alphabet[stanza.Number]));
                    });
                }
            }

            if (organTemplate.Any(s => s)) // part for organ
            {
                if (organTemplate.GetRange(0, 2).Any(s => s) && organTemplate.GetRange(2, 2).Any(s => s)) // there is upper and lower parts, ie grandstaff must be used
                {
                    score.AppendLine(CreateGrandStaff(organTemplate, "organ", song.Stanzas, markOrgan));
                }
                else // use only one staff
                {
                    List<List<string>> staffs = Lilypond.GetVoices(organTemplate, "organ");

                    if (staffs[0].Count() == 0)
                    {
                        score.AppendLine(CreateStaff(staffs[1], "lower", markOrgan));
                    }
                    else
                    {
                        score.AppendLine(CreateStaff(staffs[0], "upper", markOrgan));
                    }

                    if (markOrgan >= 0)
                    {
                        song.Stanzas.ForEach((IStanza stanza) => {
                            score.AppendLine(String.Format(@"\new Lyrics \lyricsto ""voiceS"" \lyrics{0}", alphabet[stanza.Number]));
                        });
                    }
                }
            }

            if (voiceTemplate.Any(s => s) && organTemplate.Any(s => s)) // at least one voice in human voice and organ voice, ie. two separate staffs
            {
                score.AppendLine(@">>");
            }

            score.AppendLine(@"}");

            return score.ToString();
        }

        public static List<List<string>> GetVoices(List<bool> template, string instrument)
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
    }
}
