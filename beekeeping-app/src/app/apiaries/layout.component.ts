import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
        private farmService: FarmService,
        private router: Router,
        private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.workerService.getFarmAndUserWorker(this.farmService.farmValue.id).subscribe(worker => {
            this.worker = worker;
            if (this.router.url.includes('apiaries/') && !this.router.url.includes('beefamilies/')) {
                this.apiaryService.getById(this.extractApiaryId()).subscribe(apiary => {
                    this.apiaryService.apiary.subscribe(apiary => {
                        this.apiary = apiary;
                    });
                });
            } else {
                this.apiaryService.apiary.subscribe(apiary => {
                    this.apiary = apiary;
                });
            }
        });
    }

    extractApiaryId() {
        const url = this.router.url.substring(this.router.url.indexOf('apiaries')).substring(9);
        console.log(this.router.url);
        return +url.substring(0, url.indexOf('/'));
    }

    goToApiaries() {
        const url = this.route.snapshot['_routerState'].url;
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
            this.router.navigateByUrl('/apiaries');
        });
    }
}