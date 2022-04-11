import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LayoutComponent } from './layout.component';
import { ListComponent } from './list.component';
import { AddBeeFamilyComponent } from './add.component';
import { HomeComponent } from './home.component';
import { AuthGuard } from '../_helpers';

const componentsModule = () => import('../beehive-components/beehive-components.module').then(x => x.BeehiveComponentsModule);
const queensModule = () => import('../queens/queens.module').then(x => x.QueensModule);

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', component: ListComponent },
            { path: ':id/home', component: HomeComponent },
            { path: 'add', component: AddBeeFamilyComponent },
            { path: 'edit/:id', component: AddBeeFamilyComponent},
            { path: ':id/components', loadChildren: componentsModule, canActivate: [AuthGuard] },
            { path: ':id/queens', loadChildren: queensModule, canActivate: [AuthGuard] }
            //{ path: 'edit/:id', component: AddEditComponent }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class BeeFamiliesRoutingModule { }