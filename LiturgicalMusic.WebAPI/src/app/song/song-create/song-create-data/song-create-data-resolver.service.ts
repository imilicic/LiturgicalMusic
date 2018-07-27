import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { Observable } from "rxjs/Observable";

import { Composer } from "../../shared/models/composer.model";
import { ComposerService } from "../../shared/composer.service";

@Injectable()
export class SongCreateDataResolverService implements Resolve<Composer[]> {
    constructor(private composerService: ComposerService) { }

    resolve(): Observable<Composer[]> {
        return this.composerService.getComposers();
    }
}