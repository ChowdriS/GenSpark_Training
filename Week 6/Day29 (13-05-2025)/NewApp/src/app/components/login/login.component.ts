import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../service/AuthService';
import { User } from '../../models/User';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  user: User = { username: '', password: '' };
  error = '';

  @Output() loginSuccess = new EventEmitter<User>();

  constructor(private auth: AuthService) {}

  onLogin() {
    const result = this.auth.login(this.user);
    if (result) {
      this.loginSuccess.emit(result);
    } else {
      this.error = 'Invalid credentials';
    }
  }
}
