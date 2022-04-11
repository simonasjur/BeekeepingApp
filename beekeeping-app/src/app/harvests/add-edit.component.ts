import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { FarmService } from '../_services/farm.service';
import { AlertService } from '../_services/alert.service';
import { Apiary, BeeFamily, Farm, Harvest, TodoItem, TodoItemPriorities2, TodoItemPriority2LabelMapping } from '../_models';
import { TodoService } from '../_services/todo-item.service';
import { BeeFamilyService } from '../_services/beefamily.service';
import { ApiaryService } from '../_services/apiary.service';
import { HarvestService } from '../_services/harvest.service';

@Component({ templateUrl: 'add-edit.component.html'})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    id: string;
    isAddMode!: boolean;
    loading = false;
    submitted = false;
    harvest: Harvest;
    typesOfCategories: string[] = ['Bendra', 'Bitynas', 'Avilys'];
    beeFamilies: BeeFamily[];
    apiaries: Apiary[];

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private farmService: FarmService,
        private alertService: AlertService,
        private beehiveService: BeeFamilyService,
        private apiaryService: ApiaryService,
        private harvestService: HarvestService
    ) {}

    ngOnInit() {
        this.id = this.route.snapshot.params['id'];
        
        this.isAddMode = !this.id;
        if (!this.isAddMode) {
            this.harvestService.getById(this.id).subscribe(harvest => {
                this.harvest = harvest;
                if (this.harvest.beeFamilyId != 0) {
                    //this.selectedCategoryIndex = 2;
                }
                if (this.harvest.apiaryId != 0) {
                    //this.selectedCategoryIndex = 1;
                }
            });
        }

        this.form = this.formBuilder.group({
            title: ['', Validators.required],
            dueDate: ['', Validators.required],
            priority: ['', Validators.required],
            description: [''],
            isComplete: [false],
            apiaryId: [],
            beeFamilyId: [],
        });

        if (!this.isAddMode) {
            this.harvestService.getById(this.id)
                .pipe(first())
                .subscribe(x => this.form.patchValue(x));
        }
    }

    onSubmit() {
    }
}
