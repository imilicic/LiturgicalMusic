import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { DomSanitizer } from "@angular/platform-browser";

import { Song } from "../models/song.model";
import { SongCommonService } from "../services/song-common.service";

@Component({
    templateUrl: "./song-view.component.html"
})
export class SongViewComponent implements OnInit {
    pdfFileName: string;
    song: Song;

    constructor(private activatedRoute: ActivatedRoute, private domSanitizer: DomSanitizer, private songCommonService: SongCommonService) { }

    ngOnInit() {
        this.song = this.activatedRoute.snapshot.data.song;
        this.pdfFileName = this.songCommonService.createPdfFileName(this.song);
    }
}