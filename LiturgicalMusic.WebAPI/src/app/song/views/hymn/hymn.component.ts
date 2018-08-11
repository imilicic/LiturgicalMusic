import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { DomSanitizer } from "@angular/platform-browser";

import { InstrumentalPart } from "../../models/instrumentalPart.model";
import { Song } from "../../models/song.model";
import { SongService } from "../../services/song.service";
import { Stanza } from "../../models/stanza.model";

@Component({
    selector: "hymn",
    templateUrl: "./hymn.component.html"
})
export class HymnComponent implements OnInit {
    @Input() song: Song;
    @Output() songVoices = new EventEmitter();
    chosenVoicesOrgan: any[] = [];
    chosenVoicesParts: any[][] = [];
    chosenVoicesVoice: any[] = [];
    lyrics: any[] = [];
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
        let key: string = "";
        let timeNumerator: number = undefined;
        let timeDenominator: number = undefined;
        let code: any = undefined;

        if (this.song.Code != undefined) {
            code = JSON.parse(this.song.Code);
            let time: string = code.Time.split("/");

            timeNumerator = parseInt(time[0]);
            timeDenominator = parseInt(time[1]);
            key = code.Key;
        }

        window.scrollTo(0, 0);
        this.pdfFileName = "app/assets/pdf/" + this.song.Title;

        if (this.song.Composer != undefined) {
            this.pdfFileName += this.song.Composer.Name + this.song.Composer.Surname;
        } else if (this.song.Arranger != undefined) {
            this.pdfFileName += this.song.Arranger.Name + this.song.Arranger.Surname;
        }

        this.pdfFileName += ".pdf";

        let controls: any = {};
        this.song.Template.forEach((b, i) => {
            if (i <= 3) { // SATB voice
                if (b) {
                    let voice: string = undefined;
                    let voiceRelative: string = undefined;

                    this.chosenVoicesVoice.push({
                        voice: this.voices[i],
                        controlName: "Voice" + this.voices[i],
                        controlNameRelative: "Voice" + this.voices[i] + "Relative"
                    });

                    if (code != undefined) {
                        voice = code["Voice" + this.voices[i]];

                        if (voice != undefined) {
                            voice = voice.split("keyTime ")[1];
                        }

                        voiceRelative = code["Voice" + this.voices[i] + "Relative"];
                    }

                    controls["Voice" + this.voices[i]] = new FormControl(voice, Validators.required);
                    controls["Voice" + this.voices[i] + "Relative"] = new FormControl(voiceRelative, Validators.required);
                }
            } else { // SATB organ
                if (b) {
                    let voice: string = undefined;
                    let voiceRelative: string = undefined;

                    this.chosenVoicesOrgan.push({
                        voice: this.voices[i-4],
                        controlName: "Organ" + this.voices[i-4],
                        controlNameRelative: "Organ" + this.voices[i-4] + "Relative"
                    });

                    if (code != undefined) {
                        voice = code["Organ" + this.voices[i - 4]];

                        if (voice != undefined) {
                            voice = voice.split("keyTime ")[1];
                        }

                        voiceRelative = code["Organ" + this.voices[i - 4] + "Relative"];
                    }

                    controls["Organ" + this.voices[i - 4]] = new FormControl(voice, Validators.required);
                    controls["Organ" + this.voices[i - 4] + "Relative"] = new FormControl(voiceRelative, Validators.required);
                }
            }
        });

        if (this.song.InstrumentalParts != undefined) {
            this.song.InstrumentalParts.forEach(p => {
                let voices: any = [];
                let partCode: any = undefined;

                if (p.Code != undefined) {
                    partCode = JSON.parse(p.Code);
                }

                p.Template.forEach((b, i) => {
                    if (b) {
                        let voice: string = undefined;
                        let voiceRelative: string = undefined;

                        voices.push({
                            voice: this.voices[i],
                            controlName: p.Position + "Part" + this.voices[i],
                            controlNameRelative: p.Position + "Part" + this.voices[i] + "Relative",
                            partName: p.Position
                        });

                        if (partCode != undefined) {
                            voice = partCode[this.voices[i]];

                            if (voice != undefined) {
                                voice = voice.split("keyTime ")[1];
                            }

                            voiceRelative = partCode[this.voices[i] + "Relative"];
                        }

                        controls[p.Position + "Part" + this.voices[i]] = new FormControl(voice, Validators.required);
                        controls[p.Position + "Part" + this.voices[i] + "Relative"] = new FormControl(voiceRelative, Validators.required);
                    }
                });
                this.chosenVoicesParts.push(voices);
            });
        }

        this.key = new FormControl(key, Validators.required);
        this.timeNumerator = new FormControl(timeNumerator, Validators.required);
        this.timeDenominator = new FormControl(timeDenominator, Validators.required);

        controls["Key"] = this.key;
        controls["timeNumerator"] = this.timeNumerator;
        controls["timeDenominator"] = this.timeDenominator;

        this.voiceForm = new FormGroup(controls);

        if (this.song.Stanzas != null) {
            this.song.Stanzas.forEach(stanza => {
                let controlName: string = 'stanza' + (this.lyrics.length + 1);

                this.lyrics.push({
                    control: new FormControl(stanza.Text, Validators.required),
                    controlName: controlName
                });

                this.voiceForm.addControl(controlName, this.lyrics[this.lyrics.length - 1].control);
            });
        } else {
            this.lyrics.push({
                control: new FormControl("", Validators.required),
                controlName: 'stanza1'
            });

            this.voiceForm.addControl("stanza1", this.lyrics[0].control);
        }
    }

    appendStanza() {
        this.lyrics.push({
            controlName: 'stanza' + (this.lyrics.length + 1),
            control: new FormControl("", Validators.required)
        });

        this.voiceForm.addControl(this.lyrics[this.lyrics.length - 1].controlName, this.lyrics[this.lyrics.length - 1].control);
    }

    deleteStanza() {
        if (this.lyrics.length > 1) {
            let stanza: any = this.lyrics.pop();
            this.voiceForm.removeControl(stanza.controlName);
        }
    }

    private prependToCode(code: any, copyFrom: any, instrument: string, template: boolean[], keepSameKey: boolean = true) {
        this.voices.forEach((v, i) => {
            if (template[i]) {
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

        code["Time"] = formValues.timeNumerator + "/" + formValues.timeDenominator;
        code["Key"] = formValues.Key;

        this.prependToCode(code, formValues, "Voice", this.song.Template.slice(0, 4));
        this.prependToCode(code, formValues, "Organ", this.song.Template.slice(4, 8));

        if (this.song.InstrumentalParts != undefined) {
            this.song.InstrumentalParts.forEach(part => {
                let newPart: any = {};
                this.prependToCode(newPart, formValues, part.Position + "Part", part.Template, false);
                part.Code = JSON.stringify(newPart);
            });
        }
        
        this.song.Code = JSON.stringify(code);
        let stanzas: Stanza[] = [];

        this.lyrics.forEach((l, i) => {
            let stanza: Stanza = new Stanza();
            let foundStanza: Stanza = undefined;

            if (this.song.Stanzas != undefined) {
                foundStanza = this.song.Stanzas.find(s => s.Number == i + 1);
            }

            stanza = {
                Id: undefined,
                Number: i + 1,
                Text: formValues[l.controlName]
            };

            if (foundStanza != undefined) {
                stanza.Id = foundStanza.Id;
            }

            stanzas.push(stanza);
        });

        this.song.Stanzas = stanzas;
    }

    createSongEmit(formValues: any) {
        this.createSong(formValues);
        this.songVoices.emit(this.song);
    }

    hasError(name: string) {
        return this.voiceForm.controls[name].touched && this.voiceForm.controls[name].invalid;
    }

    hasSuccess(name: string) {
        return this.voiceForm.controls[name].value != undefined && this.voiceForm.controls[name].valid;
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