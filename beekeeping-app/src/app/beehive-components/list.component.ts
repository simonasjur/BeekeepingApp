import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { DeleteDialog } from '../_components/delete-dialog.component';

import { ApiaryBeeFamily, BeeFamily, Beehive, BeehiveBeefamily, BeehiveComponentType, BeehiveTypes } from '../_models';
import { BeehiveComponent, BeehiveComponentType2LabelMapping } from '../_models';
import { AlertService } from '../_services/alert.service';
import { ApiaryBeeFamilyService } from '../_services/apiary-beefamily.service';
import { BeeFamilyService } from '../_services/beefamily.service';
import { BeehiveComponentService } from '../_services/beehive-component.service';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';
import { BeehiveService } from '../_services/beehive.service';

@Component({
    selector: 'beehive-components-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.css']
})
export class BeehiveComponentsListComponent implements OnInit {
    beehiveComponents: BeehiveComponent[];
    beehive: Beehive;
    beefamily: BeeFamily;
    beehiveBeefamily: BeehiveBeefamily;
    apiaryBeefamily: ApiaryBeeFamily;
    loading = true;

    displayedColumns = ['position', 'type', 'insertDate', 'action'];

    constructor(private beehiveComponentService: BeehiveComponentService,
                private beehiveService: BeehiveService,
                private beefamilyService: BeeFamilyService,
                private beehiveFamilyService: BeehiveBeefamilyService,
                private apiaryFamilyService: ApiaryBeeFamilyService,
                private alertService: AlertService,
                private route: ActivatedRoute,
                public dialog: MatDialog) {
    }

    ngOnInit() {
        const id = this.route.snapshot.params['id'];
        
        this.beefamilyService.getById(id).subscribe(beefamily => {
            this.beefamily = beefamily;
            this.beehiveFamilyService.getBeefamilyBeehive(beefamily.id).subscribe(beehiveBeefamilies => {
                this.beehiveBeefamily = beehiveBeefamilies[0];
                this.beehiveService.getById(this.beehiveBeefamily.beehiveId).subscribe(beehive => {
                    this.beehive = beehive;
                    this.apiaryFamilyService.getBeefamilyApiaries(this.beefamily.id).subscribe(apiaryFamily => {
                        this.apiaryBeefamily = apiaryFamily[0];
                        this.beehiveComponentService.getBeehiveComponents(this.beehive.id).subscribe({
                            next: beehiveComponents => {
                                this.beehiveComponents = beehiveComponents;
                                this.loading = false;
                        }});
                    });
                });
            });
        })
    }
    
    get beehiveComponentType2LabelMapping() {
        return BeehiveComponentType2LabelMapping;
    }

    get beehiveComponentType() {
        return BeehiveComponentType;
    }

    isDadano() {
        return this.beehive.type === BeehiveTypes.Dadano;
    }

    delete(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.beehiveComponentService.delete(id).subscribe({
                    next: () => {
                        this.beehiveComponents = this.beehiveComponents.filter(x => x.id !== id);
                        this.alertService.success('Komponentas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: error => {
                        this.alertService.error("Nepavyko ištrinti");
                    }
                });
            }
        });

        
    }
}