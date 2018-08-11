import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";

import { Song } from "../models/song.model";
import { SongService } from "../services/song.service";

@Component({
    templateUrl: "./song-edit.component.html"
})
export class SongEditComponent implements OnInit {
    currentPage: number = 1;
    song: Song;
    spinner: boolean = false;

    constructor(private route: ActivatedRoute, private router: Router, private songService: SongService) { }

    ngOnInit() {
        this.song = this.route.snapshot.data.song;
    }

    nextPage(event: Song) {
        this.currentPage += 1;
        this.song = event;
    }

    updateSong(event: Song) {
        this.spinner = true;
        this.currentPage = 0;

        this.songService.updateSong(event).subscribe((response: Song) => {
            this.spinner = false;
            this.song = response;
            this.router.navigate(['songs/view/', this.song.Id]);
        });
    }
}