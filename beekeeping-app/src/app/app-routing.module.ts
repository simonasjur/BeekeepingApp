import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home';
import { AuthGuard } from './_helpers';

const userModule = () => import('./user/user.module').then(x => x.UserModule);
const farmsModule = () => import('./farms/farms.module').then(x => x.FarmsModule);

const routes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'farms', loadChildren: farmsModule, canActivate: [AuthGuard] },
    { path: 'user', loadChildren: userModule },

    
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }