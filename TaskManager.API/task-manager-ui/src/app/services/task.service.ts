import { Injectable } from '@angular/core';
import { HttpClient,HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { tap } from 'rxjs/operators';
import { ConfigService } from './config.service';

export interface TaskItem {
  id?: number;
  title: string;
  isComplete?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private API_URL: string = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.API_URL = this.configService.getApiUrl() + '/api/tasks';
  }

  getTasks(): Observable<TaskItem[]> {
    console.log('TRACE 2: HTTP GET request sent to API URL:', this.API_URL);
    return this.http.get<TaskItem[]>(this.API_URL, {
      headers: {
        'Cache-Control': 'no-store, no-cache, must-revalidate, max-age=0',
        'Pragma': 'no-cache',
        'Expires': '0',
      }
    }).pipe(tap(data => console.log('INTERCEPTOR CHECK: RAW DATA BEFORE RETURN:', data)));
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Client Error: ${error.error.message}`;
    } else {
      errorMessage = `Server Error: Code ${error.status}, body was: ${JSON.stringify(error.error)}`;
    }
    console.error(errorMessage);
    return throwError(() => new Error('Task retrieval failed. See console for details.'));
  }

  addTask(task: Partial<TaskItem>): Observable<TaskItem> {
    return this.http.post<TaskItem>(this.API_URL, task);
  }

  deleteTask(id: number) {
    return this.http.delete(`${this.API_URL}/${id}`);
  }

  updateTask(id: number, task: Partial<TaskItem>) {
    return this.http.put(`${this.API_URL}/${id}`, task);
  }

  toggleComplete(id: number) {
    // backend provides PATCH /api/tasks/{id}/toggle-complete
    return this.http.patch<TaskItem>(`${this.API_URL}/${id}/toggle-complete`, {});
  }

  resetDemoData(): Observable<any> {
    const resetUrl = `${this.API_URL}/reset-demo`;
    return this.http.post(resetUrl, {}, {responseType: 'text'})
      .pipe(
        catchError(this.handleError) 
      );
  }
}
