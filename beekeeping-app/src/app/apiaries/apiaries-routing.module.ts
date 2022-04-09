import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../_helpers';

import { LayoutComponent } from './layout.component';
import { ListComponent } from './list.component';
import { AddEditComponent } from './add-edit.component';
import { HomeComponent } from './home.component';

const beeFamiliesModule = () => import('../beefamilies/beefamilies.module').then(x => x.BeeFamiliesModule);

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', component: ListComponent },
            { path: 'add', component: AddEditComponent },
            { path: ':id', component: HomeComponent }, 
            { path: ':id/edit', component: AddEditComponent },
            { path: ':id/beefamilies', loadChildren: beeFamiliesModule, canActivate: [AuthGuard] }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ApiariesRoutingModule { }