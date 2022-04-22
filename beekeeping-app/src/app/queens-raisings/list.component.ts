import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Breed2LabelMapping, QueensRaising } from '../_models';
import { FarmService } from '../_services/farm.service';
import { AlertService } from '../_services/alert.service';
import { MatDialog } from '@angular/material/dialog';
import { QueensRaisingService } from '../_services/queens-raising.service';
import { QueenService } from '../_services/queen.service';

@Component({
    selector: 'queens-raisings-list',
    templateUrl: './list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    queensRaisings: QueensRaising[];
    loadedDataCount = 0;
    displayedColumns: string[] = ['no', 'daysLeft', 'breed', 'startDate', 'larvaCount', 'queensCount', 'proc', 'action'];

    constructor(private queensRaisingService: QueensRaisingService,
                private queenService: QueenService,
                private farmService: FarmService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute,
                public dialog: MatDialog) {
    }

    ngOnInit() {
        this.queensRaisingService.getFarmQueensRaisings(this.farmService.farmValue.id)
            .subscribe(raisings => {
                this.queensRaisings = raisings;
                this.queensRaisings.forEach(raising => {
                    this.queenService.getById(raising.motherId).subscribe(queen => {
                        raising.queen = queen;
                        this.loadedDataCount++;
                    });
                    raising.daysLeft = this.calcDaysLeft(raising.startDate);
                });
            });
    }

    calcDaysLeft(startDate: Date) {
        const oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
        const finishDate = new Date(startDate);
        finishDate.setDate(finishDate.getDate() + 16);
        const todayDate = new Date();
        return Math.round((finishDate.valueOf() - todayDate.valueOf()) / oneDay);
    }

    isDataLoading() {
        return !this.queensRaisings || this.loadedDataCount != this.queensRaisings.length;
    }

    get breed2LabelMapping() {
        return Breed2LabelMapping;
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

    // deleteApiary(id: number): void {
    //     const dialogRef = this.dialog.open(DeleteDialog);
    
    //     dialogRef.afterClosed().subscribe(result => {
    //         if (result) {
    //             this.apiaryService.delete(id).subscribe({
    //                 next: () => {
    //                     this.apiaries = this.apiaries.filter(x => x.id !== id);
    //                     this.alertService.success('Bitynas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
    //                 },
    //                 error: error => {
    //                     this.alertService.error(error);
    //                 }
    //             });
    //         }
    //     });
    // }
}