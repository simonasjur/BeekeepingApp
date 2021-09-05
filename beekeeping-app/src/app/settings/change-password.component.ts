import {Component, Inject} from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { first } from 'rxjs/operators';
import { MustMatch } from '../_helpers';
import { User } from '../_models';
import { AlertService } from '../_services/alert.service';
import { UserService } from '../_services/user.service';

@Component({
    selector: 'change-password',
    templateUrl: 'change-password.component.html',
    styleUrls: ['change-password.component.css']
  })
  export class ChangePasswordComponent {
    form: FormGroup;
    public loading = false;
    public submitted = false;
  
    constructor(
      public dialogRef: MatDialogRef<ChangePasswordComponent>,
      private userService: UserService,
      private alertService: AlertService,
      private formBuilder: FormBuilder) {}
  
    ngOnInit() {
      const formOptions: AbstractControlOptions = { validators: MustMatch('password', 'confirmPassword') };
      this.form = this.formBuilder.group({
          currentPassword: ['', Validators.required],
          password: ['', [Validators.required, Validators.minLength(6)]],
          confirmPassword: ['', Validators.required]
      }, formOptions);
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    updatePassword(): void {
      this.submitted = true;

      // reset alerts on submit
      this.alertService.clear("alert-1");

      // stop here if form is invalid
      if (this.form.invalid) {
          return;
      }
      const {value, valid} = this.form;
      if(valid) {
        let user = { id: this.userService.userValue.id, password: this.f.currentPassword.value,
        newPassword: this.f.password.value }
        console.log(value);
        this.userService.updatePassword(this.userService.userValue.id, user)
            .pipe(first())
            .subscribe({
              next: () => {
                  this.dialogRef.close(value);
                  this.alertService.success('Slaptažodis sėkmingai atnaujintas', { autoClose: true });
              },
              error: error => {
                  console.log(error);
                  this.alertService.error(error, { id: 'alert-1' });
                  this.loading = false;
              }
          });
        }
    }
  
  }