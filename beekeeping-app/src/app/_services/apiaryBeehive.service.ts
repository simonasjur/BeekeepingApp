import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { ApiaryBeehive } from '../_models'

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class ApiaryBeehiveService {
    constructor(private http: HttpClient) { }

    getOneApiaryBeehives(apiaryId: number) {
        return this.http.get<ApiaryBeehive[]>(`${baseUrl}/apiaries/${apiaryId}/apiaryBeehives`);
    }

    create(params: any) {
        return this.http.post(`${baseUrl}/apiaryBeehives`, params);
    }
}