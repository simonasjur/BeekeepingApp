import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { BeefamilyQueen } from '../_models'
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/operators';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class BeefamilyQueenService {
    //private apiaryFamilySubject: BehaviorSubject<ApiaryBeeFamily>;
    //public apiaryFamily: Observable<ApiaryBeeFamily>;

    constructor(private http: HttpClient) {
        /*this.apiaryFamilySubject = new BehaviorSubject<ApiaryBeeFamily>(null);
        this.apiaryFamily = this.apiaryFamilySubject.asObservable();*/
    }

    /*clearApiaryFamily() {
        this.apiaryFamilySubject.next(null);
    }*/

    getLivingBeefamilyQueen(beefamilyId: number) {
        return this.http.get<BeefamilyQueen[]>(`${baseUrl}/beefamilies/${beefamilyId}/beefamilyQueens?$filter=takeOutDate eq null`);
    }

    /*getBeefamilyApiaries(beefamilyId: number) {
        return this.http.get<ApiaryBeeFamily[]>(`${baseUrl}/beeFamilies/${beefamilyId}/apiaryBeeFamilies?$filter=departDate eq null`)
        .pipe(map(apiaryFamilies => {
            this.apiaryFamilySubject.next(apiaryFamilies[0]);
            return apiaryFamilies;
        }));
    }*/

    create(params: any) {
        return this.http.post(`${baseUrl}/beefamilyQueens`, params);
    }
}