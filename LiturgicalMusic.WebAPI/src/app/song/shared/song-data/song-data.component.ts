import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";

import { Composer } from "../../shared/models/composer.model";
import { InstrumentalPart } from "../../shared/models/instrumentalPart.model";
import { Song } from "../../shared/models/song.model";

@Component({
    selector: "song-data",
    templateUrl: "./song-data.component.html"
})
export class SongDataComponent implements OnInit {
    composers: Composer[] = [];
    otherParts: boolean[] = [false, false, false];
    partNames: string[] = ["prelude", "interlude", "coda"];
    partVoices: boolean[][] = [[false, false, false, false], [false, false, false, false], [false, false, false, false]];
    @Input() song: Song = undefined;
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
        let arrangerId: number = undefined;
        let composerId: number = undefined;
        let otherInformation: string = undefined;
        let source: string = undefined;
        let title: string = undefined;
        let type: string = undefined;

        if (this.song != undefined) {
            if (this.song.Arranger) {
                arrangerId = this.song.Arranger.Id;
            }

            if (this.song.Composer) {
                composerId = this.song.Composer.Id;
            }

            otherInformation = this.song.OtherInformation;
            source = this.song.Source;
            this.templateVoices = this.song.Template;
            title = this.song.Title;
            type = this.song.Type;
            
            if (this.song.InstrumentalParts != undefined) {
                this.song.InstrumentalParts.forEach(part => {
                    let i: number = this.partNames.indexOf(part.Position);
                    this.otherParts[i] = true;
                    this.partVoices[i] = part.Template;
                });
            }
        }

        this.arranger = new FormControl(arrangerId);
        this.composer = new FormControl(composerId);
        this.otherInformation = new FormControl(otherInformation);
        this.source = new FormControl(source);
        this.template = new FormControl(this.templateVoices, Validators.required);
        this.title = new FormControl(title, Validators.required);
        this.type = new FormControl(type, Validators.required);

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
        let newSong: Song = new Song(this.song);

        newSong.Arranger = this.composers.find(c => c.Id == formValues.arranger);
        newSong.Composer = this.composers.find(c => c.Id == formValues.composer);
        newSong.InstrumentalParts = undefined;
        newSong.OtherInformation = formValues.otherInformation;
        newSong.Source = formValues.source;
        newSong.Template = formValues.template;
        newSong.Title = formValues.title;
        newSong.Type = formValues.type;

        if (this.otherParts.some(b => b)) {
            newSong.InstrumentalParts = [];

            this.otherParts.forEach((b, i) => {
                if (b) {
                    let part: InstrumentalPart;

                    if (this.song) {
                        part = new InstrumentalPart(this.song.InstrumentalParts.find(p => p.Position == this.partNames[i]));
                    } else {
                        part = new InstrumentalPart();
                    }

                    part.Position = this.partNames[i];
                    part.Template = this.partVoices[i];
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