import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { NgxPaginationModule } from "ngx-pagination";

import { ComposerResolverService } from "./services/composer-resolver.service";
import { ComposerService } from "./services/composer.service";
import { HymnEditComponent } from "./views/hymn/hymn-edit.component";
import { LyricsComponent } from "./views/lyrics/lyrics.component";
import { PsalmEditComponent } from "./views/psalm/psalm-edit.component";
import { SongCommonService } from "./services/song-common.service";
import { SongEditComponent } from "./views/song-edit.component";
import { SongResolverService } from "./services/song-resolver.service";
import { SongRouteActivatorService } from "./services/song-route-activator.service";
import { SongRoutingModule } from "./song-routing.module";
import { SongSearchComponent } from "./views/song-search.component";
import { SongService } from "./services/song.service";
import { SongSessionService } from "./services/song-session.service";
import { SongViewComponent } from "./views/song-view.component";
import { VoiceComponent } from "./views/voice/voice.component";

@NgModule({
    imports: [
        CommonModule,
        NgxPaginationModule,
        ReactiveFormsModule,
        RouterModule,
        SongRoutingModule
    ],
    declarations: [
        HymnEditComponent,
        LyricsComponent,
        PsalmEditComponent,
        SongEditComponent,
        SongSearchComponent,
        SongViewComponent,
        VoiceComponent
    ],
    providers: [
        ComposerResolverService,
        ComposerService,
        SongCommonService,
        SongSessionService,
        SongResolverService,
        SongRouteActivatorService,
        SongService
    ]
})
export class SongModule { }