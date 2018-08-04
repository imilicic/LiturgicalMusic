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
using System.Reflection;

namespace LiturgicalMusic.Repository
{
    public class Lilypond
    {
        #region Properties
        public string FileName { get; set; }
        public bool DeleteTempFiles { get; set; }
        public string Key { get; set; }
        public string Path { get; set; }
        public ISong Song { get; set; }
        public string Time { get; set; }

        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string ARRANGER_TEXT = "Harmonizacija:";
        private const string CODA_PART = "coda";
        private const string HUMAN_VOICE_PARTS = "voice";
        private const string HUMAN_VOICE_PARTS_PROPERTY = "Voice";
        private const string INSTRUMENT_PARTS = "organ";
        private const string INSTRUMENT_PARTS_PROPERTY = "Organ";
        private const string INTERLUDE_PART = "interlude";
        private const string LOWER_STAFF = "lower";
        private const string MARKED_VOICE = "voiceS";
        private const string PRELUDE_PART = "prelude";
        private const string UPPER_STAFF = "upper";
        private const string VOICES = "SATB";
        #endregion Properties

        #region Constructors
        public Lilypond(ISong song, string path, string fileName, bool deleteTempFiles)
        {
            this.Song = song;
            this.Path = path;
            this.FileName = fileName;
            this.DeleteTempFiles = deleteTempFiles;

            dynamic code = JsonConvert.DeserializeObject(Song.Code);
            this.Key = code.Key;
            this.Time = code.Time;
        }
        #endregion Constructors

        #region Methods
        public async Task<string> CreateFileAsync()
        {
            List<ICode> organCode = ExtractCode(Song.Code, INSTRUMENT_PARTS_PROPERTY);
            List<ICode> voiceCode = ExtractCode(Song.Code, HUMAN_VOICE_PARTS_PROPERTY);
            IInstrumentalPart prelude = null, interlude = null, coda = null;

            if (Song.InstrumentalParts.Count() > 0)
            {
                prelude = Song.InstrumentalParts.Find(p => p.Position.Equals(PRELUDE_PART));
                interlude = Song.InstrumentalParts.Find(p => p.Position.Equals(INTERLUDE_PART));
                coda = Song.InstrumentalParts.Find(p => p.Position.Equals(CODA_PART));
            }

            using (StreamWriter file = new StreamWriter(String.Format(@"{0}\{1}.ly", Path, FileName)))
            {
                await file.WriteLineAsync(CreateHeader());

                if (prelude != null)
                {
                    List<ICode> preludeCode = ExtractCode(prelude.Code);

                    await file.WriteLineAsync(CreateVoices(preludeCode, prelude.Template, PRELUDE_PART));
                    await file.WriteLineAsync(CreatePartScore(prelude.Template, PRELUDE_PART, true));
                }

                await file.WriteLineAsync(CreateVoices(voiceCode, Song.Template.GetRange(0, 4), HUMAN_VOICE_PARTS));
                await file.WriteLineAsync(CreateVoices(organCode, Song.Template.GetRange(4, 4), INSTRUMENT_PARTS));
                await file.WriteLineAsync(CreateLyrics(Song.Stanzas));
                await file.WriteLineAsync(CreateMainScore());

                if (interlude != null)
                {
                    List<ICode> interludeCode = ExtractCode(interlude.Code);

                    await file.WriteLineAsync(CreateVoices(interludeCode, interlude.Template, INTERLUDE_PART));
                    await file.WriteLineAsync(CreatePartScore(interlude.Template, INTERLUDE_PART));
                }

                if (coda != null)
                {
                    List<ICode> codaCode = ExtractCode(coda.Code);

                    await file.WriteLineAsync(CreateVoices(codaCode, coda.Template, CODA_PART));
                    await file.WriteLineAsync(CreatePartScore(coda.Template, CODA_PART));
                }
            }

            using (StreamWriter file = new StreamWriter(String.Format(@"{0}\{1}.bat", Path, FileName)))
            {
                file.WriteLine(String.Format(@"lilypond ""{0}.ly""", FileName));
            }

            Process process = new Process();
            process.StartInfo.WorkingDirectory = Path;
            process.StartInfo.FileName = String.Format("{0}.bat", FileName);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();

            if (DeleteTempFiles)
            {
                File.Delete(String.Format(@"{0}\{1}.ly", Path, FileName));
                File.Delete(String.Format(@"{0}\{1}.bat", Path, FileName));
            }

            return String.Format(@"{0}\{1}.pdf", Path, FileName);
        }

        private string CreateGrandStaff(List<bool> template, string instrument, List<IStanza> stanzas = null, int markedVoice = -1)
        {
            StringBuilder grandStaff = new StringBuilder();
            List<List<string>> staffs = GetVoices(template, instrument);

            grandStaff.AppendLine(@"\new GrandStaff <<");
            grandStaff.AppendLine(CreateStaff(staffs[0], UPPER_STAFF, markedVoice));

            if (markedVoice >= 0)
            {
                stanzas.ForEach((IStanza stanza) => {
                    grandStaff.AppendLine(String.Format(@"\new Lyrics \lyricsto ""{0}"" \lyrics{1}", MARKED_VOICE, ALPHABET[stanza.Number - 1]));
                });
            }

            grandStaff.AppendLine(CreateStaff(staffs[1], LOWER_STAFF));
            grandStaff.AppendLine(@">>");

            return grandStaff.ToString();
        }

        private string CreateHeader()
        {
            StringBuilder header = new StringBuilder();

            header.AppendLine(@"\include ""global-vkg.ily""");
            header.AppendLine(@"\pointAndClickOff");
            header.AppendLine(@"\header{");
            header.AppendLine(String.Format(@"title = ""{0}""", Song.Title));
            header.AppendLine(String.Format(@"othersource = ""{0}""", Song.OtherInformations));

            if (Song.Composer != null)
            {
                header.AppendLine(String.Format(@"composer = ""{0} {1}""", Song.Composer.Name, Song.Composer.Surname));
            }

            if (Song.Arranger != null)
            {
                header.AppendLine(String.Format(@"arranger = ""{0} {1} {2}""", ARRANGER_TEXT, Song.Arranger.Name, Song.Arranger.Surname));
            }

            header.AppendLine(String.Format(@"source = ""{0}""", Song.Source));
            header.AppendLine(@"tagline = """"");
            header.AppendLine(@"}");

            if (Song.Type.Equals("hymn"))
            {
                header.AppendLine(String.Format(@"keyTime = {{ \key {0} \time {1} }}", Key, Time));
            }

            return header.ToString();
        }

        private string CreateLyrics(List<IStanza> stanzas)
        {
            StringBuilder lyrics = new StringBuilder();

            stanzas.ForEach((IStanza stanza) => {
                lyrics.AppendLine(String.Format(@"lyrics{0} = \lyricmode {{", ALPHABET[stanza.Number - 1]));
                lyrics.AppendLine(stanza.Text);
                lyrics.AppendLine("}");
            });

            return lyrics.ToString();
        }

        private string CreateMainScore()
        {
            StringBuilder score = new StringBuilder();
            List<bool> voiceTemplate = Song.Template.GetRange(0, 4);
            List<bool> organTemplate = Song.Template.GetRange(4, 4);

            score.AppendLine(@"\score {");
            score.AppendLine(@"<<");

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

            score.AppendLine(CreateStavesForScore(voiceTemplate, markVoice, HUMAN_VOICE_PARTS));
            score.AppendLine(CreateStavesForScore(organTemplate, markOrgan, INSTRUMENT_PARTS));

            score.AppendLine(@">>");
            score.AppendLine(@"}");

            return score.ToString();
        }

        private string CreatePartScore(List<bool> template, string instrument, bool isPrelude = false)
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
                List<List<string>> staffs = GetVoices(template, instrument);

                if (staffs[0].Count() == 0)
                {
                    score.AppendLine(CreateStaff(staffs[1], LOWER_STAFF));
                }
                else
                {
                    score.AppendLine(CreateStaff(staffs[0], UPPER_STAFF));
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

        private string CreateStaff(List<string> voices, string position, int markedVoice = -1)
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
                        staff.AppendLine(String.Format(@"\new Voice = ""{0}"" << \{1} >>", MARKED_VOICE, voice));
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
                        staff.AppendLine(String.Format(@"\new Voice = ""{0}"" << \voice{1} \{2} >>", MARKED_VOICE, numbers[i], voice));
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

        private string CreateStavesForScore(List<bool> template, int markVoice, string voiceName)
        {
            StringBuilder staves = new StringBuilder();

            if (template.Any(s => s))
            {
                if (template.GetRange(0, 2).Any(s => s) && template.GetRange(2, 2).Any(s => s)) // there is upper and lower parts, ie grandstaff must be used
                {
                    staves.AppendLine(CreateGrandStaff(template, voiceName, Song.Stanzas, markVoice));
                }
                else // use only one staff
                {
                    List<List<string>> staffs = GetVoices(template, voiceName);

                    if (staffs[0].Count() == 0)
                    {
                        staves.AppendLine(CreateStaff(staffs[1], LOWER_STAFF, markVoice));
                    }
                    else
                    {
                        staves.AppendLine(CreateStaff(staffs[0], UPPER_STAFF, markVoice));
                    }

                    if (markVoice >= 0)
                    {
                        Song.Stanzas.ForEach((IStanza stanza) => {
                            staves.AppendLine(String.Format(@"\new Lyrics \lyricsto ""{0}"" \lyrics{1}", MARKED_VOICE, ALPHABET[stanza.Number - 1]));
                        });
                    }
                }
            }

            return staves.ToString();
        }

        private string CreateVoices(List<ICode> codeList, List<bool> template, string instrument)
        {
            StringBuilder voices = new StringBuilder();

            if (template.Any(s => s)) // there is at least one voice
            {
                int j = 0;
                for (int i = 0; i < template.Count; i++)
                {
                    if (template[i])
                    {
                        voices.AppendLine(String.Format(@"{0}{1} = \relative {2} {{", instrument, VOICES[i], codeList[j].VoiceRelative));
                        voices.AppendLine(codeList[j].Voice);
                        voices.AppendLine(@"}");

                        j += 1;
                    }
                }
            }

            return voices.ToString();
        }

        private List<ICode> ExtractCode(string codeString, string mustContain = null)
        {
            Dictionary<string, string> codeRaw = JsonConvert.DeserializeObject<Dictionary<string, string>>(codeString);
            List<string> voices = new List<string>() { "", "", "", "" };
            List<string> relatives = new List<string>() { "", "", "", "" };
            int i = 0;
            List<string> keys;
            
            if (mustContain != null)
            {
                keys = codeRaw.Keys.Where(n => n.Contains(mustContain)).ToList();
            }
            else
            {
                keys = codeRaw.Keys.ToList();
            }

            foreach (string key in keys)
            {
                if (key.Contains("Relative"))
                {
                    if (key.Contains("Soprano")) i = 0;
                    else if (key.Contains("Alto")) i = 1;
                    else if (key.Contains("Tenor")) i = 2;
                    else if (key.Contains("Bass")) i = 3;

                    relatives[i] = key;
                }
                else
                {
                    if (key.Contains("Soprano")) i = 0;
                    else if (key.Contains("Alto")) i = 1;
                    else if (key.Contains("Tenor")) i = 2;
                    else if (key.Contains("Bass")) i = 3;

                    voices[i] = key;
                }
            }

            List<ICode> codeList = new List<ICode>();

            for (i = 0; i < voices.Count; i++)
            {
                if (codeRaw.ContainsKey(voices[i]))
                {
                    codeList.Add(new Code
                    {
                        Voice = codeRaw[voices[i]],
                        VoiceRelative = codeRaw[relatives[i]]
                    });
                }
            }

            return codeList;
        }

        private List<List<string>> GetVoices(List<bool> template, string instrument)
        {
            List<string> staff1 = new List<string>();
            List<string> staff2 = new List<string>();

            for (int i = 0; i < template.Count; i++)
            {
                if (template[i])
                {
                    if (i <= 1)
                    {
                        staff1.Add(String.Concat(instrument, VOICES[i]));
                    }
                    else
                    {
                        staff2.Add(String.Concat(instrument, VOICES[i]));
                    }
                }
            }

            return new List<List<string>> { staff1, staff2 };
        }
        #endregion Methods
    }
}
