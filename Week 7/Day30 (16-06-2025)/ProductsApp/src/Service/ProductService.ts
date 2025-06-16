import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductResponse } from '../Model/Product';

@Injectable({ providedIn: 'root' })
export class ProductService {
  constructor(private http: HttpClient) {}
  private baseUrl = 'https://dummyjson.com/products';
  searchProducts(query = '', limit = 10, skip = 0): Observable<ProductResponse> {
    const url = `${this.baseUrl}/search?q=${query}&limit=${limit}&skip=${skip}`;
    return this.http.get<ProductResponse>(url);
  }
}
