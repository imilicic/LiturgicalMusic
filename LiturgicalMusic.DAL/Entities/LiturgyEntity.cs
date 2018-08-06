using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.DAL
{
    [Table("LiturgyCategories")]
    public class LiturgyEntity
    {
        #region Constructors
        public LiturgyEntity()
        {
            this.Songs = new HashSet<SongEntity>();
        }
        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of category.
        /// </summary>
        /// <value>The name.</value>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the songs having this category.
        /// </summary>
        /// <value>This songs.</value>
        public ICollection<SongEntity> Songs { get; set; }
        #endregion Properties
    }
}
