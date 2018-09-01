import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";

import { CommonService } from "./common.service";
import { Composer } from "../models/composer.model";

@Injectable()
export class ComposerService {
    url: string;

    constructor(private commonService: CommonService, private http: Http) {
        this.url = "/api/composers/";
    }

    getComposers(): Observable<Composer[]> {
        return this.http.get(this.url + "get")
            .map((response: Response) => <Composer[]>response.json())
            .catch(this.commonService.handleError);
    }
}