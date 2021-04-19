import { Component } from '@angular/core';
import { first } from 'rxjs/operators';

import { UserService } from './_services/user.service';
import { FarmService } from './_services/farm.service';
import { User } from './_models';
import { Router } from '@angular/router';

@Component({ selector: 'app',
 templateUrl: 'app.component.html'})
 
export class AppComponent {
    title = 'Beekeeping app'
    user: User;
    farms = null;

    constructor(private userService: UserService,
                private farmService: FarmService) {
        this.userService.user.subscribe(user => this.user = user);
    }

    ngOnInit() {
        if (this.userService.userValue && this.userService.userValue.id)
        this.farmService.getAll().subscribe(farms => this.farms = farms);
    }

    loadFarms() {
        this.farmService.getAll().subscribe(farms => this.farms = farms);
    }

    logout() {
        this.farmService.clearFarm();
        this.userService.logout();
    }
}
