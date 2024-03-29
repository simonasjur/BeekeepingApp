import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ApiaryBeeFamily, BeeFamily, BeeFamilyOrigin2LabelMapping, BeefamilyQueen, BeeFamilyState2LabelMapping, Beehive, BeehiveBeefamily, BeehiveComponent, BeehiveComponentType, BeehiveType2LabelMapping, Breed, Breed2LabelMapping, Color2LabelMapping, Queen, QueenState, Worker } from '../_models';
import { ApiaryBeeFamilyService } from '../_services/apiary-beefamily.service';
import { ApiaryService } from '../_services/apiary.service';
import { BeefamilyQueenService } from '../_services/beefamily-queen.service';
import { BeeFamilyService } from '../_services/beefamily.service';
import { BeehiveComponentService } from '../_services/beehive-component.service';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';
import { BeehiveService } from '../_services/beehive.service';
import { FarmService } from '../_services/farm.service';
import { QueenService } from '../_services/queen.service';
import { WorkerService } from '../_services/worker.service';
import { EditBeefamilyOriginDialog } from './edit-beefamily-origin-dialog.component';

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
    beefamilyQueen: BeefamilyQueen;
    queenBreed: Breed;
    expandPercent: number;
    beefamilies = [];
    isolatedFarmQueens: Queen[];
    beehiveComponents: BeehiveComponent[];
    loading = true;
    worker: Worker;

    beefamilyTableColumns: string[] = ['apiary', 'origin', 'state', 'queen'];

    constructor(private route: ActivatedRoute,
        private beefamilyService: BeeFamilyService,
        private beehiveService: BeehiveService,
        private beehiveFamilyService: BeehiveBeefamilyService,
        private apiaryFamilyService: ApiaryBeeFamilyService,
        private apiaryService: ApiaryService,
        private beefamilyQueenService: BeefamilyQueenService,
        private queenService: QueenService,
        private workerService: WorkerService,
        private farmService: FarmService,
        private beehiveComponentsService: BeehiveComponentService,
        private dialog: MatDialog) {
    }

    ngOnInit() {
        this.id = this.route.snapshot.params['id'];

        this.workerService.getFarmAndUserWorker(this.farmService.farmValue.id).subscribe(worker => {
            this.worker = worker;
            this.beefamilyService.getById(this.id).subscribe(beefamily => {
                this.beefamily = beefamily;
                this.beehiveFamilyService.getBeefamilyBeehive(beefamily.id).subscribe(beehiveBeefamilies => {
                    this.beehiveBeefamily = beehiveBeefamilies[0];
                    this.apiaryFamilyService.getBeefamilyApiary(beefamily.id).subscribe(apiaryFamilies => {
                        this.apiaryBeefamily = apiaryFamilies[0];
                        this.beefamilyQueenService.getLivingBeefamilyQueen(this.id).subscribe(beefamilyQueen => {
                            if (beefamilyQueen.length > 0)
                            {
                                this.beefamilyQueen = beefamilyQueen[0];
                                this.queenBreed = beefamilyQueen[0].queen.breed;
                            }
                            this.beehiveService.getById(this.beehiveBeefamily.beehiveId).subscribe(beehive => {
                                this.beehive = beehive;
                                if (this.beehive.nestCombs != null) {
                                    this.expandPercent = Math.round((this.beehive.nestCombs / this.beehive.maxNestCombs) * 100);
                                }
                                this.beefamilies = [
                                    {
                                        apiary: this.apiaryBeefamily.apiary,
                                        origin: this.beefamily.origin,
                                        state: this.beefamily.state,
                                        food: this.beefamily.requiredFoodForWinter,
                                        breed: this.queenBreed
                                    }
                                ];
                                this.beehiveComponentsService.getBeehiveComponents(beehive.id). subscribe(components => {
                                    this.beehiveComponents = components;
                                    this.queenService.getFarmQueens(this.farmService.farmValue.id).subscribe(queens => {
                                        this.isolatedFarmQueens = queens.filter(q => q.state === QueenState.Isolated);
                                        this.loading = false;
                                    });
                                })
                            })
                        })
                    })
                });
            });
        });
        
        this.apiaryService.clearApiary();
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

    get breed2LabelMapping() {
        return Breed2LabelMapping;
    }

    havingIsolatedQueens() {
        return !(!this.isolatedFarmQueens || this.isolatedFarmQueens.length === 0);
    }

    oneHullBeehiveHoneySupers() {
        return this.beehiveComponents.filter(bc => bc.type === BeehiveComponentType.Meduvė).length;
    }

    oneHullBeehiveHoneyMiniSupers() {
        return this.beehiveComponents.filter(bc => bc.type === BeehiveComponentType.Pusmeduvė).length;
    }

    multiHullBeehiveHoneySupers() {
        const queenExcluder = this.beehiveComponents.find(bc => bc.type === BeehiveComponentType.SkiriamojiTvorelė);
        if (queenExcluder) {
            return this.beehiveComponents.filter(bc => bc.type === BeehiveComponentType.Aukstas).length - queenExcluder.position;
        }
        return 0;
    }

    multiHullBeehiveNestSupers() {
        const queenExcluder = this.beehiveComponents.find(bc => bc.type === BeehiveComponentType.SkiriamojiTvorelė);
        if (queenExcluder) {
            return queenExcluder.position;
        }
        return this.beehiveComponents.filter(bc => bc.type === BeehiveComponentType.Aukstas).length;
    }

    isQueenExcluderExist() {
        return this.beehiveComponents.find(bc => bc.type === BeehiveComponentType.SkiriamojiTvorelė);
    }

    isBottomGateExist() {
        return this.beehiveComponents.find(bc => bc.type === BeehiveComponentType.DugnoSklendė);
    }

    isBeeDecreaserExist() {
        return this.beehiveComponents.find(bc => bc.type === BeehiveComponentType.Išleistuvas);
    }

    isFeederExist() {
        return this.beehiveComponents.find(bc => bc.type === BeehiveComponentType.Maitintuvė);
    }

    openEditBeefamilyOriginDialog() {
        const dialogRef = this.dialog.open(EditBeefamilyOriginDialog, {
            data: {
                id: this.beefamily.id,
                origin: this.beefamily.origin,
                state: this.beefamily.state
            }
        });
    }
}