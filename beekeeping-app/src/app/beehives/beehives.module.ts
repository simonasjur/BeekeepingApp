import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select'; 
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MatRippleModule, MAT_NATIVE_DATE_FORMATS } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';

import { BeehivesRoutingModule } from './beehives-routing.module';
import { LayoutComponent } from './layout.component';
import { ListComponent } from './list.component';
import { ApiaryBeehiveComponent } from '../apiaryBeehives/apiaryBeehives.component';
import { AddComponent } from './add.component';
import { AddApiaryBeehiveDialog } from './add-apiaryBeehive-dialog.component';
//import { AddEditComponent } from './add-edit.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        BeehivesRoutingModule,
        MatListModule,
        MatIconModule,
        MatFormFieldModule,
        MatSelectModule,
        MatInputModule,
        MatDatepickerModule,
        MatRippleModule,
        MatNativeDateModule,
        MatTableModule,
        MatButtonModule
    ],
    declarations: [
        LayoutComponent,
        ListComponent,
        ApiaryBeehiveComponent,
        AddComponent,
        AddApiaryBeehiveDialog
        //AddEditComponent
    ],
    providers: [
        DatePipe
    ]
})
export class BeehivesModule { }