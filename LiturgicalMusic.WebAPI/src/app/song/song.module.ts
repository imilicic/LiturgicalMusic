import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";

import { ComposerService } from "./shared/composer.service";
import { SongCreateComponent } from "./song-create/song-create.component";
import { SongCreateDataComponent } from "./song-create/song-create-data/song-create-data.component";
import { SongCreateHymnComponent } from "./song-create/song-create-hymn/song-create-hymn.component";
import { SongRoutingModule } from "./song-routing.module";
import { SongSearchComponent } from "./song-search/song-search.component";
import { SongService } from "./shared/song.service";

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        SongRoutingModule
    ],
    declarations: [
        SongCreateComponent,
        SongCreateDataComponent,
        SongCreateHymnComponent,
        SongSearchComponent
    ],
    providers: [
        ComposerService,
        SongService
    ]
})
export class SongModule { }