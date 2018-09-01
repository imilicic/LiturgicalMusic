export class Composer {
    Id: number = undefined;
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