export class Stanza {
    Id: number;
    Number: number;
    Text: string;

    constructor(stanza?: Stanza) {
        if (stanza) {
            this.Id = stanza.Id;
            this.Number = stanza.Number;
            this.Text = stanza.Text;
        }
    }
}