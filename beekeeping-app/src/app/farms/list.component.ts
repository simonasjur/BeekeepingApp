import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { first } from 'rxjs/operators';
import { User } from '../_models';

import { FarmService } from '../_services/farm.service';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alert.service';

@Component({ templateUrl: 'list.component.html',
styleUrls: ['list.component.css'] })
export class ListComponent implements OnInit {
    user: User;
    farms = null;
    itemsPerPage: number;
    pageNumber:number;
    totalItems:number;

    constructor(private farmService: FarmService,
                private userService: UserService,
                private alertService: AlertService) {
                    
                }

    ngOnInit() {
        this.farmService.getAll()
                        .pipe(first())
                        .subscribe(farms => this.farms = farms);
        this.userService.user.subscribe(user => this.user = user);
    }


    /*onPaginate(pageEvent: PageEvent) {
        this.itemsPerPage = +pageEvent.pageSize;
        this.pageNumber = +pageEvent.pageIndex + 1;
        this.farmService.getAll()
            .pipe(first())
            .subscribe(farms => this.farms = farms);
        this.totalItems = this.farms.length;
        this.farmService.getFarms(this.itemsPerPage, this.pageNumber);
        console.log("page number " + this.pageNumber);
        console.log("items per page " + this.itemsPerPage);
    }*/

    deleteFarm(id: number) {
        const farm = this.farms.find(x => x.id === id);
        farm.isDeleting = true;
        this.farmService.delete(id)
            .pipe(first())
            .subscribe(() => { this.farms = this.farms.filter(x => x.id !== id);
                this.alertService.success('Ūkis ' + farm.name + ' ištrintas', { keepAfterRouteChange: true, autoClose: true });
            })
    }
}
