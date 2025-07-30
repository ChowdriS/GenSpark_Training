import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { baseUrl } from '../misc/constants';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  constructor(private http: HttpClient){}
  getProductsWithCategory(page:number,categoryId: any, pageSize:number=10){
    return this.http.get(`${baseUrl}/products/paged?page=${page}&pageSize=${pageSize}&categoryId=${categoryId}`);
  }
  getProductsWithOutCategory(page:number, pageSize:number=10){
    return this.http.get(`${baseUrl}/products/paged?page=${page}&pageSize=${pageSize}`);
  }

  getProductById(id : number){
    return this.http.get(`${baseUrl}/products/${id}`);
  }
}
