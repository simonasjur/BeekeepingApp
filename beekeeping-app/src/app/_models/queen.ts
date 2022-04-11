import { Colors } from "./beehive";

export class Queen {
    id: number;
    breed: Breed;
    state: QueenState;
    hatchingDate: Date;
    markingColor: Colors;
    isFertilized: boolean;
    broodStartDate: Date;
    farmId: number;
}

export enum QueenState {
    Cell = 1,
    GyvenaAvilyje = 2,
    PriduodamaŠeimai = 4,
    IzoliuotaNarvelyje = 8,
    Parduota = 16,
    Išsispietusi = 32,
    Numirusi = 64
}

export const QueenState2LabelMapping: Record<QueenState, string> = {
    [QueenState.Cell]: "Lopšys",
    [QueenState.GyvenaAvilyje]: "Gyvena avilyje",
    [QueenState.PriduodamaŠeimai]: "Priduodama šeimai",
    [QueenState.IzoliuotaNarvelyje]: "Izoliuota narvelyje",
    [QueenState.Parduota]: "Parduota",
    [QueenState.Išsispietusi]: "Išsispietusi",
    [QueenState.Numirusi]: "Numirusi"
};

export enum Breed {
    Bakfasto, Karnika, Mišrūnė, Nežinoma
}

export const Breed2LabelMapping: Record<Breed, string> = {
    [Breed.Bakfasto]: "Bakfasto",
    [Breed.Karnika]: "Karnika",
    [Breed.Mišrūnė]: "Mišrūnė",
    [Breed.Nežinoma]: "Nežinoma"
};