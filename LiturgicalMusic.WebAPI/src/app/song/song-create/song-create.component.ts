import { Component } from "@angular/core";
import { Router } from "@angular/router";

import { Song } from "../shared/models/song.model";
import { SongService } from "../shared/song.service";

@Component({
    templateUrl: "./song-create.component.html"
})
export class SongCreateComponent {
    currentPage: number = 1;
    song: Song;
    spinner: boolean = false;

    constructor(private router: Router, private songService: SongService) { }

    createSong(newSong: Song) {
        this.currentPage = 0;
        this.spinner = true;

        this.songService.createSong(newSong)
            .subscribe((response: Song) => {
                this.spinner = false;
                this.song = response;
                this.router.navigate(['../search/', this.song.Id]);
            });
    }

    nextPage(event: Song) {
        this.song = event;
        this.currentPage += 1;
    }
}