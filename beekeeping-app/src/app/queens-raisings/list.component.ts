import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Breed2LabelMapping, Queen, QueensRaising, QueenState, Worker } from '../_models';
import { FarmService } from '../_services/farm.service';
import { AlertService } from '../_services/alert.service';
import { MatDialog } from '@angular/material/dialog';
import { QueensRaisingService } from '../_services/queens-raising.service';
import { QueenService } from '../_services/queen.service';
import { DeleteDialog } from '../_components/delete-dialog.component';
import { WorkerService } from '../_services/worker.service';

@Component({
    selector: 'queens-raisings-list',
    templateUrl: './list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    queensRaisings: QueensRaising[];
    livingFarmQueens: Queen[];
    worker: Worker;
    loadedDataCount = 0;
    displayedColumns: string[] = ['no', 'daysLeft', 'breed', 'startDate', 'larvaCount', 'queensCount', 'proc', 'action'];

    constructor(private queensRaisingService: QueensRaisingService,
                private queenService: QueenService,
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
                this.livingFarmQueens = queens.filter(q => q.state === QueenState.LvingInBeehive &&
                                                        q.isFertilized === true);
                this.queensRaisingService.getFarmQueensRaisings(this.farmService.farmValue.id)
                .subscribe(raisings => {
                    this.queensRaisings = raisings;
                    this.queensRaisings.forEach(raising => {
                        this.queenService.getById(raising.motherId).subscribe(queen => {
                            raising.queen = queen;
                            this.loadedDataCount++;
                        });
                        raising.finishDate = new Date(raising.startDate);
                        raising.finishDate.setDate(raising.finishDate.getDate() + 12);
                        raising.daysLeft = this.calcDaysLeft(raising.startDate);
                    });
                });
            });
        });
    }

    calcDaysLeft(startDate: Date) {
        const oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
        const finishDate = new Date(startDate);
        console.log(finishDate);
        finishDate.setDate(finishDate.getDate() + 12);
        console.log(finishDate);
        const todayDate = new Date();
        return Math.round((finishDate.valueOf() - todayDate.valueOf()) / oneDay);
    }

    isDataLoading() {
        return !this.queensRaisings || this.loadedDataCount != this.queensRaisings.length;
    }

    isThereAreLivingQueens() {
        return this.livingFarmQueens.length != 0;
    }

    get breed2LabelMapping() {
        return Breed2LabelMapping;
    }

    deleteQueensRaising(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.queensRaisingService.delete(id).subscribe({
                    next: () => {
                        this.queensRaisings = this.queensRaisings.filter(x => x.id !== id);
                        this.loadedDataCount--;
                        this.alertService.success('Motinėlių auginimas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: () => {
                        this.alertService.error("Nepavyko ištrinti motinėlių auginimo");
                    }
                });
            }
        });
    }
}