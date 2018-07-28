import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { DomSanitizer } from "@angular/platform-browser";

import { Song } from "../../shared/models/song.model";
import { SongService } from "../../shared/song.service";
import { InstrumentalPart } from "../../shared/models/instrumentalPart.model";

@Component({
    selector: "create-hymn",
    templateUrl: "./song-create-hymn.component.html"
})
export class SongCreateHymnComponent implements OnInit {
    @Input() song: Song;
    @Output() songVoices = new EventEmitter();
    chosenVoicesOrgan: any[] = [];
    chosenVoicesParts: any[][] = [];
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
        this.song.Template.forEach((b, i) => {
            if (i <= 3) { // SATB voice
                if (b) {
                    this.chosenVoicesVoice.push({
                        voice: this.voices[i],
                        controlName: "Voice" + this.voices[i],
                        controlNameRelative: "Voice" + this.voices[i] + "Relative"
                    });

                    controls["Voice" + this.voices[i]] = new FormControl("", Validators.required);
                    controls["Voice" + this.voices[i] + "Relative"] = new FormControl("", Validators.required);
                }
            } else { // SATB organ
                if (b) {
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
        
        if (this.song.InstrumentalParts != undefined) {
            this.song.InstrumentalParts.forEach(p => {
                let voices: any = [];
                p.Template.forEach((b, i) => {
                    if (b) {
                        voices.push({
                            voice: this.voices[i],
                            controlName: p.Position + "Part" + this.voices[i],
                            controlNameRelative: p.Position + "Part" + this.voices[i] + "Relative",
                            partName: p.Position
                        });

                        controls[p.Position + "Part" + this.voices[i]] = new FormControl("", Validators.required);
                        controls[p.Position + "Part" + this.voices[i] + "Relative"] = new FormControl("", Validators.required);
                    }
                });
                this.chosenVoicesParts.push(voices);
            });
        }

        this.key = new FormControl("", Validators.required);
        this.timeNumerator = new FormControl("", Validators.required);
        this.timeDenominator = new FormControl("", Validators.required);

        controls["Key"] = this.key;
        controls["timeNumerator"] = this.timeNumerator;
        controls["timeDenominator"] = this.timeDenominator;

        this.voiceForm = new FormGroup(controls);
    }

    private prependToCode(code: any, copyFrom: any, instrument: string, keepSameKey: boolean = true) {
        this.voices.forEach(v => {
            if (copyFrom[instrument + v]) {
                if (v == "Soprano" || v == "Alto") {
                    if (keepSameKey) {
                        code[instrument + v] = "\\clef \"treble\" \\keyTime " + copyFrom[instrument + v];
                    } else {
                        code[v] = "\\clef \"treble\" \\keyTime " + copyFrom[instrument + v];
                    }
                } else {
                    if (keepSameKey) {
                        code[instrument + v] = "\\clef \"bass\" \\keyTime " + copyFrom[instrument + v];
                    } else {
                        code[v] = "\\clef \"bass\" \\keyTime " + copyFrom[instrument + v];
                    }
                }

                if (keepSameKey) {
                    code[instrument + v + "Relative"] = copyFrom[instrument + v + "Relative"];
                } else {
                    code[v + "Relative"] = copyFrom[instrument + v + "Relative"];
                }
            }
        });
    }

    createSong(formValues: any) {
        let code = {};
        let instrumentalParts: InstrumentalPart[] = [];

        code["Time"] = formValues.timeNumerator + "/" + formValues.timeDenominator;
        code["Key"] = formValues.Key;

        this.prependToCode(code, formValues, "Organ");
        this.prependToCode(code, formValues, "Voice");

        this.song.InstrumentalParts.forEach(part => {
            let newPart: any = {};
            this.prependToCode(newPart, formValues, part.Position + "Part", false);
            part.Code = JSON.stringify(newPart);
        });

        this.song.Code = JSON.stringify(code);
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