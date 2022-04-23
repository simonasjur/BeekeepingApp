import { Worker } from "./worker";

export class Farm {
    id: number;
    name: string;
    creationDate: Date;
    isDeleting: boolean = false;
    worker: Worker;
}
