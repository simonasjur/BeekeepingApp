import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { AlertService } from '../_services/alert.service';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialog } from '../_components/delete-dialog.component';
import { Feeding, Food } from '../_models';
import { FeedingService } from '../_services/feeding.service';
import { FoodService } from '../_services/food.service';
import { FarmService } from '../_services/farm.service';

@Component({
    selector: 'feedings-list',
    templateUrl: './list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    feedings: Feeding[];
    foods: Food[];
    beefamilyId: number;
    loading = true;
    displayedColumns: string[] = ['date', 'foodName', 'quantity', 'action'];
    foodColumns: string[] = ['name', 'action'];

    constructor(private feedingService: FeedingService,
                private foodService: FoodService,
                private farmService: FarmService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute,
                public dialog: MatDialog) {
    }

    ngOnInit() {
        this.extractBeefamilyId();
        this.feedingService.getBeefamilyFeedings(this.beefamilyId).subscribe(feedings => {
            this.feedings = feedings.sort((a, b) => a.date > b.date ? -1 : a.date < b.date ? 1 : 0);
            this.foodService.getFarmFoods(this.farmService.farmValue.id).subscribe(foods => {
                this.foods = foods;
                this.loading = false;
            });
        });
    }

    extractBeefamilyId() {
        const url = this.router.url.substring(this.router.url.indexOf('beefamilies')).substring(12);
        this.beefamilyId = +url.substring(0, url.indexOf('/'));
    }

    isThereAnyFood() {
        return this.foods && this.foods.length > 0;
    }

    deleteFeeding(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.feedingService.delete(id).subscribe({
                    next: () => {
                        this.feedings = this.feedings.filter(x => x.id !== id);
                        this.alertService.success('Maitinimas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: () => {
                        this.alertService.error("Nepavyko ištrinti maitinimo");
                    }
                });
            }
        });
    }

    deleteFood(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.foodService.delete(id).subscribe({
                    next: () => {
                        this.foods = this.foods.filter(x => x.id !== id);
                        this.feedings = this.feedings.filter(x => x.foodId !== id);
                        this.alertService.success('Bičių maistas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: () => {
                        this.alertService.error("Nepavyko ištrinti bičių maisto");
                    }
                });
            }
        });
    }
}