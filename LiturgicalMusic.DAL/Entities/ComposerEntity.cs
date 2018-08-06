using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LiturgicalMusic.Common;

namespace LiturgicalMusic.DAL
{
    [Table("Composers")]
    public class ComposerEntity : IEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets composer name.
        /// </summary>
        /// <value>The composer name.</value>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets composer surname.
        /// </summary>
        /// <value>The composer surname.</value>
        [Required, MaxLength(50)]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets composed songs by this composer.
        /// </summary>
        /// <value>The composed songs.</value>
        [InverseProperty("Composer")]
        public ICollection<SongEntity> ComposedSongs { get; set; }

        /// <summary>
        /// Gets or sets arranged songs by this composer.
        /// </summary>
        /// <value>The arranged songs.</value>
        [InverseProperty("Arranger")]
        public ICollection<SongEntity> ArrangedSongs { get; set; }
        #endregion Properties
    }
}
