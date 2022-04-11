import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Harvest } from '../_models';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class HarvestService {

    constructor(private http: HttpClient) {
     }

    create(harvest: Harvest) {
        return this.http.post(`${environment.apiUrl}/harvests`, harvest);
    }

    getById(id) {
        return this.http.get<Harvest>(`${environment.apiUrl}/harvests/${id}`);
    }

    update(id, params) {
        params.id = id;
        return this.http.put(`${environment.apiUrl}/harvests/${id}`, params);
    }

    delete(id) {
        return this.http.delete<Harvest>(`${environment.apiUrl}/harvests/${id}`);
    }

    getFarmAllHarvests(farmId: number) {
        return this.http.get<Harvest[]>(`${environment.apiUrl}/farms/${farmId}/harvests`);
    }
}
