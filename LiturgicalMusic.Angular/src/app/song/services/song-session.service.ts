import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

import { Song } from "../models/song.model";

@Injectable()
export class SongSessionService {
    action: string;
    song: Song;

    constructor(private router: Router) { }

    moveTo(to: string) {
        this.router.navigate([to]);
    }
}