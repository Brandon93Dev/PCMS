import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError, catchError } from 'rxjs';
import { environment } from '../../environment/environment';
import { Category } from '../models/category.model';
import { HttpErrorResponse } from '@angular/common/http';
import { HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private apiUrl = environment.apiUrl;
  private categoryApiUrl = this.apiUrl + '/categories'

  constructor(private http: HttpClient) { }

  getAll(): Observable<Category[]> {
    return this.http
      .get<Category[]>(
        `${this.categoryApiUrl}`
      )
      .pipe(
        catchError(
          this.handleError
        ));
  }

  getTree(): Observable<Category[]> {
    return this.http
      .get<Category[]>(
        `${this.categoryApiUrl}/tree`
      ).pipe(
        catchError(
          this.handleError
        ));
  }

add(category: Category): Observable<HttpResponse<Category>> {
  return this.http.post<Category>(
    `${this.categoryApiUrl}`,
    category,
    { observe: 'response' }
  ).pipe(
    catchError(this.handleError)
  );
}

private handleError(error: HttpErrorResponse): Observable<never> {
  console.error('Service error occurred:', error);
  return throwError(() => error); // preserve status
}
}