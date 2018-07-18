import { Component, OnInit } from "@angular/core";

import { Song } from "../shared/models/song.model";
import { SongService } from "../shared/song.service";

@Component({
    templateUrl: "./song-search.component.html"
})
export class SongSearchComponent implements OnInit {
    songs: Song[] = [];

    constructor(private songService: SongService) { }

    ngOnInit() {
        this.songService.getSongs()
            .subscribe(result => {
                this.songs = result;
            });
    }
}