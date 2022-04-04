import { NgModule } from '@angular/core';

import { ApiariesRoutingModule } from './apiaries-routing.module';
import { LayoutComponent } from './layout.component';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list.component';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AddEditComponent } from './add-edit.component';
import { ReactiveFormsModule } from '@angular/forms';
import { GoogleMapsModule } from '@angular/google-maps';
import { MatButtonModule } from '@angular/material/button';
import { HomeComponent } from './home.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
    imports: [
        CommonModule,
        ApiariesRoutingModule,
        MatTableModule,
        MatIconModule,
        MatTableModule,
        MatInputModule,
        MatFormFieldModule, 
        ReactiveFormsModule,
        GoogleMapsModule,
        MatButtonModule,
        MatProgressSpinnerModule
    ],
    declarations: [
        LayoutComponent,
        ListComponent,
        AddEditComponent,
        HomeComponent
    ]
})
export class ApiariesModule { }