import { Component, Input, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";

@Component({
    selector: "key-time",
    templateUrl: "./key-time.component.html"
})
export class KeyTimeComponent implements OnInit {
    @Input() keyOnly: boolean = false;
    @Input() existingKey: string;
    @Input() existingTimeDenominator: number;
    @Input() existingTimeNumerator: number;
    keyNamesMajor: any[] = [];
    keyNamesMinor: any[] = [];
    timeNumerators: number[] = [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
    timeDenominators: number[] = [2, 4, 8];

    key: FormControl;
    timeDenominator: FormControl;
    timeNumerator: FormControl;

    keyTimeGroup: FormGroup;

    ngOnInit() {
        this.keyNamesMajor.push({Code: "ces", Name: "Ces"});
        this.keyNamesMajor.push({Code: "c", Name: "C"});
        this.keyNamesMajor.push({Code: "cis", Name: "Cis"});
        this.keyNamesMajor.push({Code: "des", Name: "Des"});
        this.keyNamesMajor.push({Code: "d", Name: "D"});
        this.keyNamesMajor.push({Code: "es", Name: "Es"});
        this.keyNamesMajor.push({Code: "e", Name: "E"});
        this.keyNamesMajor.push({Code: "f", Name: "F"});
        this.keyNamesMajor.push({Code: "fis", Name: "Fis"});
        this.keyNamesMajor.push({Code: "ges", Name: "Ges"});
        this.keyNamesMajor.push({Code: "g", Name: "G"});
        this.keyNamesMajor.push({Code: "aes", Name: "As"});
        this.keyNamesMajor.push({Code: "a", Name: "A"});
        this.keyNamesMajor.push({Code: "bes", Name: "B"});
        this.keyNamesMajor.push({ Code: "b", Name: "H" });

        this.keyNamesMinor.push({Code: "c", Name: "c"});
        this.keyNamesMinor.push({Code: "cis", Name: "cis"});
        this.keyNamesMinor.push({Code: "d", Name: "d"});
        this.keyNamesMinor.push({Code: "dis", Name: "cis"});
        this.keyNamesMinor.push({Code: "es", Name: "es"});
        this.keyNamesMinor.push({Code: "e", Name: "e"});
        this.keyNamesMinor.push({Code: "f", Name: "f"});
        this.keyNamesMinor.push({Code: "fis", Name: "fis"});
        this.keyNamesMinor.push({Code: "g", Name: "g"});
        this.keyNamesMinor.push({Code: "gis", Name: "gis"});
        this.keyNamesMinor.push({Code: "a", Name: "a"});
        this.keyNamesMinor.push({Code: "bes", Name: "b"});
        this.keyNamesMinor.push({Code: "b", Name: "h"});

        this.key = new FormControl(this.existingKey, Validators.required);
        this.timeDenominator = new FormControl(this.existingTimeDenominator);
        this.timeNumerator = new FormControl(this.existingTimeNumerator);

        this.keyTimeGroup = new FormGroup({
            key: this.key,
            timeDenominator: this.timeDenominator,
            timeNumerator: this.timeNumerator
        });
    }

    getKeyTime() {
        let code: any = {
            Key: this.keyTimeGroup.controls["key"].value,
        };

        if (!this.keyOnly) {
            code["Time"] = this.keyTimeGroup.controls["timeNumerator"].value + "/" + this.keyTimeGroup.controls["timeDenominator"].value;
        }

        return code;
    }

    previewEnabled() {
        if (this.keyOnly) {
            return this.keyTimeGroup.controls["key"].invalid;
        } else {
            return this.keyTimeGroup.controls["key"].invalid || this.keyTimeGroup.controls["timeNumerator"].invalid || this.keyTimeGroup.controls["timeDenominator"].invalid;
        }
    }
}