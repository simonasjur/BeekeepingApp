import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { NestReduction } from '../_models';


const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class NestReductionService {
    constructor(private http: HttpClient) { }

    getBeefamilyNestReductions(beefamilyId: number) {
        return this.http.get<NestReduction[]>(`${baseUrl}/beefamilies/${beefamilyId}/nestreductions`);
    }

    getById(id: number) {
        return this.http.get<NestReduction>(`${baseUrl}/nestreductions/${id}`);
    }

    create(params: any) {
        return this.http.post<NestReduction>(`${baseUrl}/nestreductions`, params);
    }

    update(id: number, params: any) {
        return this.http.put(`${baseUrl}/nestreductions/${id}`, params);
    }

    delete(id: number) {
        return this.http.delete(`${baseUrl}/nestreductions/${id}`);
    }
}