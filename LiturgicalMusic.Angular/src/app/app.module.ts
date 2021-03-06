import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from "@angular/http";
import { NgModule } from '@angular/core';
import { RouterModule } from "@angular/router";

import { AppComponent } from './app.component';
import { AppRoutingModule } from "./app-routing.module";
import { HomeComponent } from "./home/home.component";
import { NavbarComponent } from "./shared/navbar/navbar.component";
import { SongModule } from "./song/song.module";

@NgModule({
    imports: [
        AppRoutingModule,
        BrowserModule,
        HttpModule,
        RouterModule,
        SongModule,
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        NavbarComponent
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
