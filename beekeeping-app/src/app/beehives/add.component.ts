import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { BeehiveTypes, BeehiveType2LabelMapping } from '../_models';
import { BeehiveService } from '../_services/beehive.service';

@Component({
    selector: 'add-beehive',
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
            nestCombs: ['', [Validators.required, Validators.max(16), Validators.pattern('^(0|[1-9][0-9]*)$')]],
            acquireDay: ['']
        });

        this.id = this.route.snapshot.params['id'];
        
        if (this.id !== undefined) {
            this.isAddMode = false;
            this.form.addControl('id', new FormControl('', Validators.required));
            this.beehiveService.getById(this.id).subscribe(beehive => this.form.patchValue(beehive));
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

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;

        if (this.isAddMode) {
            this.form.patchValue({isEmpty: 'true', farmId: '1', nestCombs: '0'});
        }
        
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

        if (this.isAddMode) {
            this.createBeehive();
        } else {
            this.updateBeehive();
        }
    }

    private createBeehive() {
        this.beehiveService.create(this.form.value).pipe(first())
            .subscribe(() => {
                this.router.navigate(['../'], { relativeTo: this.route });
            })
            .add(() => this.loading = false);
    }

    private updateBeehive() {
        this.beehiveService.update(this.id, this.form.value).pipe(first())
            .subscribe(() => {
                this.router.navigate(['../../'], { relativeTo: this.route });
            })
            .add(() => this.loading = false);
    }
}