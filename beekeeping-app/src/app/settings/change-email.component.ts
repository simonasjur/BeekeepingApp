import {Component, Inject} from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { first } from 'rxjs/operators';
import { User } from '../_models';
import { AlertService } from '../_services/alert.service';
import { UserService } from '../_services/user.service';

@Component({
    selector: 'change-email',
    templateUrl: 'change-email.component.html',
    styleUrls: ['change-email.component.css']
  })
  export class ChangeEmailComponent {
    form: FormGroup;
    public loading = false;
    public submitted = false;
  
    constructor(
      public dialogRef: MatDialogRef<ChangeEmailComponent>,
      private userService: UserService,
      private alertService: AlertService,
      private formBuilder: FormBuilder) {}
  
    ngOnInit() {
      this.form = this.formBuilder.group({
          password: ['', Validators.required],
          newEmail: ['', [Validators.required, Validators.email]]
      });
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    updateEmail(): void {
      this.submitted = true;
      console.log(this.submitted);
      console.log(this.f.newEmail.errors);
      // reset alerts on submit
      this.alertService.clear("alert-2");

      // stop here if form is invalid
      if (this.form.invalid) {
          return;
      }
      const {value, valid} = this.form;
      if(valid) {
        const user = { id: this.userService.userValue.id, 
                       password: this.f.password.value,
                       email: this.f.newEmail.value }
        console.log(value);
        this.userService.updateEmail(this.userService.userValue.id, user)
            .pipe(first())
            .subscribe({
              next: () => {
                  this.dialogRef.close(value);
                  this.userService.updateLocalStorageUser().subscribe();
                  this.alertService.success('El. paštas sėkmingai pakeistas', { autoClose: true });
              },
              error: error => {
                  console.log(error);
                  this.alertService.error(error, { id: 'alert-2' });
                  this.loading = false;
              }
          });
        }
    }
  
  }