import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Apiary, ApiaryBeeFamily, BeeFamily, Beehive, BeehiveBeefamily } from '../_models';
import { ApiaryBeeFamilyService } from '../_services/apiary-beefamily.service';
import { ApiaryService } from '../_services/apiary.service';
import { BeeFamilyService } from '../_services/beefamily.service';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';
import { BeehiveService } from '../_services/beehive.service';

@Component({ 
    templateUrl: 'layout.component.html'
})
export class LayoutComponent {
    beefamily: BeeFamily;
    apiaryFamily: ApiaryBeeFamily;
    beehiveFamily: BeehiveBeefamily;
    beehive: Beehive;
    apiary: Apiary;

    constructor(private beefamilyService: BeeFamilyService,
        private apiaryFamilyService: ApiaryBeeFamilyService,
        private beehiveFamilyService: BeehiveBeefamilyService,
        private beehiveService: BeehiveService) {
    }

    ngOnInit() {
        this.beehiveService.clearBeehive();
        this.beefamilyService.family.subscribe(beefamily => {
            this.beefamily = beefamily;
            this.apiaryFamilyService.apiaryFamily.subscribe(apiaryFamily => {
                this.apiaryFamily = apiaryFamily;
                this.beehiveFamilyService.behiveFamily.subscribe(beehiveFamily => {
                    this.beehiveFamily = beehiveFamily;
                    this.beehiveService.beehive.subscribe(beehive => {
                        this.beehive = beehive;
                    })
                })
            })
        });
    }
}