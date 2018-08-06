using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.Model.Common
{
    public interface ISong
    {
        #region Properties
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets song title.
        /// </summary>
        /// <value>The song title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets song template.
        /// </summary>
        /// <value>The song template.</value>
        IList<bool> Template { get; set; }

        /// <summary>
        /// Gets or sets the song type.
        /// </summary>
        /// <value><c>hymn</c> or <c>psalm</c></value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets code.
        /// </summary>
        /// <value>The code.</value>
        string Code { get; set; }

        /// <summary>
        /// Gets or sets source of a song.
        /// </summary>
        /// <value>The source.</value>
        string Source { get; set; }

        /// <summary>
        /// Gets or sets other informations of a song.
        /// </summary>
        /// <value>The other informations.</value>
        string OtherInformations { get; set; }

        /// <summary>
        /// Gets or sets stanzas.
        /// </summary>
        /// <value>The stanzas.</value>
        IList<IStanza> Stanzas { get; set; }

        /// <summary>
        /// Gets or sets the composer.
        /// </summary>
        /// <value>The composer.</value>
        IComposer Composer { get; set; }

        /// <summary>
        /// Gets or sets the arranger.
        /// </summary>
        /// <value>The arranger.</value>
        IComposer Arranger { get; set; }

        /// <summary>
        /// Gets or sets instrumental parts.
        /// </summary>
        /// <value>The instrumental parts.</value>
        IList<IInstrumentalPart> InstrumentalParts { get; set; }

        /// <summary>
        /// Gets or sets theme categories.
        /// </summary>
        /// <value>The theme categories.</value>
        IList<int> ThemeCategories { get; set; }

        /// <summary>
        /// Gets or sets liturgy categories.
        /// </summary>
        /// <value>The liturgy categories.</value>
        IList<int> LiturgyCategories { get; set; }
        #endregion Properties
    }
}
