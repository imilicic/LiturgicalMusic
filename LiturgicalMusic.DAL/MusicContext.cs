using System.Data.Entity;

namespace LiturgicalMusic.DAL
{
    public class MusicContext : DbContext
    {
        public DbSet<ComposerEntity> Composers { get; set; }
        public DbSet<SongEntity> Songs { get; set; }
        public DbSet<ThemeEntity> ThemeCategories { get; set; }
        public DbSet<LiturgyEntity> LiturgyCategories { get; set; }
        public DbSet<InstrumentalPartEntity> InstrumentalParts { get; set; }
        public DbSet<StanzaEntity> Stanzas { get; set; }
    }
}
