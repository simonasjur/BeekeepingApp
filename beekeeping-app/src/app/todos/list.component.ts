import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { catchError, count, debounceTime, delay, distinctUntilChanged, first, map, startWith, switchMap, tap } from 'rxjs/operators';
import { Apiary, BeeFamily, Farm, TodoItem, TodoItemPriority2LabelMapping, User } from '../_models';

import { FarmService } from '../_services/farm.service';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alert.service';
import { TodoService } from '../_services/todoItem.service';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSort } from '@angular/material/sort';
import { Observable, merge, of as observableOf, of } from 'rxjs';
import { ApiaryService } from '../_services/apiary.service';
import { BeeFamilyService } from '../_services/beeFamily.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';

@Component({ selector: 'todos-list',
    templateUrl: 'list.component.html',
styleUrls: ['list.component.css'],
animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
 })
export class ListComponent {
    form: FormGroup;
    formSecond: FormGroup;
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
    filter = new FormControl();

    public displayedColumns = ['priority', 'title', 'dueDate', 'category', 'action'];

    @ViewChild('firstSort') firstSort: MatSort;
    @ViewChild('secondSort') secondSort: MatSort;
    @ViewChild('firstPaginator') firstPaginator: MatPaginator;
    @ViewChild('secondPaginator') secondPaginator: MatPaginator;

    constructor(private todoService: TodoService,
                private farmService: FarmService,
                private apiaryService: ApiaryService,
                private beeFamiliesService: BeeFamilyService,
                private alertService: AlertService,
                private formBuilder: FormBuilder) {
                    
                }

    
    ngOnInit() {
        this.form = this.formBuilder.group({
            filter: [],
            apiaryId: [],
            beeFamilyId: [],
        });
        this.formSecond = this.formBuilder.group({
            filter: [],
            apiaryId: [],
            beeFamilyId: [],
        });
    }
    
    ngAfterViewInit() {
        this.apiaryService.getFarmApiaries(this.farmService.farmValue.id).subscribe(apiaries => {
            this.apiaries = apiaries;
            this.beeFamiliesService.getFarmAllBeeFamilies(this.farmService.farmValue.id).subscribe(beeFamilies => {
                this.beeFamilies = beeFamilies;
                this.filteredAndPagedTodos = merge(this.form.valueChanges, this.firstSort.sortChange, this.firstPaginator.page)
                    .pipe(
                        startWith({}),
                        debounceTime(200),
                        switchMap(() => {
                        this.loading = true;
                        if (this.form.get('apiaryId').value !== null && this.form.get('beeFamilyId').value !== null) {
                            return this.todoService.getAllFarmTodosByApiaryAndBeeFamily(
                                this.farmService.farmValue.id,
                                false,
                                this.form.get('filter').value,
                                this.firstSort.active,
                                this.firstSort.direction,
                                this.firstPaginator.pageIndex,
                                this.form.get('apiaryId').value,
                                this.form.get('beeFamilyId').value
                            );
                        } else if (this.form.get('apiaryId').value !== null) {
                            return this.todoService.getAllFarmTodosByApiary(
                                this.farmService.farmValue.id,
                                false,
                                this.form.get('filter').value,
                                this.firstSort.active,
                                this.firstSort.direction,
                                this.firstPaginator.pageIndex,
                                this.form.get('apiaryId').value
                            );
                        } else if (this.form.get('beeFamilyId').value !== null) {
                            return this.todoService.getAllFarmTodosByBeeFamily(
                                this.farmService.farmValue.id,
                                false,
                                this.form.get('filter').value,
                                this.firstSort.active,
                                this.firstSort.direction,
                                this.firstPaginator.pageIndex,
                                this.form.get('beeFamilyId').value
                            );}                        
                        else {
                            return this.todoService.getAllFarmTodosByFilterPaged(
                                this.farmService.farmValue.id,
                                false,
                                this.form.get('filter').value,
                                this.firstSort.active,
                                this.firstSort.direction,
                                this.firstPaginator.pageIndex,
                            );}
                        }),
                        map(data => {
                        // Flip flag to show that loading has finished.
                        this.loading = false;
                        if (this.form.get('apiaryId').value !== null && this.form.get('beeFamilyId').value !== null) {
                            this.todoService.getAllFarmTodosByApiaryAndBeeFamily(
                                this.farmService.farmValue.id,
                                false,
                                this.form.get('filter').value,
                                this.firstSort.active,
                                this.firstSort.direction,
                                this.firstPaginator.pageIndex,
                                this.form.get('apiaryId').value,
                                this.form.get('beeFamilyId').value
                            ).subscribe(todos => this.resultsLength = todos.length);
                        } else if (this.form.get('apiaryId').value !== null) {
                            this.todoService.getAllFarmTodosByApiary(
                                this.farmService.farmValue.id,
                                false,
                                this.form.get('filter').value,
                                this.firstSort.active,
                                this.firstSort.direction,
                                this.firstPaginator.pageIndex,
                                this.form.get('apiaryId').value
                            ).subscribe(todos => this.resultsLength = todos.length);
                        } else if (this.form.get('beeFamilyId').value !== null) {
                            this.todoService.getAllFarmTodosByBeeFamily(
                                this.farmService.farmValue.id,
                                false,
                                this.form.get('filter').value,
                                this.firstSort.active,
                                this.firstSort.direction,
                                this.firstPaginator.pageIndex,
                                this.form.get('beeFamilyId').value
                            ).subscribe(todos => this.resultsLength = todos.length);
                        } else {
                            this.todoService.getAllFarmTodosByFilterPaged(
                                this.farmService.farmValue.id,
                                false,
                                this.form.get('filter').value,
                                this.firstSort.active,
                                this.firstSort.direction,
                                this.firstPaginator.pageIndex,
                            ).subscribe(todos => this.resultsLength = todos.length);  
                        }
                        return data;
                        }),
                        catchError(() => {
                        this.loading = false;
                        return observableOf([]);
                        })
                    );
                   
                this.filteredAndPagedTodosCompleted = merge(this.formSecond.valueChanges, this.secondSort.sortChange, this.secondPaginator.page)
                    .pipe(
                        startWith({}),
                        debounceTime(200),
                        switchMap(() => {
                        this.loading = true;
                        if (this.formSecond.get('apiaryId').value !== null && this.formSecond.get('beeFamilyId').value !== null) {
                            return this.todoService.getAllFarmTodosByApiaryAndBeeFamily(
                                this.farmService.farmValue.id,
                                true,
                                this.formSecond.get('filter').value,
                                this.secondSort.active,
                                this.secondSort.direction,
                                this.secondPaginator.pageIndex,
                                this.formSecond.get('apiaryId').value,
                                this.formSecond.get('beeFamilyId').value
                            );
                        } else if (this.formSecond.get('apiaryId').value !== null) {
                            return this.todoService.getAllFarmTodosByApiary(
                                this.farmService.farmValue.id,
                                true,
                                this.formSecond.get('filter').value,
                                this.secondSort.active,
                                this.secondSort.direction,
                                this.secondPaginator.pageIndex,
                                this.formSecond.get('apiaryId').value
                            );
                        } else if (this.formSecond.get('beeFamilyId').value !== null) {
                            return this.todoService.getAllFarmTodosByBeeFamily(
                                this.farmService.farmValue.id,
                                true,
                                this.formSecond.get('filter').value,
                                this.secondSort.active,
                                this.secondSort.direction,
                                this.secondPaginator.pageIndex,
                                this.formSecond.get('beeFamilyId').value
                            );}                        
                        else {
                            return this.todoService.getAllFarmTodosByFilterPaged(
                                this.farmService.farmValue.id,
                                true,
                                this.formSecond.get('filter').value,
                                this.secondSort.active,
                                this.secondSort.direction,
                                this.secondPaginator.pageIndex,
                            );}
                        }),
                        map(data => {
                        // Flip flag to show that loading has finished.
                        this.loading = false;
                        if (this.formSecond.get('apiaryId').value !== null && this.formSecond.get('beeFamilyId').value !== null) {
                            this.todoService.getAllFarmTodosByApiaryAndBeeFamily(
                                this.farmService.farmValue.id,
                                true,
                                this.formSecond.get('filter').value,
                                this.secondSort.active,
                                this.secondSort.direction,
                                this.secondPaginator.pageIndex,
                                this.formSecond.get('apiaryId').value,
                                this.formSecond.get('beeFamilyId').value
                            ).subscribe(todos => this.resultsLengthCompleted = todos.length);
                        } else if (this.formSecond.get('apiaryId').value !== null) {
                            this.todoService.getAllFarmTodosByApiary(
                                this.farmService.farmValue.id,
                                true,
                                this.formSecond.get('filter').value,
                                this.secondSort.active,
                                this.secondSort.direction,
                                this.secondPaginator.pageIndex,
                                this.formSecond.get('apiaryId').value
                            ).subscribe(todos => this.resultsLengthCompleted = todos.length);
                        } else if (this.formSecond.get('beeFamilyId').value !== null) {
                            this.todoService.getAllFarmTodosByBeeFamily(
                                this.farmService.farmValue.id,
                                true,
                                this.formSecond.get('filter').value,
                                this.secondSort.active,
                                this.secondSort.direction,
                                this.secondPaginator.pageIndex,
                                this.formSecond.get('beeFamilyId').value
                            ).subscribe(todos => this.resultsLengthCompleted = todos.length);
                        } else {
                            this.todoService.getAllFarmTodosByFilterPaged(
                                this.farmService.farmValue.id,
                                true,
                                this.formSecond.get('filter').value,
                                this.secondSort.active,
                                this.secondSort.direction,
                                this.secondPaginator.pageIndex,
                            ).subscribe(todos => this.resultsLengthCompleted = todos.length);  
                        }
                        return data;
                        }),
                        catchError(() => {
                        this.loading = false;
                        return observableOf([]);
                        })
                    );
            });
        });
    }

    get todoItemPriority2LabelMapping() {
        return TodoItemPriority2LabelMapping;
    }

    resetPaging(): void {
        this.firstPaginator.pageIndex = 0;
    }

    resetPagingCompleted(): void {
        this.secondPaginator.pageIndex = 0;
    }

    deleteTodo(id: number): void {
        this.todoService.delete(id).subscribe(() => this.ngAfterViewInit());
    }

    doneTodo(id: number): void {
        this.todoService.getById(id).subscribe(todo => {
            todo.isComplete = true;
            this.todoService.update(id, todo)
            .pipe(first())
            .subscribe(() => {
                this.alertService.success('Užduotis atlikta', { keepAfterRouteChange: true, autoClose: true });
                this.ngAfterViewInit();
            })
            .add(() => this.loading = false);
        });
    }
}
