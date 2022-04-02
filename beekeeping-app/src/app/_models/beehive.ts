export class Beehive {
    id: number;
    type: BeehiveTypes;
    isEmpty: boolean;
    no: number;
    acquireDay: Date;
    color: Colors;
    maxNestCombs: number;
    nestCombs: number;
    maxHoneyCombsSupers: number;
    farmId: number;
}

export enum BeehiveTypes {
    Dadano, Daugiaaukštis, NukleosoSekcija
}

export const BeehiveType2LabelMapping: Record<BeehiveTypes, string> = {
    [BeehiveTypes.Dadano]: "Dadano",
    [BeehiveTypes.Daugiaaukštis]: "Daugiaaukštis",
    [BeehiveTypes.NukleosoSekcija]: "Nukleuso sekcija"
};

export enum Colors
{
    Mėlyna, Geltona, Balta, Raudona, Žalia
}

export const Color2LabelMapping: Record<Colors, string> = {
    [Colors.Mėlyna]: "Mėlyna",
    [Colors.Geltona]: "Geltona",
    [Colors.Balta]: "Balta",
    [Colors.Raudona]: "Raudona",
    [Colors.Žalia]: "Žalia"
};