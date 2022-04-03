import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { BeehiveBeefamily } from '../_models'
import { map } from 'rxjs/operators';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class BeehiveBeefamilyService {
    constructor(private http: HttpClient) { }

    getAllBeefamiliesActiveBeehivebeefamilies(beefamilyId: number) {
        return this.http.get<BeehiveBeefamily[]>(`${environment.apiUrl}/beefamilies/${beefamilyId}/beehiveBeefamilies?$filter=departDate eq null`);
    }
}