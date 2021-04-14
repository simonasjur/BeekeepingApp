import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { LayoutComponent } from './layout.component';
import { LoginComponent } from './login.component';
import { RegisterComponent } from './register.component';
import { MatSidenavModule } from  '@angular/material/sidenav';
import { MatToolbarModule } from  '@angular/material/toolbar';
import { MatListModule } from  '@angular/material/list';
import { MatButtonModule } from  '@angular/material/button';
import { MatIconModule } from  '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        UserRoutingModule,
        MatSidenavModule,
        MatToolbarModule,
        MatListModule,
        MatButtonModule,
        MatIconModule,
        MatMenuModule,
        MatCardModule,
        MatInputModule
    ],
    declarations: [
        LayoutComponent,
        LoginComponent
    ]
})
export class UserModule { }