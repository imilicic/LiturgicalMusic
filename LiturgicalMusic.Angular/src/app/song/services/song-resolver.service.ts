import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router } from "@angular/router";
import { Observable } from "rxjs/Observable";

import { Song } from "../models/song.model";
import { SongService } from "./song.service";

@Injectable()
export class SongResolverService implements Resolve<Song>{
    constructor(private songService: SongService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Song> {
        return this.songService.getSongById(route.params.songId);
    }
}