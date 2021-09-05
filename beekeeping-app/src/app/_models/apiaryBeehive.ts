import { Beehive } from "./beehive";

export class ApiaryBeehive {
    id?: number;
    arriveDate: Date;
    x: number;
    y: number;
    departDate?: Date;
    apiaryId: number;
    beehiveId: number;
    beehive?: Beehive;
}