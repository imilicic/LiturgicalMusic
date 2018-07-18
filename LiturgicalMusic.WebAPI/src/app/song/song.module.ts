import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";

import { SongService } from "./shared/song.service";
import { SongRoutingModule } from "./song-routing.module";
import { SongSearchComponent } from "./song-search/song-search.component";

@NgModule({
    imports: [
        CommonModule,
        SongRoutingModule
    ],
    declarations: [
        SongSearchComponent
    ],
    providers: [
        SongService
    ]
})
export class SongModule { }