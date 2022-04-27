import { Food } from "./food";

export class Feeding {
    id: number;
    date: Date;
    quantity: number;
    foodId: number;
    beeFamilyId: number;
    food?: Food;
}