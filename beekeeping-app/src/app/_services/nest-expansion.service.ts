import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { NestExpansion } from '../_models';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class NestExpansionService {
    constructor(private http: HttpClient) { }

    getBeefamilyNestExpansions(beefamilyId: number) {
        return this.http.get<NestExpansion[]>(`${baseUrl}/beefamilies/${beefamilyId}/nestexpansions`);
    }

    getById(id: number) {
        return this.http.get<NestExpansion>(`${baseUrl}/nestexpansions/${id}`);
    }

    create(params: any) {
        return this.http.post<NestExpansion>(`${baseUrl}/nestexpansions`, params);
    }

    update(id: number, params: any) {
        return this.http.put(`${baseUrl}/nestexpansions/${id}`, params);
    }

    delete(id: number) {
        return this.http.delete(`${baseUrl}/nestexpansions/${id}`);
    }
}