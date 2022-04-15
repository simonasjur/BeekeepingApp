import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Queen, QueenState } from '../_models'
import { map } from 'rxjs/operators';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class QueenService {
    constructor(private http: HttpClient) { }

    getFarmQueens(farmId: number) {
        return this.http.get<Queen[]>(`${baseUrl}/farms/${farmId}/queens`);
    }

    getById(id: number) {
        return this.http.get<Queen>(`${baseUrl}/queens/${id}`);
    }

    create(params: any) {
        return this.http.post<Queen>(`${baseUrl}/queens`, params);
    }

    update(id: number, params: any) {
        return this.http.put(`${baseUrl}/queens/${id}`, params);
    }

    delete(id: number) {
        return this.http.delete(`${baseUrl}/queens/${id}`);
    }
}