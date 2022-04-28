import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Food } from '../_models';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class FoodService {
    constructor(private http: HttpClient) { }

    getFarmFoods(farmId: number) {
        return this.http.get<Food[]>(`${baseUrl}/farms/${farmId}/foods`);
    }

    getById(id: number) {
        return this.http.get<Food>(`${baseUrl}/foods/${id}`);
    }

    create(params: any) {
        return this.http.post<Food>(`${baseUrl}/foods`, params);
    }

    update(id: number, params: any) {
        return this.http.put(`${baseUrl}/foods/${id}`, params);
    }

    delete(id: number) {
        return this.http.delete(`${baseUrl}/foods/${id}`);
    }
}