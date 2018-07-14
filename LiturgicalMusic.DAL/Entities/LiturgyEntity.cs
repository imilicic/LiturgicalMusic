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
        public LiturgyEntity()
        {
            this.Songs = new HashSet<SongEntity>();
        }

        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<SongEntity> Songs { get; set; }
    }
}
