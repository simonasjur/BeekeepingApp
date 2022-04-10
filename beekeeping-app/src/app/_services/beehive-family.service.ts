import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { ApiaryBeeFamily, BeehiveBeefamily } from '../_models'
import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class BeehiveBeefamilyService {
    private behiveFamilySubject: BehaviorSubject<BeehiveBeefamily>;
    public behiveFamily: Observable<BeehiveBeefamily>;

    constructor(private http: HttpClient) { 
        this.behiveFamilySubject = new BehaviorSubject<BeehiveBeefamily>(null);
        this.behiveFamily = this.behiveFamilySubject.asObservable();
    }

    clearBeehiveFamily() {
        this.behiveFamilySubject.next(null);
    }

    getBeefamilyBeehive(beefamilyId: number) {
        return this.http.get<BeehiveBeefamily[]>(`${environment.apiUrl}/beefamilies/${beefamilyId}/beehiveBeefamilies?$filter=departDate eq null`)
        .pipe(map(familyBeehives => {
            this.behiveFamilySubject.next(familyBeehives[0]);
            return familyBeehives;
        }));
    }
}