import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../services/authService';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {
  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });
  
  error: string | null = null;
  loading = false;

  constructor(private authService: AuthService) {}

  onSubmit() {
    if (this.loginForm.invalid) return;
    
    this.loading = true;
    this.error = null;
    
    const { username, password } = this.loginForm.value;
    this.authService.login(username!, password!).subscribe({
      next: () => this.loading = false,
      error: (err) => {
        this.error = err;
        this.loading = false;
      }
    });
  }
}