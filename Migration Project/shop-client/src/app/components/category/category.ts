import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CategoryService } from '../../services/category-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-category',
  imports: [RouterLink,CommonModule],
  templateUrl: './category.html',
  styleUrl: './category.css'
})
export class Category {
  categories: any[] = [];
  page = 1;
  pageSize = 10;
  totalPages = 1;

  constructor(private categoryService: CategoryService) {}

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getCategories(this.page).subscribe({
      next:(res:any)=>{
      this.categories = res.items.$values;
      this.totalPages = res.totalPages;
      },
      error:(err)=>{
        console.log(err)
      }
    });
  }

  deleteCategory(id: number) {
    if (confirm('Are you sure you want to delete this?')) {
      this.categoryService.deleteCategory(id).subscribe(() => {
        this.loadCategories();
      });
    }
  }

  prevPage() {
    if (this.page > 1) {
      this.page--;
      this.loadCategories();
    }
  }

  nextPage() {
    if (this.page < this.totalPages) {
      this.page++;
      this.loadCategories();
    }
  }
}
