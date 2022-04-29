import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { catchError, first, map, startWith, switchMap } from 'rxjs/operators';
import { Apiary, ApiaryBeeFamily, BeeFamily, Farm, Harvest, HarvestProduct2LabelMapping, HarvestProducts, TodoItem, TodoItemPriority2LabelMapping, User, Worker } from '../_models';

import { FarmService } from '../_services/farm.service';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alert.service';
import { TodoService } from '../_services/todo-item.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSort } from '@angular/material/sort';
import { Observable, merge, of as observableOf } from 'rxjs';
import { ApiaryService } from '../_services/apiary.service';
import { BeeFamilyService } from '../_services/beefamily.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { HarvestService } from '../_services/harvest.service';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup } from '@angular/forms';
import { formatDate } from '@angular/common';
import { ApiaryBeeFamilyService } from '../_services/apiary-beefamily.service';
import { DeleteDialog } from '../_components/delete-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { WorkerService } from '../_services/worker.service';

@Component({ templateUrl: 'list.component.html',
styleUrls: ['list.component.css']})
export class ListComponent {
    dataSource: MatTableDataSource<Harvest>;
    harvests: Harvest[];
    oldHarvests: Harvest[];
    apiaries: Apiary[];
    beeFamilies: BeeFamily[] = [];
    form: FormGroup;
    honey: number = 0;
    wax: number = 0;
    bread: number = 0;
    propolis: number = 0;
    fromApiary: boolean = false;
    fromFamily: boolean = false;
    apiary: Apiary;
    apiaryId: number;
    families: BeeFamily[];
    loading: boolean = true;
    worker: Worker;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    public displayedColumns = ['id', 'product', 'category', 'startDate', 'endDate', 'quantity', 'action'];

    public footerColumns = ['id', 'product', 'category', 'startDate', 'endDate'];

    constructor(private harvestService: HarvestService,
                private farmService: FarmService,
                private userService: UserService,
                private apiaryService: ApiaryService,
                private beehiveService: BeeFamilyService,
                private alertService: AlertService,
                private beeFamiliesService: BeeFamilyService,
                private apiaryFamilyService: ApiaryBeeFamilyService,
                private workerService: WorkerService,
                private router: Router,
                private route: ActivatedRoute,
                private formBuilder: FormBuilder,
                private dialog: MatDialog) {
                }
    
    ngOnInit() {
        this.form = this.formBuilder.group({
            apiaryId: [],
            beeFamilyId: [],
            startDate: [],
            endDate: [],
            product: []
        });
        
        if (this.router.url.includes('beefamilies')) {
            this.fromFamily = true;
        } else if (this.router.url.includes('apiaries')) {
            this.fromApiary = true;
            const url = this.router.url.substring(this.router.url.indexOf('apiaries/')).substring(9);
            this.apiaryId = +url.substring(0, url.indexOf('/'));
        }

        this.workerService.getFarmAndUserWorker(this.farmService.farmValue.id).subscribe(worker => {
            this.worker = worker;
            this.harvestService.getFarmAllHarvests(this.farmService.farmValue.id).subscribe(harvests => {
                this.apiaryService.getFarmApiaries(this.farmService.farmValue.id).subscribe(apiaries => {
                    this.beeFamiliesService.getFarmAllBeeFamilies(this.farmService.farmValue.id).subscribe(beeFamilies => {
    
                            if (this.fromApiary) {
                                this.apiaryFamilyService.getOneApiaryBeeFamilies(this.apiaryId).subscribe(apiaryFamilies => {
                                    this.apiaries = apiaries.filter(apiary => apiary.id === this.apiaryId);
                                    apiaryFamilies.forEach(apiaryFamily => this.beeFamilies.push(apiaryFamily.beeFamily));
                                    this.beeFamilies = this.beeFamilies.filter(family => harvests.some(harvest => harvest.beeFamilyId == family.id));
                                    const apiaryHarvests = harvests.filter(harvest => harvest.apiaryId == this.apiaryId);
                                    const familiesHarvests = harvests.filter(harvest => this.beeFamilies.some(family => family.id == harvest.beeFamilyId));
                                    this.harvests = apiaryHarvests.concat(familiesHarvests);
                                    this.calculateOverall(this.harvests);
                                    this.dataSource = new MatTableDataSource(this.harvests);
                                    this.dataSource.paginator = this.paginator;
                                    this.dataSource.sort = this.sort;
                                    this.oldHarvests = this.harvests;
                                    this.form.valueChanges.subscribe(() => {
                                        this.harvests = this.oldHarvests;
                                        if (this.form.get('startDate').value !== null) {
                                            const startDate = formatDate(this.form.get('startDate').value,'yyyy-MM-dd','en_US');
                                            this.harvests = this.harvests.filter(x => x.startDate != null && formatDate(x.startDate, 'yyyy-MM-dd','en_US') >= startDate);
                                        }
                                        if (this.form.get('endDate').value !== null) {
                                            const endDate = formatDate(this.form.get('endDate').value,'yyyy-MM-dd','en_US');
                                            this.harvests = this.harvests.filter(x => x.endDate != null && formatDate(x.endDate, 'yyyy-MM-dd','en_US') <= endDate);
                                        }
                                        if (this.form.get('apiaryId').value !== null && this.form.get('beeFamilyId').value !== null) {
                                            this.harvests = this.harvests.filter(x => x.apiaryId === this.form.get('apiaryId').value || x.beeFamilyId === this.form.get('beeFamilyId').value);
                                        } else if (this.form.get('apiaryId').value !== null) {
                                            this.harvests = this.harvests.filter(x => x.apiaryId === this.form.get('apiaryId').value);
                                        } else if (this.form.get('beeFamilyId').value !== null) {
                                            this.harvests = this.harvests.filter(x => x.beeFamilyId === this.form.get('beeFamilyId').value);
                                        }
            
                                        if (this.form.get('product').value !== null) {
                                            this.harvests = this.harvests.filter(x => x.product === this.form.get('product').value);
                                        }
            
                                        this.calculateOverall(this.harvests);
                                        this.dataSource = new MatTableDataSource(this.harvests);
                                    
                                    });
                                    this.loading = false;
                                    });
                            } else {
                                this.harvests = harvests;
                                this.apiaries = apiaries.filter(apiary => this.harvests.some(harvest => harvest.apiaryId === apiary.id));
                                this.beeFamilies = beeFamilies.filter(family => this.harvests.some(harvest => harvest.beeFamilyId === family.id));
                                this.calculateOverall(this.harvests);
                                this.dataSource = new MatTableDataSource(this.harvests);
                                this.dataSource.paginator = this.paginator;
                                this.dataSource.sort = this.sort;
                                this.oldHarvests = this.harvests;
                                this.form.valueChanges.subscribe(() => {
                                    this.harvests = this.oldHarvests;
                                    if (this.form.get('startDate').value !== null) {
                                        const startDate = formatDate(this.form.get('startDate').value,'yyyy-MM-dd','en_US');
                                        this.harvests = this.harvests.filter(x => x.startDate != null && formatDate(x.startDate, 'yyyy-MM-dd','en_US') >= startDate);
                                    }
                                    if (this.form.get('endDate').value !== null) {
                                        const endDate = formatDate(this.form.get('endDate').value,'yyyy-MM-dd','en_US');
                                        this.harvests = this.harvests.filter(x => x.endDate != null && formatDate(x.endDate, 'yyyy-MM-dd','en_US') <= endDate);
                                    }
                                    if (this.form.get('apiaryId').value !== null && this.form.get('beeFamilyId').value !== null) {
                                        this.harvests = this.harvests.filter(x => x.apiaryId === this.form.get('apiaryId').value || x.beeFamilyId === this.form.get('beeFamilyId').value);
                                    } else if (this.form.get('apiaryId').value !== null) {
                                        this.harvests = this.harvests.filter(x => x.apiaryId === this.form.get('apiaryId').value);
                                    } else if (this.form.get('beeFamilyId').value !== null) {
                                        this.harvests = this.harvests.filter(x => x.beeFamilyId === this.form.get('beeFamilyId').value);
                                    }
        
                                    if (this.form.get('product').value !== null) {
                                        this.harvests = this.harvests.filter(x => x.product === this.form.get('product').value);
                                    }
        
                                    this.calculateOverall(this.harvests);
                                    this.dataSource = new MatTableDataSource(this.harvests);
                                
                                });
                                this.loading = false;
                            }
                    });
                });
            });
        });
    }

    get harvestProduct2LabelMapping() {
        return HarvestProduct2LabelMapping;
    }

    get products() {
        return HarvestProducts;
    }

    get harvestProducts() {
        return Object.values(this.products).filter(value => typeof value === 'number');
    }

    deleteHarvest(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
               this.harvestService.delete(id).subscribe({
                    next: () => {
                        this.dataSource.data = this.dataSource.data.filter(x => x.id !== id);
                        this.harvests = this.harvests.filter(x => x.id !== id);
                        this.oldHarvests = this.oldHarvests.filter(x => x.id !== id);
                        this.calculateOverall(this.dataSource.data);
                        this.alertService.success('Kopimas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: error => {
                        this.alertService.error(error);
                    }
                }); 
            }
        });
        
    }

    calculateOverall(harvests) {
        this.bread = 0;
        this.wax = 0;
        this.propolis = 0;
        this.honey = 0;

        for (let i = 0; i < harvests.length; i++) {
            switch (harvests[i].product) {
                case 0:
                    this.bread += harvests[i].quantity;
                    break;
                case 1:
                    this.wax += harvests[i].quantity;
                    break;
                case 2:
                    this.propolis += harvests[i].quantity;
                    break;
                case 3:
                    this.honey += harvests[i].quantity;
                    break;
            }
        }
    }
}
