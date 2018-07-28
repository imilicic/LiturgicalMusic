import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";

import { Composer } from "../../shared/models/composer.model";
import { InstrumentalPart } from "../../shared/models/instrumentalPart.model";
import { Song } from "../../shared/models/song.model";

@Component({
    selector: "create-data",
    templateUrl: "./song-create-data.component.html"
})
export class SongCreateDataComponent implements OnInit {
    composers: Composer[] = [];
    createdSong: Song;
    otherParts: boolean[] = [false, false, false];
    partNames: string[] = ["prelude", "interlude", "coda"];
    partVoices: boolean[][] = [[false, false, false, false], [false, false, false, false], [false, false, false, false]];
    @Output() songData = new EventEmitter();
    templateVoices: boolean[] = [false, false, false, false, true, true, true, true];

    arranger: FormControl;
    composer: FormControl;
    otherInformation: FormControl;
    source: FormControl;
    template: FormControl;
    title: FormControl;
    type: FormControl;

    //stanzas: FormControl;
    //instrumentalParts: FormControl;
    themeCategories: FormControl;
    liturgyCategories: FormControl;
    songForm: FormGroup;

    constructor(private route: ActivatedRoute) { }

    ngOnInit() {
        this.composers = this.route.snapshot.data["composers"];

        this.arranger = new FormControl();
        this.composer = new FormControl();
        this.otherInformation = new FormControl();
        this.source = new FormControl();
        this.template = new FormControl(this.templateVoices, Validators.required);
        this.title = new FormControl("", Validators.required);
        this.type = new FormControl("", Validators.required);

        this.songForm = new FormGroup({
            arranger: this.arranger,
            composer: this.composer,
            otherInformation: this.otherInformation,
            source: this.source,
            template: this.template,
            title: this.title,
            type: this.type
        });
    }

    createSong(formValues: any) {
        let newSong: Song = {
            Arranger: this.composers.find(c => c.Id == formValues.arranger),
            Composer: this.composers.find(c => c.Id == formValues.composer),
            OtherInformation: formValues.otherInformation,
            Source: formValues.source,
            Template: formValues.template,
            Title: formValues.title,
            Type: formValues.type,

            Code: undefined,
            Id: undefined,
            Stanzas: undefined,
            InstrumentalParts: undefined,
            ThemeCategories: undefined,
            LiturgyCategories: undefined
        }

        if (this.otherParts.some(b => b)) {
            newSong.InstrumentalParts = [];

            this.otherParts.forEach((b, i) => {
                if (b) {
                    let part = new InstrumentalPart();
                    part.Position = this.partNames[i];
                    part.Template = this.partVoices[i];
                    part.Id = undefined;
                    part.Type = newSong.Type;

                    newSong.InstrumentalParts.push(part);
                }
            });
        }
        this.songData.emit(newSong);
    }

    voicesInvalid() {
        return !this.templateVoices.some(b => b);
    }

    partsInvalid() {
        for (let i = 0; i < this.otherParts.length; i++) {
            if (this.otherParts[i]) {
                if (!this.partVoices[i].some(b => b)) {
                    return true;
                }
            }
        }

        return false;
    }

    otherPartsCheck(id: number) {
        this.otherParts[id] = !this.otherParts[id];
    }

    updateTemplate(position: number) {
        this.templateVoices[position] = !this.templateVoices[position];
    }

    updatePartTemplate(part: number, position: number) {
        this.partVoices[part][position] = !this.partVoices[part][position];
    }
}