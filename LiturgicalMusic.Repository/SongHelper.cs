using LiturgicalMusic.Common;
using LiturgicalMusic.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Repository
{
    public class SongHelper
    {
        private const string PATH_TO_WEB_API = @"E:\vs projects\LiturgicalMusic\LiturgicalMusic.WebAPI";
        private const string PDF_ASSETS_DIR = @"app\assets\pdf";
        private const string SOURCE_DIR = "src";
        private const string TEMP_DIR = "temp";

        public async static Task CreatePdfAsync(ISong song, string tempFileName, bool deleteTempFiles)
        {
            string tempDir = String.Format(@"{0}\{1}", PATH_TO_WEB_API, TEMP_DIR);
            string srcDir = String.Format(@"{0}\{1}", PATH_TO_WEB_API, SOURCE_DIR);
            Lilypond lyGenerator = new Lilypond(song, tempDir, tempFileName, deleteTempFiles);
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

        public static string CreateIncludeString(IOptions options)
        {
            string include = "";

            if (options.Composer)
            {
                include = String.Concat(include, "Composer,");
            }

            if (options.Arranger)
            {
                include = String.Concat(include, "Arranger,");
            }

            if (options.Stanzas)
            {
                include = String.Concat(include, "Stanzas,");
            }

            if (options.InstrumentalParts)
            {
                include = String.Concat(include, "InstrumentalParts,");
            }

            if (options.LiturgyCategories)
            {
                include = String.Concat(include, "LiturgyCategories,");
            }

            if (options.ThemeCategories)
            {
                include = String.Concat(include, "ThemeCategories,");
            }

            return include;
        }

        public static string Hash(string text)
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

        public static string SongFileName(ISong song)
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

        public async static Task UpdatePdfAsync(ISong song, string tempFileName, string oldSongFileName, bool deleteTempFiles)
        {
            string newSongFileName = SongFileName(song);

            if (oldSongFileName != newSongFileName)
            {
                string delete = String.Format(@"{0}\{1}\{2}\{3}.pdf", PATH_TO_WEB_API, SOURCE_DIR, PDF_ASSETS_DIR, oldSongFileName);
                File.Delete(delete);
            }

            await CreatePdfAsync(song, tempFileName, deleteTempFiles);
        }
    }
}
