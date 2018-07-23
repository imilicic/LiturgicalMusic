import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";

import { Song } from "../../shared/models/song.model";

@Component({
    selector: "create-hymn",
    templateUrl: "./song-create-hymn.component.html"
})
export class SongCreateHymnComponent implements OnInit {
    @Input() song: Song;
    @Output() songVoices = new EventEmitter();
    voices: any[] = ["Soprano", "Alto", "Tenor", "Bass"];
    chosenVoicesOrgan: any[] = [];
    chosenVoicesVoice: any[] = [];

    key: FormControl;
    timeNumerator: FormControl;
    timeDenominator: FormControl;
    voiceForm: FormGroup;

    ngOnInit() {
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
        this.songVoices.emit(this.song);
    }
}