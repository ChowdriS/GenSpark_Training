import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ColorService } from '../../services/color-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-color-create',
  imports: [ReactiveFormsModule,CommonModule,RouterLink],
  templateUrl: './color-create.html',
  styleUrl: './color-create.css'
})
export class ColorCreate {
  colorForm! : FormGroup;
  constructor(private fb: FormBuilder, private colorService: ColorService, private router: Router) {
    this.colorForm = this.fb.group({
      colorName: ['', Validators.required]
    });
  }
  
  
  ngOnInit(): void {}

  onSubmit() {
    if (this.colorForm.valid) {
      const dto = {
        colorName: this.colorForm.value.colorName
      };
      this.colorService.createColor(dto).subscribe({
        next: () => this.router.navigate(['/home']),
        error: (err:any) => alert('Error creating color: ' + err.message || err)
      });
    }
  }
}
