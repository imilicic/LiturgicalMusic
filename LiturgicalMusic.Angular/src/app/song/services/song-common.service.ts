import { Injectable } from "@angular/core";

import { InstrumentalPart } from "../models/instrumentalPart.model";
import { Song } from "../models/song.model";
import { Template } from "../models/template.model";
import { SongSessionService } from "../services/song-session.service";

@Injectable()
export class SongCommonService {
    liturgyCategories: string[] = ["Ulazna", "Darovna", "Pričesna", "Gospodine", "Slava", "Svet", "Jaganjče", "Misa", "Psalam", "Misne", "Posljednica"];
    themeCategories: string[] = ["Slavljenje", "Zahvala", "Kajanje", "Klanjanje", "Prigodna", "Marijanske", "Srce Isusovo"];
    voices: string[] = ["Soprano", "Alto", "Tenor", "Bass"];
    partPositions: string[] = ["prelude", "interlude", "coda"];

    constructor(private songSessionService: SongSessionService) { }

    createInstrumentalParts(positions: number[], partsTemplateVoices: any[][], voiceFormValues: any[][], mapCode: boolean = true) {
        positions.forEach((n, i) => {
            let part: InstrumentalPart = undefined;

            if (this.songSessionService.song.InstrumentalParts != undefined) {
                let position = partsTemplateVoices[i][0].Instrument.toLocaleLowerCase();
                part = this.songSessionService.song.InstrumentalParts.find(p => p.Position == position);
            } else {
                this.songSessionService.song.InstrumentalParts = [];
            }

            if (part == undefined) {
                if (voiceFormValues[n]['Template'].some((b: boolean) => b)) {
                    part = new InstrumentalPart();

                    part.Id = undefined;
                    part.Position = partsTemplateVoices[i][0].Instrument.toLocaleLowerCase();
                    part.Type = "hymn";
                    part.Template = voiceFormValues[n]['Template'];

                    if (mapCode) {
                        part.Code = JSON.stringify(this.mapper(voiceFormValues[n]['Code'], partsTemplateVoices[i][0].Instrument));
                    } else {
                        part.Code = part.Code = JSON.stringify(voiceFormValues[n]['Code']);
                    }

                    this.songSessionService.song.InstrumentalParts.push(part);
                }
            } else {
                if (voiceFormValues[n]['Template'].some((b: boolean) => b)) {
                    part.Template = voiceFormValues[n]['Template'];

                    if (mapCode) {
                        part.Code = JSON.stringify(this.mapper(voiceFormValues[n]['Code'], partsTemplateVoices[i][0].Instrument));
                    } else {
                        part.Code = part.Code = JSON.stringify(voiceFormValues[n]['Code']);
                    }
                } else {
                    let position: number = this.songSessionService.song.InstrumentalParts.findIndex(p => p.Position == part.Position);
                    this.songSessionService.song.InstrumentalParts.splice(position, 1);
                }
            }
        });
    }

    createPdfFileName(song: Song) {
        let pdfFileName: string = "app/assets/pdf/" + song.Title;

        if (song.Composer != undefined) {
            pdfFileName += song.Composer.Name + song.Composer.Surname;
        } else if (song.Arranger != undefined) {
            pdfFileName += song.Arranger.Name + song.Arranger.Surname;
        }

        pdfFileName += ".pdf";

        return pdfFileName;
    }

    createTemplateVoices(instrument: string, template: boolean[], templateVoices: Template[]) {
        template.forEach((t, i) => {
            templateVoices.push({
                ControlName: instrument + this.voices[i],
                ControlNameRelative: instrument + this.voices[i] + "Relative",
                Instrument: instrument,
                Name: this.voices[i],
                Use: t
            });
        });
    }

    extractTemplate(keys: string[]) {
        let template: boolean[] = [false, false, false, false];

        keys.forEach(k => {
            if (k.indexOf(this.voices[0]) >= 0) template[0] = true;
            else if (k.indexOf(this.voices[1]) >= 0) template[1] = true;
            else if (k.indexOf(this.voices[2]) >= 0) template[2] = true;
            else if (k.indexOf(this.voices[3]) >= 0) template[3] = true;
        });

        return template;
    }

    getInstrumentalPartCode(song: Song, position: string) {
        if (song.InstrumentalParts == undefined) {
            return undefined;
        }

        let result: InstrumentalPart = song.InstrumentalParts.find(p => p.Position == position.toLocaleLowerCase());

        if (result == undefined) {
            return undefined;
        }

        let code = JSON.parse(result.Code);

        return this.mapper(code, position, true);
    }

    getSongCode(song: Song) {
        if (song.Code == undefined) {
            return undefined;
        }

        return JSON.parse(song.Code);
    }

    mapper(code: any, instrument: string, back: boolean = false) {
        let oldKeys: string[] = Object.keys(code);
        let newKeys: string[];
        let newObject = {};

        if (back) {
            newKeys = oldKeys.map(p => instrument + p);
        } else {
            newKeys = oldKeys.map(p => p.replace(new RegExp(instrument, 'g'), ''));
        }

        newKeys.forEach((k, i) => {
            newObject[k] = code[oldKeys[i]]
        });

        return newObject;
    }
}