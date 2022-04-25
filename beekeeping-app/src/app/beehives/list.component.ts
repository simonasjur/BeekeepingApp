import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { DeleteDialog } from '../_components/delete-dialog.component';

import { Beehive, BeehiveType2LabelMapping, Worker } from '../_models';
import { AlertService } from '../_services/alert.service';
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

    displayedColumns = ['no', 'type', 'action'];

    constructor(private beehiveService: BeehiveService,
                private farmService: FarmService,
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
                    this.loading = false;
                }});
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