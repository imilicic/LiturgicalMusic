import { Stanza } from "./stanza.model";
import { InstrumentalPart } from "./instrumentalPart.model";
import { Composer } from "./composer.model";

export class Song {
    Arranger: Composer;
    Code: string;
    Composer: Composer;
    Id: number;
    InstrumentalParts: InstrumentalPart[];
    LiturgyCategories: number[];
    OtherInformation: string;
    Source: string;
    Stanzas: Stanza[];
    Template: boolean[];
    ThemeCategories: number[];
    Title: string;
    Type: string;

    constructor(song?: Song) {
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
            this.OtherInformation = song.OtherInformation;
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
        }
    }
}