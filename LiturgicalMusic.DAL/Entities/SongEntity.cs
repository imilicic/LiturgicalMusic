using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Common;

namespace LiturgicalMusic.DAL
{
    [Table("Songs")]
    public class SongEntity : IEntity
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
        [Required, MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets song template.
        /// </summary>
        /// <value>The song template.</value>
        [Required]
        public int Template { get; set; }

        /// <summary>
        /// Gets or sets the song type.
        /// </summary>
        /// <value><c>hymn</c> or <c>psalm</c></value>
        [Required, MaxLength(20)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets code.
        /// </summary>
        /// <value>The code.</value>
        [Required, MaxLength(-1)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets whether song has other parts or not.
        /// </summary>
        /// <value><c>true</c> if song has other parts, <c>false</c> otherwise.</value>
        [Required]
        public bool OtherParts { get; set; }

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
        /// Gets or sets the composer ID.
        /// </summary>
        /// <value>The composer ID.</value>
        public int? ComposerId { get; set; }

        /// <summary>
        /// Gets or sets the arranger ID.
        /// </summary>
        /// <value>The arranger ID.</value>
        public int? ArrangerId { get; set; }

        /// <summary>
        /// Gets or sets the composer.
        /// </summary>
        /// <value>The composer.</value>
        public ComposerEntity Composer { get; set; }

        /// <summary>
        /// Gets or sets the arranger.
        /// </summary>
        /// <value>The arranger.</value>
        public ComposerEntity Arranger { get; set; }

        /// <summary>
        /// Gets or sets instrumental parts.
        /// </summary>
        /// <value>The instrumental parts.</value>
        public ICollection<InstrumentalPartEntity> InstrumentalParts { get; set; }

        /// <summary>
        /// Gets or sets liturgy categories.
        /// </summary>
        /// <value>The liturgy categories.</value>
        public ICollection<SongLiturgyEntity> LiturgyCategories { get; set; }

        /// <summary>
        /// Gets or sets theme categories.
        /// </summary>
        /// <value>The theme categories.</value>
        public ICollection<SongThemeEntity> ThemeCategories { get; set; }

        /// <summary>
        /// Gets or sets stanzas.
        /// </summary>
        /// <value>The stanzas.</value>
        public ICollection<StanzaEntity> Stanzas { get; set; }
        #endregion Properties
    }
}
