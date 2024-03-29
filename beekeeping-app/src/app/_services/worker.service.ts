import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { Worker } from '../_models';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class WorkerService {

    constructor(private http: HttpClient) { 

    }

    getFarmAndUserWorker(farmId: number) {
        return this.http.get<Worker>(`${baseUrl}/farms/${farmId}/farmworker`);
    }

    getUserWorkers() {
        return this.http.get<Worker[]>(`${baseUrl}/farmworkers`);
    }

    getFarmAllWorkers(farmId: number) {
        return this.http.get<Worker[]>(`${baseUrl}/farms/${farmId}/farmworkers`);
    }

    getFarmWorker(farmId: number, userId: number) {
        return this.http.get<Worker>(`${baseUrl}/farms/${farmId}/farmworkers/${userId}`);
    }

    edit(farmId: number, workerId: number, permissions: string) {
        return this.http.put(`${baseUrl}/farms/${farmId}/farmworkers/${workerId}/${permissions}`, '');
    }

    delete(farmId: number, workerId: number) {
        return this.http.delete(`${baseUrl}/farms/${farmId}/farmworkers/${workerId}`);
    }
}