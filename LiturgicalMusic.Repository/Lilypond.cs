﻿using System;
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
        /// <summary>
        /// Gets or sets antiphona.
        /// </summary>
        /// <value>The antiphona.</value>
        public string Antiphona { get; set; }

        /// <summary>
        /// Whether antiphona is measured or not.
        /// </summary>
        /// <value><c>true</c> if antiphona is measured, <c>false</c> otherwise.</value>
        public bool AntiphonaMeasured { get; set; }

        /// <summary>
        /// Whether psalm is measured or not.
        /// </summary>
        /// <value><c>true</c> if psalm is measured, <c>false</c> otherwise.</value>
        public bool PsalmMeasured { get; set; }

        /// <summary>
        /// Gets or sets file name for PDF file.
        /// </summary>
        /// <value>The file name.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets whether to delete temporary files or not.
        /// </summary>
        /// <value><c>true</c> if you want to delete temporary files, <c>false</c> otherwise</value>
        public bool DeleteTempFiles { get; set; }

        /// <summary>
        /// Gets or sets song key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets path to folder in which file should be created.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the song.
        /// </summary>
        /// <value>The song.</value>
        public ISong Song { get; set; }

        /// <summary>
        /// Gets or sets the song time.
        /// </summary>
        /// <value>The song time.</value>
        public string Time { get; set; }
        #endregion Properties

        #region Constants
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
        private const string ANTIPHONA = "Antiphona";
        private const string PSALM = "Psalm";
        #endregion Constants

        #region Constructors
        /// <summary>
        /// Initializes new instance of <see cref="Lilypond"/> class.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <param name="path">The path.</param>
        /// <param name="fileName">The name of file.</param>
        /// <param name="deleteTempFiles">Whether to delete temporary files or not.</param>
        public Lilypond(ISong song, string path, string fileName, bool deleteTempFiles)
        {
            this.Song = song;
            this.Path = path;
            this.FileName = fileName;
            this.DeleteTempFiles = deleteTempFiles;

            dynamic code = JsonConvert.DeserializeObject(Song.Code);
            this.Key = code.Key;
            this.Time = code.Time;
            this.Antiphona = code.AntiphonaStanza;

            if (code.PsalmMeasured != null)
            {
                this.PsalmMeasured = code.PsalmMeasured;
            }
            else
            {
                this.PsalmMeasured = true;
            }

            if (code.AntiphonaMeasured != null)
            {
                this.AntiphonaMeasured = code.AntiphonaMeasured;
            }
            else
            {
                this.AntiphonaMeasured = true;
            }
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Creates PDF file containg score.
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateFileAsync()
        {
            IInstrumentalPart prelude = null, interlude = null, coda = null;

            if (Song.InstrumentalParts.Count() > 0)
            {
                prelude = Song.InstrumentalParts.SingleOrDefault(p => p.Position.Equals(PRELUDE_PART));
                interlude = Song.InstrumentalParts.SingleOrDefault(p => p.Position.Equals(INTERLUDE_PART));
                coda = Song.InstrumentalParts.SingleOrDefault(p => p.Position.Equals(CODA_PART));
            }

            using (StreamWriter file = new StreamWriter(String.Format(@"{0}\{1}.ly", Path, FileName)))
            {
                await file.WriteLineAsync(CreateHeader());

                if (prelude != null)
                {
                    IList<ICode> preludeCode = ExtractCode(prelude.Code);

                    await file.WriteLineAsync(CreateVoices(preludeCode, prelude.Template, PRELUDE_PART));
                    await file.WriteLineAsync(CreatePartScore(prelude.Template, PRELUDE_PART, true));
                }

                if (Song.Type.Equals("hymn"))
                {
                    IList<ICode> voiceCode = ExtractCode(Song.Code, HUMAN_VOICE_PARTS_PROPERTY);
                    IList<ICode> organCode = ExtractCode(Song.Code, INSTRUMENT_PARTS_PROPERTY);

                    IList<bool> voiceTemplate = Song.Template.Take(4).ToList();
                    IList<bool> organTemplate = Song.Template.Skip(4).ToList();

                    await file.WriteLineAsync(CreateVoices(voiceCode, voiceTemplate, HUMAN_VOICE_PARTS));
                    await file.WriteLineAsync(CreateVoices(organCode, organTemplate, INSTRUMENT_PARTS));

                    await file.WriteLineAsync(CreateLyrics(Song.Stanzas));

                    await file.WriteLineAsync(CreateMainScore(HUMAN_VOICE_PARTS, INSTRUMENT_PARTS, voiceTemplate, organTemplate, Song.Stanzas.Count));
                }
                else
                {
                    IList<IStanza> antiphona = new List<IStanza> { new Stanza { Text = Antiphona, Number = 1 } };
                    IList<ICode> antiphonaVoiceCode = ExtractCode(Song.Code, String.Concat(ANTIPHONA, HUMAN_VOICE_PARTS_PROPERTY));
                    IList<ICode> antiphonaOrganCode = ExtractCode(Song.Code, String.Concat(ANTIPHONA, INSTRUMENT_PARTS_PROPERTY));
                    IList<ICode> psalmVoiceCode = ExtractCode(Song.Code, String.Concat(PSALM, HUMAN_VOICE_PARTS_PROPERTY));
                    IList<ICode> psalmOrganCode = ExtractCode(Song.Code, String.Concat(PSALM, INSTRUMENT_PARTS_PROPERTY));

                    IList<bool> antiphonaVoiceTemplate = ExtractTemplate(Song.Code, String.Concat(ANTIPHONA, HUMAN_VOICE_PARTS_PROPERTY));
                    IList<bool> antiphonaOrganTemplate = ExtractTemplate(Song.Code, String.Concat(ANTIPHONA, INSTRUMENT_PARTS_PROPERTY));
                    IList<bool> psalmVoiceTemplate = ExtractTemplate(Song.Code, String.Concat(PSALM, HUMAN_VOICE_PARTS_PROPERTY));
                    IList<bool> psalmOrganTemplate = ExtractTemplate(Song.Code, String.Concat(PSALM, INSTRUMENT_PARTS_PROPERTY));

                    await file.WriteLineAsync(CreateVoices(antiphonaVoiceCode, antiphonaVoiceTemplate, String.Concat(ANTIPHONA, HUMAN_VOICE_PARTS)));
                    await file.WriteLineAsync(CreateVoices(antiphonaOrganCode, antiphonaOrganTemplate, String.Concat(ANTIPHONA, INSTRUMENT_PARTS)));

                    await file.WriteLineAsync(CreateVoices(psalmVoiceCode, psalmVoiceTemplate, String.Concat(PSALM, HUMAN_VOICE_PARTS)));
                    await file.WriteLineAsync(CreateVoices(psalmOrganCode, psalmOrganTemplate, String.Concat(PSALM, INSTRUMENT_PARTS)));

                    await file.WriteLineAsync(CreateLyrics(Song.Stanzas));
                    await file.WriteLineAsync(CreateLyrics(antiphona, ANTIPHONA));

                    await file.WriteLineAsync(CreateMainScore(String.Concat(ANTIPHONA, HUMAN_VOICE_PARTS), String.Concat(ANTIPHONA, INSTRUMENT_PARTS), antiphonaVoiceTemplate, antiphonaOrganTemplate, 1, AntiphonaMeasured, ANTIPHONA, true));
                    await file.WriteLineAsync(CreateMainScore(String.Concat(PSALM, HUMAN_VOICE_PARTS), String.Concat(PSALM, INSTRUMENT_PARTS), psalmVoiceTemplate, psalmOrganTemplate, Song.Stanzas.Count, PsalmMeasured));
                }

                if (interlude != null)
                {
                    IList<ICode> interludeCode = ExtractCode(interlude.Code);

                    await file.WriteLineAsync(CreateVoices(interludeCode, interlude.Template, INTERLUDE_PART));
                    await file.WriteLineAsync(CreatePartScore(interlude.Template, INTERLUDE_PART));
                }

                if (coda != null)
                {
                    IList<ICode> codaCode = ExtractCode(coda.Code);

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

        /// <summary>
        /// Creates lilypond grandstaff score.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="instrument">The instrument name.</param>
        /// <param name="stanzas">The stanzas.</param>
        /// <param name="markedVoice">Whether to mark this voice or not.</param>
        /// <returns></returns>
        private string CreateGrandStaff(IList<bool> template, string instrument, bool measured = true, IList<IStanza> stanzas = null, int markedVoice = -1, string lyricsNamePrefix = "")
        {
            StringBuilder grandStaff = new StringBuilder();
            IList<IList<string>> staffs = GetVoices(template, instrument);

            if (measured)
            {
                grandStaff.AppendLine(@"\new GrandStaff <<");
            }
            else
            {
                grandStaff.AppendLine(@"\new GrandStaff \with { \remove ""Time_signature_engraver"" } <<");
            }

            grandStaff.AppendLine(CreateStaff(staffs[0], UPPER_STAFF, measured, markedVoice));

            if (markedVoice >= 0)
            {
                foreach (IStanza stanza in stanzas)
                {
                    grandStaff.AppendLine(String.Format(@"\new Lyrics \lyricsto ""{0}"" \{1}lyrics{2}", MARKED_VOICE, lyricsNamePrefix, ALPHABET[stanza.Number - 1]));
                }
            }

            grandStaff.AppendLine(CreateStaff(staffs[1], LOWER_STAFF, measured));
            grandStaff.AppendLine(@">>");

            return grandStaff.ToString();
        }

        /// <summary>
        /// Creates lilypond headers.
        /// </summary>
        /// <returns></returns>
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
            else
            {
                header.AppendLine(String.Format(@"keyTime = {{ \key {0} }}", Key));
            }

            return header.ToString();
        }

        /// <summary>
        /// Creates lilypond lyrics.
        /// </summary>
        /// <param name="stanzas">The stanzas.</param>
        /// <returns></returns>
        private string CreateLyrics(IList<IStanza> stanzas, string lyricsNamePrefix = "")
        {
            StringBuilder lyrics = new StringBuilder();

            foreach (IStanza stanza in stanzas)
            {
                lyrics.AppendLine(String.Format(@"{0}lyrics{1} = \lyricmode {{", lyricsNamePrefix, ALPHABET[stanza.Number - 1]));
                lyrics.AppendLine(stanza.Text);
                lyrics.AppendLine("}");
            }

            return lyrics.ToString();
        }

        /// <summary>
        /// Creates lilypond score for main part of song.
        /// </summary>
        /// <returns></returns>
        private string CreateMainScore(string humanVoiceName, string instrumentVoiceName, IList<bool> voiceTemplate, IList<bool> organTemplate, int numberOfStanzas, bool measured = true, string lyricsNamePrefix = "", bool alignRight = false)
        {
            StringBuilder score = new StringBuilder();

            if (alignRight)
            {
                score.AppendLine(@"\markup \fill-line {");
                score.AppendLine(@"\hspace #1");
            }

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

            score.AppendLine(CreateStavesForScore(voiceTemplate, markVoice, humanVoiceName, numberOfStanzas, measured, lyricsNamePrefix));
            score.AppendLine(CreateStavesForScore(organTemplate, markOrgan, instrumentVoiceName, numberOfStanzas, measured, lyricsNamePrefix));

            score.AppendLine(@">>");

            if (alignRight)
            {
                score.AppendLine(@"\layout {}");
                score.AppendLine(@"}");
            }

            score.AppendLine(@"}");

            return score.ToString();
        }

        /// <summary>
        /// Creates lilypond score for prelude, interlude and coda.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="instrument">The instrument name.</param>
        /// <param name="isPrelude">Whether this is prelude or not.</param>
        /// <returns></returns>
        private string CreatePartScore(IList<bool> template, string instrument, bool isPrelude = false)
        {
            StringBuilder score = new StringBuilder();

            if (isPrelude)
            {
                score.AppendLine(@"\markup \fill-line {");
                score.AppendLine(@"\hspace #1");
            }

            score.AppendLine(@"\score {");

            if (template.Take(2).Any(s => s) && template.Skip(2).Any(s => s)) // there is upper and lower parts, ie grandstaff must be used
            {
                score.AppendLine(CreateGrandStaff(template, instrument));
            }
            else // use only one staff
            {
                IList<IList<string>> staffs = GetVoices(template, instrument);

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

        /// <summary>
        /// Creates lilypond staff.
        /// </summary>
        /// <param name="voices">The voices.</param>
        /// <param name="position">The staff position, upper or lower.</param>
        /// <param name="markedVoice">Whether to mark a voice.</param>
        /// <returns></returns>
        private string CreateStaff(IList<string> voices, string position, bool measured = true, int markedVoice = -1)
        {
            StringBuilder staff = new StringBuilder();
            int i = 0;
            IList<string> numbers = new List<string> { "One", "Two" };

            if (measured)
            {
                staff.AppendLine(String.Format(@"\new Staff = ""{0}"" <<", position));
            }
            else
            {
                staff.AppendLine(String.Format(@"\new Staff = ""{0}"" \with {{ \remove ""Time_signature_engraver"" }} <<", position));
            }

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

        /// <summary>
        /// Creates lilypond staves for score.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="markVoice">Whether to mark a voice.</param>
        /// <param name="voiceName">The name of a voice.</param>
        /// <returns></returns>
        private string CreateStavesForScore(IList<bool> template, int markVoice, string voiceName, int numberOfStanzas, bool measured, string lyricsNamePrefix = "")
        {
            StringBuilder staves = new StringBuilder();

            if (template.Any(s => s))
            {
                if (template.Take(2).Any(s => s) && template.Skip(2).Any(s => s)) // there is upper and lower parts, ie grandstaff must be used
                {
                    staves.AppendLine(CreateGrandStaff(template, voiceName, measured, Song.Stanzas, markVoice, lyricsNamePrefix));
                }
                else // use only one staff
                {
                    IList<IList<string>> staffs = GetVoices(template, voiceName);

                    if (staffs[0].Count() == 0)
                    {
                        staves.AppendLine(CreateStaff(staffs[1], LOWER_STAFF, measured, markVoice));
                    }
                    else
                    {
                        staves.AppendLine(CreateStaff(staffs[0], UPPER_STAFF, measured, markVoice));
                    }

                    if (markVoice >= 0)
                    {
                        for (int i = 0; i < numberOfStanzas; i++)
                        {
                            staves.AppendLine(String.Format(@"\new Lyrics \lyricsto ""{0}"" \{1}lyrics{2}", MARKED_VOICE, lyricsNamePrefix, ALPHABET[i]));
                        }
                    }
                }
            }

            return staves.ToString();
        }

        /// <summary>
        /// Creates lilypond voices.
        /// </summary>
        /// <param name="codeList">The list of codes representing voices.</param>
        /// <param name="template">The template.</param>
        /// <param name="instrument">The instrument name.</param>
        /// <returns></returns>
        private string CreateVoices(IList<ICode> codeList, IList<bool> template, string instrument)
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

        /// <summary>
        /// Extracts code from string.
        /// </summary>
        /// <param name="codeString">The string which represents code.</param>
        /// <param name="mustContain">Whether key must contain a string.</param>
        /// <returns></returns>
        private IList<ICode> ExtractCode(string codeString, string mustContain = null)
        {
            Dictionary<string, string> codeRaw = JsonConvert.DeserializeObject<Dictionary<string, string>>(codeString);
            IList<string> voices = new List<string>() { "", "", "", "" };
            IList<string> relatives = new List<string>() { "", "", "", "" };
            int i = 0;
            IList<string> keys;
            
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

            IList<ICode> codeList = new List<ICode>();

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

        /// <summary>
        /// Extracts template from code.
        /// </summary>
        /// <param name="codeString">The string which represents code.</param>
        /// <param name="mustContain">Whether key must contain a string.</param>
        /// <returns></returns>
        private IList<bool> ExtractTemplate(string codeString, string mustContain = null)
        {
            Dictionary<string, string> codeRaw = JsonConvert.DeserializeObject<Dictionary<string, string>>(codeString);
            IList<bool> template = new List<bool> { false, false, false, false };
            IList<string> keys;

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
                if (key.Contains("Soprano")) template[0] = true;
                else if (key.Contains("Alto")) template[1] = true;
                else if (key.Contains("Tenor")) template[2] = true;
                else if (key.Contains("Bass")) template[3] = true;
            }

            return template;
        }

        /// <summary>
        /// Gets and creates voice names from template and instrument name.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="instrument">The instrument name.</param>
        /// <returns></returns>
        private IList<IList<string>> GetVoices(IList<bool> template, string instrument)
        {
            IList<string> staff1 = new List<string>();
            IList<string> staff2 = new List<string>();

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

            return new List<IList<string>> { staff1, staff2 };
        }
        #endregion Methods
    }
}
