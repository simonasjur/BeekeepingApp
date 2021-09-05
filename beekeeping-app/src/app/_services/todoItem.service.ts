import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { map } from 'rxjs/operators';
import { TodoItem } from '../_models';
import { BehaviorSubject, Observable } from 'rxjs';

const baseUrl = `${environment.apiUrl}`;

@Injectable({ providedIn: 'root' })
export class TodoService {

    constructor(private http: HttpClient) {
     }

    create(todoItem: TodoItem) {
        return this.http.post(`${environment.apiUrl}/todoItems`, todoItem);
    }

    getAllFarmTodosByFilterPaged(farmId: number, isComplete: boolean, filter: string, sort: string, order: string, page: number) {
        if (filter === null) {
            filter = '';
        }
        const top = 10;
        const skip = page * top;
        return this.http.get<TodoItem[]>(`${environment.apiUrl}/farms/${farmId}/todoItems?$filter=isComplete eq ${isComplete} and (contains(title,'${filter}') or contains(description,'${filter}'))&$top=${top}&$skip=${skip}&$orderby=${sort} ${order}`);
    }

    getAllFarmTodosByApiary(farmId: number, isComplete: boolean, filter: string, sort: string, order: string, page: number, apiaryId: number) {
        if (filter === null) {
            filter = '';
        }
        const top = 10;
        const skip = page * top;
        return this.http.get<TodoItem[]>(`${environment.apiUrl}/farms/${farmId}/todoItems?$filter=isComplete eq ${isComplete} and apiaryId eq ${apiaryId} and (contains(title,'${filter}') or contains(description,'${filter}'))&$top=${top}&$skip=${skip}&$orderby=${sort} ${order}`);
    }

    getAllFarmTodosByBeehive(farmId: number, isComplete: boolean, filter: string, sort: string, order: string, page: number, beehiveId: number) {
        if (filter === null) {
            filter = '';
        }
        const top = 10;
        const skip = page * top;
        return this.http.get<TodoItem[]>(`${environment.apiUrl}/farms/${farmId}/todoItems?$filter=isComplete eq ${isComplete} and beehiveId eq ${beehiveId} and (contains(title,'${filter}') or contains(description,'${filter}'))&$top=${top}&$skip=${skip}&$orderby=${sort} ${order}`);
    }

    getAllFarmTodosByApiaryAndBeehive(farmId: number, isComplete: boolean, filter: string, sort: string, order: string, page: number, apiaryId: number, beehiveId: number) {
        if (filter === null) {
            filter = '';
        }
        const top = 10;
        const skip = page * top;
        return this.http.get<TodoItem[]>(`${environment.apiUrl}/farms/${farmId}/todoItems?$filter=isComplete eq ${isComplete} and apiaryId eq ${apiaryId} and beehiveId eq ${beehiveId} and (contains(title,'${filter}') or contains(description,'${filter}'))&$top=${top}&$skip=${skip}&$orderby=${sort} ${order}`);
    }

    getAllFarmTodosByFilter(farmId: number, isComplete: boolean, filter: string) {
        if (filter === null) {
            filter = '';
        }
        return this.http.get<TodoItem[]>(`${environment.apiUrl}/farms/${farmId}/todoItems?$filter=isComplete eq ${isComplete} and contains(title,'${filter}') or contains(description,'${filter}')`);
    }

    getAllFarmFilteredAnPaged(farmId: number, sort: string, order: string, page: number, completed: boolean) {
        const top = 10;
        const skip = page * top;
        return this.http.get<TodoItem[]>(`${environment.apiUrl}/farms/${farmId}/todoItems?$filter=isComplete eq ${completed}&$top=${top}&$skip=${skip}&$orderby=${sort} ${order}`);
    }

    getAllFarmUncompletedTodoItems(farmId: number) {
        return this.http.get<TodoItem[]>(`${environment.apiUrl}/farms/${farmId}/todoItems?$filter=isComplete eq false`);
    }

    getAllFarmCompletedTodoItems(farmId: number) {
        return this.http.get<TodoItem[]>(`${environment.apiUrl}/farms/${farmId}/todoItems?$filter=isComplete eq true`);
    }

    getById(id) {
        return this.http.get<TodoItem>(`${environment.apiUrl}/todoItems/${id}`);
    }

    update(id, params) {
        params.id = id;
        return this.http.put(`${environment.apiUrl}/todoItems/${id}`, params);
    }

    delete(id) {
        return this.http.delete<TodoItem>(`${environment.apiUrl}/todoItems/${id}`);
    }
}
