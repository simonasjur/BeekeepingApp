import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { FarmService } from '../_services/farm.service';
import { AlertService } from '../_services/alert.service';
import { MatDialog } from '@angular/material/dialog';
import { FrameType2LabelMapping, NestExpansion, Worker } from '../_models';
import { NestExpansionService } from '../_services/nest-expansion.service';
import { DeleteDialog } from '../_components/delete-dialog.component';
import { WorkerService } from '../_services/worker.service';

@Component({
    selector: 'nest-expansions-list',
    templateUrl: './list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    nestExpansions: NestExpansion[];
    beefamilyId: number;
    worker: Worker;
    loading = true;
    displayedColumns: string[] = ['date', 'frameType', 'combSheets', 'combs', 'action'];

    constructor(private nestExpansionService: NestExpansionService,
                private workerService: WorkerService,
                private farmService: FarmService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute,
                public dialog: MatDialog) {
    }

    ngOnInit() {
        this.extractBeefamilyId();
        this.workerService.getFarmAndUserWorker(this.farmService.farmValue.id).subscribe(worker => {
            this.worker = worker;
            this.nestExpansionService.getBeefamilyNestExpansions(this.beefamilyId).subscribe(expansions => {
                this.nestExpansions = expansions.sort((a, b) => a.date > b.date ? -1 : a.date < b.date ? 1 : 0);
                this.loading = false;
            });
        });
    }

    extractBeefamilyId() {
        const url = this.router.url.substring(this.router.url.indexOf('beefamilies')).substring(12);
        this.beefamilyId = +url.substring(0, url.indexOf('/'));
    }

    get frameType2LabelMapping() {
        return FrameType2LabelMapping;
    }

    deleteNestExpansion(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.nestExpansionService.delete(id).subscribe({
                    next: () => {
                        this.nestExpansions = this.nestExpansions.filter(x => x.id !== id);
                        this.alertService.success('Avilio plėtimas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: () => {
                        this.alertService.error("Nepavyko ištrinti avilio plėtimo");
                    }
                });
            }
        });
    }
}