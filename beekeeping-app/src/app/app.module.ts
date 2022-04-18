import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { JwtInterceptor, ErrorInterceptor } from './_helpers';
import { HomeComponent } from './home/home.component';
import { AlertComponent } from './_components';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from  '@angular/material/sidenav';
import { MatToolbarModule } from  '@angular/material/toolbar';
import { MatListModule } from  '@angular/material/list';
import { MatButtonModule } from  '@angular/material/button';
import { MatIconModule } from  '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import {MatDividerModule} from '@angular/material/divider';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { HomeRoutingModule } from './home/home-routing.module';
import { AlertModule } from './_components/alert.module';
import { MatRippleModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatMomentDateModule, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import { MatDialogModule } from '@angular/material/dialog';
import { DeleteDialog } from './_components/delete-dialog.component';

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        FormsModule,
        HttpClientModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        MatSidenavModule,
        MatToolbarModule,
        MatListModule,
        MatButtonModule,
        MatIconModule,
        MatMenuModule,
        MatCardModule,
        MatInputModule,
        MatDividerModule,
        MatProgressSpinnerModule,
        HomeRoutingModule,
        AlertModule,
        MatRippleModule,
        MatMomentDateModule,
        MatDialogModule
    ],
    declarations: [
        AppComponent,
        DeleteDialog
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: {useUtc: true, strict: true}},
        { provide: MAT_DATE_LOCALE, useValue: 'lt-LT'}
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
