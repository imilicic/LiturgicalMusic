import { Component, OnInit, ViewChild, ViewChildren, QueryList } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";

import { InstrumentalPart } from "../../models/instrumentalPart.model";
import { KeyTimeComponent } from "../../shared/keyTime/key-time.component";
import { LyricsComponent } from "../../shared/lyrics/lyrics.component";
import { Song } from "../../models/song.model";
import { SongCommonService } from "../../services/song-common.service";
import { SongService } from "../../services/song.service";
import { SongSessionService } from "../../services/song-session.service";
import { Stanza } from "../../models/stanza.model";
import { Template } from "../../models/template.model";
import { VoiceComponent } from "../../shared/voice/voice.component";

@Component({
    templateUrl: "./hymn-edit.component.html"
})
export class HymnEditComponent implements OnInit {
    @ViewChildren(VoiceComponent) instrumentVoices: QueryList<VoiceComponent>;
    @ViewChild(LyricsComponent) lyrics: LyricsComponent;
    @ViewChild(KeyTimeComponent) keyTime: KeyTimeComponent;
    partsTemplateVoices: Template[][];
    pdfFileName: string;
    preview: boolean = false;
    spinner: boolean = false;
    templateVoices: Template[] = [];
    key: string;
    timeNumerator: number;
    timeDenominator: number;

    constructor(
        private domSanitizer: DomSanitizer,
        private songCommonService: SongCommonService,
        private songService: SongService,
        private songSessionService: SongSessionService) { }

    ngOnInit() {
        window.scrollTo(0, 0);

        let code: any;
        let partVoices: boolean[][] = Array(3).fill(Array(4).fill(false));
        this.pdfFileName = this.songCommonService.createPdfFileName(this.songSessionService.song);
        this.partsTemplateVoices = [[],[],[]];

        if (this.songSessionService.song.InstrumentalParts != undefined) {
            this.songSessionService.song.InstrumentalParts.forEach(p => {
                let i = this.songCommonService.partPositions.indexOf(p.Position);
                partVoices[i] = p.Template;
            });
        }

        this.songCommonService.createTemplateVoices('Voice', this.songSessionService.song.Template.slice(0, 4), this.templateVoices);
        this.songCommonService.createTemplateVoices('Organ', this.songSessionService.song.Template.slice(4, 8), this.templateVoices);
        this.songCommonService.createTemplateVoices('Prelude', partVoices[0], this.partsTemplateVoices[0]);
        this.songCommonService.createTemplateVoices('Interlude', partVoices[1], this.partsTemplateVoices[1]);
        this.songCommonService.createTemplateVoices('Coda', partVoices[2], this.partsTemplateVoices[2]);

        if (this.songSessionService.song.Code != undefined) {
            code = JSON.parse(this.songSessionService.song.Code);
            let time: string = code.Time.split("/");

            this.timeNumerator = parseInt(time[0]);
            this.timeDenominator = parseInt(time[1]);
            this.key = code.Key;
        }
    }

    createSong() {
        let code = {};
        let voiceFormValues: any[] = [];

        this.instrumentVoices.forEach(v => {
            voiceFormValues.push(v.getFormValues());
        });

        code = { ...voiceFormValues[1]['Code'], ...voiceFormValues[2]['Code'], ...this.keyTime.getKeyTime() };

        this.songSessionService.song.Template = [].concat(voiceFormValues[1]['Template'], voiceFormValues[2]['Template']);
        this.songSessionService.song.Code = JSON.stringify(code);
        this.songSessionService.song.Stanzas = this.lyrics.getFormValues();
        this.songCommonService.createInstrumentalParts([0, 3, 4], this.partsTemplateVoices, voiceFormValues);
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

    getTemplateVoices(instrument: string) {
        return this.templateVoices.filter(t => t.Instrument == instrument);
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
        return !this.templateVoices.some(t => t.Use);
    }
}