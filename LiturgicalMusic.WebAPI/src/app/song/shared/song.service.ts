import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/observable/throw";

@Injectable()
export class SongService {
    constructor(private http: Http) { }

    getSongs(): Observable<any> {
        let query = "/api/songs/search";
        return this.http.get(query)
            .map((response: Response) => <any>response.json())
            .catch(this.handleError);
    }

    handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || "Server error");
    }
}