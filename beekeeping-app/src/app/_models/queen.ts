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
    LvingInBeehive = 2,
   // PriduodamaŠeimai = 4, //kol kas nenaudojama
    Isolated = 8,
    Selled = 16,
    Swarmed = 32,
    Dead = 64
}

export const QueenState2LabelMapping: Record<QueenState, string> = {
    [QueenState.Cell]: "Lopšys",
    [QueenState.LvingInBeehive]: "Gyvena avilyje",
//    [QueenState.PriduodamaŠeimai]: "Priduodama šeimai",
    [QueenState.Isolated]: "Izoliuota narvelyje",
    [QueenState.Selled]: "Parduota",
    [QueenState.Swarmed]: "Išsispietusi",
    [QueenState.Dead]: "Numirusi"
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