using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model;
using LiturgicalMusic.Model.Common;
using Newtonsoft.Json;

namespace LiturgicalMusic.Repository
{
    public class Lilypond
    {
        public static string CreateHeader(ISong song)
        {
            StringBuilder header = new StringBuilder();

            header.Append(@"\include ""global-vkg.ily""");
            header.Append(@"\pointAndClickOff");
            header.Append(@"\header{");
            header.AppendFormat(@"title = ""{0}""", song.Title);
            header.AppendFormat(@"othersource = ""{0}""", song.OtherInformation);

            if (song.Composer != null)
            {
                header.AppendFormat(@"composer = ""{0} {1}""", song.Composer.Name, song.Composer.Surname);
            }

            if (song.Arranger != null)
            {
                header.AppendFormat(@"arranger = ""Harmonizacija: {0} {1}""", song.Arranger.Name, song.Arranger.Surname);
            }

            header.AppendFormat(@"source = ""{0}""", song.Source);
            header.Append(@"tagline = """"");
            header.Append(@"}");

            return header.ToString();
        }

        public static string CreateVoices(ISong song)
        {
            StringBuilder voices = new StringBuilder();
            ICode code = JsonConvert.DeserializeObject<Code>(song.Code);
            List<bool> template = song.Template.Select(c => Convert.ToBoolean(Convert.ToInt16(c.ToString()))).ToList();
            List<bool> voiceTemplate = template.GetRange(0, 4);
            List<bool> organTemplate = template.GetRange(4, 4);

            if (song.Type == "hymn")
            {
                voices.AppendFormat(@"keyTime = {{ \key {0} \time {1} }}", code.Key, code.Time);
            }

            if (voiceTemplate.Any(s => s)) // there is at least one human voice
            {
                if (voiceTemplate[0])
                {
                    voices.AppendFormat(@"voiceS = \relative {0} {{", code.VoiceSopranoRelative);
                    voices.Append(code.VoiceSoprano);
                    voices.Append(@"}");
                }
                if (voiceTemplate[1])
                {
                    voices.AppendFormat(@"voiceA = \relative {0} {{", code.VoiceAltoRelative);
                    voices.Append(code.VoiceAlto);
                    voices.Append(@"}");
                }
                if (voiceTemplate[2])
                {
                    voices.AppendFormat(@"voiceT = \relative {0} {{", code.VoiceTenorRelative);
                    voices.Append(code.VoiceTenor);
                    voices.Append(@"}");
                }
                if (voiceTemplate[3])
                {
                    voices.AppendFormat(@"voiceB = \relative {0} {{", code.VoiceBassRelative);
                    voices.Append(code.VoiceBass);
                    voices.Append(@"}");
                }
            }

            if (organTemplate.Any(s => s)) // there is at least one organ voice
            {
                if (organTemplate[0])
                {
                    voices.AppendFormat(@"organS = \relative {0} {{", code.OrganSopranoRelative);
                    voices.Append(code.OrganSoprano);
                    voices.Append(@"}");
                }
                if (organTemplate[1])
                {
                    voices.AppendFormat(@"organA = \relative {0} {{", code.OrganAltoRelative);
                    voices.Append(code.OrganAlto);
                    voices.Append(@"}");
                }
                if (organTemplate[2])
                {
                    voices.AppendFormat(@"organT = \relative {0} {{", code.OrganTenorRelative);
                    voices.Append(code.OrganTenor);
                    voices.Append(@"}");
                }
                if (organTemplate[3])
                {
                    voices.AppendFormat(@"organB = \relative {0} {{", code.OrganBassRelative);
                    voices.Append(code.OrganBass);
                    voices.Append(@"}");
                }
            }

            return voices.ToString();
        }

        public static string CreateGrandStaff(List<bool> template, string instrument, int markedVoice = -1)
        {
            StringBuilder grandStaff = new StringBuilder();
            List<List<string>> staffs = GetVoices(template, instrument);

            grandStaff.Append(@"\new GrandStaff <<");
            grandStaff.Append(CreateStaff(staffs[0], markedVoice));
            grandStaff.Append(CreateStaff(staffs[1]));
            grandStaff.Append(@">>");

            return grandStaff.ToString();
        }

        public static string CreateStaff(List<string> voices, int markedVoice = -1)
        {
            StringBuilder staff = new StringBuilder();
            int i = 0;
            List<string> numbers = new List<string> { "One", "Two" };

            staff.Append(@"\new Staff <<");

            foreach (string voice in voices)
            {
                if (voices.Count() == 1)
                {
                    if (markedVoice == 0)
                    {
                        staff.AppendFormat(@"\new Voice = ""voiceS"" << \{0} >>", voice);
                    }
                    else
                    {
                        staff.AppendFormat(@"\new Voice << \{0} >>", voice);
                    }
                }
                else
                {
                    if (markedVoice == i)
                    {
                        staff.AppendFormat(@"\new Voice = ""voiceS"" << \voice{0} \{1} >>", numbers[i], voice);
                    }
                    else
                    {
                        staff.AppendFormat(@"\new Voice << \voice{0} \{1} >>", numbers[i], voice);
                    }
                    i++;
                }
            }

            staff.Append(">>");
            return staff.ToString();
        }

        public static string CreateScore(ISong song)
        {
            StringBuilder score = new StringBuilder();
            List<bool> template = song.Template.Select(c => Convert.ToBoolean(Convert.ToInt16(c.ToString()))).ToList();
            List<bool> voiceTemplate = template.GetRange(0, 4);
            List<bool> organTemplate = template.GetRange(4, 4);

            score.Append(@"\score {");

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
                score.Append(@"<<");
            }

            if (voiceTemplate.Any(s => s)) // part for voice
            {
                if (voiceTemplate.GetRange(0, 2).Any(s => s) && voiceTemplate.GetRange(2, 2).Any(s => s)) // there is upper and lower parts, ie grandstaff must be used
                {
                    score.Append(CreateGrandStaff(voiceTemplate, "voice", markVoice));
                }
                else // use only one staff
                {
                    List<List<string>> staffs = Lilypond.GetVoices(voiceTemplate, "voice");

                    if (staffs[0].Count() == 0)
                    {
                        score.Append(CreateStaff(staffs[1], markVoice));
                    }
                    else
                    {
                        score.Append(CreateStaff(staffs[0], markVoice));
                    }
                }
            }

            if (organTemplate.Any(s => s)) // part for organ
            {
                if (organTemplate.GetRange(0, 2).Any(s => s) && organTemplate.GetRange(2, 2).Any(s => s)) // there is upper and lower parts, ie grandstaff must be used
                {
                    score.Append(CreateGrandStaff(organTemplate, "organ", markOrgan));
                }
                else // use only one staff
                {
                    List<List<string>> staffs = Lilypond.GetVoices(organTemplate, "organ");

                    if (staffs[0].Count() == 0)
                    {
                        score.Append(CreateStaff(staffs[1], markOrgan));
                    }
                    else
                    {
                        score.Append(CreateStaff(staffs[0], markOrgan));
                    }
                }
            }

            if (voiceTemplate.Any(s => s) && organTemplate.Any(s => s)) // at least one voice in human voice and organ voice, ie. two separate staffs
            {
                score.Append(@">>");
            }

            score.Append(@"}");

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
