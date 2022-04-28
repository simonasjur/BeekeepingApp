import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { DeleteDialog } from '../_components/delete-dialog.component';

import { BeefamilyQueen, Breed2LabelMapping, Color2LabelMapping, Queen, QueenState, QueenState2LabelMapping, Worker } from '../_models';
import { AlertService } from '../_services/alert.service';
import { BeefamilyQueenService } from '../_services/beefamily-queen.service';
import { FarmService } from '../_services/farm.service';
import { QueenService } from '../_services/queen.service';
import { WorkerService } from '../_services/worker.service';

@Component({
    selector: 'beefamily-queens-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    beefamilyQueens: BeefamilyQueen[];
    familyQueen: BeefamilyQueen;
    beefamilyId: number;
    worker: Worker;
    isolatedFarmQueens: Queen[];
    loading = true;
    apiaryId: number;

    displayedColumns = ['breed', 'hatchingDate', 'markingColor', 'insertDate', 'takeOutDate', 'state', 'action'];

    constructor(private beefamilyQueenService: BeefamilyQueenService,
                private queenService: QueenService,
                private farmService: FarmService,
                private workerService: WorkerService,
                private alertService: AlertService,
                private router: Router,
                public dialog: MatDialog) {
    }

    ngOnInit() {
        this.extractIds();
        this.workerService.getFarmAndUserWorker(this.farmService.farmValue.id).subscribe(worker => {
            this.worker = worker;
            this.beefamilyQueenService.getBeefamilyQueens(this.beefamilyId)
            .subscribe({
                next: beefamilyQueens => {
                    this.beefamilyQueens = beefamilyQueens.sort((a, b) => a.insertDate > b.insertDate ? -1 : a.insertDate < b.insertDate ? 1 : 0 ||
                                                                          a.takeOutDate > b.takeOutDate ? -1 : a.takeOutDate < b.takeOutDate ? 1 : 0);
                    this.familyQueen = beefamilyQueens.find(bq => !bq.takeOutDate);
                    this.queenService.getFarmQueens(this.farmService.farmValue.id).subscribe(queens => {
                        this.isolatedFarmQueens = queens.filter(q => q.state === QueenState.Isolated);
                        this.loading = false;
                    });
                },
                error: () => {
                    this.backToHomeWithError();
                }
            });
        });
    }

    extractIds() {
        const urlEntry = this.router.url.substring(10);
        this.apiaryId = +urlEntry.substring(0, urlEntry.indexOf('/'));
        const url = this.router.url.substring(this.router.url.indexOf('beefamilies')).substring(12);
        this.beefamilyId = +url.substring(0, url.indexOf('/'));
    }

    private backToHomeWithError() {
        this.alertService.error('Operacija negalima');
        this.router.navigate(['/']);
    }

    havingIsolatedQueens() {
        return !(!this.isolatedFarmQueens || this.isolatedFarmQueens.length === 0);
    }

    get breed2LabelMapping() {
        return Breed2LabelMapping;
    }

    get queenState2LabelMapping() {
        return QueenState2LabelMapping;
    }

    get color2LabelMapping() {
        return Color2LabelMapping;
    }

    deleteBeefamilyQueen(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.beefamilyQueenService.delete(id).subscribe({
                    next: () => {
                        this.beefamilyQueens = this.beefamilyQueens.filter(x => x.id !== id);
                        this.alertService.success('Šeimos motinėlės įrašas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: error => {
                        this.alertService.error('Nepavyko ištrinti. Serverio klaida');
                    }
                });
            }
        });
    }
}