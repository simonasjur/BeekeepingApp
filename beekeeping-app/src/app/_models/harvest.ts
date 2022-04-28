export class Harvest {
    id: number;
    product: HarvestProducts;
    startDate: Date;
    endDate: Date;
    quantity: number;
    farmId: number;
    apiaryId: number;
    beeFamilyId: number;
    isDeleting: boolean = false;
}

export enum HarvestProducts {
    BeeBread,
    Beewax,
    Propolis,
    Honey
}

export const HarvestProduct2LabelMapping: Record<HarvestProducts, string> = {
    [HarvestProducts.BeeBread]: "Duonelė",
    [HarvestProducts.Beewax]: "Vaškas",
    [HarvestProducts.Propolis]: "Pikis",
    [HarvestProducts.Honey]: "Medus"
};
