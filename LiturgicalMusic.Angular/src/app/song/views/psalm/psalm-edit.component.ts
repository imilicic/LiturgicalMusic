import { Component, ViewChild, ViewChildren, OnInit, QueryList } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";

import { InstrumentalPart } from "../../models/instrumentalPart.model";
import { LyricsComponent } from "../../shared/lyrics/lyrics.component";
import { Song } from "../../models/song.model";
import { SongCommonService } from "../../services/song-common.service";
import { SongService } from "../../services/song.service";
import { SongSessionService } from "../../services/song-session.service";
import { Stanza } from "../../models/stanza.model";
import { Template } from "../../models/template.model";
import { VoiceComponent } from "../../shared/voice/voice.component";
import { KeyTimeComponent } from "../../shared/keyTime/key-time.component";

@Component({
    templateUrl: "./psalm-edit.component.html"
})
export class PsalmEditComponent implements OnInit {
    @ViewChildren(VoiceComponent) instrumentVoices: QueryList<VoiceComponent>;
    @ViewChildren(LyricsComponent) lyrics: QueryList<LyricsComponent>;
    @ViewChild(KeyTimeComponent) keyTime: KeyTimeComponent;
    antiphonaTemplate: boolean[] = [false, false, false, false, true, true, true, true];
    antiphonaTemplateVoices: Template[] = [];
    partsTemplateVoices: Template[][] = [[], [], []];
    pdfFileName: string;
    preview: boolean = false;
    psalamTemplate: boolean[] = [false, false, false, false, true, true, true, true];
    psalamTemplateVoices: Template[] = [];
    spinner: boolean = false;
    key: string;

    constructor(
        private domSanitizer: DomSanitizer,
        private songCommonService: SongCommonService,
        private songService: SongService,
        private songSessionService: SongSessionService) { }

    ngOnInit() {
        window.scrollTo(0, 0);
        let code: any = undefined;
        let partVoices: boolean[][] = Array(3).fill(Array(4).fill(false));
        this.pdfFileName = this.songCommonService.createPdfFileName(this.songSessionService.song);

        if (this.songSessionService.song.InstrumentalParts != undefined) {
            this.songSessionService.song.InstrumentalParts.forEach(p => {
                let i = this.songCommonService.partPositions.indexOf(p.Position);
                partVoices[i] = p.Template;
            });

            let code = JSON.parse(this.songSessionService.song.Code);
            let keys: string[] = Object.keys(code);

            let avtemplate = this.songCommonService.extractTemplate(keys.filter(k => k.indexOf("AntiphonaVoice") >= 0));
            let aotemplate = this.songCommonService.extractTemplate(keys.filter(k => k.indexOf("AntiphonaOrgan") >= 0));
            let pvtemplate = this.songCommonService.extractTemplate(keys.filter(k => k.indexOf("PsalmVoice") >= 0));
            let potemplate = this.songCommonService.extractTemplate(keys.filter(k => k.indexOf("PsalmOrgan") >= 0));

            this.antiphonaTemplate = avtemplate.concat(aotemplate);
            this.psalamTemplate = pvtemplate.concat(potemplate);
        }

        this.songCommonService.createTemplateVoices("AntiphonaVoice", this.antiphonaTemplate.slice(0, 4), this.antiphonaTemplateVoices);
        this.songCommonService.createTemplateVoices("AntiphonaOrgan", this.antiphonaTemplate.slice(4, 8), this.antiphonaTemplateVoices);
        this.songCommonService.createTemplateVoices("PsalmVoice", this.psalamTemplate.slice(0, 4), this.psalamTemplateVoices);
        this.songCommonService.createTemplateVoices("PsalmOrgan", this.psalamTemplate.slice(4, 8), this.psalamTemplateVoices);
        this.songCommonService.createTemplateVoices('Prelude', partVoices[0], this.partsTemplateVoices[0]);
        this.songCommonService.createTemplateVoices('Interlude', partVoices[1], this.partsTemplateVoices[1]);
        this.songCommonService.createTemplateVoices('Coda', partVoices[2], this.partsTemplateVoices[2]);

        if (this.songSessionService.song.Code != undefined) {
            code = JSON.parse(this.songSessionService.song.Code);

            this.key = code.Key;
        }
    }

    createSong() {
        let code = {};
        let voiceFormValues: any[] = [];
        let stanzaFormValues: any[] = [];

        this.instrumentVoices.forEach(v => {
            voiceFormValues.push(v.getFormValues());
        });

        this.lyrics.forEach(l => {
            stanzaFormValues.push(l.getFormValues());
        });

        code = { ...voiceFormValues[1]['Code'], ...voiceFormValues[2]['Code'], ...voiceFormValues[3]['Code'], ...voiceFormValues[4]['Code'], ...this.keyTime.getKeyTime() };

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

        code["AntiphonaStanza"] = stanzaFormValues[0][0]['Text'];

        this.songSessionService.song.Template = [].concat(voiceFormValues[3]['Template'], voiceFormValues[4]['Template']);
        this.songSessionService.song.Code = JSON.stringify(code);
        this.songSessionService.song.Stanzas = stanzaFormValues[1];
        this.songCommonService.createInstrumentalParts([0, 5, 6], this.partsTemplateVoices, voiceFormValues);
    }

    createUpdateSong() {
        this.spinner = true;
        this.createSong();

        if (this.songSessionService.action == "create") {
            this.songService.createSong(this.songSessionService.song).subscribe((response: Song) => {
                this.spinner = false;
                this.songSessionService.moveTo("songs/view/" + response.Id);
            });
        } else {
            this.songService.updateSong(this.songSessionService.song).subscribe((response: Song) => {
                this.spinner = false;
                this.songSessionService.moveTo("songs/view/" + response.Id);
            });
        }
    }

    getAntiphonaStanza() {
        if (this.songSessionService.song.Code == undefined) {
            return undefined;
        }

        let song = JSON.parse(this.songSessionService.song.Code);
        let antiphona: Stanza = new Stanza();

        antiphona.Text = song["AntiphonaStanza"];
        antiphona.Number = 0;

        return [antiphona];
    }

    getTemplateVoices(instrument: string) {
        if (instrument.indexOf("Antiphona") >= 0) {
            return this.antiphonaTemplateVoices.filter(t => t.Instrument == instrument);
        } else {
            return this.psalamTemplateVoices.filter(t => t.Instrument == instrument);
        }
    }

    previewSong() {
        this.preview = true;
        this.spinner = true;
        this.createSong();

        this.songService.previewSong(this.songSessionService.song)
            .subscribe((response: Song) => {
                this.spinner = false;
            });
    }

    voicesInvalid() {
        return !(this.antiphonaTemplateVoices.some(t => t.Use) && this.psalamTemplateVoices.some(t => t.Use));
    }
}