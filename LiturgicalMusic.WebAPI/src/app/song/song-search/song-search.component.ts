import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Response } from "@angular/http";
import { PaginatePipe } from "ngx-pagination";

import { Filter } from "../shared/models/filter.model";
import { Song } from "../shared/models/song.model";
import { SongService } from "../shared/song.service";

@Component({
    templateUrl: "./song-search.component.html"
})
export class SongSearchComponent implements OnInit {
    pageNumber: number = 1;
    pageSize: number = 20;
    songs: Song[] = undefined;
    spinner: boolean = false;
    songCount: number = 0;

    ascending: FormControl;
    orderBy: FormControl;
    title: FormControl;
    searchForm: FormGroup;

    constructor(private songService: SongService) { }

    ngOnInit() {
        this.ascending = new FormControl(true, Validators.required);
        this.orderBy = new FormControl("title", Validators.required);
        this.title = new FormControl("", Validators.required);

        this.searchForm = new FormGroup({
            ascending: this.ascending,
            orderBy: this.orderBy,
            title: this.title
        });
    }

    pageChanged(page: number) {
        this.pageNumber = page;
        this.searchSongs(this.searchForm.value);
    }

    searchSongs(values: any) {
        this.spinner = true;

        let filter: Filter = new Filter();
        filter.Title = values.title;

        this.songService.searchSongs(filter, values.orderBy, values.ascending, this.pageNumber, this.pageSize).subscribe((response: Response) => {
            this.spinner = false;
            this.songCount = parseInt(response.headers.get("Song-count"));
            this.songs = response.json();
        });
    }
}