import { Component, OnInit } from '@angular/core';
import { MatCalendarCellClassFunction, MatCalendarCellCssClasses } from '@angular/material/datepicker';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { Beehive, Breed2LabelMapping, Color2LabelMapping, DevelopmentPlace2LabelMapping, Queen, QueensRaising, QueenState2LabelMapping, Worker } from '../_models';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';
import { FarmService } from '../_services/farm.service';
import { QueenService } from '../_services/queen.service';
import { QueensRaisingService } from '../_services/queens-raising.service';
import { WorkerService } from '../_services/worker.service';

@Component({
    selector: 'queens-raising',
    templateUrl: 'queens-raising.component.html',
    styleUrls: ['queens-raising.component.css']
})
export class QueensRaisingComponent implements OnInit {
    queensRaising: QueensRaising;
    queen: Queen;
    beehive: Beehive;
    today: Date;
    worker: Worker;
    loading = true;

    constructor(private queensRaisingService: QueensRaisingService,
                private queenService: QueenService,
                private beehiveBeefamilyService: BeehiveBeefamilyService,
                private workerService: WorkerService,
                private farmService: FarmService,
                private router: Router,
                public dialog: MatDialog,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.today = new Date();
        const id = this.route.snapshot.params['id'];
        this.workerService.getFarmAndUserWorker(this.farmService.farmValue.id).subscribe(worker => {
            this.worker = worker;
            this.queensRaisingService.getById(id).subscribe(queensRaising => {
                this.queensRaising = queensRaising;
                this.queensRaising.finishDate = new Date(this.queensRaising.startDate);
                this.queensRaising.finishDate.setDate(this.queensRaising.finishDate.getDate() + 12);
                this.queensRaising.finishDate.setHours(this.queensRaising.finishDate.getHours() + 3);
                this.queensRaising.sealingDate = new Date(this.queensRaising.startDate);
                this.queensRaising.sealingDate.setDate( this.queensRaising.sealingDate.getDate() + 4);
                this.queenService.getById(this.queensRaising.motherId).subscribe(queen => {
                    this.queen = queen;
                    this.beehiveBeefamilyService.getBeefamilyBeehive(this.queensRaising.beefamilyId). subscribe(beehiveBeefamily => {
                        if (beehiveBeefamily.length > 0) {
                            this.beehive = beehiveBeefamily[0].beehive;
                        }
                        this.loading = false;
                    });
                });
            });
        });
        
    }

    isRaisingDoneInOneMonth() {
        const startDate = new Date(this.queensRaising.startDate);
        const finishDate = new Date(this.queensRaising.finishDate);
        return startDate.getMonth() === finishDate.getMonth();
    }

    get breed2LabelMapping() {
        return Breed2LabelMapping;
    }

    get developmentPlace2LabelMapping() {
        return DevelopmentPlace2LabelMapping;
    }

    dateClass() {
        return (date: Date): MatCalendarCellCssClasses => {
            date = new Date(date);
            const highlightDate = [this.queensRaising.startDate, this.queensRaising.sealingDate, this.queensRaising.finishDate]
                .map(strDate => new Date(strDate))
                .some(d => {
                    return d.getDate() === date.getDate() && d.getMonth() === date.getMonth() && d.getFullYear() === date.getFullYear()
                });
            return highlightDate ? 'special-date' : '';
        };
    }

    goToList() {
        this.router.navigate(['../'], { relativeTo: this.route });
    }
}