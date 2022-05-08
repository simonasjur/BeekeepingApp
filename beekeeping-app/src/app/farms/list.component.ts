import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { first } from 'rxjs/operators';
import { Role2LabelMapping, User, Worker } from '../_models';

import { FarmService } from '../_services/farm.service';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alert.service';
import { DeleteDialog } from '../_components/delete-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InvitationService } from '../_services/invitation.service';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkerService } from '../_services/worker.service';

@Component({ templateUrl: 'list.component.html',
styleUrls: ['list.component.css'] })
export class ListComponent implements OnInit {
    user: User;
    farms = null;
    itemsPerPage: number;
    pageNumber:number;
    totalItems:number;
    form: FormGroup;
    errors: boolean = false;
    loading: boolean = false;
    loading2: boolean = false;
    submitted = false;
    workers: Worker[];

    constructor(private formBuilder: FormBuilder,
                private farmService: FarmService,
                private userService: UserService,
                private invitationService: InvitationService,
                private workerService: WorkerService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute,
                private dialog: MatDialog) {
                
                }

    ngOnInit() {
        this.form = this.formBuilder.group({
            code: ['', Validators.required]
        });
        this.loading2 = true;
        this.farmService.getAll().pipe(first()).subscribe(farms => {
                this.farms = farms;
                this.userService.user.subscribe(user => {
                    this.user = user;
                    this.workerService.getUserWorkers().subscribe(workers => {
                        this.workers = workers;
                        this.workers.forEach(worker => {
                            this.farms.forEach(farm => {
                                if (worker.farmId == farm.id) {
                                    farm.worker = worker;
                                };
                            });
                        });
                        this.loading2 = false;
                    });
                });
            });
    }

    get f() { return this.form.controls; }

    get role2LabelMapping() {
        return Role2LabelMapping;
    }

    onSubmit() {
        this.submitted = true;

        this.errors = null;
        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        this.invitationService.validateCode(this.form.get('code').value).subscribe({
            next: result => {
                
                this.farmService.updateLocalStorageFarm(result).subscribe(() => {
                    this.userService.updateLocalStorageUser().subscribe(() => {
                        this.loading = false;
                        this.router.navigate(['/home'], { relativeTo: this.route });
                    });
                });
            },
            error: err => {
                this.loading = false;
                this.errors = true;
            }
        });
    }


    /*onPaginate(pageEvent: PageEvent) {
        this.itemsPerPage = +pageEvent.pageSize;
        this.pageNumber = +pageEvent.pageIndex + 1;
        this.farmService.getAll()
            .pipe(first())
            .subscribe(farms => this.farms = farms);
        this.totalItems = this.farms.length;
        this.farmService.getFarms(this.itemsPerPage, this.pageNumber);
        console.log("page number " + this.pageNumber);
        console.log("items per page " + this.itemsPerPage);
    }*/

    loadFarm(id) {       
        this.farmService.updateLocalStorageFarm(id).subscribe(() => {
            this.router.navigate(['../home/'], { relativeTo: this.route });
        });
        //this.currentFarm = this.farmService.farmValue;
    }

    deleteFarm(id: number) {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                const farm = this.farms.find(x => x.id === id);
                farm.isDeleting = true;
                this.farmService.delete(id)
                    .pipe(first())
                    .subscribe(() => { 
                        this.farms = this.farms.filter(x => x.id !== id);
                        this.alertService.success('Ūkis ' + farm.name + ' ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    })
            }
        });
        
    }
}
