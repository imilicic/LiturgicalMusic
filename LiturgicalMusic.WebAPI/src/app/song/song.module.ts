import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { NgxPaginationModule } from "ngx-pagination";

import { ComposerResolverService } from "./services/composer-resolver.service";
import { ComposerService } from "./services/composer.service";
import { HymnComponent } from "./views/hymn/hymn.component";
import { SongCreateComponent } from "./views/song-create.component";
import { SongDataComponent } from "./views/song-data/song-data.component";
import { SongEditComponent } from "./views/song-edit.component";
import { SongResolverService } from "./services/song-resolver.service";
import { SongRouteActivatorService } from "./services/song-route-activator.service";
import { SongRoutingModule } from "./song-routing.module";
import { SongSearchComponent } from "./views/song-search.component";
import { SongService } from "./services/song.service";
import { SongViewComponent } from "./views/song-view.component";

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
        SongEditComponent,
        SongSearchComponent,
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