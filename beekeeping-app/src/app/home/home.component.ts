import { Component } from '@angular/core';

import { User } from '../_models';
import { UserService } from '../_services';

@Component({ templateUrl: 'home.component.html' })
export class HomeComponent {
    user: User;

    constructor(private userService: UserService) {
        this.user = this.userService.userValue;
    }
}