import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError, catchError } from 'rxjs';
import { environment } from '../../environment/environment';
import { Product } from '../models/product.model';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = environment.apiUrl;
  private productApiUrl = this.apiUrl + '/products';

  constructor(private http: HttpClient) { }

  //#region endpoints
  getAll(params?: any): Observable<Product[]> {
    return this.http
      .get<Product[]>(
        this.productApiUrl,
        { params })
      .pipe(
        catchError(
          this.handleError
        ));
  }


  //2 custom endpoints to allow the user to switch between the endpoints
  getCustomJson(params?: any): Observable<Product[]> {
    return this.http
      .get<Product[]>(
        `${this.productApiUrl}/CustomJson`,
        { params })
      .pipe(
        catchError(
          this.handleError
        ));
  }

  manualLookup(params?: any): Observable<Product[]> {
    return this.http
      .get<Product[]>(
        `${this.productApiUrl}/ManualLookup`,
        { params })
      .pipe(
        catchError(
          this.handleError
        ));
  }

  getById(id: number): Observable<Product> {
    return this.http
      .get<Product>(
        `${this.productApiUrl}/${id}`)
      .pipe(
        catchError(
          this.handleError
        ));
  }

  add(product: Product): Observable<Product> {
    return this.http
      .post<Product>(
        this.productApiUrl,
        product)
      .pipe(
        catchError(
          this.handleError
        ));
  }

  update(id: number, product: Product): Observable<Product> {
    return this.http
      .put<Product>(
        `${this.productApiUrl}/${id}`,
        product)
      .pipe(
        catchError(
          this.handleError
        ));
  }

  delete(id: number): Observable<void> {
    return this.http
      .delete<void>(
        `${this.productApiUrl}/${id}`)
      .pipe(
        catchError(
          this.handleError
        ));
  }
  //#endregion

  getFuzzySearch(params: any): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/products/fuzzy`, { params });
  }

  //error handling
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('Service error occurred:', error);
    return throwError(() => error); // preserve status
  }
}
