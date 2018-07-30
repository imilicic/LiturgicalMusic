export class Composer {
    Id: number;
    Name: string;
    Surname: string;

    constructor(composer?: Composer) {
        if (composer) {
            this.Id = composer.Id;
            this.Name = composer.Name;
            this.Surname = composer.Surname;
        }
    }
}