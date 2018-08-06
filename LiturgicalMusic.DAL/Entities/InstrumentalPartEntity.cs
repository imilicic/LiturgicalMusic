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
    [Table("InstrumentalParts")]
    public class InstrumentalPartEntity : IEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets template of instrumental part.
        /// </summary>
        /// <value>The template of instrumental part.</value>
        [Required]
        public int Template { get; set; }

        /// <summary>
        /// Gets or sets instrumental part position in song.
        /// </summary>
        /// <value><c>prelude</c>, <c>interlude</c> or <c>coda</c></value>
        [Required,MaxLength(20)]
        public string Position { get; set; }

        /// <summary>
        /// Gets or sets type of instrumental part.
        /// </summary>
        /// <value><c>hymn</c> or <c>psalm</c></value>
        [Required, MaxLength(20)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets code of instrumental part.
        /// </summary>
        /// <value>The code.</value>
        [Required, MaxLength(-1)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets ID of song for which this part is written.
        /// </summary>
        /// <value>The ID of song.</value>
        [Required]
        public int SongId { get; set; }

        /// <summary>
        /// Gets or sets song to which part belongs to.
        /// </summary>
        /// <value>The song.</value>
        public SongEntity Song { get; set; }
        #endregion Properties
    }
}
