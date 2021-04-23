import { Component } from '@angular/core';

import { UserService } from '../_services/user.service';
import { FarmService } from '../_services/farm.service';
import { Farm, User } from '../_models';
import { Router } from '@angular/router';
import { AlertService } from '../_services/alert.service';

@Component({ selector: 'home',
 templateUrl: 'home.component.html',
 styleUrls: ['home.component.css']})
 
export class HomeComponent {
    user: User;
    farms = null;
    currentFarm: Farm;

    constructor(private userService: UserService,
                private farmService: FarmService,
                private router: Router,
                public alertService: AlertService) {
    }

    ngOnInit() {
        this.userService.user.subscribe(user => this.user = user);
        this.farmService.getAll().subscribe(() => {
            this.farmService.farms.subscribe(farms => this.farms = farms);
        });
        this.farmService.farm.subscribe(farm => this.currentFarm = farm);
        if (this.user.defaultFarmId != null && !localStorage.getItem('farm')) {
            this.farmService.updateLocalStorageFarm(this.user.defaultFarmId).subscribe();
        }
        console.log('configured routes: ', this.router.config);
    }

    loadFarm(id) {       
        this.farmService.updateLocalStorageFarm(id).subscribe();
        this.currentFarm = this.farmService.farmValue;
    }

    logout() {
        this.farmService.clearFarm();
        this.userService.logout();
    }
}