import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { BeefamilyQueen, Breed2LabelMapping, Color2LabelMapping, Queen, QueenState2LabelMapping } from '../_models';
import { AlertService } from '../_services/alert.service';
import { BeefamilyQueenService } from '../_services/beefamily-queen.service';

@Component({
    selector: 'beefamily-queens-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.css']
})
export class ListComponent implements OnInit {
    beefamilyQueens: BeefamilyQueen[];
    familyQueen: BeefamilyQueen;
    beefamilyId: number;
    loading = true;
    apiaryId: number;

    displayedColumns = ['breed', 'hatchingDate', 'markingColor', 'insertDate', 'takeOutDate', 'state', 'action'];

    constructor(private beefamilyQueenService: BeefamilyQueenService,
                private alertService: AlertService,
                private router: Router) {
    }

    ngOnInit() {
        this.extractIds();
        this.beefamilyQueenService.getBeefamilyQueens(this.beefamilyId)
            .subscribe({
                next: beefamilyQueens => {
                    this.beefamilyQueens = beefamilyQueens;
                    this.familyQueen = beefamilyQueens.find(bq => !bq.takeOutDate);
                    this.loading = false;
                },
                error: () => {
                    this.backToHomeWithError();
                }
            });
    }

    extractIds() {
        const urlEntry = this.router.url.substring(10);
        this.apiaryId = +urlEntry.substring(0, urlEntry.indexOf('/'));
        const url = this.router.url.substring(this.router.url.indexOf('beefamilies')).substring(12);
        this.beefamilyId = +url.substring(0, url.indexOf('/'));
    }

    private backToHomeWithError() {
        this.alertService.error('Operacija negalima');
        this.router.navigate(['/']);
    }

    get breed2LabelMapping() {
        return Breed2LabelMapping;
    }

    get queenState2LabelMapping() {
        return QueenState2LabelMapping;
    }

    get color2LabelMapping() {
        return Color2LabelMapping;
    }

    /*get beehiveType2LabelMapping() {
        return BeehiveType2LabelMapping;
    }*/

    /*deleteBeehive(id: number): void {
        this.beehiveService.delete(id).subscribe({
            next: () => {
                this.emptyBeehives = this.emptyBeehives.filter(x => x.id !== id);
                this.alertService.success('Avilys sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
            },
            error: error => {
                this.alertService.error(error);
            }
        });
    }*/
}
//Isemimas: nurodoma nauja busena motinos, data. Queen, beefamilyQueen
//Idejimas turimos: nurodomas avilio numeris, pasirenkama motina is saraso, data