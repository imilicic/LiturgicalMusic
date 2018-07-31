using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.DAL
{
    [Table("Stanzas")]
    public class StanzaEntity
    {
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required, MaxLength(-1)]
        public string Text { get; set; }

        [Required]
        public int SongId { get; set; }
        public SongEntity Song { get; set; }
    }
}
