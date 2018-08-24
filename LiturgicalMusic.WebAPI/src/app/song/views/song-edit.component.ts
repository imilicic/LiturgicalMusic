import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";

import { Composer } from "../models/composer.model";
import { Song } from "../models/song.model";
import { SongCommonService } from "../services/song-common.service";
import { SongSessionService } from "../services/song-session.service";

@Component({
    templateUrl: "./song-edit.component.html"
})
export class SongEditComponent implements OnInit {
    composers: Composer[] = [];
    song: Song = undefined;
    liturgyCategories: boolean[] = [false, false, false, false, false, false, false, false, false];
    themeCategories: boolean[] = [false, false, false, false, false, false];

    arranger: FormControl;
    composer: FormControl;
    otherInformations: FormControl;
    source: FormControl;
    title: FormControl;
    type: FormControl;
    songForm: FormGroup;

    constructor(private route: ActivatedRoute, private songCommonService: SongCommonService, private songSessionService: SongSessionService) { }

    ngOnInit() {
        this.composers = this.route.snapshot.data["composers"];
        this.song = this.route.snapshot.data["song"];

        if (this.song == undefined) {
            this.songSessionService.action = "create";
        } else {
            this.songSessionService.action = "edit";

            this.song.LiturgyCategories.forEach(l => this.liturgyCategories[l - 1] = true);
            this.song.ThemeCategories.forEach(t => this.themeCategories[t - 1] = true);
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
            title = this.song.Title;
            type = this.song.Type;
        }

        this.arranger = new FormControl(arrangerId);
        this.composer = new FormControl(composerId);
        this.otherInformations = new FormControl(otherInformations);
        this.source = new FormControl(source);
        this.title = new FormControl(title, Validators.required);
        this.type = new FormControl(type, Validators.required);

        this.songForm = new FormGroup({
            arranger: this.arranger,
            composer: this.composer,
            otherInformations: this.otherInformations,
            source: this.source,
            title: this.title,
            type: this.type
        });
    }

    createSong(formValues: any) {
        let newSong: Song = new Song(this.song);

        newSong.Arranger = this.composers.find(c => c.Id == formValues.arranger);
        newSong.Composer = this.composers.find(c => c.Id == formValues.composer);
        newSong.OtherInformations = formValues.otherInformations;
        newSong.Source = formValues.source;
        newSong.Title = formValues.title;
        newSong.Type = formValues.type;
        newSong.LiturgyCategories = [];
        newSong.ThemeCategories = [];

        this.liturgyCategories.forEach((l, i) => {
            if (l) {
                newSong.LiturgyCategories.push(i + 1);
            }
        });

        this.themeCategories.forEach((l, i) => {
            if (l) {
                newSong.ThemeCategories.push(i + 1);
            }
        });

        this.songSessionService.songSession = newSong;

        if (this.songSessionService.action == "create") {
            this.songSessionService.moveTo("songs/edit/" + newSong.Type);
        } else {
            this.songSessionService.moveTo("songs/edit/" + this.song.Id + "/" + this.song.Type);
        }
    }

    invalidCategories() {
        return !(this.liturgyCategories.some(b => b) || this.themeCategories.some(b => b));
    }

    updateCategory(category: string, i: number) {
        if (category == "liturgy") {
            this.liturgyCategories[i] = !this.liturgyCategories[i];
        } else if (category == "theme") {
            this.themeCategories[i] = !this.themeCategories[i];
        }
    }
}