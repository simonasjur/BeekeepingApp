import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Beehive, BeehiveComponentType, BeehiveTypes } from '../_models';
import { BeehiveComponent, BeehiveComponentType2LabelMapping } from '../_models';
import { AlertService } from '../_services/alert.service';
import { BeehiveComponentService } from '../_services/beehive-component.service';
import { BeehiveService } from '../_services/beehive.service';

@Component({
    selector: 'beehive-components-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.css']
})
export class BeehiveComponentsListComponent implements OnInit {
    beehiveComponents: BeehiveComponent[];
    beehive: Beehive;
    loading = true;

    displayedColumns = ['position', 'type', 'insertDate', 'action'];

    constructor(private beehiveComponentService: BeehiveComponentService,
                private beehiveService: BeehiveService,
                private alertService: AlertService,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        //this.route.snapshot.params['beehiveId']
        this.beehiveComponentService.getBeehiveComponents(this.route.snapshot.params['beehiveId'])
            .subscribe({
                next: beehiveComponents => {
                    this.beehiveComponents = beehiveComponents;
                    this.loading = false;
                }});
        this.beehiveService.getById(this.route.snapshot.params['beehiveId'])
            .subscribe(beehive => this.beehive = beehive);
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
}