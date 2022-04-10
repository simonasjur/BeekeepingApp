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
import {MatChipsModule} from '@angular/material/chips';

import { LayoutComponent } from './layout.component';
import { BeehiveComponentsRoutingModule } from './beehive-components-routing.module';
import { BeehiveComponentsListComponent } from './list.component';
import { AddEditComponent } from './add-edit.component';


@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        BeehiveComponentsRoutingModule,
        MatListModule,
        MatIconModule,
        MatFormFieldModule,
        MatSelectModule,
        MatInputModule,
        MatDatepickerModule,
        MatTableModule,
        MatButtonModule,
        MatProgressSpinnerModule,
        MatChipsModule
    ],
    declarations: [
        LayoutComponent,
        BeehiveComponentsListComponent,
        AddEditComponent
    ]
})
export class BeehiveComponentsModule { }