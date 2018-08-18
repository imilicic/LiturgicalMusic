import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { DomSanitizer } from "@angular/platform-browser";

import { InstrumentalPart } from "../../models/instrumentalPart.model";
import { Song } from "../../models/song.model";
import { SongService } from "../../services/song.service";
import { SongSessionService } from "../../services/song-session.service";
import { Stanza } from "../../models/stanza.model";

class Template {
    ControlName: string;
    ControlNameRelative: string;
    Instrument: string;
    Name: string;
    Use: boolean;
}

@Component({
    templateUrl: "./hymn-edit.component.html"
})
export class HymnEditComponent implements OnInit {
    song: Song;
    chosenVoicesParts: any[][] = [];
    lyrics: any[] = [];
    pdfFileName: string;
    preview: boolean = false;
    templateVoices: Template[] = [];
    spinner: boolean = false;
    voices: any[] = ["Soprano", "Alto", "Tenor", "Bass"];

    key: FormControl;
    template: FormControl;
    timeNumerator: FormControl;
    timeDenominator: FormControl;
    voiceForm: FormGroup;

    constructor(private domSanitizer: DomSanitizer, private songService: SongService, private songSessionService: SongSessionService) { }

    ngOnInit() {
        this.song = this.songSessionService.songSession;

        let key: string = "";
        let timeNumerator: number = undefined;
        let timeDenominator: number = undefined;
        let code: any = undefined;
        let controls: any = {};

        if (this.song.Template == undefined) {
            this.song.Template = [false, false, false, false, true, true, true, true];
        }

        this.templateVoices.push({
            ControlName: "VoiceSoprano",
            ControlNameRelative: "VoiceSopranoRelative",
            Instrument: "Glas",
            Name: "Sopran",
            Use: this.song.Template[0]
        });
        this.templateVoices.push({
            ControlName: "VoiceAlto",
            ControlNameRelative: "VoiceAltoRelative",
            Instrument: "Glas",
            Name: "Alt",
            Use: this.song.Template[1]
        });
        this.templateVoices.push({
            ControlName: "VoiceTenor",
            ControlNameRelative: "VoiceTenorRelative",
            Instrument: "Glas",
            Name: "Tenor",
            Use: this.song.Template[2]
        });
        this.templateVoices.push({
            ControlName: "VoiceBass",
            ControlNameRelative: "VoiceBassRelative",
            Instrument: "Glas",
            Name: "Bas",
            Use: this.song.Template[3]
        });
        this.templateVoices.push({
            ControlName: "OrganSoprano",
            ControlNameRelative: "OrganSopranoRelative",
            Instrument: "Orgulje",
            Name: "Sopran",
            Use: this.song.Template[4]
        });
        this.templateVoices.push({
            ControlName: "OrganAlto",
            ControlNameRelative: "OrganAltoRelative",
            Instrument: "Orgulje",
            Name: "Alt",
            Use: this.song.Template[5]
        });
        this.templateVoices.push({
            ControlName: "OrganTenor",
            ControlNameRelative: "OrganTenorRelative",
            Instrument: "Orgulje",
            Name: "Tenor",
            Use: this.song.Template[6]
        });
        this.templateVoices.push({
            ControlName: "OrganBass",
            ControlNameRelative: "OrganBassRelative",
            Instrument: "Orgulje",
            Name: "Bas",
            Use: this.song.Template[7]
        });

        if (this.song.Code != undefined) {
            code = JSON.parse(this.song.Code);
            let time: string = code.Time.split("/");

            timeNumerator = parseInt(time[0]);
            timeDenominator = parseInt(time[1]);
            key = code.Key;
        }

        this.templateVoices.forEach(t => {
            if (t.Use) {
                let voice: string = undefined;
                let voiceRelative: string = undefined;

                if (code != undefined) {
                    voice = code[t.ControlName];

                    if (voice != undefined) {
                        voice = voice.split("keyTime ")[1];
                    }

                    voiceRelative = code[t.ControlNameRelative];
                }

                controls[t.ControlName] = new FormControl(voice, Validators.required);
                controls[t.ControlNameRelative] = new FormControl(voiceRelative, Validators.required);
            }
        });

        window.scrollTo(0, 0);
        this.pdfFileName = "app/assets/pdf/" + this.song.Title;

        if (this.song.Composer != undefined) {
            this.pdfFileName += this.song.Composer.Name + this.song.Composer.Surname;
        } else if (this.song.Arranger != undefined) {
            this.pdfFileName += this.song.Arranger.Name + this.song.Arranger.Surname;
        }

        this.pdfFileName += ".pdf";

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

        this.song.Template = this.templateVoices.map(t => t.Use);

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

    updateTemplate(template: Template) {
        template.Use = !template.Use;

        if (template.Use) {
            this.voiceForm.addControl(template.ControlName, new FormControl("", Validators.required));
            this.voiceForm.addControl(template.ControlNameRelative, new FormControl("", Validators.required));
        } else {
            this.voiceForm.removeControl(template.ControlName);
            this.voiceForm.removeControl(template.ControlNameRelative);
        }
    }

    voicesInvalid() {
        return !this.templateVoices.some(t => t.Use);
    }
}