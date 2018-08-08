import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})

export class TaskService {
  private apiUrl;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.apiUrl = baseUrl + 'api/tasks';
  }

  getAll(): Observable<Array<Task>> {
    return this.http.get<Array<Task>>(this.apiUrl);
  }

  add(task: Task): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, task);
  }

  update(task: Task): Observable<Task> {
    return this.http.put<Task>(this.apiUrl, task);
  }

  delete(id: AAGUID): Observable<any> {
    return this.http.delete<Task>(`${this.apiUrl}/${id}`);
  }
}

export class Task
{
  id: AAGUID;
  name: string;
  description: string;
  completed: boolean;
}
