using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Model.Common;

namespace LiturgicalMusic.Model
{
    public class Song : ISong
    {
        #region Properties
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets song title.
        /// </summary>
        /// <value>The song title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets song template.
        /// </summary>
        /// <value>The song template.</value>
        public IList<bool> Template { get; set; }

        /// <summary>
        /// Gets or sets the song type.
        /// </summary>
        /// <value><c>hymn</c> or <c>psalm</c></value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets source of a song.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets other informations of a song.
        /// </summary>
        /// <value>The other informations.</value>
        public string OtherInformations { get; set; }

        /// <summary>
        /// Gets or sets stanzas.
        /// </summary>
        /// <value>The stanzas.</value>
        public IList<IStanza> Stanzas { get; set; }

        /// <summary>
        /// Gets or sets the composer.
        /// </summary>
        /// <value>The composer.</value>
        public IComposer Composer { get; set; }

        /// <summary>
        /// Gets or sets the arranger.
        /// </summary>
        /// <value>The arranger.</value>
        public IComposer Arranger { get; set; }

        /// <summary>
        /// Gets or sets instrumental parts.
        /// </summary>
        /// <value>The instrumental parts.</value>
        public IList<IInstrumentalPart> InstrumentalParts { get; set; }

        /// <summary>
        /// Gets or sets theme categories.
        /// </summary>
        /// <value>The theme categories.</value>
        public IList<int> ThemeCategories { get; set; }

        /// <summary>
        /// Gets or sets liturgy categories.
        /// </summary>
        /// <value>The liturgy categories.</value>
        public IList<int> LiturgyCategories { get; set; }
        #endregion Properties
    }
}
