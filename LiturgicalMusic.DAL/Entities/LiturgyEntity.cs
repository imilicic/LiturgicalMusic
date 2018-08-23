using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LiturgicalMusic.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.DAL
{
    [Table("LiturgyCategories")]
    public class LiturgyEntity : IEntity
    {
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
        public ICollection<SongLiturgyEntity> Songs { get; set; }
        #endregion Properties
    }
}
