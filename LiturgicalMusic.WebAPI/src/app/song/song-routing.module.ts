import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { SongCreateComponent } from "./song-create/song-create.component";
import { SongSearchComponent } from "./song-search/song-search.component";

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: "search", component: SongSearchComponent },
            { path: "create", component: SongCreateComponent }
        ])
    ]
})
export class SongRoutingModule { }