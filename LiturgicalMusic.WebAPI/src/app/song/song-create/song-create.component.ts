import { Component } from "@angular/core";

import { Song } from "../shared/models/song.model";
import { SongService } from "../shared/song.service";

@Component({
    templateUrl: "./song-create.component.html"
})
export class SongCreateComponent {
    currentPage: number = 1;
    song: Song;
    created: boolean = false;

    constructor(private songService: SongService) { }

    createSong(newSong: Song) {
        this.currentPage = 0;

        this.songService.createSong(newSong)
            .subscribe((response: Song) => {
                this.song = response;
                this.created = true;
            });
    }

    nextPage(event: Song) {
        this.song = event;
        this.currentPage += 1;
    }
}