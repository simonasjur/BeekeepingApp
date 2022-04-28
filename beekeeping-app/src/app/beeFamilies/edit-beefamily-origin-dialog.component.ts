import { Component, Inject } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ActivatedRoute, Router } from "@angular/router";
import { BeeFamily, BeeFamilyOrigin2LabelMapping, BeeFamilyOrigins } from "../_models";
import { AlertService } from "../_services/alert.service";
import { BeeFamilyService } from "../_services/beefamily.service";

@Component({
    selector: 'edit-beefamily-origin-dialog',
    templateUrl: 'edit-beefamily-origin-dialog.component.html'
  })
  export class EditBeefamilyOriginDialog {
    form: FormGroup;
    submitted = false;
    loading = false;

    constructor(public dialogRef: MatDialogRef<EditBeefamilyOriginDialog>,
                                  @Inject(MAT_DIALOG_DATA) public data: BeeFamily,
                private formBuilder: FormBuilder,
                private beefamilyService: BeeFamilyService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute) {
    }
    
    ngOnInit() {
        this.form = this.formBuilder.group({
            id: ['', Validators.required],
            origin: ['', Validators.required],
            state: ['', Validators.required]
        });
        this.form.patchValue(this.data);
    }

    get beeFamilyOriginsNames() {
        return Object.values(BeeFamilyOrigins).filter(value => typeof value === 'number');
    }

    get beeFamilyOrigin2LabelMapping() {
        return BeeFamilyOrigin2LabelMapping;
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
        
        this.beefamilyService.update(this.form.controls['id'].value, this.form.value).subscribe({
            next: () => {
                this.alertService.success('Šeimos kilmė sėkmingai atnaujinta', { keepAfterRouteChange: true, autoClose: true });
                this.closeDialog();
            },
            error: () => {
                this.alertService.error('Serverio klaida: nepavyko atnaujinti šeimos kilmės');
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