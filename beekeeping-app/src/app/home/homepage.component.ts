import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { interval, Observable, Subject, Subscription, timer } from "rxjs";
import { startWith, switchMap } from "rxjs/operators";
import { Invitation, Worker } from "../_models";
import { FarmService } from "../_services/farm.service";
import { InvitationService } from "../_services/invitation.service";
import { UserService } from "../_services/user.service";
import { WorkerService } from "../_services/worker.service";

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
    worker: Worker;
    userId: number;

    constructor(private formBuilder: FormBuilder,
                private farmService: FarmService,
                private invitationService: InvitationService,
                private workerService: WorkerService,
                private router: Router,
                private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.farmService.farm.subscribe(farm => {
            if (farm) {
                this.workerService.getFarmAndUserWorker(farm.id).subscribe(worker => {
                    this.worker = worker;
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
        this.invitationService.getCode(this.farmId).subscribe(invitation => {
            this.invitation = invitation;
            this.loading2 = false;
            this.remainingSeconds = (invitation.expirationDate.getMinutes() - new Date().getMinutes())
            * 60 +  invitation.expirationDate.getSeconds() - new Date().getSeconds();
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
}