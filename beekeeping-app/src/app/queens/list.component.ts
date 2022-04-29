import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { AlertService } from '../_services/alert.service';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialog } from '../_components/delete-dialog.component';
import { Breed2LabelMapping, Color2LabelMapping, Queen, QueenState, QueenState2LabelMapping, Worker } from '../_models';
import { FarmService } from '../_services/farm.service';
import { WorkerService } from '../_services/worker.service';
import { QueenService } from '../_services/queen.service';

@Component({
    selector: 'queens-list',
    templateUrl: './list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    queens: Queen[];
    worker: Worker;
    loading = true;
    displayedColumns: string[] = ['id', 'state', 'breed', 'isFertilized', 'markingColor', 'hatchingDate', 'broodStartDate', 'action'];

    constructor(private queenService: QueenService,
                private farmService: FarmService,
                private workerService: WorkerService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute,
                public dialog: MatDialog) {
    }

    ngOnInit() {
        this.workerService.getFarmAndUserWorker(this.farmService.farmValue.id).subscribe(worker => {
            this.worker = worker;
            this.queenService.getFarmQueens(this.farmService.farmValue.id).subscribe(queens => {
                this.queens = queens;
                this.loading = false;
            });
        });
    }

    get queenState2LabelMapping() {
        return QueenState2LabelMapping;
    }

    get breed2LabelMapping() {
        return Breed2LabelMapping;
    }

    get color2LabelMapping() {
        return Color2LabelMapping;
    }

    isDeleteAllowed(state: QueenState) {
        return state != QueenState.Cell && state != QueenState.LvingInBeehive;
    }

    deleteQueen(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.queenService.delete(id).subscribe({
                    next: () => {
                        this.queens = this.queens.filter(x => x.id !== id);
                        this.alertService.success('Motinėlė sėkmingai ištrinta', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: () => {
                        this.alertService.error("Nepavyko ištrinti motinėlės");
                    }
                });
            }
        });
    }
}