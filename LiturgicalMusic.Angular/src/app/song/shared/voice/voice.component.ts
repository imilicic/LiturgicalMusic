import { Component, EventEmitter, Input, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";

import { Template } from "../../models/template.model";

@Component({
    selector: "instrument-voices",
    templateUrl: "./voice.component.html"
})
export class VoiceComponent implements OnInit {
    @Input() code: any;
    @Input() templateVoices: Template[];

    voiceForm: FormGroup;

    ngOnInit() {
        let controls: any = {};

        this.templateVoices.forEach(t => {
            if (t.Use) {
                let voice: string = undefined;
                let voiceRelative: string = undefined;

                if (this.code != undefined) {
                    voice = this.code[t.ControlName];

                    if (voice != undefined) {
                        voice = voice.split("keyTime ")[1];
                    }

                    voiceRelative = this.code[t.ControlNameRelative];
                }

                controls[t.ControlName] = new FormControl(voice, Validators.required);
                controls[t.ControlNameRelative] = new FormControl(voiceRelative, Validators.required);
            }
        });

        this.voiceForm = new FormGroup(controls);
    }

    getFormValues() {
        let code = {};

        this.prependToCode(code, this.voiceForm.value);

        if (Object.keys(code).length === 0) {
            return { Code: undefined, Template: this.templateVoices.map(t => t.Use) };
        } else {
            return { Code: code, Template: this.templateVoices.map(t => t.Use) };
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

    private prependToCode(code: any, copyFrom: any, keepSameKey: boolean = true) {
        this.templateVoices.forEach(t => {
            if (t.Use) {
                if (t.Name == "Soprano" || t.Name == "Alto") {
                    if (keepSameKey) {
                        code[t.Instrument + t.Name] = "\\clef \"treble\" \\keyTime " + copyFrom[t.Instrument + t.Name];
                    } else {
                        code[t.Name] = "\\clef \"treble\" \\keyTime " + copyFrom[t.Instrument + t.Name];
                    }
                } else {
                    if (keepSameKey) {
                        code[t.Instrument + t.Name] = "\\clef \"bass\" \\keyTime " + copyFrom[t.Instrument + t.Name];
                    } else {
                        code[t.Name] = "\\clef \"bass\" \\keyTime " + copyFrom[t.Instrument + t.Name];
                    }
                }

                if (keepSameKey) {
                    code[t.Instrument + t.Name + "Relative"] = copyFrom[t.Instrument + t.Name + "Relative"];
                } else {
                    code[t.Name + "Relative"] = copyFrom[t.Instrument + t.Name + "Relative"];
                }
            }
        });
    }
}