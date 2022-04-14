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
import { MatChipsModule } from '@angular/material/chips';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';

import { LayoutComponent } from './layout.component';
import { BeefamilyQueensRoutingModule } from './beefamily-queens-routing.module';
import { ListComponent } from './list.component';
//import { AddEditComponent } from './add-edit.component';
//import { BeehiveComponentsListComponent } from './list.component';



@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        BeefamilyQueensRoutingModule,
        MatListModule,
        MatIconModule,
        MatFormFieldModule,
        MatSelectModule,
        MatInputModule,
        MatDatepickerModule,
        MatTableModule,
        MatButtonModule,
        MatProgressSpinnerModule,
        MatChipsModule,
        MatCheckboxModule,
        MatTabsModule,
        MatCardModule
    ],
    declarations: [
        LayoutComponent,
        ListComponent
//        AddEditComponent
    ]
})
export class BeefamilyQueensModule { }