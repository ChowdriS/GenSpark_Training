import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ColorService } from '../../services/color-service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../services/category-service';

@Component({
  selector: 'app-category-edit',
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './category-edit.html',
  styleUrl: './category-edit.css'
})
export class CategoryEdit implements OnInit {
  categoryForm!: FormGroup;
  formErrors: string[] = [];

  constructor(private fb: FormBuilder,private categoryService : CategoryService, private route : ActivatedRoute,private router : Router) {}

  ngOnInit() {
    this.categoryForm = this.fb.group({
      categoryId: [''],          
      category1: ['', Validators.required]
    });
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.categoryService.getCategoryById(id).subscribe(
        {
          next:(res:any)=>{
            const existingColor = res;
            if (existingColor) {
              this.categoryForm.patchValue({
                categoryId: existingColor.categoryId,
                category1: existingColor.name
              });
            }
          },
          error:(err:any)=>{
            console.log(err);
          }
      }
    );
    }
  }

  hasError(controlName: string): boolean {
    const control = this.categoryForm.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onSubmit() {
    this.formErrors = [];
    if (this.categoryForm.invalid) {
      alert("Form is invalid!");
      return;
    }
    const formData = this.categoryForm.value;
    const payload = {
      categoryName : formData.category1
    }
    this.categoryService.editCategory(formData.categoryId,payload).subscribe({
      next:(res:any)=>{
        this.router.navigate(['../home/category']);
      },
      error:(err:any)=>{
        console.log(err);
      }
    })
  }
}
