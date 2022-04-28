import { Queen } from "./queen";

export class QueensRaising {
    id: number;
    startDate: Date;
    larvaCount: number;
    developmentPlace: DevelopmentPlace;
    cappedCellCount: number;
    queensCount: number;
    motherId: number;
    beefamilyId: number;

    daysLeft: number;
    queen?: Queen;
}

export enum DevelopmentPlace
{
    Beehive, Incubator
}

export const DevelopmentPlace2LabelMapping: Record<DevelopmentPlace, string> = {
    [DevelopmentPlace.Beehive]: "Avilyje",
    [DevelopmentPlace.Incubator]: "Inkubatoriuje"
};