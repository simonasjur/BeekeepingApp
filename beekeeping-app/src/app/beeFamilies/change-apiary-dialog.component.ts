import { Component, Inject } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ActivatedRoute, Router } from "@angular/router";
import { Apiary, ApiaryBeeFamily, BeeFamilyOrigin2LabelMapping, BeeFamilyOrigins } from "../_models";
import { AlertService } from "../_services/alert.service";
import { ApiaryBeeFamilyService } from "../_services/apiary-beefamily.service";
import { ApiaryService } from "../_services/apiary.service";
import { FarmService } from "../_services/farm.service";

@Component({
    selector: 'change-apiary-dialog',
    templateUrl: 'change-apiary-dialog.component.html',
    styleUrls: ['change-apiary-dialog.component.css']
  })
  export class ChangeApiaryDialog {
    form: FormGroup;
    apiaries: Apiary[];
    today: Date;
    submitted = false;
    loading = false;
    formLoading = true;

    constructor(public dialogRef: MatDialogRef<ChangeApiaryDialog>,
                                  @Inject(MAT_DIALOG_DATA) public data: ApiaryBeeFamily,
                private formBuilder: FormBuilder,
                private apiaryBeefamilyService: ApiaryBeeFamilyService,
                private apiaryService: ApiaryService,
                private farmService: FarmService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute) {
    }
    
    ngOnInit() {
        this.today = new Date();
        this.form = this.formBuilder.group({
            id: ['', Validators.required],
            arriveDate: ['', Validators.required],
            departDate: ['', Validators.required],
            apiaryId: ['', Validators.required]
        });
        this.form.patchValue(this.data);
        this.apiaryService.getFarmApiaries(this.farmService.farmValue.id).subscribe(apiaries => {
            this.apiaries = apiaries.filter(a => a.id != this.data.apiaryId);
            this.formLoading = false;
        });
    }

    get f() {
        return this.form.controls;
    }

    onNoClick() {
        this.dialogRef.close();
    }

    onSubmit() {
        this.submitted = true;

        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        
        this.apiaryBeefamilyService.update(this.form.controls['id'].value, this.form.value).subscribe({
            next: () => {
                const newApiaryBeefamily = {
                    arriveDate: this.form.controls['departDate'].value,
                    apiaryId: this.form.controls['apiaryId'].value,
                    beeFamilyId: this.data.beeFamilyId
                };
                this.apiaryBeefamilyService.create(newApiaryBeefamily).subscribe({
                    next: () => {
                        this.alertService.success('Šeima sėkmingai perkelta', { keepAfterRouteChange: true, autoClose: true });
                        this.closeDialog();
                    }
                })
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko perkelti šeimos');
                this.closeDialog();
            }
        });
    }

    closeDialog() {
        this.dialogRef.close();
        this.loading = false;
        const url = this.route.snapshot['_routerState'].url;
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
            this.router.navigate([url]);
        });
    }
}