import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { HomeComponent } from "./home/home.component";

@NgModule({
    imports: [
        RouterModule.forRoot([
            { path: "home", component: HomeComponent },
            { path: "", redirectTo: "home", pathMatch: "full" },
            { path: "song", loadChildren: "app/song/song.module#SongModule" },
        ])
    ]
})
export class AppRoutingModule { }