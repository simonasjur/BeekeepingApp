import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { BeeFamilyOrigin2LabelMapping, BeeFamilyOrigins } from '../_models';
import { BeeFamilyService } from '../_services/beeFamily.service';

@Component({
    selector: 'add-beeFamily',
    templateUrl: 'add.component.html',
    styleUrls: ['add.component.css']
})
export class AddComponent implements OnInit {
    form: FormGroup;
    id: number;
    isAddMode = true;
    submitted = false;
    loading = false;

    constructor(private formBuilder: FormBuilder,
                private beeFamilyService: BeeFamilyService,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.form = this.formBuilder.group({
            /*type: ['', Validators.required],
            no: ['', [Validators.required, Validators.pattern('^([1-9][0-9]*)$')]],
            isEmpty: ['', Validators.required],
            farmId: ['', Validators.required],
            nestCombs: ['', [Validators.required, Validators.max(16), Validators.pattern('^(0|[1-9][0-9]*)$')]],
            acquireDay: ['']*/
            origin: ['', Validators.required],
            farmId: ['', Validators.required]
        });

        this.id = this.route.snapshot.params['id'];
        
        if (this.id !== undefined) {
            this.isAddMode = false;
            this.form.addControl('id', new FormControl('', Validators.required));
            this.beeFamilyService.getById(this.id).subscribe(beehive => this.form.patchValue(beehive));
        }
    }

    get beeFamilyOriginsNames() {
        return Object.values(BeeFamilyOrigins).filter(value => typeof value === 'number');
    }

    get beeFamilyOrigin2LabelMapping() {
        return BeeFamilyOrigin2LabelMapping;
    }

    /*get beehiveTypes() {
        return BeehiveTypes;
    }

    get beehiveTypesNames() {
        return Object.values(BeehiveTypes).filter(value => typeof value === 'number');
    }

    get beehiveType2LabelMapping() {
        return BeehiveType2LabelMapping;
    }*/

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    onSubmit() {
        if (!this.submitted) {
            if (this.isAddMode) {
                this.form.patchValue({farmId: '1'});
            }
            /*if (this.form.controls['type'].value === this.beehiveTypes.Daugiaaukstis) {
                this.form.removeControl('nestCombs');
            }*/
        }

        this.submitted = true;
        
        if (this.form.invalid) {
            return;
        }

        this.loading = true;

        if (this.isAddMode) {
            this.createBeeFamily();
        } else {
            this.updateBeeFamily();
        }
    }

    private createBeeFamily() {
        this.beeFamilyService.create(this.form.value).pipe(first())
            .subscribe(() => {
                this.backToBeeFamiliesList();
            })
            .add(() => this.loading = false);
    }

    private updateBeeFamily() {
        this.beeFamilyService.update(this.id, this.form.value).pipe(first())
            .subscribe(() => {
                this.backToBeeFamiliesList();
            })
            .add(() => this.loading = false);
    }

    backToBeeFamiliesList() {
        if (this.isAddMode) {
            this.router.navigate(['../'], { relativeTo: this.route });
        } else {
            this.router.navigate(['../../'], { relativeTo: this.route });
        }
    }
}