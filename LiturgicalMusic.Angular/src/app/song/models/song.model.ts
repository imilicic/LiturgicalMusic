import { Stanza } from "./stanza.model";
import { InstrumentalPart } from "./instrumentalPart.model";
import { Composer } from "./composer.model";

export class Song {
    Arranger: Composer;
    Code: string = undefined;
    Composer: Composer;
    Id: number = undefined;
    InstrumentalParts: InstrumentalPart[];
    LiturgyCategories: number[] = [];
    OtherInformations: string = undefined;
    Source: string = undefined;
    Stanzas: Stanza[];
    Template: boolean[];
    ThemeCategories: number[] = [];
    Title: string = undefined;
    Type: string = undefined;

    constructor(song?: Song) {
        this.Template = Array(4).fill(false).concat(Array(4).fill(true));

        if (song) {
            this.Arranger = new Composer(song.Arranger);
            this.Code = song.Code;
            this.Composer = new Composer(song.Composer);
            this.Id = song.Id;

            if (song.InstrumentalParts != undefined) {
                this.InstrumentalParts = [];

                song.InstrumentalParts.forEach(part => {
                    this.InstrumentalParts.push(new InstrumentalPart(part));
                });
            } else {
                this.InstrumentalParts = undefined;
            }

            this.LiturgyCategories = Array.from(song.LiturgyCategories);
            this.OtherInformations = song.OtherInformations;
            this.Source = song.Source;

            if (song.Stanzas != undefined) {
                this.Stanzas = [];

                song.Stanzas.forEach(stanza => {
                    this.Stanzas.push(new Stanza(stanza));
                });
            } else {
                this.Stanzas = undefined;
            }

            this.Template = Array.from(song.Template);
            this.ThemeCategories = Array.from(song.ThemeCategories);
            this.Title = song.Title;
            this.Type = song.Type;
        } else {
            this.Arranger = new Composer();
            this.Composer = new Composer();
        }
    }
}