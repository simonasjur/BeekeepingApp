import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DatesValidator } from '../_helpers/dates.validator';

import { BeefamilyQueen, Beehive, BeehiveType2LabelMapping, BeehiveTypes, Breed2LabelMapping, Color2LabelMapping, Colors, Queen, QueenState, QueenState2LabelMapping } from '../_models';
import { AlertService } from '../_services/alert.service';
import { BeefamilyQueenService } from '../_services/beefamily-queen.service';
import { FarmService } from '../_services/farm.service';
import { QueenService } from '../_services/queen.service';

@Component({
    selector: 'add-edit-beefamily-queen',
    templateUrl: 'add-edit.component.html',
    styleUrls: ['add-edit.component.css']
})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    id: number;
    beefamilyQueen: BeefamilyQueen;
    queen: Queen;
    isolatedQueens: Queen[];
    isAddMode = true;
    submitted = false;
    loading = false;
    addFormLoading = true;
    editFormLoading = true;

    constructor(private beefamilyQueenService: BeefamilyQueenService,
                private queenService: QueenService,
                private farmService: FarmService,
                private alertService: AlertService,
                private formBuilder: FormBuilder,
                private router: Router,
                private route: ActivatedRoute) {
    }
 
    ngOnInit() {
        const formOptions: AbstractControlOptions = { validators: DatesValidator('insertDate', 'takeOutDate') };
        this.form = this.formBuilder.group({
            insertDate: ['', Validators.required],
            takeOutDate: ['', Validators.required],
            state: ['', Validators.required],
            queen: ['', Validators.required]
        }, formOptions);

        this.id = this.route.snapshot.params['id'];
        if (this.id) {
            this.isAddMode = false;
            this.form.addControl('id', new FormControl('', Validators.required));
            this.beefamilyQueenService.getById(this.id).subscribe({
                next: beefamilyQueen => {
                    this.beefamilyQueen = beefamilyQueen;
                    this.form.patchValue(beefamilyQueen);
                    this.editFormLoading = false;
                },
                error: () => {
                    this.alertService.error('Operacija negalima');
                    this.router.navigate(['/'], { relativeTo: this.route });
                }
            });
            this.form.controls['insertDate'].disable();
            this.addFormLoading = false;
        } else {
            this.queenService.getFarmQueens(this.farmService.farmValue.id).subscribe({
                next: queens => {
                    this.isolatedQueens = queens.filter(q => q.state === QueenState.Isolated);
                    this.addFormLoading = false;
                },
                error: () => {
                    this.alertService.error('Operacija nesėkminga');
                    this.goBack();
                }
            });
            this.editFormLoading = false;
        }
    }

    get queenState2LabelMapping() {
        return QueenState2LabelMapping;
    }

    get breed2LabelMapping() {
        return Breed2LabelMapping;
    }

    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;
        if (this.isAddMode) {
            this.form.patchValue({'state': QueenState.LvingInBeehive});
            //Inserting value just for validations, creating beefamilyQueen takeOutDate doesn't matter
            this.form.patchValue({'takeOutDate': this.form.controls['insertDate'].value});
        }

        if (this.form.invalid) {
            return;
        }
        this.loading = true;

        if (!this.isAddMode) {
            this.beefamilyQueen.queen.state = this.form.controls['state'].value;
            this.form.controls['insertDate'].enable();
            this.update();
        } else {
            this.form.controls['queen'].value.state = QueenState.LvingInBeehive;
            this.create();
        } 
    }

    private create() {
        const newFamilyQueen = {
            insertDate: this.form.controls['insertDate'].value,
            queenId: this.form.controls['queen'].value.id,
            beefamilyId: this.extractBeefamilyId()
        }
        this.beefamilyQueenService.create(newFamilyQueen).subscribe({
            next: () => {
                this.queenService.update(this.form.controls['queen'].value.id, this.form.controls['queen'].value).subscribe({
                    next: () => {
                        this.alertService.success('Motinėlė sėkmingai įdėta į šeimą', { keepAfterRouteChange: true, autoClose: true });
                        this.goBack();
                    },
                    error: () => {
                        this.alertService.error('Operacija nesėkminga');
                        this.goBack();
                    }
                });
            },
            error: () => {
                this.alertService.error('Nepavyko atlikti operacijos');
                this.goBack();
            }
        });
    }

    private update() {
        this.beefamilyQueenService.update(this.id, this.form.value).subscribe({
            next: () => {
                this.queenService.update(this.beefamilyQueen.queenId, this.beefamilyQueen.queen).subscribe({
                    next: () => {
                        this.alertService.success('Motinėlė sėkmingai išimta iš šeimos', { keepAfterRouteChange: true, autoClose: true });
                        this.goBack();
                    },
                    error: () => {
                        this.alertService.error('Operacija nesėkminga');
                        this.goBack();
                    }
                });
            },
            error: () => {
                this.alertService.error('Nepavyko atlikti operacijos');
                this.goBack();
            }
        });
    }

    goBack() {
        if (this.isAddMode) {
            this.router.navigate(['../'], { relativeTo: this.route });
        } else {
            this.router.navigate(['../../'], { relativeTo: this.route });
        }
    }

    extractBeefamilyId() {
        const url = this.router.url.substring(this.router.url.indexOf('beefamilies')).substring(12);
        return +url.substring(0, url.indexOf('/'));
    }
}