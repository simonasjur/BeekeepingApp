import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { BeehiveTypes, BeehiveType2LabelMapping } from '../_models';
import { BeehiveService } from '../_services/beehive.service';

@Component({
    selector: 'add-beehive',
    templateUrl: 'add.component.html'
})
export class AddComponent implements OnInit {
    form: FormGroup;
    submitted = false;
    loading = false;

    constructor(private formBuilder: FormBuilder,
                private beehiveService: BeehiveService,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.form = this.formBuilder.group({
            type: ['', Validators.required],
            no: ['', [Validators.required, Validators.pattern('^([1-9][0-9]*)$')]],
            isEmpty: ['', Validators.required],
            farmId: ['', Validators.required],
            acquireDay: ['']
        });
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

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;

        this.form.patchValue({isEmpty: 'true', farmId: '1'});
        if (this.form.invalid) {
            console.log('invalidas');

            return;
        }
        /*const incorrectDate = this.form.controls['acquireDay'].value;
        var correctDate.DatePipe.transform(incorrectDate, 'yyyy-MM-dd');
        this.form.patchValue({acquireDay: correctDate});
        if (this.form.invalid) {
            console.log('invalidas 2');
            return;
        }*/

        this.loading = true;
        this.createBeehive();
    }

    private createBeehive() {
        this.beehiveService.create(this.form.value).pipe(first())
            .subscribe(() => {
                this.router.navigate(['../'], { relativeTo: this.route });
            })
            .add(() => this.loading = false);
    }
}