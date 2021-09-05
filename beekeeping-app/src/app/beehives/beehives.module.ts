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

import { BeehivesRoutingModule } from './beehives-routing.module';
import { LayoutComponent } from './layout.component';
import { ListComponent } from './list.component';
import { ApiaryBeehiveComponent } from '../apiaryBeehives/apiaryBeehives.component';
import { AddComponent } from './add.component';
import { AddApiaryBeehiveDialog } from './add-apiaryBeehive-dialog.component';

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
        MatTableModule,
        MatButtonModule,
    ],
    declarations: [
        LayoutComponent,
        ListComponent,
        ApiaryBeehiveComponent,
        AddComponent,
        AddApiaryBeehiveDialog
    ]
})
export class BeehivesModule { }