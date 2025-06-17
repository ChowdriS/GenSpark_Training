import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from './authService';
import { Product, ProductsResponse } from '../models/productModel';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private http = inject(HttpClient);
  private authService = inject(AuthService);
  private readonly API_URL = 'https://dummyjson.com/products';

  private getHeaders(): HttpHeaders {
    const token = this.authService.getAccessToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  searchProducts(query: string, skip = 0, limit = 30): Observable<ProductsResponse> {
    const params = new HttpParams()
      .set('q', query)
      .set('skip', skip.toString())
      .set('limit', limit.toString());

    return this.http.get<ProductsResponse>(`${this.API_URL}/search`, {
      params,
      headers: this.getHeaders()
    }).pipe(
      catchError(error => {
        return throwError(() => error.error?.message || 'Failed to load products');
      })
    );
  }

  getProduct(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.API_URL}/${id}`, {
      headers: this.getHeaders()
    }).pipe(
      catchError(error => {
        return throwError(() => error.error?.message || 'Failed to load product');
      })
    );
  }
}