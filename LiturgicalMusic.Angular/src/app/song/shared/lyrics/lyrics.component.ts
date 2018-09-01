import { Component, Input, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";

import { Stanza } from "../../models/stanza.model";

@Component({
    selector: "song-lyrics",
    templateUrl: "./lyrics.component.html"
})
export class LyricsComponent implements OnInit {
    @Input() changeable: boolean = true;
    lyrics: any[] = [];
    @Input() maxStanzas: number = 20;
    @Input() stanzas: Stanza[];

    lyricsForm: FormGroup;

    ngOnInit() {
        this.lyricsForm = new FormGroup({});

        if (this.stanzas != undefined && this.stanzas.length > 0) {
            this.stanzas.forEach(stanza => {
                this.appendStanza(stanza.Text);
            });
        } else {
            this.appendStanza();
        }
    }

    appendStanza(text: string = "") {
        this.lyrics.push({
            controlName: 'stanza' + (this.lyrics.length + 1),
            control: new FormControl(text, Validators.required)
        });

        this.lyricsForm.addControl(this.lyrics[this.lyrics.length - 1].controlName, this.lyrics[this.lyrics.length - 1].control);
    }

    deleteStanza() {
        if (this.lyrics.length > 1) {
            let stanza: any = this.lyrics.pop();
            this.lyricsForm.removeControl(stanza.controlName);
        }
    }

    getFormValues() {
        let stanzas: Stanza[] = [];

        this.lyrics.forEach((l, i) => {
            let stanza: Stanza = new Stanza();
            let foundStanza: Stanza = undefined;

            if (this.stanzas != undefined) {
                foundStanza = this.stanzas.find(s => s.Number == i + 1);
            }

            stanza = {
                Id: undefined,
                Number: i + 1,
                Text: this.lyricsForm.controls[l.controlName].value
            };

            if (foundStanza != undefined) {
                stanza.Id = foundStanza.Id;
            }

            stanzas.push(stanza);
        });

        return stanzas;
    }

    hasError(name: string) {
        return this.lyricsForm.controls[name].touched && this.lyricsForm.controls[name].invalid;
    }

    hasSuccess(name: string) {
        return this.lyricsForm.controls[name].value != undefined && this.lyricsForm.controls[name].valid;
    }
}