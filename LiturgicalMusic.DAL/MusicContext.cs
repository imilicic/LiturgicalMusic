using System.Data.Entity;

namespace LiturgicalMusic.DAL
{
    public class MusicContext : DbContext
    {
        #region Properties
        /// <summary>
        /// Gets or sets composers.
        /// </summary>
        /// <value>The composers.</value>
        public DbSet<ComposerEntity> Composers { get; set; }

        /// <summary>
        /// Gets or sets songs.
        /// </summary>
        /// <value>The songs.</value>
        public DbSet<SongEntity> Songs { get; set; }

        /// <summary>
        /// Gets or sets theme categories.
        /// </summary>
        /// <value>The theme categories.</value>
        public DbSet<ThemeEntity> ThemeCategories { get; set; }

        /// <summary>
        /// Gets or sets liturgy categories.
        /// </summary>
        /// <value>The liturgy categories.</value>
        public DbSet<LiturgyEntity> LiturgyCategories { get; set; }

        /// <summary>
        /// Gets or sets instrumental parts.
        /// </summary>
        /// <value>The instrumental parts.</value>
        public DbSet<InstrumentalPartEntity> InstrumentalParts { get; set; }

        /// <summary>
        /// Gets or sets song and liturgy categories.
        /// </summary>
        public DbSet<SongLiturgyEntity> SongLiturgyCategories { get; set; }

        /// <summary>
        /// Gets or sets song and theme categories.
        /// </summary>
        public DbSet<SongThemeEntity> SongThemeCategories { get; set; }

        /// <summary>
        /// Gets or sets stanzas.
        /// </summary>
        /// <value>The stanzas.</value>
        public DbSet<StanzaEntity> Stanzas { get; set; }
        #endregion Properties
    }
}
