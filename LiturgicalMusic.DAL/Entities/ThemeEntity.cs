using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.DAL
{
    [Table("ThemeCategories")]
    public class ThemeEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets category name.
        /// </summary>
        /// <value>The name.</value>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets songs having this category.
        /// </summary>
        /// <value>The songs.</value>
        public ICollection<SongThemeEntity> Songs { get; set; }
        #endregion Properties
    }
}
