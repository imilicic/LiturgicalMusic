using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LiturgicalMusic.DAL
{
    [Table("Composers")]
    public class ComposerEntity : IEntity
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Surname { get; set; }

        [InverseProperty("Composer")]
        public ICollection<SongEntity> ComposedSongs { get; set; }

        [InverseProperty("Arranger")]
        public ICollection<SongEntity> ArrangedSongs { get; set; }
    }
}
