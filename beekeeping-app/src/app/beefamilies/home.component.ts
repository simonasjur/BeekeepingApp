import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiaryBeeFamily, BeeFamily, BeeFamilyOrigin2LabelMapping, BeeFamilyState2LabelMapping, Beehive, BeehiveBeefamily, BeehiveType2LabelMapping, Color2LabelMapping } from '../_models';
import { ApiaryBeeFamilyService } from '../_services/apiary-beefamily.service';
import { BeeFamilyService } from '../_services/beefamily.service';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';
import { BeehiveService } from '../_services/beehive.service';

@Component({
    selector: 'beefamily-home',
    templateUrl: 'home.component.html',
    styleUrls: ['home.component.css']
})
export class HomeComponent implements OnInit {
    id: number;
    beefamily: BeeFamily;
    beehive: Beehive;
    beehiveBeefamily: BeehiveBeefamily;
    apiaryBeefamily: ApiaryBeeFamily;
    expandPercent: number;
    beefamilies = [];

    beefamilyTableColumns: string[] = ['apiary', 'origin', 'state', 'food'];

    constructor(private route: ActivatedRoute,
        private beefamilyService: BeeFamilyService,
        private beehiveService: BeehiveService,
        private beehiveFamilyService: BeehiveBeefamilyService,
        private apiaryFamilyService: ApiaryBeeFamilyService) {
    }

    ngOnInit() {
        this.id = this.route.snapshot.params['id'];

        this.beefamilyService.getById(this.id).subscribe(beefamily => {
            this.beefamily = beefamily;
            this.beehiveFamilyService.getAllBeefamiliesActiveBeehivebeefamilies(beefamily.id).subscribe(beehiveBeefamilies => {
                this.beehiveBeefamily = beehiveBeefamilies[0];
                this.apiaryFamilyService.getBeefamilyApiaries(beefamily.id).subscribe(apiaryFamilies => {
                    this.apiaryBeefamily = apiaryFamilies[0];
                    this.beehiveService.getById(this.beehiveBeefamily.id).subscribe(beehive => {
                        this.beehive = beehive;
                        console.log(JSON.stringify(this.beehive))
                        if (this.beehive.nestCombs != null) {
                            this.expandPercent = Math.round((this.beehive.nestCombs / this.beehive.maxNestCombs) * 100);
                        }
                        this.beefamilies = [
                            {
                                apiary: this.apiaryBeefamily.apiary,
                                origin: this.beefamily.origin,
                                state: this.beefamily.state,
                                food: this.beefamily.requiredFoodForWinter
                            }
                        ];
                    })
                })
            });
        })
    }

    get beeFamilyState2LabelMapping() {
        return BeeFamilyState2LabelMapping;
    }

    get beeFamilyOrigin2LabelMapping() {
        return BeeFamilyOrigin2LabelMapping;
    }

    get beehiveType2LabelMapping() {
        return BeehiveType2LabelMapping;
    }

    get color2LabelMapping() {
        return Color2LabelMapping;
    }
}