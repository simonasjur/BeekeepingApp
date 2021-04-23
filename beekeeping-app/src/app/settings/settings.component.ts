import { Component } from '@angular/core';

import { UserService } from '../_services/user.service';
import { FarmService } from '../_services/farm.service';
import { AlertService } from '../_services/alert.service';
import { Farm, User } from '../_models';
import { MatDialog } from '@angular/material/dialog';
import { ChangePasswordComponent } from './change-password.component';
import { first } from 'rxjs/operators';
import { ChangeEmailComponent } from './change-email.component';

@Component({ selector: 'settings',
    templateUrl: 'settings.component.html',
    styleUrls: ['settings.component.css']})
 
export class SettingsComponent {
    user: User;
    farm: Farm;
    defaultFarm: Farm;
    farms = null;
    loading: boolean;

    constructor(private userService: UserService,
                private farmService: FarmService,
                private alertService: AlertService,
                private dialog: MatDialog) {
        this.userService.user.subscribe(user => this.user = user);
        this.farmService.farm.subscribe(farm => this.farm = farm);
        this.farmService.getAll().subscribe(farms => this.farms = farms);
        this.farmService.defaultFarm.subscribe(defaultFarm => this.defaultFarm = defaultFarm);
    }

    changeDefaultFarm(id: number): void {
      let user = { defaultFarmId: id,
                    id: this.user.id };
      this.userService.update(this.user.id, user)
          .pipe(first())
          .subscribe(() => {
              this.alertService.success('Numatytasis ūkis pakeistas', { autoClose: true });
          })
          .add(() => this.loading = false);
    }

    openChangePasswordDialog(): void {
        const dialogRef = this.dialog.open(ChangePasswordComponent, {
          width: '600px',
          height: '500px',
          autoFocus: false,
          disableClose: true
        });
      }

      openChangeEmailDialog(): void {
        const dialogRef = this.dialog.open(ChangeEmailComponent, {
          width: '600px',
          height: '400px',
          autoFocus: false,
          disableClose: true
        });
      }

      openChangeDefaultFarmDialog(): void {
        
      }
}