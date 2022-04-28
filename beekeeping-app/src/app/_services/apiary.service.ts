import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Apiary } from '../_models'
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class ApiaryService {
    private apiarySubject: BehaviorSubject<Apiary>;
    public apiary: Observable<Apiary>;

    constructor(private http: HttpClient) { 
        this.apiarySubject = new BehaviorSubject<Apiary>(null);
        this.apiary = this.apiarySubject.asObservable();
    }

    clearApiary() {
        this.apiarySubject.next(null);
    }

    getFarmApiaries(farmId: number) {
        return this.http.get<Apiary[]>(`${baseUrl}/farms/${farmId}/apiaries`);
    }

    create(params: any) {
        return this.http.post(`${baseUrl}/apiaries`, params);
    }

    getById(id: number) {
        return this.http.get<Apiary>(`${baseUrl}/apiaries/${id}`)
        .pipe(map(apiary => {
            this.apiarySubject.next(apiary);
            return apiary;
        }));
    }

    update(id, params) {
        params.id = id;
        return this.http.put(`${baseUrl}/apiaries/${id}`, params);
    }

    delete(id) {
        return this.http.delete<Apiary>(`${environment.apiUrl}/apiaries/${id}`);
    }
}