import { NgModule } from '@angular/core';

import { ApiariesRoutingModule } from './apiaries-routing.module';
import { LayoutComponent } from './layout.component';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list.component';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AddComponent } from './add.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
    imports: [
        CommonModule,
        ApiariesRoutingModule,
        MatTableModule,
        MatIconModule,
        MatTableModule,
        MatInputModule,
        MatFormFieldModule, 
        ReactiveFormsModule
    ],
    declarations: [
        LayoutComponent,
        ListComponent,
        AddComponent
    ]
})
export class ApiariesModule { }