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
    private farmsSubject: BehaviorSubject<Farm[]>;
    public farms: Observable<Farm[]>;
    private defaultFarmSubject: BehaviorSubject<Farm>;
    public defaultFarm: Observable<Farm>;

    constructor(
        private router: Router,
        private http: HttpClient,
        private userService: UserService
    ) {
        this.farmSubject = new BehaviorSubject<Farm>(JSON.parse(localStorage.getItem('farm')));
        this.farm = this.farmSubject.asObservable();
        this.farmsSubject = new BehaviorSubject<Farm[]>(null);
        this.farms = this.farmsSubject.asObservable();
        this.defaultFarmSubject = new BehaviorSubject<Farm>(null);
        this.defaultFarm = this.defaultFarmSubject.asObservable();
    }

    public get farmsValue(): Farm[] {
        return this.farmsSubject.value;
    }

    public get farmValue(): Farm {
        return this.farmSubject.value;
    }

    public get userId(): number {
        return this.userService.userValue.id;
    }

    public get defaultFarmValue(): Farm {
        return this.defaultFarmSubject.value;
    }

    clearFarm() {
        localStorage.removeItem('farm');
        this.farmSubject.next(null);
    }

    getAll() {
        if (this.userService.userValue.id)
            return this.http.get<Farm[]>(`${environment.apiUrl}/users/${this.userId}/farms`)
                .pipe(map(x => {
                    this.farmsSubject.next(x);
                    return x;
                }));
    }

    getFarms(itemsPerPage: number, pageNumber: number) {
        const top = itemsPerPage;
        const skip = pageNumber * itemsPerPage;
        return this.http.get<Farm[]>(`${environment.apiUrl}/users/${this.userId}/farms?$top=${top}&$skip=${skip}`);
    }

    create(farm: Farm) {
        return this.http.post<Farm>(`${environment.apiUrl}/farms`, farm)
            .pipe(map(x => {
                this.userService.updateLocalStorageUser().subscribe();
                let farms = this.farmsValue;
                farms.push(x);
                this.farmsSubject.next(farms);
                return x;
            }));
    }

    getById(id) {
        return this.http.get<Farm>(`${environment.apiUrl}/farms/${id}`);
    }

    update(id, params: Farm) {
        params.id = id;
        return this.http.put(`${environment.apiUrl}/farms/${id}`, params)
            .pipe(map(x => {
                let farms = this.farmsSubject.value.filter(f => f.id !== id);
                farms.push(params);
                this.farmsSubject.next(farms);
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
        return this.http.delete<Farm>(`${environment.apiUrl}/farms/${id}`)
            .pipe(map(x => {
                this.userService.updateLocalStorageUser().subscribe(user => {
                    if (user.defaultFarmId !== null) {
                        this.getById(user.defaultFarmId).subscribe(farm => {
                            this.defaultFarmSubject.next(farm);
                        });
                    }
                });
                let farms = this.farmsSubject.value.filter(f => f.id !== x.id);
                this.farmsSubject.next(farms);


                // auto logout if the logged in user deleted their own record
                if (this.farmValue &&  id == this.farmValue.id) {
                    localStorage.removeItem('farm');
                    this.farmSubject.next(null);
                }
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
