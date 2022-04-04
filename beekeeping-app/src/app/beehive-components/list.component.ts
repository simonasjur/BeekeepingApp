import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { BeehiveComponentType } from '../_models';
import { BeehiveComponent, BeehiveComponentType2LabelMapping } from '../_models';
import { AlertService } from '../_services/alert.service';
import { BeehiveComponentService } from '../_services/beehive-component.service';

@Component({
    selector: 'beehive-components-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.css']
})
export class BeehiveComponentsListComponent implements OnInit {
    beehiveComponents: BeehiveComponent[];
    loading = true;

    displayedColumns = ['position', 'type', 'insertDate'];

    constructor(private beehiveComponentService: BeehiveComponentService,
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
    }
    

    get beehiveComponentType2LabelMapping() {
        return BeehiveComponentType2LabelMapping;
    }

    get beehiveComponentType() {
        return BeehiveComponentType;
    }

    /*deleteBeehive(id: number): void {
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