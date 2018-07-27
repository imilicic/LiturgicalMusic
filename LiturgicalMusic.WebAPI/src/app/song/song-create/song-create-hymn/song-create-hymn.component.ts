import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { DomSanitizer } from "@angular/platform-browser";

import { Song } from "../../shared/models/song.model";
import { SongService } from "../../shared/song.service";

@Component({
    selector: "create-hymn",
    templateUrl: "./song-create-hymn.component.html"
})
export class SongCreateHymnComponent implements OnInit {
    @Input() song: Song;
    @Output() songVoices = new EventEmitter();
    chosenVoicesOrgan: any[] = [];
    chosenVoicesVoice: any[] = [];
    pdfFileName: string;
    preview: boolean = false;
    spinner: boolean = false;
    voices: any[] = ["Soprano", "Alto", "Tenor", "Bass"];

    key: FormControl;
    timeNumerator: FormControl;
    timeDenominator: FormControl;
    voiceForm: FormGroup;

    constructor(private domSanitizer: DomSanitizer, private songService: SongService) { }

    ngOnInit() {
        this.pdfFileName = "app/assets/pdf/" + this.song.Title;

        if (this.song.Composer != null) {
            this.pdfFileName += this.song.Composer.Name + this.song.Composer.Surname;
        } else if (this.song.Arranger != null) {
            this.pdfFileName += this.song.Arranger.Name + this.song.Arranger.Surname;
        }

        this.pdfFileName += ".pdf";

        let controls: any = {};
        this.song.Template.split("").forEach((b, i) => {
            if (i <= 3) { // SATB voice
                if (parseInt(b)) {
                    this.chosenVoicesVoice.push({
                        voice: this.voices[i],
                        controlName: "Voice" + this.voices[i],
                        controlNameRelative: "Voice" + this.voices[i] + "Relative"
                    });

                    controls["Voice" + this.voices[i]] = new FormControl("", Validators.required);
                    controls["Voice" + this.voices[i] + "Relative"] = new FormControl("", Validators.required);
                }
            } else { // SATB organ
                if (parseInt(b)) {
                    this.chosenVoicesOrgan.push({
                        voice: this.voices[i-4],
                        controlName: "Organ" + this.voices[i-4],
                        controlNameRelative: "Organ" + this.voices[i-4] + "Relative"
                    });

                    controls["Organ" + this.voices[i - 4]] = new FormControl("", Validators.required);
                    controls["Organ" + this.voices[i - 4] + "Relative"] = new FormControl("", Validators.required);
                }
            }
        });

        this.key = new FormControl("", Validators.required);
        this.timeNumerator = new FormControl("", Validators.required);
        this.timeDenominator = new FormControl("", Validators.required);

        controls["Key"] = this.key;
        controls["timeNumerator"] = this.timeNumerator;
        controls["timeDenominator"] = this.timeDenominator;

        this.voiceForm = new FormGroup(controls);
    }

    createSong(formValues: any) {
        let time: string = formValues.timeNumerator + "/" + formValues.timeDenominator;
        formValues = JSON.parse(JSON.stringify(formValues)); // deep copy

        formValues["Time"] = time;
        delete formValues["timeNumerator"];
        delete formValues["timeDenominator"];

        if (formValues["OrganSoprano"]) {
            formValues["OrganSoprano"] = "\\clef \"treble\" \\keyTime " + formValues["OrganSoprano"];
        }
        if (formValues["OrganAlto"]) {
            formValues["OrganAlto"] = "\\clef \"treble\" \\keyTime " + formValues["OrganAlto"];
        }
        if (formValues["OrganTenor"]) {
            formValues["OrganTenor"] = "\\clef \"bass\" \\keyTime " + formValues["OrganTenor"];
        }
        if (formValues["OrganBass"]) {
            formValues["OrganBass"] = "\\clef \"bass\" \\keyTime " + formValues["OrganBass"];
        }
        if (formValues["VoiceSoprano"]) {
            formValues["VoiceSoprano"] = "\\clef \"treble\" \\keyTime " + formValues["VoiceSoprano"];
        }
        if (formValues["VoiceAlto"]) {
            formValues["VoiceAlto"] = "\\clef \"treble\" \\keyTime " + formValues["VoiceAlto"];
        }
        if (formValues["VoiceTenor"]) {
            formValues["VoiceTenor"] = "\\clef \"bass\" \\keyTime " + formValues["VoiceTenor"];
        }
        if (formValues["VoiceBass"]) {
            formValues["VoiceBass"] = "\\clef \"bass\" \\keyTime " + formValues["VoiceBass"];
        }

        this.song.Code = JSON.stringify(formValues);
    }

    createSongEmit(formValues: any) {
        this.createSong(formValues);
        this.songVoices.emit(this.song);
    }

    hasError(name: string) {
        return this.voiceForm.controls[name].touched && this.voiceForm.controls[name].invalid;
    }

    hasSuccess(name: string) {
        return this.voiceForm.controls[name].touched && this.voiceForm.controls[name].valid;
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
}