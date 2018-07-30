import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router } from "@angular/router";

import { SongService } from "../shared/song.service";
import { Song } from "../shared/models/song.model";

@Injectable()
export class SongRouteActivatorService implements CanActivate {
    constructor(private songService: SongService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot) {
        this.songService.getSongById(route.params.songId).subscribe((song: Song) => {
            this.router.routerState.root.data["song"] = song;
        });

        return true;
    }
}