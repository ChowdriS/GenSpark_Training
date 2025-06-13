import { Component, EventEmitter, Input, Output } from '@angular/core';
import { User } from '../../models/User';
import { AuthService } from '../../service/AuthService';
@Component({
  selector: 'app-profile',
  standalone: true,
  templateUrl: './profile.component.html'
})
export class ProfileComponent {
  @Input() user!: User;
  @Output() loggedOut = new EventEmitter<void>();
  constructor(private auth: AuthService) {}
  logout() {
    this.auth.logout();
    this.loggedOut.emit();
  }
}
