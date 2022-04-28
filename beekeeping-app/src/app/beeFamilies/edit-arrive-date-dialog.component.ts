import { Component, Inject } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ActivatedRoute, Router } from "@angular/router";
import { ApiaryBeeFamily } from "../_models";
import { AlertService } from "../_services/alert.service";
import { ApiaryBeeFamilyService } from "../_services/apiary-beefamily.service";

@Component({
    selector: 'edit-arrive-date-dialog',
    templateUrl: 'edit-arrive-date-dialog.component.html'
  })
  export class EditArriveDateDialog {
    form: FormGroup;
    today: Date;
    submitted = false;
    loading = false;

    constructor(public dialogRef: MatDialogRef<EditArriveDateDialog>,
                                  @Inject(MAT_DIALOG_DATA) public data: ApiaryBeeFamily,
                private formBuilder: FormBuilder,
                private apiaryBeefamilyService: ApiaryBeeFamilyService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute) {
    }
    
    ngOnInit() {
        this.today = new Date();
        this.form = this.formBuilder.group({
            id: ['', Validators.required],
            arriveDate: ['', Validators.required]
        });
        this.form.patchValue(this.data);
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
                this.alertService.success('Pastatymo data sėkmingai atnaujinta', { keepAfterRouteChange: true, autoClose: true });
                this.closeDialog();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko atnaujinti pastatymo datos');
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