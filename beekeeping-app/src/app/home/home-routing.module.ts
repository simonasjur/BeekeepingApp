import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home.component';
import { SettingsComponent } from '../settings/settings.component';
import { AuthGuard } from '../_helpers';

const farmsModule = () => import('../farms/farms.module').then(x => x.FarmsModule);

const routes: Routes = [
    {
        path: '', component: HomeComponent,
        children: [
            { path: 'farms', loadChildren: farmsModule, canActivate: [AuthGuard] },
            { path: 'settings', component: SettingsComponent, canActivate: [AuthGuard] }
        ]
    }
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HomeRoutingModule { }