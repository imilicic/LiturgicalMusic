export class InstrumentalPart {
    Id: number;
    Position: string;
    Type: string;
    Code: string;
    Template: boolean[];

    constructor(part?: InstrumentalPart) {
        if (part) {
            this.Id = part.Id;
            this.Position = part.Position;
            this.Type = part.Type;
            this.Code = part.Code;
            this.Template = Array.from(part.Template);
        }
    }
}