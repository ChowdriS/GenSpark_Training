import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { baseUrl } from '../misc/constants';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
  constructor(private http: HttpClient){}
  getAllNews(){
    return this.http.get(`${baseUrl}/news`);
  }

  getAll() {
    return this.http.get(`${baseUrl}/news`);
  }

  getById(id: number) {
    return this.http.get(`${baseUrl}/news/${id}`);
  }

  create(dto: any) {
    return this.http.post(`${baseUrl}/news/create`, dto);
  }

  update(id: number, dto: any) {
    return this.http.put(`${baseUrl}/news/edit/${id}`, dto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${baseUrl}/news/delete/${id}`);
  }
}
