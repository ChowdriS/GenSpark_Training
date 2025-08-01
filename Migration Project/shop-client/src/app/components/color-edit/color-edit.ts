import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ColorService } from '../../services/color-service';
import { Router, RouterLink } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-color-edit',
  imports: [ReactiveFormsModule,NgClass,RouterLink],
  templateUrl: './color-edit.html',
  styleUrl: './color-edit.css'
})
export class ColorEdit {
  colorForm!: FormGroup;
  formErrors: string[] = [];

  constructor(private fb: FormBuilder,private colorService : ColorService, private route : ActivatedRoute,private router : Router) {}

  ngOnInit() {
    this.colorForm = this.fb.group({
      colorId: [''],          
      color1: ['', Validators.required]
    });
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.colorService.getById(id).subscribe(
        {
          next:(res:any)=>{
            const existingColor = res;
            if (existingColor) {
              this.colorForm.patchValue({
                colorId: existingColor.colorId,
                color1: existingColor.color1
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
    const control = this.colorForm.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onSubmit() {
    this.formErrors = [];
    if (this.colorForm.invalid) {
      alert("Form is invalid!");
      return;
    }
    const formData = this.colorForm.value;
    const payload = {
      colorName : formData.color1
    }
    this.colorService.editColor(formData.colorId,payload).subscribe({
      next:(res:any)=>{
        this.router.navigate(['../home/color']);
      },
      error:(err:any)=>{
        console.log(err);
      }
    })
  }
}
