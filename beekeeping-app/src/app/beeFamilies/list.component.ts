import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { BeeFamilyService } from '../_services/beefamily.service';
import { BeeFamily, User, ApiaryBeeFamily, Apiary, BeeFamilyState2LabelMapping, BeeFamilyOrigin2LabelMapping } from '../_models';
import { ApiaryBeeFamilyService } from '../_services/apiary-beefamily.service';
import { MatDialog } from '@angular/material/dialog';
import { AddApiaryBeehiveDialog } from './add-apiary-beehive-dialog.component';
import { FarmService } from '../_services/farm.service';
import { ApiaryService } from '../_services/apiary.service';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
    selector: 'beefamily-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    beeFamilies: BeeFamily[];
    apiaryBeeFamilies: ApiaryBeeFamily[];
    apiaries: Apiary[];
    user: User;
    apiaryId: number;
    currentApiary: Apiary;
    apiarySelectForm: FormGroup;
    //showEmptyBeehives: boolean;
    firstTableDisplayedColumns: string[] = ['id', 'state', 'origin', 'arriveDate', 'action'];

    constructor(//private beehiveService: BeeFamilyService,
                private apiaryBeehiveService: ApiaryBeeFamilyService,
                //private farmService: FarmService,
                private apiaryService: ApiaryService,
                //private formBuilder: FormBuilder,
                //private router: Router,
                private route: ActivatedRoute,
                private dialog: MatDialog) {
        //this.showEmptyBeehives = false;
    }

    ngOnInit() {
        this.apiaryId = this.route.snapshot.params['id'];
        /*this.apiarySelectForm = this.formBuilder.group({
            apiary: []
        });
        this.apiaryService.getFarmApiaries(this.farmService.farmValue.id)
            .subscribe(apiaries => { 
                this.apiaries = apiaries;
                this.currentApiary = apiaries.find(({id}) => id == this.apiaryId);
                console.log(this.apiaryId);
                console.log(this.currentApiary);
                this.apiarySelectForm.controls['apiary'].setValue(this.currentApiary);
                console.log(this.apiarySelectForm.get('apiary').value);
            });*/
        this.apiaryBeehiveService.getOneApiaryBeeFamilies(this.apiaryId)
            .subscribe(apiaryBeehives => this.apiaryBeeFamilies = apiaryBeehives);
        this.apiaryService.getById(this.apiaryId)
            .subscribe(apiary => this.currentApiary = apiary);
    }

    /*get BeehiveTypes() {
        return BeehiveTypes;
    }

    get Colors() {
        return Colors;
    }

    get beehiveType2LabelMapping() {
        return BeehiveType2LabelMapping;
    }*/

    get beeFamilyState2LabelMapping() {
        return BeeFamilyState2LabelMapping;
    }

    get beeFamilyOrigin2LabelMapping() {
        return BeeFamilyOrigin2LabelMapping;
    }

    /*changeShowEmptyBeehivesValue() {
        this.showEmptyBeehives = !this.showEmptyBeehives;
    }*/

    /*openAddApiaryBeehiveDialog(currentBeehiveId: any) {
        const dialogRef = this.dialog.open(AddApiaryBeehiveDialog, {
            data: {
                apiaryId: this.apiaryId,
                beehiveId: currentBeehiveId,
                beehive: this.beeFamilies.find(b => b.id === currentBeehiveId)
            }
        });
    }*/
}