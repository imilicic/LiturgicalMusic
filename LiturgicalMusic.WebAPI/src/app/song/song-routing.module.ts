import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { SongCreateComponent } from "./song-create/song-create.component";
import { ComposerResolverService } from "./shared/composer-resolver.service";
import { SongSearchComponent } from "./song-search/song-search.component";
import { SongUpdateComponent } from "./song-update/song-update.component";
import { SongViewComponent } from "./song-view/song-view.component";
import { SongRouteActivatorService } from "./shared/song-route-activator.service";
import { SongResolverService } from "./shared/song-resolver.service";

@NgModule({
    imports: [
        RouterModule.forChild([
            //{ path: "search/:songId", component: SongViewComponent, canActivate: [SongRouteActivatorService], resolve: { song: SongResolverService } },
            { path: "search/:songId", component: SongViewComponent, resolve: { song: SongResolverService } },
            { path: "search", component: SongSearchComponent },
            { path: "create/:songId", component: SongUpdateComponent, canActivate: [SongRouteActivatorService], resolve: { composers: ComposerResolverService, song: SongResolverService } },
            { path: "create", component: SongCreateComponent, resolve: { composers: ComposerResolverService } }
        ])
    ]
})
export class SongRoutingModule { }