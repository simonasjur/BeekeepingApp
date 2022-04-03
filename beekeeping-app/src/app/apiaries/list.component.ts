import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { BeeFamilyService } from '../_services/beefamily.service';
import { User, Apiary } from '../_models';
import { ApiaryService } from '../_services/apiary.service';
import { FarmService } from '../_services/farm.service';
import { ApiaryBeeFamilyService } from '../_services/apiary-beefamily.service';
import { AlertService } from '../_services/alert.service';

@Component({
    selector: 'apiaries-list',
    templateUrl: './list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    apiaries: Apiary[];
    displayedColumns: string[] = ['name', 'familiesCount', 'action'];

    constructor(private apiaryService: ApiaryService,
                private farmService: FarmService,
                private apiaryFamiliesService: ApiaryBeeFamilyService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.apiaryService.getFarmApiaries(this.farmService.farmValue.id)
            .subscribe(apiaries => {
                this.apiaries = apiaries;
                apiaries.forEach(apiary => 
                    this.apiaryFamiliesService.getOneApiaryBeeFamilies(apiary.id).subscribe(families =>
                        apiary.familiesCount = families.length
                ));
            });
    }

    deleteApiary(id: number): void {
        this.apiaryService.delete(id).subscribe({
            next: () => {
                this.apiaries = this.apiaries.filter(x => x.id !== id);
                this.alertService.success('Bitynas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
            },
            error: error => {
                this.alertService.error(error);
            }
        });
    }
}