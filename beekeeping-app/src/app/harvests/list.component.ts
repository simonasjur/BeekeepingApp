import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { catchError, first, map, startWith, switchMap } from 'rxjs/operators';
import { Apiary, BeeFamily, Farm, Harvest, TodoItem, TodoItemPriority2LabelMapping, User } from '../_models';

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

@Component({ templateUrl: 'list.component.html'})
export class ListComponent {
    dataSource: MatTableDataSource<Harvest>;
    harvests: Harvest[];

    public displayedColumns = ['id', 'product', 'startDate', 'endDate'];

    constructor(private harvestService: HarvestService,
                private farmService: FarmService,
                private userService: UserService,
                private apiaryService: ApiaryService,
                private beehiveService: BeeFamilyService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute) {
                }
    
    ngOnInit() {
        this.harvestService.getFarmAllHarvests(this.farmService.farmValue.id).subscribe(harvests => {
            this.harvests = harvests;
            this.dataSource = new MatTableDataSource(harvests);
        });
    }
}
