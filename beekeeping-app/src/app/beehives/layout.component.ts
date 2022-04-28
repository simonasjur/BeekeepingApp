import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({ 
    templateUrl: 'layout.component.html'
})
export class LayoutComponent { 
    fromApiary: boolean = false;

    constructor(
        private router: Router) {
    }

    ngOnInit() {
        if (this.router.url.includes('apiaries')) {
            this.fromApiary = true;
        }
    }
}