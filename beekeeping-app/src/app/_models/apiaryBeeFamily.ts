import { Apiary } from "./apiary";
import { BeeFamily } from "./beeFamily";

export class ApiaryBeeFamily {
    id?: number;
    arriveDate: Date;
    departDate?: Date;
    apiaryId: number;
    beeFamilyId: number;
    beeFamily?: BeeFamily;
    apiary?: Apiary;
}