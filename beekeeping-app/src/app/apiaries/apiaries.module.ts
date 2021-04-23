import { NgModule } from '@angular/core';

import { ApiariesRoutingModule } from './apiaries-routing.module';
import { LayoutComponent } from './layout.component';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list.component';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
//import { ApiaryBeehiveComponent } from '../apiaryBeehives/apiaryBeehives.component';
//import { AddComponent } from './add.component';
//import { AddApiaryBeehiveDialog } from './add-apiaryBeehive-dialog.component';
//import { AddEditComponent } from './add-edit.component';

@NgModule({
    imports: [
        CommonModule,
        ApiariesRoutingModule,
        MatTableModule,
        MatIconModule
    ],
    declarations: [
        LayoutComponent,
        ListComponent
    ]
})
export class ApiariesModule { }