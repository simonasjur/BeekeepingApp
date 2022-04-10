import { Component } from '@angular/core';
import { Apiary } from '../_models';
import { ApiaryService } from '../_services/apiary.service';

@Component({ 
    templateUrl: 'layout.component.html'
})
export class LayoutComponent { 
    apiary: Apiary;

    constructor(private apiaryService: ApiaryService) {
    }

    ngOnInit() {
        this.apiaryService.apiary.subscribe(apiary => {
            this.apiary = apiary;
        })
    }
}