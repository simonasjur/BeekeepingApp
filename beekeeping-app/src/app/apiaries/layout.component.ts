import { Component } from '@angular/core';
import { Apiary, Farm, Worker } from '../_models';
import { ApiaryService } from '../_services/apiary.service';
import { FarmService } from '../_services/farm.service';
import { WorkerService } from '../_services/worker.service';

@Component({ 
    templateUrl: 'layout.component.html'
})
export class LayoutComponent { 
    apiary: Apiary;
    worker: Worker;

    constructor(private apiaryService: ApiaryService,
        private workerService: WorkerService,
        private farmService: FarmService) {
    }

    ngOnInit() {
        this.apiaryService.apiary.subscribe(apiary => {
            this.apiary = apiary;
            this.workerService.getFarmAndUserWorker(this.farmService.farmValue.id).subscribe(worker => {
                this.worker = worker;
            });
        })
    }
}