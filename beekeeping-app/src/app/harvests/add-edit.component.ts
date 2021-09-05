import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { FarmService } from '../_services/farm.service';
import { AlertService } from '../_services/alert.service';
import { Apiary, Beehive, Farm, TodoItem, TodoItemPriorities2, TodoItemPriority2LabelMapping } from '../_models';
import { TodoService } from '../_services/todoItem.service';
import { BeehiveService } from '../_services/beehive.service';
import { ApiaryService } from '../_services/apiary.service';

@Component({ templateUrl: 'add-edit.component.html'})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    id: string;
    isAddMode!: boolean;
    loading = false;
    submitted = false;
    todo: TodoItem;
    typesOfCategories: string[] = ['Bendra', 'Bitynas', 'Avilys'];
    beehives: Beehive[];
    apiaries: Apiary[];
    checked = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private farmService: FarmService,
        private alertService: AlertService,
        private todoService: TodoService,
        private beehiveService: BeehiveService,
        private apiaryService: ApiaryService
    ) {}

    ngOnInit() {
        this.beehiveService.getFarmAllBeehives(this.farmService.farmValue.id)
        .subscribe(beehives => this.beehives = beehives);
        this.apiaryService.getFarmApiaries(this.farmService.farmValue.id)
        .subscribe(apiaries => this.apiaries = apiaries);

        this.id = this.route.snapshot.params['id'];
        
        this.isAddMode = !this.id;
        if (!this.isAddMode) {
            this.todoService.getById(this.id).subscribe(todo => this.todo = todo);
        }

        this.form = this.formBuilder.group({
            title: ['', Validators.required],
            dueDate: ['', Validators.required],
            priority: ['', Validators.required],
            description: [''],
            isComplete: [false],
            apiaryId: [],
            beehiveId: [],
        });

        if (!this.isAddMode) {
            this.todoService.getById(this.id)
                .pipe(first())
                .subscribe(x => this.form.patchValue(x));
        }
    }

    get todoItemPriority2LabelMapping() {
        return TodoItemPriority2LabelMapping;
    }

    get priorities() {
        return TodoItemPriorities2;
    }

    get todoPriorities() {
        return Object.values(this.priorities).filter(value => typeof value === 'number');
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.form.invalid) {
            return;
        }

        this.loading = true;

        const todo = {
            "title": this.form.get('title').value,
            "priority": this.form.get('priority').value,
            "description": this.form.get('description').value,
            "dueDate": this.form.get('dueDate').value,
            "isComplete": this.form.get('isComplete').value,
            "farmId": this.farmService.farmValue.id,
            "apiaryId": this.form.get('apiaryId').value,
            "beehiveId": this.form.get('beehiveId').value
        }
        
        console.log(todo)
        if (this.isAddMode) {
            this.createTodo(todo);
        } else {
            this.updateTodo(todo);
        }
    }

    private createTodo(todo) {
        console.log(todo)
        this.todoService.create(todo)
            .pipe(first())
            .subscribe(() => {
                this.alertService.success('Užduotis pridėta', { keepAfterRouteChange: true, autoClose: true });
                this.router.navigate(['../'], { relativeTo: this.route });
            })
            .add(() => this.loading = false);
    }

    private updateTodo(todo) {
        this.todoService.update(this.id, todo)
            .pipe(first())
            .subscribe(() => {
                this.alertService.success('Užduotis atnaujinta', { keepAfterRouteChange: true, autoClose: true });
                this.router.navigate(['../../'], { relativeTo: this.route });
            })
            .add(() => this.loading = false);
    }
}
