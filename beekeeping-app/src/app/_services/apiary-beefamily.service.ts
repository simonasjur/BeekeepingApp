import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { ApiaryBeeFamily } from '../_models'

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class ApiaryBeeFamilyService {
    constructor(private http: HttpClient) { }

    getOneApiaryBeeFamilies(apiaryId: number) {
        return this.http.get<ApiaryBeeFamily[]>(`${baseUrl}/apiaries/${apiaryId}/apiaryBeeFamilies`);
    }

    getBeefamilyApiaries(beefamilyId: number) {
        return this.http.get<ApiaryBeeFamily[]>(`${baseUrl}/beeFamilies/${beefamilyId}/apiaryBeeFamilies?$filter=departDate eq null`);
    }

    create(params: any) {
        return this.http.post(`${baseUrl}/apiaryBeeFamilies`, params);
    }
}