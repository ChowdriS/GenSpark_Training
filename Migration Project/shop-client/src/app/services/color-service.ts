import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { baseUrl } from '../misc/constants';

@Injectable({
  providedIn: 'root'
})
export class ColorService {

  constructor(private http : HttpClient) {
  }

  getColors(){
    return this.http.get(`${baseUrl}/color`);
  }
  createColor(payload : any){
    return this.http.post(`${baseUrl}/color/create`,payload);
  }
  editColor(id:number ,payload : any){
    return this.http.put(`${baseUrl}/color/edit/${id}`,payload);
  }
  getById(id:any){
    return this.http.get(`${baseUrl}/color/details/${id}`);
  }
  deleteColor(id:any){
    return this.http.delete(`${baseUrl}/color/delete/${id}`);
  }

}
