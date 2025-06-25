import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tick } from '@angular/core/testing';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReceipeService {

  constructor(private http : HttpClient) { }

  getAllReceipe(): Observable<{ recipes: any[] }> {
    return this.http.get<{ recipes: any[] }>('https://dummyjson.com/recipes');
  }
}
