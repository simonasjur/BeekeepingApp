import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { BeehiveService } from '../_services/beehive.service';
import { Beehive, BeehiveTypes, User, Colors, ApiaryBeehive } from '../_models';
import { ApiaryBeehiveService } from '../_services/apiaryBeehive.service';
import { MatDialog } from '@angular/material/dialog';
import { AddApiaryBeehiveDialog } from './add-apiaryBeehive-dialog.component';

@Component({
    selector: 'beehive-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    beehives: Beehive[];
    apiaryBeehives: ApiaryBeehive[];
    user: User;
    showEmptyBeehives: boolean;

    constructor(private beehiveService: BeehiveService,
                private apiaryBeehiveService: ApiaryBeehiveService,
                private router: Router,
                private route: ActivatedRoute,
                private dialog: MatDialog) {
        this.showEmptyBeehives = false;
    }

    ngOnInit() {
        this.user = JSON.parse(localStorage.getItem('user'));
        this.beehiveService.getFarmEmptyBeehives(this.user.defaultFarmId)
            .subscribe(beehives => this.beehives = beehives);
        this.apiaryBeehiveService.getOneApiaryBeehives(1)
            .subscribe(apiaryBeehives => this.apiaryBeehives = apiaryBeehives);
    }

    get BeehiveTypes() {
        return BeehiveTypes;
    }

    get Colors() {
        return Colors;
    }

    changeShowEmptyBeehivesValue() {
        this.showEmptyBeehives = !this.showEmptyBeehives;
    }

    openAddApiaryBeehiveDialog(currentBeehiveId: any) {
        const dialogRef = this.dialog.open(AddApiaryBeehiveDialog, {
            data: {
                apiaryId: 1,
                beehiveId: currentBeehiveId,
                beehive: this.beehives.find(b => b.id === currentBeehiveId)
            }
        });
    }
    /*deleteUser(id: string) {
        const user = this.users.find(x => x.id === id);
        if (!user) return;
        user.isDeleting = true;
        this.userService.delete(id)
            .pipe(first())
            .subscribe(() => this.users = this.users.filter(x => x.id !== id));
    }*/
}