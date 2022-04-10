import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { BeeFamily } from '../_models'
import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class BeeFamilyService {
    private familySubject: BehaviorSubject<BeeFamily>;
    public family: Observable<BeeFamily>;

    constructor(private http: HttpClient) {
        this.familySubject = new BehaviorSubject<BeeFamily>(null);
        this.family = this.familySubject.asObservable();
    }

    public get familyValue(): BeeFamily {
        return this.familySubject.value;
    }

    clearFamily() {
        this.familySubject.next(null);
    }

    getFarmAllBeeFamilies(farmId: number) {
        return this.http.get<BeeFamily[]>(`${baseUrl}/farms/${farmId}/beeFamilies`);
    }

    /*getFarmEmptyBeehives(farmId: number) {
        return this.http.get<BeeFamily[]>(`${baseUrl}/farms/${farmId}/beefamilies`).pipe(
            map(beehives => beehives.filter(beehive => beehive.isEmpty === true)));
    }*/

    getById(id: number) {
        return this.http.get<BeeFamily>(`${baseUrl}/beeFamilies/${id}`)
        .pipe(map(family => {
            this.familySubject.next(family);
            return family;
        }));
    }

    create(params: any) {
        return this.http.post(`${baseUrl}/beeFamilies`, params);
    }

    update(id: number, params: any) {
        return this.http.put(`${baseUrl}/beeFamilies/${id}`, params);
    }

    /*delete(id: string) {
        return this.http.delete(`${baseUrl}/beeFamilies/${id}`);
    }*/
}