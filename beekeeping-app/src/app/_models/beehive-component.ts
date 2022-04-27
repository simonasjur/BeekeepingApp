export class BeehiveComponent {
    id: number;
    type: BeehiveComponentType;
    position?: number;
    installationDate: Date;
    beehiveId: number;
}

export enum BeehiveComponentType {
    Meduvė = 1, //HoneySuper
    Pusmeduvė = 2, //HoneyMiniSuper
    Išleistuvas = 4, //bee decreaser
    SkiriamojiTvorelė = 8, //queen excluder
    Aukstas = 16, //super
    DugnoSklendė = 32, //bottom Gate
    Maitintuvė = 64 //feeder
}

export const BeehiveComponentType2LabelMapping: Record<BeehiveComponentType, string> = {
    [BeehiveComponentType.Meduvė]: "Meduvė",
    [BeehiveComponentType.Pusmeduvė]: "Pusmeduvė",
    [BeehiveComponentType.Išleistuvas]: "Išleistuvas",
    [BeehiveComponentType.SkiriamojiTvorelė]: "Skiriamoji tvorelė",
    [BeehiveComponentType.Aukstas]: "Aukštas",
    [BeehiveComponentType.DugnoSklendė]: "Dugno sklendė",
    [BeehiveComponentType.Maitintuvė]: "Maitintuvė"
};