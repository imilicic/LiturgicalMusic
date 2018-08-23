using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiturgicalMusic.DAL
{
    [Table("SongThemes")]
    public class SongThemeEntity
    {
        [Key, Column(Order = 0)]
        public int SongId { get; set; }
        [Key, Column(Order = 1)]
        public int ThemeId { get; set; }

        public SongEntity Song { get; set; }
        public ThemeEntity Theme { get; set; }
    }
}
