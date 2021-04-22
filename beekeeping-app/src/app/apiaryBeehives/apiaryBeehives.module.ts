import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DragDropModule } from '@angular/cdk/drag-drop';

import { ApiaryBeehiveComponent } from '../apiaryBeehives/apiaryBeehives.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        DragDropModule
    ],
    declarations: [
        ApiaryBeehiveComponent
    ]
})
export class ApiaryBeehivesModule { }