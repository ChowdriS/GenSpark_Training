import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user';

@Component({
  selector: 'app-add-user',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-user.html',
})
export class AddUserComponent {
  userData :any ;

  isSubmitting = false;

  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  addUser() {
    this.isSubmitting = true;
    this.userService.addUser(this.userData).subscribe({
      next: (res) => {
        
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        this.isSubmitting = false;
        alert('Error adding user');
      }
    });
  }
}