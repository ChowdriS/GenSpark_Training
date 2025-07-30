import { Component } from '@angular/core';
import { CategoryService } from '../../services/category-service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-category-create',
  imports: [ReactiveFormsModule,RouterLink,CommonModule],
  templateUrl: './category-create.html',
  styleUrl: './category-create.css'
})
export class CategoryCreate {
categoryForm! : FormGroup;
  constructor(private fb: FormBuilder, private categoryService: CategoryService, private router: Router) {
    this.categoryForm = this.fb.group({
      categoryName: ['', Validators.required]
    });
  }
  ngOnInit(): void {}

  onSubmit() {
    if (this.categoryForm.valid) {
      const dto = {
        categoryName: this.categoryForm.value.categoryName
      };
      this.categoryService.createCategory(dto).subscribe({
        next: () => this.router.navigate(['/home/category']),
        error: (err:any) => alert('Error creating category: ' + err.message || err)
      });
    }
  }
}
