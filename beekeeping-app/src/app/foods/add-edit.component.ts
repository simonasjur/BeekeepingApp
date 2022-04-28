import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AlertService } from '../_services/alert.service';
import { FarmService } from '../_services/farm.service';
import { FoodService } from '../_services/food.service';

@Component({
    selector: 'add-edit-food',
    templateUrl: 'add-edit.component.html',
    styleUrls: ['add-edit.component.css']
})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    id: number;
    isAddMode = true;
    submitted = false;
    loading = false;
    formLoading = true;

    constructor(private foodService: FoodService,
                private farmService: FarmService,
                private alertService: AlertService,
                private formBuilder: FormBuilder,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.form = this.formBuilder.group({
            name: ['', Validators.required],
            farmId: [this.farmService.farmValue.id],
        });

        this.id = this.route.snapshot.params['id'];

        if (this.id !== undefined) {
            this.isAddMode = false;
            this.form.addControl('id', new FormControl('', Validators.required));
            this.foodService.getById(this.id).subscribe(food => {
                this.form.patchValue(food);
                this.formLoading = false;
            });
        } else {
            this.formLoading = false;
        }
    }

    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;
        
        if (this.form.invalid) {
            return;
        }
      
        this.loading = true;
       
        if (this.isAddMode) {
            this.createFood();
        } else {
            this.updateFood();
        }
    }

    private createFood() {
        this.foodService.create(this.form.value).subscribe({
            next: () => {
                this.alertService.success('Bičių maistas sėkmingai sukurtas', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko sukurti bičių maisto');
            }
        });
    }

    private updateFood() {
        this.foodService.update(this.id, this.form.value).subscribe({
            next: () => {
                this.alertService.success('Bičių maistas sėkmingai atnaujintas', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko atnaujinti bičių maisto');
            }
        });
    }

    backToList() {
        if (this.isAddMode) {
            this.router.navigate(['../../'], { relativeTo: this.route });
        } else {
            this.router.navigate(['../../../'], { relativeTo: this.route });
        }
    }
}