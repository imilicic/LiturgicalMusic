import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { HomeComponent } from "./home/home.component";

@NgModule({
    imports: [
        RouterModule.forRoot([
            { path: "home", component: HomeComponent },
            { path: "", redirectTo: "home", pathMatch: "full" },
            { path: "songs", loadChildren: 'app/song/song.module#SongModule' },
            { path: "**", redirectTo: "home" }
        ])
    ]
})
export class AppRoutingModule { }