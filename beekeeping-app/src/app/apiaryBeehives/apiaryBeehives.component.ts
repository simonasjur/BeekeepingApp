import { Component, OnInit } from '@angular/core';

import { ApiaryBeehive } from '../_models';
import { ApiaryBeehiveService } from '../_services/apiaryBeehive.service';

@Component({
    selector: 'apiaryBeehives',
    templateUrl: './apiaryBeehives.component.html',
    styleUrls: ['./apiaryBeehives.component.css']
  })
export class ApiaryBeehiveComponent implements OnInit {
    apiaryBeehives: ApiaryBeehive[];

    constructor(private apiaryBeehiveService: ApiaryBeehiveService) {}

    ngOnInit() {
        this.apiaryBeehiveService.getOneApiaryBeehives(1)
            .subscribe(apiaryBeehives => this.apiaryBeehives = apiaryBeehives);
    }
}