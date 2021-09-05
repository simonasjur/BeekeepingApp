import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Super } from '../_models'
import { map } from 'rxjs/operators';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class SuperService {
    constructor(private http: HttpClient) { }

    create(params: any) {
        return this.http.post(`${baseUrl}/supers`, params);
    }
}