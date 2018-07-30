export class Stanza {
    Number: number;
    Text: string;

    constructor(stanza?: Stanza) {
        if (stanza) {
            this.Number = stanza.Number;
            this.Text = stanza.Text;
        }
    }
}