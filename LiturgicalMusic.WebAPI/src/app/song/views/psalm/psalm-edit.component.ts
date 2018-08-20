import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";

import { Song } from "../../models/song.model";
import { SongSessionService } from "../../services/song-session.service";
import { Template } from "../../models/template.model";

@Component({
    templateUrl: "./psalm-edit.component.html"
})
export class PsalmEditComponent implements OnInit {
    lyrics: any[] = [];
    song: Song;
    antiphonaTemplateVoices: Template[] = [];
    psalmTemplateVoices: Template[] = [];
    antiphonaTemplate: boolean[] = [false, false, false, false, true, true, true, true];

    key: FormControl;

    voiceForm: FormGroup;

    constructor(private songSessionService: SongSessionService) { }

    ngOnInit() {
        this.song = this.songSessionService.songSession;

        let key: string = "";
        let code: any = undefined;
        let controls: any = {};

        if (this.song.Template == undefined) {
            this.song.Template = [false, false, false, false, true, true, true, true];
        }

        this.antiphonaTemplateVoices.push({
            ControlName: "VoiceSoprano",
            ControlNameRelative: "VoiceSopranoRelative",
            Instrument: "Glas",
            Name: "Sopran",
            Use: this.antiphonaTemplate[0]
        });
        this.antiphonaTemplateVoices.push({
            ControlName: "VoiceAlto",
            ControlNameRelative: "VoiceAltoRelative",
            Instrument: "Glas",
            Name: "Alt",
            Use: this.antiphonaTemplate[1]
        });
        this.antiphonaTemplateVoices.push({
            ControlName: "VoiceTenor",
            ControlNameRelative: "VoiceTenorRelative",
            Instrument: "Glas",
            Name: "Tenor",
            Use: this.antiphonaTemplate[2]
        });
        this.antiphonaTemplateVoices.push({
            ControlName: "VoiceBass",
            ControlNameRelative: "VoiceBassRelative",
            Instrument: "Glas",
            Name: "Bas",
            Use: this.antiphonaTemplate[3]
        });
        this.antiphonaTemplateVoices.push({
            ControlName: "OrganSoprano",
            ControlNameRelative: "OrganSopranoRelative",
            Instrument: "Orgulje",
            Name: "Sopran",
            Use: this.antiphonaTemplate[4]
        });
        this.antiphonaTemplateVoices.push({
            ControlName: "OrganAlto",
            ControlNameRelative: "OrganAltoRelative",
            Instrument: "Orgulje",
            Name: "Alt",
            Use: this.antiphonaTemplate[5]
        });
        this.antiphonaTemplateVoices.push({
            ControlName: "OrganTenor",
            ControlNameRelative: "OrganTenorRelative",
            Instrument: "Orgulje",
            Name: "Tenor",
            Use: this.antiphonaTemplate[6]
        });
        this.antiphonaTemplateVoices.push({
            ControlName: "OrganBass",
            ControlNameRelative: "OrganBassRelative",
            Instrument: "Orgulje",
            Name: "Bas",
            Use: this.antiphonaTemplate[7]
        });

        if (this.song.Code != undefined) {
            code = JSON.parse(this.song.Code);

            key = code.Key;
        }

        this.antiphonaTemplateVoices.forEach(t => {
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

        this.key = new FormControl(key, Validators.required);
        controls["Key"] = this.key;

        this.voiceForm = new FormGroup(controls);
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

    hasError(name: string) {
        return this.voiceForm.controls[name].touched && this.voiceForm.controls[name].invalid;
    }

    hasSuccess(name: string) {
        return this.voiceForm.controls[name].value != undefined && this.voiceForm.controls[name].valid;
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
}