import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://dummyjson.com/users';

  constructor(private http: HttpClient) {}
  getUsers(skip: number = 0, limit: number = 30): Observable<any> {
    const url = `${this.apiUrl}?skip=${skip}&limit=${limit}`;
    return this.http.get<any>(url).pipe(
      tap(() => console.log(`Fetched users (skip=${skip}, limit=${limit})`))
    );
  }

  addUser(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/add`, userData).pipe(
      tap(() => console.log('Added user'))
    );
  }
}
