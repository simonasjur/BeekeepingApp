import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { catchError, first, map, startWith, switchMap } from 'rxjs/operators';
import { Farm, TodoItem, TodoItemPriority2LabelMapping, User } from '../_models';

import { FarmService } from '../_services/farm.service';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alert.service';
import { TodoService } from '../_services/todoItem.service';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSort } from '@angular/material/sort';
import { Observable, merge, of as observableOf } from 'rxjs';

@Component({ templateUrl: 'list.component.html',
styleUrls: ['list.component.css'] })
export class ListComponent implements OnInit {
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

    public displayedColumns = ['priority', 'title', 'dueDate', 'category', 'action'];

    @ViewChild('firstSort') firstSort: MatSort;
    @ViewChild('secondSort') secondSort: MatSort;
    @ViewChild('firstPaginator') firstPaginator: MatPaginator;
    @ViewChild('secondPaginator') secondPaginator: MatPaginator;

    constructor(private todoService: TodoService,
                private farmService: FarmService,
                private userService: UserService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute) {
                    
                }

    ngOnInit() {
        /*this.todoService.getAllFarmUncompletedTodoItems(this.farmService.farmValue.id)
                        .pipe(first())
                        .subscribe(todos => this.dataSourceUncompleted.data = todos as TodoItem[]);
        this.todoService.getAllFarmCompletedTodoItems(this.farmService.farmValue.id)
                        .pipe(first())
                        .subscribe(todos => this.dataSourceCompleted.data = todos as TodoItem[]);
        this.userService.user.subscribe(user => this.user = user);*/
    }

    ngAfterViewInit() {
        console.log(this.resultsLength);
        this.filteredAndPagedTodos = merge(this.firstSort.sortChange, this.firstPaginator.page)
            .pipe(
                startWith({}),
                switchMap(() => {
                this.loading = true;
                return this.todoService.getAllFarmFilteredAnPaged(
                    this.farmService.farmValue.id, this.firstSort.active, this.firstSort.direction, this.firstPaginator.pageIndex, false);
                }),
                map(data => {
                // Flip flag to show that loading has finished.
                this.loading = false;
                this.todoService.getAllFarmUncompletedTodoItems(this.farmService.farmValue.id).subscribe(todos =>
                    this.resultsLength = todos.length);
                console.log(this.resultsLength)

                return data;
                }),
                catchError(() => {
                this.loading = false;
                return observableOf([]);
                })
            );


        this.filteredAndPagedTodosCompleted = merge(this.secondSort.sortChange, this.secondPaginator.page)
            .pipe(
                startWith({}),
                switchMap(() => {
                this.loading = true;
                return this.todoService.getAllFarmFilteredAnPaged(
                    this.farmService.farmValue.id, this.secondSort.active, this.secondSort.direction, this.secondPaginator.pageIndex, true);
                }),
                map(data => {
                // Flip flag to show that loading has finished.
                this.loading = false;
                this.todoService.getAllFarmCompletedTodoItems(this.farmService.farmValue.id).subscribe(todos =>
                    this.resultsLengthCompleted = todos.length);
                console.log(this.resultsLength)

                return data;
                }),
                catchError(() => {
                this.loading = false;
                return observableOf([]);
                })
            );
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
