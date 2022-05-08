import { Component } from '@angular/core';

import { UserService } from '../_services/user.service';
import { FarmService } from '../_services/farm.service';
import { Farm, User } from '../_models';
import { ActivatedRoute, Router } from '@angular/router';
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
                private route: ActivatedRoute,
                public alertService: AlertService) {
    }

    ngOnInit() {
        this.userService.user.subscribe(user => {
                this.user = user;
                if (!this.user.defaultFarmId)
                {
                    this.router.navigate(['/farms']);
                }
        });
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
        this.farmService.updateLocalStorageFarm(id).subscribe(() => {
            this.currentFarm = this.farmService.farmValue;
            const url = this.route.snapshot['_routerState'].url;
            this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
                this.router.navigate([url]);
            });
        });
        
    }

    logout() {
        this.farmService.clearFarm();
        this.userService.logout();
    }
}