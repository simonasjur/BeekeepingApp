import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { _MatTabBodyBase } from '@angular/material/tabs';
import { ActivatedRoute, Router } from '@angular/router';
import { EqualLessthanValidator } from '../_helpers/equal-lessthan.validator';
import { BeehiveBeefamily, BeehiveTypes, NestReduction } from '../_models';


import { AlertService } from '../_services/alert.service';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';
import { NestReductionService } from '../_services/nest-reduction.service';


@Component({
    selector: 'add-edit-nest-reduction',
    templateUrl: 'add-edit.component.html',
    styleUrls: ['add-edit.component.css']
})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    nestReductionId: number;
    beefamilyId: number;
    today: Date;
    thisYearLastReduction: NestReduction;
    beehiveBeefamily: BeehiveBeefamily;
    isAddMode = true;
    submitted = false;
    loading = false;
    formLoading = true;

    constructor(private nestReductionService: NestReductionService,
                private beehiveBeefamilyService: BeehiveBeefamilyService,
                private alertService: AlertService,
                private formBuilder: FormBuilder,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.extractBeefamilyId();
        this.today = new Date();
        const formOptions: AbstractControlOptions = {validators: EqualLessthanValidator('stayedCombs', 'stayedBroodCombs')};
        this.form = this.formBuilder.group({
            date: ['', Validators.required],
            stayedCombs: ['', [Validators.required, Validators.pattern('^([1-9][0-9]*)$')]],
            stayedBroodCombs: ['',[Validators.required, Validators.pattern('^(0|[1-9][0-9]*)$')]],
            stayedHoney: ['',[Validators.required, Validators.pattern('[0-9]+\\.?[0-9]*')]],
            requiredFoodForWinter: ['',[Validators.required, Validators.pattern('[0-9]+\\.?[0-9]*')]],
            beefamilyId: [this.beefamilyId]
        }, formOptions);

        this.nestReductionId = this.route.snapshot.params['id'];
        
        if (this.nestReductionId !== undefined) {
            this.isAddMode = false;
            this.form.addControl('id', new FormControl('', Validators.required));
            this.nestReductionService.getById(this.nestReductionId).subscribe(reduction => this.form.patchValue(reduction));
        }

        this.beehiveBeefamilyService.getBeefamilyBeehive(this.beefamilyId).subscribe(beehiveBeefamily => {
            if (beehiveBeefamily.length > 0) {
                this.beehiveBeefamily = beehiveBeefamily[0];
            } 
            this.nestReductionService.getBeefamilyNestReductions(this.beefamilyId).subscribe(reductions => {
                if (reductions.length > 0) {
                    if (this.isAddMode) {
                        const lastReductionDate = new Date(reductions[0].date);
                        if (lastReductionDate.getFullYear() === this.today.getFullYear()) {
                            this.thisYearLastReduction = reductions[0];
                        }
                    } else {
                        if (reductions.length > 1) {
                            const lastReductionDate = new Date(reductions[1].date);
                            if (lastReductionDate.getFullYear() === this.today.getFullYear()) {
                                this.thisYearLastReduction = reductions[1];
                            }
                        }
                    }
                }
                //Adds max validator to stayedCombs if this year reduction done in Dadan beehive
                if (this.beehiveBeefamily && this.beehiveBeefamily.beehive.type === BeehiveTypes.Dadano &&
                    (this.isAddMode || (!this.isAddMode && this.nestReductionId == reductions[0].id))) {
                    this.form.controls['stayedCombs'].addValidators(Validators.max(this.beehiveBeefamily.beehive.maxNestCombs));
                }
                this.formLoading = false;
            });
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

        const formReductionDate = new Date(this.form.controls['date'].value);
        if (formReductionDate.getFullYear() == this.today.getFullYear())
        {
            if (this.thisYearLastReduction) {
                const lastReductionDate = new Date(this.thisYearLastReduction.date);
                const lastReductionDateString = formatDate(lastReductionDate,'yyyy-MM-dd','en-US');
                const formReductionDateString = formatDate(formReductionDate,'yyyy-MM-dd','en-US');
                console.log(lastReductionDateString);
                console.log(formReductionDateString);
                if (formReductionDateString <= lastReductionDateString) {
                    this.alertService.error('Šių metų siaurinimas negali būti ankstesnis už paskutinį siaurinimą ar vienodas: ' +
                                            lastReductionDateString);
                    return;
                }
            }
        }
        
        console.log(this.form.value);

        this.loading = true;
       
        if (this.isAddMode) {
            this.createNestReduction();
        } else {
            this.updateNestReduction();
        }
    }

    private createNestReduction() {
        this.nestReductionService.create(this.form.value).subscribe({
            next: () => {
                this.alertService.success('Lizdo siaurinimas sėkmingai sukurtas', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko sukurti lizdo siaurinimo');
                this.loading = false;
            }
        });
    }

    private updateNestReduction() {
        this.nestReductionService.update(this.nestReductionId, this.form.value).subscribe({
            next: () => {
                this.alertService.success('Lizdo siaurinimas sėkmingai atnaujintas', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko atnaujinti lizdo siaurinimo');
                this.loading = false;
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