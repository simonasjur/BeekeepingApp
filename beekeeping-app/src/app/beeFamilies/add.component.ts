import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Apiary, BeeFamilyOrigin2LabelMapping, BeeFamilyOrigins, Beehive, BeehiveTypes } from '../_models';
import { AlertService } from '../_services/alert.service';
import { ApiaryService } from '../_services/apiary.service';
import { BeeFamilyService } from '../_services/beefamily.service';
import { BeehiveService } from '../_services/beehive.service';
import { FarmService } from '../_services/farm.service';

@Component({
    selector: 'add-beefamily',
    templateUrl: 'add.component.html',
    styleUrls: ['add.component.css']
})
export class AddBeeFamilyComponent implements OnInit {
    beehive: Beehive;
    apiaries: Apiary[];
    form: FormGroup;
    isAddMode = true;
    submitted = false;
    loading = false;

    constructor(private formBuilder: FormBuilder,
                private beeFamilyService: BeeFamilyService,
                private farmService: FarmService,
                private beehiveService: BeehiveService,
                private apiaryservice: ApiaryService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.form = this.formBuilder.group({
            origin: ['', Validators.required],
            farmId: ['', Validators.required],
            arriveDate: ['', Validators.required],
            apiaryId: ['', Validators.required],
            beehiveId: ['', Validators.required],
            nestCombs: ['', [Validators.required, Validators.max(16), Validators.pattern('^([1-9][0-9]*)$')]],
            supersCount: ['', [Validators.required, Validators.pattern('[1-9]')]]
        });

        const beehiveId = this.route.snapshot.params['id'];
        this.beehiveService.getById(beehiveId)
            .subscribe(beehive => this.beehive = beehive);
        this.apiaryservice.getFarmApiaries(this.farmService.farmValue.id)
            .subscribe(apiaries => this.apiaries = apiaries);

        
    }

    get beeFamilyOriginsNames() {
        return Object.values(BeeFamilyOrigins).filter(value => typeof value === 'number');
    }

    get beeFamilyOrigin2LabelMapping() {
        return BeeFamilyOrigin2LabelMapping;
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    isTypeDadano() {
        return this.beehive.type === BeehiveTypes.Dadano;
    }

    onSubmit() {
        if (!this.submitted) {
            this.form.patchValue({farmId: this.farmService.farmValue.id});
            this.form.patchValue({beehiveId: this.beehive.id});
            if (this.isTypeDadano()) {
                this.form.patchValue({supersCount: '1'});
            } else {
                this.form.patchValue({nestCombs: '1'})
            }
        }

        this.submitted = true;
        
        if (this.form.invalid) {
            return;
        }

        this.loading = true;

        this.createBeeFamily();
    }

    private createBeeFamily() {
        this.beeFamilyService.create(this.form.value).pipe(first())
            .subscribe(() => {
                this.apiaryservice.getById(this.form.get('apiaryId').value).subscribe(() => {
                    this.alertService.success('Avilys sėkmingai apgyvendintas', { keepAfterRouteChange: true, autoClose: true });
                    this.goToApiaryFamilies();
                })
                .add(() => this.loading = false);
            })
    }

    backToBeeFamiliesList() {
        this.router.navigate(['../../'], { relativeTo: this.route });
    }

    goToApiaryFamilies() {
        const apiaryId = this.form.controls['apiaryId'].value;
        const url = '/apiaries/' + apiaryId + '/beefamilies';
        this.router.navigate([url], { relativeTo: this.route });
    }
}