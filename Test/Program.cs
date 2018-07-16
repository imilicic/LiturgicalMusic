using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LiturgicalMusic.Model;
using LiturgicalMusic.Model.Common;
using LiturgicalMusic.Repository;
using LiturgicalMusic.Repository.Common;
using AutoMapper;

namespace Test
{
    class SourceCode
    {
        public string Key { get; set; }
        public string Time { get; set; }
        public string OrganSoprano { get; set; }
        public string OrganAlto { get; set; }
        public string OrganTenor { get; set; }
        public string OrganBass { get; set; }
        public string OrganSopranoRelative { get; set; }
        public string OrganAltoRelative { get; set; }
        public string OrganTenorRelative { get; set; }
        public string OrganBassRelative { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // INSERT ThemeCatogories and LiturgyCategories
            //INSERT INTO ThemeCategories VALUES ('Zahvala');
            //INSERT INTO ThemeCategories VALUES ('Klanjenje');
            //INSERT INTO ThemeCategories VALUES ('Slavljenje');
            //INSERT INTO ThemeCategories VALUES ('Kristu Kralju');
            //INSERT INTO ThemeCategories VALUES ('Srcu Isusovu');

            //INSERT INTO LiturgyCategories VALUES ('Misne');
            //INSERT INTO LiturgyCategories VALUES ('Ulazna');
            //INSERT INTO LiturgyCategories VALUES ('Darovna');
            //INSERT INTO LiturgyCategories VALUES ('Pričesna');
            //INSERT INTO LiturgyCategories VALUES ('Antifona');
            //INSERT INTO LiturgyCategories VALUES ('Psalam');
            //INSERT INTO LiturgyCategories VALUES ('Misa');
            //INSERT INTO LiturgyCategories VALUES ('Gospodine');
            //INSERT INTO LiturgyCategories VALUES ('Slava');
            //INSERT INTO LiturgyCategories VALUES ('Svet');
            //INSERT INTO LiturgyCategories VALUES ('Jaganjče');
            //INSERT INTO LiturgyCategories VALUES ('Marijanske');
            //INSERT INTO LiturgyCategories VALUES ('Mateju');
            //INSERT INTO LiturgyCategories VALUES ('Josipu');
            //INSERT INTO LiturgyCategories VALUES ('Ivanu Krstitelju');

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<ISong, SongEntity>()
                    .ForMember(dest => dest.OtherParts, opt => opt.MapFrom(s => s.Stanzas.Count() > 0));
                cfg.CreateMap<SongEntity, ISong>();
                cfg.CreateMap<IStanza, StanzaEntity>().ReverseMap();
                cfg.CreateMap<IComposer, ComposerEntity>().ReverseMap();
                cfg.CreateMap<IInstrumentalPart, InstrumentalPartEntity>().ReverseMap();
                cfg.CreateMap<int, LiturgyEntity>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s));
                cfg.CreateMap<LiturgyEntity, int>();
            });
            var mapper = config.CreateMapper();

            using (var db = new MusicContext())
            {
                //CREATE SONG 1
                //SourceCode source = new SourceCode
                //{
                //    Key = "f \\major",
                //    Time = "2/2",
                //    OrganSopranoRelative = "f'",
                //    OrganAltoRelative = "f'",
                //    OrganTenorRelative = "f",
                //    OrganBassRelative = "f",
                //    OrganSoprano = "\\clef \"treble\" \\keyTime f2 e4 f | g2 g | g4 a bes a | g2 f | c' bes4 c | d2 c | g4 a bes a | g2 f | g g4 bes | a2 f | a4 bes c bes | a2 g | c bes4 a | g2 f \\bar \"||\"",
                //    OrganAlto = "\\clef \"treble\" \\keyTime f2 e4 f | e2 d | e4 f g f | e2 f | a g4 a | bes2 a | e4 f g f | e2 f | g e4 g | f2 f | f4 g a g | f2 e | a g4 f | e2 f \\bar \"||\"",
                //    OrganTenor = "\\clef \"bass\" \\keyTime f2 e4 f | c'2 b | c4 c c c | bes2 a | c c4 c | bes2 c | c4 c c c | bes2 a | d c4 c | c2 c | d4 d c c | c2 c | c c4 c | bes2 a \\bar \"||\"",
                //    OrganBass = "\\clef \"bass\" \\keyTime f2 e4 f | c ( e) g2 | c8 [ bes] a4 g f | c2 f | f g4 f | bes2 a | c8 [ bes] a4 g f | c2 f | bes c4 e, | f2 a | d,4 g e e | f ( a) c2 | f, e4 f | c2 f \\bar \"||\""
                //};

                //ISong song = new Song
                //{
                //    Title = "Oče naš dobri",
                //    Template = 15,
                //    Type = "measured",
                //    Code = JsonConvert.SerializeObject(source),
                //    Source = "Cithara octochorda",
                //    Stanzas = new List<IStanza>
                //    {
                //        new Stanza { Number = 1, Text = "Oče naš dobri, u te vjerujemo,\r\nkoj' si na nebu kao i na zemlji:\r\njedan u boštvu, a troj si u Trojstvu\r\nsav na svem svijetu." },
                //        new Stanza { Number = 2, Text = "Mi tvoji sluge stvoreni od tebe,\r\npo tvom smo Sinu spašeni od pakla,\r\nod Svetog Duha krstom posvećeni\r\nza djecu tvoju." },
                //        new Stanza { Number = 3, Text = "Darove ove stavljamo pred tebe,\r\nrad naših ruku, plod sa naših njiva.\r\nKruh taj i vino, Bože, ti posveti,\r\npo svojoj riječi." },
                //        new Stanza { Number = 4, Text = "Od sviju zala ti nas, Bože, brani,\r\nvodi nas stazom kreposnog života,\r\nk nebeskoj slavi daj nam doći svima\r\nživjeti s tobom." }
                //    },
                //    LiturgyCategories = new List<int> { 15 }
                //};

                //song.Arranger = mapper.Map<IComposer>(db.Composers.Where(c => c.Name.Equals("Đuro")).SingleOrDefault());

                //ISongRepository songRepository = new SongRepository(mapper);
                //song = songRepository.CreateSong(song);

                // CREATE SONG 2
                //SourceCode source2 = new SourceCode
                //{
                //    Key = "es \\major",
                //    Time = "2/2",
                //    OrganSopranoRelative = "c'",
                //    OrganAltoRelative = "c'",
                //    OrganTenorRelative = "c",
                //    OrganBassRelative = "c",
                //    OrganSoprano = "\\clef \"treble\" \\keyTime es2 \\p d4 es4 | f2 f2 | f4 g4 as4 g4 | f2 es2 | bes'2 \\f as4 bes4 | c2 bes2 | f4 g4 as4 g4 | f2 es2 | f2 \\mf f4 as4 | g2 es2 | g4 \\< as4 bes4 as4 \\! | g2 \\> f2 | bes2 \\! as4 g4 | f2 es2 \\bar \"||\"",
                //    OrganAlto = "\\clef \"treble\" \\keyTime bes2 b4 c4 | d2 c2 | d4 es4 f4 es4 ~ | es4 d4 bes2 | es1 ~ | es1 | f4 es8 [ d8 ] c4 bes4 | as2 g2 | c4 bes4 as4 c4 | b2 c2 | bes4 es4 d4 c8 [ d8 ] | es2 d2 | d4 g4 f4 es4 ~ | es4 d4 bes2 \\bar \"||\"",
                //    OrganTenor = "\\clef \"bass\" \\keyTime g'1 | bes2 a2 | bes2 c2 ~ | c4 bes8 [ as8 ] g2 | g2 as4 des4 | c4 as4 g2 | bes4 es,2 ~ es8 [ d8 ] | c4 d4 es2 | c2. f8 [ es8 ] | d2 c2 | es2 f8 [ g8 ] as4 | bes4 c4 d2 | bes4 es4 c2 ~ | c4 bes8 [ as8 ] g2 \\bar \"||\"",
                //    OrganBass = "\\clef \"bass\" \\keyTime es2 g4 c,4 | bes4 d4 f2 | bes8 [ as8 ] g4 f4 c4 | as4 bes4 es,2 | es'4 des4 c4 g4 | as4 c4 es2 | d4 c8 [ bes8 ] as4 es4 | f2 c'2 | as4 g4 f4 f4 | g2 as2 | es4 c'4 bes4 f4 | g4 a4 bes2 | g'4 es4 f4 c8 [ bes8 ] | as4 bes4 es2 \\bar \"||\""
                //};

                //SourceCode instrumentalSource = new SourceCode
                //{
                //    OrganSopranoRelative = "c'",
                //    OrganAltoRelative = "c'",
                //    OrganBassRelative = "c",
                //    OrganSoprano = "\\clef \"treble\"  \\keyTime R1 ^\\markup{ \\italic {Mirno} } | R1 | r2 es2 | d4 es4 \\voiceOne f2 | f2 \\breathe f4 g4 | as4 g4 f2 | es1 \\bar \"||\"",
                //    OrganAlto = "\\clef \"treble\" \\keyTime R1 | r4 as4 g4 as4 | as4 bes4 c4 bes4 | as4 g4 \\change Staff = \"upper\" \\voiceTwo bes4 es4 | d4 c4 d4 es4 | es2 ~ es4 d4 | es1 \\bar \"||\"",
                //    OrganBass = "\\clef \"bass\" \\keyTime es2 d4 es4 | f2 f2 | f4 g4 as4 g4 | f4 es4 d4 c4 | \\oneVoice bes4 a4 bes8 [ as8 ] g4 | f4 g4 as4 bes4 | es,1 \\bar \"||\""
                //};

                //SourceCode instrumentalSource2 = new SourceCode
                //{
                //    OrganSopranoRelative = "c'",
                //    OrganAltoRelative = "c'",
                //    OrganTenorRelative = "c'",
                //    OrganBassRelative = "a",
                //    OrganSoprano = "\\clef \"treble\" \\keyTime bes'2 -- as4 -- g4 -- | f1 | bes'2 as4 g4 | f1 \\< | es1\\! \bar \"|.\"",
                //    OrganAlto = "\\clef \"treble\" \\keyTime <es g>4 <d f> <c es> <bes es> | es4 d8 [ c8 ] d2 | es2 bes'2 | c2 bes4 as4 | g1 \\bar \"|.\" ",
                //    OrganTenor = "\\clef \"bass\" \\keyTime R1 | c2 \\sustainOn bes2 ~ | bes4 es4 d4 es4 ~ | es4 d8 [ c8 ] d2 | es1 \\bar \"|.\"",
                //    OrganBass = "\\clef \"bass\" \\keyTime R1 | as4 ( f4 ) bes4 ( as4 ) | g2 f4 es4 | as2 bes4 bes,4 | <es bes'>1 \\bar \"|.\""
                //};

                //ISong song = new Song
                //{
                //    Title = "Oče naš dobri",
                //    Template = 15,
                //    Type = "measured",
                //    Code = JsonConvert.SerializeObject(source2),
                //    Source = "Cithara octochorda",
                //    Stanzas = new List<IStanza>
                //    {
                //        new Stanza { Number = 1, Text = "Oče naš dobri, u te vjerujemo,\r\nkoj' si na nebu kao i na zemlji:\r\njedan u boštvu, a troj si u Trojstvu\r\nsav na svem svijetu." },
                //        new Stanza { Number = 2, Text = "Mi tvoji sluge stvoreni od tebe,\r\npo tvom smo Sinu spašeni od pakla,\r\nod Svetog Duha krstom posvećeni\r\nza djecu tvoju." },
                //        new Stanza { Number = 3, Text = "Darove ove stavljamo pred tebe,\r\nrad naših ruku, plod sa naših njiva.\r\nKruh taj i vino, Bože, ti posveti,\r\npo svojoj riječi." },
                //        new Stanza { Number = 4, Text = "Od sviju zala ti nas, Bože, brani,\r\nvodi nas stazom kreposnog života,\r\nk nebeskoj slavi daj nam doći svima\r\nživjeti s tobom." }
                //    },
                //    LiturgyCategories = new List<int> { 15 },
                //    InstrumentalParts = new List<IInstrumentalPart>
                //    {
                //        new InstrumentalPart
                //        {
                //            Template = 13,
                //            Position = "prelude",
                //            Type = "measured",
                //            Code = JsonConvert.SerializeObject(instrumentalSource)
                //        },
                //        new InstrumentalPart
                //        {
                //            Template = 15,
                //            Position = "coda",
                //            Type = "measured",
                //            Code = JsonConvert.SerializeObject(instrumentalSource2)
                //        }
                //    }
                //};

                //song.Arranger = mapper.Map<IComposer>(db.Composers.FirstOrDefault(c => c.Id.Equals(3)));

                //ISongRepository songRepository = new SongRepository(mapper);
                //song = songRepository.CreateSong(song);

                // GET ALL SONGS
                //ISongRepository songRepository = new SongRepository(mapper);
                //List<ISong> songs = songRepository.GetAllSongs();

                // GET SONG BY ID
                //ISongRepository songRepository = new SongRepository(mapper);
                //ISong song = songRepository.GetSongById(10);

                // CREATE COMPOSER
                //IComposer composer = new Composer
                //{
                //    Name = "Franjo",
                //    Surname = "Dugan"
                //};

                //IComposerRepository composerRepository = new ComposerRepository(mapper);
                //composer = composerRepository.CreateComposer(composer);

                // GET COMPOSERS
                //IComposerRepository composerRepository = new ComposerRepository(mapper);
                //List<IComposer> composers = composerRepository.GetAllComposers();
            }
        }
    }
}
