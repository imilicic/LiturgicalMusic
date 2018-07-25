import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router } from "@angular/router";

import { Song } from "../shared/models/song.model";
import { SongService } from "../shared/song.service";

@Injectable()
export class SongViewResolverService implements Resolve<Song>{
    constructor(private songService: SongService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot) {
        return this.songService.getSongById(route.params.songId);
    }
}