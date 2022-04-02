import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Beehive, BeehiveTypes } from '../_models'
import { map } from 'rxjs/operators';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class BeehiveService {
    constructor(private http: HttpClient) { }

    getFarmAllBeehives(farmId: number) {
        return this.http.get<Beehive[]>(`${baseUrl}/farms/${farmId}/beehives`).pipe (
            map(beehives => beehives.filter(beehive => beehive.type !== BeehiveTypes.NukleosoSekcija)));
    }

    /*getFarmEmptyBeehives(farmId: number) {
        return this.http.get<BeeFamily[]>(`${baseUrl}/farms/${farmId}/beefamilies`).pipe(
            map(beehives => beehives.filter(beehive => beehive.isEmpty === true)));
    }*/

    getById(id: number) {
        return this.http.get<Beehive>(`${baseUrl}/beehives/${id}`);
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