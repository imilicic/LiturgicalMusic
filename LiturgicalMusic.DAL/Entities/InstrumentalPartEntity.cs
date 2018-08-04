﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.Common;

namespace LiturgicalMusic.DAL
{
    [Table("InstrumentalParts")]
    public class InstrumentalPartEntity : IEntity
    {
        public int Id { get; set; }

        [Required]
        public int Template { get; set; }

        [Required,MaxLength(20)]
        public string Position { get; set; }

        [Required, MaxLength(20)]
        public string Type { get; set; }

        [Required, MaxLength(-1)]
        public string Code { get; set; }

        [Required]
        public int SongId { get; set; }
        public SongEntity Song { get; set; }
    }
}
