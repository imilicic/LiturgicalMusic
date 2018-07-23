import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/observable/throw";

import { Composer } from "./models/composer.model";

@Injectable()
export class ComposerService {
    constructor(private http: Http) { }

    getComposers(): Observable<Composer[]> {
        let query = "/api/composers/get";
        return this.http.get(query)
            .map((response: Response) => <Composer[]>response.json())
            .catch(this.handleError);
    }

    handleError(error: Response) {
        console.error(error);
        return Observable.throw(error);
    }
}