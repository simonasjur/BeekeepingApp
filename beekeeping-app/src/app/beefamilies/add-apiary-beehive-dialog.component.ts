import { Component, Inject } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ActivatedRoute, Router } from "@angular/router";
import { concat, merge } from "rxjs";
//import { ApiaryBeeFamily, BeeFamily, BeehiveTypes, Super } from "../_models";
import { ApiaryBeeFamilyService } from "../_services/apiary-beefamily.service";
import { BeeFamilyService } from "../_services/beefamily.service";

@Component({
    selector: 'add-apiary-beehive-dialog',
    templateUrl: 'add-apiary-beehive-dialog.component.html',
    styleUrls: ['add-apiary-beehive-dialog.component.css']
  })
  export class AddApiaryBeehiveDialog {
    form: FormGroup;
    submitted = false;
    loading = false;

    constructor(public dialogRef: MatDialogRef<AddApiaryBeehiveDialog>,
                                  //@Inject(MAT_DIALOG_DATA) public data: ApiaryBeeFamily,
                private formBuilder: FormBuilder,
                private apiaryBeehiveService: ApiaryBeeFamilyService,
                private beehiveService: BeeFamilyService,
                private router: Router,
                private route: ActivatedRoute) {
    }
    
    ngOnInit() {
        this.form = this.formBuilder.group({
            arriveDate: ['', Validators.required],
            nestCombs: ['', [Validators.required, Validators.max(16), Validators.pattern('^([1-9][0-9]*)$')]],
            supersCount: ['', [Validators.required, Validators.pattern('[1-9]')]]
        });
    }

    get f() {
        return this.form.controls;
    }

    /*isTypeDadano() {
        return this.data.beeFamily.type === BeehiveTypes.Dadano;
    }*/

    onNoClick() {
        this.dialogRef.close();
    }

    addApiaryBeehive() {
        //Adds missing form control value
        if (!this.submitted) {
            /*if (this.isTypeDadano()) {
                this.form.patchValue({supersCount: 1});
            } else {
                this.form.patchValue({nestCombs: 1});
            }*/
        }

        this.submitted = true;

        if (this.form.invalid) {
            return;
        }

        this.loading = true;

        //Construct apiary beehive for post request
        const apiaryBeehive = {
            arriveDate: this.form.controls["arriveDate"].value,
            x: 0,
            y: 0,
            //apiaryId: this.data.apiaryId,
            //beehiveId: this.data.beeFamilyId
        };

        //Construct beehive for put request
        /*let beehive = this.data.beeFamily;
        beehive.isEmpty = false;
        if (this.isTypeDadano()) {
            beehive.nestCombs =this.form.controls['nestCombs'].value;
        }  */     

        //Supers for Daugiaaukstis beehive
        /*let supers: Super[] = [];
        if (!this.isTypeDadano()) {
            for (let i = 0; i < this.form.controls['supersCount'].value; i++) {
                const beehiveSuper: Super = {
                    position: i + 1,
                    color: 0,
                    installationDate: this.form.controls['arriveDate'].value,
                    beehiveId: this.data.beeFamilyId
                }
                supers.push(beehiveSuper);
            }
        }*/
        
        //Sends requests to api
       /* this.apiaryBeehiveService.create(apiaryBeehive).subscribe(() => {
            this.beehiveService.update(beehive.id, beehive).subscribe(() => {
                if (!this.isTypeDadano()) {
                    let superRequestSendCount = 0;
                    const sObservables = supers.map(s => this.superService.create(s));
                    concat(...sObservables).subscribe(() => {
                        superRequestSendCount++;
                        if (superRequestSendCount === supers.length) {
                            this.closeDialog()
                        }
                    });
                } else {
                    this.closeDialog();
                }
            });           
        });*/
    }

    /**Method for closing dialog after requests are finished*/
    closeDialog() {
        this.dialogRef.close();
        this.loading = false;
        const url = this.route.snapshot['_routerState'].url;
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
            this.router.navigate([url]);
        });
    }
}