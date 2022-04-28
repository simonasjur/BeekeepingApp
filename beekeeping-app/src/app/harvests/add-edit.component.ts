import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { FarmService } from '../_services/farm.service';
import { AlertService } from '../_services/alert.service';
import { Apiary, BeeFamily, BeehiveBeefamily, Farm, Harvest, HarvestProduct2LabelMapping, HarvestProducts, TodoItem, TodoItemPriorities2, TodoItemPriority2LabelMapping } from '../_models';
import { TodoService } from '../_services/todo-item.service';
import { BeeFamilyService } from '../_services/beefamily.service';
import { ApiaryService } from '../_services/apiary.service';
import { HarvestService } from '../_services/harvest.service';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';

@Component({ templateUrl: 'add-edit.component.html',
styleUrls: ['add-edit.component.css']})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    id: string;
    isAddMode!: boolean;
    loading = false;
    submitted = false;
    harvest: Harvest;
    typesOfCategories: string[] = ['Bendra', 'Bitynas', 'Avilys'];
    beeFamilies: BeeFamily[];
    beehiveBeefamilies: BeehiveBeefamily[] = [];
    apiaries: Apiary[];
    selectedCategoryIndex: number = 0;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private farmService: FarmService,
        private alertService: AlertService,
        private apiaryService: ApiaryService,
        private harvestService: HarvestService,
        private beeFamilyService: BeeFamilyService,
        private beehiveBeeFamilyService: BeehiveBeefamilyService
    ) {}

    ngOnInit() {
        this.apiaryService.getFarmApiaries(this.farmService.farmValue.id).subscribe(apiaries => {
            this.apiaries = apiaries;
            this.beeFamilyService.getFarmAllBeeFamilies(this.farmService.farmValue.id).subscribe(beeFamilies => {
                this.beeFamilies = beeFamilies;
                beeFamilies.forEach(beefamily => this.beehiveBeeFamilyService.getBeefamilyBeehive(beefamily.id)
                    .subscribe(beehiveBeeFamily => {
                        this.beehiveBeefamilies = [...this.beehiveBeefamilies, ...beehiveBeeFamily];
                    }));
            });
        });

        this.id = this.route.snapshot.params['id'];
        
        this.isAddMode = !this.id;
        if (!this.isAddMode) {
            this.harvestService.getById(this.id).subscribe(harvest => {
                this.harvest = harvest;
                if (this.harvest.beeFamilyId != 0) {
                    this.selectedCategoryIndex = 2;
                }
                if (this.harvest.apiaryId != 0) {
                    this.selectedCategoryIndex = 1;
                }
            });
        }

        this.form = this.formBuilder.group({
            product: ['', Validators.required],
            startDate: [''],
            endDate: [''],
            quantity: ['', [Validators.required, Validators.pattern(/^[+]?\d+([.]\d+)?$/)]],
            farmId: [this.farmService.farmValue.id],
            apiaryId: [],
            beeFamilyId: [],
        });

        if (!this.isAddMode) {
            this.harvestService.getById(this.id)
                .pipe(first())
                .subscribe(x => this.form.patchValue(x));
        }
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

        if(this.form.get('startDate').value) {
            this.form.get('startDate').enable();
        } else {
            this.form.get('startDate').disable();
        }

        if(this.form.get('endDate').value) {
            this.form.get('endDate').enable();
        } else {
            this.form.get('endDate').disable();
        }

        if (this.isAddMode) {
            this.createHarvest(this.form.value);
        } else {
            this.updateHarvest(this.form.value);
        }
    }

    private createHarvest(harvest) {
        console.log(JSON.stringify(harvest))
        this.harvestService.create(harvest)
            .pipe(first())
            .subscribe(() => {
                this.alertService.success('Kopimas pridėtas', { keepAfterRouteChange: true, autoClose: true });
                this.router.navigate(['../'], { relativeTo: this.route });
            })
            .add(() => this.loading = false);
    }

    private updateHarvest(harvest) {
        this.harvestService.update(this.id, harvest)
            .pipe(first())
            .subscribe(() => {
                this.alertService.success('Kopimas atnaujintas', { keepAfterRouteChange: true, autoClose: true });
                this.router.navigate(['../../'], { relativeTo: this.route });
            })
            .add(() => this.loading = false);
    }
}
