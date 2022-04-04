export class BeehiveComponent {
    id: number;
    type: BeehiveComponentType;
    position?: number;
    installationDate: Date;
    beehiveId: number;
}

export enum BeehiveComponentType {
    Meduvė = 1,
    Pusmeduvė = 2,
    Išleistuvas = 4,
    SkiriamojiTvorelė = 8,
    Aukstas = 16,
    DugnoSklendė = 32,
    Maitintuvė = 64
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