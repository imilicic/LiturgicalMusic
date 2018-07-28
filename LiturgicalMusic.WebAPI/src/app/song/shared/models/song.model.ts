import { Stanza } from "./stanza.model";
import { InstrumentalPart } from "./instrumentalPart.model";
import { Composer } from "./composer.model";

export class Song {
    Id: number;
    Title: string;
    Template: boolean[];
    Type: string;
    Code: string;
    Source: string;
    OtherInformation: string;
    Stanzas: Stanza[];
    Composer: Composer;
    Arranger: Composer;
    InstrumentalParts: InstrumentalPart[];
    ThemeCategories: number[];
    LiturgyCategories: number[];
}