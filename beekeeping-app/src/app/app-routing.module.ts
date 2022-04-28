import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { SettingsComponent } from './settings/settings.component';
import { AuthGuard } from './_helpers';

const userModule = () => import('./user/user.module').then(x => x.UserModule);
const homeModule = () => import('./home/home.module').then(x => x.HomeModule);

const routes: Routes = [
    { path: '', loadChildren: homeModule },
    { path: 'user', loadChildren: userModule },
    
    // otherwise redirect to home
    { path: '**', redirectTo: 'home' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }