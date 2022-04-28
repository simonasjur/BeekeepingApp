import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, NavigationEnd, Router, RoutesRecognized } from '@angular/router';
import { filter, first, pairwise } from 'rxjs/operators';

import { Beehive, BeehiveComponent, BeehiveComponentType, BeehiveComponentType2LabelMapping, BeehiveTypes } from '../_models';
import { AlertService } from '../_services/alert.service';
import { BeehiveComponentService } from '../_services/beehive-component.service';
import { BeehiveBeefamilyService } from '../_services/beehive-family.service';
import { BeehiveService } from '../_services/beehive.service';

@Component({
    selector: 'add-edit-beehive-component',
    templateUrl: 'add-edit.component.html',
    styleUrls: ['add-edit.component.css']
})
export class AddEditComponent implements OnInit {
    form: FormGroup;
    id: number;
    beehiveId: number;
    beehive: Beehive;
    today: Date;
    existingComponents: BeehiveComponent[];
    isAddMode = true;
    submitted = false;
    loading = false;
    formLoading = true;

    constructor(private beehiveComponentService: BeehiveComponentService,
                private beehiveService: BeehiveService,
                private beehiveBeefamilyService: BeehiveBeefamilyService,
                private alertService: AlertService,
                private formBuilder: FormBuilder,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.today = new Date();
        this.form = this.formBuilder.group({
            type: ['', Validators.required],
            position: ['', [Validators.required, Validators.pattern('^([1-9][0-9]*)$')]],
            installationDate: ['', Validators.required],
            beehiveId: ['', Validators.required]
        });

        this.id = this.route.snapshot.params['id'];
        
        if (this.id !== undefined) {
            this.isAddMode = false;
            this.form.addControl('id', new FormControl('', Validators.required));
            this.beehiveComponentService.getById(this.id).subscribe(beehiveComponent => this.form.patchValue(beehiveComponent));
        }

        const urlEntry = this.router.url.substring(1, 9);
        if (urlEntry !== 'apiaries')
        {
            this.router.navigate(['/'], { relativeTo: this.route });
        }
        this.beehiveBeefamilyService.getBeefamilyBeehive(this.extractBeefamilyId()).subscribe(beefamilyBeehive => {
            if (beefamilyBeehive.length === 0) {
                this.router.navigate(['/'], { relativeTo: this.route });
            }
            this.beehiveId = beefamilyBeehive[0].beehiveId;
            this.beehiveService.getById(this.beehiveId).subscribe(beehive => {
                this.beehive = beehive;
                this.beehiveComponentService.getBeehiveComponents(this.beehiveId)
                    .subscribe(components => {
                    this.existingComponents = components;
                    this.formLoading = false;
                });
            });
        });  
    }

    extractBeefamilyId() {
        const url = this.router.url.substring(this.router.url.indexOf('beefamilies')).substring(12);
        return +url.substring(0, url.indexOf('/'));
    }

    get beehiveComponentType() {
        return BeehiveComponentType;
    }

    get beehiveComponentTypeNames() {
        return Object.values(BeehiveComponentType).filter(value => typeof value === 'number');
    }

    get beehiveComponentType2LabelMapping() {
        return BeehiveComponentType2LabelMapping;
    }

    get f() { return this.form.controls; }

    isPositionAllowed(type: BeehiveComponentType) {
        if (type === this.beehiveComponentType.Aukstas ||
            type === this.beehiveComponentType.SkiriamojiTvorelė ||
            type === this.beehiveComponentType.Išleistuvas) {
                return true;
            }
        return false;
    }

    isDadano() {
        return this.beehive.type === BeehiveTypes.Dadano;
    }

    isQueenExcluderExist() {
        if (!this.isAddMode) {
            return null;
        }
        return this.existingComponents.find(e => e.type === BeehiveComponentType.SkiriamojiTvorelė);
    }

    isBottomGateExist() {
        if (!this.isAddMode) {
            return null;
        }
        return this.existingComponents.find(e => e.type === BeehiveComponentType.DugnoSklendė);
    }

    isBeeDecreaserExist() {
        if (!this.isAddMode) {
            return null;
        }
        return this.existingComponents.find(e => e.type === BeehiveComponentType.Išleistuvas);
    }

    existingSupersCount() {
        return this.existingComponents.filter(e => e.type === BeehiveComponentType.Aukstas).length;
    }

    isPositionCorrect() {
        const formPosition: number = this.form.controls['position'].value;
        if (formPosition) {
            const supersCount: number = this.existingSupersCount();
            if (this.form.controls['type'].value === BeehiveComponentType.Aukstas) {
                if (this.isAddMode) {
                    if (formPosition > supersCount + 1) {
                        return false;
                    }
                } else if (formPosition > supersCount) {
                    return false;
                }
            } else if (this.form.controls['type'].value === BeehiveComponentType.SkiriamojiTvorelė &&
                       formPosition >= supersCount) {
                return false
            } else if (this.form.controls['type'].value === BeehiveComponentType.Išleistuvas &&
                       formPosition >= supersCount) {
                return false;
            }
        }
        return true;
    }

    isHoneySuperCanBeAdded() {
        return this.honeySupersFreeSpace() >= 1;
    }

    isHoneyMiniSuperCanBeAdded() {
        return this.honeySupersFreeSpace() >= 0.5;
    }

    honeySupersFreeSpace() {
        const honeySupers = this.existingComponents.filter(e => e.type === BeehiveComponentType.Meduvė);
        const honeyMiniSupers = this.existingComponents.filter(e => e.type === BeehiveComponentType.Pusmeduvė);
        return this.beehive.maxHoneyCombsSupers - honeySupers.length - honeyMiniSupers.length * 0.5;
    }

    onSubmit() {
        if (!this.submitted) {
            if (this.isAddMode) {
                this.form.patchValue({beehiveId: this.beehiveId}); 
            }
            if (!this.isPositionAllowed(this.form.controls['type'].value)) {
                this.form.patchValue({position: '1'});
            }
        }
   
        this.submitted = true;
        
        if (this.form.invalid) {
            return;
        }
      
        this.loading = true;

        if (!this.isPositionAllowed(this.form.controls['type'].value)) {
            this.form.patchValue({position: ''});
        }
       
        if (this.isAddMode) {
            this.createBeehiveComponent();
        } else {
            this.updateBeehiveComponent();
        }
    }

    private createBeehiveComponent() {
        this.beehiveComponentService.create(this.form.value).subscribe({
            next: () => {
                this.alertService.success('Komponentas sėkmingai sukurtas', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: error => {
                if (this.isPositionCorrect()){
                    this.alertService.error("Serverio klaida");
                } else {
                    this.alertService.error("Bloga pozicijos reikšmė. Dabartinis avilio aukštų kiekis - " + 
                                            this.existingSupersCount());
                }
                this.loading = false;
            }
        });
    }

    private updateBeehiveComponent() {
        this.beehiveComponentService.update(this.id, this.form.value).subscribe({
            next: () => {
                this.alertService.success('Komponento informacija sėkmingai atnaujinta', { keepAfterRouteChange: true, autoClose: true });
                this.backToList();
            },
            error: error => {
                if (this.isPositionCorrect()){
                    this.alertService.error("Serverio klaida");
                } else {
                    this.alertService.error("Bloga pozicijos reikšmė. Dabartinis avilio aukštų kiekis - " + 
                                            this.existingSupersCount());
                }
                this.loading = false;
            }
        });
    }

    backToList() {
        if (this.isAddMode) {
            this.router.navigate(['../'], { relativeTo: this.route });
        } else {
            this.router.navigate(['../../'], { relativeTo: this.route });
        }
    }
}