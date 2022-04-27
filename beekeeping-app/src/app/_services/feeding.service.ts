import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Feeding } from '../_models';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class FeedingService {
    constructor(private http: HttpClient) { }

    getBeefamilyFeedings(beefamilyId: number) {
        return this.http.get<Feeding[]>(`${baseUrl}/beefamilies/${beefamilyId}/feedings`);
    }

    getById(id: number) {
        return this.http.get<Feeding>(`${baseUrl}/feedings/${id}`);
    }

    create(params: any) {
        return this.http.post<Feeding>(`${baseUrl}/feedings`, params);
    }

    update(id: number, params: any) {
        return this.http.put(`${baseUrl}/feedings/${id}`, params);
    }

    delete(id: number) {
        return this.http.delete(`${baseUrl}/feedings/${id}`);
    }
}