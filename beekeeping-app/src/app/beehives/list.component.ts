import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { BeehiveService } from '../_services/beehive.service';
import { Beehive, BeehiveTypes, User, Colors, ApiaryBeehive, BeehiveType2LabelMapping } from '../_models';
import { ApiaryBeehiveService } from '../_services/apiaryBeehive.service';
import { MatDialog } from '@angular/material/dialog';
import { AddApiaryBeehiveDialog } from './add-apiaryBeehive-dialog.component';
import { FarmService } from '../_services/farm.service';

@Component({
    selector: 'beehive-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    beehives: Beehive[];
    apiaryBeehives: ApiaryBeehive[];
    user: User;
    apiaryId: number;
    showEmptyBeehives: boolean;
    firstTableDisplayedColumns: string[] = ['no', 'type', 'action'];
    secondTableDisplayedColumns: string[] = ['no', 'type', 'date', 'action'];

    constructor(private beehiveService: BeehiveService,
                private apiaryBeehiveService: ApiaryBeehiveService,
                private farmService: FarmService,
                private router: Router,
                private route: ActivatedRoute,
                private dialog: MatDialog) {
        this.showEmptyBeehives = false;
    }

    ngOnInit() {
        this.apiaryId = this.route.snapshot.params['apiaryId'];
        this.beehiveService.getFarmEmptyBeehives(this.farmService.farmValue.id)
            .subscribe(beehives => this.beehives = beehives);
        this.apiaryBeehiveService.getOneApiaryBeehives(this.apiaryId)
            .subscribe(apiaryBeehives => this.apiaryBeehives = apiaryBeehives);
    }

    get BeehiveTypes() {
        return BeehiveTypes;
    }

    get Colors() {
        return Colors;
    }

    get beehiveType2LabelMapping() {
        return BeehiveType2LabelMapping;
    }

    changeShowEmptyBeehivesValue() {
        this.showEmptyBeehives = !this.showEmptyBeehives;
    }

    openAddApiaryBeehiveDialog(currentBeehiveId: any) {
        const dialogRef = this.dialog.open(AddApiaryBeehiveDialog, {
            data: {
                apiaryId: this.apiaryId,
                beehiveId: currentBeehiveId,
                beehive: this.beehives.find(b => b.id === currentBeehiveId)
            }
        });
    }
}