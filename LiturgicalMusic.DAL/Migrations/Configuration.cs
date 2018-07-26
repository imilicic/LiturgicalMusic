namespace LiturgicalMusic.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LiturgicalMusic.DAL.MusicContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LiturgicalMusic.DAL.MusicContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Composers.AddOrUpdate(
                new ComposerEntity() { Id = 1, Name = "Ðuro", Surname = "Tomašiæ" },
                new ComposerEntity() { Id = 2, Name = "Franjo", Surname = "Dugan" },
                new ComposerEntity() { Id = 3, Name = "Dejan", Surname = "Bubalo" }
                );
            
            context.Songs.AddOrUpdate(
                new SongEntity() { Title = "Oèe naš dobri", Template = 15, Code = "{ \"OrganSoprano\":\"\\clef \"treble\" \\keyTime es2 \\p d4 es4 |\n  f2 f2 |\n  f4 g4 as4 g4 |\n  f2 es2 |\n  bes'2 \\f as4 bes4 |\n  c2 bes2 |\n  f4 g4 as4 g4 |\n  f2 es2 |\n  f2 \\mf f4 as4 |\n  g2 es2 |\n  g4 \\< as4 bes4 as4 \\! |\n  g2 \\> f2 |\n  bes2 \\! as4 g4 |\n  f2 es2 \\bar \"||\"\",\"OrganSopranoRelative\":\"c'\",\"OrganAlto\":\"\\clef \"treble\" \\keyTime bes2 b4 c4 |\n  d2 c2 |\n  d4 es4 f4 es4 ~ |\n  es4 d4 bes2 |\n  es1 ~ |\n  es1 |\n  f4 es8 [ d8 ] c4 bes4 |\n  as2 g2 |\n  c4 bes4 as4 c4 |\n  b2 c2 |\n  bes4 es4 d4 c8 [ d8 ] |\n  es2 d2 |\n  d4 g4 f4 es4 ~ |\n  es4 d4 bes2 \\bar \"||\"\",\"OrganAltoRelative\":\"c'\",\"OrganTenor\":\"\\clef \"bass\" \\keyTime g'1 |\n  bes2 a2 |\n  bes2 c2 ~ |\n  c4 bes8 [ as8 ] g2 |\n  g2 as4 des4 |\n  c4 as4 g2 |\n  bes4 es,2 ~ es8 [ d8 ] |\n  c4 d4 es2 |\n  c2. f8 [ es8 ] |\n  d2 c2 |\n  es2 f8 [ g8 ] as4 |\n  bes4 c4 d2 |\n  bes4 es4 c2 ~ |\n  c4 bes8 [ as8 ] g2 \\bar \"||\"\",\"OrganTenorRelative\":\"c\",\"OrganBass\":\"\\clef \"bass\" \\keyTime es2 g4 c,4 |\n  bes4 d4 f2 |\n  bes8 [ as8 ] g4 f4 c4 |\n  as4 bes4 es,2 |\n  es'4 des4 c4 g4 |\n  as4 c4 es2 |\n  d4 c8 [ bes8 ] as4 es4 |\n  f2 c'2 |\n  as4 g4 f4 f4 |\n  g2 as2 |\n  es4 c'4 bes4 f4 |\n  g4 a4 bes2 |\n  g'4 es4 f4 c8 [ bes8 ] |\n  as4 bes4 es2 \\bar \"||\"\",\"OrganBassRelative\":\"c\",\"Key\":\"es \\major\",\"Time\":\"2/2\"}", Type = "hymn", OtherParts = false, Source = "PGPN 224", ArrangerId = 2 }
                );
        }
    }
}
