import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { SongSearchComponent } from "./song-search/song-search.component";

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: "search", component: SongSearchComponent }
        ])
    ]
})
export class SongRoutingModule { }