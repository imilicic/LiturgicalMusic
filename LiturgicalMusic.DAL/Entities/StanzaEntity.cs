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
        [Key, Column(Order = 0)]
        public int Number { get; set; }
        public string Text { get; set; }
        [Key, Column(Order = 1)]
        public SongEntity Song { get; set; }
    }
}
