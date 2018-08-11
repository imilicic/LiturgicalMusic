import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { ComposerResolverService } from "./services/composer-resolver.service";
import { HymnEditComponent } from "./views/hymn-edit/hymn-edit.component";
import { SongEditComponent } from "./views/song-edit.component";
import { SongSearchComponent } from "./views/song-search.component";
import { SongViewComponent } from "./views/song-view.component";
import { SongRouteActivatorService } from "./services/song-route-activator.service";
import { SongResolverService } from "./services/song-resolver.service";

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: "view/:songId",
                component: SongViewComponent,
                canActivate: [SongRouteActivatorService],
                resolve: { song: SongResolverService }
            },
            {
                path: "search",
                component: SongSearchComponent
            },
            {
                path: "edit",
                resolve: { composers: ComposerResolverService },
                children: [
                    {
                        path: "",
                        component: SongEditComponent
                    },
                    {
                        path: "hymn",
                        component: HymnEditComponent
                    }
                ]
            },
            {
                path: "edit/:songId",
                canActivate: [SongRouteActivatorService],
                resolve: { composers: ComposerResolverService, song: SongResolverService },
                children: [
                    {
                        path: "",
                        component: SongEditComponent
                    },
                    {
                        path: "hymn",
                        component: HymnEditComponent
                    }
                ]
            },
        ])
    ]
})
export class SongRoutingModule { }