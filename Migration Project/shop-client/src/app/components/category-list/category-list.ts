import { Component, OnInit, Query, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CategoryService } from '../../services/category-service';
import { Category } from '../../model/Models';

@Component({
  selector: 'app-category-list',
  imports: [RouterLink],
  templateUrl: './category-list.html',
  styleUrl: './category-list.css'
})
export class CategoryList implements OnInit {
  categories = signal<Category[]>([]);
  currentPage = signal<number>(1);
    ngOnInit(): void {
      this.getCategoryList();
    }
    constructor(private categoryService : CategoryService){}
    getCategoryList(){
      this.categoryService.getCategoriesForSideBar(this.currentPage(),10).subscribe({
        next:(res:any)=>{
          var data = res.items;
          console.log(data)
          data = data?.$values;
          console.log(data)
          this.categories.set(data);
          console.log(this.categories())
        },
        error:(err:any)=>{
          console.log(err);
        }
      })
    }
    // goToPage(page:number){
          // this.currentPage.set(page);
    // }
}
