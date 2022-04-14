import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { BeeFamily, BeefamilyQueen, BeehiveBeefamily, Breed, Breed2LabelMapping, Color2LabelMapping, Colors, Queen, QueenState, QueenState2LabelMapping } from '../_models';
import { AlertService } from '../_services/alert.service';
import { BeefamilyQueenService } from '../_services/beefamily-queen.service';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';
import { FarmService } from '../_services/farm.service';
import { QueenService } from '../_services/queen.service';

@Component({
    selector: 'add-edit-queen-component',
    templateUrl: 'add-edit.component.html',
    styleUrls: ['add-edit.component.css']
})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    id: number;
    beefamilyId: number;
    beehiveBeefamily: BeehiveBeefamily;
    beefamilyQueen: BeefamilyQueen;
    queen: Queen;
    isAddMode = true;
    submitted = false;
    loading = false;
    checked = false;

    constructor(private queenService: QueenService,
                private farmService: FarmService,
                private beehiveBeefamilyService: BeehiveBeefamilyService,
                private beefamilyQueenService: BeefamilyQueenService,
                private alertService: AlertService,
                private formBuilder: FormBuilder,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.form = this.formBuilder.group({
            beehiveNo: [''],
            insertDate: ['', Validators.required],
            breed: ['', Validators.required],
            hatchingDate: [''],
            markingColor: [''],
            isFertilized: [this.checked],
            broodStartDate: [''],
            state: ['', Validators.required],
            farmId: ['', Validators.required]
        });

        this.id = this.route.snapshot.params['id'];
        const urlEntry = this.router.url.substring(1, 9);

        //Cant create queen without beefamily
        if (!this.id && urlEntry !== 'apiaries') {
            this.backToHomeWithError();
        }

        if (urlEntry === 'apiaries') {   
            const url = this.router.url.substring(this.router.url.indexOf('beefamilies')).substring(12);
            this.beefamilyId = +url.substring(0, url.indexOf('/'));
            this.beehiveBeefamilyService.getBeefamilyBeehive(this.beefamilyId)
                .subscribe({
                    next: beehiveBeeFamily => {
                        this.beehiveBeefamily = beehiveBeeFamily[0];
                        if (this.beehiveBeefamily) {
                            this.form.patchValue({beehiveNo: beehiveBeeFamily[0].beehive.no});
                            this.form.controls['beehiveNo'].disable();
                        } else {
                            this.backToHomeWithError();
                        } 
                    },
                    error: () => {
                        this.backToHomeWithError();
                    }
                });
        }

        if (this.id !== undefined) {
            this.isAddMode = false;
            if (this.beefamilyId) {
                this.beefamilyQueenService.getLivingBeefamilyQueen(this.beefamilyId)
                    .subscribe({
                        next: beefamilyQueens => {
                            this.beefamilyQueen = beefamilyQueens[0];
                            if (this.beefamilyQueen){
                                this.id = this.beefamilyQueen.queenId;
                                this.form.patchValue({insertDate: this.beefamilyQueen.insertDate});
                                this.loadQueenData();
                            } else {
                                this.backToHomeWithError();
                            }
                        },
                        error: () => {
                            this.backToHomeWithError();
                        }
                    });
            } else {
                this.loadQueenData();
            }
        }
    }

    private loadQueenData() {
        this.form.addControl('id', new FormControl('', Validators.required));
        this.queenService.getById(this.id).subscribe({
            next: queen => {
                this.form.patchValue(queen);
                this.queen = queen;
            },
            error: () => {
                this.backToHomeWithError();
            } 
        });
    }

    private backToHomeWithError() {
        this.alertService.error('Operacija negalima');
        this.router.navigate(['/']);
    }

    isCellStatePosible() {
        if (this.queen && this.queen.state === QueenState.Cell) {
            return true;
        }
        return this.isAddMode;
    }

    isLvingInBeehiveStatePosible() {
        if (this.queen && 
            (this.queen.state === QueenState.LvingInBeehive || this.queen.state === QueenState.Cell)) {
            return true;
        }
        return this.isAddMode;
    }

    isStateDisabled() {
        return this.queen && this.queen.state === QueenState.LvingInBeehive;
    }

    isIsolatedStatePosible() {
        return this.queen && this.queen.state === QueenState.Isolated;
    }

    isSelledDeadStatesPosible() {
        return this.queen && (this.queen.state === QueenState.Isolated ||
            this.queen.state === QueenState.Dead || this.queen.state === QueenState.Selled ||
            this.queen.state === QueenState.Swarmed);
    }

    isSwarmedStatePosible() {
        return this.queen && (this.queen.state === QueenState.Dead ||
            this.queen.state === QueenState.Selled || this.queen.state === QueenState.Swarmed);
    }

    get queenState() {
        return QueenState;
    }

    get queenStateNames() {
        return Object.values(QueenState).filter(value => typeof value === 'number');
    }

    get queenState2LabelMapping() {
        return QueenState2LabelMapping;
    }

    get breedNames() {
        return Object.values(Breed).filter(value => typeof value === 'number');
    }

    get breed2LabelMapping() {
        return Breed2LabelMapping;
    }

    get colorNames() {
        return Object.values(Colors).filter(value => typeof value === 'number');
    }

    get color2LabelMapping() {
        return Color2LabelMapping;
    }

    get f() { return this.form.controls; }

    onSubmit() {
        if (!this.submitted) {
            if (this.isAddMode) {
                this.form.patchValue({farmId: this.farmService.farmValue.id}); 
            }
            if (this.form.controls['isFertilized'].value === false) {
                this.form.patchValue({broodStartDate: ''});
            }
            if (!this.beehiveBeefamily) {
                this.form.patchValue({insertDate: '2000-01-01'});
            }
        }
   
        this.submitted = true;
        
        if (this.form.invalid) {
            return;
        }
      
        this.loading = true;
       
        if (this.isAddMode) {
            this.createQueen();
        } else {
            this.updateQueen();
        }
    }

    private createQueen() {
        this.queenService.create(this.form.value).subscribe({
            next: queen => {
                if (this.beehiveBeefamily) {
                    const familyQueen = {
                        insertDate: this.form.controls['insertDate'].value,
                        queenId: queen.id,
                        beefamilyId: this.beefamilyId
                    }
                    this.beefamilyQueenService.create(familyQueen).subscribe({
                        next: () => {
                            this.alertService.success('Motinėlė sėkmingai įdėta', { keepAfterRouteChange: true, autoClose: true });
                            this.backToList();
                        },
                        error: error => {
                            this.alertService.error('Serverio klaida');
                            this.loading = false;
                        }
                    });
                } else {
                    this.alertService.success('Motinėlė sėkmingai įdėta', { keepAfterRouteChange: true, autoClose: true });
                    this.backToList();
                }
            },
            error: error => {
                this.alertService.error('Serverio klaida');
                this.loading = false;
            }
        });
    }

    private updateQueen() {
        this.queenService.update(this.id, this.form.value).subscribe({
            next: () => {
                if (this.beefamilyQueen) {
                    const familyQueen = {
                        id: this.beefamilyQueen.id,
                        insertDate: this.form.controls['insertDate'].value
                    }
                    this.beefamilyQueenService.update(this.beefamilyQueen.id, familyQueen).subscribe({
                        next: () => {
                            this.alertService.success('Motinėlės informacija sėkmingai atnaujinta', { keepAfterRouteChange: true, autoClose: true });
                            this.backToList();
                        },
                        error: () => {
                            this.alertService.error('Serverio klaida');
                            this.loading = false;
                        }
                    });
                } else {
                    this.alertService.success('Motinėlės informacija sėkmingai atnaujinta', { keepAfterRouteChange: true, autoClose: true });
                    this.backToList();
                }
            },
            error: error => {
                this.alertService.error('Serverio klaida');
                this.loading = false;
            }
        });
    }

    backToList() {
        if (this.isAddMode) {
            if (this.beefamilyId) {
                this.router.navigate(['../../beefamilyqueens'], { relativeTo: this.route });
            } else {
                this.router.navigate(['../'], { relativeTo: this.route });
            } 
        } else {
            if (this.beefamilyId) {
                this.router.navigate(['../../../beefamilyqueens'], { relativeTo: this.route });
            } else {
                this.router.navigate(['../../'], { relativeTo: this.route });
            } 
        }
    }
}