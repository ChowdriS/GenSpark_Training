import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ContactService } from '../../services/contact-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-contact-us',
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './contact-us.html',
  styleUrl: './contact-us.css'
})
export class ContactUs {
  contactForm: FormGroup;
  captchaMessage: string | null = null; 

  constructor(private router : Router, private fb: FormBuilder, private contactService : ContactService) {
    this.contactForm = this.fb.group({
      cusName: ['', Validators.required],
      cusPhone: ['', Validators.required],
      cusEmail: ['', [Validators.required, Validators.email]],
      cusContent: ['', Validators.required],
    });
  }

  hasError(controlName: string): boolean {
    const control = this.contactForm.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onSubmit() {
    if (this.contactForm.valid) {
      const payload = {
        Name : this.contactForm.value.cusName,
        Phone : this.contactForm.value.cusPhone,
        Email :this.contactForm.value.cusEmail,
        Content :this.contactForm.value.cusContent,
      }
      this.contactService.submit(payload).subscribe({
        next:(res:any)=>{
          this.router.navigate(['home']);
        },
        error:(err:any)=>{
          console.log(err);
        }
      })
    } else {
      this.contactForm.markAllAsTouched();
    }
  }
}
