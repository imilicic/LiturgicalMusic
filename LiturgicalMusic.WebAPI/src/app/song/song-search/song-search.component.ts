import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";

import { Filter } from "../shared/models/filter.model";
import { Song } from "../shared/models/song.model";
import { SongService } from "../shared/song.service";

@Component({
    templateUrl: "./song-search.component.html"
})
export class SongSearchComponent implements OnInit {
    songs: Song[] = undefined;
    spinner: boolean = false;

    title: FormControl;
    searchForm: FormGroup;

    constructor(private songService: SongService) { }

    ngOnInit() {
        this.title = new FormControl("", Validators.required);

        this.searchForm = new FormGroup({
            title: this.title
        });
    }

    searchSongs(values: any) {
        this.spinner = true;

        let filter: Filter = new Filter();
        filter.Title = values.title;
        console.log(filter);

        this.songService.searchSongs(filter).subscribe((response: Song[]) => {
            this.spinner = false;
            this.songs = response;
        });
    }
}