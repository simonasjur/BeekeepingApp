import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { DeleteDialog } from '../_components/delete-dialog.component';

import { Beehive, BeehiveType2LabelMapping, Worker } from '../_models';
import { AlertService } from '../_services/alert.service';
import { ApiaryBeeFamilyService } from '../_services/apiary-beefamily.service';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';
import { BeehiveService } from '../_services/beehive.service';
import { FarmService } from '../_services/farm.service';
import { WorkerService } from '../_services/worker.service';

@Component({
    selector: 'beehive-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    beehives: Beehive[];
    emptyBeehives: Beehive[];
    worker: Worker;
    loading = true;

    data = [];
    displayedColumns = ['no', 'type', 'action'];
    displayedColumns2 = ['no', 'type', 'apiary', 'action'];

    constructor(private beehiveService: BeehiveService,
                private farmService: FarmService,
                private beehiveBeefamilyService: BeehiveBeefamilyService,
                private apiaryBeefamilyService: ApiaryBeeFamilyService,
                private alertService: AlertService,
                private workerService: WorkerService,
                private dialog: MatDialog,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.checkUrl();
        this.workerService.getFarmAndUserWorker(this.farmService.farmValue.id).subscribe(worker => {
            this.worker = worker;
            this.beehiveService.getFarmAllBeehives(this.farmService.farmValue.id)
            .subscribe({
                next: beehives => {
                    this.beehives = beehives.filter(b => b.isEmpty === false);
                    this.emptyBeehives = beehives.filter(b => b.isEmpty === true);
                    this.beehives.forEach(beehive => {
                        this.beehiveBeefamilyService.getBeehiveBeefamily(beehive.id).subscribe(beehiveBeefamily => {
                            this.apiaryBeefamilyService.getBeefamilyApiary(beehiveBeefamily[0].beeFamilyId).subscribe(apiaryBeefamily => {
                                const beehiveData = {
                                    beehive: beehive,
                                    beefamily: apiaryBeefamily[0].beeFamily,
                                    apiary: apiaryBeefamily[0].apiary
                                };
                                this.data.push(beehiveData);
                                if (this.data.length === this.beehives.length) {
                                    this.sortDataByBeehiveNo();
                                }
                            });
                        });
                    });
                }
            });
        });
    }

    checkUrl() {
        const urlEntry = this.router.url.substring(1, 10);
        if (urlEntry !== 'beehives') {
            if (urlEntry === 'apiaries/') {
                this.router.navigate(['../home/'], { relativeTo: this.route });
            } else {
                this.router.navigate(['/'], { relativeTo: this.route });
            }
        }
    }

    sortDataByBeehiveNo() {
        this.data.sort((d1,d2) => d1.beehive.no - d2.beehive.no);
        this.loading = false;
    }

    get beehiveType2LabelMapping() {
        return BeehiveType2LabelMapping;
    }

    deleteBeehive(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.beehiveService.delete(id).subscribe({
                    next: () => {
                        this.emptyBeehives = this.emptyBeehives.filter(x => x.id !== id);
                        this.alertService.success('Avilys sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: error => {
                        this.alertService.error(error);
                    }
                });
            }
        }); 
    }
}