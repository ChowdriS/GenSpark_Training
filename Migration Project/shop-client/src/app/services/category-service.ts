import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Category } from '../model/Models';
import { CategoryRequestDTO } from '../model/DTOs';
import { Observable } from 'rxjs';
import { baseUrl } from '../misc/constants';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  constructor(private http: HttpClient) {}
  getCategories(page: number): Observable<Category[]> {
    return this.http.get<Category[]>(`${baseUrl}/category/${page}`);
  }
  getCategoriesForSideBar(page: number,pageSize = 10): Observable<Category[]> {
    return this.http.get<Category[]>(`${baseUrl}/category/${page}?${pageSize}`);
  }

  createCategory(category: CategoryRequestDTO): Observable<Category> {
    return this.http.post<Category>(`${baseUrl}/category/create`, category);
  }

  editCategory(id: number, category: any): Observable<Category> {
    return this.http.put<Category>(`${baseUrl}/category/edit/${id}`, category);
  }

  getCategoryById(id: any): Observable<Category> {
    return this.http.get<Category>(`${baseUrl}/category/details/${id}`);
  }

  deleteCategory(id: number): Observable<any> {
    return this.http.delete(`${baseUrl}/category/delete/${id}`);
  }
}
