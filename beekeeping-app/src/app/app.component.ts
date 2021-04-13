import { Component } from '@angular/core';
import { first } from 'rxjs/operators';

import { UserService } from './_services/user.service';
import { FarmService } from './_services/farm.service';
import { User } from './_models';

@Component({ selector: 'app', templateUrl: 'app.component.html' })
export class AppComponent {
    title = 'Beekeeping app'
    user: User;
    farms = null;

    constructor(private userService: UserService,
                private farmService: FarmService) {
        this.userService.user.subscribe(user => this.user = user);
    }

    getUserAndFarms() {
        this.farmService.getAll(5)
            .pipe(first())
            .subscribe(farms => this.farms = farms);
    }

    logout() {
        this.userService.logout();
    }
}
