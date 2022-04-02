import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Beehive } from '../_models';
import { BeehiveService } from '../_services/beehive.service';

@Component({
    selector: 'home-beehive',
    templateUrl: 'home.component.html',
    styleUrls: ['home.component.css']
})
export class HomeComponent implements OnInit {
    id: number;
    beehive: Beehive;
    
    constructor(private beehiveService: BeehiveService,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.id = this.route.snapshot.params['id'];
        this.beehiveService.getById(this.id).subscribe(beehive => this.beehive = beehive);
    }
}