import { Component, OnInit } from '@angular/core';

import {  } from '../_models';
import { BeehiveComponent } from '../_models/beehive-component';
import { AlertService } from '../_services/alert.service';
import { BeehiveService } from '../_services/beehive.service';
import { FarmService } from '../_services/farm.service';

@Component({
    selector: 'beehive-components-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    beehiveComponents: BeehiveComponent[];
    loading = true;

    displayedColumns = ['no', 'type', 'action'];

    constructor(private beehiveService: BeehiveService,
                private farmService: FarmService,
                private alertService: AlertService) {
    }

    ngOnInit() {
        /*this.beehiveService.getFarmAllBeehives(this.farmService.farmValue.id)
            .subscribe({
                next: beehives => {
                    this.beehiveComponents = beehives.filter(b => b.isEmpty === false);
                    this.emptyBeehives = beehives.filter(b => b.isEmpty === true);
                    this.loading = false;
                }});*/
    }

    /*ngAfterViewInit() {
    }

    get beehiveType2LabelMapping() {
        return BeehiveType2LabelMapping;
    }

    deleteBeehive(id: number): void {
        this.beehiveService.delete(id).subscribe({
            next: () => {
                this.emptyBeehives = this.emptyBeehives.filter(x => x.id !== id);
                this.alertService.success('Avilys sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
            },
            error: error => {
                this.alertService.error(error);
            }
        });
    }*/
}