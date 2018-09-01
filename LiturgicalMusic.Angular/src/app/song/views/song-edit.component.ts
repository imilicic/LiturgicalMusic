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
    composers: Composer[];
    existingSong: Song;
    liturgyCategories: boolean[];
    themeCategories: boolean[];

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
        this.existingSong = this.route.snapshot.data["song"];

        this.liturgyCategories = Array(this.songCommonService.liturgyCategories.length).fill(false);
        this.themeCategories = Array(this.songCommonService.themeCategories.length).fill(false);

        if (this.existingSong == undefined) {
            this.songSessionService.action = "create";
            this.songSessionService.song = new Song();
        } else {
            this.songSessionService.song = new Song(this.existingSong);
            this.songSessionService.action = "edit";

            this.existingSong.LiturgyCategories.forEach(l => this.liturgyCategories[l - 1] = true);
            this.existingSong.ThemeCategories.forEach(t => this.themeCategories[t - 1] = true);
        }

        this.buildForm();
    }

    private buildForm() {
        this.arranger = new FormControl(this.songSessionService.song.Arranger.Id);
        this.composer = new FormControl(this.songSessionService.song.Composer.Id);
        this.otherInformations = new FormControl(this.songSessionService.song.OtherInformations);
        this.source = new FormControl(this.songSessionService.song.Source);
        this.title = new FormControl(this.songSessionService.song.Title, Validators.required);
        this.type = new FormControl(this.songSessionService.song.Type, Validators.required);

        this.songForm = new FormGroup({
            arranger: this.arranger,
            composer: this.composer,
            otherInformations: this.otherInformations,
            source: this.source,
            title: this.title,
            type: this.type
        });
    }

    createUpdateSong(formValues: any) {
        this.songSessionService.song.Arranger = this.composers.find(c => c.Id == formValues.arranger);
        this.songSessionService.song.Composer = this.composers.find(c => c.Id == formValues.composer);
        this.songSessionService.song.OtherInformations = formValues.otherInformations;
        this.songSessionService.song.Source = formValues.source;
        this.songSessionService.song.Title = formValues.title;
        this.songSessionService.song.Type = formValues.type;
        this.songSessionService.song.LiturgyCategories = [];
        this.songSessionService.song.ThemeCategories = [];

        this.liturgyCategories.forEach((l, i) => {
            if (l) {
                this.songSessionService.song.LiturgyCategories.push(i + 1);
            }
        });

        this.themeCategories.forEach((l, i) => {
            if (l) {
                this.songSessionService.song.ThemeCategories.push(i + 1);
            }
        });

        if (this.songSessionService.action == "create") {
            this.songSessionService.moveTo("songs/edit/" + this.songSessionService.song.Type);
        } else {
            this.songSessionService.moveTo("songs/edit/" + this.existingSong.Id + "/" + this.existingSong.Type);
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