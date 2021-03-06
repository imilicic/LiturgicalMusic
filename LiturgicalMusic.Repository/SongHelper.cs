﻿using LiturgicalMusic.Common;
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
        #region Constants
        private const string PATH_TO_PDF_FILES = @"E:\vs projects\LiturgicalMusic\LiturgicalMusic.Angular";
        private const string PDF_ASSETS_DIR = @"app\assets\pdf";
        private const string SOURCE_DIR = "src";
        private const string TEMP_DIR = "temp";
        #endregion Constants

        #region Methods
        /// <summary>
        /// Starts creating PDF file and moves it to another folder.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <param name="tempFileName">The file name for temporary files.</param>
        /// <param name="deleteTempFiles">Whether to delete temporary files or not.</param>
        /// <returns></returns>
        public async static Task CreatePdfAsync(ISong song, string tempFileName, bool deleteTempFiles)
        {
            string tempDir = String.Format(@"{0}\{1}", PATH_TO_PDF_FILES, TEMP_DIR);
            string srcDir = String.Format(@"{0}\{1}", PATH_TO_PDF_FILES, SOURCE_DIR);
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

        /// <summary>
        /// Deletes PDF score file of a song.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
        public static bool DeletePdf(ISong song)
        {
            string srcDir = String.Format(@"{0}\{1}", PATH_TO_PDF_FILES, SOURCE_DIR);
            string songFileName = SongFileName(song);
            string pdfFilePath = String.Format(@"{0}\{1}\{2}.pdf", srcDir, PDF_ASSETS_DIR, songFileName);

            if (File.Exists(pdfFilePath))
            {
                File.Delete(pdfFilePath);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates hash from string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates song file name.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates PDF file.
        /// </summary>
        /// <param name="song">The song.</param>
        /// <param name="tempFileName">The file name for temporary files.</param>
        /// <param name="oldSongFileName">The file name before update.</param>
        /// <param name="deleteTempFiles">Whether to delete temporary files or not.</param>
        /// <returns></returns>
        public async static Task UpdatePdfAsync(ISong song, string tempFileName, string oldSongFileName, bool deleteTempFiles)
        {
            string newSongFileName = SongFileName(song);

            if (oldSongFileName != newSongFileName)
            {
                string delete = String.Format(@"{0}\{1}\{2}\{3}.pdf", PATH_TO_PDF_FILES, SOURCE_DIR, PDF_ASSETS_DIR, oldSongFileName);
                File.Delete(delete);
            }

            await CreatePdfAsync(song, tempFileName, deleteTempFiles);
        }

        #endregion Methods
    }
}
