import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { baseUrl } from '../misc/constants';
import { Cart } from '../model/Models';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  constructor(private http: HttpClient) {}
  addToCart(ProductId: number) {
    return this.http.get(`${baseUrl}/shoppingcart/add/${ProductId}`,{ withCredentials: true });
  }
  getCart(){
    return this.http.get(`${baseUrl}/shoppingcart`, { withCredentials: true });
  }
  updateCart(payload : any){
    return this.http.put(`${baseUrl}/shoppingcart/update`, payload, { withCredentials: true });
  }
  deleteCart(ProductId: number) {
    return this.http.delete(`${baseUrl}/shoppingcart/delete/${ProductId}`, { withCredentials: true });
  }
  clearCart() {
    return this.http.get(`${baseUrl}/shoppingcart/clear`, { withCredentials: true });
  }
  processOrder(payload: any) {
    return this.http.post(`${baseUrl}/shoppingcart/processorder`, payload, { withCredentials: true });
  }
}
