import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LayoutComponent } from './layout.component';
import { ListComponent } from './list.component';
import { AddBeeFamilyComponent } from './add.component';
import { HomeComponent } from './home.component';
import { AuthGuard } from '../_helpers';

const componentsModule = () => import('../beehive-components/beehive-components.module').then(x => x.BeehiveComponentsModule);
const queensModule = () => import('../queens/queens.module').then(x => x.QueensModule);
const beefamilyQueensModule = () => import('../beefamily-queens/beefamily-queens.module').then(x => x.BeefamilyQueensModule);
const nestExpansionsModule = () => import('../nest-expansions/nest-expansions.module').then(x => x.NestExpansionsModule);
const nestReductionsModule = () => import('../nest-reductions/nest-reductions.module').then(x => x.NestReductionsModule);
const beehivesModule = () => import('../beehives/beehives.module').then(x => x.BeehivesModule);
const feedingsModule = () => import('../feedings/feedings.module').then(x => x.FeedingsModule);

const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: '', component: ListComponent },
            { path: ':id/home', component: HomeComponent },
            { path: 'add', component: AddBeeFamilyComponent },
            { path: 'edit/:id', component: AddBeeFamilyComponent},
            { path: ':id/components', loadChildren: componentsModule, canActivate: [AuthGuard] },
            { path: ':id/queens', loadChildren: queensModule, canActivate: [AuthGuard] },
            { path: ':id/beefamilyqueens', loadChildren: beefamilyQueensModule, canActivate: [AuthGuard] },
            { path: ':id/nestexpansions', loadChildren: nestExpansionsModule, canActivate: [AuthGuard] },
            { path: ':id/beehives', loadChildren: beehivesModule, canActivate: [AuthGuard] },
            { path: ':id/nestreductions', loadChildren: nestReductionsModule, canActivate: [AuthGuard] },
            { path: ':id/feedings', loadChildren: feedingsModule, canActivate: [AuthGuard] }
            //{ path: 'edit/:id', component: AddEditComponent }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class BeeFamiliesRoutingModule { }