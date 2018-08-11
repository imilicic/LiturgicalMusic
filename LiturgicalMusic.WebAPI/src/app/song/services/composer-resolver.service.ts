import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { Observable } from "rxjs/Observable";

import { Composer } from "../models/composer.model";
import { ComposerService } from "./composer.service";

@Injectable()
export class ComposerResolverService implements Resolve<Composer[]> {
    constructor(private composerService: ComposerService) { }

    resolve(): Observable<Composer[]> {
        return this.composerService.getComposers();
    }
}