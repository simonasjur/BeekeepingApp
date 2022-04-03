import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { catchError, first, map, startWith, switchMap } from 'rxjs/operators';
import { Apiary, BeeFamily, Farm, TodoItem, TodoItemPriority2LabelMapping, User } from '../_models';

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

@Component({ templateUrl: 'list.component.html'})
export class ListComponent {
    user: User;
    farm: Farm;
    itemsPerPage: number;
    pageNumber:number;
    totalItems:number;
    loading: boolean = true;
    resultsLength = 0;
    resultsLengthCompleted = 0;
    filteredAndPagedTodos: Observable<TodoItem[]>;
    filteredAndPagedTodosCompleted: Observable<TodoItem[]>;
    beeFamilies: BeeFamily[];
    apiaries: Apiary[];
    expandedElement: TodoItem | null;

    public displayedColumns = ['priority', 'title', 'dueDate', 'category', 'action'];

    @ViewChild('firstSort') firstSort: MatSort;
    @ViewChild('secondSort') secondSort: MatSort;
    @ViewChild('firstPaginator') firstPaginator: MatPaginator;
    @ViewChild('secondPaginator') secondPaginator: MatPaginator;

    constructor(private todoService: TodoService,
                private farmService: FarmService,
                private userService: UserService,
                private apiaryService: ApiaryService,
                private beehiveService: BeeFamilyService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute) {
                    
                }
}
