import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import {  } from '../_models'
import { map } from 'rxjs/operators';
import { BeehiveComponent } from '../_models/beehive-component';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class BeehiveComponentService {
    constructor(private http: HttpClient) { }

    getBeehiveComponents(beehiveId: number) {
        return this.http.get<BeehiveComponent[]>(`${baseUrl}/beehives/${beehiveId}/beehiveComponents`);
    }

    getById(id: number) {
        return this.http.get<BeehiveComponent>(`${baseUrl}/beehiveComponents/${id}`);
    }

    create(params: any) {
        return this.http.post(`${baseUrl}/beehiveComponents`, params);
    }

    update(id: number, params: any) {
        return this.http.put(`${baseUrl}/beehiveComponents/${id}`, params);
    }

    delete(id: number) {
        return this.http.delete(`${baseUrl}/beehiveComponents/${id}`);
    }
}