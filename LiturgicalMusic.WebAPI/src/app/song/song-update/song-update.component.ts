import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";

import { Song } from "../shared/models/song.model";
import { SongService } from "../shared/song.service";

@Component({
    templateUrl: "./song-update.component.html"
})
export class SongUpdateComponent implements OnInit {
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
            this.router.navigate(['../../search/', this.song.Id]);
        });
    }
}