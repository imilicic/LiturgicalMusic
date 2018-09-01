import { Injectable } from "@angular/core";
import { Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/observable/throw";

@Injectable()
export class CommonService {
    handleError(error: Response) {
        console.error(error);
        return Observable.throw(error);
    }
}