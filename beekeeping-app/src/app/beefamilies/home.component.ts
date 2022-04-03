import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BeeFamilyService } from '../_services/beefamily.service';

@Component({
    selector: 'beefamily-home',
    templateUrl: 'home.component.html',
    styleUrls: ['home.component.css']
})
export class HomeComponent implements OnInit {
    id: number;

    constructor(private route: ActivatedRoute,
        private beefamilyService: BeeFamilyService) {
    }

    ngOnInit() {
        this.id = this.route.snapshot.params['id'];
    }
}