import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FrameType, FrameType2LabelMapping } from '../_models';

import { AlertService } from '../_services/alert.service';
import { NestExpansionService } from '../_services/nest-expansion.service';

@Component({
    selector: 'add-edit-nest-expansion',
    templateUrl: 'add-edit.component.html',
    styleUrls: ['add-edit.component.css']
})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    nestExpansionId: number;
    beefamilyId: number;
    isAddMode = true;
    submitted = false;
    loading = false;

    constructor(private nestExpansionsService: NestExpansionService,
                private alertService: AlertService,
                private formBuilder: FormBuilder,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.extractBeefamilyId();

        this.form = this.formBuilder.group({
            date: ['', Validators.required],
            frameType: ['', Validators.required],
            combSheets: ['',[Validators.required, Validators.pattern('^(0|[1-9][0-9]*)$')]],
            combs: ['',[Validators.required, Validators.pattern('^(0|[1-9][0-9]*)$')]],
            beefamilyId: [this.beefamilyId]
        });

        this.nestExpansionId = this.route.snapshot.params['id'];
        
        if (this.nestExpansionId !== undefined) {
            this.isAddMode = false;
            this.form.addControl('id', new FormControl('', Validators.required));
            this.nestExpansionsService.getById(this.nestExpansionId).subscribe(expansion => this.form.patchValue(expansion));
        }
    }

    extractBeefamilyId() {
        const url = this.router.url.substring(this.router.url.indexOf('beefamilies')).substring(12);
        this.beefamilyId = +url.substring(0, url.indexOf('/'));
    }

    get frameTypeNames() {
        return Object.values(FrameType).filter(value => typeof value === 'number');
    }

    get frameType2LabelMapping() {
        return FrameType2LabelMapping;
    }

    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;
        
        if (this.form.invalid) {
            return;
        }
      
        this.loading = true;
       
        if (this.isAddMode) {
            this.createNestExpansion();
        } else {
            this.updateNestExpansion();
        }
    }

    private createNestExpansion() {
        this.nestExpansionsService.create(this.form.value).subscribe({
            next: () => {
                this.alertService.success('Avilio plėtimas sėkmingai sukurtas', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko sukurti avilio plėtimo');
            }
        });
    }

    private updateNestExpansion() {
        this.nestExpansionsService.update(this.nestExpansionId, this.form.value).subscribe({
            next: () => {
                this.alertService.success('Avilio plėtimas sėkmingai atnaujintas', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko atnaujinti avilio plėtimo');
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