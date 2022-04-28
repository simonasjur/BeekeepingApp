import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { BeeFamilyService } from '../_services/beefamily.service';
import { BeeFamily, User, ApiaryBeeFamily, Apiary, BeeFamilyState2LabelMapping, BeeFamilyOrigin2LabelMapping, Worker, Beehive, Queen, Breed2LabelMapping, BeehiveType2LabelMapping } from '../_models';
import { ApiaryBeeFamilyService } from '../_services/apiary-beefamily.service';
import { MatDialog } from '@angular/material/dialog';
import { AddApiaryBeehiveDialog } from './add-apiary-beehive-dialog.component';
import { FarmService } from '../_services/farm.service';
import { ApiaryService } from '../_services/apiary.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BeehiveService } from '../_services/beehive.service';
import { WorkerService } from '../_services/worker.service';
import { BeefamilyQueenService } from '../_services/beefamily-queen.service';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';
import { EditArriveDateDialog } from './edit-arrive-date-dialog.component';

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
    worker: Worker;
    apiarySelectForm: FormGroup;
    //showEmptyBeehives: boolean;
    displayedColumns: string[] = ['beehiveNo', 'beehiveType', 'queen', 'state', 'origin', 'arriveDate'];
    data = [];
    loading = true;


    constructor(private beehiveService: BeehiveService,
                private apiaryBeehiveService: ApiaryBeeFamilyService,
                private apiaryBeefamilyService: ApiaryBeeFamilyService,
                private apiaryService: ApiaryService,
                private beefamilyService: BeeFamilyService,
                private workerService: WorkerService,
                private farmService: FarmService,
                private beefamilyQueenService: BeefamilyQueenService,
                private beehiveBeefamilyService : BeehiveBeefamilyService,
                //private formBuilder: FormBuilder,
                //private router: Router,
                private route: ActivatedRoute,
                private dialog: MatDialog) {
        //this.showEmptyBeehives = false;
    }

    ngOnInit() {
        this.beefamilyService.clearFamily();
        this.apiaryBeehiveService.clearApiaryFamily();
        this.beehiveService.clearBeehive();
        this.apiaryBeefamilyService.clearApiaryFamily();

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

        //Gets all information about bee families and stores in data array
        this.workerService.getFarmAndUserWorker(this.farmService.farmValue.id).subscribe(worker => {
            this.worker = worker;
            this.apiaryBeehiveService.getOneApiaryBeeFamilies(this.apiaryId).subscribe(apiaryBeefamilies => {
                this.apiaryBeeFamilies = apiaryBeefamilies;
                if (this.apiaryBeeFamilies.length > 0) {
                    this.apiaryBeeFamilies.forEach(ab => {
                        this.beehiveBeefamilyService.getBeefamilyBeehive(ab.beeFamilyId).subscribe(beefamilyBeehive => {
                            if (beefamilyBeehive.length > 0) {
                                this.beefamilyQueenService.getLivingBeefamilyQueen(ab.beeFamilyId).subscribe(beefamilyQueen => {
                                    var queen: Queen;
                                    if (beefamilyQueen.length > 0) {
                                        queen = beefamilyQueen[0].queen;
                                    }
                                    const familyData = {
                                        apiaryBeefamily: ab,
                                        family: ab.beeFamily,
                                        beehive: beefamilyBeehive[0].beehive,
                                        queen: queen
                                    };
                                    this.data.push(familyData);
                                    if (this.data.length === this.apiaryBeeFamilies.length) {
                                        this.sortDataByBeehiveNo();
                                    }
                                });
                            }
                        });
                    });
                } else {
                    this.loading = false;
                }
            });
        });
    }

    sortDataByBeehiveNo() {
        this.data.sort((d1,d2) => d1.beehive.no - d2.beehive.no);
        this.loading = false;
    }

    /*get BeehiveTypes() {
        return BeehiveTypes;
    }

    get Colors() {
        return Colors;
    }*/

    get beehiveType2LabelMapping() {
        return BeehiveType2LabelMapping;
    }

    get beeFamilyState2LabelMapping() {
        return BeeFamilyState2LabelMapping;
    }

    get beeFamilyOrigin2LabelMapping() {
        return BeeFamilyOrigin2LabelMapping;
    }

    get breed2LabelMapping() {
        return Breed2LabelMapping;
    }

    openEditArriveDateDialog(apiaryBeefamily: ApiaryBeeFamily) {
        const dialogRef = this.dialog.open(EditArriveDateDialog, {
            data: {
                id: apiaryBeefamily.id,
                arriveDate: apiaryBeefamily.arriveDate
            }
        });
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