using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiturgicalMusic.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new MusicContext())
            {
                //List<ComposerEntity> composers = new List<ComposerEntity> {
                //    new ComposerEntity() { Name = "Ivan", Surname = "Andrić" },
                //    new ComposerEntity() { Name = "Josip", Surname = "Bervar" },
                //    new ComposerEntity() { Name = "Igor", Surname = "Blažević" },
                //    new ComposerEntity() { Name = "Dejan", Surname = "Bubalo" },
                //    new ComposerEntity() { Name = "Anselmo", Surname = "Canjuga" },
                //    new ComposerEntity() { Name = "Franjo", Surname = "Dugan" },
                //    new ComposerEntity() { Name = "Ljubomir", Surname = "Galetić" },
                //    new ComposerEntity() { Name = "Pero", Surname = "Ivanišić Crnkovački" },
                //    new ComposerEntity() { Name = "Anđelko", Surname = "Klobučar" },
                //    new ComposerEntity() { Name = "Mato", Surname = "Lešćan" },
                //    new ComposerEntity() { Name = "Franjo", Surname = "Lužević" },
                //    new ComposerEntity() { Name = "Đuka", Surname = "Marić" },
                //    new ComposerEntity() { Name = "Šime", Surname = "Marović" },
                //    new ComposerEntity() { Name = "Miroslav", Surname = "Martinjak" },
                //    new ComposerEntity() { Name = "Anđelko", Surname = "Milanović" },
                //    new ComposerEntity() { Name = "Vilko", Surname = "Novak" },
                //    new ComposerEntity() { Name = "Stanislav", Surname = "Preprek" },
                //    new ComposerEntity() { Name = "Bernardin", Surname = "Sokol" },
                //    new ComposerEntity() { Name = "Zlatko", Surname = "Špoljar" },
                //    new ComposerEntity() { Name = "Đuro", Surname = "Tomašić" },
                //    new ComposerEntity() { Name = "Slavko", Surname = "Topić" }
                //};
                //composers.ForEach(c => db.Composers.Add(c));

                //string json = "{\"Key\":\"f\",\"Time\":\"2/2\",\"OrganSopran\":\"\\clef \"treble\" \\keyTime f2 e4 f | g2 g | g4 a bes a | g2 f | c\' bes4 c | d2 c | g4 a bes a | g2 f | g g4 bes | a2 f | a4 bes c bes | a2 g | c bes4 a | g2 f \\bar \"||\"\"}";
                //string lyrics = "{'Lyrics':['Oče naš dobri, u te vjerujemo, koj' si na nebu kao i na zemlji: jedan u boštvu, a troj si u Trojstvu sav na svem svijetu.','Mi tvoji sluge stvoreni od tebe, po tvom smo Sinu spašeni od pakla, od Svetog Duha krstom posvećeni za djecu tvoju.','Darove ove stavljamo pred tebe, rad naših ruku, plod sa naših njiva. Kruh taj i vino, Bože, ti posveti, po svojoj riječi.','Od sviju zala ti nas, Bože, brani, vodi nas stazom kreposnog života, k nebeskoj slavi daj nam doći svima živjeti s tobom.']}";
                Console.Write("Key: ");
                string key = Console.ReadLine();
                Console.Write("Time: ");
                string time = Console.ReadLine();
                Console.Write("OrganSopran: ");
                string organSopran = Console.ReadLine();
                Proba score = new Proba
                {
                    Key = key,
                    Time = time,
                    OrganSopran = organSopran
                };

                string json = JsonConvert.SerializeObject(score);
                Console.WriteLine(json);

                // natrag
                Proba objekt = JsonConvert.DeserializeObject<Proba>(json);

                Console.WriteLine(objekt.Key);
                Console.WriteLine(objekt.Time);
                Console.WriteLine(objekt.OrganSopran);

                //ComposerEntity composer = new ComposerEntity
                //{
                //    Name = "Đuka",
                //    Surname = "Marić"
                //};

                //db.Composers.Add(composer);

                //SongEntity music = new SongEntity
                //{
                //    Title = "Oče naš dobri",
                //    Template = "SATB",
                //    Type = "measured",
                //    Code = "{'key':'f','time':'2/2','S':'\\clef \"treble\" \\keyTime f2 e4 f | g2 g | g4 a bes a | g2 f | c' bes4 c | d2 c | g4 a bes a | g2 f | g g4 bes | a2 f | a4 bes c bes | a2 g | c bes4 a | g2 f \\bar \"||\"','A':'\\clef \"treble\" \\keyTime f2 e4 f | e2 d | e4 f g f | e2 f | a g4 a | bes2 a | e4 f g f | e2 f | g e4 g | f2 f | f4 g a g | f2 e | a g4 f | e2 f \\bar \"||\"','T':'\\clef \"bass\" \\keyTime f2 e4 f | c'2 b | c4 c c c | bes2 a | c c4 c | bes2 c | c4 c c c | bes2 a | d c4 c | c2 c | d4 d c c | c2 c | c c4 c | bes2 a \\bar \"||\"','B':'\\clef \"bass\" \\keyTime f2 e4 f | c ( e) g2 | c8 [ bes] a4 g f | c2 f | f g4 f | bes2 a | c8 [ bes] a4 g f | c2 f | bes c4 e, | f2 a | d,4 g e e | f ( a) c2 | f, e4 f | c2 f \\bar \"||\"','lyrics':['Oče naš dobri, u te vjerujemo, koj' si na nebu kao i na zemlji: jedan u boštvu, a troj si u Trojstvu sav na svem svijetu.','Mi tvoji sluge stvoreni od tebe, po tvom smo Sinu spašeni od pakla, od Svetog Duha krstom posvećeni za djecu tvoju.','Darove ove stavljamo pred tebe, rad naših ruku, plod sa naših njiva. Kruh taj i vino, Bože, ti posveti, po svojoj riječi.','Od sviju zala ti nas, Bože, brani, vodi nas stazom kreposnog života, k nebeskoj slavi daj nam doći svima živjeti s tobom.']}",
                //    OtherParts = 0
                //};

                //music.Arranger = db.Composers.FirstOrDefault(c => c.Name == "Đuka" && c.Surname == "Marić");

                //db.Songs.Add(music);
                //db.SaveChanges();
            }
        }
    }

    class Proba
    {
        public Proba() { }
        public string Key { get; set; }
        public string Time { get; set; }
        public string OrganSopran { get; set; }
        public string OrganAlt { get; set; }
        public string OrganTenor { get; set; }
        public string OrganBass { get; set; }
        public string VoiceSopran { get; set; }
        public string VoiceAlt { get; set; }
        public string VoiceTenor { get; set; }
        public string VoiceBass { get; set; }
    }
}
