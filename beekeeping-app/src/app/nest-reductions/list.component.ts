import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { FarmService } from '../_services/farm.service';
import { AlertService } from '../_services/alert.service';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialog } from '../_components/delete-dialog.component';
import { NestReduction } from '../_models';
import { NestReductionService } from '../_services/nest-reduction.service';

@Component({
    selector: 'nest-reductions-list',
    templateUrl: './list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    nestReductions: NestReduction[];
    thisYearLastReduction: NestReduction;
    beefamilyId: number;
    today: Date;
    loading = true;
    displayedColumns: string[] = ['date', 'stayedCombs', 'stayedBroodCombs', 'stayedHoney', 'requiredFoodForWinter', 'action'];

    constructor(private nestReductionService: NestReductionService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute,
                public dialog: MatDialog) {
    }

    ngOnInit() {
        this.extractBeefamilyId();
        this.today = new Date();
        this.nestReductionService.getBeefamilyNestReductions(this.beefamilyId).subscribe(reductions => {
            this.nestReductions = reductions;
            if (reductions.length > 0) {
                const lastReductionDate = new Date(reductions[0].date);
                if (this.today.getFullYear() == lastReductionDate.getFullYear()) {
                    this.thisYearLastReduction = reductions[0];
                }
            }
            this.loading = false;
        });
    }

    extractBeefamilyId() {
        const url = this.router.url.substring(this.router.url.indexOf('beefamilies')).substring(12);
        this.beefamilyId = +url.substring(0, url.indexOf('/'));
    }

    isActionsAllowed(nestReduction: NestReduction) {
        if (nestReduction.id === this.thisYearLastReduction.id) {
            return true;
        }
        const nestReductionDate = new Date(nestReduction.date);
        return nestReductionDate.getFullYear() !== this.today.getFullYear();
    }

    deleteNestReduction(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.nestReductionService.delete(id).subscribe({
                    next: () => {
                        this.nestReductions = this.nestReductions.filter(x => x.id !== id);
                        this.alertService.success('Lizdo siaurinimas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: () => {
                        this.alertService.error("Nepavyko ištrinti lizdo siaurinimo");
                    }
                });
            }
        });
    }
}