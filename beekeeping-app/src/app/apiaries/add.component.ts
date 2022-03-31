import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { ApiaryService } from '../_services/apiary.service';
import { FarmService } from '../_services/farm.service';

@Component({
    selector: 'add-apiary',
    templateUrl: 'add.component.html',
    styleUrls: ['add.component.css']
})
export class AddComponent implements OnInit {
    form: FormGroup;
    id: number;
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
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.form = this.formBuilder.group({
            name: ['', Validators.required],
            longitude: ['0'],
            latitude: ['0'],
            farmId: [this.farmService.farmValue.id]
        });

        navigator.geolocation.getCurrentPosition((position) => {
            this.center = {
              lat: position.coords.latitude,
              lng: position.coords.longitude,
            }
        })

        /*this.id = this.route.snapshot.params['id'];
        
        if (this.id !== undefined) {
            this.isAddMode = false;
            this.form.addControl('id', new FormControl('', Validators.required));
            this.beehiveService.getById(this.id).subscribe(beehive => this.form.patchValue(beehive));
        }*/
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;
        
        if (this.form.invalid) {
            console.log('invalidas');
            return;
        }

        this.loading = true;

        this.createApiary();
    }

    private createApiary() {

        // this.apiaryService.create(this.form.value).pipe(first())
        //     .subscribe(() => {
        //         this.router.navigate(['../'], { relativeTo: this.route });
        //     })
        //     .add(() => this.loading = false);
    }

    addMarker(location) {
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