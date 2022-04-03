export class BeehiveComponent {
    id: number;
    type: BeehiveComponentType;
    position?: number;
    insertDate: Date;
    beehiveId: number;
}

export enum BeehiveComponentType {
    Meduvė = 1,
    Pusmeduvė = 2,
    Išleistuvas = 4,
    SkiriamojiTvorelė = 8,
    Aukštas = 16,
    DugnoSklendė = 32,
    Maitintuvė = 64
}

export const BeehiveComponentType2LabelMapping: Record<BeehiveComponentType, string> = {
    [BeehiveComponentType.Meduvė]: "Meduvė",
    [BeehiveComponentType.Pusmeduvė]: "Pusmeduvė",
    [BeehiveComponentType.Išleistuvas]: "Išleistuvas",
    [BeehiveComponentType.SkiriamojiTvorelė]: "Skiriamoji tvorelė",
    [BeehiveComponentType.Aukštas]: "Aukštas",
    [BeehiveComponentType.DugnoSklendė]: "Dugno sklendė",
    [BeehiveComponentType.Maitintuvė]: "Maitintuvė"
};