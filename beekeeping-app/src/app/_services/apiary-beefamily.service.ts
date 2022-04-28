import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { ApiaryBeeFamily } from '../_models'
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/operators';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class ApiaryBeeFamilyService {
    private apiaryFamilySubject: BehaviorSubject<ApiaryBeeFamily>;
    public apiaryFamily: Observable<ApiaryBeeFamily>;

    constructor(private http: HttpClient) {
        this.apiaryFamilySubject = new BehaviorSubject<ApiaryBeeFamily>(null);
        this.apiaryFamily = this.apiaryFamilySubject.asObservable();
    }

    clearApiaryFamily() {
        this.apiaryFamilySubject.next(null);
    }

    getOneApiaryBeeFamilies(apiaryId: number) {
        return this.http.get<ApiaryBeeFamily[]>(`${baseUrl}/apiaries/${apiaryId}/apiaryBeeFamilies`);
    }

    getBeefamilyApiaries(beefamilyId: number) {
        return this.http.get<ApiaryBeeFamily[]>(`${baseUrl}/beeFamilies/${beefamilyId}/apiaryBeeFamilies?$filter=departDate eq null`)
        .pipe(map(apiaryFamilies => {
            this.apiaryFamilySubject.next(apiaryFamilies[0]);
            return apiaryFamilies;
        }));
    }

    create(params: any) {
        return this.http.post(`${baseUrl}/apiaryBeeFamilies`, params);
    }

    update(id: number, params: any) {
        return this.http.put(`${baseUrl}/apiaryBeeFamilies/${id}`, params);
    }
}