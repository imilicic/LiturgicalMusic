import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";

import { ComposerService } from "./shared/composer.service";
import { SongCreateComponent } from "./song-create/song-create.component";
import { SongCreateDataComponent } from "./song-create/song-create-data/song-create-data.component";
import { SongCreateDataResolverService } from "./song-create/song-create-data/song-create-data-resolver.service";
import { SongCreateHymnComponent } from "./song-create/song-create-hymn/song-create-hymn.component";
import { SongRoutingModule } from "./song-routing.module";
import { SongSearchComponent } from "./song-search/song-search.component";
import { SongService } from "./shared/song.service";
import { SongViewComponent } from "./song-view/song-view.component";
import { SongViewResolverService } from "./song-view/song-view-resolver.service";
import { SongViewRouteActivatorService } from "./song-view/song-view-route-activator.service";

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule,
        SongRoutingModule
    ],
    declarations: [
        SongCreateComponent,
        SongCreateDataComponent,
        SongCreateHymnComponent,
        SongSearchComponent,
        SongViewComponent
    ],
    providers: [
        ComposerService,
        SongCreateDataResolverService,
        SongService,
        SongViewResolverService,
        SongViewRouteActivatorService
    ]
})
export class SongModule { }