import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Apiary } from '../_models';
import { AlertService } from '../_services/alert.service';
import { ApiaryService } from '../_services/apiary.service';
import { FarmService } from '../_services/farm.service';

@Component({
    selector: 'apiary-home',
    templateUrl: 'home.component.html',
    styleUrls: ['home.component.css']})
export class HomeComponent implements OnInit {
    id: number;
    apiary: Apiary;
    marker;
    options: google.maps.MapOptions = {
        disableDoubleClickZoom: true,
        maxZoom: 15,
        minZoom: 8,
    }
    circleCenter: google.maps.LatLngLiteral;
    center: google.maps.LatLngLiteral;
    radius = 3000;
    zoom = 12;

    constructor(private formBuilder: FormBuilder,
                private apiaryService: ApiaryService,
                private farmService: FarmService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.id = this.route.snapshot.params['id'];
        
        this.apiaryService.getById(this.id).subscribe(apiary => {
                this.apiary = apiary;
                const location = {
                    lat: apiary.latitude,
                    lng: apiary.longitude
                };
                this.circleCenter = location;
                this.center = location;
                this.marker = {
                    position: location,
                    options: { animation: google.maps.Animation.DROP }
                };
        });
    }
}