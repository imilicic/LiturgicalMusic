import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";

import { Filter } from "../models/filter.model";
import { Song } from "../models/song.model";
import { CommonService } from "./common.service";

@Injectable()
export class SongService {
    headers: Headers;
    options: RequestOptions;
    url: string;

    constructor(private commonService: CommonService, private http: Http) {
        this.initialize();
    }

    private initialize() {
        this.headers = new Headers({ 'Content-Type': 'application/json' });
        this.options = new RequestOptions({ headers: this.headers });
        this.url = "/api/songs/";
    }

    createSong(song: Song): Observable<Song> {
        return this.http.post(this.url + "create", JSON.stringify(song), this.options)
            .map((response: Response) => <Song>response.json())
            .catch(this.commonService.handleError);
    }

    deleteSong(songId: number) {
        return this.http.delete(this.url + "delete/" + songId, this.options)
            .catch(this.commonService.handleError);
    }

    getSongById(songId: number): Observable<Song> {
        let query = this.url + "get?songId=" + songId;
        query += "&&options=Arranger";
        query += "&&options=Composer";
        query += "&&options=Stanzas";
        query += "&&options=InstrumentalParts";
        query += "&&options=LiturgyCategories";
        query += "&&options=ThemeCategories";

        return this.http.get(query)
            .map((response: Response) => <Song>response.json())
            .catch(this.commonService.handleError);
    }

    searchSongs(filter: Filter, orderBy: string, ascending: boolean, pageNumber: number, pageSize: number): Observable<Response> {
        let query = this.url + "get?searchQuery=" + filter.Title;
        query += "&&options=Composer";
        query += "&&options=Arranger";
        query += "&&orderBy=" + orderBy;
        query += "&&ascending=" + ascending;
        query += "&&pageNumber=" + pageNumber;
        query += "&&pageSize=" + pageSize;

        return this.http.get(query)
            .catch(this.commonService.handleError);
    }

    previewSong(song: Song) {
        return this.http.post(this.url + "preview/", JSON.stringify(song), this.options)
            .map((response: Response) => <Song>response.json())
            .catch(this.commonService.handleError);
    }

    updateSong(song: Song) {
        return this.http.put(this.url + "update/" + song.Id, JSON.stringify(song), this.options)
            .map((response: Response) => <Song>response.json())
            .catch(this.commonService.handleError);
    }
}