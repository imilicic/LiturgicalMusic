import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { SongCreateComponent } from "./song-create/song-create.component";
import { SongSearchComponent } from "./song-search/song-search.component";
import { SongViewComponent } from "./song-view/song-view.component";
import { SongViewRouteActivatorService } from "./song-view/song-view-route-activator.service";
import { SongViewResolverService } from "./song-view/song-view-resolver.service";

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: "search/:songId", component: SongViewComponent, canActivate: [SongViewRouteActivatorService], resolve: { song: SongViewResolverService } },
            { path: "search", component: SongSearchComponent },
            { path: "create", component: SongCreateComponent }
        ])
    ]
})
export class SongRoutingModule { }