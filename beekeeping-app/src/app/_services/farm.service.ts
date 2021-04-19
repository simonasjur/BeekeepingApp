import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { Farm, User } from '../_models';
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

    public get userId(): number {
        return this.userService.userValue.id;
    }

    clearFarm() {
        localStorage.removeItem('farm');
        this.farmSubject.next(null);
    }

    getAll() {
        if (this.userService.userValue.id)
            return this.http.get<Farm[]>(`${environment.apiUrl}/users/${this.userId}/farms`);
    }

    getFarms(itemsPerPage: number, pageNumber: number) {
        const top = itemsPerPage;
        const skip = pageNumber * itemsPerPage;
        return this.http.get<Farm[]>(`${environment.apiUrl}/users/${this.userId}/farms?$top=${top}&$skip=${skip}`);
    }

    create(farm: Farm) {
        return this.http.post(`${environment.apiUrl}/farms`, farm)
            .pipe(map(x => {
                this.userService.updateLocalStorageUser().subscribe();
                return x;
            }));
    }

    getById(id) {
        return this.http.get<Farm>(`${environment.apiUrl}/farms/${id}`);
    }

    update(id, params) {
        return this.http.put(`${environment.apiUrl}/farms/${id}`, params)
            .pipe(map(x => {
                // update stored farm if the logged in user updated current farm
                if (id == this.farmValue.id) {
                    // update local storage
                    const farm = { ...this.farmValue, ...params };
                    localStorage.setItem('farm', JSON.stringify(farm));

                    // publish updated farm to subscribers
                    this.farmSubject.next(farm);
                }
                return x;
            }));
    }

    delete(id) {
        return this.http.delete(`${environment.apiUrl}/farms/${id}`)
            .pipe(map(x => {
                this.userService.updateLocalStorageUser().subscribe();
                // auto logout if the logged in user deleted their own record
                /*if (id == this.farmValue.id) {
                    //this.logout();
                }*/
                return x;
            }));
    }

    updateLocalStorageFarm(id) {
        return this.http.get<Farm>(`${environment.apiUrl}/farms/${id}`)
        .pipe(map(farm => {
            localStorage.setItem('farm', JSON.stringify(farm));
            this.farmSubject.next(farm);
            return farm;
        }));
    }
}
