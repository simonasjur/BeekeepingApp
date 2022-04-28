import { Beehive } from "./beehive";

export class BeehiveBeefamily {
    id: number;
    arriveDate: Date;
    departDate: Date;
    beehiveId: number;
    beeFamilyId: number;
    beehive?: Beehive;
    isDeleting: boolean = false;
}