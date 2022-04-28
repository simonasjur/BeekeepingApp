import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FarmService } from "../_services/farm.service";
import { WorkerService } from "../_services/worker.service";
import { Worker } from "../_models"
import { FormArray, FormBuilder, FormGroup } from "@angular/forms";
import { AlertService } from "../_services/alert.service";

@Component({
    selector: 'worker-dialog',
    templateUrl: 'worker-dialog.component.html',
    styleUrls: ['worker-dialog.component.css']
})
export class WorkerDialog {
    worker: Worker;
    dataSource = [];
    tableForm: FormGroup;
    loading: boolean = true;
    loading2: boolean = false;

    displayedColumns: string[] = ['object', 'create', 'edit', 'delete'];
    objects: string[] = ['Šeimos', 'Aviliai', 'Avilio komponentai', 'Kopimai', 'Plėtimai', 'Siaurinimai', 
    'Motinėlės', 'Motinėlių auginimai', 'Užduotys', 'Maitinimai'];

    constructor(
        public dialogRef: MatDialogRef<WorkerDialog>,
        private workerService: WorkerService,
        private farmService: FarmService,
        private formBuilder: FormBuilder,
        private alertService: AlertService,
        @Inject(MAT_DIALOG_DATA) public data: number,
    ) {}

    ngOnInit() {
        this.workerService.getFarmWorker(this.farmService.farmValue.id, this.data).subscribe(worker => {
            this.worker = worker;
            var permissionsString = this.worker.permissions;
            console.log(permissionsString)

            for (let i = 0; i < permissionsString.length; i+=3) {
                const permission = {
                    object: this.objects[i/3],
                    create: Boolean(Number(permissionsString[i])),
                    edit: Boolean(Number(permissionsString[i+1])),
                    delete: Boolean(Number(permissionsString[i+2]))
                }
                this.dataSource.push(permission);
            }
            this.tableForm = this.formBuilder.group({
                permissions: this.formBuilder.array([])
            })
            this.setPermissionsForm();
            this.loading = false;
        });
    }

    onSubmit() {
        this.loading2 = true;
        let permissionsString = '';
        this.tableForm.value.permissions.forEach(p => {
            permissionsString += String(Number(p.create)) + String(Number(p.edit)) + String(Number(p.delete));
        });

        this.workerService.edit(this.worker.farmId, this.worker.userId, permissionsString).subscribe({
            next: () => {
                this.loading2 = false;
                this.dialogRef.close();
                this.alertService.success('Darbininkas sėkmingai pakeistas', { keepAfterRouteChange: true, autoClose: true });
            },
            error: error => {
                this.loading2 = false;
                this.alertService.error(error);
            }
        });
    }

    private setPermissionsForm() {
        const permissionCtrl = this.tableForm.get('permissions') as FormArray;
        this.dataSource.forEach((permission)=>{
            permissionCtrl.push(this.setPermissionsFormArray(permission))
        });
    };
    
    private setPermissionsFormArray(permission) {
        return this.formBuilder.group({
            create:[permission.create],
            edit:[permission.edit],
            delete:[permission.delete] 
        });
    }
}