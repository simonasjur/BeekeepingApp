import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home.component';
import { SettingsComponent } from '../settings/settings.component';
import { AuthGuard } from '../_helpers';

const farmsModule = () => import('../farms/farms.module').then(x => x.FarmsModule);
const apiariesModule = () => import('../apiaries/apiaries.module').then(x => x.ApiariesModule);
const todosModule = () => import('../todos/todo.module').then(x => x.TodoModule);
const harvestsModule = () => import('../harvests/harvests.module').then(x => x.HarvestsModule);
const beehivesModule = () => import('../beehives/beehives.module').then(x => x.BeehivesModule);

const routes: Routes = [
    {
        path: '', component: HomeComponent,
        children: [
            { path: 'farms', loadChildren: farmsModule, canActivate: [AuthGuard] },
            { path: 'settings', component: SettingsComponent, canActivate: [AuthGuard] },
            { path: 'apiaries', loadChildren: apiariesModule, canActivate: [AuthGuard] },
            { path: 'todos', loadChildren: todosModule, canActivate: [AuthGuard] },
            { path: 'harvests', loadChildren: harvestsModule, canActivate: [AuthGuard] },
            { path: 'beehives', loadChildren: beehivesModule, canActivate: [AuthGuard] }
        ]
    }
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HomeRoutingModule { }
