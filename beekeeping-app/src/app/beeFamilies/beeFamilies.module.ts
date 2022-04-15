import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select'; 
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatMenuModule } from '@angular/material/menu'; 

import { BeeFamiliesRoutingModule } from './beefamilies-routing.module';
import { LayoutComponent } from './layout.component';
import { ListComponent } from './list.component';
import { ApiaryBeehiveComponent } from '../apiaryBeehives/apiaryBeehives.component';
import { AddBeeFamilyComponent } from './add.component';
import { AddApiaryBeehiveDialog } from './add-apiary-beehive-dialog.component';
import { HomeComponent } from './home.component';
import { MatCardModule } from '@angular/material/card';
import {MatChipsModule} from '@angular/material/chips';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        BeeFamiliesRoutingModule,
        MatListModule,
        MatIconModule,
        MatFormFieldModule,
        MatSelectModule,
        MatInputModule,
        MatDatepickerModule,
        MatTableModule,
        MatButtonModule,
        MatProgressSpinnerModule,
        MatCardModule,
        MatChipsModule,
        MatMenuModule
    ],
    declarations: [
        LayoutComponent,
        ListComponent,
        ApiaryBeehiveComponent,
        AddBeeFamilyComponent,
        HomeComponent
        //AddApiaryBeehiveDialog
    ]
})
export class BeeFamiliesModule { }