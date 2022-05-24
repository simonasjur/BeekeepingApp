import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
        private beehiveService: BeehiveService,
        private router: Router,
        private route: ActivatedRoute) {
    }

    ngOnInit() {
        //beefamily, apiaryfamily, beehivefamily, beehive
        this.beehiveService.clearBeehive();
        if (this.router.url.includes('beefamilies/')) {
            const beefamilyId = this.extractBeefamilyId();
            this.beefamilyService.getById(beefamilyId).subscribe(beefamily => {
                this.apiaryFamilyService.getBeefamilyApiary(beefamilyId).subscribe(() => {
                    this.beehiveFamilyService.getBeefamilyBeehive(beefamilyId).subscribe(beefamilyBeehive => {
                        this.beehiveService.getById(beefamilyBeehive[0].beehiveId).subscribe(() => {
                            this.beefamilyService.family.subscribe(beefamily => {
                                this.beefamily = beefamily;
                                this.apiaryFamilyService.apiaryFamily.subscribe(apiaryFamily => {
                                    this.apiaryFamily = apiaryFamily;
                                    this.beehiveFamilyService.behiveFamily.subscribe(beehiveFamily => {
                                        this.beehiveFamily = beehiveFamily;
                                        this.beehiveService.beehive.subscribe(beehive => {
                                            this.beehive = beehive;
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        } else {
            this.beefamilyService.family.subscribe(beefamily => {
                this.beefamily = beefamily;
                this.apiaryFamilyService.apiaryFamily.subscribe(apiaryFamily => {
                    this.apiaryFamily = apiaryFamily;
                    this.beehiveFamilyService.behiveFamily.subscribe(beehiveFamily => {
                        this.beehiveFamily = beehiveFamily;
                        this.beehiveService.beehive.subscribe(beehive => {
                            this.beehive = beehive;
                        });
                    });
                });
            });
        }
    }

    extractBeefamilyId() {
        const url = this.router.url.substring(this.router.url.indexOf('beefamilies')).substring(12);
        return +url.substring(0, url.indexOf('/'));
    }

    goToBeefamilies() {
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
            this.router.navigateByUrl('/apiaries/' + this.apiaryFamily.apiaryId + '/beefamilies');
        });
    }
}