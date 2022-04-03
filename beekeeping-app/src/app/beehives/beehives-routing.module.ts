import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LayoutComponent } from './layout.component';
import { ListComponent } from './list.component';
import { AddEditComponent } from './add-edit.component';
import { AddBeeFamilyComponent } from '../beefamilies/add.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', component: ListComponent },
            { path: 'add', component: AddEditComponent },
            { path: 'edit/:id', component: AddEditComponent},
            { path: ':id/beefamilies', component: AddBeeFamilyComponent}
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class BeehivesRoutingModule { }