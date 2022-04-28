import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { BeeFamilyService } from '../_services/beefamily.service';
import { User, Apiary, Worker } from '../_models';
import { ApiaryService } from '../_services/apiary.service';
import { FarmService } from '../_services/farm.service';
import { ApiaryBeeFamilyService } from '../_services/apiary-beefamily.service';
import { AlertService } from '../_services/alert.service';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialog } from '../_components/delete-dialog.component';
import { WorkerService } from '../_services/worker.service';

@Component({
    selector: 'apiaries-list',
    templateUrl: './list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    apiaries: Apiary[];
    displayedColumns: string[] = ['name', 'familiesCount', 'action'];
    displayedColumns2: string[] = ['name', 'familiesCount'];
    worker: Worker;

    constructor(private apiaryService: ApiaryService,
                private farmService: FarmService,
                private apiaryFamiliesService: ApiaryBeeFamilyService,
                private workerService: WorkerService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute,
                public dialog: MatDialog) {
    }

    ngOnInit() {
        this.apiaryService.getFarmApiaries(this.farmService.farmValue.id)
            .subscribe(apiaries => {
                this.apiaries = apiaries;
                this.workerService.getFarmAndUserWorker(this.farmService.farmValue.id).subscribe(worker => {
                    this.worker = worker;
                    apiaries.forEach(apiary => 
                        this.apiaryFamiliesService.getOneApiaryBeeFamilies(apiary.id).subscribe(families =>
                            apiary.familiesCount = families.length
                    ));
                });
            });
        this.apiaryService.clearApiary();
    }

    // deleteApiary(id: number): void {
    //     this.apiaryService.delete(id).subscribe({
    //         next: () => {
    //             this.apiaries = this.apiaries.filter(x => x.id !== id);
    //             this.alertService.success('Bitynas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
    //         },
    //         error: error => {
    //             this.alertService.error(error);
    //         }
    //     });
    // }

    deleteApiary(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.apiaryService.delete(id).subscribe({
                    next: () => {
                        this.apiaries = this.apiaries.filter(x => x.id !== id);
                        this.alertService.success('Bitynas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: error => {
                        this.alertService.error(error);
                    }
                });
            }
        });
    }
}