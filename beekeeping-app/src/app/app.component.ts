import { Component } from '@angular/core';

import { UserService } from './_services';
import { User } from './_models';

@Component({ selector: 'app', templateUrl: 'app.component.html' })
export class AppComponent {
    title = 'Beekeeping app'
    user: User;

    constructor(private userService: UserService) {
        this.userService.user.subscribe(x => this.user = x);
    }

    logout() {
        this.userService.logout();
    }
}
