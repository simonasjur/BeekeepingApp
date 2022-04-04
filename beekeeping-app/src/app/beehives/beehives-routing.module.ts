import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../_helpers';

import { LayoutComponent } from './layout.component';
import { ListComponent } from './list.component';
import { AddEditComponent } from './add-edit.component';
import { AddBeeFamilyComponent } from '../beefamilies/add.component';

const beehiveComponentsModule = () => import('../beehive-components/beehive-components.module').then(x => x.BeehiveComponentsModule);

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', component: ListComponent },
            { path: 'add', component: AddEditComponent },
            { path: 'edit/:id', component: AddEditComponent},
            { path: ':id/beefamilies', component: AddBeeFamilyComponent},
            { path: ':beehiveId/beecomponents', loadChildren: beehiveComponentsModule, canActivate: [AuthGuard] }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class BeehivesRoutingModule { }