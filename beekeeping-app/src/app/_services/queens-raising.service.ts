import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { QueensRaising } from '../_models'
import { map } from 'rxjs/operators';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class QueensRaisingService {
    constructor(private http: HttpClient) { }

    getFarmQueensRaisings(farmId: number) {
        return this.http.get<QueensRaising[]>(`${baseUrl}/farms/${farmId}/queensraisings`);
    }

    getById(id: number) {
        return this.http.get<QueensRaising>(`${baseUrl}/queensraisings/${id}`);
    }

    create(params: any) {
        return this.http.post<QueensRaising>(`${baseUrl}/queensraisings`, params);
    }

    update(id: number, params: any) {
        return this.http.put(`${baseUrl}/queensraisings/${id}`, params);
    }

    delete(id: number) {
        return this.http.delete(`${baseUrl}/queensraisings/${id}`);
    }
}