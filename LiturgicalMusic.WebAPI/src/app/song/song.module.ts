import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { NgxPaginationModule } from "ngx-pagination";

import { ComposerResolverService } from "./shared/composer-resolver.service";
import { ComposerService } from "./shared/composer.service";
import { HymnComponent } from "./shared/hymn/hymn.component";
import { SongCreateComponent } from "./song-create/song-create.component";
import { SongDataComponent } from "./shared/song-data/song-data.component";
import { SongResolverService } from "./shared/song-resolver.service";
import { SongRouteActivatorService } from "./shared/song-route-activator.service";
import { SongRoutingModule } from "./song-routing.module";
import { SongSearchComponent } from "./song-search/song-search.component";
import { SongService } from "./shared/song.service";
import { SongUpdateComponent } from "./song-update/song-update.component";
import { SongViewComponent } from "./song-view/song-view.component";

@NgModule({
    imports: [
        CommonModule,
        NgxPaginationModule,
        ReactiveFormsModule,
        RouterModule,
        SongRoutingModule
    ],
    declarations: [
        HymnComponent,
        SongCreateComponent,
        SongDataComponent,
        SongSearchComponent,
        SongUpdateComponent,
        SongViewComponent
    ],
    providers: [
        ComposerResolverService,
        ComposerService,
        SongResolverService,
        SongRouteActivatorService,
        SongService
    ]
})
export class SongModule { }