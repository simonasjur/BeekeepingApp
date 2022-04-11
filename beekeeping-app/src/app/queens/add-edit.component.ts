import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Breed, Breed2LabelMapping, Color2LabelMapping, Colors, QueenState, QueenState2LabelMapping } from '../_models';
import { AlertService } from '../_services/alert.service';
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
    isAddMode = true;
    submitted = false;
    loading = false;
    checked = false;

    constructor(private queenService: QueenService,
                private farmService: FarmService,
                private alertService: AlertService,
                private formBuilder: FormBuilder,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.form = this.formBuilder.group({
            breed: ['', Validators.required],
            hatchingDate: [''],
            markingColor: [''],
            isFertilized: [this.checked],
            broodStartDate: [''],
            state: ['', Validators.required],
            farmId: ['', Validators.required]
        });

        this.id = this.route.snapshot.params['id'];
        
        if (this.id !== undefined) {
            this.isAddMode = false;
            this.form.addControl('id', new FormControl('', Validators.required));
            this.queenService.getById(this.id).subscribe(queen => this.form.patchValue(queen));
        }
        console.log(this.route);
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
            next: () => {
                this.alertService.success('Motinėlė sėkmingai įdėta', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
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
                this.alertService.success('Motinėlės informacija sėkmingai atnaujinta', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: error => {
                this.alertService.error('Serverio klaida');
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