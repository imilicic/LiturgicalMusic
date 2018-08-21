import { Component, ViewChildren, OnInit, QueryList } from "@angular/core";
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
    templateUrl: "./psalm-edit.component.html"
})
export class PsalmEditComponent implements OnInit {
    @ViewChildren(VoiceComponent) instrumentVoices: QueryList<VoiceComponent>;
    @ViewChildren(LyricsComponent) lyrics: QueryList<LyricsComponent>;
    antiphonaTemplate: boolean[] = [false, false, false, false, true, true, true, true];
    antiphonaTemplateVoices: Template[] = [];
    partPositions: string[] = ["prelude", "interlude", "coda"];
    partsTemplateVoices: Template[][] = [[], [], []];
    pdfFileName: string;
    preview: boolean = false;
    psalamTemplate: boolean[] = [false, false, false, false, true, true, true, true];
    psalamTemplateVoices: Template[] = [];
    song: Song;
    spinner: boolean = false;
    voices: string[] = ["Soprano", "Alto", "Tenor", "Bass"];

    key: FormControl;
    voiceForm: FormGroup;

    constructor(private domSanitizer: DomSanitizer, private songService: SongService, private songSessionService: SongSessionService) { }

    ngOnInit() {
        this.song = this.songSessionService.songSession;

        let key: string = "";
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

        this.createTemplateVoices("AntiphonaVoice", this.antiphonaTemplate.slice(0, 4), this.antiphonaTemplateVoices);
        this.createTemplateVoices("AntiphonaOrgan", this.antiphonaTemplate.slice(4, 8), this.antiphonaTemplateVoices);
        this.createTemplateVoices("PsalmVoice", this.psalamTemplate.slice(0, 4), this.psalamTemplateVoices);
        this.createTemplateVoices("PsalmOrgan", this.psalamTemplate.slice(4, 8), this.psalamTemplateVoices);
        this.createTemplateVoices('Prelude', partVoices[0], this.partsTemplateVoices[0]);
        this.createTemplateVoices('Interlude', partVoices[1], this.partsTemplateVoices[1]);
        this.createTemplateVoices('Coda', partVoices[2], this.partsTemplateVoices[2]);

        if (this.song.Code != undefined) {
            code = JSON.parse(this.song.Code);

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
        controls["Key"] = this.key;

        this.voiceForm = new FormGroup(controls);
    }

    createSong(formValues: any) {
        let code = {};
        let voiceFormValues: any[] = [];
        let stanzaFormValues: any[] = [];

        this.instrumentVoices.forEach(v => {
            voiceFormValues.push(v.getFormValues());
        });

        this.lyrics.forEach(l => {
            stanzaFormValues.push(l.getFormValues());
        });

        code = { ...voiceFormValues[1]['Code'], ...voiceFormValues[2]['Code'], ...voiceFormValues[3]['Code'], ...voiceFormValues[4]['Code'] };

        let psalmKey = Object.keys(code).find(k => k.indexOf("PsalmOrgan") >= 0);
        let antiphonaKey = Object.keys(code).find(k => k.indexOf("AntiphonaOrgan") >= 0);

        if (code[psalmKey].indexOf("\\time") >= 0) {
            code["PsalmMeasured"] = true;
        } else {
            code["PsalmMeasured"] = false;
        }

        if (code[antiphonaKey].indexOf("\\time") >= 0) {
            code["AntiphonaMeasured"] = true;
        } else {
            code["AntiphonaMeasured"] = false;
        }

        code["Key"] = formValues.Key;
        code["AntiphonaStanza"] = stanzaFormValues[0][0]['Text'];

        this.song.Template = [].concat(voiceFormValues[3]['Template'], voiceFormValues[4]['Template']);
        this.song.Code = JSON.stringify(code);

        [0, 5, 6].forEach((n, i) => {
            let part: InstrumentalPart = undefined;

            if (this.song.InstrumentalParts != undefined) {
                let position = this.partsTemplateVoices[i][0].Instrument.toLocaleLowerCase();
                this.song.InstrumentalParts.find(p => p.Position == position);
            } else {
                this.song.InstrumentalParts = [];
            }

            if (part == undefined) {
                if (voiceFormValues[n]['Template'].some((b: boolean) => b)) {
                    this.song.InstrumentalParts.push({
                        Id: undefined,
                        Position: this.partsTemplateVoices[i][0].Instrument.toLocaleLowerCase(),
                        Type: "hymn",
                        Code: JSON.stringify(voiceFormValues[n]['Code']),
                        Template: voiceFormValues[n]['Template']
                    });
                }
            } else {
                part.Code = JSON.stringify(voiceFormValues[n]['Code']);
                part.Template = voiceFormValues[n]['Template'];
            }
        });

        let stanzas: Stanza[] = stanzaFormValues[1];

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

    getTemplateVoices(instrument: string) {
        if (instrument.indexOf("Antiphona") >= 0) {
            return this.antiphonaTemplateVoices.filter(t => t.Instrument == instrument);
        } else {
            return this.psalamTemplateVoices.filter(t => t.Instrument == instrument);
        }
    }

    previewEnabled() {
        return this.voiceForm.controls["Key"].invalid;
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
        return !(this.antiphonaTemplateVoices.some(t => t.Use) && this.psalamTemplateVoices.some(t => t.Use));
    }
}