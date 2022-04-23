import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Invitation } from '../_models';
import { map } from 'rxjs/operators';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class InvitationService {
    constructor(private http: HttpClient) { }

    getCode(farmId: number) {
        return this.http.get<Invitation>(`${baseUrl}/farms/${farmId}/invitation`)
        .pipe(
            map( invitation => {
              // invitation?.expirationDate = new Date(invitation?.expirationDate);
              console.log(invitation)
              console.log(farmId)
              return invitation;
            })
          );
    }

    validateCode(code: string) {
        return this.http.get(`${baseUrl}/invitation/${code}`);
    }
}