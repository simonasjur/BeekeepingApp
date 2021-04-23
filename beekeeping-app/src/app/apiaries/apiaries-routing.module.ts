import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../_helpers';

import { LayoutComponent } from './layout.component';
import { ListComponent } from './list.component';
import { AddComponent } from './add.component';

const beehivesModule = () => import('../beehives/beehives.module').then(x => x.BeehivesModule);

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', component: ListComponent },
            { path: ':apiaryId/beehives', loadChildren: beehivesModule, canActivate: [AuthGuard] },
            { path: 'add', component: AddComponent }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ApiariesRoutingModule { }