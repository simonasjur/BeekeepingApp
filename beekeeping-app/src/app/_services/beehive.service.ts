import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Beehive, BeehiveTypes } from '../_models'
import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class BeehiveService {
    private beehiveSubject: BehaviorSubject<Beehive>;
    public beehive: Observable<Beehive>;

    constructor(private http: HttpClient) { 
        this.beehiveSubject = new BehaviorSubject<Beehive>(null);
        this.beehive = this.beehiveSubject.asObservable();
    }

    clearBeehive() {
        this.beehiveSubject.next(null);
    }

    getFarmAllBeehives(farmId: number) {
        return this.http.get<Beehive[]>(`${baseUrl}/farms/${farmId}/beehives`).pipe (
            map(beehives => beehives.filter(beehive => beehive.type !== BeehiveTypes.NukleosoSekcija)));
    }

    /*getFarmEmptyBeehives(farmId: number) {
        return this.http.get<BeeFamily[]>(`${baseUrl}/farms/${farmId}/beefamilies`).pipe(
            map(beehives => beehives.filter(beehive => beehive.isEmpty === true)));
    }*/

    getById(id: number) {
        return this.http.get<Beehive>(`${baseUrl}/beehives/${id}`)
        .pipe(map(beehive => {
            this.beehiveSubject.next(beehive);
            return beehive;
        }));
    }

    create(params: any) {
        return this.http.post(`${baseUrl}/beehives`, params);
    }

    update(id: number, params: any) {
        return this.http.put(`${baseUrl}/beehives/${id}`, params);
    }

    delete(id: number) {
        return this.http.delete(`${baseUrl}/beehives/${id}`);
    }
}