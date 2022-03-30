export class BeeFamily {
    id: number;
    isNucleus: boolean;
    origin: BeeFamilyOrigins;
    state: BeeFamilyStates;
    requiredFoodForWinter: number;
    farmId: number;
}

export enum BeeFamilyStates {
    Gyvena,
    Išsispietus,
    Išmirusi,
    SujungtaSuKitaŠeima
}

export const BeeFamilyState2LabelMapping: Record<BeeFamilyStates, string> = {
    [BeeFamilyStates.Gyvena]: "Gyvena bitės",
    [BeeFamilyStates.Išsispietus]: "Išsispietusios",
    [BeeFamilyStates.Išmirusi]: "Išmirusi šeima",
    [BeeFamilyStates.SujungtaSuKitaŠeima]: "Sujungta su kita šeima"
};

export enum BeeFamilyOrigins {
    Spiečius,
    IšKitosŠeimos,
    Pirkta,
    Padovanota
}

export const BeeFamilyOrigin2LabelMapping: Record<BeeFamilyOrigins, string> = {
    [BeeFamilyOrigins.Spiečius]: "Spiečius",
    [BeeFamilyOrigins.IšKitosŠeimos]: "Iš kitos šeimos",
    [BeeFamilyOrigins.Pirkta]: "Pirkta",
    [BeeFamilyOrigins.Padovanota]: "Padovanota"
};