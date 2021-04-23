import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Apiary } from '../_models'

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class ApiaryService {
    constructor(private http: HttpClient) { }

    getFarmApiaries(farmId: number) {
        return this.http.get<Apiary[]>(`${baseUrl}/farms/${farmId}/apiaries`);
    }

    create(params: any) {
        return this.http.post(`${baseUrl}/apiaries`, params);
    }
}