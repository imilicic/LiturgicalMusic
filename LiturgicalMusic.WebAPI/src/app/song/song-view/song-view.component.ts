import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { DomSanitizer } from "@angular/platform-browser";

import { Song } from "../shared/models/song.model";

@Component({
    templateUrl: "./song-view.component.html"
})
export class SongViewComponent implements OnInit {
    pdfFileName: string;
    song: Song;

    constructor(private activatedRoute: ActivatedRoute, private domSanitizer: DomSanitizer) { }

    ngOnInit() {
        this.song = this.activatedRoute.snapshot.data.song;
        this.pdfFileName = "app/assets/pdf/" + this.song.Title;

        if (this.song.Composer != null) {
            this.pdfFileName += this.song.Composer.Name + this.song.Composer.Surname;
        } else if (this.song.Arranger != null) {
            this.pdfFileName += this.song.Arranger.Name + this.song.Arranger.Surname;
        }

        this.pdfFileName += ".pdf";
    }
}