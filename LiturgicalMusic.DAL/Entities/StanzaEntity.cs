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
    [Table("Stanzas")]
    public class StanzaEntity : IEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets stanza number.
        /// </summary>
        /// <value>The number.</value>
        [Required]
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets stanza text.
        /// </summary>
        /// <value>The text.</value>
        [Required, MaxLength(-1)]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets song ID.
        /// </summary>
        /// <value>The song ID.</value>
        [Required]
        public int SongId { get; set; }

        /// <summary>
        /// Gets or sets the song.
        /// </summary>
        /// <value>The song.</value>
        public SongEntity Song { get; set; }
        #endregion Properties
    }
}
