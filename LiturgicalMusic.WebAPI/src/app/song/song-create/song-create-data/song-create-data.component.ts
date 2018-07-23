import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";

import { Composer } from "../../shared/models/composer.model";
import { ComposerService } from "../../shared/composer.service";
import { Song } from "../../shared/models/song.model";

@Component({
    selector: "create-data",
    templateUrl: "./song-create-data.component.html"
})
export class SongCreateDataComponent implements OnInit {
    composers: Composer[] = [];
    createdSong: Song;
    templateVoices: boolean[] = [false, false, false, false, true, true, true, true];
    @Output() songData = new EventEmitter();

    arranger: FormControl;
    composer: FormControl;
    otherInformation: FormControl;
    source: FormControl;
    template: FormControl;
    title: FormControl;
    type: FormControl;

    //stanzas: FormControl;
    //instrumentalParts: FormControl;
    themeCategories: FormControl;
    liturgyCategories: FormControl;
    songForm: FormGroup;

    constructor(private composerService: ComposerService) { }

    ngOnInit() {
        this.composerService.getComposers()
            .subscribe(result => {
                this.composers = result;
            });

        let templateValue: string = this.templateVoices.map(b => String(+b)).join("");

        this.arranger = new FormControl();
        this.composer = new FormControl();
        this.otherInformation = new FormControl();
        this.source = new FormControl();
        this.template = new FormControl(templateValue, Validators.required);
        this.title = new FormControl("", Validators.required);
        this.type = new FormControl("", Validators.required);

        this.songForm = new FormGroup({
            arranger: this.arranger,
            composer: this.composer,
            otherInformation: this.otherInformation,
            source: this.source,
            template: this.template,
            title: this.title,
            type: this.type
        });
    }

    createSong(formValues: any) {
        let newSong: Song = {
            Arranger: this.composers.find(c => c.Id == formValues.arranger),
            Composer: this.composers.find(c => c.Id == formValues.composer),
            OtherInformation: formValues.otherInformation,
            Source: formValues.source,
            Template: formValues.template,
            Title: formValues.title,
            Type: formValues.type,

            Code: undefined,
            Id: undefined,
            Stanzas: undefined,
            InstrumentalParts: undefined,
            ThemeCategories: undefined,
            LiturgyCategories: undefined
        }

        this.songData.emit(newSong);
    }

    updateTemplate(event: any, position: any) {
        this.templateVoices[position] = !this.templateVoices[position];
        let templateValue: string = this.templateVoices.map(b => String(+b)).join("");

        this.template.setValue(templateValue);
    }
}