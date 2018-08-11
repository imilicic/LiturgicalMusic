import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { SongCreateComponent } from "./views/song-create.component";
import { ComposerResolverService } from "./services/composer-resolver.service";
import { SongEditComponent } from "./views/song-edit.component";
import { SongSearchComponent } from "./views/song-search.component";
import { SongViewComponent } from "./views/song-view.component";
import { SongRouteActivatorService } from "./services/song-route-activator.service";
import { SongResolverService } from "./services/song-resolver.service";

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: "view/:songId", component: SongViewComponent, canActivate: [SongRouteActivatorService], resolve: { song: SongResolverService } },
            { path: "search", component: SongSearchComponent },
            { path: "edit/:songId", component: SongEditComponent, canActivate: [SongRouteActivatorService], resolve: { composers: ComposerResolverService, song: SongResolverService } },
            { path: "create", component: SongCreateComponent, resolve: { composers: ComposerResolverService } }
        ])
    ]
})
export class SongRoutingModule { }