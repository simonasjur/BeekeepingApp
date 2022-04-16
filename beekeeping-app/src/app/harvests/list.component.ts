import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { catchError, first, map, startWith, switchMap } from 'rxjs/operators';
import { Apiary, BeeFamily, Farm, Harvest, HarvestProduct2LabelMapping, TodoItem, TodoItemPriority2LabelMapping, User } from '../_models';

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

@Component({ templateUrl: 'list.component.html',
styleUrls: ['list.component.css']})
export class ListComponent {
    dataSource: MatTableDataSource<Harvest>;
    harvests: Harvest[];
    apiaries: Apiary[];
    beeFamilies: BeeFamily[];
    form: FormGroup;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    public displayedColumns = ['id', 'product', 'category', 'startDate', 'endDate', 'action'];

    constructor(private harvestService: HarvestService,
                private farmService: FarmService,
                private userService: UserService,
                private apiaryService: ApiaryService,
                private beehiveService: BeeFamilyService,
                private alertService: AlertService,
                private beeFamiliesService: BeeFamilyService,
                private router: Router,
                private route: ActivatedRoute,
                private formBuilder: FormBuilder) {
                }
    
    ngOnInit() {
        this.form = this.formBuilder.group({
            apiaryId: [],
            beeFamilyId: []
        });

        this.harvestService.getFarmAllHarvests(this.farmService.farmValue.id).subscribe(harvests => {
            this.harvests = harvests;
            this.dataSource = new MatTableDataSource(harvests);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
            this.apiaryService.getFarmApiaries(this.farmService.farmValue.id).subscribe(apiaries => {
                this.apiaries = apiaries.filter(apiary => this.harvests.some(harvest => harvest.apiaryId === apiary.id));
                this.beeFamiliesService.getFarmAllBeeFamilies(this.farmService.farmValue.id).subscribe(beeFamilies => {
                    this.beeFamilies = beeFamilies.filter(family => this.harvests.some(harvest => harvest.beeFamilyId === family.id));
                    this.form.valueChanges.subscribe(() => {
                        if (this.form.get('apiaryId').value !== null && this.form.get('beeFamilyId').value !== null) {
                            this.dataSource = new MatTableDataSource(harvests
                                .filter(x => x.apiaryId === this.form.get('apiaryId').value || x.beeFamilyId === this.form.get('beeFamilyId').value));
                        } else if (this.form.get('apiaryId').value !== null && this.form.get('beeFamilyId').value === null) {
                            this.dataSource = new MatTableDataSource(harvests.filter(x => x.apiaryId === this.form.get('apiaryId').value || x.beeFamilyId !== 0));
                            console.log(harvests.filter(x => x.apiaryId === this.form.get('apiaryId').value || x.beeFamilyId !== null))
                        } else if (this.form.get('beeFamilyId').value !== null && this.form.get('apiaryId').value === null) {
                            this.dataSource = new MatTableDataSource(harvests.filter(x => x.beeFamilyId === this.form.get('beeFamilyId').value || x.apiaryId !== 0));
                        } else {
                            this.dataSource = new MatTableDataSource(harvests);
                        }
                    });
                });
            });
        });
    }

    get harvestProduct2LabelMapping() {
        return HarvestProduct2LabelMapping;
    }

    deleteHarvest(id: number): void {
        this.harvestService.delete(id).subscribe({
            next: () => {
                this.dataSource.data = this.dataSource.data.filter(x => x.id !== id);
                this.alertService.success('Kopimas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
            },
            error: error => {
                this.alertService.error(error);
            }
        });
    }
}
