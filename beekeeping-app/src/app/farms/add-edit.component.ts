import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { FarmService } from '../_services/farm.service';
import { AlertService } from '../_services/alert.service';
import { Farm } from '../_models';

@Component({ templateUrl: 'add-edit.component.html',
styleUrls: ['add-edit.component.css'] })
export class AddEditComponent implements OnInit {
    form: FormGroup;
    id: string;
    isAddMode!: boolean;
    loading = false;
    submitted = false;
    farm: Farm;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private farmService: FarmService,
        private alertService: AlertService
    ) {}

    ngOnInit() {
        this.id = this.route.snapshot.params['id'];
        
        this.isAddMode = !this.id;
        if (!this.isAddMode) {
            this.farmService.getById(this.id).subscribe(farm => this.farm = farm);
        }

        this.form = this.formBuilder.group({
            name: ['', Validators.required],
            creationDate: ['', Validators.required]
        });

        if (!this.isAddMode) {
            this.farmService.getById(this.id)
                .pipe(first())
                .subscribe(x => this.form.patchValue(x));
        }
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        if (this.isAddMode) {
            this.createFarm();
        } else {
            this.updateFarm();
        }
    }

    private createFarm() {
        this.farmService.create(this.form.value)
            .pipe(first())
            .subscribe(() => {
                this.alertService.success('Ūkis pridėtas', { keepAfterRouteChange: true, autoClose: true });
                this.router.navigate(['../'], { relativeTo: this.route });
            })
            .add(() => this.loading = false);
    }

    private updateFarm() {
        this.farmService.update(this.id, this.form.value)
            .pipe(first())
            .subscribe(() => {
                this.alertService.success('Ūkis atnaujintas', { keepAfterRouteChange: true, autoClose: true });
                this.router.navigate(['../../'], { relativeTo: this.route });
            })
            .add(() => this.loading = false);
    }
}
