import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class RecipeService {
  private apiUrl = 'https://dummyjson.com/recipes?limit=30&skip=0';
  private http = inject(HttpClient);

  getRecipes(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
}
