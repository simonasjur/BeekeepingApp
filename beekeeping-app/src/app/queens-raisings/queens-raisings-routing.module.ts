import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddEditComponent } from './add-edit.component';

import { LayoutComponent } from './layout.component';
import { ListComponent } from './list.component';
import { QueensRaisingComponent } from './queens-raising.component';

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', component: ListComponent },
            { path: 'add', component: AddEditComponent },
            { path: 'edit/:id', component: AddEditComponent},
            { path: ':id', component: QueensRaisingComponent}
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class QueensRaisingsRoutingModule { }