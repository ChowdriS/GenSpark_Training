import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { baseUrl } from '../misc/constants';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
  constructor(private http: HttpClient){}
  getAllNews(){
    return this.http.get(`${baseUrl}/news`);
  }
}
