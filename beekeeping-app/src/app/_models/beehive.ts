export class Beehive {
    id: number;
    type: BeehiveTypes;
    no: number;
    isEmpty: boolean;
    acquireDay: Date;
    color: Colors;
    nestCombs: number;
    requiredFoodForWinter: number;
    farmId: number;
}

export enum BeehiveTypes {
    Dadano,
    Daugiaaukstis
}

export const BeehiveType2LabelMapping: Record<BeehiveTypes, string> = {
    [BeehiveTypes.Dadano]: "Dadano avilys",
    [BeehiveTypes.Daugiaaukstis]: "Daugiaaukštis avilys"
};

export enum Colors {
    Melyna, 
    Geltona, 
    Balta, 
    Raudona, 
    Zalia
}