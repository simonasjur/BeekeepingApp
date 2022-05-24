import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { EqualLessthanValidator } from '../_helpers/equal-lessthan.validator';

import { Beehive, BeehiveBeefamily, BeehiveType2LabelMapping, BeehiveTypes, Breed, Breed2LabelMapping, Color2LabelMapping, Colors, DevelopmentPlace, DevelopmentPlace2LabelMapping, Queen, QueenState } from '../_models';
import { AlertService } from '../_services/alert.service';
import { BeeFamilyService } from '../_services/beefamily.service';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';
import { BeehiveService } from '../_services/beehive.service';
import { FarmService } from '../_services/farm.service';
import { QueenService } from '../_services/queen.service';
import { QueensRaisingService } from '../_services/queens-raising.service';

@Component({
    selector: 'add-edit-queens-raising',
    templateUrl: 'add-edit.component.html',
    styleUrls: ['add-edit.component.css']
})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    queens: Queen[];
    beehiveBeefamilies: BeehiveBeefamily[] = [];
    id: number;
    isAddMode = true;
    submitted = false;
    loading = false;
    editFormLoading = true;
    dataLoading = true;

    constructor(private queensRaisingService: QueensRaisingService,
                private queensService: QueenService,
                private beefamilyService: BeeFamilyService,
                private beehiveBeefamilyService: BeehiveBeefamilyService,
                private farmService: FarmService,
                private alertService: AlertService,
                private formBuilder: FormBuilder,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        const formOptions: AbstractControlOptions = { 
            validators: [EqualLessthanValidator('larvaCount', 'cappedCellCount'),
                         EqualLessthanValidator('cappedCellCount', 'queensCount')] 
        };
        this.form = this.formBuilder.group({
            startDate: ['', Validators.required],
            larvaCount: ['',[Validators.required, Validators.pattern('^([1-9][0-9]*)$')]],
            developmentPlace: ['', Validators.required],
            cappedCellCount: ['',[Validators.required, Validators.pattern('^(0|[1-9][0-9]*)$')]],
            queensCount: ['',[Validators.required, Validators.pattern('^(0|[1-9][0-9]*)$')]],
            motherId: ['', Validators.required],
            beefamilyId: ['', Validators.required]
        }, formOptions);

        this.queensService.getFarmQueens(this.farmService.farmValue.id).subscribe(queens => {
            this.queens = queens.filter(q => q.state === QueenState.LvingInBeehive &&
                                             q.isFertilized === true);
            this.beefamilyService.getFarmAllBeeFamilies(this.farmService.farmValue.id).subscribe({
                next: beeFamilies => {
                    beeFamilies.forEach(beefamily => this.beehiveBeefamilyService.getBeefamilyBeehive(beefamily.id)
                        .subscribe(beehiveBeeFamily => {
                            this.beehiveBeefamilies = [...this.beehiveBeefamilies, ...beehiveBeeFamily];
                            if (beeFamilies.length === this.beehiveBeefamilies.length) {
                                this.dataLoading = false;
                            }
                        }));
                }
            });
        });
        

        this.id = this.route.snapshot.params['id'];
        
        if (this.id !== undefined) {
            this.isAddMode = false;
            this.form.addControl('id', new FormControl('', Validators.required));
            this.queensRaisingService.getById(this.id).subscribe(raising => {
                this.form.patchValue(raising);
                this.editFormLoading = false;
            });
        } else {
            this.editFormLoading = false;
        }
    }

    isFormLoading() {
        return this.editFormLoading || this.dataLoading;
    }

    get developmentPlaceNames() {
        return Object.values(DevelopmentPlace).filter(value => typeof value === 'number');
    }

    get developmentPlace2LabelMapping() {
        return DevelopmentPlace2LabelMapping;
    }

    get breed2LabelMapping() {
        return Breed2LabelMapping;
    }

    get f() { return this.form.controls; }

    onSubmit() {
        if (!this.submitted) {
            if (this.isAddMode) {
                this.form.patchValue({cappedCellCount: '0'});
                this.form.patchValue({queensCount: '0'});
            }
        }
   
        this.submitted = true;
        
        if (this.form.invalid) {
            return;
        }
      
        this.loading = true;
       
        if (this.isAddMode) {
            this.createQueensRaising();
        } else {
            this.updateQueensRaising();
        }
    }

    private createQueensRaising() {
        this.queensRaisingService.create(this.form.value).subscribe({
            next: () => {
                this.alertService.success('Motinėlių auginimas sėkmingai sukurtas', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko sukurti motinėlių auginimo');
            }
        });
    }

    private updateQueensRaising() {
        this.queensRaisingService.update(this.id, this.form.value).subscribe({
            next: () => {
                this.alertService.success('Motinėlių auginimas sėkmingai atnaujintas', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko atnaujinti motinėlių auginimo');
            }
        });
    }

    backToList() {
        if (this.isAddMode) {
            this.router.navigate(['../'], { relativeTo: this.route });
        } else {
            this.router.navigate([`../../${this.id}`], { relativeTo: this.route });
        }
    }
}