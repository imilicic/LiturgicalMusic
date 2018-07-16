using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.DAL
{
    [Table("Songs")]
    public class SongEntity
    {
        public SongEntity()
        {
            this.ThemeCategories = new HashSet<ThemeEntity>();
            this.LiturgyCategories = new HashSet<LiturgyEntity>();
        }

        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public int Template { get; set; }

        [Required, MaxLength(20)]
        public string Type { get; set; }

        [Required, MaxLength(-1)]
        public string Code { get; set; }

        [Required]
        public bool OtherParts { get; set; }
        public string Source { get; set; }
        public string OtherInformations { get; set; }

        public ComposerEntity Composer { get; set; }
        public ComposerEntity Arranger { get; set; }

        public ICollection<ThemeEntity> ThemeCategories { get; set; }
        public ICollection<LiturgyEntity> LiturgyCategories { get; set; }

        public ICollection<InstrumentalPartEntity> InstrumentalParts { get; set; }
        public ICollection<StanzaEntity> Stanzas { get; set; }
    }
}
