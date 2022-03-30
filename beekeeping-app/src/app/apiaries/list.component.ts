import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { BeeFamilyService } from '../_services/beeFamily.service';
import { User, Apiary } from '../_models';
import { ApiaryService } from '../_services/apiary.service';
import { FarmService } from '../_services/farm.service';

@Component({
    selector: 'apiaries-list',
    templateUrl: './list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    apiaries: Apiary[];
    displayedColumns: string[] = ['name', 'action'];

    constructor(private apiaryService: ApiaryService,
                private farmService: FarmService,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.apiaryService.getFarmApiaries(this.farmService.farmValue.id)
            .subscribe(apiaries => this.apiaries = apiaries);
    }
}