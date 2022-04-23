import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { EqualLessthanValidator } from '../_helpers/equal-lessthan.validator';

import { Beehive, BeehiveType2LabelMapping, BeehiveTypes, Color2LabelMapping, Colors } from '../_models';
import { AlertService } from '../_services/alert.service';
import { BeehiveService } from '../_services/beehive.service';
import { FarmService } from '../_services/farm.service';

@Component({
    selector: 'add-edit-beehive',
    templateUrl: 'add-edit.component.html',
    styleUrls: ['add-edit.component.css']
})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    id: number;
    beehive: Beehive;
    isAddMode = true;
    submitted = false;
    loading = false;

    constructor(private beehiveService: BeehiveService,
                private farmService: FarmService,
                private alertService: AlertService,
                private formBuilder: FormBuilder,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        const formOptions: AbstractControlOptions = { validators: EqualLessthanValidator('maxNestCombs', 'nestCombs')};
        this.form = this.formBuilder.group({
            type: ['', Validators.required],
            isEmpty: ['', Validators.required],
            no: ['', [Validators.required, Validators.pattern('^([1-9][0-9]*)$')]],
            maxNestCombs: ['', [Validators.required, Validators.max(16), Validators.pattern('^(0|[1-9][0-9]*)$')]],
            maxHoneyCombsSupers: ['', [Validators.required, Validators.max(4), Validators.pattern('^(0|[1-9][0-9]*)$')]],
            nestCombs: ['', [Validators.required, Validators.pattern('^(0|[1-9][0-9]*)$')]],
            acquireDay: [''],
            color: [''],
            farmId: ['', Validators.required],
        }, formOptions);

        this.id = this.route.snapshot.params['id'];
        
        if (this.id !== undefined) {
            this.isAddMode = false;
            this.form.addControl('id', new FormControl('', Validators.required));
            this.beehiveService.getById(this.id).subscribe(beehive => {
                this.form.patchValue(beehive);
                this.beehive = beehive;
            });
        }
    }

    get beehiveTypes() {
        return BeehiveTypes;
    }

    get beehiveTypesNames() {
        return Object.values(BeehiveTypes).filter(value => typeof value === 'number');
    }

    get beehiveType2LabelMapping() {
        return BeehiveType2LabelMapping;
    }

    get colorsNames() {
        return Object.values(Colors).filter(value => typeof value === 'number');
    }

    get color2LabelMapping() {
        return Color2LabelMapping;
    }

    get f() { return this.form.controls; }

    isBeehiveEmpty() {
        return !this.beehive || this.beehive.isEmpty;
    }

    onSubmit() {
        if (!this.submitted) {
            if (this.isAddMode) {
                this.form.patchValue({farmId: this.farmService.farmValue.id});
                this.form.patchValue({isEmpty: 'true'});
                this.form.patchValue({nestCombs: '0'});    
            }
            if (this.form.controls['type'].value === this.beehiveTypes.Daugiaaukštis) {
                this.form.patchValue({maxNestCombs: '0'});
                this.form.patchValue({maxHoneyCombsSupers: '0'});
                this.form.patchValue({nestCombs: '0'});
            }
        }
   
        this.submitted = true;
        
        if (this.form.invalid) {
            return;
        }
      
        this.loading = true;

        if (this.form.controls['type'].value === this.beehiveTypes.Daugiaaukštis) {
            this.form.patchValue({maxNestCombs: ''});
            this.form.patchValue({maxHoneyCombsSupers: ''});
            this.form.patchValue({nestCombs: ''});
        }
       
        if (this.isAddMode) {
            this.createBeehive();
        } else {
            this.updateBeehive();
        }
    }

    private createBeehive() {
        if (this.form.controls['type'].value === this.beehiveTypes.Daugiaaukštis) {
            const beehive = {
                type: this.form.controls['type'].value,
                no: this.form.controls['no'].value,
                isEmpty: this.form.controls['isEmpty'].value,
                farmId: this.form.controls['farmId'].value
            }
            this.beehiveService.create(beehive).pipe(first())
                .subscribe(() => {
                    this.alertService.success('Avilys sėkmingai sukurtas', { keepAfterRouteChange: true, autoClose: true });
                    this.backToBeehivesList();
                })
                .add(() => this.loading = false);
        } else if (this.form.controls['type'].value === this.beehiveTypes.Dadano) {
            this.beehiveService.create(this.form.value).pipe(first())
                .subscribe(() => {
                    this.alertService.success('Avilys sėkmingai sukurtas', { keepAfterRouteChange: true, autoClose: true });
                    this.backToBeehivesList();
                })
                .add(() => this.loading = false);
        } else {
            this.backToBeehivesList();
        }
    }

    private updateBeehive() {
        this.beehiveService.update(this.id, this.form.value).pipe(first())
            .subscribe(() => {
                this.alertService.success('Avilio informacija sėkmingai atnaujinta', { keepAfterRouteChange: true, autoClose: true });
                this.backToBeehivesList();
            })
            .add(() => this.loading = false);
    }

    backToBeehivesList() {
        if (this.isAddMode) {
            this.router.navigate(['../'], { relativeTo: this.route });
        } else {
            this.router.navigate(['../../'], { relativeTo: this.route });
        }
    }
}