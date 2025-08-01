import { Injectable } from '@angular/core';
import { baseUrl } from '../misc/constants';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  constructor(private http: HttpClient) {}

  getOrders(){
    return this.http.get(`${baseUrl}/order/all`);
  }
  exportOrders() {
    return this.http.get(`${baseUrl}/order/export`, { responseType: 'blob' });
  }
}
