import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/observable/throw";

import { Song } from "./models/song.model";

@Injectable()
export class SongService {
    constructor(private http: Http) { }

    createSong(song: Song): Observable<Song> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.post("/api/songs/create", JSON.stringify(song), options)
            .map((response: Response) => <Song>response.json())
            .catch(this.handleError);
    }

    getSongById(songId: number): Observable<Song> {
        let query = "/api/songs/get?songId=" + songId;
        return this.http.get(query)
            .map((response: Response) => <Song>response.json())
            .catch(this.handleError);
    }

    getSongs(): Observable<Song[]> {
        let query = "/api/songs/search";
        return this.http.get(query)
            .map((response: Response) => <Song[]>response.json())
            .catch(this.handleError);
    }

    handleError(error: Response) {
        console.error(error);
        return Observable.throw(error);
    }

    previewSong(song: Song) {
        let headers = new Headers({ 'Content-Type': 'application/json'});
        let options = new RequestOptions({ headers: headers });

        return this.http.post("/api/songs/preview", JSON.stringify(song), options)
            .map((response: Response) => <Song>response.json())
            .catch(this.handleError);
    }

    updateSong(song: Song) {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.put("/api/songs/create", JSON.stringify(song), options)
            .map((response: Response) => <Song>response.json())
            .catch(this.handleError);
    }
}