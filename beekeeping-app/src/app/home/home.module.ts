import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AlertModule } from '../_components/alert.module';

import { HomeRoutingModule } from './home-routing.module';
import { SettingsComponent } from '../settings/settings.component';
import { HomeComponent } from './home.component';
import { ChangePasswordComponent } from '../settings/change-password.component';

import { MatSidenavModule } from  '@angular/material/sidenav';
import { MatToolbarModule } from  '@angular/material/toolbar';
import { MatListModule } from  '@angular/material/list';
import { MatButtonModule } from  '@angular/material/button';
import { MatIconModule } from  '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import {MatDividerModule} from '@angular/material/divider';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatTabsModule} from '@angular/material/tabs';
import {MatDialogModule} from '@angular/material/dialog';
import {OverlayModule} from '@angular/cdk/overlay';
import { AlertComponent } from '../_components';
import { ChangeEmailComponent } from '../settings/change-email.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        AlertModule,
        HomeRoutingModule,
        MatSidenavModule,
        MatToolbarModule,
        MatListModule,
        MatButtonModule,
        MatIconModule,
        MatMenuModule,
        MatCardModule,
        MatInputModule,
        MatDividerModule,
        MatProgressSpinnerModule,
        MatTabsModule,
        MatDialogModule,
        OverlayModule
    ],
    declarations: [
        SettingsComponent,
        HomeComponent,
        ChangePasswordComponent,
        ChangeEmailComponent
    ]
})
export class HomeModule { }