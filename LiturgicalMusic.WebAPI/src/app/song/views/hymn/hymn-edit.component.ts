﻿import { Component, OnInit, ViewChild, ViewChildren, QueryList } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { DomSanitizer } from "@angular/platform-browser";

import { InstrumentalPart } from "../../models/instrumentalPart.model";
import { LyricsComponent } from "../lyrics/lyrics.component";
import { Song } from "../../models/song.model";
import { SongService } from "../../services/song.service";
import { SongSessionService } from "../../services/song-session.service";
import { Stanza } from "../../models/stanza.model";
import { Template } from "../../models/template.model";
import { VoiceComponent } from "../voice/voice.component";

@Component({
    templateUrl: "./hymn-edit.component.html"
})
export class HymnEditComponent implements OnInit {
    @ViewChildren(VoiceComponent) instrumentVoices: QueryList<VoiceComponent>;
    @ViewChild(LyricsComponent) lyrics: LyricsComponent;
    partPositions: string[] = ["prelude", "interlude", "coda"];
    partsTemplateVoices: Template[][] = [[], [], []];
    pdfFileName: string;
    preview: boolean = false;
    song: Song;
    spinner: boolean = false;
    templateVoices: Template[] = [];
    voices: string[] = ["Soprano", "Alto", "Tenor", "Bass"];

    key: FormControl;
    template: FormControl;
    timeNumerator: FormControl;
    timeDenominator: FormControl;
    voiceForm: FormGroup;

    constructor(private domSanitizer: DomSanitizer, private songService: SongService, private songSessionService: SongSessionService) { }

    ngOnInit() {
        window.scrollTo(0, 0);
        this.song = this.songSessionService.songSession;

        let key: string = "";
        let timeNumerator: number = undefined;
        let timeDenominator: number = undefined;
        let code: any = undefined;
        let controls: any = {};
        let partVoices: boolean[][] = [[false, false, false, false], [false, false, false, false], [false, false, false, false]];

        if (this.song.Template == undefined) {
            this.song.Template = [false, false, false, false, true, true, true, true];
        }

        if (this.song.InstrumentalParts != undefined) {
            this.song.InstrumentalParts.forEach(p => {
                let i = this.partPositions.indexOf(p.Position);
                partVoices[i] = p.Template;
            });
        }

        this.createTemplateVoices('Voice', this.song.Template.slice(0, 4), this.templateVoices);
        this.createTemplateVoices('Organ', this.song.Template.slice(4, 8), this.templateVoices);
        this.createTemplateVoices('Prelude', partVoices[0], this.partsTemplateVoices[0]);
        this.createTemplateVoices('Interlude', partVoices[1], this.partsTemplateVoices[1]);
        this.createTemplateVoices('Coda', partVoices[2], this.partsTemplateVoices[2]);

        if (this.song.Code != undefined) {
            code = JSON.parse(this.song.Code);
            let time: string = code.Time.split("/");

            timeNumerator = parseInt(time[0]);
            timeDenominator = parseInt(time[1]);
            key = code.Key;
        }
        
        this.pdfFileName = "app/assets/pdf/" + this.song.Title;

        if (this.song.Composer != undefined) {
            this.pdfFileName += this.song.Composer.Name + this.song.Composer.Surname;
        } else if (this.song.Arranger != undefined) {
            this.pdfFileName += this.song.Arranger.Name + this.song.Arranger.Surname;
        }

        this.pdfFileName += ".pdf";

        this.key = new FormControl(key, Validators.required);
        this.timeNumerator = new FormControl(timeNumerator, Validators.required);
        this.timeDenominator = new FormControl(timeDenominator, Validators.required);

        controls["Key"] = this.key;
        controls["timeNumerator"] = this.timeNumerator;
        controls["timeDenominator"] = this.timeDenominator;

        this.voiceForm = new FormGroup(controls);
    }

    createSong(formValues: any) {
        let code = {};
        let voiceFormValues: any[] = [];

        this.instrumentVoices.forEach(v => {
            voiceFormValues.push(v.getFormValues());
        });

        code = { ...voiceFormValues[1]['Code'], ...voiceFormValues[2]['Code'] };
        code["Time"] = formValues.timeNumerator + "/" + formValues.timeDenominator;
        code["Key"] = formValues.Key;

        this.song.Template = [].concat(voiceFormValues[1]['Template'], voiceFormValues[2]['Template']);
        this.song.Code = JSON.stringify(code);

        [0, 3, 4].forEach((n, i) => {
            let part: InstrumentalPart = undefined;

            if (this.song.InstrumentalParts != undefined) {
                let position = this.partsTemplateVoices[i][0].Instrument.toLocaleLowerCase();
                part = this.song.InstrumentalParts.find(p => p.Position == position);
            } else {
                this.song.InstrumentalParts = [];
            }

            if (part == undefined) {
                if (voiceFormValues[n]['Template'].some((b: boolean) => b)) {
                    part = new InstrumentalPart();

                    part.Id = undefined;
                    part.Position = this.partsTemplateVoices[i][0].Instrument.toLocaleLowerCase();
                    part.Type = "hymn";
                    part.Code = JSON.stringify(this.mapper(voiceFormValues[n]['Code'], this.partsTemplateVoices[i][0].Instrument));
                    part.Template = voiceFormValues[n]['Template'];

                    this.song.InstrumentalParts.push(part);
                }
            } else {
                part.Code = JSON.stringify(this.mapper(voiceFormValues[n]['Code'], this.partsTemplateVoices[i][0].Instrument));
                part.Template = voiceFormValues[n]['Template'];
            }
        });

        let stanzas: Stanza[] = this.lyrics.getFormValues();

        stanzas.forEach((stanza, i) => {
            let foundStanza: Stanza = undefined;

            if (this.song.Stanzas != undefined) {
                foundStanza = this.song.Stanzas.find(s => s.Number == i + 1);
            }

            if (foundStanza != undefined) {
                stanza.Id = foundStanza.Id;
            }
        });

        this.song.Stanzas = stanzas;
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

    createUpdateSong(formValues: any) {
        this.spinner = true;
        this.createSong(formValues);

        if (this.songSessionService.action == "create") {
            this.songService.createSong(this.song).subscribe((response: Song) => {
                this.spinner = false;
                this.songSessionService.moveTo("songs/view/" + response.Id);
            });
        } else {
            this.songService.updateSong(this.song).subscribe((response: Song) => {
                this.spinner = false;
                this.songSessionService.moveTo("songs/view/" + response.Id);
            });
        }
    }

    getInstrumentalPartCode(position: string) {
        if (this.song.InstrumentalParts == undefined) {
            return undefined;
        }

        let result: InstrumentalPart = this.song.InstrumentalParts.find(p => p.Position == position.toLocaleLowerCase());

        if (result == undefined) {
            return undefined;
        }

        let code = JSON.parse(result.Code);

        return this.mapper(code, position, true);
    }

    getSongCode() {
        if (this.song.Code == undefined) {
            return undefined;
        }

        return JSON.parse(this.song.Code);
    }

    getTemplateVoices(instrument: string) {
        return this.templateVoices.filter(t => t.Instrument == instrument);
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

    previewEnabled() {
        return this.voiceForm.controls["Key"].invalid || this.voiceForm.controls["timeNumerator"].invalid || this.voiceForm.controls["timeDenominator"].invalid;
    }

    previewSong(formValues: any) {
        this.preview = true;
        this.spinner = true;
        this.createSong(formValues);

        this.songService.previewSong(this.song)
            .subscribe((response: Song) => {
                this.spinner = false;
            });
    }

    voicesInvalid() {
        return !this.templateVoices.some(t => t.Use);
    }
}