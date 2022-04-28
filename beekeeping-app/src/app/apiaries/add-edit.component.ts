import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Apiary } from '../_models';
import { AlertService } from '../_services/alert.service';
import { ApiaryService } from '../_services/apiary.service';
import { FarmService } from '../_services/farm.service';
import { Location } from '@angular/common'

@Component({
    selector: 'add-apiary',
    templateUrl: 'add-edit.component.html',
    styleUrls: ['add-edit.component.css']
})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    id: number;
    apiary: Apiary;
    changed: boolean = false;
    isAddMode = true;
    submitted = false;
    loading = false;
    center: google.maps.LatLngLiteral;
    marker: any = null;
    options: google.maps.MapOptions = {
        disableDoubleClickZoom: true,
        maxZoom: 15,
        minZoom: 8,
    }
    circleCenter: google.maps.LatLngLiteral;
    radius = 3000;
    zoom = 12;

    constructor(private formBuilder: FormBuilder,
                private apiaryService: ApiaryService,
                private farmService: FarmService,
                private alertService: AlertService,
                private router: Router,
                private route: ActivatedRoute,
                private location: Location) {
    }

    ngOnInit() {
        this.id = this.route.snapshot.params['id'];
        
        this.isAddMode = !this.id;
        if (!this.isAddMode) {
            this.apiaryService.getById(this.id).subscribe(apiary => {
                this.apiary = apiary;
                this.center = {
                    lat: apiary.latitude,
                    lng: apiary.longitude
                }
                this.addMarker(this.center);
                this.changed = false;
            });
        } else {
            navigator.geolocation.getCurrentPosition((position) => {
                this.center = {
                  lat: position.coords.latitude,
                  lng: position.coords.longitude,
                }
            })
        }

        this.form = this.formBuilder.group({
            name: ['', Validators.required],
            latitude: ['', Validators.required],
            longitude: ['', Validators.required]
        });

        if (!this.isAddMode) {
            this.apiaryService.getById(this.id)
                .pipe(first())
                .subscribe(x => {
                    this.form.patchValue(x);
                    this.apiaryService.clearApiary();
                });
        }
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;
        console.log(JSON.stringify(this.marker));

        if (this.changed){
            this.form.controls['latitude'].setValue(this.marker?.position?.toJSON().lat);
            this.form.controls['longitude'].setValue(this.marker?.position?.toJSON().lng);
        } else {
            this.form.controls['latitude'].setValue(this.marker?.position?.lat);
            this.form.controls['longitude'].setValue(this.marker?.position?.lng);
        }

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.form.invalid) {
            return;
        }

        this.loading = true;

        if (this.isAddMode) {
            const apiary = {
                ...this.form.value,
                "farmId": this.farmService.farmValue.id
            }
            this.createApiary(apiary);
        } else {
            this.updateApiary(this.form.value);
        }
    }

    private createApiary(apiary) {
        this.apiaryService.create(apiary)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Bitynas pridėtas', { keepAfterRouteChange: true, autoClose: true });
                    this.location.back();
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            })
    }

    private updateApiary(apiary) {
        this.apiaryService.update(this.id, apiary)
        .pipe(first())
        .subscribe({
            next: () => {
                this.location.back();
                this.alertService.success('Bitynas atnaujintas', { keepAfterRouteChange: true, autoClose: true });
            },
            error: error => {
                this.alertService.error(error);
                this.loading = false;
            }
        })
    }

    goBack() {
        this.location.back();
    }

    addMarker(location) {
        this.changed = true;
        this.marker = {
          position: location,
          options: { animation: google.maps.Animation.DROP }
        };
        this.circleCenter = location;
      }
    
      click(event: google.maps.MapMouseEvent) {
        console.log(event.latLng.toJSON().lat)
        this.addMarker(event.latLng);
      }
}