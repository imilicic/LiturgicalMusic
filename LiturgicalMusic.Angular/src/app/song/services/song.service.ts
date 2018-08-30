import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/observable/throw";

import { Filter } from "../models/filter.model";
import { Song } from "../models/song.model";

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

    deleteSong(songId: number) {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.delete("/api/songs/delete/" + songId, options)
            .catch(this.handleError);
    }

    getSongById(songId: number): Observable<Song> {
        let query = "/api/songs/get?songId=" + songId;

        query += "&&options=Arranger";
        query += "&&options=Composer";
        query += "&&options=Stanzas";
        query += "&&options=InstrumentalParts";
        query += "&&options=LiturgyCategories";
        query += "&&options=ThemeCategories";

        return this.http.get(query)
            .map((response: Response) => <Song>response.json())
            .catch(this.handleError);
    }

    searchSongs(filter: Filter, orderBy: string, ascending: boolean, pageNumber: number, pageSize: number): Observable<Response> {
        let query = "/api/songs/get?searchQuery=" + filter.Title;

        query += "&&options=Composer&&options=Arranger"

        query += "&&orderBy=" + orderBy + "&&ascending=" + ascending + "&&pageNumber=" + pageNumber + "&&pageSize=" + pageSize;

        return this.http.get(query)
            .catch(this.handleError);
    }

    handleError(error: Response) {
        console.error(error);
        return Observable.throw(error);
    }

    previewSong(song: Song) {
        let headers = new Headers({ 'Content-Type': 'application/json'});
        let options = new RequestOptions({ headers: headers });

        return this.http.post("/api/songs/preview/", JSON.stringify(song), options)
            .map((response: Response) => <Song>response.json())
            .catch(this.handleError);
    }

    updateSong(song: Song) {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.put("/api/songs/update/" + song.Id, JSON.stringify(song), options)
            .map((response: Response) => <Song>response.json())
            .catch(this.handleError);
    }
}