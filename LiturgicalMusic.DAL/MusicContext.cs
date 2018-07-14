using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LiturgicalMusic.DAL
{
    public class MusicContext : DbContext
    {
        public DbSet<ComposerEntity> Composers { get; set; }
        public DbSet<SongEntity> Songs { get; set; }
        public DbSet<ThemeEntity> ThemeCategories { get; set; }
        public DbSet<LiturgyEntity> LiturgyCategories { get; set; }
    }
}
