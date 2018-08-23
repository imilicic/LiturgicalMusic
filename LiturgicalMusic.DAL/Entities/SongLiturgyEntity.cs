using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.DAL
{
    [Table("SongLiturgies")]
    public class SongLiturgyEntity
    {
        [Key, Column(Order = 0)]
        public int SongId { get; set; }
        [Key, Column(Order = 1)]
        public int LiturgyId { get; set; }

        public SongEntity Song { get; set; }
        public LiturgyEntity Liturgy { get; set; }
    }
}
