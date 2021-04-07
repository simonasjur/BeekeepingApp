import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { Farm } from '../_models';
import { UserService } from '../_services/user.service';

@Injectable({ providedIn: 'root' })
export class FarmService {
    private farmSubject: BehaviorSubject<Farm>;
    public farm: Observable<Farm>;

    constructor(
        private router: Router,
        private http: HttpClient,
        private userService: UserService
    ) {
        this.farmSubject = new BehaviorSubject<Farm>(JSON.parse(localStorage.getItem('farm')));
        this.farm = this.farmSubject.asObservable();
    }

    public get farmValue(): Farm {
        return this.farmSubject.value;
    }

    getAll(count: number) {
        return this.http.get<Farm[]>(`${environment.apiUrl}/users/${this.userService.userValue.id}/farms?$top=${count}`);
    }

    getDefaultFarm() {
        return this.http.get<Farm>(`${environment.apiUrl}/farms/${this.farmValue.id}`);
    }
}
