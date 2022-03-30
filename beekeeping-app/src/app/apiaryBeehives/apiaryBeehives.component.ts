import { Component, OnInit } from '@angular/core';

import { ApiaryBeeFamily } from '../_models';
import { ApiaryBeeFamilyService } from '../_services/apiaryBeeFamily.service';

@Component({
    selector: 'apiaryBeehives',
    templateUrl: './apiaryBeehives.component.html',
    styleUrls: ['./apiaryBeehives.component.css']
  })
export class ApiaryBeehiveComponent implements OnInit {
    apiaryBeehives: ApiaryBeeFamily[];

    constructor(private apiaryBeehiveService: ApiaryBeeFamilyService) {}

    ngOnInit() {
        this.apiaryBeehiveService.getOneApiaryBeeFamilies(1)
            .subscribe(apiaryBeehives => this.apiaryBeehives = apiaryBeehives);
    }
}