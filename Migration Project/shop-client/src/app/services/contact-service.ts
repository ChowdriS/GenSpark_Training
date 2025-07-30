import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { baseUrl } from '../misc/constants';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  constructor(private http: HttpClient) {}
  submit(payload : any) {
    return this.http.post(`${baseUrl}/contact/submit`,payload);
  }
}
