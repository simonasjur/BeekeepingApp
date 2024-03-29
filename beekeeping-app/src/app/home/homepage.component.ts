import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { ActivatedRoute, Router } from "@angular/router";
import { interval, Observable, Subject, Subscription, timer } from "rxjs";
import { startWith, switchMap } from "rxjs/operators";
import { DeleteDialog } from "../_components/delete-dialog.component";
import { Harvest, Invitation, Worker } from "../_models";
import { AlertService } from "../_services/alert.service";
import { FarmService } from "../_services/farm.service";
import { HarvestService } from "../_services/harvest.service";
import { InvitationService } from "../_services/invitation.service";
import { UserService } from "../_services/user.service";
import { WorkerService } from "../_services/worker.service";
import { WorkerDialog } from "./worker-dialog.component";

@Component({ selector: 'home',
 templateUrl: 'homepage.component.html',
 styleUrls: ['homepage.component.css']})
 
export class HomepageComponent {
    farmId: number;
    invitation: Invitation = null;
    remainingSeconds: number;
    progressbarValue = 100;
    curSec: number = 0;
    timer$ = interval(100);
    subscription: Subscription = null;
    minutes: number;
    seconds2: number;
    secondsString: string;
    disabled: boolean = false;
    submitted = false;
    form: FormGroup;
    errors: boolean = false;
    loading: boolean = false;
    loading2: boolean = false;
    mainLoading: boolean = false;
    worker: Worker;
    userId: number;
    workers: Worker[];
    harvests: Harvest[];
    chartData = [];
    overall: number = 0;

    // chart options
    showXAxis = true;
    showYAxis = true;
    gradient = false;
    showLegend = false;
    showXAxisLabel = true;
    xAxisLabel = 'Data';
    showYAxisLabel = true;
    yAxisLabel = 'Kiekis (kg)';
    trimXAxisTicks = true;

    //view: any[] = [1000, 350];

    colorScheme = {
        domain: ['#FFD700', '#FFD700', '#FFD700', '#FFD700']
    };

    displayedColumns: string[] = ['firstName', 'lastName', 'action'];

    constructor(private formBuilder: FormBuilder,
                private farmService: FarmService,
                private invitationService: InvitationService,
                private workerService: WorkerService,
                private harvestService: HarvestService,
                private alertService: AlertService,
                private dialog: MatDialog,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.mainLoading = true;
        this.farmService.farm.subscribe(farm => {
            if (farm) {
                this.harvestService.getFarmThisYearHoneyHarvests(farm.id).subscribe(harvests => {
                    this.harvests = harvests.filter(h => h.product == 3);
                    for (let i = 0; i < this.harvests.length; i++) {
                        this.overall += this.harvests[i].quantity;
                        console.log(new Date(this.harvests[i].startDate))
                        const date = new Date(this.harvests[i].startDate).toISOString().substring(0,10);
                        const data = {
                            name: date,
                            value: this.harvests[i].quantity
                        };
                        this.chartData.push(data);
                    }
                    this.workerService.getFarmAndUserWorker(farm.id).subscribe(worker => {
                        this.worker = worker;
                        if (worker.role == 0) {
                            this.workerService.getFarmAllWorkers(farm.id).subscribe(workers => {
                                this.workers = workers;
                            });
                        };
                        this.mainLoading = false;
                    });
                });
            }
        });
        this.form = this.formBuilder.group({
            code: ['', Validators.required]
        });
    }

    ngAfterViewInit() {
        
    }

    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;

        this.errors = null;
        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        this.invitationService.validateCode(this.form.get('code').value).subscribe({
            next: result => {
                this.loading = false;
                this.router.navigate(['/farms'], { relativeTo: this.route });
            },
            error: err => {
                this.loading = false;
                this.errors = true;
            }
        });
    }

    getInviteCode() {
        this.loading2 = true;
        this.disabled = true;
        this.invitationService.getCode(this.farmService.farmValue.id).subscribe(invitation => {
            this.invitation = invitation;
            console.log(invitation)
            this.loading2 = false;
            let remainingMinutes = invitation.expirationDate.getMinutes() - new Date().getMinutes();
            if (remainingMinutes < 0) {
                remainingMinutes = invitation.expirationDate.getMinutes() - new Date().getMinutes() + 60;
            }
            this.remainingSeconds = remainingMinutes
            * 60 +  invitation.expirationDate.getSeconds() - new Date().getSeconds();
            console.log(new Date().getMinutes())
            this.progressbarValue = this.remainingSeconds / 600 * 100;
            
            this.startTimer(this.remainingSeconds * 10);
        });
    }

    startTimer(seconds: number) {
        if (this.subscription !== null) {
            this.subscription.unsubscribe();
        }
            
        this.subscription = this.timer$.subscribe((sec) => {
          this.progressbarValue = this.progressbarValue - 1/60;
          this.curSec = sec;
          this.remainingSeconds = this.remainingSeconds - 0.1;
          const first2Str = String(this.remainingSeconds % 60).slice(0, 2);
          this.seconds2 = Number(first2Str);
          this.secondsString = String(this.seconds2);
          if (this.secondsString.length === 1) {
            this.secondsString = '0'.concat(this.secondsString);
          }
          this.minutes = Math.floor(this.remainingSeconds / 60);
    
          if (this.curSec === seconds) {
            this.subscription.unsubscribe();
            this.invitation = null;
            this.disabled = false;
          }
        });
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    deleteWorker(id: number): void {
        const dialogRef = this.dialog.open(DeleteDialog);
    
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.workers = this.workers.filter(x => x.userId !== id);
                this.workerService.delete(this.farmService.farmValue.id, id).subscribe({
                    next: () => {
                        this.workers = this.workers.filter(x => x.userId !== id);
                        this.alertService.success('Darbininkas sėkmingai ištrintas', { keepAfterRouteChange: true, autoClose: true });
                    },
                    error: error => {
                        this.alertService.error(error);
                    }
                });
            }
        });
    }

    editWorker(id: number): void {
        const dialogRef = this.dialog.open(WorkerDialog, {
            data: id,
            width: "500px",
            height: "600px"
        });
    }
}