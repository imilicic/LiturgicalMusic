import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";

import { Composer } from "../models/composer.model";
import { InstrumentalPart } from "../models/instrumentalPart.model";
import { Song } from "../models/song.model";
import { SongSessionService } from "../services/song-session.service";

@Component({
    templateUrl: "./song-edit.component.html"
})
export class SongEditComponent implements OnInit {
    composers: Composer[] = [];
    otherParts: boolean[] = [false, false, false];
    partNames: string[] = ["prelude", "interlude", "coda"];
    partVoices: boolean[][] = [[false, false, false, false], [false, false, false, false], [false, false, false, false]];
    song: Song = undefined;
    templateVoices: boolean[] = [false, false, false, false, true, true, true, true];

    arranger: FormControl;
    composer: FormControl;
    otherInformations: FormControl;
    source: FormControl;
    template: FormControl;
    title: FormControl;
    type: FormControl;
    songForm: FormGroup;

    themeCategories: FormControl;
    liturgyCategories: FormControl;

    constructor(private route: ActivatedRoute, private songSessionService: SongSessionService) { }

    ngOnInit() {
        this.composers = this.route.snapshot.data["composers"];
        this.song = this.route.snapshot.data["song"];

        if (this.song == undefined) {
            this.songSessionService.action = "create";
        } else {
            this.songSessionService.action = "edit";
        }

        let arrangerId: number = undefined;
        let composerId: number = undefined;
        let otherInformations: string = undefined;
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

            otherInformations = this.song.OtherInformations;
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
        this.otherInformations = new FormControl(otherInformations);
        this.source = new FormControl(source);
        this.template = new FormControl(this.templateVoices, Validators.required);
        this.title = new FormControl(title, Validators.required);
        this.type = new FormControl(type, Validators.required);

        this.songForm = new FormGroup({
            arranger: this.arranger,
            composer: this.composer,
            otherInformations: this.otherInformations,
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
        newSong.OtherInformations = formValues.otherInformations;
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

        this.songSessionService.songSession = newSong;

        if (this.songSessionService.action == "create") {
            this.songSessionService.moveTo("songs/edit/hymn");
        } else {
            this.songSessionService.moveTo("songs/edit/" + this.song.Id + "/hymn");
        }
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