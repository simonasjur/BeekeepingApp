import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Food } from '../_models';

import { AlertService } from '../_services/alert.service';
import { FarmService } from '../_services/farm.service';
import { FeedingService } from '../_services/feeding.service';
import { FoodService } from '../_services/food.service';

@Component({
    selector: 'add-edit-feeding',
    templateUrl: 'add-edit.component.html',
    styleUrls: ['add-edit.component.css']
})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    feedingId: number;
    beefamilyId: number;
    foods: Food[];
    today: Date;
    isAddMode = true;
    submitted = false;
    loading = false;
    formLoading = true;

    constructor(private feedingService: FeedingService,
                private foodService: FoodService,
                private farmService: FarmService,
                private alertService: AlertService,
                private formBuilder: FormBuilder,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.extractBeefamilyId();
        this.today = new Date();
        this.form = this.formBuilder.group({
            date: ['', Validators.required],
            quantity: ['', [Validators.required, Validators.pattern('[0-9]+\\.?[0-9]*')]],
            beefamilyId: [this.beefamilyId],
            foodId: ['', Validators.required],
        });

        this.foodService.getFarmFoods(this.farmService.farmValue.id).subscribe(foods => {
            this.foods = foods;
            this.feedingId = this.route.snapshot.params['id'];
            if (this.feedingId !== undefined) {
                this.isAddMode = false;
                this.form.addControl('id', new FormControl('', Validators.required));
                this.feedingService.getById(this.feedingId).subscribe(feeding => {
                    this.form.patchValue(feeding);
                    this.formLoading = false;
                });
            } else {
                this.formLoading = false;
            }
        });
    }

    extractBeefamilyId() {
        const url = this.router.url.substring(this.router.url.indexOf('beefamilies')).substring(12);
        this.beefamilyId = +url.substring(0, url.indexOf('/'));
    }

    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;
        
        if (this.form.invalid) {
            return;
        }
      
        this.loading = true;
       
        if (this.isAddMode) {
            this.createFeeding();
        } else {
            this.updateFeeding();
        }
    }

    private createFeeding() {
        this.feedingService.create(this.form.value).subscribe({
            next: () => {
                this.alertService.success('Maitinimas sėkmingai sukurtas', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko sukurti maitinimo');
            }
        });
    }

    private updateFeeding() {
        this.feedingService.update(this.feedingId, this.form.value).subscribe({
            next: () => {
                this.alertService.success('Maitinimas sėkmingai atnaujintas', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko atnaujinti maitinimo');
            }
        });
    }

    backToList() {
        if (this.isAddMode) {
            this.router.navigate(['../'], { relativeTo: this.route });
        } else {
            this.router.navigate(['../../'], { relativeTo: this.route });
        }
    }
}